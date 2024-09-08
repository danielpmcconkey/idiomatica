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
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = (Guid)language.UniqueKey,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                if (page is null || page.UniqueKey is null)
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
                    fifthSplitTrimmed, (Guid)page.UniqueKey, 4, (Guid)language.UniqueKey);


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
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = (Guid)language.UniqueKey,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                if (page is null || page.UniqueKey is null)
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
                    fifthSplitTrimmed, (Guid)page.UniqueKey, 4, (Guid)language.UniqueKey);


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
            var languageUserId = CommonFunctions.GetSpanishLanguageUserKey(context);
            Guid flashCardId = CommonFunctions.GetFlashCard1Id(context, languageUserId);
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
            var languageUserId = CommonFunctions.GetSpanishLanguageUserKey(context);
            Guid flashCardId = CommonFunctions.GetFlashCard1Id(context, languageUserId);
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
            Guid paragraphId = CommonFunctions.GetParagraph14706Id(context);
            string expectedValue = "África del Norte, África septentrional o África norsahariana (a veces llamada África Blanca) es la subregión norte de África. Está compuesta por cinco países: Argelia, Egipto, Libia, Marruecos y Túnez. Además, incluye a la República Árabe Saharaui Democrática (que es un Estado con reconocimiento limitado) y otros territorios que dependen de países externos a la subregión: Canarias, Ceuta y Melilla (que dependen de España), Madeira (de Portugal) y Lampedusa e Linosa (de Italia).";


            string actualValue = ParagraphApi.ParagraphReadAllText(context, paragraphId);

            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod()]
        public async Task ParagraphReadAllTextAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid paragraphId = CommonFunctions.GetParagraph14706Id(context);
            string expectedValue = "África del Norte, África septentrional o África norsahariana (a veces llamada África Blanca) es la subregión norte de África. Está compuesta por cinco países: Argelia, Egipto, Libia, Marruecos y Túnez. Además, incluye a la República Árabe Saharaui Democrática (que es un Estado con reconocimiento limitado) y otros territorios que dependen de países externos a la subregión: Canarias, Ceuta y Melilla (que dependen de España), Madeira (de Portugal) y Lampedusa e Linosa (de Italia).";

            

            string actualValue = await ParagraphApi.ParagraphReadAllTextAsync(context, paragraphId);
            Assert.AreEqual(expectedValue, actualValue);            
        }


        [TestMethod()]
        public void ParagraphsCreateFromPageTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = (Guid)language.UniqueKey,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                if (page is null || page.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // create the paragraphs
                var paragraphs = ParagraphApi.ParagraphsCreateFromPage(
                    context, (Guid)page.UniqueKey, (Guid)language.UniqueKey);
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
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = (Guid)language.UniqueKey,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                if (page is null || page.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // create the paragraphs
                var paragraphs = await ParagraphApi.ParagraphsCreateFromPageAsync(
                    context, (Guid)page.UniqueKey, (Guid)language.UniqueKey);
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
            Guid pageId = CommonFunctions.GetPage400Id(context);
            int ppOrd = 0;
            int sentenceOrdinal = 1;
            string expectedValue = "Está compuesta por cinco países: Argelia, Egipto, Libia, Marruecos y Túnez.";
            
            var paragraphs = ParagraphApi.ParagraphsReadByPageId(context, pageId);
            Assert.IsNotNull(paragraphs);
            var seventhParagraph = paragraphs.Where(x => x.Ordinal == ppOrd).FirstOrDefault();
            Assert.IsNotNull(seventhParagraph);
            Assert.IsNotNull(seventhParagraph.UniqueKey);
            seventhParagraph.Sentences = SentenceApi.SentencesReadByParagraphId(
                context, (Guid)seventhParagraph.UniqueKey);
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
            Guid pageId = CommonFunctions.GetPage400Id(context);
            int ppOrd = 0;
            int sentenceOrdinal = 1;
            string expectedValue = "Está compuesta por cinco países: Argelia, Egipto, Libia, Marruecos y Túnez.";

            var paragraphs = await ParagraphApi.ParagraphsReadByPageIdAsync(context, pageId);
            Assert.IsNotNull(paragraphs);
            var seventhParagraph = paragraphs.Where(x => x.Ordinal == ppOrd).FirstOrDefault();
            Assert.IsNotNull(seventhParagraph);
            Assert.IsNotNull(seventhParagraph.UniqueKey);
            seventhParagraph.Sentences = await SentenceApi.SentencesReadByParagraphIdAsync(
                context, (Guid)seventhParagraph.UniqueKey);
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
            Guid paragraphId = CommonFunctions.GetParagraph14590Id(context);
            string fromCode = "ES";
            string toCode = "EN-US";
            string expectedInput = "Había una vez una pareja que quería un hijo. La esposa, que estaba esperando, tenía antojo de una planta llamada rapunzel, que crecía en un jardín cercano perteneciente a una hechicera.";
            string expectedTranslation = "Once upon a time there was a couple who wanted a child. The wife, who was expecting, had a craving for a plant called rapunzel, which grew in a nearby garden belonging to a sorceress.";

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
            var context = CommonFunctions.CreateContext();
            Guid paragraphId = CommonFunctions.GetParagraph14590Id(context);
            string fromCode = "ES";
            string toCode = "EN-US";
            string expectedInput = "Había una vez una pareja que quería un hijo. La esposa, que estaba esperando, tenía antojo de una planta llamada rapunzel, que crecía en un jardín cercano perteneciente a una hechicera.";
            string expectedTranslation = "Once upon a time there was a couple who wanted a child. The wife, who was expecting, had a craving for a plant called rapunzel, which grew in a nearby garden belonging to a sorceress.";

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
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedText = "Cada tarde, después de la escuela. Los niños iban a jugar al jardín del gigante. Era un jardín grande y bonito.";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = (Guid)language.UniqueKey,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                if (page is null || page.UniqueKey is null)
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
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string expectedText = "Cada tarde, después de la escuela. Los niños iban a jugar al jardín del gigante. Era un jardín grande y bonito.";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null || language.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                
                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = (Guid)language.UniqueKey,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null || book.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                if (page is null || page.UniqueKey is null)
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