using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<Guid, BookUser> BookUserById = new ConcurrentDictionary<Guid, BookUser>();
        private static ConcurrentDictionary<(Guid bookId, Guid userId), BookUser> BookUserByBookIdAndUserId = new ConcurrentDictionary<(Guid bookId, Guid userId), BookUser>();

        #region create

        public static BookUser? BookUserCreate(BookUser bookUser, IdiomaticaContext context)
        {
            int numRows = context.Database.ExecuteSql($"""
                        INSERT INTO [Idioma].[BookUser]
                              ([BookId]
                              ,[LanguageUserKey]
                              ,[IsArchived]
                              ,[CurrentPageKey]
                              ,[Id])
                        VALUES
                              ({bookUser.BookId}
                              ,{bookUser.LanguageUserId}
                              ,{bookUser.IsArchived}
                              ,{bookUser.CurrentPageId}
                              ,{bookUser.Id})
                        """);
            if (numRows < 1) throw new InvalidDataException("creating BookUser affected 0 rows");

            // add it to cache
            BookUserById[bookUser.Id] = bookUser;
            return bookUser;
        }
        public static async Task<BookUser?> BookUserCreateAsync(BookUser value, IdiomaticaContext context)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserCreate(value, context);
            });
        }

        #endregion

        #region read
        public static BookUser? BookUserByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserById.TryGetValue(key, out BookUser? value))
            {
                return value;
            }

            // read DB
            value = context.BookUsers.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookUserById[key] = value;
            return value;
        }
        public static async Task<BookUser?> BookUserByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserByIdRead(key, context);
            });
        }
        public static BookUser? BookUserByBookIdAndUserIdRead(
            (Guid bookId, Guid userId) key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserByBookIdAndUserId.ContainsKey(key))
            {
                return BookUserByBookIdAndUserId[key];
            }

            // read DB
            var value = context.BookUsers
                .Where(x => x.LanguageUser != null &&
                    x.LanguageUser.UserId == key.userId &&
                    x.BookId == key.bookId)
                .FirstOrDefault();
            if (value is null) return null;
            // write to cache
            BookUserByBookIdAndUserId[key] = value;
            BookUserById[(Guid)value.Id] = value;
            return value;
        }
        public static async Task<BookUser?> BookUserByBookIdAndUserIdReadAsync(
            (Guid bookId, Guid userId) key, IdiomaticaContext context)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserByBookIdAndUserIdRead(key, context);
            });
        }
        #endregion

        #region update
        public static void BookUserUpdate(BookUser bookUser, IdiomaticaContext context)
        {
            int numRows = context.Database.ExecuteSql($"""
                        update [Idioma].[BookUser]
                              set [BookId] = {bookUser.BookId}
                              ,[LanguageUserKey] = {bookUser.LanguageUserId}
                              ,[IsArchived] = {bookUser.IsArchived}
                              ,[CurrentPageKey] = {bookUser.CurrentPageId}
                        where Id = {bookUser.Id}
                        ;
                        """);
            if (numRows < 1)
            {
                throw new InvalidDataException("BookUser update affected 0 rows");
            };
            // now update the cache
            BookUserById[(Guid)bookUser.Id] = bookUser;

            var cachedItem1 = BookUserByBookIdAndUserId.Where(x => x.Value.Id == bookUser.Id).FirstOrDefault();
            if (cachedItem1.Value != null)
            {
                BookUserByBookIdAndUserId[cachedItem1.Key] = bookUser;
            }
            return;
        }
        public static async Task BookUserUpdateAsync(BookUser value, IdiomaticaContext context)
        {
            await Task.Run(() =>
            {
                BookUserUpdate(value, context);
            });
        }
        #endregion
    }
}
