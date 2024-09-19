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
                    dbContextFactory,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText
                    );
                if (newBook is null) { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)newBook.Id;

                var newBookFromDb = BookApi.BookRead(dbContextFactory, (Guid)newBook.Id);
                var newPageFromDb = PageApi.PageReadFirstByBookId(dbContextFactory, (Guid)newBook.Id);

                // assert
                Assert.IsNotNull(newBookFromDb);
                Assert.IsNotNull(newPageFromDb);
                Assert.AreEqual(expectedTitle, newBookFromDb.Title);
            }
            finally
            {
                // clean-up
                if (bookId is not null) if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
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
                    dbContextFactory,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText
                    );
                if (newBook is null) { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)newBook.Id;

                var newBookFromDb = await BookApi.BookReadAsync(dbContextFactory, (Guid)newBook.Id);
                var newPageFromDb = await PageApi.PageReadFirstByBookIdAsync(dbContextFactory, (Guid)newBook.Id);

                Assert.IsNotNull(newBookFromDb);
                Assert.IsNotNull(newPageFromDb);
                Assert.AreEqual(expectedTitle, newBookFromDb.Title);
            }
            finally
            {
                // clean-up
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void BookListReadTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var originalPacket = new BookListDataPacket(dbContextFactory, isBrowse: true); // isbrowse will have the below search pull back all the books whether we have a bookUser or not 
            int expectedCount = originalPacket.BookListRowsToDisplay;
            Guid loggedInUserId = Guid.NewGuid();
            
            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = BookApi.BookListRead(dbContextFactory, loggedInUserId, originalPacket);

            Assert.AreEqual(0, originalRowCount);
            Assert.IsNotNull(newPacket.BookListRows);
            Assert.AreEqual(expectedCount, newPacket.BookListRows.Count);            
        }
        [TestMethod()]
        public async Task BookListReadAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var originalPacket = new BookListDataPacket(dbContextFactory, isBrowse: true); // isbrowse will have the below search pull back all the books whether we have a bookUser or not 
            int expectedCount = originalPacket.BookListRowsToDisplay;
            Guid loggedInUserId = Guid.NewGuid();

            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = await BookApi.BookListReadAsync(dbContextFactory, loggedInUserId, originalPacket);

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
            

            var originalPacket = new BookListDataPacket(dbContextFactory, false);
            originalPacket.OrderBy = (int)AvailableBookListSortProperties.DIFFICULTY;
            originalPacket.ShouldSortAscending = true;

            Guid loggedInUserId = CommonFunctions.GetTestUserId(dbContextFactory);

            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = await BookApi.BookListReadAsync(dbContextFactory, loggedInUserId, originalPacket);
            
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
            


            var originalPacket = new BookListDataPacket(dbContextFactory, false);
            originalPacket.OrderBy = (int)AvailableBookListSortProperties.DIFFICULTY;
            originalPacket.ShouldSortAscending = true;

            Guid loggedInUserId = CommonFunctions.GetTestUserId(dbContextFactory);

            int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
            var newPacket = BookApi.BookListRead(dbContextFactory, loggedInUserId, originalPacket);

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
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;
                
                // take the first row -- should be pretty empty
                var firstRow = BookApi.BookListRowByBookIdAndUserIdRead(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                Assert.IsNotNull(firstRow);
                Assert.IsNotNull(firstRow.Title);
                
                string actualTitle = firstRow.Title;

                // start reading it
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    dbContextFactory, loginService, (Guid)bookId);
                Assert.IsNotNull (readDataPacket);
                
                // clear page and move forward
                OrchestrationApi.OrchestrateClearPageAndMove(dbContextFactory, readDataPacket, 2);

                // refresh the stats
                BookUserStatApi.BookUserStatsUpdateByBookUserId(dbContextFactory, bookUserId);

                // grab the row again -- expect it to be new
                var secondRow = BookApi.BookListRowByBookIdAndUserIdRead(dbContextFactory, (Guid)bookId, (Guid)userId);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
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
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                // take the first row -- should be pretty empty
                var firstRow = await BookApi.BookListRowByBookIdAndUserIdReadAsync(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                Assert.IsNotNull(firstRow);
                Assert.IsNotNull(firstRow.Title);

                string actualTitle = firstRow.Title;

                // start reading it
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    dbContextFactory, loginService, (Guid)bookId);
                Assert.IsNotNull(readDataPacket);

                // clear page and move forward
                await OrchestrationApi.OrchestrateClearPageAndMoveAsync(dbContextFactory, readDataPacket, 2);

                // refresh the stats
                await BookUserStatApi.BookUserStatsUpdateByBookUserIdAsync(dbContextFactory, bookUserId);

                // grab the row again -- expect it to be new
                var secondRow = await BookApi.BookListRowByBookIdAndUserIdReadAsync(dbContextFactory, (Guid)bookId, (Guid)userId);
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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void BookReadTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var bookId = CommonFunctions.GetBook13Id(dbContextFactory);

            string expectedTitle = "Cenicienta";
            var book = BookApi.BookRead(dbContextFactory, bookId);

            Assert.IsNotNull(book);
            Assert.AreEqual(expectedTitle, book.Title);
        }
        [TestMethod()]
        public async Task BookReadAsyncTest()
        {
            
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var bookId = CommonFunctions.GetBook13Id(dbContextFactory);

            string expectedTitle = "Cenicienta";
            var book = await BookApi.BookReadAsync(dbContextFactory, bookId);

            Assert.IsNotNull(book);
            Assert.AreEqual(expectedTitle, book.Title);
            
        }


        [TestMethod()]
        public void BookReadPageCountTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            Guid bookId = CommonFunctions.GetBook17Id(dbContextFactory);
            int expectedPageCount = 5;
            int actualPageCount = BookApi.BookReadPageCount(dbContextFactory, bookId);
            Assert.AreEqual(expectedPageCount, actualPageCount);
        }

        [TestMethod()]
        public async Task BookReadPageCountAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            
            Guid bookId = CommonFunctions.GetBook17Id(dbContextFactory);
            int expectedPageCount = 5;
            int actualPageCount = await BookApi.BookReadPageCountAsync(dbContextFactory, bookId);
            Assert.AreEqual(expectedPageCount, actualPageCount);
        }
    }
}