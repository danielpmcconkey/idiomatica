using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;

namespace Logic.Services.Level1
{
    public static class PageUserApiL1
    {
        public static async Task<PageUser?> PageUserReadBookmarkedOrFirstAsync(
            IdiomaticaContext context, int bookUserId)
        {
            if (bookUserId < 1) ErrorHandler.LogAndThrow();
            var bookUser = await DataCache.BookUserByIdReadAsync(bookUserId, context);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (bookUser.LanguageUserId is null || bookUser.LanguageUserId < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (bookUser.BookId is null || bookUser.BookId < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // try to pull the bookmarked page
            if (bookUser.CurrentPageID is not null || bookUser.CurrentPageID > 0)
            {
                return await PageUserReadByPageIdAndLanguageUserIdAsync(
                    context, (int)bookUser.CurrentPageID, (int)bookUser.LanguageUserId);
            }
            // no bookmark, pull page 1
            return await PageUserReadByOrderWithinBookAsync(
                context, (int)bookUser.LanguageUserId, 1, (int)bookUser.BookId);
        }
        public static async Task<PageUser?> PageUserReadByPageIdAndLanguageUserIdAsync(
            IdiomaticaContext context, int currentPageID, int languageUserId)
        {
            if (currentPageID < 1) ErrorHandler.LogAndThrow();
            if (languageUserId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.PageUserByPageIdAndLanguageUserIdReadAsync(
                    (currentPageID, languageUserId), context);
        }
        public static async Task<PageUser?> PageUserReadByOrderWithinBookAsync(
            IdiomaticaContext context, int languageUserId, int pageOrdinal, int bookId)
        {
            if (languageUserId < 1) ErrorHandler.LogAndThrow();
            if (bookId < 1) ErrorHandler.LogAndThrow();

            return await DataCache.PageUserByLanguageUserIdOrdinalAndBookIdReadAsync(
                (languageUserId, pageOrdinal, bookId), context);
        }
        public static async Task<PageUser?> PageUserCreateForPageIdAndUserId(
            IdiomaticaContext context, int pageId, int userId)
        {
            // set up all the de-nulled values
            if (pageId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            var page = await DataCache.PageByIdReadAsync(pageId, context);
            if (page is null || page.BookId is null || page.BookId < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var book = await DataCache.BookByIdReadAsync((int)page.BookId, context);
            if (book is null || book.Id is null || book.Id < 1 || book.LanguageId is null || book.LanguageId < 1)
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

            // make sure it doesn't already exist
            var existingPageUser = await DataCache.PageUserByPageIdAndLanguageUserIdReadAsync(
                (pageId, (int)languageUser.Id), context);
            if (existingPageUser is not null) return existingPageUser;

            // nope, definitely create it
            // but first, more stuff to look up
            var bookUser = await DataCache.BookUserByBookIdAndUserIdReadAsync(
                ((int)book.Id, userId), context);
            if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // save it for real
            var pageUser = new PageUser()
            {
                BookUserId = bookUser.Id,
                PageId = pageId
            };
            bool didSave = await DataCache.PageUserCreateAsync(pageUser, context);
            if (!didSave || pageUser.Id == null || pageUser.Id == 0)
            {
                ErrorHandler.LogAndThrow(2310);
                return null;
            }
            return pageUser;
        }
        public static async Task PageUserMarkAsReadAsync(IdiomaticaContext context, int pageUserId)
        {
            var readDate = DateTime.Now;
            await PageUserUpdateReadDateAsync(context, pageUserId, readDate);
        }
        public static async Task PageUserUpdateReadDateAsync(IdiomaticaContext context, int id, DateTime readDate)
        {
            var pu = await DataCache.PageUserByIdReadAsync(id, context);

            pu.ReadDate = readDate;
            await DataCache.PageUserUpdateAsync(pu, context);
            return;
        }
        public static async Task PageUserUpdateUnknowWordsToWellKnown(IdiomaticaContext context, int pageUserId)
        {
            await DataCache.WordUsersUpdateStatusByPageUserIdAndStatus(
                pageUserId, AvailableWordUserStatus.UNKNOWN, AvailableWordUserStatus.WELLKNOWN, context);
        }
    }
}
