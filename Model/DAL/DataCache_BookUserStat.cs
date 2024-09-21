using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Model.DAL
{
    public static partial class DataCache
    {
        
        #region create
        public static bool BookUserStatsByBookIdAndUserIdCreate(
            (Guid bookId, Guid userId) key, List<BookUserStat> value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            foreach (var item in value)
            {
                context.Database.ExecuteSql($"""
                    INSERT INTO [Idioma].[BookUserStat]
                               ([LanguageUserId]
                               ,[BookId]
                               ,[Key]
                               ,[ValueString]
                               ,[ValueNumeric])
                         VALUES
                               ({item.LanguageUserId}
                               ,{item.BookId}
                               ,{item.Key}
                               ,{item.ValueString}
                               ,{item.ValueNumeric})
                    """);

            }
            // write it to the cache
            BookUserStatsByBookIdAndUserId[key] = value;
            return true;
        }
        public static async Task<bool> BookUserStatsByBookIdAndUserIdCreateAsync(
            (Guid bookId, Guid userId) key, List<BookUserStat> value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<bool?>.Run(() =>
            {
                return BookUserStatsByBookIdAndUserIdCreate(key, value, dbContextFactory);
            });
        }
        #endregion
        #region read
        public static List<BookUserStat> BookUserStatsByBookIdAndUserIdRead(
            (Guid bookId, Guid userId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (BookUserStatsByBookIdAndUserId.ContainsKey(key))
            {
                return BookUserStatsByBookIdAndUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.BookUserStats
                .Where(x => x.LanguageUser != null &&
                    x.LanguageUser.UserId == key.userId &&
                    x.BookId == key.bookId)
                .ToList();
            // write to cache
            BookUserStatsByBookIdAndUserId[key] = value;
            return value;
        }
        public static async Task<List<BookUserStat>> BookUserStatsByBookIdAndUserIdReadAsync(
            (Guid bookId, Guid userId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<BookUserStat>>.Run(() =>
            {
                return BookUserStatsByBookIdAndUserIdRead(key, dbContextFactory);
            });
        }
        #endregion

        #region delete
        public static bool BookUserStatsByBookIdAndUserIdDelete(
            (Guid bookId, Guid userId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();



            // remove from context
            var existingList = context.BookUserStats
                .Where(x => x.BookId == key.bookId 
                    && x.LanguageUser != null &&
                    x.LanguageUser.UserId == key.userId);
            foreach (var existingItem in existingList)
            {
                context.BookUserStats.Remove(existingItem);
            }

            // remove from the DB
            context.Database.ExecuteSql($"""
                delete bus
                from [Idioma].[BookUserStat] bus
                left join [Idioma].[LanguageUser] lu on bus.LanguageUserId = lu.Id
                where bus.BookId = {key.bookId}
                and lu.UserId = {key.userId}
                """);
            // remove from cache
            if (BookUserStatsByBookIdAndUserId.ContainsKey(key))
            {
                if (!BookUserStatsByBookIdAndUserId.TryRemove(key, out var value))
                {
                    throw new InvalidDataException($"Failed to remove BookUserStatsByBookIdAndUserId from cache where key = {key}");
                }
            }
            return true;
        }
        public static async Task<bool> BookUserStatsByBookIdAndUserIdDeleteAsync(
            (Guid bookId, Guid userId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<bool>.Run(() => {
                return BookUserStatsByBookIdAndUserIdDelete(key, dbContextFactory);
            });
        }

        #endregion
    }
}
