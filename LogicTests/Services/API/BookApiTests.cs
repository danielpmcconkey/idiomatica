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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookApiTests
    {
        [TestMethod()]
        public void BookCreateAndSaveTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            string expectedTitle = TestConstants.NewBookTitle;

            try
            {
                // act

                var newBook = BookApi.BookCreateAndSave(
                    context,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText
                    );
                int newId = (newBook is not null) ? (newBook.Id is not null) ? (int)newBook.Id : 0 : 0;

                var newBookFromDb = BookApi.BookGet(context, newId);
                var newPageFromDb = PageApi.PageReadFirstByBookId(context, newId);

                // assert
                Assert.IsNotNull(newBookFromDb);
                Assert.IsNotNull(newPageFromDb);
                Assert.AreEqual(expectedTitle, newBookFromDb.Title);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task BookCreateAndSaveAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            string expectedTitle = TestConstants.NewBookTitle;

            try
            {
                // act

                var newBook = await BookApi.BookCreateAndSaveAsync(
                    context,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText
                    );
                int newId = (newBook is not null) ? (newBook.Id is not null) ? (int)newBook.Id : 0 : 0;

                var newBookFromDb = await BookApi.BookGetAsync(context, newId);
                var newPageFromDb = await PageApi.PageReadFirstByBookIdAsync(context, newId);

                // assert
                Assert.IsNotNull(newBookFromDb);
                Assert.IsNotNull(newPageFromDb);
                Assert.AreEqual(expectedTitle, newBookFromDb.Title);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public void BookGetTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 6;
            string expectedTitle = "Tierras desconocidas";
            try
            {
                // act
                var book = BookApi.BookGet(context, bookId);
                // assert
                Assert.IsNotNull(book);
                Assert.AreEqual(expectedTitle, book.Title);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task BookGetAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int bookId = 6;
            string expectedTitle = "Tierras desconocidas";
            try
            {
                // act
                var book = await BookApi.BookGetAsync(context, bookId);
                // assert
                Assert.IsNotNull(book);
                Assert.AreEqual(expectedTitle, book.Title);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public void BookGetPageCountTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 6;
            int expectedPageCount = 13;
            try
            {
                // act
                int actualPageCount = BookApi.BookGetPageCount(context, bookId);
                // assert
                Assert.AreEqual(expectedPageCount, actualPageCount);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public async Task BookGetPageCountAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int bookId = 6;
            int expectedPageCount = 13;
            try
            {
                // act
                int actualPageCount = await BookApi.BookGetPageCountAsync(context, bookId);
                // assert
                Assert.AreEqual(expectedPageCount, actualPageCount);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public async Task BookListReadAsyncTest()
        {

            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var originalPacket = new BookListDataPacket(context, false);
            int expectedCount = originalPacket.BookListRowsToDisplay;

            int loggedInUserId = 1;
            try
            {
                // act

                int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
                var newPacket = await BookApi.BookListReadAsync(context, loggedInUserId, originalPacket);

                // assert
                Assert.AreEqual(0, originalRowCount);
                Assert.IsNotNull(newPacket.BookListRows);
                Assert.AreEqual(expectedCount, newPacket.BookListRows.Count);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }
        [TestMethod()]
        public void BookListReadTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            var originalPacket = new BookListDataPacket(context, false);
            int expectedCount = originalPacket.BookListRowsToDisplay;

            int loggedInUserId = 1;
            try
            {
                // act

                int originalRowCount = originalPacket.BookListRows is null ? 0 : originalPacket.BookListRows.Count;
                var newPacket = BookApi.BookListRead(context, loggedInUserId, originalPacket);

                // assert
                Assert.AreEqual(0, originalRowCount);
                Assert.IsNotNull(newPacket.BookListRows);
                Assert.AreEqual(expectedCount, newPacket.BookListRows.Count);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

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
                Book? book = BookApi.OrchestrateBookCreationAndSubProcesses(
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
                Book? book = await BookApi.OrchestrateBookCreationAndSubProcessesAsync(
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
        public void BookListRowByBookIdAndUserIdReadTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void BookListRowByBookIdAndUserIdReadAsyncTest()
        {
            Assert.Fail();
        }
    }
}