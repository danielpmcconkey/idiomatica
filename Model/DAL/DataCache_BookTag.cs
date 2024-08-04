using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<int, List<BookTag>> BookTagsByBookId = new ConcurrentDictionary<int, List<BookTag>>();

        #region create
        public static BookTag? BookTagCreate(BookTag bookTag, IdiomaticaContext context)
        {
            if (bookTag.BookId == null || bookTag.UserId == null) 
                throw new ArgumentNullException("bookTag.BookId == null || value.UserId == null during BookTagCreate");
            if (bookTag.BookId < 1 || bookTag.UserId < 1) 
                throw new ArgumentNullException("value.BookId < 1 || value.UserId < 1 during BookTagCreate");
            
            var guid = Guid.NewGuid();
            
            int numRows = context.Database.ExecuteSql($"""
                
                INSERT INTO [Idioma].[BookTag]
                           ([BookId]
                           ,[UserId]
                           ,[Tag]
                           ,[Created]
                           ,[UniqueKey])
                     VALUES
                           ({bookTag.BookId}
                           ,{bookTag.UserId}
                           ,{bookTag.Tag}
                           ,{bookTag.Created}
                           ,{guid})
                """);
            if (numRows < 1) throw new InvalidDataException("BookTag create affected 0 rows");
            // now read it into context
            var newTag = context.BookTags.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newTag is null || newTag.Id is null || newTag.Id < 1)
                throw new InvalidDataException("Reading new BookTag after creation returned null");

            
            BookTagAddToCache(newTag);
            return newTag;
        }

        public static async Task<BookTag?> BookTagCreateAsync(BookTag value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return BookTagCreate(value, context); });
        }

        #endregion

        #region read

        public static List<BookTag> BookTagsByBookIdAndUserIdRead(
            (int bookId, int userId) key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            // pull all tags from cache
            var allTags = BookTagsByBookIdRead(key.bookId, context);
            var personalTags = allTags.Where(x => x.UserId == key.userId);

            // read community created tags from the DB 
            var communityTags = allTags
                .GroupBy(x => x.Tag, (key, g) => new BookTag { Tag = key, Count = g.Count() })
                .ToList();

            // stitch them together
            var data = from ct in communityTags
                       join pt in personalTags on ct.Tag equals pt.Tag into gj
                       from subgroup in gj.DefaultIfEmpty()
                       orderby ((subgroup == null) ? false : true) descending, ct.Count descending
                       select new BookTag
                       {
                           BookId = key.bookId,
                           Tag = ct.Tag,
                           Count = ct.Count,
                           IsPersonal = (subgroup == null) ? false : true,
                       };

            var value = data.ToList();
            
            return value;
        }
        public static async Task<List<BookTag>> BookTagsByBookIdAndUserIdReadAsync(
           (int bookId, int userId) key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            return await Task<List<BookTag>>.Run(() =>
            {
                return BookTagsByBookIdAndUserIdRead(key, context, shouldOverrideCache);
            });
        }
        public static List<BookTag> BookTagsByBookIdRead(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (BookTagsByBookId.ContainsKey(key))
            {
                return BookTagsByBookId[key];
            }
            var value = context.BookTags
                .Where(x => x.BookId == key)
                .ToList();

            // write to cache
            BookTagsByBookId[key] = value;
            return value;
        }
        public static async Task<List<BookTag>> BookTagsByBookIdReadAsync(
            int key, IdiomaticaContext context)
        {
            return await Task<List<BookTag>>.Run(() =>
            {
                return BookTagsByBookIdRead(key, context);
            });
        }

        #endregion

        #region delete


        public static void BookTagDelete(
            (int bookId, int userId, string tag) key, IdiomaticaContext context)
        {
            string trimmed = key.tag.Trim();
            if (trimmed == null) return;
            // make sure this tag actually exists
            var existingTags = context.BookTags.Where(x =>
                    x.BookId == key.bookId &&
                    x.UserId == key.userId &&
                    x.Tag == trimmed);
            if (existingTags.Any() == false) return;

            // remove them from the context and the cache
            foreach (var tag in existingTags)
            {
                context.BookTags.Remove(tag);
                BookTagRemoveFromCache(tag);
            }
            int numRows = context.Database.ExecuteSql($"""
                delete from Idioma.BookTag
                where BookId = {key.bookId}
                and UserId = {key.userId}
                and Tag = {trimmed}
                """);
            if (numRows < 1) throw new InvalidDataException("deleting BookTag affected 0 rows");
        }

        #endregion

        private static void BookTagRemoveFromCache(BookTag t)
        {
            if (t.BookId == null || t.UserId == null || t.Id == null) return;
            // is there an existing cache
            List<BookTag>? existingList;
            if (!BookTagsByBookId.TryGetValue((int)t.BookId, out existingList))
            {
                // I don't think we should ever get here, but better to be safe than sorry
                return;
            }
            // is the offensive tag present?
            var existingValue = existingList.Where(x => x.Id == t.Id).FirstOrDefault();
            if (existingValue == null) return;
            // pull a list without the tag
            var listWithoutTag = existingList.Where(x => x.Id != t.Id).ToList();
            // and write it to cache
            BookTagsByBookId[(int)t.BookId] = listWithoutTag;
            return;
        }
        private static void BookTagAddToCache(BookTag t)
        {
            if (t.BookId == null || t.UserId == null || t.Id == null) return;
            // is there an existing cache
            List<BookTag>? existingList = null;
            if (!BookTagsByBookId.TryGetValue((int)t.BookId, out existingList))
            {
                existingList = new List<BookTag>() { t };
                BookTagsByBookId[(int)t.BookId] = existingList;
                return;
            }
            
            // is the offensive tag present?
            var existingValue = existingList.Where(x => x.Id == t.Id).FirstOrDefault();
            if (existingValue != null) return;
            // add it
            existingList.Add(t);
            return;
        }
    }
}
