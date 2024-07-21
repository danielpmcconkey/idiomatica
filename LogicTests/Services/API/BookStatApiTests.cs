using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookStatApiTests
    {
        [TestMethod()]
        public async Task BookStatsCreateAndSaveAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int expectedPageCount = 3;

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

                await BookStatApi.BookStatsCreateAndSaveAsync(context, newId);

                // assert
                int actualPageCount = await BookApi.BookGetPageCountAsync(context, newId);
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
        public void BookStatsCreateAndSaveTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int expectedPageCount = 3;

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

                BookStatApi.BookStatsCreateAndSave(context, newId);

                // assert
                int actualPageCount = BookApi.BookGetPageCount(context, newId);
                // assert
                Assert.AreEqual(expectedPageCount, actualPageCount);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
    }
}