﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using Microsoft.EntityFrameworkCore;
using Model.Enums;
using System.Net;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class TokenApiTests
    {
        [TestMethod()]
        public void TokenCreateTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            string sentenceText = "Cada tarde, después de la escuela.";
            string wordText = "cada";
            string wordDisplay = "Cada";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    dbContextFactory, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.Id);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (Guid)language.Id,
                    //Language = language,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, dbContextFactory);
                Assert.IsNotNull(book); Assert.IsNotNull(book.Id);
                bookId = (Guid)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    //Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, dbContextFactory);
                Assert.IsNotNull(page); Assert.IsNotNull(page.Id);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    //Page = page,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, dbContextFactory);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.Id);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphId = paragraph.Id,
                    //Paragraph = paragraph,
                    Text = sentenceText,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                sentence = DataCache.SentenceCreate(sentence, dbContextFactory);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.Id);

                // read the word to get the Word.Id
                var word = WordApi.WordReadByLanguageIdAndText(dbContextFactory, (Guid)language.Id, wordText);
                Assert.IsNotNull(word); Assert.IsNotNull(word.Id);

                // now create the tokens
                var token = TokenApi.TokenCreate(dbContextFactory, wordDisplay, sentence, 0, word);

                // token assertions
                Assert.IsNotNull(token);
                Assert.IsNotNull(token.Id);
                Assert.AreEqual(wordDisplay, token.Display);
            }
            finally
            {
                // clean-up
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task TokenCreateAsyncTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            
            string sentenceText = "Cada tarde, después de la escuela.";
            string wordText = "cada";
            string wordDisplay = "Cada";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    dbContextFactory, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.Id);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (Guid)language.Id,
                    //Language = language,
                    Id = Guid.NewGuid()
                };
                book = await DataCache.BookCreateAsync(book, dbContextFactory);
                Assert.IsNotNull(book); Assert.IsNotNull(book.Id);
                bookId = (Guid)book.Id;
                
                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    //Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = await DataCache.PageCreateAsync(page, dbContextFactory);
                Assert.IsNotNull(page); Assert.IsNotNull(page.Id);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    //Page = page,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                paragraph = await DataCache.ParagraphCreateAsync(paragraph, dbContextFactory);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.Id);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphId = paragraph.Id,
                    //Paragraph = paragraph,
                    Text = sentenceText,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                sentence = await DataCache.SentenceCreateAsync(sentence, dbContextFactory);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.Id);

                // read the word to get the Word.Id
                var word = await WordApi.WordReadByLanguageIdAndTextAsync(dbContextFactory, (Guid)language.Id, wordText);
                Assert.IsNotNull(word); Assert.IsNotNull(word.Id);

                // now create the tokens
                var token = await TokenApi.TokenCreateAsync(dbContextFactory, wordDisplay, sentence, 0, word);
                
                // token assertions
                Assert.IsNotNull(token); 
                Assert.IsNotNull(token.Id);
                Assert.AreEqual(wordDisplay, token.Display);
            }
            finally
            {
                // clean-up
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void TokenGetChildObjectsTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            Guid tokenId = CommonFunctions.GetToken94322Id(dbContextFactory);
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(dbContextFactory);
            string expectedWordText = "había";
            Guid expectedWordUserId = CommonFunctions.GetWordUser(dbContextFactory, languageUserId, expectedWordText);

            var tokenAndChildren = TokenApi.TokenGetChildObjects(
                dbContextFactory, tokenId, languageUserId);

            Assert.IsNotNull(tokenAndChildren.t);
            Assert.IsNotNull(tokenAndChildren.t.Word);
            Assert.AreEqual(expectedWordText, tokenAndChildren.t.Word.Text);
            Assert.IsNotNull(tokenAndChildren.wu);
            Assert.AreEqual(expectedWordUserId, tokenAndChildren.wu.Id);
        }
        [TestMethod()]
        public async Task TokenGetChildObjectsAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid tokenId = CommonFunctions.GetToken94322Id(dbContextFactory);
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(dbContextFactory);
            string expectedWordText = "había";
            Guid expectedWordUserId = CommonFunctions.GetWordUser(dbContextFactory, languageUserId, expectedWordText);

            var tokenAndChildren = await TokenApi.TokenGetChildObjectsAsync(
                dbContextFactory, tokenId, languageUserId);
            Assert.IsNotNull(tokenAndChildren.t);
            Assert.IsNotNull(tokenAndChildren.t.Word);
            Assert.AreEqual(expectedWordText, tokenAndChildren.t.Word.Text);
            Assert.IsNotNull(tokenAndChildren.wu);
            Assert.AreEqual(expectedWordUserId, tokenAndChildren.wu.Id);
        }


        [TestMethod()]
        public void TokensAndWordsReadBySentenceIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid sentenceId = CommonFunctions.GetSentence24379Id(dbContextFactory);
            int expectedCount = 9;
            string expectedText = "pareja";

            var tokens = TokenApi.TokensAndWordsReadBySentenceId(
                dbContextFactory, sentenceId);
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
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            Guid sentenceId = CommonFunctions.GetSentence24379Id(dbContextFactory);
            int expectedCount = 9;
            string expectedText = "pareja";

            var tokens = await TokenApi.TokensAndWordsReadBySentenceIdAsync(
                dbContextFactory, sentenceId);
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
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            string sentenceText = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWordText = "después";
            string expectedDisplay = "después ";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    dbContextFactory, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.Id);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = language.Id,
                    //Language = language,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, dbContextFactory);
                Assert.IsNotNull(book); Assert.IsNotNull(book.Id);
                bookId = (Guid)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    //Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = DataCache.PageCreate(page, dbContextFactory);
                Assert.IsNotNull(page); Assert.IsNotNull(page.Id);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    //Page = page,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, dbContextFactory);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.Id);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphId = paragraph.Id,
                    //Paragraph = paragraph,
                    Text = sentenceText,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                sentence = DataCache.SentenceCreate(sentence, dbContextFactory);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.Id);

                // now create the tokens
                var tokens = TokenApi.TokensCreateFromSentence(
                    dbContextFactory, sentence, language);

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
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task TokensCreateFromSentenceAsyncTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string sentenceText = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWordText = "después";
            string expectedDisplay = "después ";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    dbContextFactory, TestConstants.NewBookLanguageCode);
                Assert.IsNotNull(language); Assert.IsNotNull(language.Id);

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = language.Id,
                    //Language = language,
                    Id = Guid.NewGuid()
                };
                book = await DataCache.BookCreateAsync(book, dbContextFactory);
                Assert.IsNotNull(book); Assert.IsNotNull(book.Id);
                bookId = (Guid)book.Id;

                // create an empty page
                Page? page = new()
                {
                    BookId = book.Id,
                    //Book = book,
                    Ordinal = 1,
                    OriginalText = TestConstants.NewPageText,
                    Id = Guid.NewGuid()
                };
                page = await DataCache.PageCreateAsync(page, dbContextFactory);
                Assert.IsNotNull(page); Assert.IsNotNull(page.Id);

                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    //Page = page,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                paragraph = await DataCache.ParagraphCreateAsync(paragraph, dbContextFactory);
                Assert.IsNotNull(paragraph); Assert.IsNotNull(paragraph.Id);

                // create an empty sentence
                Sentence? sentence = new()
                {
                    ParagraphId = paragraph.Id,
                    //Paragraph = paragraph,
                    Text = sentenceText,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                sentence = await DataCache.SentenceCreateAsync(sentence, dbContextFactory);
                Assert.IsNotNull(sentence); Assert.IsNotNull(sentence.Id);

                // now create the tokens
                var tokens = await TokenApi.TokensCreateFromSentenceAsync(
                    dbContextFactory, sentence, language);

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
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void TokensReadByPageIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetRapunzelPage1Id(dbContextFactory);
            Guid sentenceId = CommonFunctions.GetSentence24380Id(dbContextFactory);
            int tokenOrdinal = 4;
            int expectedCount = 195;
            string expectedDisplay = "esperando, ";

            var tokens = TokenApi.TokensReadByPageId(dbContextFactory, pageId);
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
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetRapunzelPage1Id(dbContextFactory);
            Guid sentenceId = CommonFunctions.GetSentence24380Id(dbContextFactory);
            int tokenOrdinal = 4;
            int expectedCount = 195;
            string expectedDisplay = "esperando, ";

            var tokens = await TokenApi.TokensReadByPageIdAsync(dbContextFactory, pageId);
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