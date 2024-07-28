using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.API
{
    public static class BookUserApi
    {
        public static void BookUserArchive(
            IdiomaticaContext context, int bookUserId)
        {
            if (bookUserId < 1) ErrorHandler.LogAndThrow();
            var bookUser = DataCache.BookUserByIdRead(bookUserId, context);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            bookUser.IsArchived = true;
            DataCache.BookUserUpdate(bookUser, context);
        }
        public static async Task BookUserArchiveAsync(IdiomaticaContext context, int bookUserId)
        {
            if (bookUserId < 1) ErrorHandler.LogAndThrow();
            var bookUser = await DataCache.BookUserByIdReadAsync(bookUserId, context);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            bookUser.IsArchived = true;
            await DataCache.BookUserUpdateAsync(bookUser, context);
        }


        public static BookUser? BookUserByBookIdAndUserIdRead(
            IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();

            return DataCache.BookUserByBookIdAndUserIdRead(
                (bookId, userId), context);
        }
        public static async Task<BookUser?> BookUserByBookIdAndUserIdReadAsync(
            IdiomaticaContext context, int bookId, int userId)
        {
            return await Task<BookUser?>.Run(() => 
            {
                return BookUserByBookIdAndUserIdRead(context, bookId, userId);
            });
        }


        public static BookUser? BookUserCreate(IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();

            Book? book = DataCache.BookByIdRead(bookId, context);
            if (book is null || book.Id is null || book.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (book.LanguageId is null || book.LanguageId < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            book.Pages = DataCache.PagesByBookIdRead(bookId, context);

            var firstPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (firstPage is null || firstPage.Id is null || firstPage.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var languageUser = LanguageUserApi.LanguageUserGet(context, (int)book.LanguageId, userId);

            if (languageUser is null || languageUser.Id is null || languageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // make sure that bookUser doesn't already exist before creating it
            var existingBookUser = DataCache.BookUserByBookIdAndUserIdRead(
                (bookId, userId), context);
            if (existingBookUser != null && existingBookUser.Id != null)
            {
                // dude already exists. Just return it. 
                // mark it as not-archived though
                existingBookUser.IsArchived = false;
                DataCache.BookUserUpdate(existingBookUser, context);
                return existingBookUser;
            }

            var bookUser = DataCache.BookUserCreate(new BookUser()
            {
                BookId = bookId,
                CurrentPageID = (int)firstPage.Id,
                LanguageUserId = (int)languageUser.Id
            }, context);
            if (bookUser is null || bookUser.Id == null || bookUser.Id < 1)
            {
                ErrorHandler.LogAndThrow(2250);
                return null;
            }
            return bookUser;
        }
        public static async Task<BookUser?> BookUserCreateAsync(IdiomaticaContext context, int bookId, int userId)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserCreate(context, bookId, userId);
            });
        }


        public static void BookUserUpdateBookmark(
            IdiomaticaContext context, int bookUserId, int currentPageId)
        {
            if (bookUserId < 1)
            {
                ErrorHandler.LogAndThrow(1120);
            }
            if (currentPageId < 1)
            {
                ErrorHandler.LogAndThrow(1130);
            }

            var bookUser = DataCache.BookUserByIdRead(bookUserId, context);

            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow(2050, [$"bookUserId: {bookUserId}"]);
                return;
            }
            bookUser.CurrentPageID = currentPageId;
            DataCache.BookUserUpdate(bookUser, context);
        }
        public static async Task BookUserUpdateBookmarkAsync(
            IdiomaticaContext context, int bookUserId, int currentPageId)
        {
            await Task.Run(() => { BookUserUpdateBookmark(context, bookUserId, currentPageId); });
        }
    }
}
