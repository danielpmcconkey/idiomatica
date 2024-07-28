using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using LogicTests;
using Model;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class OrchestrationApiTests
    {
        [TestMethod()]
        public void OrchestrateBookCreationAndSubProcessesTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            /*
             * this is supposed to create the book, the book stats, and the 
             * bookUser, all in one call
             * */

            string totalPagesExpected = "3";
            string totalWordCountExpected = "784";
            string distinctWordCountExpected = "241";



            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);

                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // act
                Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // read the book stats
                var totalPagesStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.TOTALPAGES)
                    .FirstOrDefault();
                if (totalPagesStat is null || totalPagesStat.Value is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string totalPagesActual = totalPagesStat.Value;

                var totalWordsStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                if (totalWordsStat is null || totalWordsStat.Value is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string totalWordCountActual = totalWordsStat.Value;

                var distinctWordsStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.DISTINCTWORDCOUNT)
                    .FirstOrDefault();
                if (distinctWordsStat is null || distinctWordsStat.Value is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string distinctWordCountActual = distinctWordsStat.Value;

                // pull the book user
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(context, (int)book.Id, (int)user.Id);

                // assert
                Assert.IsNotNull(book.BookStats);
                Assert.AreEqual(totalPagesExpected, totalPagesActual);
                Assert.AreEqual(totalWordCountExpected, totalWordCountActual);
                Assert.AreEqual(distinctWordCountExpected, distinctWordCountActual);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task OrchestrateBookCreationAndSubProcessesAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            /*
             * this is supposed to create the book, the book stats, and the 
             * bookUser, all in one call
             * */

            string totalPagesExpected = "3";
            string totalWordCountExpected = "784";
            string distinctWordCountExpected = "241";



            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);

                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // act
                Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // read the book stats
                var totalPagesStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.TOTALPAGES)
                    .FirstOrDefault();
                if (totalPagesStat is null || totalPagesStat.Value is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string totalPagesActual = totalPagesStat.Value;

                var totalWordsStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                if (totalWordsStat is null || totalWordsStat.Value is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string totalWordCountActual = totalWordsStat.Value;

                var distinctWordsStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.DISTINCTWORDCOUNT)
                    .FirstOrDefault();
                if (distinctWordsStat is null || distinctWordsStat.Value is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string distinctWordCountActual = distinctWordsStat.Value;

                // pull the book user
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(context, (int)book.Id, (int)user.Id);

                // assert
                Assert.IsNotNull(book.BookStats);
                Assert.AreEqual(totalPagesExpected, totalPagesActual);
                Assert.AreEqual(totalWordCountExpected, totalWordCountActual);
                Assert.AreEqual(distinctWordCountExpected, distinctWordCountActual);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        
        [TestMethod()]
        public void OrchestrateClearPageAndMoveTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 134;
            int wordUsersCount = 134;
            int paragraphCount = 29;
            string wordToLookUp = "conocido";
            AvailableWordUserStatus statusBeforeExpected = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfterExpected = AvailableWordUserStatus.WELLKNOWN;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.Id is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.Id is null || readDataPacket.CurrentPageUser.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // check the word and its status
                WordUser? foundUserBefore = null;
                if (!readDataPacket.AllWordUsersInPage.TryGetValue(wordToLookUp, out foundUserBefore))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                if (foundUserBefore is null || foundUserBefore.Status is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var statusBeforeActual = foundUserBefore.Status;

                // pull the current page ID before moving so you can later look up its wordUsers

                int origPageId = (int)readDataPacket.CurrentPageUser.PageId;

                // clear the unknown words and move to the new page
                var newReadDataPacket = OrchestrationApi.OrchestrateClearPageAndMove(context, readDataPacket, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageId is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.AllWordUsersInPage is null || newReadDataPacket.Paragraphs is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // pull the previous page's wordUser list
                var priorPageWordDict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(context, origPageId, (int)user.Id);
                if (priorPageWordDict is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                WordUser? foundUserAfter = null;
                if (!priorPageWordDict.TryGetValue(wordToLookUp, out foundUserAfter))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                if (foundUserAfter is null || foundUserAfter.Status is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var statusAfterActual = foundUserAfter.Status;


                // assert
                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, newReadDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
                Assert.AreEqual(statusBeforeExpected, statusBeforeActual);
                Assert.AreEqual(statusAfterExpected, statusAfterActual);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task OrchestrateClearPageAndMoveAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 134;
            int wordUsersCount = 134;
            int paragraphCount = 29;
            string wordToLookUp = "conocido";
            AvailableWordUserStatus statusBeforeExpected = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfterExpected = AvailableWordUserStatus.WELLKNOWN;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.Id is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.Id is null || readDataPacket.CurrentPageUser.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // check the word and its status
                WordUser? foundUserBefore = null;
                if (!readDataPacket.AllWordUsersInPage.TryGetValue(wordToLookUp, out foundUserBefore))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                if (foundUserBefore is null || foundUserBefore.Status is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var statusBeforeActual = foundUserBefore.Status;

                // pull the current page ID before moving so you can later look up its wordUsers

                int origPageId = (int)readDataPacket.CurrentPageUser.PageId;

                // clear the unknown words and move to the new page
                var newReadDataPacket = await OrchestrationApi.OrchestrateClearPageAndMoveAsync(context, readDataPacket, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageId is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.AllWordUsersInPage is null || newReadDataPacket.Paragraphs is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // pull the previous page's wordUser list
                var priorPageWordDict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(context, origPageId, (int)user.Id);
                if (priorPageWordDict is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                WordUser? foundUserAfter = null;
                if (!priorPageWordDict.TryGetValue(wordToLookUp, out foundUserAfter))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                if (foundUserAfter is null || foundUserAfter.Status is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var statusAfterActual = foundUserAfter.Status;


                // assert
                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, newReadDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
                Assert.AreEqual(statusBeforeExpected, statusBeforeActual);
                Assert.AreEqual(statusAfterExpected, statusAfterActual);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void OrchestrateMovePageTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 134;
            int wordUsersCount = 134;
            int paragraphCount = 29;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.Id is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }



                //finally, we get to reset the data
                var newReadDataPacket = OrchestrationApi.OrchestrateMovePage(context, readDataPacket, bookId, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageId is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.AllWordUsersInPage is null || newReadDataPacket.Paragraphs is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // assert
                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, newReadDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task OrchestrateMovePageAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 134;
            int wordUsersCount = 134;
            int paragraphCount = 29;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.Id is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }



                //finally, we get to reset the data
                var newReadDataPacket = await OrchestrationApi.OrchestrateMovePageAsync(context, readDataPacket, bookId, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageId is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.AllWordUsersInPage is null || newReadDataPacket.Paragraphs is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // assert
                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, newReadDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void OrchestrateReadDataInitTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 135;
            int wordUsersCount = 135;
            int paragraphCount = 13;
            int bookUserStatsCount = 8;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.BookUserStats is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // assert
                Assert.IsTrue(readDataPacket.BookCurrentPageNum == 1);
                Assert.IsNotNull(readDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, readDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, readDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, readDataPacket.Paragraphs.Count);
                Assert.AreEqual(bookUserStatsCount, readDataPacket.BookUserStats.Count);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task OrchestrateReadDataInitAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 135;
            int wordUsersCount = 135;
            int paragraphCount = 13;
            int bookUserStatsCount = 8;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.BookUserStats is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // assert
                Assert.IsTrue(readDataPacket.BookCurrentPageNum == 1);
                Assert.IsNotNull(readDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, readDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, readDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, readDataPacket.Paragraphs.Count);
                Assert.AreEqual(bookUserStatsCount, readDataPacket.BookUserStats.Count);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void OrchestrateResetReadDataForNewPageTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 134;
            int wordUsersCount = 134;
            int paragraphCount = 29;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.Id is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // now pretend to move the page forward and reset
                int targetPageNum = 2;

                PageUserApi.PageUserMarkAsRead(context, (int)readDataPacket.CurrentPageUser.Id);

                // create the new page user

                // but first need to pull the page
                readDataPacket.CurrentPage = PageApi.PageReadByOrdinalAndBookId(context, targetPageNum, bookId);
                if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.Id is null ||
                    readDataPacket.CurrentPage.Id < 1 || readDataPacket.LoggedInUser is null ||
                    readDataPacket.LoggedInUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                readDataPacket.CurrentPageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)readDataPacket.CurrentPage.Id, (int)readDataPacket.LoggedInUser.Id);

                if (readDataPacket.CurrentPageUser is null || readDataPacket.CurrentPageUser.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                //finally, we get to reset the data
                var newReadDataPacket = OrchestrationApi.OrchestrateResetReadDataForNewPage(
                    context, readDataPacket, (int)readDataPacket.CurrentPageUser.PageId);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageId is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.AllWordUsersInPage is null || newReadDataPacket.Paragraphs is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // assert
                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, newReadDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task OrchestrateResetReadDataForNewPageAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync
                ();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 134;
            int wordUsersCount = 134;
            int paragraphCount = 29;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.Id is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // now pretend to move the page forward and reset
                int targetPageNum = 2;

                await PageUserApi.PageUserMarkAsReadAsync(context, (int)readDataPacket.CurrentPageUser.Id);

                // create the new page user

                // but first need to pull the page
                readDataPacket.CurrentPage = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, targetPageNum, bookId);
                if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.Id is null ||
                    readDataPacket.CurrentPage.Id < 1 || readDataPacket.LoggedInUser is null ||
                    readDataPacket.LoggedInUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                readDataPacket.CurrentPageUser = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, (int)readDataPacket.CurrentPage.Id, (int)readDataPacket.LoggedInUser.Id);

                if (readDataPacket.CurrentPageUser is null || readDataPacket.CurrentPageUser.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                //finally, we get to reset the data
                var newReadDataPacket = await OrchestrationApi.OrchestrateResetReadDataForNewPageAsync(
                    context, readDataPacket, (int)readDataPacket.CurrentPageUser.PageId);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageId is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.AllWordUsersInPage is null || newReadDataPacket.Paragraphs is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // assert
                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, newReadDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }
    }
}