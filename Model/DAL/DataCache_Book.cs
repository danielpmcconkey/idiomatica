using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<int, Book> BookById = new ConcurrentDictionary<int, Book>();


        #region read
        public static async Task<Book?> BookByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (BookById.ContainsKey(key))
            {
                return BookById[key];
            }

            // read DB
            var value = context.Books.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookById[key] = value;
            return value;
        }

        #endregion

        #region create
        public static async Task<bool> BookCreateAsync(Book value, IdiomaticaContext context)
        {
            context.Books.Add(value);
            context.SaveChanges();
            if(value.Id == null || value.Id == 0) 
            {
                return false;
            }
            BookById[(int)value.Id] = value;
            return true;
        }
        #endregion

    }
}
