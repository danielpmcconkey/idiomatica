﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<(int bookId, AvailableBookStat statKey), BookStat> BookStatByBookIdAndStatKey = new ConcurrentDictionary<(int bookId, AvailableBookStat statKey), BookStat>();
        private static ConcurrentDictionary<int, List<BookStat>> BookStatsByBookId = new ConcurrentDictionary<int, List<BookStat>>();

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
            if (value == null) return null;
            // write to cache
            BookStatByBookIdAndStatKey[key] = value;
            return value;
        }
        public static async Task<List<BookStat>> BookStatsByBookIdReadAsync(
            int key, IdiomaticaContext context)
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
    }
}
