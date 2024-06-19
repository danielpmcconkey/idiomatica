using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<int, List<BookTag>> BookTagsByBookId = new ConcurrentDictionary<int, List<BookTag>>();
        private static ConcurrentDictionary<(int bookId, int userId), List<BookTag>> BookTagsByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), List<BookTag>>();

        #region create

        public static async Task<bool> BookTagCreateAsync(BookTag value, IdiomaticaContext context)
        {
            if(value.BookId == null || value.UserId == null) return false;
            if (value.BookId < 1 || value.UserId < 1) return false;
            context.BookTags.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            var currentList = BookTagsByBookIdAndUserId[((int)value.BookId, (int)value.UserId)];
            if (currentList == null)
            {
                var newList = new List<BookTag>();
                newList.Add(value);
                BookTagsByBookIdAndUserId[((int)value.BookId, (int)value.UserId)] = newList;
                return true;
            }
            currentList.Add(value);
            BookTagsByBookIdAndUserId[((int)value.BookId, (int)value.UserId)] = currentList;
            return true;
        }

        #endregion

        #region read

        public static async Task<List<BookTag>> BookTagsByBookIdAndUserIdReadAsync(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (BookTagsByBookIdAndUserId.ContainsKey(key))
            {
                return BookTagsByBookIdAndUserId[key];
            }
            // read personally created tags from the DB
            var personalTags = context.BookTags
                .Where(bt => bt.BookId == key.bookId && bt.UserId == key.userId)
                .ToList();

            // read community created tags from the DB 
            var communityTags = await BookTagsByBookIdReadAsync(key.bookId, context);
            if(communityTags == null) communityTags = new List<BookTag>();

            // stitch them together
            var data = from ct in communityTags
                       join pt in personalTags on ct.Tag equals pt.Tag into gj
                       from subgroup in gj.DefaultIfEmpty()
                       orderby ((subgroup == null) ? false : true) descending, ct.Count descending
                       select new
                       {
                           BookId = key.bookId,
                           Tag = ct.Tag,
                           Count = ct.Count,
                           IsPersonal = (subgroup == null) ? false : true,
                       };

            var value = new List<BookTag>();
            foreach (var tag in data)
            {
                value.Add(new BookTag()
                {
                    BookId = key.bookId,
                    Tag = tag.Tag,
                    Count = tag.Count, 
                    IsPersonal = tag.IsPersonal
                });
            }
            
            // write to cache
            BookTagsByBookIdAndUserId[key] = value;
            return value;
        }
        public static async Task<List<BookTag>> BookTagsByBookIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (BookTagsByBookId.ContainsKey(key))
            {
                return BookTagsByBookId[key];
            }
            var data = context.BookTags
                .Where(x => x.BookId == key)
                .GroupBy(x => x.Tag)
                .Select(g => new { tag = g.Key, count = g.Count()})
                .OrderByDescending(x => x.count);

            var value = new List<BookTag>();
            foreach (var tag in data)
            {
                value.Add(new BookTag() { BookId = key, Tag = tag.tag, Count = tag.count });
            }
            // write to cache
            BookTagsByBookId[key] = value;
            return value;
        }

        #endregion
    }
}
