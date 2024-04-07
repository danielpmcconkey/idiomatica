using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static class Upsert
    {
        public static BookStat BookStat(IdiomaticaContext context, BookStat bookStat)
        {
            var dbResult = context.BookStats
                .Where(x => x.BookId == bookStat.BookId
                    && x.Key == bookStat.Key)
                .FirstOrDefault();
            if(dbResult != null)
            {
                dbResult.Value = bookStat.Value;
                context.SaveChanges();
                return dbResult;
            }

            context.BookStats.Add(bookStat);
            context.SaveChanges();
            return bookStat;
        }
    }
}
