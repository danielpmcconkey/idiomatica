using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Logic.Telemetry;
using Model;
using Model.DAL;
using Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookStatApiTests
    {
        [TestMethod()]
        public void BookStatsCreateAndSaveTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            

            int expectedPageCount = 4;

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

                BookStatApi.BookStatsCreateAndSave(dbContextFactory, (Guid)bookId);

                int actualPageCount = BookApi.BookReadPageCount(dbContextFactory, (Guid)bookId);

                Assert.AreEqual(expectedPageCount, actualPageCount);
            }
            finally
            {
                // clean-up
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task BookStatsCreateAndSaveAsyncTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            int expectedPageCount = 4;

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

                await BookStatApi.BookStatsCreateAndSaveAsync(dbContextFactory, (Guid)bookId);

                int actualPageCount = await BookApi.BookReadPageCountAsync(dbContextFactory, (Guid)bookId);

                Assert.AreEqual(expectedPageCount, actualPageCount);
            }
            finally
            {
                // clean-up
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void BookStatsCreateAndSaveMakesDifficultyScoreTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            decimal expectedDifficulty = 13.2M;

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

                BookStatApi.BookStatsCreateAndSave(dbContextFactory, (Guid)bookId);
                var difficultyStat = DataCache.BookStatByBookIdAndStatKeyRead(
                    ((Guid)bookId, AvailableBookStat.DIFFICULTYSCORE), dbContextFactory);

                Assert.IsNotNull(difficultyStat);
                Assert.IsNotNull(difficultyStat.Value);
                decimal.TryParse(difficultyStat.Value, out decimal actualDifficulty);
                Assert.AreEqual(expectedDifficulty, actualDifficulty);
            }
            finally
            {
                // clean-up
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task BookStatsCreateAndSaveMakesDifficultyScoreAsyncTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            decimal expectedDifficulty = 13.2M;

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

                await BookStatApi.BookStatsCreateAndSaveAsync(dbContextFactory, (Guid)bookId);
                var difficultyStat = await DataCache.BookStatByBookIdAndStatKeyReadAsync(
                    ((Guid)bookId, AvailableBookStat.DIFFICULTYSCORE), dbContextFactory);

                Assert.IsNotNull(difficultyStat);
                Assert.IsNotNull(difficultyStat.Value);
                decimal.TryParse(difficultyStat.Value, out decimal actualDifficulty);                
                Assert.AreEqual(expectedDifficulty, actualDifficulty);
            }
            finally
            {
                // clean-up
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
    }
}