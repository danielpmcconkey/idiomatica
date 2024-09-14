using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Model;
using Logic.Telemetry;
using System.Net;
using Model.Enums;


namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookUserStatApiTests
    {
        [TestMethod()]
        public void BookUserStatsReadTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            int expectedStatsCount = 8;
            string expectedProgress = "0 / 3";
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);


            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                // grab the expected language user ID
                var lu = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(context, userId);
                if (lu is null || lu.Count < 1) { ErrorHandler.LogAndThrow(); return; }
                var l = lu.Where(x => x.LanguageKey == languageId).FirstOrDefault();
                if (l is null) { ErrorHandler.LogAndThrow(); return; }
                Guid expectedLanguageUserId = (Guid)l.UniqueKey;


                // carry on with the test
                var bookUserStats = BookUserStatApi.BookUserStatsRead(context, bookId, userId);
                int actualStatsCount = bookUserStats is null ? 0 : bookUserStats.Count;
                BookUserStat? progressStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.PROGRESS).FirstOrDefault();
                if (progressStat is null || progressStat.ValueString is null)
                { ErrorHandler.LogAndThrow(); return; }
                string actualProgress = progressStat.ValueString;


                // assert

                Assert.AreEqual(expectedStatsCount, actualStatsCount);
                Assert.AreEqual(expectedLanguageUserId, progressStat.LanguageUserKey);
                Assert.AreEqual(expectedProgress, actualProgress);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookUserStatsReadAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            int expectedStatsCount = 8;
            string expectedProgress = "0 / 3";
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);


            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                // grab the expected language user ID
                var lu = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(context, userId);
                if (lu is null || lu.Count < 1) { ErrorHandler.LogAndThrow(); return; }
                var l = lu.Where(x => x.LanguageKey == languageId).FirstOrDefault();
                if (l is null) { ErrorHandler.LogAndThrow(); return; }
                Guid expectedLanguageUserId = (Guid)l.UniqueKey;


                // carry on with the test
                var bookUserStats = await BookUserStatApi.BookUserStatsReadAsync(context, bookId, userId);
                int actualStatsCount = bookUserStats is null ? 0 : bookUserStats.Count;
                BookUserStat? progressStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.PROGRESS).FirstOrDefault();
                if (progressStat is null || progressStat.ValueString is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string actualProgress = progressStat.ValueString;


                // assert
                
                Assert.AreEqual(expectedStatsCount, actualStatsCount);
                Assert.AreEqual(expectedLanguageUserId, progressStat.LanguageUserKey);
                Assert.AreEqual(expectedProgress, actualProgress);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void BookUserStatsUpdateByBookUserIdTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            decimal expectedWordCount = 913.0M;
            decimal expectedDistinctKnowPercent = 0.11M;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguage(context), user);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); return; }
                // carry on with the test
                var bookUserStats = BookUserStatApi.BookUserStatsRead(context, bookId, (Guid)user.UniqueKey);
                // checking total word count here because we already tested progress
                // it's good to mix these up so we eventually get coverage
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT).FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = PageApi.PageReadFirstByBookId(context, bookId);
                if (firstPage is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, firstPage, user);
                if (pageUser is null)
                { ErrorHandler.LogAndThrow(); return; }

                // update all unknowns to well known
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser.UniqueKey);
                // mark the first page as read
                PageUserApi.PageUserMarkAsRead(context, (Guid)pageUser.UniqueKey);
                // now move forward to page 2
                var newPageUser = PageUserApi.PageUserReadByOrderWithinBook(
                    context, (Guid)bookUser.LanguageUserKey, 2, bookId);

                // now we need to regen the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(context, (Guid)bookUser.UniqueKey);
                // finally, re-pull the stats
                var newStats = BookUserStatApi.BookUserStatsRead(context, bookId, (Guid)user.UniqueKey);
                BookUserStat? distinctKnownPercentStat = newStats is null ? null :
                    newStats.Where(x => x.Key == AvailableBookUserStat.DISTINCTKNOWNPERCENT).FirstOrDefault();
                if (distinctKnownPercentStat is null || distinctKnownPercentStat.ValueNumeric is null)
                { ErrorHandler.LogAndThrow(); return; }
                decimal actualDistinctKnown = Math.Round((decimal)distinctKnownPercentStat.ValueNumeric, 2);

                // assert
                Assert.AreEqual(expectedWordCount, actualWordCount);
                Assert.AreEqual(expectedDistinctKnowPercent, actualDistinctKnown);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task BookUserStatsUpdateByBookUserIdAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            decimal expectedWordCount = 913.0M;
            decimal expectedDistinctKnowPercent = 0.11M;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguage(context), user);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); return; }
                // carry on with the test
                var bookUserStats = await BookUserStatApi.BookUserStatsReadAsync(context, bookId, (Guid)user.UniqueKey);
                // checking total word count here because we already tested progress
                // it's good to mix these up so we eventually get coverage
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT).FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = await PageApi.PageReadFirstByBookIdAsync(context, bookId);
                if (firstPage is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, firstPage, user);
                if (pageUser is null)
                    { ErrorHandler.LogAndThrow(); return; }


                // update all unknowns to well known
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser.UniqueKey);
                // mark the first page as read
                await PageUserApi.PageUserMarkAsReadAsync(context, (Guid)pageUser.UniqueKey);
                // now move forward to page 2
                var newPageUser = await PageUserApi.PageUserReadByOrderWithinBookAsync(
                    context, (Guid)bookUser.LanguageUserKey, 2, bookId);

                // now we need to regen the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(context, (Guid)bookUser.UniqueKey);
                // finally, re-pull the stats
                var newStats = await BookUserStatApi.BookUserStatsReadAsync(context, bookId, (Guid)user.UniqueKey);
                BookUserStat? distinctKnownPercentStat = newStats is null ? null :
                    newStats.Where(x => x.Key == AvailableBookUserStat.DISTINCTKNOWNPERCENT).FirstOrDefault();
                if (distinctKnownPercentStat is null || distinctKnownPercentStat.ValueNumeric is null)
                { ErrorHandler.LogAndThrow(); return; }
                decimal actualDistinctKnown = Math.Round((decimal)distinctKnownPercentStat.ValueNumeric, 2);

                // assert
                Assert.AreEqual(expectedWordCount, actualWordCount);
                Assert.AreEqual(expectedDistinctKnowPercent, actualDistinctKnown);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


    }
}