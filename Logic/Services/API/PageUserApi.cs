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
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class PageUserApi
    {
        public static PageUser? PageUserCreateForPageIdAndUserId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Page page, User user)
        {
            var book = DataCache.BookByIdRead((Guid)page.BookId, dbContextFactory);
            if (book is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var languageUser = DataCache.LanguageUserByLanguageIdAndUserIdRead(
                (book.LanguageId, user.Id), dbContextFactory);
            if (languageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // make sure it doesn't already exist
            var existingPageUser = DataCache.PageUserByPageIdAndLanguageUserIdRead(
                (page.Id, languageUser.Id), dbContextFactory);
            if (existingPageUser is not null) return existingPageUser;

            // nope, definitely create it
            // but first, more stuff to look up
            var bookUser = DataCache.BookUserByBookIdAndUserIdRead(
                (book.Id, user.Id), dbContextFactory);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // save it for real
            var pageUser = new PageUser()
            {
                Id = Guid.NewGuid(),
                BookUserId = bookUser.Id,
                BookUser = bookUser,
                PageId = page.Id,
                Page = page,
            };
            pageUser = DataCache.PageUserCreate(pageUser, dbContextFactory);
            if (pageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return pageUser;
        }
        public static async Task<PageUser?> PageUserCreateForPageIdAndUserIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Page page, User user)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserCreateForPageIdAndUserId(dbContextFactory, page, user);
            });
        }


        public static void PageUserMarkAsRead(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageUserId)
        {
            var readDate = DateTime.Now;
            PageUserUpdateReadDate(dbContextFactory, pageUserId, readDate);
        }
        public static async Task PageUserMarkAsReadAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageUserId)
        {
            var readDate = DateTime.Now;
            await PageUserUpdateReadDateAsync(dbContextFactory, pageUserId, readDate);
        }


        /// <summary>
        /// PageUserReadBookmarkedOrFirst fetches the PageUser object 
        /// associated with either BookUser's current page or, if there is no 
        /// PageUser associated to the bookmark, try to fetch the PageUser for
        /// page 1. If both of those return null, return null from this method
        /// </summary>
        public static PageUser? PageUserReadBookmarkedOrFirst(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookUserId)
        {
            var bookUser = DataCache.BookUserByIdRead(bookUserId, dbContextFactory);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // try to pull the bookmarked page
            if (bookUser.CurrentPageId is not null)
            {
                return PageUserReadByPageIdAndLanguageUserId(
                    dbContextFactory, (Guid)bookUser.CurrentPageId, (Guid)bookUser.LanguageUserId);
            }
            // no bookmark, pull page 1
            return PageUserReadByOrderWithinBook(
                dbContextFactory, (Guid)bookUser.LanguageUserId, 1, (Guid)bookUser.BookId);
        }
        public static async Task<PageUser?> PageUserReadBookmarkedOrFirstAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookUserId)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserReadBookmarkedOrFirst(dbContextFactory, bookUserId);
            });
        }


        public static PageUser? PageUserReadByOrderWithinBook(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageUserId, int pageOrdinal, Guid bookId)
        {
            return DataCache.PageUserByLanguageUserIdOrdinalAndBookIdRead(
                (languageUserId, pageOrdinal, bookId), dbContextFactory);
        }
        public static async Task<PageUser?> PageUserReadByOrderWithinBookAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageUserId, int pageOrdinal, Guid bookId)
        {
            return await DataCache.PageUserByLanguageUserIdOrdinalAndBookIdReadAsync(
                (languageUserId, pageOrdinal, bookId), dbContextFactory);
        }


        public static PageUser? PageUserReadByPageIdAndLanguageUserId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid currentPageID, Guid languageUserId)
        {
            return DataCache.PageUserByPageIdAndLanguageUserIdRead(
                    (currentPageID, languageUserId), dbContextFactory);
        }
        public static async Task<PageUser?> PageUserReadByPageIdAndLanguageUserIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid currentPageID, Guid languageUserId)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserReadByPageIdAndLanguageUserId(dbContextFactory, currentPageID, languageUserId);
            });
        }


        public static void PageUserUpdateReadDate(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid id, DateTime readDate)
        {
            var pu = DataCache.PageUserByIdRead(id, dbContextFactory);
            if (pu is null) { ErrorHandler.LogAndThrow(); return; }
            pu.ReadDate = readDate;
            DataCache.PageUserUpdate(pu, dbContextFactory);
            return;
        }
        public static async Task PageUserUpdateReadDateAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid id, DateTime readDate)
        {
            var pu = await DataCache.PageUserByIdReadAsync(id, dbContextFactory);
            if (pu is null) { ErrorHandler.LogAndThrow(); return; }
            pu.ReadDate = readDate;
            await DataCache.PageUserUpdateAsync(pu, dbContextFactory);
            return;
        }


        public static void PageUserUpdateUnknowWordsToWellKnown(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageUserId)
        {
            DataCache.WordUsersUpdateStatusByPageUserIdAndStatus(
                pageUserId, AvailableWordUserStatus.UNKNOWN, AvailableWordUserStatus.WELLKNOWN, dbContextFactory);
        }
        public static async Task PageUserUpdateUnknowWordsToWellKnownAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageUserId)
        {
            await Task.Run(() =>
            {
                PageUserUpdateUnknowWordsToWellKnown(dbContextFactory, pageUserId);
            });
        }
    }
}
