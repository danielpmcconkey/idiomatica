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
        private static ConcurrentDictionary<(int bookId, int userId), List<BookUserStat>> BookUserStatsByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), List<BookUserStat>>();

        #region create
        public static async Task<bool> BookUserStatsByBookIdAndUserIdCreate(
            (int bookId, int userId) key, List<BookUserStat> value, IdiomaticaContext context)
        {
            foreach (var item in value)
            {
                context.BookUserStats.Add(item);
                context.SaveChanges();
            }
            // write it to the cache
            BookUserStatsByBookIdAndUserId[key] = value;
            return true;
        } 
        #endregion
        #region read
        public static async Task<List<BookUserStat>> BookUserStatsByBookIdAndUserIdReadAsync(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserStatsByBookIdAndUserId.ContainsKey(key))
            {
                return BookUserStatsByBookIdAndUserId[key];
            }

            // read DB
            var value = context.BookUserStats
                .Where(x => x.LanguageUser.UserId == key.userId && x.BookId == key.bookId)
                .ToList();
            // write to cache
            BookUserStatsByBookIdAndUserId[key] = value;
            return value;
        }
        #endregion

        #region delete
        public static async Task BookUserStatsByBookIdAndUserIdDelete(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            var existingList = context.BookUserStats
                .Where(x => x.BookId == key.bookId && x.LanguageUser.UserId == key.userId);
            foreach (var existingItem in existingList)
            {
                context.BookUserStats.Remove(existingItem);
            }
            context.SaveChanges();
            if (BookUserStatsByBookIdAndUserId.ContainsKey(key))
            {
                if (!BookUserStatsByBookIdAndUserId.TryRemove(key, out var value))
                {
                    throw new InvalidDataException($"Failed to remove BookUserStatsByBookIdAndUserId from cache where key = {key}");
                }
            }
        }

        #endregion
    }
}
