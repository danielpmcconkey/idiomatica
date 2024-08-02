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
using k8s.KubeConfigModels;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class PageUserApiTests
    {
        [TestMethod()]
        public void PageUserCreateForPageIdAndUserIdTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // get the second page of the book
                var page = context.Pages.Where(x => x.BookId == bookId && x.Ordinal == 1).FirstOrDefault();
                if (page is null || page.Id is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // create the pageUser
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)page.Id, userId);

                // assert
                Assert.IsNotNull(pageUser);
                Assert.IsNotNull(pageUser.Id);
                Assert.AreEqual(page.Id, pageUser.PageId);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageUserCreateForPageIdAndUserIdAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // get the second page of the book
                var page = context.Pages.Where(x => x.BookId == bookId && x.Ordinal == 1).FirstOrDefault();
                if (page is null || page.Id is null)
                    { ErrorHandler.LogAndThrow(); return; }
                
                // create the pageUser
                var pageUser = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, (int)page.Id, userId);

                // assert
                Assert.IsNotNull(pageUser);
                Assert.IsNotNull(pageUser.Id);
                Assert.AreEqual(page.Id, pageUser.PageId);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void PageUserMarkAsReadTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                // pull the languageUser
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create the page 1 page user
                var page1 = PageApi.PageReadByOrdinalAndBookId(context, 1, bookId);
                if (page1 is null || page1.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                int page1Id = (int)page1.Id;
                var pageUserBefore = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page1Id, userId);
                if (pageUserBefore is null || pageUserBefore.Id is null)
                { ErrorHandler.LogAndThrow(); return; }

                PageUserApi.PageUserMarkAsRead(context, (int)pageUserBefore.Id);
                var pageUserAfter = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    context, page1Id, (int)languageUser.Id);
                if (pageUserAfter is null)
                { ErrorHandler.LogAndThrow(); return; }

                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(startDateTime <= pageUserAfter.ReadDate);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageUserMarkAsReadAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                // pull the languageUser
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if(language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create the page 1 page user
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 1, bookId);
                if (page1 is null || page1.Id is null)
                    { ErrorHandler.LogAndThrow(); return; }
                int page1Id = (int)page1.Id;
                var pageUserBefore = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page1Id, userId);
                if (pageUserBefore is null || pageUserBefore.Id is null)
                    { ErrorHandler.LogAndThrow(); return; }
                
                await PageUserApi.PageUserMarkAsReadAsync(context, (int)pageUserBefore.Id);
                var pageUserAfter = await PageUserApi.PageUserReadByPageIdAndLanguageUserIdAsync(
                    context, page1Id, (int)languageUser.Id);
                if (pageUserAfter is null)
                    { ErrorHandler.LogAndThrow(); return; }

                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(startDateTime <= pageUserAfter.ReadDate);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void PageUserReadBookmarkedOrFirstTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                // pull the languageUser
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create the page 1 page user
                var page1 = PageApi.PageReadByOrdinalAndBookId(context, 1, bookId);
                if (page1 is null || page1.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                int page1Id = (int)page1.Id;
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page1Id, userId);

                // create the page 2 page user
                var page2 = PageApi.PageReadByOrdinalAndBookId(context, 2, bookId);
                if (page2 is null || page2.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                int page2Id = (int)page2.Id;
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page2Id, userId);

                // make the call before every doing anything
                var pageUserBefore = PageUserApi.PageUserReadBookmarkedOrFirst(
                    context, bookUserId);
                if (pageUserBefore is null || pageUserBefore.PageId is null)
                { ErrorHandler.LogAndThrow(); return; }
                // pull the page object associated to the pageUser
                var pageBefore = PageApi.PageReadById(context, (int)pageUserBefore.PageId);
                if (pageBefore is null || pageBefore.Ordinal is null)
                { ErrorHandler.LogAndThrow(); return; }
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                BookUserApi.BookUserUpdateBookmark(context, bookUserId, page2Id);

                // now pull the page User again
                var pageUserAfter = PageUserApi.PageUserReadBookmarkedOrFirst(
                    context, bookUserId);
                if (pageUserAfter is null || pageUserAfter.PageId is null)
                { ErrorHandler.LogAndThrow(); return; }
                // pull the page object associated to the pageUser
                var pageAfter = PageApi.PageReadById(context, (int)pageUserAfter.PageId);
                if (pageAfter is null || pageAfter.Ordinal is null)
                { ErrorHandler.LogAndThrow(); return; }
                int ordinalAfterActual = (int)pageAfter.Ordinal;

                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageUserReadBookmarkedOrFirstAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                // pull the languageUser
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create the page 1 page user
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 1, bookId);
                if (page1 is null || page1.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                int page1Id = (int)page1.Id;
                var pageUser1 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page1Id, userId);

                // create the page 2 page user
                var page2 = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 2, bookId);
                if (page2 is null || page2.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                int page2Id = (int)page2.Id;
                var pageUser2 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page2Id, userId);

                // make the call before every doing anything
                var pageUserBefore = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(
                    context, bookUserId);
                if (pageUserBefore is null || pageUserBefore.PageId is null)
                    { ErrorHandler.LogAndThrow(); return; }
                // pull the page object associated to the pageUser
                var pageBefore = await PageApi.PageReadByIdAsync(context, (int)pageUserBefore.PageId);
                if (pageBefore is null || pageBefore.Ordinal is null)
                    { ErrorHandler.LogAndThrow(); return; }
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                await BookUserApi.BookUserUpdateBookmarkAsync(context, bookUserId, page2Id);

                // now pull the page User again
                var pageUserAfter = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(
                    context, bookUserId);
                if (pageUserAfter is null || pageUserAfter.PageId is null)
                    { ErrorHandler.LogAndThrow(); return; }
                // pull the page object associated to the pageUser
                var pageAfter = await PageApi.PageReadByIdAsync(context, (int)pageUserAfter.PageId);
                if (pageAfter is null || pageAfter.Ordinal is null)
                    { ErrorHandler.LogAndThrow(); return; }
                int ordinalAfterActual = (int)pageAfter.Ordinal;

                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void PageUserReadByOrderWithinBookTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int ordinalExpected = 2;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                // pull the languageUser
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create the page 2 page user
                var page2 = PageApi.PageReadByOrdinalAndBookId(context, 2, bookId);
                if (page2 is null || page2.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                int page2Id = (int)page2.Id;
                var pageUserCreated = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page2Id, userId);
                if (pageUserCreated is null || pageUserCreated.PageId is null)
                { ErrorHandler.LogAndThrow(); return; }

                // now pull the pageUser from the DB
                var pageUserRead = PageUserApi.PageUserReadByOrderWithinBook(
                    context, (int)languageUser.Id, 2, bookId);
                if (pageUserRead is null || pageUserRead.PageId is null)
                { ErrorHandler.LogAndThrow(); return; }

                // pull the page object associated to the pageUser you just pulled
                var pageAfter = PageApi.PageReadById(context, (int)pageUserRead.PageId);
                if (pageAfter is null || pageAfter.Ordinal is null)
                { ErrorHandler.LogAndThrow(); return; }
                int ordinalActual = (int)pageAfter.Ordinal;

                // assert
                Assert.AreEqual(ordinalExpected, ordinalActual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageUserReadByOrderWithinBookAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int ordinalExpected = 2;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                // pull the languageUser
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create the page 2 page user
                var page2 = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 2, bookId);
                if (page2 is null || page2.Id is null)
                    { ErrorHandler.LogAndThrow(); return; }
                int page2Id = (int)page2.Id;
                var pageUserCreated = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page2Id, userId);
                if (pageUserCreated is null || pageUserCreated.PageId is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // now pull the pageUser from the DB
                var pageUserRead = await PageUserApi.PageUserReadByOrderWithinBookAsync(
                    context, (int)languageUser.Id, 2, bookId);
                if (pageUserRead is null || pageUserRead.PageId is null)
                    { ErrorHandler.LogAndThrow(); return; }
                
                // pull the page object associated to the pageUser you just pulled
                var pageAfter = await PageApi.PageReadByIdAsync(context, (int)pageUserRead.PageId);
                if (pageAfter is null || pageAfter.Ordinal is null)
                    { ErrorHandler.LogAndThrow(); return; }
                int ordinalActual = (int)pageAfter.Ordinal;

                // assert
                Assert.AreEqual(ordinalExpected, ordinalActual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void PageUserReadByPageIdAndLanguageUserIdTest()
        {
            var context = CommonFunctions.CreateContext();
            int languageUserId = 3;
            int pageId = 79;
            int pageUserIdExpected = 590;

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
            var context = CommonFunctions.CreateContext();
            int languageUserId = 3;
            int pageId = 79;
            int pageUserIdExpected = 590;

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
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // pull the languageUser
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // get the page Id
                var page = PageApi.PageReadByOrdinalAndBookId(context, 1, bookId);
                if (page is null || page.Id is null)
                { ErrorHandler.LogAndThrow(); return; }

                // pull the page User
                var pageUserBefore = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)page.Id, userId);
                if (pageUserBefore is null || pageUserBefore.Id is null)
                { ErrorHandler.LogAndThrow(); return; }

                // now update the read date
                PageUserApi.PageUserUpdateReadDate(context, (int)pageUserBefore.Id, DateTime.Now);

                // now read it back
                var pageUserAfter = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    context, (int)page.Id, (int)languageUser.Id);

                Assert.IsNotNull(pageUserAfter);
                Assert.IsNotNull(pageUserAfter.Id);
                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(pageUserAfter.ReadDate >= startDateTime);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageUserUpdateReadDateAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            var startDateTime = DateTime.Now;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // pull the languageUser
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // get the page Id
                var page = await PageApi.PageReadByOrdinalAndBookIdAsync(context, 1, bookId);
                if (page is null || page.Id is null)
                    { ErrorHandler.LogAndThrow(); return; }
                
                // pull the page User
                var pageUserBefore = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, (int)page.Id, userId);
                if (pageUserBefore is null || pageUserBefore.Id is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // now update the read date
                await PageUserApi.PageUserUpdateReadDateAsync(context, (int)pageUserBefore.Id, DateTime.Now);

                // now read it back
                var pageUserAfter = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    context, (int)page.Id, (int)languageUser.Id);

                Assert.IsNotNull(pageUserAfter);
                Assert.IsNotNull(pageUserAfter.Id);
                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(pageUserAfter.ReadDate >= startDateTime);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void PageUserUpdateUnknowWordsToWellKnownTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            decimal expectedWordCount = 784.0M;
            decimal expectedDistinctKnowPercent = 0.51M;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                // pull the languageUser
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }


                var originalPacket = new BookListDataPacket(context, false);

                // carry on with the test
                var bookUserStats = BookUserStatApi.BookUserStatsRead(context, bookId, userId);
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT).FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = PageApi.PageReadFirstByBookId(context, bookId);
                if (firstPage is null || firstPage.Id is null || firstPage.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPage.Id, userId);
                if (pageUser is null || pageUser.Id is null || pageUser.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }


                // update all unknowns to well known
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser.Id);
                // mark the first page as read
                PageUserApi.PageUserMarkAsRead(context, (int)pageUser.Id);
                // now move forward to page 2
                var newPageUser = PageUserApi.PageUserReadByOrderWithinBook(
                    context, (int)languageUser.Id, 2, bookId);

                // now we need to regen the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(context, bookUserId);
                // finally, re-pull the stats
                var newStats = BookUserStatApi.BookUserStatsRead(context, bookId, userId);
                BookUserStat? distinctKnownPercentStat = newStats is null ? null :
                    newStats.Where(x => x.Key == AvailableBookUserStat.DISTINCTKNOWNPERCENT).FirstOrDefault();
                if (distinctKnownPercentStat is null || distinctKnownPercentStat.ValueNumeric is null)
                { ErrorHandler.LogAndThrow(); return; }
                decimal actualDistinctKnown = Math.Round((decimal)distinctKnownPercentStat.ValueNumeric, 2);

                // assert
                // this will fail because we're no longer using book 6
                Assert.AreEqual(expectedWordCount, actualWordCount);
                Assert.AreEqual(expectedDistinctKnowPercent, actualDistinctKnown);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageUserUpdateUnknowWordsToWellKnownAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            decimal expectedWordCount = 784.0M;
            decimal expectedDistinctKnowPercent = 0.51M;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                // pull the languageUser
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return; }
                var languageUser = LanguageUserApi.LanguageUserGet(context, (int)language.Id, userId);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }


                var originalPacket = new BookListDataPacket(context, false);
                
                // carry on with the test
                var bookUserStats = await BookUserStatApi.BookUserStatsReadAsync(context, bookId, userId);
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT).FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = await PageApi.PageReadFirstByBookIdAsync(context, bookId);
                if (firstPage is null || firstPage.Id is null || firstPage.Id < 1)
                    { ErrorHandler.LogAndThrow(); return; }
                var pageUser = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, (int)firstPage.Id, userId);
                if (pageUser is null || pageUser.Id is null || pageUser.Id < 1)
                    { ErrorHandler.LogAndThrow(); return; }


                // update all unknowns to well known
                await PageUserApi.PageUserUpdateUnknowWordsToWellKnownAsync(context, (int)pageUser.Id);
                // mark the first page as read
                await PageUserApi.PageUserMarkAsReadAsync(context, (int)pageUser.Id);
                // now move forward to page 2
                var newPageUser = await PageUserApi.PageUserReadByOrderWithinBookAsync(
                    context, (int)languageUser.Id, 2, bookId);

                // now we need to regen the stats
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(context, bookUserId);
                // finally, re-pull the stats
                var newStats = await BookUserStatApi.BookUserStatsReadAsync(context, bookId, userId);
                BookUserStat? distinctKnownPercentStat = newStats is null ? null :
                    newStats.Where(x => x.Key == AvailableBookUserStat.DISTINCTKNOWNPERCENT).FirstOrDefault();
                if (distinctKnownPercentStat is null || distinctKnownPercentStat.ValueNumeric is null)
                    { ErrorHandler.LogAndThrow(); return; }
                decimal actualDistinctKnown = Math.Round((decimal)distinctKnownPercentStat.ValueNumeric, 2);

                // assert
                // this will fail because we're no longer using book 6
                Assert.AreEqual(expectedWordCount, actualWordCount);
                Assert.AreEqual(expectedDistinctKnowPercent, actualDistinctKnown);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
    }
}