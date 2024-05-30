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
        private static ConcurrentDictionary<int, BookUser> BookUserById = new ConcurrentDictionary<int, BookUser>();
        private static ConcurrentDictionary<(int bookId, int userId), BookUser> BookUserByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), BookUser>();

        #region create
        public static async Task<bool> BookUserCreateAsync(BookUser value, IdiomaticaContext context)
        {
            context.BookUsers.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            BookUserById[(int)value.Id] = value;
            return true;
        }
        #endregion

        #region read
        public static async Task<BookUser?> BookUserByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserById.ContainsKey(key))
            {
                return BookUserById[key];
            }

            // read DB
            var value = context.BookUsers.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookUserById[key] = value;
            return value;
        }
        public static async Task<BookUser> BookUserByUserIdAndBookIdReadAsync(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserByBookIdAndUserId.ContainsKey(key))
            {
                return BookUserByBookIdAndUserId[key];
            }

            // read DB
            var value = context.BookUsers
                .Where(x => x.LanguageUser.UserId == key.userId && x.BookId == key.bookId)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookUserByBookIdAndUserId[key] = value;
            BookUserById[value.Id] = value;
            return value;
        }
        #endregion

        #region update
        public static async Task BookUserUpdateAsync(BookUser value, IdiomaticaContext context)
        {
            if (value.Id == null || value.Id < 1) throw new ArgumentException("ID cannot be null or 0 when updating");

            var valueFromDb = context.BookUsers.Where(x => x.Id == value.Id).FirstOrDefault();
            if (valueFromDb == null) throw new InvalidDataException("Value does not exist in the DB to update");

            valueFromDb.AudioCurrentPos = value.AudioCurrentPos;
            valueFromDb.CurrentPageID = value.CurrentPageID;
            valueFromDb.BookId = value.BookId;
            valueFromDb.IsArchived = value.IsArchived;
            valueFromDb.LanguageUserId = value.LanguageUserId;
            valueFromDb.AudioBookmarks = value.AudioBookmarks;
            context.SaveChanges();

            // now update the cache
            BookUserById[(int)value.Id] = value;

            var cachedItem1 = BookUserByBookIdAndUserId.Where(x => x.Value.Id == value.Id).FirstOrDefault();
            if (cachedItem1.Value != null)
            {
                BookUserByBookIdAndUserId[cachedItem1.Key] = value;
            }

            return;
        }
        #endregion
    }
}
