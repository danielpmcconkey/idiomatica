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
using Model.Enums;
using Microsoft.EntityFrameworkCore;


namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class ParagraphApiTests
    {
        [TestMethod()]
        public void ParagraphCreateFromSplitTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    dbContextFactory, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = language.Id,
                    Language = language,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, dbContextFactory);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, dbContextFactory);
                Assert.IsNotNull(page);

                // create the paragraph splits
                var paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(
                   dbContextFactory, TestConstants.NewPageText, language);
                Assert.IsNotNull(paragraphSplits);
                Assert.IsFalse(paragraphSplits.Length < 1);
                var fifthSplit = paragraphSplits[4];
                Assert.IsFalse(string.IsNullOrEmpty(fifthSplit));
                var fifthSplitTrimmed = fifthSplit.Trim();
                Assert.IsFalse(string.IsNullOrEmpty(fifthSplitTrimmed));


                var paragraph = ParagraphApi.ParagraphCreateFromSplit(dbContextFactory,
                    fifthSplitTrimmed, page, 4, language);


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
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task ParagraphCreateFromSplitAsyncTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    dbContextFactory, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = language.Id,
                    Language = language,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, dbContextFactory);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, dbContextFactory);
                Assert.IsNotNull(page);

                // create the paragraph splits
                var paragraphSplits = await ParagraphApi.PotentialParagraphsSplitFromTextAsync(
                   dbContextFactory, TestConstants.NewPageText, language);
                Assert.IsNotNull(paragraphSplits);
                Assert.IsFalse(paragraphSplits.Length < 1);
                var fifthSplit = paragraphSplits[4];
                Assert.IsFalse(string.IsNullOrEmpty(fifthSplit));
                var fifthSplitTrimmed = fifthSplit.Trim();
                Assert.IsFalse(string.IsNullOrEmpty(fifthSplitTrimmed));

                
                var paragraph = await ParagraphApi.ParagraphCreateFromSplitAsync(dbContextFactory,
                    fifthSplitTrimmed, page, 4, language);


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
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void ParagraphExamplePullRandomByFlashCardIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var languageUserId = CommonFunctions.GetSpanishLanguageUserId(dbContextFactory);
            Guid flashCardId = CommonFunctions.GetFlashCard1Id(dbContextFactory, languageUserId);
            AvailableLanguageCode uiLangugaeCode = AvailableLanguageCode.EN_US;
            int numTries = 5;
            HashSet<string> examples = new();
            HashSet<string> translations = new();
            int translationDuplicates = 0;
            int exampleDuplicates = 0;
            float threshold = 0.75f;
            /* 
             * 
             * we run this many times to test that we get a random 
             * paragraphtranslation back 
             * 
             */
            for (int i = 0; i < numTries; i++)
            {
                var result = ParagraphApi.ParagraphExamplePullRandomByFlashCardId(
                        dbContextFactory, flashCardId, uiLangugaeCode);
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
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var languageUserId = CommonFunctions.GetSpanishLanguageUserId(dbContextFactory);
            Guid flashCardId = CommonFunctions.GetFlashCard1Id(dbContextFactory, languageUserId);
            AvailableLanguageCode uiLangugaeCode = AvailableLanguageCode.EN_US;
            int numTries = 5;
            HashSet<string> examples = new();
            HashSet<string> translations = new();
            int translationDuplicates = 0;
            int exampleDuplicates = 0;
            float threshold = 0.75f;
            /* 
             * 
             * we run this many times to test that we get a random 
             * paragraphtranslation back 
             * 
             */
            for (int i = 0; i < numTries; i++)
            {
                var result = await ParagraphApi.ParagraphExamplePullRandomByFlashCardIdAsync(
                        dbContextFactory, flashCardId, uiLangugaeCode);
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
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid paragraphId = CommonFunctions.GetParagraph14706Id(dbContextFactory);
            string expectedValue = "África del Norte, África septentrional o África norsahariana (a veces llamada África Blanca) es la subregión norte de África. Está compuesta por cinco países: Argelia, Egipto, Libia, Marruecos y Túnez. Además, incluye a la República Árabe Saharaui Democrática (que es un Estado con reconocimiento limitado) y otros territorios que dependen de países externos a la subregión: Canarias, Ceuta y Melilla (que dependen de España), Madeira (de Portugal) y Lampedusa e Linosa (de Italia).";
            string actualValue = ParagraphApi.ParagraphReadAllText(dbContextFactory, paragraphId);
            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod()]
        public async Task ParagraphReadAllTextAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid paragraphId = CommonFunctions.GetParagraph14706Id(dbContextFactory);
            string expectedValue = "África del Norte, África septentrional o África norsahariana (a veces llamada África Blanca) es la subregión norte de África. Está compuesta por cinco países: Argelia, Egipto, Libia, Marruecos y Túnez. Además, incluye a la República Árabe Saharaui Democrática (que es un Estado con reconocimiento limitado) y otros territorios que dependen de países externos a la subregión: Canarias, Ceuta y Melilla (que dependen de España), Madeira (de Portugal) y Lampedusa e Linosa (de Italia).";
            string actualValue = await ParagraphApi.ParagraphReadAllTextAsync(dbContextFactory, paragraphId);
            Assert.AreEqual(expectedValue, actualValue);            
        }


        [TestMethod()]
        public void ParagraphsCreateFromPageTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    dbContextFactory, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = language.Id,
                    Language = language,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, dbContextFactory);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, dbContextFactory);
                Assert.IsNotNull(page);

                // create the paragraphs
                var paragraphs = ParagraphApi.ParagraphsCreateFromPage(
                    dbContextFactory, page, language);
                Assert.IsNotNull(paragraphs);
                Assert.IsFalse(paragraphs.Count < 1);
                var fifthParagraph = paragraphs.Where(x => x.Ordinal == 4).FirstOrDefault();
                Assert.IsNotNull(fifthParagraph);
                Assert.IsNotNull(fifthParagraph.Sentences);
                Assert.IsFalse(fifthParagraph.Sentences.Count < 1);

                var thirdSentence = fifthParagraph.Sentences
                    .Where(s => s.Ordinal == 2)
                    .FirstOrDefault();
                Assert.IsNotNull(thirdSentence);
                Assert.IsNotNull(thirdSentence.Text);
                var actualText = thirdSentence.Text[..20];
                Assert.AreEqual(expectedText, actualText);
            }
            finally
            {
                // clean-up
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task ParagraphsCreateFromPageAsyncTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string expectedText = "Era un jardín grande";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    dbContextFactory, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = language.Id,
                    Language = language,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, dbContextFactory);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, dbContextFactory);
                Assert.IsNotNull(page);

                // create the paragraphs
                var paragraphs = await ParagraphApi.ParagraphsCreateFromPageAsync(
                    dbContextFactory, page, language);
                Assert.IsNotNull(paragraphs);
                Assert.IsFalse(paragraphs.Count < 1);
                var fifthParagraph = paragraphs.Where(x => x.Ordinal == 4).FirstOrDefault();
                Assert.IsNotNull(fifthParagraph);
                Assert.IsNotNull(fifthParagraph.Sentences);
                Assert.IsFalse(fifthParagraph.Sentences.Count < 1);

                var thirdSentence = fifthParagraph.Sentences
                    .Where(s => s.Ordinal == 2)
                    .FirstOrDefault();
                Assert.IsNotNull(thirdSentence);
                Assert.IsNotNull(thirdSentence.Text);
                var actualText = thirdSentence.Text[..20];
                Assert.AreEqual(expectedText, actualText);
            }
            finally
            {
                // clean-up
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void ParagraphsReadByPageIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetPage400Id(dbContextFactory);
            int ppOrd = 0;
            int sentenceOrdinal = 1;
            string expectedValue = "Está compuesta por cinco países: Argelia, Egipto, Libia, Marruecos y Túnez.";

            var paragraphs = ParagraphApi.ParagraphsReadByPageId(dbContextFactory, pageId);
            Assert.IsNotNull(paragraphs);
            var seventhParagraph = paragraphs.Where(x => x.Ordinal == ppOrd).FirstOrDefault();
            Assert.IsNotNull(seventhParagraph);
            Assert.IsNotNull(seventhParagraph.Id);
            var sentences = SentenceApi.SentencesReadByParagraphId(
                dbContextFactory, (Guid)seventhParagraph.Id);
            Assert.IsNotNull(sentences);
            seventhParagraph.Sentences = sentences;
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
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetPage400Id(dbContextFactory);
            int ppOrd = 0;
            int sentenceOrdinal = 1;
            string expectedValue = "Está compuesta por cinco países: Argelia, Egipto, Libia, Marruecos y Túnez.";

            var paragraphs = await ParagraphApi.ParagraphsReadByPageIdAsync(dbContextFactory, pageId);
            Assert.IsNotNull(paragraphs);
            var seventhParagraph = paragraphs.Where(x => x.Ordinal == ppOrd).FirstOrDefault();
            Assert.IsNotNull(seventhParagraph);
            Assert.IsNotNull(seventhParagraph.Id);
            var sentences = await SentenceApi.SentencesReadByParagraphIdAsync(
                dbContextFactory, (Guid)seventhParagraph.Id);
            Assert.IsNotNull(sentences);
            seventhParagraph.Sentences = sentences;
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
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid paragraphId = CommonFunctions.GetParagraph14590Id(dbContextFactory);
            var paragraph = context.Paragraphs.Where(x => x.Id == paragraphId).FirstOrDefault();
            Assert.IsNotNull(paragraph);
            AvailableLanguageCode fromCode = AvailableLanguageCode.ES;
            AvailableLanguageCode toCode = AvailableLanguageCode.EN_US;
            string expectedInput = "Había una vez una pareja que quería un hijo. La esposa, que estaba esperando, tenía antojo de una planta llamada rapunzel, que crecía en un jardín cercano perteneciente a una hechicera.";
            string expectedTranslation = "Once upon a time there was a couple who wanted a child. The wife, who was expecting, had a craving for a plant called rapunzel, which grew in a nearby garden belonging to a sorceress.";

#if FORCEDEEPLCALL
            DataCache.ParagraphTranslationDeleteByParagraphIdAndLanguageCode(
                (paragraphId, toCode), dbContextFactory);
#endif

            (string input, string output) result = ParagraphApi.ParagraphTranslate(
                dbContextFactory, paragraph, fromCode, toCode);
            string actualInput = result.input;
            string actualTranslation = result.output;

            Assert.AreEqual(expectedInput, actualInput);
            Assert.AreEqual(expectedTranslation, actualTranslation);
        }
        [TestMethod()]
        public async Task ParagraphTranslateAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid paragraphId = CommonFunctions.GetParagraph14590Id(dbContextFactory);
            var paragraph = context.Paragraphs.Where(x => x.Id == paragraphId).FirstOrDefault();
            Assert.IsNotNull(paragraph);
            AvailableLanguageCode fromCode = AvailableLanguageCode.ES;
            AvailableLanguageCode toCode = AvailableLanguageCode.EN_US;
            string expectedInput = "Había una vez una pareja que quería un hijo. La esposa, que estaba esperando, tenía antojo de una planta llamada rapunzel, que crecía en un jardín cercano perteneciente a una hechicera.";
            string expectedTranslation = "Once upon a time there was a couple who wanted a child. The wife, who was expecting, had a craving for a plant called rapunzel, which grew in a nearby garden belonging to a sorceress.";

#if FORCEDEEPLCALL
            await DataCache.ParagraphTranslationDeleteByParagraphIdAndLanguageCodeAsync(
                (paragraphId, toCode), dbContextFactory);
#endif

            (string input, string output) result = await ParagraphApi.ParagraphTranslateAsync(
                dbContextFactory, paragraph, fromCode, toCode);
            string actualInput = result.input;
            string actualTranslation = result.output;

            Assert.AreEqual(expectedInput, actualInput);
            Assert.AreEqual(expectedTranslation, actualTranslation);
        }


        [TestMethod()]
        public void PotentialParagraphsFromSplitTextTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            string expectedText = "Cada tarde, después de la escuela. Los niños iban a jugar al jardín del gigante. Era un jardín grande y bonito.";

            try
            {
                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = learningLanguage.Id,
                    Language = learningLanguage,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, dbContextFactory);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, dbContextFactory);
                Assert.IsNotNull(page);

                // create the paragraph splits
                var paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(
                   dbContextFactory, TestConstants.NewPageText, learningLanguage);
                Assert.IsNotNull(paragraphSplits);
                Assert.IsFalse(paragraphSplits.Length < 1);
                var fifthSplit = paragraphSplits[4];
                Assert.IsFalse(string.IsNullOrEmpty(fifthSplit));
                var fifthSplitTrimmed = fifthSplit.Trim();
                Assert.IsFalse(string.IsNullOrEmpty(fifthSplitTrimmed));

                Assert.AreEqual(expectedText, fifthSplitTrimmed);
            }
            finally
            {
                // clean-up
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task PotentialParagraphsFromSplitTextTestAsync()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            string expectedText = "Cada tarde, después de la escuela. Los niños iban a jugar al jardín del gigante. Era un jardín grande y bonito.";

            try
            {
                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = learningLanguage.Id,
                    Language = learningLanguage,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, dbContextFactory);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, dbContextFactory);
                Assert.IsNotNull(page);

                // create the paragraph splits
                var paragraphSplits = await ParagraphApi.PotentialParagraphsSplitFromTextAsync(
                   dbContextFactory, TestConstants.NewPageText, learningLanguage);
                Assert.IsNotNull(paragraphSplits);
                Assert.IsFalse(paragraphSplits.Length < 1);
                var fifthSplit = paragraphSplits[4];
                Assert.IsFalse(string.IsNullOrEmpty(fifthSplit));
                var fifthSplitTrimmed = fifthSplit.Trim();
                Assert.IsFalse(string.IsNullOrEmpty(fifthSplitTrimmed));

                Assert.AreEqual(expectedText, fifthSplitTrimmed);
            }
            finally
            {
                // clean-up
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }
    }
}