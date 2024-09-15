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
        private static ConcurrentDictionary<Guid, List<BookTag>> BookTagsByBookId = new ConcurrentDictionary<Guid, List<BookTag>>();

        #region create
        public static BookTag? BookTagCreate(BookTag bookTag, IdiomaticaContext context)
        {
            int numRows = context.Database.ExecuteSql($"""
                
                INSERT INTO [Idioma].[BookTag]
                           ([BookId]
                           ,[UserKey]
                           ,[Tag]
                           ,[Created]
                           ,[Id])
                     VALUES
                           ({bookTag.BookId}
                           ,{bookTag.UserId}
                           ,{bookTag.Tag}
                           ,{bookTag.Created}
                           ,{bookTag.Id})
                """);
            if (numRows < 1) throw new InvalidDataException("BookTag create affected 0 rows");
            
            // now read it into context
            BookTagAddToCache(bookTag);
            return bookTag;
        }

        public static async Task<BookTag?> BookTagCreateAsync(BookTag value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return BookTagCreate(value, context); });
        }

        #endregion

        #region read

        public static List<BookTagRow> BookTagsByBookAndUserRead(
            (Guid bookId, Guid userId) key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            // pull all tags from cache
            var allTags = BookTagsByBookIdRead(key.bookId, context);
            var personalTags = allTags.Where(x => x.UserId == key.userId);

            // read community created tags from the DB 
            var communityTags = allTags
                .GroupBy(x => x.Tag, (tagKey, g) => new BookTagRow { 
                    Tag = tagKey, Count = g.Count(), UserId = key.userId })
                .ToList();

            // stitch them together
            var data = from ct in communityTags
                       join pt in personalTags on ct.Tag equals pt.Tag into gj
                       from subgroup in gj.DefaultIfEmpty()
                       orderby ((subgroup == null) ? false : true) descending, ct.Count descending
                       select new BookTagRow
                       {
                           BookId = key.bookId,
                           Tag = ct.Tag,
                           Count = ct.Count,
                           UserId = key.userId,
                           IsPersonal = (subgroup == null) ? false : true,
                       };

            var value = data.ToList();
            
            return value;
        }
        public static async Task<List<BookTagRow>> BookTagsByBookIdAndUserIdReadAsync(
           (Guid bookId, Guid userId) key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            return await Task<List<BookTagRow>>.Run(() =>
            {
                return BookTagsByBookAndUserRead(key, context, shouldOverrideCache);
            });
        }
        public static List<BookTag> BookTagsByBookIdRead(
            Guid key, IdiomaticaContext context)
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
            Guid key, IdiomaticaContext context)
        {
            return await Task<List<BookTag>>.Run(() =>
            {
                return BookTagsByBookIdRead(key, context);
            });
        }

        #endregion

        #region delete


        public static void BookTagDelete(
            (Guid bookId, Guid userId, string tag) key, IdiomaticaContext context)
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
                and UserKey = {key.userId}
                and Tag = {trimmed}
                """);
            if (numRows < 1) throw new InvalidDataException("deleting BookTag affected 0 rows");
        }

        #endregion

        private static void BookTagRemoveFromCache(BookTag t)
        {
            // is there an existing cache
            List<BookTag>? existingList;
            if (!BookTagsByBookId.TryGetValue((Guid)t.BookId, out existingList))
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
            BookTagsByBookId[(Guid)t.BookId] = listWithoutTag;
            return;
        }
        private static void BookTagAddToCache(BookTag t)
        {
            // is there an existing cache
            List<BookTag>? existingList = null;
            if (!BookTagsByBookId.TryGetValue((Guid)t.BookId, out existingList))
            {
                existingList = new List<BookTag>() { t };
                BookTagsByBookId[(Guid)t.BookId] = existingList;
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
