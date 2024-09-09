using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using System.Net;
using Model.Enums;

namespace Logic.Services.API
{
    public static class PageUserApi
    {
        public static PageUser? PageUserCreateForPageIdAndUserId(
            IdiomaticaContext context, Guid pageId, Guid userId)
        {
            // set up all the de-nulled values
            var page = DataCache.PageByIdRead(pageId, context);
            if (page is null || page.BookKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var book = DataCache.BookByIdRead((Guid)page.BookKey, context);
            if (book is null || book.UniqueKey is null || book.LanguageKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var languageUser = DataCache.LanguageUserByLanguageIdAndUserIdRead(
                ((Guid)book.LanguageKey, userId), context);
            if (languageUser is null || languageUser.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // make sure it doesn't already exist
            var existingPageUser = DataCache.PageUserByPageIdAndLanguageUserIdRead(
                (pageId, (Guid)languageUser.UniqueKey), context);
            if (existingPageUser is not null) return existingPageUser;

            // nope, definitely create it
            // but first, more stuff to look up
            var bookUser = DataCache.BookUserByBookIdAndUserIdRead(
                ((Guid)book.UniqueKey, userId), context);
            if (bookUser is null || bookUser.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // save it for real
            var pageUser = new PageUser()
            {
                BookUserKey = bookUser.UniqueKey,
                PageKey = pageId
            };
            pageUser = DataCache.PageUserCreate(pageUser, context);
            if (pageUser is null || pageUser.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow(2310);
                return null;
            }
            return pageUser;
        }
        public static async Task<PageUser?> PageUserCreateForPageIdAndUserIdAsync(
            IdiomaticaContext context, Guid pageId, Guid userId)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserCreateForPageIdAndUserId(context, pageId, userId);
            });
        }


        public static void PageUserMarkAsRead(
            IdiomaticaContext context, Guid pageUserId)
        {
            var readDate = DateTime.Now;
            PageUserUpdateReadDate(context, pageUserId, readDate);
        }
        public static async Task PageUserMarkAsReadAsync(
            IdiomaticaContext context, Guid pageUserId)
        {
            var readDate = DateTime.Now;
            await PageUserUpdateReadDateAsync(context, pageUserId, readDate);
        }


        /// <summary>
        /// PageUserReadBookmarkedOrFirst fetches the PageUser object 
        /// associated with either BookUser's current page or, if there is no 
        /// PageUser associated to the bookmark, try to fetch the PageUser for
        /// page 1. If both of those return null, return null from this method
        /// </summary>
        public static PageUser? PageUserReadBookmarkedOrFirst(
            IdiomaticaContext context, Guid bookUserId)
        {
            var bookUser = DataCache.BookUserByIdRead(bookUserId, context);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (bookUser.LanguageUserKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (bookUser.BookKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // try to pull the bookmarked page
            if (bookUser.CurrentPageKey is not null)
            {
                return PageUserReadByPageIdAndLanguageUserId(
                    context, (Guid)bookUser.CurrentPageKey, (Guid)bookUser.LanguageUserKey);
            }
            // no bookmark, pull page 1
            return PageUserReadByOrderWithinBook(
                context, (Guid)bookUser.LanguageUserKey, 1, (Guid)bookUser.BookKey);
        }
        public static async Task<PageUser?> PageUserReadBookmarkedOrFirstAsync(
            IdiomaticaContext context, Guid bookUserId)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserReadBookmarkedOrFirst(context, bookUserId);
            });
        }


        public static PageUser? PageUserReadByOrderWithinBook(
            IdiomaticaContext context, Guid languageUserId, int pageOrdinal, Guid bookId)
        {
            return DataCache.PageUserByLanguageUserIdOrdinalAndBookIdRead(
                (languageUserId, pageOrdinal, bookId), context);
        }
        public static async Task<PageUser?> PageUserReadByOrderWithinBookAsync(
            IdiomaticaContext context, Guid languageUserId, int pageOrdinal, Guid bookId)
        {
            return await DataCache.PageUserByLanguageUserIdOrdinalAndBookIdReadAsync(
                (languageUserId, pageOrdinal, bookId), context);
        }


        public static PageUser? PageUserReadByPageIdAndLanguageUserId(
            IdiomaticaContext context, Guid currentPageID, Guid languageUserId)
        {
            return DataCache.PageUserByPageIdAndLanguageUserIdRead(
                    (currentPageID, languageUserId), context);
        }
        public static async Task<PageUser?> PageUserReadByPageIdAndLanguageUserIdAsync(
            IdiomaticaContext context, Guid currentPageID, Guid languageUserId)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserReadByPageIdAndLanguageUserId(context, currentPageID, languageUserId);
            });
        }


        public static void PageUserUpdateReadDate(
            IdiomaticaContext context, Guid id, DateTime readDate)
        {
            var pu = DataCache.PageUserByIdRead(id, context);
            if (pu is null) { ErrorHandler.LogAndThrow(); return; }
            pu.ReadDate = readDate;
            DataCache.PageUserUpdate(pu, context);
            return;
        }
        public static async Task PageUserUpdateReadDateAsync(
            IdiomaticaContext context, Guid id, DateTime readDate)
        {
            var pu = await DataCache.PageUserByIdReadAsync(id, context);
            if (pu is null) { ErrorHandler.LogAndThrow(); return; }
            pu.ReadDate = readDate;
            await DataCache.PageUserUpdateAsync(pu, context);
            return;
        }


        public static void PageUserUpdateUnknowWordsToWellKnown(
            IdiomaticaContext context, Guid pageUserId)
        {
            DataCache.WordUsersUpdateStatusByPageUserIdAndStatus(
                pageUserId, AvailableWordUserStatus.UNKNOWN, AvailableWordUserStatus.WELLKNOWN, context);
        }
        public static async Task PageUserUpdateUnknowWordsToWellKnownAsync(
            IdiomaticaContext context, Guid pageUserId)
        {
            await Task.Run(() =>
            {
                PageUserUpdateUnknowWordsToWellKnown(context, pageUserId);
            });
        }
    }
}
