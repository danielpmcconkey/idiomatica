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
            int bookId = 0;
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
                if (newBook is null || newBook.Id is null) { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)newBook.Id;

                var newBookFromDb = BookApi.BookRead(context, (int)newBook.Id);
                var newPageFromDb = PageApi.PageReadFirstByBookId(context, (int)newBook.Id);

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
            int bookId = 0;
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
                if (newBook is null || newBook.Id is null) { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)newBook.Id;

                var newBookFromDb = await BookApi.BookReadAsync(context, (int)newBook.Id);
                var newPageFromDb = await PageApi.PageReadFirstByBookIdAsync(context, (int)newBook.Id);

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
        public async Task BookListReadAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            var originalPacket = new BookListDataPacket(context, false);
            int expectedCount = originalPacket.BookListRowsToDisplay;

            int loggedInUserId = 1;

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
            var originalPacket = new BookListDataPacket(context, false);
            int expectedCount = originalPacket.BookListRowsToDisplay;
            int loggedInUserId = 1;
            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = BookApi.BookListRead(context, loggedInUserId, originalPacket);

            Assert.AreEqual(0, originalRowCount);
            Assert.IsNotNull(newPacket.BookListRows);
            Assert.AreEqual(expectedCount, newPacket.BookListRows.Count);            
        }


        [TestMethod()]
        public void BookListRowByBookIdAndUserIdReadTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string expectedTitle = TestConstants.NewBookTitle;
            string secondProgressExpected = "1 / 3";


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;
                
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
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string expectedTitle = TestConstants.NewBookTitle;
            string secondProgressExpected = "1 / 3";


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;
                
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
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void BookReadTest()
        {
            var context = CommonFunctions.CreateContext();
            int bookId = 6;
            string expectedTitle = "Tierras desconocidas";
            var book = BookApi.BookRead(context, bookId);
            
            Assert.IsNotNull(book);
            Assert.AreEqual(expectedTitle, book.Title);
            
        }
        [TestMethod()]
        public async Task BookReadAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            int bookId = 6;
            string expectedTitle = "Tierras desconocidas";
            var book = await BookApi.BookReadAsync(context, bookId);

            Assert.IsNotNull(book);
            Assert.AreEqual(expectedTitle, book.Title);
            
        }


        [TestMethod()]
        public void BookReadPageCountTest()
        {
            var context = CommonFunctions.CreateContext();
            int bookId = 6;
            int expectedPageCount = 13;
            int actualPageCount = BookApi.BookReadPageCount(context, bookId);
            Assert.AreEqual(expectedPageCount, actualPageCount);
        }

        [TestMethod()]
        public async Task BookReadPageCountAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            int bookId = 6;
            int expectedPageCount = 13;
            int actualPageCount = await BookApi.BookReadPageCountAsync(context, bookId);
            Assert.AreEqual(expectedPageCount, actualPageCount);
        }
    }
}