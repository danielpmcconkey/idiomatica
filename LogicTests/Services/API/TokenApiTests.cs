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
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string sentenceText = "Cada tarde, después de la escuela.";
            string wordText = "cada";
            string wordDisplay = "Cada";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.Id);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                Assert.IsNotNull(book); Assert.IsNotNull(book.Id);
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
                Assert.IsNotNull(page); Assert.IsNotNull(page.Id);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.Id);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphId = paragraph.Id,
                    Text = sentenceText,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                sentence = DataCache.SentenceCreate(sentence, context);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.Id);

                // read the word to get the Word.Id
                var word = WordApi.WordReadByLanguageIdAndText(context, (int)language.Id, wordText);
                Assert.IsNotNull(word); Assert.IsNotNull(word.Id);

                // now create the tokens
                var token = TokenApi.TokenCreate(context, wordDisplay, (int)sentence.Id, 0, (int)word.Id);

                // token assertions
                Assert.IsNotNull(token);
                Assert.IsNotNull(token.Id);
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
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string sentenceText = "Cada tarde, después de la escuela.";
            string wordText = "cada";
            string wordDisplay = "Cada";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.Id);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = await DataCache.BookCreateAsync(book, context);
                Assert.IsNotNull(book); Assert.IsNotNull(book.Id);
                bookId = (int)book.Id;
                
                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = await DataCache.PageCreateAsync(page, context);
                Assert.IsNotNull(page); Assert.IsNotNull(page.Id);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = await DataCache.ParagraphCreateAsync(paragraph, context);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.Id);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphId = paragraph.Id,
                    Text = sentenceText,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                sentence = await DataCache.SentenceCreateAsync(sentence, context);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.Id);

                // read the word to get the Word.Id
                var word = WordApi.WordReadByLanguageIdAndTextAsync(context, (int)language.Id, wordText);
                Assert.IsNotNull(word); Assert.IsNotNull(word.Id);

                // now create the tokens
                var token = await TokenApi.TokenCreateAsync(context, wordDisplay, (int)sentence.Id, 0, (int)word.Id);
                
                // token assertions
                Assert.IsNotNull(token); 
                Assert.IsNotNull(token.Id);
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
            int tokenId = 4361;
            int languageUserId = 1;
            string expectedWordText = "mido";
            int expectedWordUserId = 1;

            var tokenAndChildren = TokenApi.TokenGetChildObjects(
                context, tokenId, languageUserId);

            Assert.IsNotNull(tokenAndChildren.t);
            Assert.IsNotNull(tokenAndChildren.t.Word);
            Assert.AreEqual(expectedWordText, tokenAndChildren.t.Word.Text);
            Assert.IsNotNull(tokenAndChildren.wu);
            Assert.AreEqual(expectedWordUserId, tokenAndChildren.wu.Id);
        }
        [TestMethod()]
        public async Task TokenGetChildObjectsAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            int tokenId = 4361;
            int languageUserId = 1;
            string expectedWordText = "mido";
            int expectedWordUserId = 1;

            var tokenAndChildren = await TokenApi.TokenGetChildObjectsAsync(
                context, tokenId, languageUserId);
            Assert.IsNotNull(tokenAndChildren.t);
            Assert.IsNotNull(tokenAndChildren.t.Word);
            Assert.AreEqual(expectedWordText, tokenAndChildren.t.Word.Text);
            Assert.IsNotNull(tokenAndChildren.wu);
            Assert.AreEqual(expectedWordUserId, tokenAndChildren.wu.Id);
        }


        [TestMethod()]
        public void TokensAndWordsReadBySentenceIdTest()
        {
            var context = CommonFunctions.CreateContext();
            int sentenceId = 14187;
            int expectedCount = 14;
            string expectedText = "metros";

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
            int sentenceId = 14187;
            int expectedCount = 14;
            string expectedText = "metros";

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
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string sentenceText = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWordText = "después";
            string expectedDisplay = "después ";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.Id);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                Assert.IsNotNull(book); Assert.IsNotNull(book.Id);
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
                Assert.IsNotNull(page); Assert.IsNotNull(page.Id);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.Id);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphId = paragraph.Id,
                    Text = sentenceText,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                sentence = DataCache.SentenceCreate(sentence, context);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.Id);

                // now create the tokens
                var tokens = TokenApi.TokensCreateFromSentence(
                    context, (int)sentence.Id, (int)language.Id);

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
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string sentenceText = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWordText = "después";
            string expectedDisplay = "después ";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.Id);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (int)language.Id,
                    UniqueKey = Guid.NewGuid()
                };
                book = await DataCache.BookCreateAsync(book, context);
                Assert.IsNotNull(book); Assert.IsNotNull(book.Id);
                bookId = (int)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    UniqueKey = Guid.NewGuid()
                };
                page = await DataCache.PageCreateAsync(page, context);
                Assert.IsNotNull(page); Assert.IsNotNull(page.Id);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = await DataCache.ParagraphCreateAsync(paragraph, context);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.Id);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphId = paragraph.Id,
                    Text = sentenceText,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                sentence = await DataCache.SentenceCreateAsync(sentence, context);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.Id);

                // now create the tokens
                var tokens = await TokenApi.TokensCreateFromSentenceAsync(
                    context, (int)sentence.Id, (int)language.Id);

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
            int pageId = 3;
            int sentenceId = 14181;
            int tokenOrdinal = 11;
            int expectedCount = 247;
            string expectedDisplay = "Londres. ";

            var tokens = TokenApi.TokensReadByPageId(context, pageId);
            Assert.IsNotNull(tokens);
            Assert.AreEqual(expectedCount, tokens.Count);
            var targetToken = tokens
                .Where(x => x.SentenceId == sentenceId && x.Ordinal == tokenOrdinal)
                .FirstOrDefault();
            Assert.IsNotNull(targetToken);
            Assert.AreEqual(expectedDisplay, targetToken.Display);
        }
        [TestMethod()]
        public async Task TokensReadByPageIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            int pageId = 3;
            int sentenceId = 14181;
            int tokenOrdinal = 11;
            int expectedCount = 247;
            string expectedDisplay = "Londres. ";

            var tokens = await TokenApi.TokensReadByPageIdAsync(context, pageId);
            Assert.IsNotNull(tokens);
            Assert.AreEqual(expectedCount, tokens.Count);
            var targetToken = tokens
                .Where(x => x.SentenceId == sentenceId && x.Ordinal == tokenOrdinal)
                .FirstOrDefault();
            Assert.IsNotNull(targetToken);
            Assert.AreEqual(expectedDisplay, targetToken.Display);
        }
    }
}