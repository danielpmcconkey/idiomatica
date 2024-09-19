using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Logic.Telemetry;
using Model;
using System.Net;
using Model.Enums;
using Microsoft.EntityFrameworkCore;
using Model.DAL;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class PageUserApiTests
    {
        [TestMethod()]
        public void PageUserCreateForPageIdAndUserIdTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var startDateTime = DateTime.Now;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // get the second page of the book
                var page = context.Pages
                    .Where(x => x.BookId == bookId && x.Ordinal == 1)
                    .FirstOrDefault();
                Assert.IsNotNull(page);

                // create the pageUser
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, page, user);

                // assert
                Assert.IsNotNull(pageUser);
                Assert.IsNotNull(pageUser.Id);
                Assert.AreEqual(page.Id, pageUser.PageId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task PageUserCreateForPageIdAndUserIdAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var startDateTime = DateTime.Now;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // get the second page of the book
                var page = context.Pages
                    .Where(x => x.BookId == bookId && x.Ordinal == 1)
                    .FirstOrDefault();
                Assert.IsNotNull(page);
                
                // create the pageUser
                var pageUser = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    dbContextFactory, page, user);

                // assert
                Assert.IsNotNull(pageUser);
                Assert.IsNotNull(pageUser.Id);
                Assert.AreEqual(page.Id, pageUser.PageId);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void PageUserMarkAsReadTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var startDateTime = DateTime.Now;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 1 page user
                var page1 = PageApi.PageReadByOrdinalAndBookId(dbContextFactory, 1, (Guid)bookId);
                Assert.IsNotNull(page1);
                Guid page1Id = (Guid)page1.Id;
                var pageUserBefore = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, page1, user);
                Assert.IsNotNull(pageUserBefore);

                PageUserApi.PageUserMarkAsRead(dbContextFactory, (Guid)pageUserBefore.Id);
                var pageUserAfter = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    dbContextFactory, page1Id, (Guid)languageUser.Id);
                Assert.IsNotNull(pageUserAfter);

                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(startDateTime <= pageUserAfter.ReadDate);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task PageUserMarkAsReadAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var startDateTime = DateTime.Now;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 1 page user
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(dbContextFactory, 1, (Guid)bookId);
                Assert.IsNotNull(page1);
                Guid page1Id = (Guid)page1.Id;
                var pageUserBefore = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    dbContextFactory, page1, user);
                Assert.IsNotNull(pageUserBefore);
                
                await PageUserApi.PageUserMarkAsReadAsync(dbContextFactory, (Guid)pageUserBefore.Id);
                var pageUserAfter = await PageUserApi.PageUserReadByPageIdAndLanguageUserIdAsync(
                    dbContextFactory, page1Id, (Guid)languageUser.Id);
                Assert.IsNotNull(pageUserAfter);

                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(startDateTime <= pageUserAfter.ReadDate);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void PageUserReadBookmarkedOrFirstTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 1 page user
                var page1 = PageApi.PageReadByOrdinalAndBookId(dbContextFactory, 1, (Guid)bookId);
                Assert.IsNotNull(page1);
                Guid page1Id = (Guid)page1.Id;
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, page1, user);

                // create the page 2 page user
                var page2 = PageApi.PageReadByOrdinalAndBookId(dbContextFactory, 2, (Guid)bookId);
                Assert.IsNotNull(page2);
                Guid page2Id = (Guid)page2.Id;
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, page2, user);

                // make the call before every doing anything
                var pageUserBefore = PageUserApi.PageUserReadBookmarkedOrFirst(
                    dbContextFactory, bookUserId);
                Assert.IsNotNull(pageUserBefore);
                // pull the page object associated to the pageUser
                var pageBefore = PageApi.PageReadById(dbContextFactory, (Guid)pageUserBefore.PageId);
                Assert.IsNotNull(pageBefore);
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                BookUserApi.BookUserUpdateBookmark(dbContextFactory, bookUserId, page2Id);

                // now pull the page User again
                var pageUserAfter = PageUserApi.PageUserReadBookmarkedOrFirst(
                    dbContextFactory, bookUserId);
                Assert.IsNotNull(pageUserAfter);
                // pull the page object associated to the pageUser
                var pageAfter = PageApi.PageReadById(dbContextFactory, (Guid)pageUserAfter.PageId);
                Assert.IsNotNull(pageAfter);
                int ordinalAfterActual = (int)pageAfter.Ordinal;

                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task PageUserReadBookmarkedOrFirstAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 1 page user
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(dbContextFactory, 1, (Guid)bookId);
                Assert.IsNotNull(page1);
                Guid page1Id = (Guid)page1.Id;
                var pageUser1 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    dbContextFactory, page1, user);

                // create the page 2 page user
                var page2 = await PageApi.PageReadByOrdinalAndBookIdAsync(dbContextFactory, 2, (Guid)bookId);
                Assert.IsNotNull(page2);
                Guid page2Id = (Guid)page2.Id;
                var pageUser2 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    dbContextFactory, page2, user);

                // make the call before every doing anything
                var pageUserBefore = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(
                    dbContextFactory, bookUserId);
                Assert.IsNotNull(pageUserBefore);
                // pull the page object associated to the pageUser
                var pageBefore = await PageApi.PageReadByIdAsync(dbContextFactory, (Guid)pageUserBefore.PageId);
                Assert.IsNotNull(pageBefore);
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                await BookUserApi.BookUserUpdateBookmarkAsync(dbContextFactory, bookUserId, page2Id);

                // now pull the page User again
                var pageUserAfter = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(
                    dbContextFactory, bookUserId);
                Assert.IsNotNull(pageUserAfter);
                // pull the page object associated to the pageUser
                var pageAfter = await PageApi.PageReadByIdAsync(dbContextFactory, (Guid)pageUserAfter.PageId);
                Assert.IsNotNull(pageAfter);
                int ordinalAfterActual = (int)pageAfter.Ordinal;

                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void PageUserReadByOrderWithinBookTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int ordinalExpected = 2;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 2 page user
                var page2 = PageApi.PageReadByOrdinalAndBookId(dbContextFactory, 2, (Guid)bookId);
                Assert.IsNotNull(page2);
                Guid page2Id = (Guid)page2.Id;
                var pageUserCreated = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, page2, user);
                Assert.IsNotNull(pageUserCreated);

                // now pull the pageUser from the DB
                var pageUserRead = PageUserApi.PageUserReadByOrderWithinBook(
                    dbContextFactory, (Guid)languageUser.Id, 2, (Guid)bookId);
                Assert.IsNotNull(pageUserRead);

                // pull the page object associated to the pageUser you just pulled
                var pageAfter = PageApi.PageReadById(dbContextFactory, (Guid)pageUserRead.PageId);
                Assert.IsNotNull(pageAfter);
                int ordinalActual = (int)pageAfter.Ordinal;

                // assert
                Assert.AreEqual(ordinalExpected, ordinalActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task PageUserReadByOrderWithinBookAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int ordinalExpected = 2;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 2 page user
                var page2 = await PageApi.PageReadByOrdinalAndBookIdAsync(dbContextFactory, 2, (Guid)bookId);
                Assert.IsNotNull(page2);
                Guid page2Id = (Guid)page2.Id;
                var pageUserCreated = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    dbContextFactory, page2, user);
                Assert.IsNotNull(pageUserCreated);

                // now pull the pageUser from the DB
                var pageUserRead = await PageUserApi.PageUserReadByOrderWithinBookAsync(
                    dbContextFactory, (Guid)languageUser.Id, 2, (Guid)bookId);
                Assert.IsNotNull(pageUserRead);
                
                // pull the page object associated to the pageUser you just pulled
                var pageAfter = await PageApi.PageReadByIdAsync(dbContextFactory, (Guid)pageUserRead.PageId);
                Assert.IsNotNull(pageAfter);
                int ordinalActual = (int)pageAfter.Ordinal;

                // assert
                Assert.AreEqual(ordinalExpected, ordinalActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void PageUserReadByPageIdAndLanguageUserIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(dbContextFactory);
            Guid pageId = CommonFunctions.GetGuionParaPage1Id(dbContextFactory);
            Guid pageUserIdExpected = CommonFunctions.GetPageUser380Id(dbContextFactory, languageUserId);

            var pageUser = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    dbContextFactory, pageId, languageUserId);

            Assert.IsNotNull(pageUser);
            Assert.IsNotNull(pageUser.Id);
            Assert.AreEqual(pageId, pageUser.PageId);
            Assert.AreEqual(pageUserIdExpected, pageUser.Id);
        }
        [TestMethod()]
        public async Task PageUserReadByPageIdAndLanguageUserIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(dbContextFactory);
            Guid pageId = CommonFunctions.GetGuionParaPage1Id(dbContextFactory);
            Guid pageUserIdExpected = CommonFunctions.GetPageUser380Id(dbContextFactory, languageUserId);

            var pageUser = await PageUserApi.PageUserReadByPageIdAndLanguageUserIdAsync(
                    dbContextFactory, pageId, languageUserId);

            Assert.IsNotNull(pageUser);
            Assert.IsNotNull(pageUser.Id);
            Assert.AreEqual(pageId, pageUser.PageId);
            Assert.AreEqual(pageUserIdExpected, pageUser.Id);
        }


        [TestMethod()]
        public void PageUserUpdateReadDateTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var startDateTime = DateTime.Now;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // get the page Id
                var page = PageApi.PageReadByOrdinalAndBookId(dbContextFactory, 1, (Guid)bookId);
                Assert.IsNotNull(page);

                // pull the page User
                var pageUserBefore = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, page, user);
                Assert.IsNotNull(pageUserBefore);

                // now update the read date
                PageUserApi.PageUserUpdateReadDate(
                    dbContextFactory, (Guid)pageUserBefore.Id, DateTime.Now);

                // now read it back
                var pageUserAfter = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    dbContextFactory, (Guid)page.Id, (Guid)languageUser.Id);

                Assert.IsNotNull(pageUserAfter);
                Assert.IsNotNull(pageUserAfter.Id);
                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(pageUserAfter.ReadDate >= startDateTime);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task PageUserUpdateReadDateAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var startDateTime = DateTime.Now;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // get the page Id
                var page = await PageApi.PageReadByOrdinalAndBookIdAsync(dbContextFactory, 1, (Guid)bookId);
                Assert.IsNotNull(page);
                
                // pull the page User
                var pageUserBefore = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    dbContextFactory, page, user);
                Assert.IsNotNull(pageUserBefore);

                // now update the read date
                await PageUserApi.PageUserUpdateReadDateAsync(
                    dbContextFactory, (Guid)pageUserBefore.Id, DateTime.Now);

                // now read it back
                var pageUserAfter = await PageUserApi.PageUserReadByPageIdAndLanguageUserIdAsync(
                    dbContextFactory, (Guid)page.Id, (Guid)languageUser.Id);

                Assert.IsNotNull(pageUserAfter);
                Assert.IsNotNull(pageUserAfter.Id);
                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(pageUserAfter.ReadDate >= startDateTime);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void PageUserUpdateUnknowWordsToWellKnownTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            decimal expectedWordCount = 784.0M;
            decimal expectedDistinctKnowPercent = 0.46M;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);


                var originalPacket = new BookListDataPacket(dbContextFactory, false);

                // carry on with the test
                var bookUserStats = BookUserStatApi.BookUserStatsRead(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT).FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = PageApi.PageReadFirstByBookId(dbContextFactory, (Guid)bookId);
                Assert.IsNotNull(firstPage);
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, firstPage, user);
                Assert.IsNotNull(pageUser);

                // update all unknowns to well known
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(
                    dbContextFactory, (Guid)pageUser.Id);
                // mark the first page as read
                PageUserApi.PageUserMarkAsRead(dbContextFactory, (Guid)pageUser.Id);
                // now move forward to page 2
                var newPageUser = PageUserApi.PageUserReadByOrderWithinBook(
                    dbContextFactory, (Guid)languageUser.Id, 2, (Guid)bookId);

                // now we need to regen the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(dbContextFactory, bookUserId);
                // finally, re-pull the stats
                var newStats = BookUserStatApi.BookUserStatsRead(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                BookUserStat? distinctKnownPercentStat = newStats is null ? null :
                    newStats.Where(x => x.Key == AvailableBookUserStat.DISTINCTKNOWNPERCENT)
                    .FirstOrDefault();
                Assert.IsNotNull(distinctKnownPercentStat);
                Assert.IsNotNull(distinctKnownPercentStat.ValueNumeric);
                decimal actualDistinctKnown = Math.Round(
                    (decimal)distinctKnownPercentStat.ValueNumeric, 2);

                // assert
                // this will fail because we're no longer using book 6
                Assert.AreEqual(expectedWordCount, actualWordCount);
                Assert.AreEqual(expectedDistinctKnowPercent, actualDistinctKnown);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task PageUserUpdateUnknowWordsToWellKnownAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            decimal expectedWordCount = 784.0M;
            decimal expectedDistinctKnowPercent = 0.46M;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(dbContextFactory);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);


                var originalPacket = new BookListDataPacket(dbContextFactory, false);
                
                // carry on with the test
                var bookUserStats = await BookUserStatApi.BookUserStatsReadAsync(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT).FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = await PageApi.PageReadFirstByBookIdAsync(dbContextFactory, (Guid)bookId);
                Assert.IsNotNull(firstPage);
                var pageUser = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    dbContextFactory, firstPage, user);
                Assert.IsNotNull(pageUser);

                // update all unknowns to well known
                await PageUserApi.PageUserUpdateUnknowWordsToWellKnownAsync(
                    dbContextFactory, (Guid)pageUser.Id);
                // mark the first page as read
                await PageUserApi.PageUserMarkAsReadAsync(dbContextFactory, (Guid)pageUser.Id);
                // now move forward to page 2
                var newPageUser = await PageUserApi.PageUserReadByOrderWithinBookAsync(
                    dbContextFactory, (Guid)languageUser.Id, 2, (Guid)bookId);

                // now we need to regen the stats
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(dbContextFactory, bookUserId);
                // finally, re-pull the stats
                var newStats = await BookUserStatApi.BookUserStatsReadAsync(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                BookUserStat? distinctKnownPercentStat = newStats is null ? null :
                    newStats.Where(x => x.Key == AvailableBookUserStat.DISTINCTKNOWNPERCENT)
                    .FirstOrDefault();
                Assert.IsNotNull(distinctKnownPercentStat);
                Assert.IsNotNull(distinctKnownPercentStat.ValueNumeric);
                decimal actualDistinctKnown = Math.Round(
                    (decimal)distinctKnownPercentStat.ValueNumeric, 2);

                // assert
                // this will fail because we're no longer using book 6
                Assert.AreEqual(expectedWordCount, actualWordCount);
                Assert.AreEqual(expectedDistinctKnowPercent, actualDistinctKnown);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }
    }
}