using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.DAL;
using Logic.Telemetry;

namespace Logic.Services.API
{
    public static class BookTagApi
    {
        public static async Task BookTagAdd(IdiomaticaContext context, int bookId, int userId, string tag)
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
            bool didSave = await DataCache.BookTagCreateAsync(newTag, context);
            if (!didSave || newTag.Id == null || newTag.Id < 1)
            {
                ErrorHandler.LogAndThrow(2440);
            }
        }
        public static async Task BookTagRemove(IdiomaticaContext context, int bookId, int userId, string tag)
        {

            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            string trimmedTag = tag.Trim().ToLower();
            if (trimmedTag == string.Empty) return;

            await DataCache.BookTagDelete((bookId, userId, trimmedTag), context);
        }
        public static async Task<List<BookTag>> BookTagsGetByBookIdAndUserId(
            IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            var tags = await DataCache.BookTagsByBookIdAndUserIdReadAsync((bookId, userId), context);
            return tags;
        }
    }
}
