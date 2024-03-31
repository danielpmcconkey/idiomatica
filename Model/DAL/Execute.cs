using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static class Execute
    {
        public static int File(IdiomaticaContext context, string filePath)
        {
            int rowsAffected = 0;
            using var transaction = context.Database.BeginTransaction();
            try
            {
                string contents = System.IO.File.ReadAllText(filePath);
                var queries = contents.Split(';');
                foreach (var query in queries)
                {
                    try
                    {
                        rowsAffected += context.Database.ExecuteSqlRaw(query);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Error in query: {query}");
                        throw;
                    }
                }
                transaction.Commit();
                return rowsAffected;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public static int NonQuery(IdiomaticaContext context, string query)
        {
            return context.Database.ExecuteSqlRaw(query);
        }
    }
}
