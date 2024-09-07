using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.DAL;
using Logic.Telemetry;
using System.Reflection.Metadata.Ecma335;

namespace Logic.Services.API
{
    public static class BookTagApi
    {
        public static void BookTagAdd(
            IdiomaticaContext context, Guid bookId, Guid userId, string tag)
        {
            string trimmedTag = tag.Trim().ToLower();
            if (trimmedTag == string.Empty) return;

            
            DateTimeOffset created = DateTimeOffset.UtcNow;
            // check if this user already saved this tag
            var existingTag = context.BookTags.Where(x =>
                    x.BookKey == bookId &&
                    x.UserKey == userId &&
                    x.Tag == trimmedTag)
                .FirstOrDefault();
            if (existingTag != null) return;
            var newTag = new BookTag()
            {
                BookKey = bookId,
                UserKey = userId,
                Tag = trimmedTag,
                Created = created,
                IsPersonal = true
            };
            newTag = DataCache.BookTagCreate(newTag, context);
            if (newTag is null || newTag.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow(2440);
            }
        }
        public static async Task BookTagAddAsync(
            IdiomaticaContext context, Guid bookId, Guid userId, string tag)
        {
            await Task.Run(() => BookTagAdd(context, bookId, userId, tag));
        }


        public static void BookTagRemove(
            IdiomaticaContext context, Guid bookId, Guid userId, string tag)
        {

            string trimmedTag = tag.Trim().ToLower();
            if (trimmedTag == string.Empty) return;

            DataCache.BookTagDelete((bookId, userId, trimmedTag), context);
        }
        public static async Task BookTagRemoveAsync(
            IdiomaticaContext context, Guid bookId, Guid userId, string tag)
        {
            await Task.Run(() => BookTagRemove(context, bookId, userId, tag));
        }


        public static List<BookTag> BookTagsGetByBookIdAndUserId(
            IdiomaticaContext context, Guid bookId, Guid userId)
        {
            var tags = DataCache.BookTagsByBookIdAndUserIdRead((bookId, userId), context);
            return tags;
        }
        public static async Task<List<BookTag>> BookTagsGetByBookIdAndUserIdAsync(
            IdiomaticaContext context, Guid bookId, Guid userId)
        {
            return await Task<List<BookTag>>.Run(() =>
            {
                return BookTagsGetByBookIdAndUserId(context, bookId, userId);
            });
        }
    }
}
