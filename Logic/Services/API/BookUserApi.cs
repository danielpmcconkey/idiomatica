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
        public static async Task<BookUser?> BookUserByBookIdAndUserIdReadAsync(
            IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();

            return await DataCache.BookUserByBookIdAndUserIdReadAsync(
                (bookId, userId), context);
        }
        public static async Task<BookUser?> BookUserCreateAsync(IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();

            Book? book = await DataCache.BookByIdReadAsync(bookId, context);
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

            book.Pages = await DataCache.PagesByBookIdReadAsync(bookId, context);

            var firstPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (firstPage is null || firstPage.Id is null || firstPage.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            var languageUser = await DataCache.LanguageUserByLanguageIdAndUserIdReadAsync(
                    ((int)book.LanguageId, userId), context);
            if (languageUser is null || languageUser.Id is null || languageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // make sure that bookUser doesn't already exist before creating it
            var existingBookUser = await DataCache.BookUserByBookIdAndUserIdReadAsync(
                (bookId, userId), context);
            if (existingBookUser != null && existingBookUser.Id != null)
            {
                // dude already exists. Just return it. 
                // mark it as not-archived though
                existingBookUser.IsArchived = false;
                context.SaveChanges();
                // now update the stats in case they're super old
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(context, (int)existingBookUser.Id);
                return existingBookUser;
            }

            BookUser bookUser = new BookUser()
            {
                BookId = bookId,
                CurrentPageID = firstPage.Id,
                LanguageUserId = languageUser.Id
            };

            bool didSaveBookUser = await DataCache.BookUserCreateAsync(bookUser, context);
            if (!didSaveBookUser || bookUser.Id == null || bookUser.Id < 1)
            {
                ErrorHandler.LogAndThrow(2250);
                return null;
            }

            // now update BookUserStats
            await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(context, NullHandler.ThrowIfNullOrZeroInt(bookUser.Id));

            return bookUser;


        }
        public static async Task BookUserUpdateBookmarkAsync(IdiomaticaContext context, int bookUserId, int currentPageId)
        {
            if (bookUserId < 1)
            {
                ErrorHandler.LogAndThrow(1120);
            }
            if (currentPageId < 1)
            {
                ErrorHandler.LogAndThrow(1130);
            }

            var bookUser = await DataCache.BookUserByIdReadAsync(bookUserId, context);

            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow(2050, [$"bookUserId: {bookUserId}"]);
                return;
            }
            bookUser.CurrentPageID = currentPageId;
            await DataCache.BookUserUpdateAsync(bookUser, context);
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
    }
}
