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
using System.Net;
using Logic.Conjugator.Spanish;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class PageApiTests
    {
        [TestMethod()]
        public void PageCreateFromPageSplitTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedResult = "alió un hermosa flor";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // pull language from the db
                var language = DataCache.LanguageByCodeRead(TestConstants.NewBookLanguageCode, context);
                if (language is null || language.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // divide text into paragraphs
                string[] paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(
                    context, TestConstants.NewBookText, TestConstants.NewBookLanguageCode);

                var pageSplits = PageApi.PageSplitsCreateFromParagraphSplits(paragraphSplits);
                if (pageSplits is null || pageSplits.Count == 0)
                { ErrorHandler.LogAndThrow(); return; }

                var pageSplit = pageSplits.FirstOrDefault();

                // create page objects

                string pageSplitTextTrimmed = pageSplit.pageText.Trim();
                if (string.IsNullOrEmpty(pageSplitTextTrimmed))
                { ErrorHandler.LogAndThrow(); return; }
                Page? page = PageApi.PageCreateFromPageSplit(context,
                    pageSplit.pageNum, pageSplitTextTrimmed,
                    bookId, (Guid)language.UniqueKey);
                if (page is null || page.UniqueKey is null || page.OriginalText is null)
                { ErrorHandler.LogAndThrow(); return; }

                string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageCreateFromPageSplitAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedResult = "alió un hermosa flor";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // pull language from the db
                var language = await DataCache.LanguageByCodeReadAsync(TestConstants.NewBookLanguageCode, context);
                if (language is null || language.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // divide text into paragraphs
                string[] paragraphSplits = await ParagraphApi.PotentialParagraphsSplitFromTextAsync(
                    context, TestConstants.NewBookText, TestConstants.NewBookLanguageCode);

                var pageSplits = await PageApi.PageSplitsCreateFromParagraphSplitsAsync(
                    paragraphSplits);
                if (pageSplits is null || pageSplits.Count == 0)
                    { ErrorHandler.LogAndThrow(); return; }

                var pageSplit = pageSplits.FirstOrDefault();

                // create page objects

                string pageSplitTextTrimmed = pageSplit.pageText.Trim();
                if (string.IsNullOrEmpty(pageSplitTextTrimmed))
                    { ErrorHandler.LogAndThrow(); return; }
                Page? page = await PageApi.PageCreateFromPageSplitAsync(context,
                    pageSplit.pageNum, pageSplitTextTrimmed,
                    bookId, (Guid)language.UniqueKey);
                if (page is null || page.UniqueKey is null || page.OriginalText is null)
                    { ErrorHandler.LogAndThrow(); return; }

                string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void PageReadByIdTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid pageId = CommonFunctions.GetPage392Id(context);
            string expectedResult = "erías pirenaicas.33?"; // the right-most 20 chars

            var page = PageApi.PageReadById(context, pageId);
            if (page == null || string.IsNullOrEmpty(page.OriginalText))
            { ErrorHandler.LogAndThrow(); return; }
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }
        [TestMethod()]
        public async Task PageReadByIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid pageId = CommonFunctions.GetPage392Id(context);
            string expectedResult = "erías pirenaicas.33?"; // the right-most 20 chars

            var page = await PageApi.PageReadByIdAsync(context, pageId);
            if (page == null || string.IsNullOrEmpty(page.OriginalText))
            { ErrorHandler.LogAndThrow(); return; }
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }


        [TestMethod()]
        public void PageReadByOrdinalAndBookIdTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            int ordinal = 2;
            string expectedResult = "riunfa sobre el mal."; // the right-most 20 chars

            var page = PageApi.PageReadByOrdinalAndBookId(context, ordinal, bookId);
            if (page == null || string.IsNullOrEmpty(page.OriginalText))
            { ErrorHandler.LogAndThrow(); return; }
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }
        [TestMethod()]
        public async Task PageReadByOrdinalAndBookIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            int ordinal = 2;
            string expectedResult = "riunfa sobre el mal."; // the right-most 20 chars
            

            var page = await PageApi.PageReadByOrdinalAndBookIdAsync(context, ordinal, bookId);
            if (page == null || string.IsNullOrEmpty(page.OriginalText))
            { ErrorHandler.LogAndThrow(); return; }
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }


        [TestMethod()]
        public void PageReadFirstByBookIdTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            string expectedResult = "n lugar de Rapunzel."; // the right-most 20 chars

            var page = PageApi.PageReadFirstByBookId(context, bookId);
            if (page == null || string.IsNullOrEmpty(page.OriginalText))
            { ErrorHandler.LogAndThrow(); return; }
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }
        [TestMethod()]
        public async Task PageReadFirstByBookIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            string expectedResult = "n lugar de Rapunzel."; // the right-most 20 chars

            var page = await PageApi.PageReadFirstByBookIdAsync(context, bookId);
            if (page == null || string.IsNullOrEmpty(page.OriginalText))
            { ErrorHandler.LogAndThrow(); return; }
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }


        [TestMethod()]
        public void PageSplitsCreateFromParagraphSplitsTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedResult = "a una escena hermosa";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // divide text into paragraphs
                string[] paragraphSplits =  ParagraphApi.PotentialParagraphsSplitFromText(
                    context, TestConstants.NewBookText, TestConstants.NewBookLanguageCode);

                var pageSplits = PageApi.PageSplitsCreateFromParagraphSplits(paragraphSplits);
                if (pageSplits is null || pageSplits.Count == 0)
                { ErrorHandler.LogAndThrow(); return; }

                var textSecondPage = pageSplits[1].pageText;

                string actualResults = textSecondPage.Substring(textSecondPage.Length - 20, 20);

                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageSplitsCreateFromParagraphSplitsAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedResult = "a una escena hermosa";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // divide text into paragraphs
                string[] paragraphSplits = await ParagraphApi.PotentialParagraphsSplitFromTextAsync(
                    context, TestConstants.NewBookText, TestConstants.NewBookLanguageCode);

                var pageSplits = await PageApi.PageSplitsCreateFromParagraphSplitsAsync(paragraphSplits);
                if (pageSplits is null || pageSplits.Count == 0)
                { ErrorHandler.LogAndThrow(); return; }

                var textSecondPage = pageSplits[1].pageText;

                string actualResults = textSecondPage.Substring(textSecondPage.Length - 20, 20);

                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
    }
}