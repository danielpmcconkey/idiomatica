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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class SentenceApiTests
    {
        [TestMethod()]
        public void PotentialSentencesSplitFromTextTest()
        {
            var context = CommonFunctions.CreateContext();
            string expectedValue = "Los niños iban a jugar al jardín del gigante.";

            var language = LanguageApi.LanguageReadByCode(
                context, TestConstants.NewBookLanguageCode);
            if (language is null || language.UniqueKey is null)
            { ErrorHandler.LogAndThrow(); return; }

            var potentialSentences = SentenceApi.PotentialSentencesSplitFromText(
                context, TestConstants.NewPageText, (Guid)language.UniqueKey);

            Assert.AreEqual(expectedValue, potentialSentences[5]);
        }
        [TestMethod()]
        public async Task PotentialSentencesSplitFromTextAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            string expectedValue = "Los niños iban a jugar al jardín del gigante.";
            var language = await LanguageApi.LanguageReadByCodeAsync(
                context, TestConstants.NewBookLanguageCode);
            if (language is null || language.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }

            var potentialSentences = await SentenceApi.PotentialSentencesSplitFromTextAsync(
                context, TestConstants.NewPageText, (Guid)language.UniqueKey);

            Assert.AreEqual(expectedValue, potentialSentences[5]);
        }


        [TestMethod()]
        public void SentenceCreateTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string sentence1 = "Cada tarde, después de la escuela.";
            string sentence2 = "Los niños iban a jugar al jardín del gigante.";

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
                
                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageKey = page.UniqueKey,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                if (paragraph is null || paragraph.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                // create two sentences
                var sentence1created = SentenceApi.SentenceCreate(
                    context, sentence1, (Guid)language.UniqueKey, 0, (Guid)paragraph.UniqueKey);
                var sentence2created = SentenceApi.SentenceCreate(
                    context, sentence2, (Guid)language.UniqueKey, 1, (Guid)paragraph.UniqueKey);
                // read all the sentences
                var sentencesRead = SentenceApi.SentencesReadByParagraphId(
                    context, (Guid)paragraph.UniqueKey);
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
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task SentenceCreateAsyncTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string sentence1 = "Cada tarde, después de la escuela.";
            string sentence2 = "Los niños iban a jugar al jardín del gigante.";

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
                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageKey = page.UniqueKey,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                if (paragraph is null || paragraph.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                // create two sentences
                var sentence1created = await SentenceApi.SentenceCreateAsync(
                    context, sentence1, (Guid)language.UniqueKey, 0, (Guid)paragraph.UniqueKey);
                var sentence2created = await SentenceApi.SentenceCreateAsync(
                    context, sentence2, (Guid)language.UniqueKey, 1, (Guid)paragraph.UniqueKey);
                // read all the sentences
                var sentencesRead = await SentenceApi.SentencesReadByParagraphIdAsync(
                    context, (Guid)paragraph.UniqueKey);
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
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void SentencesReadByPageIdTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid pageId = 378;
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
            var context = CommonFunctions.CreateContext();
            Guid pageId = 378;
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
            var context = CommonFunctions.CreateContext();
            Guid paragraphId = 14594;
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
            var context = CommonFunctions.CreateContext();
            Guid paragraphId = 14594;
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