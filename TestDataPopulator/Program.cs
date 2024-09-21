

using Microsoft.Extensions.Configuration;

namespace TestDataPopulator
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            // only run this in your own lower environment do NOT run this against the prod database
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
            TestDataPopulator testDataPopulator = new(ConnectionStrings.TestString);
            testDataPopulator.PopulateData();

            TestDataPopulator devDataPopulator = new (ConnectionStrings.DevString);
            devDataPopulator.PopulateData();
#endif
        }
        public class ConnectionStrings
        {
            public string? DevString { get; set; }
            public string? TestString { get; set; }
        }
    }
}
