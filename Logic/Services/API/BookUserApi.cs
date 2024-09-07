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
            IdiomaticaContext context, Guid bookUserId)
        {
            var bookUser = DataCache.BookUserByIdRead(bookUserId, context);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            bookUser.IsArchived = true;
            DataCache.BookUserUpdate(bookUser, context);
        }
        public static async Task BookUserArchiveAsync(IdiomaticaContext context, Guid bookUserId)
        {
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
            IdiomaticaContext context, Guid bookId, Guid userId)
        {
            return DataCache.BookUserByBookIdAndUserIdRead(
                (bookId, userId), context);
        }
        public static async Task<BookUser?> BookUserByBookIdAndUserIdReadAsync(
            IdiomaticaContext context, Guid bookId, Guid userId)
        {
            return await Task<BookUser?>.Run(() => 
            {
                return BookUserByBookIdAndUserIdRead(context, bookId, userId);
            });
        }


        public static BookUser? BookUserCreate(IdiomaticaContext context, Guid bookId, Guid userId)
        {
            Book? book = DataCache.BookByIdRead(bookId, context);
            if (book is null || book.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (book.LanguageKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            book.Pages = DataCache.PagesByBookIdRead(bookId, context);

            var firstPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if (firstPage is null || firstPage.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var languageUser = LanguageUserApi.LanguageUserGet(context, (Guid)book.LanguageKey, userId);

            if (languageUser is null || languageUser.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // make sure that bookUser doesn't already exist before creating it
            var existingBookUser = DataCache.BookUserByBookIdAndUserIdRead(
                (bookId, userId), context);
            if (existingBookUser != null && existingBookUser.UniqueKey != null)
            {
                // dude already exists. Just return it. 
                // mark it as not-archived though
                existingBookUser.IsArchived = false;
                DataCache.BookUserUpdate(existingBookUser, context);
                return existingBookUser;
            }

            var bookUser = DataCache.BookUserCreate(new BookUser()
            {
                BookKey = bookId,
                CurrentPageKey = (Guid)firstPage.UniqueKey,
                LanguageUserKey = (Guid)languageUser.UniqueKey
            }, context);
            if (bookUser is null || bookUser.UniqueKey == null)
            {
                ErrorHandler.LogAndThrow(2250);
                return null;
            }
            return bookUser;
        }
        public static async Task<BookUser?> BookUserCreateAsync(IdiomaticaContext context, Guid bookId, Guid userId)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserCreate(context, bookId, userId);
            });
        }


        public static void BookUserUpdateBookmark(
            IdiomaticaContext context, Guid bookUserId, Guid currentPageId)
        {
            var bookUser = DataCache.BookUserByIdRead(bookUserId, context);

            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow(2050, [$"bookUserId: {bookUserId}"]);
                return;
            }
            bookUser.CurrentPageKey = currentPageId;
            DataCache.BookUserUpdate(bookUser, context);
        }
        public static async Task BookUserUpdateBookmarkAsync(
            IdiomaticaContext context, Guid bookUserId, Guid currentPageId)
        {
            await Task.Run(() => { BookUserUpdateBookmark(context, bookUserId, currentPageId); });
        }
    }
}
