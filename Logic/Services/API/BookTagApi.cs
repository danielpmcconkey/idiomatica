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
        public static void BookTagAdd(IdiomaticaContext context, int bookId, int userId, string tag)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            string trimmedTag = tag.Trim().ToLower();
            if (trimmedTag == string.Empty) return;

            
            DateTimeOffset created = DateTimeOffset.UtcNow;
            // check if this user already saved this tag
            var existingTag = context.BookTags.Where(x =>
                    x.BookId == bookId &&
                    x.UserId == userId &&
                    x.Tag == trimmedTag)
                .FirstOrDefault();
            if (existingTag != null) return;
            var newTag = new BookTag()
            {
                BookId = bookId,
                UserId = userId,
                Tag = trimmedTag,
                Created = created,
                IsPersonal = true
            };
            newTag = DataCache.BookTagCreate(newTag, context);
            if (newTag is null || newTag.Id is null || newTag.Id < 1)
            {
                ErrorHandler.LogAndThrow(2440);
            }
        }
        public static async Task BookTagAddAsync(IdiomaticaContext context, int bookId, int userId, string tag)
        {
            await Task.Run(() => BookTagAdd(context, bookId, userId, tag));
        }
        public static void BookTagRemove(IdiomaticaContext context, int bookId, int userId, string tag)
        {

            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            string trimmedTag = tag.Trim().ToLower();
            if (trimmedTag == string.Empty) return;

            DataCache.BookTagDelete((bookId, userId, trimmedTag), context);
        }
        public static async Task BookTagRemoveAsync(IdiomaticaContext context, int bookId, int userId, string tag)
        {
            await Task.Run(() => BookTagRemove(context, bookId, userId, tag));
        }
        public static List<BookTag> BookTagsGetByBookIdAndUserId(
            IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            var tags = DataCache.BookTagsByBookIdAndUserIdRead((bookId, userId), context);
            return tags;
        }
        public static async Task<List<BookTag>> BookTagsGetByBookIdAndUserIdAsync(
            IdiomaticaContext context, int bookId, int userId)
        {
            return await Task<List<BookTag>>.Run(() =>
            {
                return BookTagsGetByBookIdAndUserId(context, bookId, userId);
            });
        }
    }
}
