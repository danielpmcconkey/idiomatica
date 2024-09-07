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
            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        INSERT INTO [Idioma].[BookUser]
                              ([BookId]
                              ,[LanguageUserId]
                              ,[IsArchived]
                              ,[CurrentPageKey]
                              ,[AudioBookmarks]
                              ,[AudioCurrentPos]
                              ,[UniqueKey])
                        VALUES
                              ({bookUser.BookKey}
                              ,{bookUser.LanguageUserKey}
                              ,{bookUser.IsArchived}
                              ,{bookUser.CurrentPageKey}
                              ,{bookUser.AudioBookmarks}
                              ,{bookUser.AudioCurrentPos}
                              ,{guid})
                        """);
            if (numRows < 1) throw new InvalidDataException("creating BookUser affected 0 rows");


            var newEntity = context.BookUsers.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.UniqueKey is null)
            {
                throw new InvalidDataException("newEntity is null in BookUserCreate");
            }


            // add it to cache
            BookUserById[(Guid)newEntity.UniqueKey] = newEntity;
            return newEntity;
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
            value = context.BookUsers.Where(x => x.UniqueKey == key)
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
                    x.LanguageUser.UserKey == key.userId &&
                    x.BookKey == key.bookId)
                .FirstOrDefault();
            if (value is null || value.UniqueKey is null) return null;
            // write to cache
            BookUserByBookIdAndUserId[key] = value;
            BookUserById[(Guid)value.UniqueKey] = value;
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
            if (bookUser.UniqueKey == null) 
                throw new ArgumentException("ID cannot be null or 0 when updating");
            
            int numRows = context.Database.ExecuteSql($"""
                        update [Idioma].[BookUser]
                              set [BookId] = {bookUser.BookKey}
                              ,[LanguageUserId] = {bookUser.LanguageUserKey}
                              ,[IsArchived] = {bookUser.IsArchived}
                              ,[CurrentPageKey] = {bookUser.CurrentPageKey}
                              ,[AudioBookmarks] = {bookUser.AudioBookmarks}
                              ,[AudioCurrentPos] = {bookUser.AudioCurrentPos}
                        where Id = {bookUser.UniqueKey}
                        ;
                        """);
            if (numRows < 1)
            {
                throw new InvalidDataException("BookUser update affected 0 rows");
            };
            // now update the cache
            BookUserById[(Guid)bookUser.UniqueKey] = bookUser;

            var cachedItem1 = BookUserByBookIdAndUserId.Where(x => x.Value.UniqueKey == bookUser.UniqueKey).FirstOrDefault();
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
