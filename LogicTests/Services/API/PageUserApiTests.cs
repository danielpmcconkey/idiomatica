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
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                User? user = userService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // get the second page of the book
                var page = context.Pages.Where(x => x.BookId == bookId && x.Ordinal == 1).FirstOrDefault();
                if (page is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // create the pageUser
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page, user);

                // assert
                Assert.IsNotNull(pageUser);
                Assert.IsNotNull(pageUser.Id);
                Assert.AreEqual(page.Id, pageUser.PageId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageUserCreateForPageIdAndUserIdAsyncTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                User? user = userService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // get the second page of the book
                var page = context.Pages.Where(x => x.BookId == bookId && x.Ordinal == 1).FirstOrDefault();
                if (page is null)
                    { ErrorHandler.LogAndThrow(); return; }
                
                // create the pageUser
                var pageUser = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page, user);

                // assert
                Assert.IsNotNull(pageUser);
                Assert.IsNotNull(pageUser.Id);
                Assert.AreEqual(page.Id, pageUser.PageId);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void PageUserMarkAsReadTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = userService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // pull the languageUser
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (Guid)language.Id, userId);
                if (languageUser is null) { ErrorHandler.LogAndThrow(); return; }

                // create the page 1 page user
                var page1 = PageApi.PageReadByOrdinalAndBookId(context, 1, bookId);
                if (page1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                Guid page1Id = (Guid)page1.Id;
                var pageUserBefore = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page1, user);
                if (pageUserBefore is null)
                { ErrorHandler.LogAndThrow(); return; }

                PageUserApi.PageUserMarkAsRead(context, (Guid)pageUserBefore.Id);
                var pageUserAfter = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    context, page1Id, (Guid)languageUser.Id);
                if (pageUserAfter is null)
                { ErrorHandler.LogAndThrow(); return; }

                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(startDateTime <= pageUserAfter.ReadDate);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageUserMarkAsReadAsyncTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = userService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // pull the languageUser
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if(language is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(context, (Guid)language.Id, userId);
                if (languageUser is null) { ErrorHandler.LogAndThrow(); return; }

                // create the page 1 page user
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 1, bookId);
                if (page1 is null)
                    { ErrorHandler.LogAndThrow(); return; }
                Guid page1Id = (Guid)page1.Id;
                var pageUserBefore = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page1, user);
                if (pageUserBefore is null)
                    { ErrorHandler.LogAndThrow(); return; }
                
                await PageUserApi.PageUserMarkAsReadAsync(context, (Guid)pageUserBefore.Id);
                var pageUserAfter = await PageUserApi.PageUserReadByPageIdAndLanguageUserIdAsync(
                    context, page1Id, (Guid)languageUser.Id);
                if (pageUserAfter is null)
                    { ErrorHandler.LogAndThrow(); return; }

                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(startDateTime <= pageUserAfter.ReadDate);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 1 page user
                var page1 = PageApi.PageReadByOrdinalAndBookId(context, 1, (Guid)bookId);
                Assert.IsNotNull(page1);
                Guid page1Id = (Guid)page1.Id;
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page1, user);

                // create the page 2 page user
                var page2 = PageApi.PageReadByOrdinalAndBookId(context, 2, (Guid)bookId);
                Assert.IsNotNull(page2);
                Guid page2Id = (Guid)page2.Id;
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page2, user);

                // make the call before every doing anything
                var pageUserBefore = PageUserApi.PageUserReadBookmarkedOrFirst(
                    context, bookUserId);
                Assert.IsNotNull(pageUserBefore);
                // pull the page object associated to the pageUser
                var pageBefore = PageApi.PageReadById(context, (Guid)pageUserBefore.PageId);
                Assert.IsNotNull(pageBefore);
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                BookUserApi.BookUserUpdateBookmark(context, bookUserId, page2Id);

                // now pull the page User again
                var pageUserAfter = PageUserApi.PageUserReadBookmarkedOrFirst(
                    context, bookUserId);
                Assert.IsNotNull(pageUserAfter);
                // pull the page object associated to the pageUser
                var pageAfter = PageApi.PageReadById(context, (Guid)pageUserAfter.PageId);
                Assert.IsNotNull(pageAfter);
                int ordinalAfterActual = (int)pageAfter.Ordinal;

                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 1 page user
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 1, (Guid)bookId);
                Assert.IsNotNull(page1);
                Guid page1Id = (Guid)page1.Id;
                var pageUser1 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page1, user);

                // create the page 2 page user
                var page2 = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 2, (Guid)bookId);
                Assert.IsNotNull(page2);
                Guid page2Id = (Guid)page2.Id;
                var pageUser2 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page2, user);

                // make the call before every doing anything
                var pageUserBefore = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(
                    context, bookUserId);
                Assert.IsNotNull(pageUserBefore);
                // pull the page object associated to the pageUser
                var pageBefore = await PageApi.PageReadByIdAsync(context, (Guid)pageUserBefore.PageId);
                Assert.IsNotNull(pageBefore);
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                await BookUserApi.BookUserUpdateBookmarkAsync(context, bookUserId, page2Id);

                // now pull the page User again
                var pageUserAfter = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(
                    context, bookUserId);
                Assert.IsNotNull(pageUserAfter);
                // pull the page object associated to the pageUser
                var pageAfter = await PageApi.PageReadByIdAsync(context, (Guid)pageUserAfter.PageId);
                Assert.IsNotNull(pageAfter);
                int ordinalAfterActual = (int)pageAfter.Ordinal;

                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            int ordinalExpected = 2;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 2 page user
                var page2 = PageApi.PageReadByOrdinalAndBookId(context, 2, (Guid)bookId);
                Assert.IsNotNull(page2);
                Guid page2Id = (Guid)page2.Id;
                var pageUserCreated = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page2, user);
                Assert.IsNotNull(pageUserCreated);

                // now pull the pageUser from the DB
                var pageUserRead = PageUserApi.PageUserReadByOrderWithinBook(
                    context, (Guid)languageUser.Id, 2, (Guid)bookId);
                Assert.IsNotNull(pageUserRead);

                // pull the page object associated to the pageUser you just pulled
                var pageAfter = PageApi.PageReadById(context, (Guid)pageUserRead.PageId);
                Assert.IsNotNull(pageAfter);
                int ordinalActual = (int)pageAfter.Ordinal;

                // assert
                Assert.AreEqual(ordinalExpected, ordinalActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            int ordinalExpected = 2;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the page 2 page user
                var page2 = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 2, (Guid)bookId);
                Assert.IsNotNull(page2);
                Guid page2Id = (Guid)page2.Id;
                var pageUserCreated = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page2, user);
                Assert.IsNotNull(pageUserCreated);

                // now pull the pageUser from the DB
                var pageUserRead = await PageUserApi.PageUserReadByOrderWithinBookAsync(
                    context, (Guid)languageUser.Id, 2, (Guid)bookId);
                Assert.IsNotNull(pageUserRead);
                
                // pull the page object associated to the pageUser you just pulled
                var pageAfter = await PageApi.PageReadByIdAsync(context, (Guid)pageUserRead.PageId);
                Assert.IsNotNull(pageAfter);
                int ordinalActual = (int)pageAfter.Ordinal;

                // assert
                Assert.AreEqual(ordinalExpected, ordinalActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void PageUserReadByPageIdAndLanguageUserIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(context);
            Guid pageId = CommonFunctions.GetPage384Id(context);
            Guid pageUserIdExpected = CommonFunctions.GetPageUser380Id(context, languageUserId);

            var pageUser = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    context, pageId, languageUserId);

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
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(context);
            Guid pageId = CommonFunctions.GetPage384Id(context);
            Guid pageUserIdExpected = CommonFunctions.GetPageUser380Id(context, languageUserId);

            var pageUser = await PageUserApi.PageUserReadByPageIdAndLanguageUserIdAsync(
                    context, pageId, languageUserId);

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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            var startDateTime = DateTime.Now;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                User? user = loginService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // get the page Id
                var page = PageApi.PageReadByOrdinalAndBookId(context, 1, (Guid)bookId);
                Assert.IsNotNull(page);

                // pull the page User
                var pageUserBefore = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page, user);
                Assert.IsNotNull(pageUserBefore);

                // now update the read date
                PageUserApi.PageUserUpdateReadDate(
                    context, (Guid)pageUserBefore.Id, DateTime.Now);

                // now read it back
                var pageUserAfter = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    context, (Guid)page.Id, (Guid)languageUser.Id);

                Assert.IsNotNull(pageUserAfter);
                Assert.IsNotNull(pageUserAfter.Id);
                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(pageUserAfter.ReadDate >= startDateTime);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            var startDateTime = DateTime.Now;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                User? user = loginService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // get the page Id
                var page = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 1, (Guid)bookId);
                Assert.IsNotNull(page);
                
                // pull the page User
                var pageUserBefore = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page, user);
                Assert.IsNotNull(pageUserBefore);

                // now update the read date
                await PageUserApi.PageUserUpdateReadDateAsync(
                    context, (Guid)pageUserBefore.Id, DateTime.Now);

                // now read it back
                var pageUserAfter = await PageUserApi.PageUserReadByPageIdAndLanguageUserIdAsync(
                    context, (Guid)page.Id, (Guid)languageUser.Id);

                Assert.IsNotNull(pageUserAfter);
                Assert.IsNotNull(pageUserAfter.Id);
                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(pageUserAfter.ReadDate >= startDateTime);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            decimal expectedWordCount = 784.0M;
            decimal expectedDistinctKnowPercent = 0.51M;

            try
            {
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);


                var originalPacket = new BookListDataPacket(context, false);

                // carry on with the test
                var bookUserStats = BookUserStatApi.BookUserStatsRead(
                    context, (Guid)bookId, (Guid)userId);
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT).FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = PageApi.PageReadFirstByBookId(context, (Guid)bookId);
                Assert.IsNotNull(firstPage);
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, firstPage, user);
                Assert.IsNotNull(pageUser);

                // update all unknowns to well known
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(
                    context, (Guid)pageUser.Id);
                // mark the first page as read
                PageUserApi.PageUserMarkAsRead(context, (Guid)pageUser.Id);
                // now move forward to page 2
                var newPageUser = PageUserApi.PageUserReadByOrderWithinBook(
                    context, (Guid)languageUser.Id, 2, (Guid)bookId);

                // now we need to regen the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(context, bookUserId);
                // finally, re-pull the stats
                var newStats = BookUserStatApi.BookUserStatsRead(
                    context, (Guid)bookId, (Guid)userId);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            decimal expectedWordCount = 784.0M;
            decimal expectedDistinctKnowPercent = 0.51M;

            try
            {
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                User? user = loginService.GetLoggedInUser(context);
                Assert.IsNotNull(user);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);


                var originalPacket = new BookListDataPacket(context, false);
                
                // carry on with the test
                var bookUserStats = await BookUserStatApi.BookUserStatsReadAsync(
                    context, (Guid)bookId, (Guid)userId);
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT).FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = await PageApi.PageReadFirstByBookIdAsync(context, (Guid)bookId);
                Assert.IsNotNull(firstPage);
                var pageUser = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, firstPage, user);
                Assert.IsNotNull(pageUser);

                // update all unknowns to well known
                await PageUserApi.PageUserUpdateUnknowWordsToWellKnownAsync(
                    context, (Guid)pageUser.Id);
                // mark the first page as read
                await PageUserApi.PageUserMarkAsReadAsync(context, (Guid)pageUser.Id);
                // now move forward to page 2
                var newPageUser = await PageUserApi.PageUserReadByOrderWithinBookAsync(
                    context, (Guid)languageUser.Id, 2, (Guid)bookId);

                // now we need to regen the stats
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(context, bookUserId);
                // finally, re-pull the stats
                var newStats = await BookUserStatApi.BookUserStatsReadAsync(
                    context, (Guid)bookId, (Guid)userId);
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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }
    }
}