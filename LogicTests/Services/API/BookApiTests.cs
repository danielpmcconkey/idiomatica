using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Model;
using System.Net;
using Logic.Telemetry;
using k8s.KubeConfigModels;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookApiTests
    {
        [TestMethod()]
        public void BookCreateAndSaveTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedTitle = TestConstants.NewBookTitle;

            try
            {
                var newBook = BookApi.BookCreateAndSave(
                    context,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText
                    );
                if (newBook is null || newBook.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)newBook.UniqueKey;

                var newBookFromDb = BookApi.BookRead(context, (Guid)newBook.UniqueKey);
                var newPageFromDb = PageApi.PageReadFirstByBookId(context, (Guid)newBook.UniqueKey);

                // assert
                Assert.IsNotNull(newBookFromDb);
                Assert.IsNotNull(newPageFromDb);
                Assert.AreEqual(expectedTitle, newBookFromDb.Title);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookCreateAndSaveAsyncTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedTitle = TestConstants.NewBookTitle;

            try
            {
                var newBook = await BookApi.BookCreateAndSaveAsync(
                    context,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText
                    );
                if (newBook is null || newBook.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)newBook.UniqueKey;

                var newBookFromDb = await BookApi.BookReadAsync(context, (Guid)newBook.UniqueKey);
                var newPageFromDb = await PageApi.PageReadFirstByBookIdAsync(context, (Guid)newBook.UniqueKey);

                Assert.IsNotNull(newBookFromDb);
                Assert.IsNotNull(newPageFromDb);
                Assert.AreEqual(expectedTitle, newBookFromDb.Title);
            }
            finally
            {
                // clean-up
                await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public async Task BookListReadAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            var originalPacket = new BookListDataPacket(context, true);
            int expectedCount = originalPacket.BookListRowsToDisplay;

            Guid loggedInUserId = Guid.NewGuid(); Assert.Fail();

            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = await BookApi.BookListReadAsync(context, loggedInUserId, originalPacket);

            // assert
            Assert.AreEqual(0, originalRowCount);
            Assert.IsNotNull(newPacket.BookListRows);
            Assert.AreEqual(expectedCount, newPacket.BookListRows.Count);
            
        }
        [TestMethod()]
        public void BookListReadTest()
        {
            var context = CommonFunctions.CreateContext();
            var originalPacket = new BookListDataPacket(context, true);
            int expectedCount = originalPacket.BookListRowsToDisplay;
            Guid loggedInUserId = Guid.NewGuid(); Assert.Fail();
            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = BookApi.BookListRead(context, loggedInUserId, originalPacket);

            Assert.AreEqual(0, originalRowCount);
            Assert.IsNotNull(newPacket.BookListRows);
            Assert.AreEqual(expectedCount, newPacket.BookListRows.Count);            
        }


        [TestMethod()]
        public async Task BookListReadSortByDifficultyAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            var originalPacket = new BookListDataPacket(context, false);
            originalPacket.OrderBy = (int)AvailableBookListSortProperties.DIFFICULTY;
            originalPacket.ShouldSortAscending = true;

            Guid loggedInUserId = Guid.NewGuid(); Assert.Fail();

            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = await BookApi.BookListReadAsync(context, loggedInUserId, originalPacket);
            
            Assert.IsNotNull(newPacket.BookListRows);
            Assert.IsTrue(newPacket.BookListRows.Count > 0);

            decimal lastDifficulty = 0.0M;
            for (int i = 0; i < newPacket.BookListRows.Count; i++)
            {
                var thisDifficulty = newPacket.BookListRows[i].DifficultyScore;
                Assert.IsNotNull(thisDifficulty);
                Assert.IsTrue(thisDifficulty >= lastDifficulty);
                lastDifficulty = (decimal)thisDifficulty;
            }
        }
        [TestMethod()]
        public void BookListReadSortByDifficultyTest()
        {
            var context = CommonFunctions.CreateContext();
            var originalPacket = new BookListDataPacket(context, false);
            originalPacket.OrderBy = (int)AvailableBookListSortProperties.DIFFICULTY;
            originalPacket.ShouldSortAscending = true;

            Guid loggedInUserId = Guid.NewGuid(); Assert.Fail();

            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = BookApi.BookListRead(context, loggedInUserId, originalPacket);

            Assert.IsNotNull(newPacket.BookListRows);
            Assert.IsTrue(newPacket.BookListRows.Count > 0);

            decimal lastDifficulty = 0.0M;
            for (int i = 0; i < newPacket.BookListRows.Count; i++)
            {
                var thisDifficulty = newPacket.BookListRows[i].DifficultyScore;
                Assert.IsNotNull(thisDifficulty);
                Assert.IsTrue(thisDifficulty >= lastDifficulty);
                lastDifficulty = (decimal)thisDifficulty;
            }
        }


        [TestMethod()]
        public void BookListRowByBookIdAndUserIdReadTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedTitle = TestConstants.NewBookTitle;
            string secondProgressExpected = "1 / 3";


            try
            {
                var userService = CommonFunctions.CreateUserService();
                if(userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;
                
                // take the first row -- should be pretty empty
                var firstRow = BookApi.BookListRowByBookIdAndUserIdRead(context, bookId, userId);
                if (firstRow is null || firstRow.Title is null) { ErrorHandler.LogAndThrow(); return; }

                string actualTitle = firstRow.Title;

                // start reading it
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null) { ErrorHandler.LogAndThrow(); return; }
                
                // clear page and move forward
                OrchestrationApi.OrchestrateClearPageAndMove(context, readDataPacket, 2);

                // refresh the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(context, bookUserId);

                // grab the row again -- expect it to be new
                var secondRow = BookApi.BookListRowByBookIdAndUserIdRead(context, bookId, userId);
                if (secondRow is null || secondRow.Progress is null)
                { ErrorHandler.LogAndThrow(); return; }
                
                string secondProgressActual = secondRow.Progress;

                // assert
                Assert.AreEqual(expectedTitle, actualTitle);
                Assert.AreEqual(secondProgressExpected, secondProgressActual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookListRowByBookIdAndUserIdReadAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedTitle = TestConstants.NewBookTitle;
            string secondProgressExpected = "1 / 3";


            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;
                
                // take the first row -- should be pretty empty

                var firstRow = await BookApi.BookListRowByBookIdAndUserIdReadAsync(
                    context, bookId, userId);
                if (firstRow is null || firstRow.Title is null)
                { ErrorHandler.LogAndThrow(); return; }
                string actualTitle = firstRow.Title;

                // start reading it
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    context, userService, bookId);
                if (readDataPacket is null)
                { ErrorHandler.LogAndThrow(); return; }
                // clear page and move forward
                await OrchestrationApi.OrchestrateClearPageAndMoveAsync(context, readDataPacket, 2);

                // refresh the stats
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(context, bookUserId);

                // grab the row again -- expect it to be new
                var secondRow = await BookApi.BookListRowByBookIdAndUserIdReadAsync(
                    context, bookId, userId);
                if (secondRow is null || secondRow.Progress is null)
                { ErrorHandler.LogAndThrow(); return; }
                string secondProgressActual = secondRow.Progress;

                // assert
                Assert.AreEqual(expectedTitle, actualTitle);
                Assert.AreEqual(secondProgressExpected, secondProgressActual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void BookReadTest()
        {
            var context = CommonFunctions.CreateContext();
            //int bookId = 13;
            Guid bookId = Guid.NewGuid(); Assert.Fail();
            string expectedTitle = "Cenicienta";
            var book = BookApi.BookRead(context, bookId);
            
            Assert.IsNotNull(book);
            Assert.AreEqual(expectedTitle, book.Title);
            
        }
        [TestMethod()]
        public async Task BookReadAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            //int bookId = 13;
            Guid bookId = Guid.NewGuid(); Assert.Fail();
            string expectedTitle = "Cenicienta";
            var book = await BookApi.BookReadAsync(context, bookId);

            Assert.IsNotNull(book);
            Assert.AreEqual(expectedTitle, book.Title);
            
        }


        [TestMethod()]
        public void BookReadPageCountTest()
        {
            var context = CommonFunctions.CreateContext();
            //int bookId = 17;
            Guid bookId = Guid.NewGuid(); Assert.Fail();
            int expectedPageCount = 5;
            int actualPageCount = BookApi.BookReadPageCount(context, bookId);
            Assert.AreEqual(expectedPageCount, actualPageCount);
        }

        [TestMethod()]
        public async Task BookReadPageCountAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            //int bookId = 17;
            Guid bookId = Guid.NewGuid(); Assert.Fail();
            int expectedPageCount = 5;
            int actualPageCount = await BookApi.BookReadPageCountAsync(context, bookId);
            Assert.AreEqual(expectedPageCount, actualPageCount);
        }
    }
}