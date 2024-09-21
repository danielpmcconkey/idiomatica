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
        
        #region create

        public static BookUser? BookUserCreate(BookUser bookUser, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            int numRows = context.Database.ExecuteSql($"""
                        INSERT INTO [Idioma].[BookUser]
                              ([BookId]
                              ,[LanguageUserId]
                              ,[IsArchived]
                              ,[CurrentPageId]
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
        public static async Task<BookUser?> BookUserCreateAsync(BookUser value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserCreate(value, dbContextFactory);
            });
        }

        #endregion

        #region read
        public static BookUser? BookUserByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (BookUserById.TryGetValue(key, out BookUser? value))
            {
                return value;
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            value = context.BookUsers.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookUserById[key] = value;
            return value;
        }
        public static async Task<BookUser?> BookUserByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserByIdRead(key, dbContextFactory);
            });
        }
        public static BookUser? BookUserByBookIdAndUserIdRead(
            (Guid bookId, Guid userId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (BookUserByBookIdAndUserId.ContainsKey(key))
            {
                return BookUserByBookIdAndUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();


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
            (Guid bookId, Guid userId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserByBookIdAndUserIdRead(key, dbContextFactory);
            });
        }
        #endregion

        #region update
        public static void BookUserUpdate(BookUser bookUser, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            int numRows = context.Database.ExecuteSql($"""
                        update [Idioma].[BookUser]
                              set [BookId] = {bookUser.BookId}
                              ,[LanguageUserId] = {bookUser.LanguageUserId}
                              ,[IsArchived] = {bookUser.IsArchived}
                              ,[CurrentPageId] = {bookUser.CurrentPageId}
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
        public static async Task BookUserUpdateAsync(BookUser value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            await Task.Run(() =>
            {
                BookUserUpdate(value, dbContextFactory);
            });
        }
        #endregion
    }
}
