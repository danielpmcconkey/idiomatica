using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;

namespace Logic.Services.API
{
    public static class PageUserApi
    {
        public static PageUser? PageUserReadBookmarkedOrFirst(
            IdiomaticaContext context, int bookUserId)
        {
            if (bookUserId < 1) ErrorHandler.LogAndThrow();
            var bookUser = DataCache.BookUserByIdRead(bookUserId, context);
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
                return PageUserReadByPageIdAndLanguageUserId(
                    context, (int)bookUser.CurrentPageID, (int)bookUser.LanguageUserId);
            }
            // no bookmark, pull page 1
            return PageUserReadByOrderWithinBook(
                context, (int)bookUser.LanguageUserId, 1, (int)bookUser.BookId);
        }
        public static async Task<PageUser?> PageUserReadBookmarkedOrFirstAsync(
            IdiomaticaContext context, int bookUserId)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserReadBookmarkedOrFirst(context, bookUserId);
            });
        }
        public static PageUser? PageUserReadByPageIdAndLanguageUserId(
            IdiomaticaContext context, int currentPageID, int languageUserId)
        {
            if (currentPageID < 1) ErrorHandler.LogAndThrow();
            if (languageUserId < 1) ErrorHandler.LogAndThrow();
            return DataCache.PageUserByPageIdAndLanguageUserIdRead(
                    (currentPageID, languageUserId), context);
        }
        public static async Task<PageUser?> PageUserReadByPageIdAndLanguageUserIdAsync(
            IdiomaticaContext context, int currentPageID, int languageUserId)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserReadByPageIdAndLanguageUserId(context, currentPageID, languageUserId);
            });
        }
        public static PageUser? PageUserReadByOrderWithinBook(
            IdiomaticaContext context, int languageUserId, int pageOrdinal, int bookId)
        {
            if (languageUserId < 1) ErrorHandler.LogAndThrow();
            if (bookId < 1) ErrorHandler.LogAndThrow();

            return DataCache.PageUserByLanguageUserIdOrdinalAndBookIdRead(
                (languageUserId, pageOrdinal, bookId), context);
        }
        public static async Task<PageUser?> PageUserReadByOrderWithinBookAsync(
            IdiomaticaContext context, int languageUserId, int pageOrdinal, int bookId)
        {
            if (languageUserId < 1) ErrorHandler.LogAndThrow();
            if (bookId < 1) ErrorHandler.LogAndThrow();

            return await DataCache.PageUserByLanguageUserIdOrdinalAndBookIdReadAsync(
                (languageUserId, pageOrdinal, bookId), context);
        }
        public static PageUser? PageUserCreateForPageIdAndUserId(
            IdiomaticaContext context, int pageId, int userId)
        {
            // set up all the de-nulled values
            if (pageId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            var page = DataCache.PageByIdRead(pageId, context);
            if (page is null || page.BookId is null || page.BookId < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var book = DataCache.BookByIdRead((int)page.BookId, context);
            if (book is null || book.Id is null || book.Id < 1 || book.LanguageId is null || book.LanguageId < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var languageUser = DataCache.LanguageUserByLanguageIdAndUserIdRead(
                ((int)book.LanguageId, userId), context);
            if (languageUser is null || languageUser.Id is null || languageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // make sure it doesn't already exist
            var existingPageUser = DataCache.PageUserByPageIdAndLanguageUserIdRead(
                (pageId, (int)languageUser.Id), context);
            if (existingPageUser is not null) return existingPageUser;

            // nope, definitely create it
            // but first, more stuff to look up
            var bookUser = DataCache.BookUserByBookIdAndUserIdRead(
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
            pageUser = DataCache.PageUserCreate(pageUser, context);
            if (pageUser is null || pageUser.Id is null || pageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow(2310);
                return null;
            }
            return pageUser;
        }
        public static async Task<PageUser?> PageUserCreateForPageIdAndUserIdAsync(
            IdiomaticaContext context, int pageId, int userId)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserCreateForPageIdAndUserId(context, pageId, userId);
            });
        }
        public static void PageUserMarkAsRead(IdiomaticaContext context, int pageUserId)
        {
            var readDate = DateTime.Now;
            PageUserUpdateReadDate(context, pageUserId, readDate);
        }
        public static async Task PageUserMarkAsReadAsync(IdiomaticaContext context, int pageUserId)
        {
            var readDate = DateTime.Now;
            await PageUserUpdateReadDateAsync(context, pageUserId, readDate);
        }
        public static void PageUserUpdateReadDate(IdiomaticaContext context, int id, DateTime readDate)
        {
            var pu = DataCache.PageUserByIdRead(id, context);

            pu.ReadDate = readDate;
            DataCache.PageUserUpdate(pu, context);
            return;
        }
        public static async Task PageUserUpdateReadDateAsync(IdiomaticaContext context, int id, DateTime readDate)
        {
            var pu = await DataCache.PageUserByIdReadAsync(id, context);

            pu.ReadDate = readDate;
            await DataCache.PageUserUpdateAsync(pu, context);
            return;
        }
        public static void PageUserUpdateUnknowWordsToWellKnown(IdiomaticaContext context, int pageUserId)
        {
            DataCache.WordUsersUpdateStatusByPageUserIdAndStatus(
                pageUserId, AvailableWordUserStatus.UNKNOWN, AvailableWordUserStatus.WELLKNOWN, context);
        }
    }
}
