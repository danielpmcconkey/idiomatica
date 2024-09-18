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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class SentenceApiTests
    {
        [TestMethod()]
        public void PotentialSentencesSplitFromTextTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string expectedValue = "Los niños iban a jugar al jardín del gigante.";

            var language = LanguageApi.LanguageReadByCode(
                context, TestConstants.NewBookLanguageCode);
            if (language is null)
            { ErrorHandler.LogAndThrow(); return; }

            var potentialSentences = SentenceApi.PotentialSentencesSplitFromText(
                context, TestConstants.NewPageText, language);

            Assert.AreEqual(expectedValue, potentialSentences[5]);
        }
        [TestMethod()]
        public async Task PotentialSentencesSplitFromTextAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string expectedValue = "Los niños iban a jugar al jardín del gigante.";
            var language = await LanguageApi.LanguageReadByCodeAsync(
                context, TestConstants.NewBookLanguageCode);
            if (language is null)
                { ErrorHandler.LogAndThrow(); return; }

            var potentialSentences = await SentenceApi.PotentialSentencesSplitFromTextAsync(
                context, TestConstants.NewPageText, language);

            Assert.AreEqual(expectedValue, potentialSentences[5]);
        }


        [TestMethod()]
        public void SentenceCreateTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string sentence1 = "Cada tarde, después de la escuela.";
            string sentence2 = "Los niños iban a jugar al jardín del gigante.";

            try
            {
                var language = LanguageApi.LanguageReadByCode(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (Guid)language.Id,
                    Language = language,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null)
                    { ErrorHandler.LogAndThrow(); return; }
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
                page = DataCache.PageCreate(page, context);
                if (page is null)
                    { ErrorHandler.LogAndThrow(); return; }
                
                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Page = page,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                if (paragraph is null)
                    { ErrorHandler.LogAndThrow(); return; }
                // create two sentences
                var sentence1created = SentenceApi.SentenceCreate(
                    context, sentence1, language, 0, paragraph);
                var sentence2created = SentenceApi.SentenceCreate(
                    context, sentence2, language, 1, paragraph);
                // read all the sentences
                var sentencesRead = SentenceApi.SentencesReadByParagraphId(
                    context, (Guid)paragraph.Id);
                if (sentencesRead is null)
                    { ErrorHandler.LogAndThrow(); return; }
                var secondSentenceRead = sentencesRead.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (secondSentenceRead is null)
                    { ErrorHandler.LogAndThrow(); return; }

                Assert.AreEqual(sentence2, secondSentenceRead.Text);

            }
            finally
            {
                // clean-up
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task SentenceCreateAsyncTest()
        {
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string sentence1 = "Cada tarde, después de la escuela.";
            string sentence2 = "Los niños iban a jugar al jardín del gigante.";

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(
                    context, TestConstants.NewBookLanguageCode);
                if (language is null)
                { ErrorHandler.LogAndThrow(); return; }
                // create an empty book
                Book? book = new()
                {
                    Title = TestConstants.NewBookTitle,
                    LanguageId = (Guid)language.Id,
                    Language = language,
                    Id = Guid.NewGuid()
                };
                book = DataCache.BookCreate(book, context);
                if (book is null)
                    { ErrorHandler.LogAndThrow(); return; }
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
                page = DataCache.PageCreate(page, context);
                if (page is null)
                    { ErrorHandler.LogAndThrow(); return; }
                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Page = page,
                    Ordinal = 1,
                    Id = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                if (paragraph is null)
                    { ErrorHandler.LogAndThrow(); return; }
                // create two sentences
                var sentence1created = await SentenceApi.SentenceCreateAsync(
                    context, sentence1, language, 0, paragraph);
                var sentence2created = await SentenceApi.SentenceCreateAsync(
                    context, sentence2, language, 1, paragraph);
                // read all the sentences
                var sentencesRead = await SentenceApi.SentencesReadByParagraphIdAsync(
                    context, (Guid)paragraph.Id);
                if(sentencesRead is null)
                    { ErrorHandler.LogAndThrow(); return; }
                var secondSentenceRead = sentencesRead.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (secondSentenceRead is null)
                    { ErrorHandler.LogAndThrow(); return; }

                Assert.AreEqual(sentence2, secondSentenceRead.Text);
            }
            finally
            {
                // clean-up
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void SentencesReadByPageIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 18;
            string expectedText = "El príncipe aprendió las palabras mágicas y visitó a Rapunzel en secreto.";


            var sentences = SentenceApi.SentencesReadByPageId(context, pageId);
            Assert.IsNotNull(sentences);
            Assert.AreEqual(expectedCount, sentences.Count);
            var targetSentence = sentences[11];
            Assert.AreEqual(expectedText, targetSentence.Text);
            
        }
        [TestMethod()]
        public async Task SentencesReadByPageIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 18;
            string expectedText = "El príncipe aprendió las palabras mágicas y visitó a Rapunzel en secreto.";
            
            var sentences = await SentenceApi.SentencesReadByPageIdAsync(context, pageId);
            Assert.IsNotNull(sentences);
            Assert.AreEqual(expectedCount, sentences.Count);
            var targetSentence = sentences[11];
            Assert.AreEqual(expectedText, targetSentence.Text);
        }


        [TestMethod()]
        public void SentencesReadByParagraphIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            Guid paragraphId = CommonFunctions.GetParagraph14594Id(context);
            int expectedCount = 3;
            int sentenceOrdinal = 2;
            string expectedText = "Rapunzel aceptó escapar con él cuando regresara.";

            var sentences = SentenceApi.SentencesReadByParagraphId(context, paragraphId);
            Assert.IsNotNull(sentences);
            Assert.AreEqual(expectedCount, sentences.Count);
            var targetSentence = sentences.Where(x => x.Ordinal == sentenceOrdinal).FirstOrDefault();
            Assert.IsNotNull(targetSentence);
            Assert.AreEqual(expectedText, targetSentence.Text);
        }

        [TestMethod()]
        public async Task SentencesReadByParagraphIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();            

            Guid paragraphId = CommonFunctions.GetParagraph14594Id(context);
            int expectedCount = 3;
            int sentenceOrdinal = 2;
            string expectedText = "Rapunzel aceptó escapar con él cuando regresara.";

            var sentences = await SentenceApi.SentencesReadByParagraphIdAsync(context, paragraphId);
            Assert.IsNotNull(sentences);
            Assert.AreEqual(expectedCount, sentences.Count);
            var targetSentence = sentences.Where(x => x.Ordinal == sentenceOrdinal).FirstOrDefault();
            Assert.IsNotNull(targetSentence);
            Assert.AreEqual(expectedText, targetSentence.Text);
        }
    }
}