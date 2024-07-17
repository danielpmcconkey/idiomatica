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


namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookUserStatApiTests
    {
        [TestMethod()]
        public async Task BookUserStatsReadAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            if (user is null || user.Id is null || user.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            var languageUser = LanguageUserApi.LanguageUserCreateAsync(context, 1, (int)user.Id);
            int bookId = 6;
            int expectedStatsCount = 8;
            string expectedProgress = "0 / 13";
            var originalPacket = new BookListDataPacket(context, false);

            try
            {
                // act
                // create the base bookUser
                var bookUser = await BookUserApi.BookUserCreateAsync(context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.LanguageUserId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, (int)user.Id);
                // update bookUserStats
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(context, (int)bookUser.Id);
                // carry on with the test
                var bookUserStats = await BookUserStatApi.BookUserStatsReadAsync(context, bookId, (int)user.Id);
                int actualStatsCount = bookUserStats is null ? 0 : bookUserStats.Count;
                BookUserStat? progressStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.PROGRESS).FirstOrDefault();
                if(progressStat is null || progressStat.LanguageUserId is null || progressStat.ValueString is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string actualProgress = progressStat.ValueString;


                // assert
                Assert.IsNotNull(bookUser);
                Assert.AreEqual(expectedStatsCount, actualStatsCount);
                Assert.AreEqual(bookUser.LanguageUserId, progressStat.LanguageUserId);
                Assert.AreEqual(expectedProgress, actualProgress);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public async Task BookUserStatsUpdateByBookUserIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreateAsync(context, 1, (int)user.Id);
                int bookId = 6;
                decimal expectedWordCount = 3046.0M;
                decimal expectedDistinctKnowPercent = 0.18M;
                
                
                /*
                 * 
                 * 
                 * you are here. the line below is failing citing that there 
                 * already is an open reader that needs to be closed first
                 * 
                 * 
                 * 
                 * 
                 * */
                
                
                
                var originalPacket = new BookListDataPacket(context, false);

                // act
                var bookUser = await BookUserApi.BookUserCreateAsync(context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.LanguageUserId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, (int)user.Id);
                // update bookUserStats
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(context, (int)bookUser.Id);
                // carry on with the test
                var bookUserStats = await BookUserStatApi.BookUserStatsReadAsync(context, bookId, (int)user.Id);
                // checking total word count here because we already tested progress
                // it's good to mix these up so we eventually get coverage
                BookUserStat? totalWordCountStat = bookUserStats is null ? null :
                    bookUserStats.Where(x => x.Key == AvailableBookUserStat.TOTALWORDCOUNT).FirstOrDefault();
                decimal actualWordCount = totalWordCountStat is null ? 0 :
                    totalWordCountStat.ValueNumeric is null ? 0 : (decimal)totalWordCountStat.ValueNumeric;

                // now move the page forward a couple of times as if we're reading it
                var firstPage = await PageApi.PageReadFirstByBookIdAsync(context, bookId);
                if (firstPage is null || firstPage.Id is null || firstPage.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var pageUser = await PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPage.Id, (int)user.Id);
                if (pageUser is null || pageUser.Id is null || pageUser.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                

                // update all unknowns to well known
                await PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser.Id);
                // mark the first page as read
                await PageUserApi.PageUserMarkAsReadAsync(context, (int)pageUser.Id);
                // now move forward to page 2
                var newPageUser = await PageUserApi.PageUserReadByOrderWithinBookAsync(
                    context, (int)bookUser.LanguageUserId, 2, bookId);

                // now we need to regen the stats
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(context, (int)bookUser.Id);
                // finally, re-pull the stats
                var newStats = await BookUserStatApi.BookUserStatsReadAsync(context, bookId, (int)user.Id);
                BookUserStat? distinctKnownPercentStat = newStats is null ? null :
                    newStats.Where(x => x.Key == AvailableBookUserStat.DISTINCTKNOWNPERCENT).FirstOrDefault();
                if(distinctKnownPercentStat is null || distinctKnownPercentStat.ValueNumeric is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                decimal actualDistinctKnown = Math.Round((decimal)distinctKnownPercentStat.ValueNumeric, 2);
                
                // assert
                Assert.AreEqual(expectedWordCount, actualWordCount);
                Assert.AreEqual(expectedDistinctKnowPercent, actualDistinctKnown);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }
    }
}