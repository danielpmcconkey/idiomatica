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
    public class TokenApiTests
    {
        [TestMethod()]
        public void TokenCreateTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string sentenceText = "Cada tarde, después de la escuela.";
            string wordText = "cada";
            string wordDisplay = "Cada";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.UniqueKey);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = (Guid)language.UniqueKey,
                    Language = language,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                Assert.IsNotNull(book); Assert.IsNotNull(book.UniqueKey);
                bookId = (Guid)book.UniqueKey;

                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                Assert.IsNotNull(page); Assert.IsNotNull(page.UniqueKey);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageKey = page.UniqueKey,
                    Page = page,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.UniqueKey);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphKey = paragraph.UniqueKey,
                    Paragraph = paragraph,
                    Text = sentenceText,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                sentence = DataCache.SentenceCreate(sentence, context);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.UniqueKey);

                // read the word to get the Word.UniqueKey
                var word = WordApi.WordReadByLanguageIdAndText(context, (Guid)language.UniqueKey, wordText);
                Assert.IsNotNull(word); Assert.IsNotNull(word.UniqueKey);

                // now create the tokens
                var token = TokenApi.TokenCreate(context, wordDisplay, sentence, 0, word);

                // token assertions
                Assert.IsNotNull(token);
                Assert.IsNotNull(token.UniqueKey);
                Assert.AreEqual(wordDisplay, token.Display);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task TokenCreateAsyncTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string sentenceText = "Cada tarde, después de la escuela.";
            string wordText = "cada";
            string wordDisplay = "Cada";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.UniqueKey);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = (Guid)language.UniqueKey,
                    Language = language,
                    UniqueKey = Guid.NewGuid()
                };
                book = await DataCache.BookCreateAsync(book, context);
                Assert.IsNotNull(book); Assert.IsNotNull(book.UniqueKey);
                bookId = (Guid)book.UniqueKey;
                
                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = await DataCache.PageCreateAsync(page, context);
                Assert.IsNotNull(page); Assert.IsNotNull(page.UniqueKey);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageKey = page.UniqueKey,
                    Page = page,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = await DataCache.ParagraphCreateAsync(paragraph, context);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.UniqueKey);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphKey = paragraph.UniqueKey,
                    Paragraph = paragraph,
                    Text = sentenceText,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                sentence = await DataCache.SentenceCreateAsync(sentence, context);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.UniqueKey);

                // read the word to get the Word.UniqueKey
                var word = await WordApi.WordReadByLanguageIdAndTextAsync(context, (Guid)language.UniqueKey, wordText);
                Assert.IsNotNull(word); Assert.IsNotNull(word.UniqueKey);

                // now create the tokens
                var token = await TokenApi.TokenCreateAsync(context, wordDisplay, sentence, 0, word);
                
                // token assertions
                Assert.IsNotNull(token); 
                Assert.IsNotNull(token.UniqueKey);
                Assert.AreEqual(wordDisplay, token.Display);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void TokenGetChildObjectsTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid tokenId = CommonFunctions.GetToken94322Id(context);
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserKey(context);
            string expectedWordText = "había";
            Guid expectedWordUserId = CommonFunctions.GetWordUser(context, languageUserId, expectedWordText);

            var tokenAndChildren = TokenApi.TokenGetChildObjects(
                context, tokenId, languageUserId);

            Assert.IsNotNull(tokenAndChildren.t);
            Assert.IsNotNull(tokenAndChildren.t.Word);
            Assert.AreEqual(expectedWordText, tokenAndChildren.t.Word.Text);
            Assert.IsNotNull(tokenAndChildren.wu);
            Assert.AreEqual(expectedWordUserId, tokenAndChildren.wu.UniqueKey);
        }
        [TestMethod()]
        public async Task TokenGetChildObjectsAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid tokenId = CommonFunctions.GetToken94322Id(context);
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserKey(context);
            string expectedWordText = "había";
            Guid expectedWordUserId = CommonFunctions.GetWordUser(context, languageUserId, expectedWordText);

            var tokenAndChildren = await TokenApi.TokenGetChildObjectsAsync(
                context, tokenId, languageUserId);
            Assert.IsNotNull(tokenAndChildren.t);
            Assert.IsNotNull(tokenAndChildren.t.Word);
            Assert.AreEqual(expectedWordText, tokenAndChildren.t.Word.Text);
            Assert.IsNotNull(tokenAndChildren.wu);
            Assert.AreEqual(expectedWordUserId, tokenAndChildren.wu.UniqueKey);
        }


        [TestMethod()]
        public void TokensAndWordsReadBySentenceIdTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid sentenceId = CommonFunctions.GetSentence24379Id(context);
            int expectedCount = 9;
            string expectedText = "pareja";

            var tokens = TokenApi.TokensAndWordsReadBySentenceId(
                context, sentenceId);
            Assert.IsNotNull(tokens);
            Assert.AreEqual(expectedCount, tokens.Count);
            var fifthToken = tokens.Where(x => x.Ordinal == 4).FirstOrDefault();
            Assert.IsNotNull(fifthToken);
            Assert.IsNotNull(fifthToken.Word);
            Assert.AreEqual(expectedText, fifthToken.Word.Text);
        }
        [TestMethod()]
        public async Task TokensAndWordsReadBySentenceIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid sentenceId = CommonFunctions.GetSentence24379Id(context);
            int expectedCount = 9;
            string expectedText = "pareja";

            var tokens = await TokenApi.TokensAndWordsReadBySentenceIdAsync(
                context, sentenceId);
            Assert.IsNotNull(tokens);
            Assert.AreEqual(expectedCount, tokens.Count);
            var fifthToken = tokens.Where(x => x.Ordinal == 4).FirstOrDefault();
            Assert.IsNotNull(fifthToken);
            Assert.IsNotNull(fifthToken.Word);
            Assert.AreEqual(expectedText, fifthToken.Word.Text);
        }


        [TestMethod()]
        public void TokensCreateFromSentenceTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string sentenceText = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWordText = "después";
            string expectedDisplay = "después ";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.UniqueKey);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = language.UniqueKey,
                    Language = language,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                Assert.IsNotNull(book); Assert.IsNotNull(book.UniqueKey);
                bookId = (Guid)book.UniqueKey;

                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, context);
                Assert.IsNotNull(page); Assert.IsNotNull(page.UniqueKey);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageKey = page.UniqueKey,
                    Page = page,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.UniqueKey);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphKey = paragraph.UniqueKey,
                    Paragraph = paragraph,
                    Text = sentenceText,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                sentence = DataCache.SentenceCreate(sentence, context);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.UniqueKey);

                // now create the tokens
                var tokens = TokenApi.TokensCreateFromSentence(
                    context, sentence, language);

                // token assertions
                Assert.IsNotNull(tokens);
                Assert.AreEqual(expectedCount, tokens.Count);

                var thirdToken = tokens.Where(x => x.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(thirdToken);
                Assert.AreEqual(expectedDisplay, thirdToken.Display);
                Assert.IsNotNull(thirdToken.Word);
                Assert.AreEqual(expectedWordText, thirdToken.Word.Text);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task TokensCreateFromSentenceAsyncTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string sentenceText = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWordText = "después";
            string expectedDisplay = "después ";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.UniqueKey);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageKey = language.UniqueKey,
                    Language = language,
                    UniqueKey = Guid.NewGuid()
                };
                book = await DataCache.BookCreateAsync(book, context);
                Assert.IsNotNull(book); Assert.IsNotNull(book.UniqueKey);
                bookId = (Guid)book.UniqueKey;

                // create an empty page
                Page? page = new()
                {
                    BookKey = book.UniqueKey,
                    Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = await DataCache.PageCreateAsync(page, context);
                Assert.IsNotNull(page); Assert.IsNotNull(page.UniqueKey);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageKey = page.UniqueKey,
                    Page = page,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = await DataCache.ParagraphCreateAsync(paragraph, context);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.UniqueKey);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphKey = paragraph.UniqueKey,
                    Paragraph = paragraph,
                    Text = sentenceText,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                sentence = await DataCache.SentenceCreateAsync(sentence, context);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.UniqueKey);

                // now create the tokens
                var tokens = await TokenApi.TokensCreateFromSentenceAsync(
                    context, sentence, language);

                // token assertions
                Assert.IsNotNull(tokens);
                Assert.AreEqual(expectedCount, tokens.Count);

                var thirdToken = tokens.Where(x => x.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(thirdToken);
                Assert.AreEqual(expectedDisplay, thirdToken.Display);
                Assert.IsNotNull(thirdToken.Word);
                Assert.AreEqual(expectedWordText, thirdToken.Word.Text);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void TokensReadByPageIdTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid pageId = CommonFunctions.GetPage378Id(context);
            Guid sentenceId = CommonFunctions.GetSentence24380Id(context);
            int tokenOrdinal = 4;
            int expectedCount = 240;
            string expectedDisplay = "esperando,";

            var tokens = TokenApi.TokensReadByPageId(context, pageId);
            Assert.IsNotNull(tokens);
            Assert.AreEqual(expectedCount, tokens.Count);
            var targetToken = tokens
                .Where(x => x.SentenceKey == sentenceId && x.Ordinal == tokenOrdinal)
                .FirstOrDefault();
            Assert.IsNotNull(targetToken);
            Assert.AreEqual(expectedDisplay, targetToken.Display);
        }
        [TestMethod()]
        public async Task TokensReadByPageIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid pageId = CommonFunctions.GetPage378Id(context);
            Guid sentenceId = CommonFunctions.GetSentence24380Id(context);
            int tokenOrdinal = 4;
            int expectedCount = 240;
            string expectedDisplay = "esperando,";

            var tokens = await TokenApi.TokensReadByPageIdAsync(context, pageId);
            Assert.IsNotNull(tokens);
            Assert.AreEqual(expectedCount, tokens.Count);
            var targetToken = tokens
                .Where(x => x.SentenceKey == sentenceId && x.Ordinal == tokenOrdinal)
                .FirstOrDefault();
            Assert.IsNotNull(targetToken);
            Assert.AreEqual(expectedDisplay, targetToken.Display);
        }
    }
}