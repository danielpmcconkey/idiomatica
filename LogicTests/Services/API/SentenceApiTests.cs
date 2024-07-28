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
    public class SentenceApiTests
    {
        [TestMethod()]
        public void PotentialSentencesSplitFromTextTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            string expectedValue = "Los niños iban a jugar al jardín del gigante.";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                var potentialSentences = SentenceApi.PotentialSentencesSplitFromText(
                    context, TestConstants.NewPageText, (int)language.Id);

                Assert.AreEqual(expectedValue, potentialSentences[5]);

            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task PotentialSentencesSplitFromTextAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            
            string expectedValue = "Los niños iban a jugar al jardín del gigante.";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                var potentialSentences = await SentenceApi.PotentialSentencesSplitFromTextAsync(
                    context, TestConstants.NewPageText, (int)language.Id);

                Assert.AreEqual(expectedValue, potentialSentences[5]);
                
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void SentenceCreateTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            string sentence1 = "Cada tarde, después de la escuela.";
            string sentence2 = "Los niños iban a jugar al jardín del gigante.";

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
                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                if (paragraph is null || paragraph.Id is null || paragraph.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // create two sentences
                var sentence1created = SentenceApi.SentenceCreate(
                    context, sentence1, (int)language.Id, 0, (int)paragraph.Id);
                var sentence2created = SentenceApi.SentenceCreate(
                    context, sentence2, (int)language.Id, 1, (int)paragraph.Id);
                // read all the sentences
                var sentencesRead = SentenceApi.SentencesReadByParagraphId(
                    context, (int)paragraph.Id);
                if (sentencesRead is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var secondSentenceRead = sentencesRead.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (secondSentenceRead is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                Assert.AreEqual(sentence2, secondSentenceRead.Text);

            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task SentenceCreateAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            string sentence1 = "Cada tarde, después de la escuela.";
            string sentence2 = "Los niños iban a jugar al jardín del gigante.";

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
                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                if (paragraph is null || paragraph.Id is null || paragraph.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // create two sentences
                var sentence1created = await SentenceApi.SentenceCreateAsync(
                    context, sentence1, (int)language.Id, 0, (int)paragraph.Id);
                var sentence2created = await SentenceApi.SentenceCreateAsync(
                    context, sentence2, (int)language.Id, 1, (int)paragraph.Id);
                // read all the sentences
                var sentencesRead = await SentenceApi.SentencesReadByParagraphIdAsync(
                    context, (int)paragraph.Id);
                if(sentencesRead is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var secondSentenceRead = sentencesRead.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (secondSentenceRead is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                Assert.AreEqual(sentence2, secondSentenceRead.Text);

            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void SentencesReadByPageIdTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int pageId = 3;
            int expectedCount = 36;
            string expectedText = "Julia es mi hermana y vivimos en la misma casa en Londres.";

            try
            {
                var sentences = SentenceApi.SentencesReadByPageId(context, pageId);
                Assert.IsNotNull(sentences);
                Assert.AreEqual(expectedCount, sentences.Count);
                var targetSentence = sentences[11];
                Assert.AreEqual(expectedText, targetSentence.Text);
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task SentencesReadByPageIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int pageId = 3;
            int expectedCount = 36;
            string expectedText = "Julia es mi hermana y vivimos en la misma casa en Londres.";

            try
            {
                var sentences = await SentenceApi.SentencesReadByPageIdAsync(context, pageId);
                Assert.IsNotNull(sentences);
                Assert.AreEqual(expectedCount, sentences.Count);
                var targetSentence = sentences[11];
                Assert.AreEqual(expectedText, targetSentence.Text);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void SentencesReadByParagraphIdTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int paragraphId = 9114;
            int expectedCount = 8;
            int sentenceOrdinal = 2;
            string expectedText = "Julia es mi hermana y vivimos en la misma casa en Londres.";

            try
            {
                var sentences = SentenceApi.SentencesReadByParagraphId(context, paragraphId);
                Assert.IsNotNull(sentences);
                Assert.AreEqual(expectedCount, sentences.Count);
                var targetSentence = sentences.Where(x => x.Ordinal == sentenceOrdinal).FirstOrDefault();
                Assert.IsNotNull(targetSentence);
                Assert.AreEqual(expectedText, targetSentence.Text);
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }

        [TestMethod()]
        public async Task SentencesReadByParagraphIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int paragraphId = 9114;
            int expectedCount = 8;
            int sentenceOrdinal = 2;
            string expectedText = "Julia es mi hermana y vivimos en la misma casa en Londres.";

            try
            {
                var sentences = await SentenceApi.SentencesReadByParagraphIdAsync(context, paragraphId);
                Assert.IsNotNull(sentences);
                Assert.AreEqual(expectedCount, sentences.Count);
                var targetSentence = sentences.Where(x => x.Ordinal == sentenceOrdinal).FirstOrDefault();
                Assert.IsNotNull(targetSentence);
                Assert.AreEqual(expectedText, targetSentence.Text);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }
    }
}