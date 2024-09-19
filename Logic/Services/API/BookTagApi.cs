using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.DAL;
using Logic.Telemetry;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class BookTagApi
    {
        public static void BookTagAdd(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Book book, User user, string tag)
        {
            string trimmedTag = tag.Trim().ToLower();
            if (trimmedTag == string.Empty) return;

            var context = dbContextFactory.CreateDbContext();
            DateTimeOffset created = DateTimeOffset.UtcNow;
            // check if this user already saved this tag
            var existingTag = context.BookTags.Where(x =>
                    x.BookId == book.Id &&
                    x.UserId == user.Id &&
                    x.Tag == trimmedTag)
                .FirstOrDefault();
            if (existingTag != null) return;
            var newTag = new BookTag()
            {
                Id = Guid.NewGuid(),
                BookId = book.Id,
                Book = book,
                UserId = user.Id,
                User = user,
                Tag = trimmedTag,
                Created = created,
            };
            newTag = DataCache.BookTagCreate(newTag, dbContextFactory);
            if (newTag is null || newTag.Id is null)
            {
                ErrorHandler.LogAndThrow();
            }
        }
        public static async Task BookTagAddAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Book book, User user, string tag)
        {
            await Task.Run(() => BookTagAdd(dbContextFactory, book, user, tag));
        }


        public static void BookTagRemove(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId, string tag)
        {

            string trimmedTag = tag.Trim().ToLower();
            if (trimmedTag == string.Empty) return;

            DataCache.BookTagDelete((bookId, userId, trimmedTag), dbContextFactory);
        }
        public static async Task BookTagRemoveAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId, string tag)
        {
            await Task.Run(() => BookTagRemove(dbContextFactory, bookId, userId, tag));
        }


        public static List<BookTagRow> BookTagsGetByBookIdAndUserId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            var tags = DataCache.BookTagsByBookAndUserRead((bookId, userId), dbContextFactory);
            return tags;
        }
        public static async Task<List<BookTagRow>> BookTagsGetByBookIdAndUserIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            return await Task<List<BookTag>>.Run(() =>
            {
                return BookTagsGetByBookIdAndUserId(dbContextFactory, bookId, userId);
            });
        }
    }
}
