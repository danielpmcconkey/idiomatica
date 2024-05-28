using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
        
        private static ConcurrentDictionary<int, List<BookListRow>> BookListRowsByUserId = new ConcurrentDictionary<int, List<BookListRow>>();
        private static ConcurrentDictionary<(int bookId, AvailableBookStat statKey), BookStat> BookStatByBookIdAndStatKey = new ConcurrentDictionary<(int bookId, AvailableBookStat statKey), BookStat>();
        private static ConcurrentDictionary<(int bookId, int userId), List<BookUserStat>> BookUserStatsByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), List<BookUserStat>>();
        private static ConcurrentDictionary<string, User> UserByApplicationUserId = new ConcurrentDictionary<string, User>();


        
        
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
        public static async Task<BookStat?> BookStatByBookIdAndStatKeyReadAsync(
            (int bookId, AvailableBookStat statKey) key, IdiomaticaContext context)
        {
            // check cache
            if (BookStatByBookIdAndStatKey.ContainsKey(key))
            {
                return BookStatByBookIdAndStatKey[key];
            }
            // read DB
            var value = context.BookStats
                .Where(x => x.BookId == key.bookId && x.Key == key.statKey)
                .FirstOrDefault();
            if(value == null) return null;
            // write to cache
            BookStatByBookIdAndStatKey[key] = value;
            return value;
        }
        
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
        
        
        public static async Task<User?> UserByApplicationUserIdReadAsync(string key, IdiomaticaContext context)
        {
            // check cache
            if (UserByApplicationUserId.ContainsKey(key))
            {
                return UserByApplicationUserId[key];
            }
            // read DB
            var value = context.Users
                .Where(u => u.ApplicationUserId == key)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            UserByApplicationUserId[key] = value;
            return value;
        }
    }
}
