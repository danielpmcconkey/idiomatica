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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class ParagraphApiTests
    {
        [TestMethod()]
        public void ParagraphCreateFromSplitTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                if (page is null || page.Id is null || page.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the paragraph splits
                var paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(
                   context, TestConstants.NewPageText, TestConstants.NewBookLanguageCode);
                if (paragraphSplits is null || paragraphSplits.Length < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var fifthSplit = paragraphSplits[4];
                if (string.IsNullOrEmpty(fifthSplit))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var fifthSplitTrimmed = fifthSplit.Trim();
                if (string.IsNullOrEmpty(fifthSplitTrimmed))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // act

                var paragraph = ParagraphApi.ParagraphCreateFromSplit(context,
                    fifthSplitTrimmed, (int)page.Id, 4, (int)language.Id);


                // assert
                Assert.IsNotNull(paragraph);
                Assert.IsNotNull(paragraph.Sentences);
                Assert.IsTrue(paragraph.Sentences.Count > 0);
                var thirdSentence = paragraph.Sentences.Where(s => s.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(thirdSentence);
                Assert.IsNotNull(thirdSentence.Text);
                var actualText = thirdSentence.Text[..20];
                Assert.AreEqual(expectedText, actualText);
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task ParagraphCreateFromSplitAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                if (page is null || page.Id is null || page.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the paragraph splits
                var paragraphSplits = await ParagraphApi.PotentialParagraphsSplitFromTextAsync(
                   context, TestConstants.NewPageText, TestConstants.NewBookLanguageCode);
                if(paragraphSplits is null || paragraphSplits.Length < 1)
                {
                    ErrorHandler.LogAndThrow(); 
                    return; 
                }
                var fifthSplit = paragraphSplits[4];
                if(string.IsNullOrEmpty(fifthSplit))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var fifthSplitTrimmed = fifthSplit.Trim();
                if (string.IsNullOrEmpty(fifthSplitTrimmed))
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // act

                var paragraph = await ParagraphApi.ParagraphCreateFromSplitAsync(context,
                    fifthSplitTrimmed, (int)page.Id, 4, (int)language.Id);


                // assert
                Assert.IsNotNull(paragraph);
                Assert.IsNotNull(paragraph.Sentences);
                Assert.IsTrue(paragraph.Sentences.Count > 0);
                var thirdSentence = paragraph.Sentences.Where(s => s.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(thirdSentence);
                Assert.IsNotNull(thirdSentence.Text);
                var actualText = thirdSentence.Text[..20];
                Assert.AreEqual(expectedText, actualText);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void ParagraphReadAllTextTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public async Task ParagraphReadAllTextAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();




            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void ParagraphsCreateFromPageTest()
        {
            
                // assert
                Assert.Fail();
            
        }
        [TestMethod()]
        public async Task ParagraphsCreateFromPageAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();




            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void ParagraphsReadByPageIdTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task ParagraphsReadByPageIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();




            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void ParagraphTranslateTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public async Task ParagraphTranslateAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();




            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void PotentialParagraphsFromSplitTextTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();




            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task PotentialParagraphsFromSplitTextTestAsync()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();




            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }


    }
}