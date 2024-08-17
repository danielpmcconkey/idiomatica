﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Logic.Telemetry;
using Model.DAL;
using Model;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookStatApiTests
    {
        [TestMethod()]
        public void BookStatsCreateAndSaveTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int expectedPageCount = 3;

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

                BookStatApi.BookStatsCreateAndSave(context, bookId);

                int actualPageCount = BookApi.BookReadPageCount(context, bookId);

                Assert.AreEqual(expectedPageCount, actualPageCount);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookStatsCreateAndSaveAsyncTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int expectedPageCount = 3;

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

                await BookStatApi.BookStatsCreateAndSaveAsync(context, bookId);

                int actualPageCount = await BookApi.BookReadPageCountAsync(context, bookId);

                Assert.AreEqual(expectedPageCount, actualPageCount);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void BookStatsCreateAndSaveMakesDifficultyScoreTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            decimal expectedDifficulty = 13.23M;

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

                BookStatApi.BookStatsCreateAndSave(context, bookId);
                var difficultyStat = DataCache.BookStatByBookIdAndStatKeyRead(
                    (bookId, AvailableBookStat.DIFFICULTYSCORE), context);

                Assert.IsNotNull(difficultyStat);
                Assert.IsNotNull(difficultyStat.Value);
                decimal.TryParse(difficultyStat.Value, out decimal actualDifficulty);
                Assert.AreEqual(expectedDifficulty, actualDifficulty);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookStatsCreateAndSaveMakesDifficultyScoreAsyncTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            decimal expectedDifficulty = 13.23M;

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

                await BookStatApi.BookStatsCreateAndSaveAsync(context, bookId);
                var difficultyStat = await DataCache.BookStatByBookIdAndStatKeyReadAsync(
                    (bookId, AvailableBookStat.DIFFICULTYSCORE), context);

                Assert.IsNotNull(difficultyStat);
                Assert.IsNotNull(difficultyStat.Value);
                decimal.TryParse(difficultyStat.Value, out decimal actualDifficulty);                
                Assert.AreEqual(expectedDifficulty, actualDifficulty);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
    }
}