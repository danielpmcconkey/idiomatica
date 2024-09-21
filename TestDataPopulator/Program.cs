

using Microsoft.Extensions.Configuration;
using Model.DAL;

namespace TestDataPopulator
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // only run this in your own lower environment do NOT run this
            // against the prod database

            try
            {
                var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings_tdp.json", optional: false);

                IConfiguration config = builder.Build();
                var ConnectionStrings = config
                    .GetSection("ConnectionStrings")
                    .Get<ConnectionStrings>();

                if (ConnectionStrings is null)
                {
                    Console.WriteLine("ConnectionStrings config not found");
                    return;
                }

                if (ConnectionStrings.DevString is null)
                {
                    Console.WriteLine("Dev connection strings not found");
                    return;
                }

                if (ConnectionStrings.TestString is null)
                {
                    Console.WriteLine("Test connection strings not found");
                    return;
                }

#if DEBUG
                Console.WriteLine("**********************************************************************");
                Console.WriteLine("Writing data to the test database");
                Console.WriteLine("**********************************************************************");

                TestDataPopulator testDataPopulator = new(ConnectionStrings.TestString);
                testDataPopulator.PopulateData();

                Console.WriteLine("**********************************************************************");
                Console.WriteLine("Writing data to the dev database");
                Console.WriteLine("**********************************************************************");

                // gotta delete the cache to prevent test DB values from being used in the dev population
                DataCache.DeleteAllCache();
                TestDataPopulator devDataPopulator = new(ConnectionStrings.DevString);
                devDataPopulator.PopulateData();

                Console.WriteLine("**********************************************************************");
                Console.WriteLine("Fin");
                Console.WriteLine("**********************************************************************");

            }
            catch (Exception ex)
            {
                Console.WriteLine("A fatal error has occurred in the data populator.");
                Console.WriteLine(ex.ToString());
                throw;
            }
#endif

        }
        public class ConnectionStrings
        {
            public string? DevString { get; set; }
            public string? TestString { get; set; }
        }

    }
}
