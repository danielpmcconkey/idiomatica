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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class PageApiTests
    {
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
        public void CreatePageFromPageSplitTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
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
                book = DataCache.BookCreate(book, context);
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
                Page? page = PageApi.CreatePageFromPageSplit(context,
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

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public void CreatePageSplitsFromParagraphSplitsTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
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
                book = DataCache.BookCreate(book, context);
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

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task CreatePageSplitsFromParagraphSplitsAsyncTest()
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
                var language = await DataCache.LanguageByCodeReadAsync(TestConstants.NewBookLanguageCode, context);
                if (language is null || language.Id is null or 0)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // divide text into paragraphs
                string[] paragraphSplits = await ParagraphApi.SplitTextToPotentialParagraphsAsync(
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

                var pageSplits = await PageApi.CreatePageSplitsFromParagraphSplitsAsync(paragraphSplits);
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
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int pageId = 3;
            string expectedResult = "varos al aeropuerto."; // the right-most 20 chars



            try
            {
                // act
                var page = PageApi.PageReadById(context, pageId);
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

                transaction.Rollback();
            }
        }
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
        public void PageReadFirstByBookIdTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 2;
            string expectedResult = "pezaron la caminata."; // the right-most 20 chars



            try
            {
                // act
                var page = PageApi.PageReadFirstByBookId(context, bookId);
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

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public void OrchestrateResetReadDataForNewPageTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 134;
            int wordUsersCount = 134;
            int paragraphCount = 29;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = PageApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.Id is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // now pretend to move the page forward and reset
                int targetPageNum = 2;

                PageUserApi.PageUserMarkAsRead(context, (int)readDataPacket.CurrentPageUser.Id);

                // create the new page user
                
                // but first need to pull the page
                readDataPacket.CurrentPage = PageApi.PageReadByOrdinalAndBookId(context, targetPageNum, bookId);
                if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.Id is null ||
                    readDataPacket.CurrentPage.Id < 1 || readDataPacket.LoggedInUser is null ||
                    readDataPacket.LoggedInUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                readDataPacket.CurrentPageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)readDataPacket.CurrentPage.Id, (int)readDataPacket.LoggedInUser.Id);
                
                if (readDataPacket.CurrentPageUser is null || readDataPacket.CurrentPageUser.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                //finally, we get to reset the data
                var newReadDataPacket = PageApi.OrchestrateResetReadDataForNewPage(
                    context, readDataPacket, (int)readDataPacket.CurrentPageUser.PageId);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageId is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.AllWordUsersInPage is null || newReadDataPacket.Paragraphs is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // assert
                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, newReadDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public void OrchestrateReadDataInitTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 135;
            int wordUsersCount = 135;
            int paragraphCount = 13;
            int bookUserStatsCount = 8;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = PageApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.BookUserStats is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // assert
                Assert.IsTrue(readDataPacket.BookCurrentPageNum == 1);
                Assert.IsNotNull(readDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, readDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, readDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, readDataPacket.Paragraphs.Count);
                Assert.AreEqual(bookUserStatsCount, readDataPacket.BookUserStats.Count);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public void OrchestrateMovePageTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 2;
            int languageId = 1;
            int wordCount = 134;
            int wordUsersCount = 134;
            int paragraphCount = 29;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                // act
                var readDataPacket = PageApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.AllWordUsersInPage is null || readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.Id is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                

                //finally, we get to reset the data
                var newReadDataPacket = PageApi.OrchestrateMovePage(context, readDataPacket, bookId, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageId is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.AllWordUsersInPage is null || newReadDataPacket.Paragraphs is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // assert
                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(wordUsersCount, newReadDataPacket.AllWordUsersInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public void PageReadByOrdinalAndBookIdTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int bookId = 2;
            int ordinal = 2;
            string expectedResult = "y viejas. ¡Ven aquí!"; // the right-most 20 chars

            try
            {
                var page = PageApi.PageReadByOrdinalAndBookId(context, ordinal, bookId);
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

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public async Task PageReadByOrdinalAndBookIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int bookId = 2;
            int ordinal = 2;
            string expectedResult = "y viejas. ¡Ven aquí!"; // the right-most 20 chars

            try
            {
                var page = await PageApi.PageReadByOrdinalAndBookIdAsync(context, ordinal, bookId);
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
    }
}