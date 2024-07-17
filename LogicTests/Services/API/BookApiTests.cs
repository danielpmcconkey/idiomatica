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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookApiTests
    {
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
    }
}