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
        private static ConcurrentDictionary<(int bookId, int userId), List<BookUserStat>> BookUserStatsByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), List<BookUserStat>>();

        #region create
        public static bool BookUserStatsByBookIdAndUserIdCreate(
            (int bookId, int userId) key, List<BookUserStat> value, IdiomaticaContext context)
        {
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
            (int bookId, int userId) key, List<BookUserStat> value, IdiomaticaContext context)
        {
            return await Task<bool?>.Run(() =>
            {
                return BookUserStatsByBookIdAndUserIdCreate(key, value, context);
            });
        }
        #endregion
        #region read
        public static List<BookUserStat> BookUserStatsByBookIdAndUserIdRead(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserStatsByBookIdAndUserId.ContainsKey(key))
            {
                return BookUserStatsByBookIdAndUserId[key];
            }

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
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            return await Task<List<BookUserStat>>.Run(() =>
            {
                return BookUserStatsByBookIdAndUserIdRead(key, context);
            });
        }
        #endregion

        #region delete
        public static bool BookUserStatsByBookIdAndUserIdDelete(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            if (key.bookId < 1) throw new ArgumentException(nameof(key.bookId));
            if (key.userId < 1) throw new ArgumentException(nameof(key.userId));

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
                from [Idiomatica].[Idioma].[BookUserStat] bus
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
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            return await Task<bool>.Run(() => {
                return BookUserStatsByBookIdAndUserIdDelete(key, context);
            });
        }

        #endregion
    }
}
