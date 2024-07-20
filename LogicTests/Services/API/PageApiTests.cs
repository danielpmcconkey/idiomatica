using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using static System.Net.Mime.MediaTypeNames;
using DeepL;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class PageApiTests
    {
        [TestMethod()]
        public async Task PageReadByIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int pageId = 3;
            string expectedResult = "varos al aeropuerto."; // the right-most 20 chars



            try
            {
                // act
                var page = await PageApi.PageReadByIdAsync(context, pageId);
                if (page == null || string.IsNullOrEmpty(page.OriginalText))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

                // assert
                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public async Task PageReadFirstByBookIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int bookId = 2;
            string expectedResult = "pezaron la caminata."; // the right-most 20 chars



            try
            {
                // act
                var page = await PageApi.PageReadFirstByBookIdAsync(context, bookId);
                if (page == null || string.IsNullOrEmpty(page.OriginalText))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

                // assert
                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public async Task CreatePageFromPageSplitAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            string expectedResult = "alió un hermosa flor";





            try
            {
                // act
                // set up a new book the same way we do in the book creation flow
                // pull language from the db
                var language = DataCache.LanguageByCodeRead(TestConstants.NewBookLanguageCode, context);
                if (language is null || language.Id is null or 0)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // divide text into paragraphs
                string[] paragraphSplits = ParagraphApi.SplitTextToPotentialParagraphs(
                    context, TestConstants.NewBookText, TestConstants.NewBookLanguageCode);


                // add the book to the DB so you can save pages using its ID
                Book? book = new Book()
                {
                    Title = TestConstants.NewBookTitle,
                    SourceURI = TestConstants.NewBookUrl,
                    LanguageId = language.Id,
                };
                book = await DataCache.BookCreateAsync(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow(2090);
                    return;
                }

                var pageSplits = PageApi.CreatePageSplitsFromParagraphSplits(paragraphSplits);
                if (pageSplits is null || pageSplits.Count == 0)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                var pageSplit = pageSplits.FirstOrDefault();

                // create page objects

                string pageSplitTextTrimmed = pageSplit.pageText.Trim();
                if (string.IsNullOrEmpty(pageSplitTextTrimmed))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                Page? page = await PageApi.CreatePageFromPageSplitAsync(context,
                    pageSplit.pageNum, pageSplitTextTrimmed,
                    (int)book.Id, (int)language.Id);
                if (page is null || page.Id is null || page.Id < 1 || page.OriginalText is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

                // assert
                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public async Task CreatePageSplitsFromParagraphSplitsTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            string expectedResult = "a una escena hermosa";



            try
            {
                // act
                // set up a new book the same way we do in the book creation flow
                // pull language from the db
                var language = DataCache.LanguageByCodeRead(TestConstants.NewBookLanguageCode, context);
                if (language is null || language.Id is null or 0)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // divide text into paragraphs
                string[] paragraphSplits = ParagraphApi.SplitTextToPotentialParagraphs(
                    context, TestConstants.NewBookText, TestConstants.NewBookLanguageCode);


                // add the book to the DB so you can save pages using its ID
                Book? book = new Book()
                {
                    Title = TestConstants.NewBookTitle,
                    SourceURI = TestConstants.NewBookUrl,
                    LanguageId = language.Id,
                };
                book = await DataCache.BookCreateAsync(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow(2090);
                    return;
                }

                var pageSplits = PageApi.CreatePageSplitsFromParagraphSplits(paragraphSplits);
                if (pageSplits is null || pageSplits.Count == 0)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                var textSecondPage = pageSplits[1].pageText;

                string actualResults = textSecondPage.Substring(textSecondPage.Length - 20, 20);

                // assert
                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public void PageReadByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PageReadFirstByBookIdTest()
        {
            Assert.Fail();
        }
    }
}