using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Logic.Telemetry;
using Model;
using System.Net;
using Model.DAL;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class WordApiTests
    {
        [TestMethod()]
        public void WordCreateTest()
        {
            var word = new Word();

            var context = CommonFunctions.CreateContext();
            Guid languageId = 2;
            string guid = Guid.NewGuid().ToString();
            string text = guid.ToLower();
            string romanization = guid;
            try
            {
                word = WordApi.WordCreate(context, languageId, text, romanization);

                Assert.IsNotNull(word);
                Assert.IsNotNull(word.UniqueKey);
                Assert.AreEqual(text, word.TextLowerCase, text);
                Assert.AreEqual(romanization, word.Romanization);
            }
            finally
            {
                // clean-up
                if (word is not null && word.UniqueKey is not null)
                {
                    DataCache.WordDeleteById((Guid)word.UniqueKey, context);
                }
            }
        }
        [TestMethod()]
        public async Task WordCreateAsyncTest()
        {
            var word = new Word();
            var context = CommonFunctions.CreateContext();

            Guid languageId = 2;
            string guid = Guid.NewGuid().ToString();
            string text = guid.ToLower();
            string romanization = guid;

            try
            {
                word = await WordApi.WordCreateAsync(context, languageId, text, romanization);
                // assert
                Assert.IsNotNull(word);
                Assert.IsNotNull(word.UniqueKey);
                Assert.AreEqual(text, word.TextLowerCase, text);
                Assert.AreEqual(romanization, word.Romanization);
            }
            finally
            {
                // clean-up
                if (word is not null && word.UniqueKey is not null)
                {
                    DataCache.WordDeleteById((Guid)word.UniqueKey, context);
                }
            }
        }


        [TestMethod()]
        public void WordGetByIdTest()
        {
            var context = CommonFunctions.CreateContext();

            Guid wordId = 35;
            string expectedText = "cuerpo";
            Guid expectedLanguageId = 1;

            var word = WordApi.WordGetById(context, wordId);

            Assert.IsNotNull(word);
            Assert.AreEqual(wordId, word.UniqueKey);
            Assert.AreEqual(expectedText, word.TextLowerCase);
            Assert.AreEqual(expectedLanguageId, word.LanguageKey);
        }
        [TestMethod()]
        public async Task WordGetByIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();

            Guid wordId = 35;
            string expectedText = "cuerpo";
            Guid expectedLanguageId = 1;

            var word = await WordApi.WordGetByIdAsync(context, wordId);

            Assert.IsNotNull(word);
            Assert.AreEqual(wordId, word.UniqueKey);
            Assert.AreEqual(expectedText, word.TextLowerCase);
            Assert.AreEqual(expectedLanguageId, word.LanguageKey);
        }


        [TestMethod()]
        public void WordReadByLanguageIdAndTextTest()
        {
            var context = CommonFunctions.CreateContext();

            Guid expectedWordId = 35;
            string text = "cuerpo";
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);

            var word = WordApi.WordReadByLanguageIdAndText(context, languageId, text);

            Assert.IsNotNull(word);
            Assert.AreEqual(expectedWordId, word.UniqueKey);
        }
        [TestMethod()]
        public async Task WordReadByLanguageIdAndTextAsyncTest()
        {
            var context = CommonFunctions.CreateContext();

            Guid expectedWordId = 35;
            string text = "cuerpo";
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);

            var word = await WordApi.WordReadByLanguageIdAndTextAsync(context, languageId, text);

            Assert.IsNotNull(word);
            Assert.AreEqual(expectedWordId, word.UniqueKey);
        }


        [TestMethod()]
        public void WordsCreateOrderedFromSentenceIdTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string sentence1 = "Cada tarde, después de la escuela.";
            Guid expectedCount = 6;
            string expectedWord = "después";
            int wordOrdinalToCheck = 2;

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

                // create sentence
                var sentence1created = SentenceApi.SentenceCreate(
                    context, sentence1, (Guid)language.UniqueKey, 0, (Guid)paragraph.UniqueKey);
                if (sentence1created is null || sentence1created.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }


                var wordOrderPair = WordApi.WordsCreateOrderedFromSentenceId(
                    context, (Guid)language.UniqueKey, (Guid)sentence1created.UniqueKey);
                int actualCount = wordOrderPair.Count;
                var checkedWord = wordOrderPair.Where(x => x.ordinal == wordOrdinalToCheck).FirstOrDefault();
                if (checkedWord.word is null || checkedWord.word.TextLowerCase is null)
                { ErrorHandler.LogAndThrow(); return; }
                string actualWord = checkedWord.word.TextLowerCase;

                // assert
                Assert.AreEqual(expectedCount, actualCount);
                Assert.AreEqual(expectedWord, actualWord);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task WordsCreateOrderedFromSentenceIdAsyncTest()
        {
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string sentence1 = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWord = "después";
            int wordOrdinalToCheck = 2;

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

                // create sentence
                var sentence1created = await SentenceApi.SentenceCreateAsync(
                    context, sentence1, (Guid)language.UniqueKey, 0, (Guid)paragraph.UniqueKey);
                if (sentence1created is null || sentence1created.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }


                var wordOrderPair = await WordApi.WordsCreateOrderedFromSentenceIdAsync(
                    context, (Guid)language.UniqueKey, (Guid)sentence1created.UniqueKey);
                int actualCount = wordOrderPair.Count;
                var checkedWord = wordOrderPair.Where(x => x.ordinal == wordOrdinalToCheck).FirstOrDefault();
                if (checkedWord.word is null || checkedWord.word.TextLowerCase is null)
                { ErrorHandler.LogAndThrow(); return; }
                string actualWord = checkedWord.word.TextLowerCase;

                // assert
                Assert.AreEqual(expectedCount, actualCount);
                Assert.AreEqual(expectedWord, actualWord);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void WordsDictReadByPageIdTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid pageId = 378;
            int expectedCount = 122;
            string wordToCheck = "nombre";
            Guid expectedId = 93;

            var dict = WordApi.WordsDictReadByPageId(context, pageId);
            if (dict is null)
            { ErrorHandler.LogAndThrow(); return; }
            int actualCount = dict.Count;
            var actualWord = dict[wordToCheck];
            if (actualWord is null || actualWord.UniqueKey is null)
            { ErrorHandler.LogAndThrow(); return; }
            Guid actualId = (Guid)actualWord.UniqueKey;

            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedCount, actualCount);
        }
        [TestMethod()]
        public async Task WordsDictReadByPageIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid pageId = 378;
            int expectedCount = 122;
            string wordToCheck = "nombre";
            Guid expectedId = 93;

            var dict = await WordApi.WordsDictReadByPageIdAsync(context, pageId);
            if (dict is null)
            { ErrorHandler.LogAndThrow(); return; }
            int actualCount = dict.Count;
            var actualWord = dict[wordToCheck];
            if (actualWord is null || actualWord.UniqueKey is null)
            { ErrorHandler.LogAndThrow(); return; }
            Guid actualId = (Guid)actualWord.UniqueKey;

            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedCount, actualCount);
        }


        [TestMethod()]
        public void WordsGetListOfReadCountTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            //using var transaction = context.Database.BeginTransaction();
            Guid language1 = 1;
            Guid language2 = 2;
            Guid bookIdSpanish = 6;
            Guid bookIdEnglish = 22;
            int countRows1Expected = 1;
            int countRows2Expected = 2;
            int countSpanishWords1Expected = 0;
            int countSpanishWords2Expected = 253;
            int countEnglishWords2Expected = 208;

            try
            {
                /*
                 * create a user, languageUser, bookUser
                 * run the WordsGetListOfReadCount query
                 * check that there's only 1 language
                 * check that the total count is 0
                 * add a second languageUser and bookUser
                 * read the first page of both
                 * re-run the WordsGetListOfReadCount query
                 * check that we now have 2 rows
                 * check the counts
                 * */

                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUser1 = LanguageUserApi.LanguageUserCreate(context, language1, (Guid)user.UniqueKey);
                if (languageUser1 is null) ErrorHandler.LogAndThrow();

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdSpanish, (Guid)user.UniqueKey);
                if (bookUser1 is null || bookUser1.UniqueKey is null || bookUser1.LanguageUserKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // pull the count
                var list1 = WordApi.WordsGetListOfReadCount(context, (Guid)user.UniqueKey);
                if (list1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                int countRows1Actual = list1.Count;
                int countSpanishWords1Actual = list1.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;

                // add the second language and bookUser
                var languageUser2 = LanguageUserApi.LanguageUserCreate(context, language2, (Guid)user.UniqueKey);
                if (languageUser2 is null) ErrorHandler.LogAndThrow();

                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdEnglish, (Guid)user.UniqueKey);
                if (bookUser2 is null || bookUser2.UniqueKey is null || bookUser2.LanguageUserKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // read the first page of book 1
                var firstPageBook1 = PageApi.PageReadFirstByBookId(context, bookIdSpanish);
                if (firstPageBook1 is null || firstPageBook1.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (Guid)firstPageBook1.UniqueKey, (Guid)user.UniqueKey);
                if (pageUser1 is null || pageUser1.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser1.UniqueKey);
                PageUserApi.PageUserMarkAsRead(context, (Guid)pageUser1.UniqueKey);

                // read the first page of book 2
                var firstPageBook2 = PageApi.PageReadFirstByBookId(context, bookIdEnglish);
                if (firstPageBook2 is null || firstPageBook2.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (Guid)firstPageBook2.UniqueKey, (Guid)user.UniqueKey);
                if (pageUser2 is null || pageUser2.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser2.UniqueKey);
                PageUserApi.PageUserMarkAsRead(context, (Guid)pageUser2.UniqueKey);

                // re-run the word count query
                var list2 = WordApi.WordsGetListOfReadCount(context, (Guid)user.UniqueKey);
                if (list2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                int countRows2Actual = list2.Count;
                int countSpanishWords2Actual = list2.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;
                int countEnglishWords2Actual = list2.Where(x => x.language == "English").FirstOrDefault().wordCount;

                // assert
                Assert.AreEqual(countRows1Expected, countRows1Actual);
                Assert.AreEqual(countRows2Expected, countRows2Actual);
                Assert.AreEqual(countSpanishWords1Expected, countSpanishWords1Actual);
                Assert.AreEqual(countSpanishWords2Expected, countSpanishWords2Actual);
                Assert.AreEqual(countEnglishWords2Expected, countEnglishWords2Actual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task WordsGetListOfReadCountAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            int language1 = 1;
            Guid language2 = 2;
            Guid bookIdSpanish = 6;
            Guid bookIdEnglish = 22;
            int countRows1Expected = 1;
            int countRows2Expected = 2;
            int countSpanishWords1Expected = 0;
            int countSpanishWords2Expected = 253;
            int countEnglishWords2Expected = 208;

            try
            {
                /*
                 * create a user, languageUser, bookUser
                 * run the WordsGetListOfReadCount query
                 * check that there's only 1 language
                 * check that the total count is 0
                 * add a second languageUser and bookUser
                 * read the first page of both
                 * re-run the WordsGetListOfReadCount query
                 * check that we now have 2 rows
                 * check the counts
                 * */

                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;
                var languageUser1 = LanguageUserApi.LanguageUserCreate(context, language1, (Guid)user.UniqueKey);
                if (languageUser1 is null) ErrorHandler.LogAndThrow();

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdSpanish, (Guid)user.UniqueKey);
                if (bookUser1 is null || bookUser1.UniqueKey is null || bookUser1.LanguageUserKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // pull the count
                var list1 = await WordApi.WordsGetListOfReadCountAsync(context, (Guid)user.UniqueKey);
                if (list1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                Guid countRows1Actual = list1.Count;
                int countSpanishWords1Actual = list1.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;

                // add the second language and bookUser
                var languageUser2 = LanguageUserApi.LanguageUserCreate(context, language2, (Guid)user.UniqueKey);
                if (languageUser2 is null) ErrorHandler.LogAndThrow();

                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdEnglish, (Guid)user.UniqueKey);
                if (bookUser2 is null || bookUser2.UniqueKey is null || bookUser2.LanguageUserKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // read the first page of book 1
                var firstPageBook1 = await PageApi.PageReadFirstByBookIdAsync(context, bookIdSpanish);
                if (firstPageBook1 is null || firstPageBook1.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (Guid)firstPageBook1.UniqueKey, (Guid)user.UniqueKey);
                if (pageUser1 is null || pageUser1.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser1.UniqueKey);
                await PageUserApi.PageUserMarkAsReadAsync(context, (Guid)pageUser1.UniqueKey);

                // read the first page of book 2
                var firstPageBook2 = await PageApi.PageReadFirstByBookIdAsync(context, bookIdEnglish);
                if (firstPageBook2 is null || firstPageBook2.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (Guid)firstPageBook2.UniqueKey, (Guid)user.UniqueKey);
                if (pageUser2 is null || pageUser2.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser2.UniqueKey);
                await PageUserApi.PageUserMarkAsReadAsync(context, (Guid)pageUser2.UniqueKey);

                // re-run the word count query
                var list2 = await WordApi.WordsGetListOfReadCountAsync(context, (Guid)user.UniqueKey);
                if (list2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                int countRows2Actual = list2.Count;
                int countSpanishWords2Actual = list2.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;
                int countEnglishWords2Actual = list2.Where(x => x.language == "English").FirstOrDefault().wordCount;

                // assert
                Assert.AreEqual(countRows1Expected, countRows1Actual);
                Assert.AreEqual(countRows2Expected, countRows2Actual);
                Assert.AreEqual(countSpanishWords1Expected, countSpanishWords1Actual);
                Assert.AreEqual(countSpanishWords2Expected, countSpanishWords2Actual);
                Assert.AreEqual(countEnglishWords2Expected, countEnglishWords2Actual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }

        [TestMethod()]
        public void WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdReadTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid language = 1;
            int countRowsExpected = 126;
            string wordToCheck = "volvió";
            int countTranslationsExpected = 3;
            string expectedTranslation1 = "he  returned: preterite \"él\" conjugation of volver";
            string expectedTranslation2 = "she  returned: preterite \"ella\" conjugation of volver";
            string expectedTranslation3 = "you  returned: preterite \"usted\" conjugation of volver";

            try
            {
                /*
                 * create a book, user, languageUser, bookUser
                 * run the API method
                 * 
                 * */

                var newBook = BookApi.BookCreateAndSave(
                    context,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText
                    );
                Assert.IsNotNull(newBook);
                Assert.IsNotNull(newBook.UniqueKey);
                bookId = (Guid)newBook.UniqueKey;

                var userService = CommonFunctions.CreateUserService();
                Assert.IsNotNull(userService);
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.UniqueKey);
                Assert.IsTrue(user.UniqueKey >= 1);
                userId = (Guid)user.UniqueKey;
                var languageUser = LanguageUserApi.LanguageUserCreate(context, language, (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.UniqueKey);

                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId, (Guid)user.UniqueKey);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.UniqueKey);
                Assert.IsNotNull(bookUser.LanguageUserKey);

                var page = newBook.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(page);
                Assert.IsNotNull(page.UniqueKey);
                var dict = WordApi
                    .WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
                        context, (Guid)page.UniqueKey, (Guid)languageUser.UniqueKey);

                Assert.IsNotNull(dict);
                Assert.AreEqual(countRowsExpected, dict.Count);

                Assert.IsTrue(dict.TryGetValue(wordToCheck, out var wordFound));
                Assert.IsNotNull(wordFound);

                var translations = wordFound.WordTranslations
                    .OrderBy(x => x.Ordinal)
                    .ToList();

                Assert.IsNotNull(translations);
                Assert.AreEqual(countTranslationsExpected, translations.Count);

                Assert.AreEqual(expectedTranslation1, translations[0].Translation);
                Assert.AreEqual(expectedTranslation2, translations[1].Translation);
                Assert.AreEqual(expectedTranslation3, translations[2].Translation);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }

        [TestMethod()]
        public async Task WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdReadAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid language = 1;
            int countRowsExpected = 126;
            string wordToCheck = "volvió";
            int countTranslationsExpected = 3;
            string expectedTranslation1 = "he  returned: preterite \"él\" conjugation of volver";
            string expectedTranslation2 = "she  returned: preterite \"ella\" conjugation of volver";
            string expectedTranslation3 = "you  returned: preterite \"usted\" conjugation of volver";

            try
            {
                /*
                 * create a book, user, languageUser, bookUser
                 * run the API method
                 * 
                 * */

                var newBook = await BookApi.BookCreateAndSaveAsync(
                    context,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText
                    );
                Assert.IsNotNull(newBook);
                Assert.IsNotNull(newBook.UniqueKey);
                bookId = (Guid)newBook.UniqueKey;

                var userService = CommonFunctions.CreateUserService();
                Assert.IsNotNull(userService);
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, language, (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.UniqueKey);

                var bookUser = await OrchestrationApi.OrchestrateBookUserCreationAndSubProcessesAsync(
                    context, bookId, (Guid)user.UniqueKey);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.UniqueKey);
                Assert.IsNotNull(bookUser.LanguageUserKey);

                var page = newBook.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(page);
                Assert.IsNotNull(page.UniqueKey);
                var dict = await WordApi
                    .WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdReadAsync(
                        context, (Guid)page.UniqueKey, (Guid)languageUser.UniqueKey);

                Assert.IsNotNull(dict);
                Assert.AreEqual(countRowsExpected, dict.Count);

                Assert.IsTrue(dict.TryGetValue(wordToCheck, out var wordFound));
                Assert.IsNotNull(wordFound);

                var translations = wordFound.WordTranslations
                    .OrderBy(x => x.Ordinal)
                    .ToList();

                Assert.IsNotNull(translations);
                Assert.AreEqual(countTranslationsExpected, translations.Count);

                Assert.AreEqual(expectedTranslation1, translations[0].Translation);
                Assert.AreEqual(expectedTranslation2, translations[1].Translation);
                Assert.AreEqual(expectedTranslation3, translations[2].Translation);
            }
            finally
            {
                // clean-up
                await CommonFunctions.CleanUpUserAsync(userId, context);
                await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }

        [TestMethod()]
        public void WordTranslationsReadByWordIdTest()
        {
            Assert.Fail();
        }
    }
}