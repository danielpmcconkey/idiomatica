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

        private static ConcurrentDictionary<int, List<BookListRow>> BookListRowsByUserId = new ConcurrentDictionary<int, List<BookListRow>>();




        #region read
        public static async Task<List<BookListRow>> BookListRowsByUserIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (BookListRowsByUserId.ContainsKey(key))
            {
                return BookListRowsByUserId[key];
            }
            // read DB
            var value = context.BookListRows.Where(x => x.UserId == key)
                .ToList();

            // write to cache
            BookListRowsByUserId[key] = value;
            return value;
        }
        #endregion

        #region delete
        /*
        * since BookListRow is just a view, delete methods 
        * will only delete the cache
        * */
        public static void BookListRowsByUserIdDeleteAsync( int key, IdiomaticaContext context)
        {
            BookListRowsByUserId.TryRemove(key, out var deletedRow);
        }
        #endregion


    }
}
