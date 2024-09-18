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
using Microsoft.EntityFrameworkCore;
using Model.Enums;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class PageApiTests
    {
        [TestMethod()]
        public void PageCreateFromPageSplitTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            string expectedResult = "alió un hermosa flor";

            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                Book? book = BookApi.BookRead(context, (Guid)bookId);
                Assert.IsNotNull(book);

                // pull language from the db
                var language = DataCache.LanguageByCodeRead(TestConstants.NewBookLanguageCode, context);
                Assert.IsNotNull(language);

                // divide text into paragraphs
                string[] paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(
                    context, TestConstants.NewBookText, language);

                var pageSplits = PageApi.PageSplitsCreateFromParagraphSplits(
                    paragraphSplits);
                Assert.IsNotNull(pageSplits);

                var pageSplit = pageSplits.FirstOrDefault();

                // create page objects

                string pageSplitTextTrimmed = pageSplit.pageText.Trim();
                Assert.IsFalse(string.IsNullOrEmpty(pageSplitTextTrimmed));
                Page? page = PageApi.PageCreateFromPageSplit(context,
                    pageSplit.pageNum, pageSplitTextTrimmed,
                    book, language);
                Assert.IsNotNull(page);

                string actualResults = page.OriginalText.Substring(
                    page.OriginalText.Length - 20, 20);

                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageCreateFromPageSplitAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            string expectedResult = "alió un hermosa flor";

            try
            {
                Assert.IsNotNull(loginService);
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                Book? book = BookApi.BookRead(context, (Guid)bookId);
                Assert.IsNotNull(book);

                // pull language from the db
                var language = await DataCache.LanguageByCodeReadAsync(TestConstants.NewBookLanguageCode, context);
                Assert.IsNotNull(language);

                // divide text into paragraphs
                string[] paragraphSplits = await ParagraphApi.PotentialParagraphsSplitFromTextAsync(
                    context, TestConstants.NewBookText, language);

                var pageSplits = await PageApi.PageSplitsCreateFromParagraphSplitsAsync(
                    paragraphSplits);
                Assert.IsNotNull(pageSplits);

                var pageSplit = pageSplits.FirstOrDefault();

                // create page objects

                string pageSplitTextTrimmed = pageSplit.pageText.Trim();
                Assert.IsFalse(string.IsNullOrEmpty(pageSplitTextTrimmed));
                Page? page = await PageApi.PageCreateFromPageSplitAsync(context,
                    pageSplit.pageNum, pageSplitTextTrimmed,
                    book, language);
                Assert.IsNotNull(page);

                string actualResults = page.OriginalText.Substring(
                    page.OriginalText.Length - 20, 20);

                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void PageReadByIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetPage392Id(context);
            string expectedResult = "erías pirenaicas.33?"; // the right-most 20 chars

            var page = PageApi.PageReadById(context, pageId);
            Assert.IsNotNull(page);
            Assert.IsFalse(string.IsNullOrEmpty(page.OriginalText));
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }
        [TestMethod()]
        public async Task PageReadByIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetPage392Id(context);
            string expectedResult = "erías pirenaicas.33?"; // the right-most 20 chars

            var page = await PageApi.PageReadByIdAsync(context, pageId);
            Assert.IsNotNull(page);
            Assert.IsFalse(string.IsNullOrEmpty(page.OriginalText));
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }


        [TestMethod()]
        public void PageReadByOrdinalAndBookIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            int ordinal = 2;
            string expectedResult = "riunfa sobre el mal."; // the right-most 20 chars


            var page = PageApi.PageReadByOrdinalAndBookId(context, ordinal, bookId);
            Assert.IsNotNull(page);
            Assert.IsFalse(string.IsNullOrEmpty(page.OriginalText));
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }
        [TestMethod()]
        public async Task PageReadByOrdinalAndBookIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            int ordinal = 2;
            string expectedResult = "riunfa sobre el mal."; // the right-most 20 chars
            

            var page = await PageApi.PageReadByOrdinalAndBookIdAsync(context, ordinal, bookId);
            Assert.IsNotNull(page);
            Assert.IsFalse(string.IsNullOrEmpty(page.OriginalText));
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }


        [TestMethod()]
        public void PageReadFirstByBookIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            string expectedResult = "n lugar de Rapunzel."; // the right-most 20 chars

            var page = PageApi.PageReadFirstByBookId(context, bookId);
            Assert.IsNotNull(page);
            Assert.IsFalse(string.IsNullOrEmpty(page.OriginalText));
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }
        [TestMethod()]
        public async Task PageReadFirstByBookIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            string expectedResult = "n lugar de Rapunzel."; // the right-most 20 chars

            var page = await PageApi.PageReadFirstByBookIdAsync(context, bookId);
            Assert.IsNotNull(page);
            Assert.IsFalse(string.IsNullOrEmpty(page.OriginalText));
            string actualResults = page.OriginalText.Substring(page.OriginalText.Length - 20, 20);

            Assert.AreEqual(expectedResult, actualResults);
        }


        [TestMethod()]
        public void PageSplitsCreateFromParagraphSplitsTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();


            string expectedResult = "a una escena hermosa";

            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // pull language from the db
                var language = DataCache.LanguageByCodeRead(
                    TestConstants.NewBookLanguageCode, context);
                Assert.IsNotNull(language);

                // divide text into paragraphs
                string[] paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(
                    context, TestConstants.NewBookText, language);

                var pageSplits = PageApi.PageSplitsCreateFromParagraphSplits(
                    paragraphSplits);
                Assert.IsNotNull(pageSplits);

                var textSecondPage = pageSplits[1].pageText;

                string actualResults = textSecondPage.Substring(textSecondPage.Length - 20, 20);

                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PageSplitsCreateFromParagraphSplitsAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();


            string expectedResult = "a una escena hermosa";

            try
            {
                Assert.IsNotNull(loginService);
                var createResult = await CommonFunctions.CreateUserAndBookAndBookUserAsync(
                    context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;

                // pull language from the db
                var language = await DataCache.LanguageByCodeReadAsync(
                    TestConstants.NewBookLanguageCode, context);
                Assert.IsNotNull(language);

                // divide text into paragraphs
                string[] paragraphSplits = await ParagraphApi.PotentialParagraphsSplitFromTextAsync(
                    context, TestConstants.NewBookText, language);

                var pageSplits = await PageApi.PageSplitsCreateFromParagraphSplitsAsync(
                    paragraphSplits);
                Assert.IsNotNull(pageSplits);

                var textSecondPage = pageSplits[1].pageText;

                string actualResults = textSecondPage.Substring(textSecondPage.Length - 20, 20);

                Assert.AreEqual(expectedResult, actualResults);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }
    }
}