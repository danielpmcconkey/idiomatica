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
            IdiomaticaContext context, Book book, User user, string tag)
        {
            string trimmedTag = tag.Trim().ToLower();
            if (trimmedTag == string.Empty) return;

            
            DateTimeOffset created = DateTimeOffset.UtcNow;
            // check if this user already saved this tag
            var existingTag = context.BookTags.Where(x =>
                    x.BookKey == book.UniqueKey &&
                    x.UserKey == user.UniqueKey &&
                    x.Tag == trimmedTag)
                .FirstOrDefault();
            if (existingTag != null) return;
            var newTag = new BookTag()
            {
                UniqueKey = Guid.NewGuid(),
                BookKey = book.UniqueKey,
                Book = book,
                UserKey = user.UniqueKey,
                User = user,
                Tag = trimmedTag,
                Created = created,
            };
            newTag = DataCache.BookTagCreate(newTag, context);
            if (newTag is null || newTag.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow(2440);
            }
        }
        public static async Task BookTagAddAsync(
            IdiomaticaContext context, Book book, User user, string tag)
        {
            await Task.Run(() => BookTagAdd(context, book, user, tag));
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
            var tags = DataCache.BookTagsByBookAndUserRead((bookId, userId), context);
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
