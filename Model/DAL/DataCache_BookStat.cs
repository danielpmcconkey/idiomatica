using Microsoft.EntityFrameworkCore;
using Model.Enums;
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
        private static ConcurrentDictionary<(Guid bookId, AvailableBookStat statKey), BookStat> BookStatByBookIdAndStatKey = [];
        private static ConcurrentDictionary<Guid, List<BookStat>> BookStatsByBookId = [];

        public static BookStat? BookStatByBookIdAndStatKeyRead(
            (Guid bookId, AvailableBookStat statKey) key, IdiomaticaContext context)
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
            if (value == null) return null;
            // write to cache
            BookStatByBookIdAndStatKey[key] = value;
            return value;
        }
        public static async Task<BookStat?> BookStatByBookIdAndStatKeyReadAsync(
            (Guid bookId, AvailableBookStat statKey) key, IdiomaticaContext context)
        {
            return await Task<BookStat?>.Run(() =>
            {
                return BookStatByBookIdAndStatKeyRead(key, context);
            });
        }
        public static List<BookStat>? BookStatsByBookIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (BookStatsByBookId.ContainsKey(key))
            {
                return BookStatsByBookId[key];
            }
            // read DB
            var value = context.BookStats
                .Where(x => x.BookId == key).ToList();
            if (value == null) return null;
            // write to cache
            BookStatsByBookId[key] = value;
            return value;
        }
        public static async Task<List<BookStat>?> BookStatsByBookIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<List<BookStat>?>.Run(() =>
            {
                return BookStatsByBookIdRead(key, context);
            });
        }
    }
}
