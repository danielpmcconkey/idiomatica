//#define FORCEDEEPLCALL // when set, all tests that use DeepL will delete the entry from the DB first

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
using Microsoft.AspNetCore.Http;


namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class ParagraphApiTests
    {
        [TestMethod()]
        public void ParagraphCreateFromSplitTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

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
                { ErrorHandler.LogAndThrow(); return; }

                // create the paragraph splits
                var paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(
                   context, TestConstants.NewPageText, TestConstants.NewBookLanguageCode);
                if (paragraphSplits is null || paragraphSplits.Length < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var fifthSplit = paragraphSplits[4];
                if (string.IsNullOrEmpty(fifthSplit))
                { ErrorHandler.LogAndThrow(); return; }
                var fifthSplitTrimmed = fifthSplit.Trim();
                if (string.IsNullOrEmpty(fifthSplitTrimmed))
                { ErrorHandler.LogAndThrow(); return; }


                var paragraph = ParagraphApi.ParagraphCreateFromSplit(context,
                    fifthSplitTrimmed, (int)page.Id, 4, (int)language.Id);


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
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task ParagraphCreateFromSplitAsyncTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                    { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

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
                { ErrorHandler.LogAndThrow(); return; }

                // create the paragraph splits
                var paragraphSplits = await ParagraphApi.PotentialParagraphsSplitFromTextAsync(
                   context, TestConstants.NewPageText, TestConstants.NewBookLanguageCode);
                if (paragraphSplits is null || paragraphSplits.Length < 1)
                    { ErrorHandler.LogAndThrow(); return; }
                var fifthSplit = paragraphSplits[4];
                if (string.IsNullOrEmpty(fifthSplit))
                    { ErrorHandler.LogAndThrow(); return; }
                var fifthSplitTrimmed = fifthSplit.Trim();
                if (string.IsNullOrEmpty(fifthSplitTrimmed))
                    { ErrorHandler.LogAndThrow(); return; }

                
                var paragraph = await ParagraphApi.ParagraphCreateFromSplitAsync(context,
                    fifthSplitTrimmed, (int)page.Id, 4, (int)language.Id);


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
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void ParagraphExamplePullRandomByFlashCardIdTest()
        {
            var context = CommonFunctions.CreateContext();

            int flashCardId = 400;
            string uiLangugaeCode = "EN-US";
            int numTries = 5;
            HashSet<string> examples = new();
            HashSet<string> translations = new();
            int translationDuplicates = 0;
            int exampleDuplicates = 0;
            float threshold = 0.75f;

            
            /* 
                * we run this many times to test that we get a random 
                * paragraphtranslation back 
                * 
                */
            for (int i = 0; i < numTries; i++)
            {
                var result = ParagraphApi.ParagraphExamplePullRandomByFlashCardId(
                        context, flashCardId, uiLangugaeCode);
                Assert.IsNotNull(result);
                Assert.IsTrue(string.IsNullOrEmpty(result.example) == false);
                Assert.IsTrue(string.IsNullOrEmpty(result.translation) == false);
                Assert.IsTrue(result.translation != result.example);
                    
                if (examples.Contains(result.example))
                {
                    exampleDuplicates++;
                }
                else examples.Add(result.example);
                if (translations.Contains(result.translation))
                {
                    translationDuplicates++;
                }
                else translations.Add(result.translation);
            }
            float percentDups = exampleDuplicates / (float)numTries;
            Assert.IsTrue(percentDups < threshold);            
        }
        [TestMethod()]
        public async Task ParagraphExamplePullRandomByFlashCardIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();

            int flashCardId = 400;
            string uiLangugaeCode = "EN-US";
            int numTries = 5;
            HashSet<string> examples = new();
            HashSet<string> translations = new();
            int translationDuplicates = 0;
            int exampleDuplicates = 0;
            float threshold = 0.75f;
            /* 
                * we run this many times to test that we get a random 
                * paragraphtranslation back 
                * 
                */
            for (int i = 0; i < numTries; i++)
            {
                var result = await ParagraphApi.ParagraphExamplePullRandomByFlashCardIdAsync(
                        context, flashCardId, uiLangugaeCode);
                Assert.IsNotNull(result);
                Assert.IsTrue(string.IsNullOrEmpty(result.example) == false);
                Assert.IsTrue(string.IsNullOrEmpty(result.translation) == false);
                Assert.IsTrue(result.translation != result.example);

                if (examples.Contains(result.example))
                {
                    exampleDuplicates++;
                }
                else examples.Add(result.example);
                if (translations.Contains(result.translation))
                {
                    translationDuplicates++;
                }
                else translations.Add(result.translation);
            }
            float percentDups = exampleDuplicates / (float)numTries;
            Assert.IsTrue(percentDups < threshold);
            
        }


        [TestMethod()]
        public void ParagraphReadAllTextTest()
        {
            var context = CommonFunctions.CreateContext();
            int paragraphId = 9114;
            string expectedValue = "Mi nombre es Daniel. Tengo 24 años. Julia es mi hermana y vivimos en la misma casa en Londres. Ella tiene 23 años. Vivimos con nuestros padres, Arthur y Clara. Estamos preparando nuestro viaje a España. Somos estudiantes de intercambio. Estamos aprendiendo español y ya sabemos mucho.";

            string actualValue = ParagraphApi.ParagraphReadAllText(context, paragraphId);

            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod()]
        public async Task ParagraphReadAllTextAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            int paragraphId = 9114;
            string expectedValue = "Mi nombre es Daniel. Tengo 24 años. Julia es mi hermana y vivimos en la misma casa en Londres. Ella tiene 23 años. Vivimos con nuestros padres, Arthur y Clara. Estamos preparando nuestro viaje a España. Somos estudiantes de intercambio. Estamos aprendiendo español y ya sabemos mucho.";

            string actualValue = await ParagraphApi.ParagraphReadAllTextAsync(context, paragraphId);
            Assert.AreEqual(expectedValue, actualValue);            
        }


        [TestMethod()]
        public void ParagraphsCreateFromPageTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

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
                { ErrorHandler.LogAndThrow(); return; }

                // create the paragraphs
                var paragraphs = ParagraphApi.ParagraphsCreateFromPage(
                    context, (int)page.Id, (int)language.Id);
                if (paragraphs is null || paragraphs.Count < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var fifthParagraph = paragraphs.Where(x => x.Ordinal == 4).FirstOrDefault();
                if (fifthParagraph is null || fifthParagraph.Sentences is null || fifthParagraph.Sentences.Count < 1)
                { ErrorHandler.LogAndThrow(); return; }

                var thirdSentence = fifthParagraph.Sentences.Where(s => s.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(thirdSentence);
                Assert.IsNotNull(thirdSentence.Text);
                var actualText = thirdSentence.Text[..20];
                Assert.AreEqual(expectedText, actualText);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task ParagraphsCreateFromPageAsyncTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                    { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

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
                    { ErrorHandler.LogAndThrow(); return; }

                // create the paragraphs
                var paragraphs = await ParagraphApi.ParagraphsCreateFromPageAsync(
                    context, (int)page.Id, (int)language.Id);
                if (paragraphs is null || paragraphs.Count < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var fifthParagraph = paragraphs.Where(x => x.Ordinal == 4).FirstOrDefault();
                if (fifthParagraph is null || fifthParagraph.Sentences is null || fifthParagraph.Sentences.Count < 1)
                { ErrorHandler.LogAndThrow(); return; }

                var thirdSentence = fifthParagraph.Sentences.Where(s => s.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(thirdSentence);
                Assert.IsNotNull(thirdSentence.Text);
                var actualText = thirdSentence.Text[..20];
                Assert.AreEqual(expectedText, actualText);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void ParagraphsReadByPageIdTest()
        {
            var context = CommonFunctions.CreateContext();
            int pageId = 3;
            int ppOrd = 6;
            int sentenceOrdinal = 1;
            string expectedValue = "Tengo 24 años.";
            
            var paragraphs = ParagraphApi.ParagraphsReadByPageId(context, pageId);
            Assert.IsNotNull(paragraphs);
            var seventhParagraph = paragraphs.Where(x => x.Ordinal == ppOrd).FirstOrDefault();
            Assert.IsNotNull(seventhParagraph);
            Assert.IsNotNull(seventhParagraph.Id);
            seventhParagraph.Sentences = SentenceApi.SentencesReadByParagraphId(
                context, (int)seventhParagraph.Id);
            Assert.IsNotNull(seventhParagraph.Sentences);
            var secondSentence = seventhParagraph.Sentences.Where(
                x => x.Ordinal == sentenceOrdinal).FirstOrDefault();
            Assert.IsNotNull(secondSentence);
            var actualValue = secondSentence.Text;
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod()]
        public async Task ParagraphsReadByPageIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            int pageId = 3;
            int ppOrd = 6;
            int sentenceOrdinal = 1;
            string expectedValue = "Tengo 24 años.";

            var paragraphs = await ParagraphApi.ParagraphsReadByPageIdAsync(context, pageId);
            Assert.IsNotNull(paragraphs);
            var seventhParagraph = paragraphs.Where(x => x.Ordinal == ppOrd).FirstOrDefault();
            Assert.IsNotNull(seventhParagraph);
            Assert.IsNotNull(seventhParagraph.Id);
            seventhParagraph.Sentences = await SentenceApi.SentencesReadByParagraphIdAsync(
                context, (int)seventhParagraph.Id);
            Assert.IsNotNull(seventhParagraph.Sentences);
            var secondSentence = seventhParagraph.Sentences.Where(
                x => x.Ordinal == sentenceOrdinal).FirstOrDefault();
            Assert.IsNotNull(secondSentence);
            var actualValue = secondSentence.Text;
            Assert.AreEqual(expectedValue, actualValue);
        }


        [TestMethod()]
        public void ParagraphTranslateTest()
        {
            var context = CommonFunctions.CreateContext();
            int paragraphId = 9114;
            string fromCode = "ES";
            string toCode = "EN-US";
            string expectedInput = "Mi nombre es Daniel. Tengo 24 años. Julia es mi hermana y vivimos en la misma casa en Londres. Ella tiene 23 años. Vivimos con nuestros padres, Arthur y Clara. Estamos preparando nuestro viaje a España. Somos estudiantes de intercambio. Estamos aprendiendo español y ya sabemos mucho.";
            string expectedTranslation = "My name is Daniel. I am 24 years old. Julia is my sister and we live in the same house in London. She is 23 years old. We live with our parents, Arthur and Clara. We are preparing our trip to Spain. We are exchange students. We are learning Spanish and we already know a lot.";

#if FORCEDEEPLCALL
            DataCache.ParagraphTranslationDeleteByParagraphIdAndLanguageCode(
                (paragraphId, toCode), context);
#endif

            (string input, string output) result = ParagraphApi.ParagraphTranslate(
                context, paragraphId, fromCode, toCode);
            string actualInput = result.input;
            string actualTranslation = result.output;

            // assert

            Assert.AreEqual(expectedInput, actualInput);
            Assert.AreEqual(expectedTranslation, actualTranslation);
        }
        [TestMethod()]
        public async Task ParagraphTranslateAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            // don't roll this back. We want to save the translation so we're not always using DeepL
            int paragraphId = 9114;
            string fromCode = "ES";
            string toCode = "EN-US";
            string expectedInput = "Mi nombre es Daniel. Tengo 24 años. Julia es mi hermana y vivimos en la misma casa en Londres. Ella tiene 23 años. Vivimos con nuestros padres, Arthur y Clara. Estamos preparando nuestro viaje a España. Somos estudiantes de intercambio. Estamos aprendiendo español y ya sabemos mucho.";
            string expectedTranslation = "My name is Daniel. I am 24 years old. Julia is my sister and we live in the same house in London. She is 23 years old. We live with our parents, Arthur and Clara. We are preparing our trip to Spain. We are exchange students. We are learning Spanish and we already know a lot.";

#if FORCEDEEPLCALL
            await DataCache.ParagraphTranslationDeleteByParagraphIdAndLanguageCodeAsync(
                (paragraphId, toCode), context);
#endif

            (string input, string output) result = await ParagraphApi.ParagraphTranslateAsync(
                context, paragraphId, fromCode, toCode);
            string actualInput = result.input;
            string actualTranslation = result.output;

            Assert.AreEqual(expectedInput, actualInput);
            Assert.AreEqual(expectedTranslation, actualTranslation);
        }


        [TestMethod()]
        public void PotentialParagraphsFromSplitTextTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string expectedText = "Cada tarde, después de la escuela. Los niños iban a jugar al jardín del gigante. Era un jardín grande y bonito.";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

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
                { ErrorHandler.LogAndThrow(); return; }

                // create the paragraph splits
                var paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(
                   context, TestConstants.NewPageText, TestConstants.NewBookLanguageCode);
                if (paragraphSplits is null || paragraphSplits.Length < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var fifthSplit = paragraphSplits[4];
                if (string.IsNullOrEmpty(fifthSplit))
                { ErrorHandler.LogAndThrow(); return; }
                var fifthSplitTrimmed = fifthSplit.Trim();
                if (string.IsNullOrEmpty(fifthSplitTrimmed))
                { ErrorHandler.LogAndThrow(); return; }

                Assert.AreEqual(expectedText, fifthSplitTrimmed);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task PotentialParagraphsFromSplitTextTestAsync()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string expectedText = "Cada tarde, después de la escuela. Los niños iban a jugar al jardín del gigante. Era un jardín grande y bonito.";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.Id is null || language.Id < 1)
                    { ErrorHandler.LogAndThrow(); return; }
                
                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.Id is null || book.Id < 1)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

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
                { ErrorHandler.LogAndThrow(); return; }

                // create the paragraph splits
                var paragraphSplits = await ParagraphApi.PotentialParagraphsSplitFromTextAsync(
                   context, TestConstants.NewPageText, TestConstants.NewBookLanguageCode);
                if (paragraphSplits is null || paragraphSplits.Length < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var fifthSplit = paragraphSplits[4];
                if (string.IsNullOrEmpty(fifthSplit))
                { ErrorHandler.LogAndThrow(); return; }
                var fifthSplitTrimmed = fifthSplit.Trim();
                if (string.IsNullOrEmpty(fifthSplitTrimmed))
                { ErrorHandler.LogAndThrow(); return; }

                Assert.AreEqual(expectedText, fifthSplitTrimmed);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
    }
}