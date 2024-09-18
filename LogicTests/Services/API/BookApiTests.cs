using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using System.Net;
using Logic.Telemetry;
using Model;
using Model.Enums;
using Microsoft.EntityFrameworkCore;
using Model.DAL;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookApiTests
    {
        [TestMethod()]
        public void BookCreateAndSaveTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            

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
                if (newBook is null) { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)newBook.Id;

                var newBookFromDb = BookApi.BookRead(context, (Guid)newBook.Id);
                var newPageFromDb = PageApi.PageReadFirstByBookId(context, (Guid)newBook.Id);

                // assert
                Assert.IsNotNull(newBookFromDb);
                Assert.IsNotNull(newPageFromDb);
                Assert.AreEqual(expectedTitle, newBookFromDb.Title);
            }
            finally
            {
                // clean-up
                if (bookId is not null) if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookCreateAndSaveAsyncTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
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
                if (newBook is null) { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)newBook.Id;

                var newBookFromDb = await BookApi.BookReadAsync(context, (Guid)newBook.Id);
                var newPageFromDb = await PageApi.PageReadFirstByBookIdAsync(context, (Guid)newBook.Id);

                Assert.IsNotNull(newBookFromDb);
                Assert.IsNotNull(newPageFromDb);
                Assert.AreEqual(expectedTitle, newBookFromDb.Title);
            }
            finally
            {
                // clean-up
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void BookListReadTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var originalPacket = new BookListDataPacket(context, true);
            int expectedCount = originalPacket.BookListRowsToDisplay;
            Guid loggedInUserId = Guid.NewGuid(); // this shouldn't work
            throw new NotImplementedException("figure out why this worked with a new guid. Is this returning all books regardless?");

            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = BookApi.BookListRead(context, loggedInUserId, originalPacket);

            Assert.AreEqual(0, originalRowCount);
            Assert.IsNotNull(newPacket.BookListRows);
            Assert.AreEqual(expectedCount, newPacket.BookListRows.Count);            
        }
        [TestMethod()]
        public async Task BookListReadAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            var originalPacket = new BookListDataPacket(context, true);
            int expectedCount = originalPacket.BookListRowsToDisplay;

            Guid loggedInUserId = CommonFunctions.GetTestUserId(context);

            throw new NotImplementedException("figure out why this worked with a new guid. Is this returning all books regardless?");

            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = await BookApi.BookListReadAsync(context, loggedInUserId, originalPacket);

            // assert
            Assert.AreEqual(0, originalRowCount);
            Assert.IsNotNull(newPacket.BookListRows);
            Assert.AreEqual(expectedCount, newPacket.BookListRows.Count);
            
        }


        [TestMethod()]
        public async Task BookListReadSortByDifficultyAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            

            var originalPacket = new BookListDataPacket(context, false);
            originalPacket.OrderBy = (int)AvailableBookListSortProperties.DIFFICULTY;
            originalPacket.ShouldSortAscending = true;

            Guid loggedInUserId = CommonFunctions.GetTestUserId(context);

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
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            


            var originalPacket = new BookListDataPacket(context, false);
            originalPacket.OrderBy = (int)AvailableBookListSortProperties.DIFFICULTY;
            originalPacket.ShouldSortAscending = true;

            Guid loggedInUserId = CommonFunctions.GetTestUserId(context);

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
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            


            string expectedTitle = TestConstants.NewBookTitle;
            string secondProgressExpected = "1 / 4";


            try
            {
                if(loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;
                
                // take the first row -- should be pretty empty
                var firstRow = BookApi.BookListRowByBookIdAndUserIdRead(
                    context, (Guid)bookId, (Guid)userId);
                Assert.IsNotNull(firstRow);
                Assert.IsNotNull(firstRow.Title);
                
                string actualTitle = firstRow.Title;

                // start reading it
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    context, loginService, (Guid)bookId);
                Assert.IsNotNull (readDataPacket);
                
                // clear page and move forward
                OrchestrationApi.OrchestrateClearPageAndMove(context, readDataPacket, 2);

                // refresh the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(context, bookUserId);

                // grab the row again -- expect it to be new
                var secondRow = BookApi.BookListRowByBookIdAndUserIdRead(context, (Guid)bookId, (Guid)userId);
                Assert.IsNotNull(secondRow);
                Assert.IsNotNull(secondRow.Progress);
                
                string secondProgressActual = secondRow.Progress;

                // assert
                Assert.AreEqual(expectedTitle, actualTitle);
                Assert.AreEqual(secondProgressExpected, secondProgressActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookListRowByBookIdAndUserIdReadAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();



            string expectedTitle = TestConstants.NewBookTitle;
            string secondProgressExpected = "1 / 4";


            try
            {
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                // take the first row -- should be pretty empty
                var firstRow = await BookApi.BookListRowByBookIdAndUserIdReadAsync(
                    context, (Guid)bookId, (Guid)userId);
                Assert.IsNotNull(firstRow);
                Assert.IsNotNull(firstRow.Title);

                string actualTitle = firstRow.Title;

                // start reading it
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    context, loginService, (Guid)bookId);
                Assert.IsNotNull(readDataPacket);

                // clear page and move forward
                await OrchestrationApi.OrchestrateClearPageAndMoveAsync(context, readDataPacket, 2);

                // refresh the stats
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(context, bookUserId);

                // grab the row again -- expect it to be new
                var secondRow = await BookApi.BookListRowByBookIdAndUserIdReadAsync(context, (Guid)bookId, (Guid)userId);
                Assert.IsNotNull(secondRow);
                Assert.IsNotNull(secondRow.Progress);

                string secondProgressActual = secondRow.Progress;

                // assert
                Assert.AreEqual(expectedTitle, actualTitle);
                Assert.AreEqual(secondProgressExpected, secondProgressActual);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void BookReadTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var bookId = CommonFunctions.GetBook13Id(context);

            string expectedTitle = "Cenicienta";
            var book = BookApi.BookRead(context, bookId);

            Assert.IsNotNull(book);
            Assert.AreEqual(expectedTitle, book.Title);
        }
        [TestMethod()]
        public async Task BookReadAsyncTest()
        {
            
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var bookId = CommonFunctions.GetBook13Id(context);

            string expectedTitle = "Cenicienta";
            var book = await BookApi.BookReadAsync(context, bookId);

            Assert.IsNotNull(book);
            Assert.AreEqual(expectedTitle, book.Title);
            
        }


        [TestMethod()]
        public void BookReadPageCountTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            Guid bookId = CommonFunctions.GetBook17Id(context);
            int expectedPageCount = 5;
            int actualPageCount = BookApi.BookReadPageCount(context, bookId);
            Assert.AreEqual(expectedPageCount, actualPageCount);
        }

        [TestMethod()]
        public async Task BookReadPageCountAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int expectedPageCount = 5;
            int actualPageCount = await BookApi.BookReadPageCountAsync(context, bookId);
            Assert.AreEqual(expectedPageCount, actualPageCount);
        }
    }
}