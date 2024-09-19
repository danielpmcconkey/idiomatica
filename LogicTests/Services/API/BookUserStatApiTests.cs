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
using Microsoft.EntityFrameworkCore;
using Model.DAL;


namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookUserStatApiTests
    {
        [TestMethod()]
        public void BookUserStatsReadTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int expectedStatsCount = 8;
            string expectedProgress = "0 / 4";
            Guid languageId = learningLanguage.Id;


            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);

                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                // grab the expected language user ID
                var lu = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(
                    dbContextFactory, (Guid)userId);
                Assert.IsNotNull(lu);
                Assert.IsFalse(lu.Count < 1);
                var l = lu.Where(x => x.LanguageId == languageId).FirstOrDefault();
                Assert.IsNotNull(l);
                Guid expectedLanguageUserId = (Guid)l.Id;


                // carry on with the test
                var bookUserStats = BookUserStatApi.BookUserStatsRead(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                int actualStatsCount = bookUserStats is null ? 0 : bookUserStats.Count;
                BookUserStat? progressStat = bookUserStats is null ? null :
                    bookUserStats
                    .Where(x => x.Key == AvailableBookUserStat.PROGRESS).FirstOrDefault();
                Assert.IsNotNull(progressStat);
                Assert.IsNotNull(progressStat.ValueString);
                string actualProgress = progressStat.ValueString;



                Assert.AreEqual(expectedStatsCount, actualStatsCount);
                Assert.AreEqual(expectedLanguageUserId, progressStat.LanguageUserId);
                Assert.AreEqual(expectedProgress, actualProgress);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task BookUserStatsReadAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int expectedStatsCount = 8;
            string expectedProgress = "0 / 4";
            Guid languageId = learningLanguage.Id;


            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);

                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                // grab the expected language user ID
                var lu = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    dbContextFactory, (Guid)userId);
                Assert.IsNotNull(lu);
                Assert.IsFalse(lu.Count < 1);
                var l = lu.Where(x => x.LanguageId == languageId).FirstOrDefault();
                Assert.IsNotNull(l);
                Guid expectedLanguageUserId = (Guid)l.Id;


                // carry on with the test
                var bookUserStats = await BookUserStatApi.BookUserStatsReadAsync(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                int actualStatsCount = bookUserStats is null ? 0 : bookUserStats.Count;
                BookUserStat? progressStat = bookUserStats is null ? null :
                    bookUserStats
                    .Where(x => x.Key == AvailableBookUserStat.PROGRESS).FirstOrDefault();
                Assert.IsNotNull(progressStat);
                Assert.IsNotNull(progressStat.ValueString);
                string actualProgress = progressStat.ValueString;


                
                Assert.AreEqual(expectedStatsCount, actualStatsCount);
                Assert.AreEqual(expectedLanguageUserId, progressStat.LanguageUserId);
                Assert.AreEqual(expectedProgress, actualProgress);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void BookUserStatsUpdateByBookUserIdTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Guid bookId = CommonFunctions.GetBook17Id(dbContextFactory);
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            decimal expectedWordCount = 905.0M;
            decimal expectedDistinctKnowPercent = 0.11M;

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

               

                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    dbContextFactory, bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);

                // carry on with the test
                var bookUserStats = BookUserStatApi.BookUserStatsRead(
                    dbContextFactory, bookId, (Guid)user.Id);
                // checking total word count here because we already tested progress
                // it's good to mix these up so we eventually get coverage
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 :
                    (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = PageApi.PageReadFirstByBookId(dbContextFactory, bookId);
                Assert.IsNotNull(firstPage);
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, firstPage, user);
                Assert.IsNotNull(pageUser);


                // update all unknowns to well known
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(dbContextFactory, (Guid)pageUser.Id);
                // mark the first page as read
                PageUserApi.PageUserMarkAsRead(dbContextFactory, (Guid)pageUser.Id);
                // now move forward to page 2
                var newPageUser = PageUserApi.PageUserReadByOrderWithinBook(
                    dbContextFactory, (Guid)bookUser.LanguageUserId, 2, bookId);

                // now we need to regen the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(dbContextFactory, (Guid)bookUser.Id);
                // finally, re-pull the stats
                var newStats = BookUserStatApi.BookUserStatsRead(
                    dbContextFactory, bookId, (Guid)user.Id);
                BookUserStat? distinctKnownPercentStat = newStats is null ? null :
                    newStats.Where(x => x.Key == AvailableBookUserStat.DISTINCTKNOWNPERCENT)
                    .FirstOrDefault();
                Assert.IsNotNull(distinctKnownPercentStat);
                Assert.IsNotNull(distinctKnownPercentStat.ValueNumeric);

                decimal actualDistinctKnown = Math.Round(
                    (decimal)distinctKnownPercentStat.ValueNumeric, 2);

                // assert
                Assert.AreEqual(expectedWordCount, actualWordCount);
                Assert.AreEqual(expectedDistinctKnowPercent, actualDistinctKnown);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task BookUserStatsUpdateByBookUserIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Guid bookId = CommonFunctions.GetBook17Id(dbContextFactory);
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            decimal expectedWordCount = 905.0M;
            decimal expectedDistinctKnowPercent = 0.11M;

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                

                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    dbContextFactory, bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);

                // carry on with the test
                var bookUserStats = await BookUserStatApi.BookUserStatsReadAsync(
                    dbContextFactory, bookId, (Guid)user.Id);
                // checking total word count here because we already tested progress
                // it's good to mix these up so we eventually get coverage
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : 
                    (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = await PageApi.PageReadFirstByBookIdAsync(dbContextFactory, bookId);
                Assert.IsNotNull(firstPage);
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, firstPage, user);
                Assert.IsNotNull(pageUser);


                // update all unknowns to well known
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(dbContextFactory, (Guid)pageUser.Id);
                // mark the first page as read
                await PageUserApi.PageUserMarkAsReadAsync(dbContextFactory, (Guid)pageUser.Id);
                // now move forward to page 2
                var newPageUser = await PageUserApi.PageUserReadByOrderWithinBookAsync(
                    dbContextFactory, (Guid)bookUser.LanguageUserId, 2, bookId);

                // now we need to regen the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(dbContextFactory, (Guid)bookUser.Id);
                // finally, re-pull the stats
                var newStats = await BookUserStatApi.BookUserStatsReadAsync(
                    dbContextFactory, bookId, (Guid)user.Id);
                BookUserStat? distinctKnownPercentStat = newStats is null ? null :
                    newStats.Where(x => x.Key == AvailableBookUserStat.DISTINCTKNOWNPERCENT)
                    .FirstOrDefault();
                Assert.IsNotNull(distinctKnownPercentStat);
                Assert.IsNotNull(distinctKnownPercentStat.ValueNumeric);
                
                decimal actualDistinctKnown = Math.Round(
                    (decimal)distinctKnownPercentStat.ValueNumeric, 2);

                // assert
                Assert.AreEqual(expectedWordCount, actualWordCount);
                Assert.AreEqual(expectedDistinctKnowPercent, actualDistinctKnown);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


    }
}