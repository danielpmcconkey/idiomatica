using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class BookUserApi
    {
        public static void BookUserArchive(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookUserId)
        {
            var bookUser = DataCache.BookUserByIdRead(bookUserId, dbContextFactory);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            bookUser.IsArchived = true;
            DataCache.BookUserUpdate(bookUser, dbContextFactory);
        }
        public static async Task BookUserArchiveAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookUserId)
        {
            var bookUser = await DataCache.BookUserByIdReadAsync(bookUserId, dbContextFactory);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            bookUser.IsArchived = true;
            await DataCache.BookUserUpdateAsync(bookUser, dbContextFactory);
        }


        public static BookUser? BookUserByBookIdAndUserIdRead(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            return DataCache.BookUserByBookIdAndUserIdRead(
                (bookId, userId), dbContextFactory);
        }
        public static async Task<BookUser?> BookUserByBookIdAndUserIdReadAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            return await Task<BookUser?>.Run(() => 
            {
                return BookUserByBookIdAndUserIdRead(dbContextFactory, bookId, userId);
            });
        }


        public static BookUser? BookUserCreate(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            Book? book = DataCache.BookByIdRead(bookId, dbContextFactory);
            if (book is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            book.Pages = DataCache.PagesByBookIdRead(bookId, dbContextFactory);

            var firstPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (firstPage is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var languageUser = LanguageUserApi.LanguageUserGet(dbContextFactory, (Guid)book.LanguageId, userId);

            if (languageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // make sure that bookUser doesn't already exist before creating it
            var existingBookUser = DataCache.BookUserByBookIdAndUserIdRead(
                (bookId, userId), dbContextFactory);
            if (existingBookUser != null)
            {
                // dude already exists. Just return it. 
                // mark it as not-archived though
                existingBookUser.IsArchived = false;
                DataCache.BookUserUpdate(existingBookUser, dbContextFactory);
                return existingBookUser;
            }

            var bookUser = DataCache.BookUserCreate(new BookUser()
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                Book = book,
                CurrentPageId = firstPage.Id,
                LanguageUserId = languageUser.Id,
                LanguageUser = languageUser,
            }, dbContextFactory);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return bookUser;
        }
        public static async Task<BookUser?> BookUserCreateAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserCreate(dbContextFactory, bookId, userId);
            });
        }


        public static void BookUserUpdateBookmark(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookUserId, Guid currentPageId)
        {
            var bookUser = DataCache.BookUserByIdRead(bookUserId, dbContextFactory);

            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            bookUser.CurrentPageId = currentPageId;
            DataCache.BookUserUpdate(bookUser, dbContextFactory);
        }
        public static async Task BookUserUpdateBookmarkAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookUserId, Guid currentPageId)
        {
            await Task.Run(() => { BookUserUpdateBookmark(dbContextFactory, bookUserId, currentPageId); });
        }
    }
}
