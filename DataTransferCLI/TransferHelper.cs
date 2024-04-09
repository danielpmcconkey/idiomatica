using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.EntityFrameworkCore;
using Model.DAL;

namespace DataTransferCLI
{
    public class TransferHelper
    {
        private const string connectionStringDev = "Server=localhost;Database=Idiomatica;Trusted_Connection=True;TrustServerCertificate=true;";
        private const string connectionStringProd = "Server=tcp:idiomatica.database.windows.net,1433;Initial Catalog=idiomatica_prod;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Default";

        private IdiomaticaContext GetContext(string env)
        {
            var connectionString = (env == "Prod") ? connectionStringProd : connectionStringDev;
            var optionsBuilder = new DbContextOptionsBuilder<IdiomaticaContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new IdiomaticaContext(optionsBuilder.Options);
        }
        internal void TransferData()
        {
            using (var contextProd = GetContext("Prod"))
            {
                using (var contextDev = GetContext("Dev"))
                {
                    //TransferTable<User>(contextDev, contextProd);
                    //TransferTable<Language>(contextDev, contextProd);
                    //TransferTable<LanguageUser>(contextDev, contextProd);
                    //TransferTable<Word>(contextDev, contextProd);
                    //TransferTable<Book>(contextDev, contextProd);
                    //TransferTable<BookStat>(contextDev, contextProd);
                    //TransferTable<Page>(contextDev, contextProd);
                    //TransferTable<Paragraph>(contextDev, contextProd);
                    //TransferTable<Sentence>(contextDev, contextProd);
                    //TransferTable<Token>(contextDev, contextProd);
                    //TransferTable<UserSetting>(contextDev, contextProd);

                    // WordParent doesn't get copied over this way
                    var wordsIn = contextDev.Words
                        .Where(x => x.ParentWords.Count > 0)
                        .Include(x => x.ParentWords);
                    foreach (var wordIn in wordsIn)
                    {
                        var wordOut = contextProd.Words.Where(x => x.Id == wordIn.Id).FirstOrDefault();
                        if (wordOut == null)
                        {
                            throw new Exception();
                        }
                        wordOut.ParentWords = new List<Word>();
                        foreach(var parentWord in wordIn.ParentWords)
                        {
                            wordOut.ParentWords.Add(parentWord);
                        }
                    }
                    contextProd.SaveChanges();
                }
            }
        }
        internal void TransferTable<T>(IdiomaticaContext contextSource, 
            IdiomaticaContext contextDestination) where T : class
        {
            foreach (var entityIn in contextSource.Set<T>())
            {
                contextDestination.Set<T>().Add(entityIn);
            }
            contextDestination.SaveChangesWithIdentityInsert<T>();
        }
    }
    public static class IdentityHelpers
    {
        public static void EnableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: true);
        public static void DisableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: false);

        private static void SetIdentityInsert<T>(DbContext context, bool enable)
        {
            var entityType = context.Model.FindEntityType(typeof(T));
            var tableName = entityType.GetTableName();
            if (tableName == "BookStat" || tableName == "UserSetting") return; // these tables don't have identity columns
            var value = enable ? "ON" : "OFF";
            var q = $"SET IDENTITY_INSERT {entityType.GetSchema()}.{tableName} {value}";
            context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entityType.GetSchema()}.[{entityType.GetTableName()}] {value}"
                );
        }

        public static void SaveChangesWithIdentityInsert<T>(this DbContext context)
        {
            using var transaction = context.Database.BeginTransaction();
            context.EnableIdentityInsert<T>();
            context.SaveChanges();
            context.DisableIdentityInsert<T>();
            transaction.Commit();
        }

    }
}
