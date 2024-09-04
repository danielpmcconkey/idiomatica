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
            int languageId = 2;
            string guid = Guid.NewGuid().ToString();
            string text = guid.ToLower();
            string romanization = guid;
            try
            {
                word = WordApi.WordCreate(context, languageId, text, romanization);

                Assert.IsNotNull(word);
                Assert.IsNotNull(word.Id);
                Assert.AreEqual(text, word.TextLowerCase, text);
                Assert.AreEqual(romanization, word.Romanization);
            }
            finally
            {
                // clean-up
                if (word is not null && word.Id is not null)
                {
                    DataCache.WordDeleteById((int)word.Id, context);
                }
            }
        }
        [TestMethod()]
        public async Task WordCreateAsyncTest()
        {
            var word = new Word();
            var context = CommonFunctions.CreateContext();

            int languageId = 2;
            string guid = Guid.NewGuid().ToString();
            string text = guid.ToLower();
            string romanization = guid;

            try
            {
                word = await WordApi.WordCreateAsync(context, languageId, text, romanization);
                // assert
                Assert.IsNotNull(word);
                Assert.IsNotNull(word.Id);
                Assert.AreEqual(text, word.TextLowerCase, text);
                Assert.AreEqual(romanization, word.Romanization);
            }
            finally
            {
                // clean-up
                if (word is not null && word.Id is not null)
                {
                    DataCache.WordDeleteById((int)word.Id, context);
                }
            }
        }


        [TestMethod()]
        public void WordGetByIdTest()
        {
            var context = CommonFunctions.CreateContext();

            int wordId = 35;
            string expectedText = "cuerpo";
            int expectedLanguageId = 1;

            var word = WordApi.WordGetById(context, wordId);

            Assert.IsNotNull(word);
            Assert.AreEqual(wordId, word.Id);
            Assert.AreEqual(expectedText, word.TextLowerCase);
            Assert.AreEqual(expectedLanguageId, word.LanguageId);
        }
        [TestMethod()]
        public async Task WordGetByIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();

            int wordId = 35;
            string expectedText = "cuerpo";
            int expectedLanguageId = 1;

            var word = await WordApi.WordGetByIdAsync(context, wordId);

            Assert.IsNotNull(word);
            Assert.AreEqual(wordId, word.Id);
            Assert.AreEqual(expectedText, word.TextLowerCase);
            Assert.AreEqual(expectedLanguageId, word.LanguageId);
        }


        [TestMethod()]
        public void WordReadByLanguageIdAndTextTest()
        {
            var context = CommonFunctions.CreateContext();

            int expectedWordId = 35;
            string text = "cuerpo";
            int languageId = 1;

            var word = WordApi.WordReadByLanguageIdAndText(context, languageId, text);

            Assert.IsNotNull(word);
            Assert.AreEqual(expectedWordId, word.Id);
        }
        [TestMethod()]
        public async Task WordReadByLanguageIdAndTextAsyncTest()
        {
            var context = CommonFunctions.CreateContext();

            int expectedWordId = 35;
            string text = "cuerpo";
            int languageId = 1;

            var word = await WordApi.WordReadByLanguageIdAndTextAsync(context, languageId, text);

            Assert.IsNotNull(word);
            Assert.AreEqual(expectedWordId, word.Id);
        }


        [TestMethod()]
        public void WordsCreateOrderedFromSentenceIdTest()
        {
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string sentence1 = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWord = "después";
            int wordOrdinalToCheck = 2;

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
                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                if (paragraph is null || paragraph.Id is null || paragraph.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }

                // create sentence
                var sentence1created = SentenceApi.SentenceCreate(
                    context, sentence1, (int)language.Id, 0, (int)paragraph.Id);
                if (sentence1created is null || sentence1created.Id is null)
                { ErrorHandler.LogAndThrow(); return; }


                var wordOrderPair = WordApi.WordsCreateOrderedFromSentenceId(
                    context, (int)language.Id, (int)sentence1created.Id);
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
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string sentence1 = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWord = "después";
            int wordOrdinalToCheck = 2;

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
                // create an empty paragraph
                Paragraph? paragraph = new()
                {
                    PageId = page.Id,
                    Ordinal = 1,
                    UniqueKey = Guid.NewGuid()
                };
                paragraph = DataCache.ParagraphCreate(paragraph, context);
                if (paragraph is null || paragraph.Id is null || paragraph.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }

                // create sentence
                var sentence1created = await SentenceApi.SentenceCreateAsync(
                    context, sentence1, (int)language.Id, 0, (int)paragraph.Id);
                if (sentence1created is null || sentence1created.Id is null)
                { ErrorHandler.LogAndThrow(); return; }


                var wordOrderPair = await WordApi.WordsCreateOrderedFromSentenceIdAsync(
                    context, (int)language.Id, (int)sentence1created.Id);
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
            int pageId = 3;
            int expectedCount = 142;
            string wordToCheck = "ciencia";
            int expectedId = 28;

            var dict = WordApi.WordsDictReadByPageId(context, pageId);
            if (dict is null)
            { ErrorHandler.LogAndThrow(); return; }
            int actualCount = dict.Count;
            var actualWord = dict[wordToCheck];
            if (actualWord is null || actualWord.Id is null)
            { ErrorHandler.LogAndThrow(); return; }
            int actualId = (int)actualWord.Id;

            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedCount, actualCount);
        }
        [TestMethod()]
        public async Task WordsDictReadByPageIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            int pageId = 3;
            int expectedCount = 142;
            string wordToCheck = "ciencia";
            int expectedId = 28;

            var dict = await WordApi.WordsDictReadByPageIdAsync(context, pageId);
            if (dict is null)
            { ErrorHandler.LogAndThrow(); return; }
            int actualCount = dict.Count;
            var actualWord = dict[wordToCheck];
            if (actualWord is null || actualWord.Id is null)
            { ErrorHandler.LogAndThrow(); return; }
            int actualId = (int)actualWord.Id;

            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedCount, actualCount);
        }


        [TestMethod()]
        public void WordsGetListOfReadCountTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            //using var transaction = context.Database.BeginTransaction();
            int language1 = 1;
            int language2 = 2;
            int bookIdSpanish = 6;
            int bookIdEnglish = 22;
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
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                var languageUser1 = LanguageUserApi.LanguageUserCreate(context, language1, (int)user.Id);
                if (languageUser1 is null) ErrorHandler.LogAndThrow();

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdSpanish, (int)user.Id);
                if (bookUser1 is null || bookUser1.Id is null || bookUser1.LanguageUserId is null)
                { ErrorHandler.LogAndThrow(); return; }

                // pull the count
                var list1 = WordApi.WordsGetListOfReadCount(context, (int)user.Id);
                if (list1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                int countRows1Actual = list1.Count;
                int countSpanishWords1Actual = list1.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;

                // add the second language and bookUser
                var languageUser2 = LanguageUserApi.LanguageUserCreate(context, language2, (int)user.Id);
                if (languageUser2 is null) ErrorHandler.LogAndThrow();

                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdEnglish, (int)user.Id);
                if (bookUser2 is null || bookUser2.Id is null || bookUser2.LanguageUserId is null)
                { ErrorHandler.LogAndThrow(); return; }

                // read the first page of book 1
                var firstPageBook1 = PageApi.PageReadFirstByBookId(context, bookIdSpanish);
                if (firstPageBook1 is null || firstPageBook1.Id is null || firstPageBook1.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPageBook1.Id, (int)user.Id);
                if (pageUser1 is null || pageUser1.Id is null || pageUser1.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser1.Id);
                PageUserApi.PageUserMarkAsRead(context, (int)pageUser1.Id);

                // read the first page of book 2
                var firstPageBook2 = PageApi.PageReadFirstByBookId(context, bookIdEnglish);
                if (firstPageBook2 is null || firstPageBook2.Id is null || firstPageBook2.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPageBook2.Id, (int)user.Id);
                if (pageUser2 is null || pageUser2.Id is null || pageUser2.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser2.Id);
                PageUserApi.PageUserMarkAsRead(context, (int)pageUser2.Id);

                // re-run the word count query
                var list2 = WordApi.WordsGetListOfReadCount(context, (int)user.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int language1 = 1;
            int language2 = 2;
            int bookIdSpanish = 6;
            int bookIdEnglish = 22;
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
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;
                var languageUser1 = LanguageUserApi.LanguageUserCreate(context, language1, (int)user.Id);
                if (languageUser1 is null) ErrorHandler.LogAndThrow();

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdSpanish, (int)user.Id);
                if (bookUser1 is null || bookUser1.Id is null || bookUser1.LanguageUserId is null)
                { ErrorHandler.LogAndThrow(); return; }

                // pull the count
                var list1 = await WordApi.WordsGetListOfReadCountAsync(context, (int)user.Id);
                if (list1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                int countRows1Actual = list1.Count;
                int countSpanishWords1Actual = list1.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;

                // add the second language and bookUser
                var languageUser2 = LanguageUserApi.LanguageUserCreate(context, language2, (int)user.Id);
                if (languageUser2 is null) ErrorHandler.LogAndThrow();

                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdEnglish, (int)user.Id);
                if (bookUser2 is null || bookUser2.Id is null || bookUser2.LanguageUserId is null)
                { ErrorHandler.LogAndThrow(); return; }

                // read the first page of book 1
                var firstPageBook1 = await PageApi.PageReadFirstByBookIdAsync(context, bookIdSpanish);
                if (firstPageBook1 is null || firstPageBook1.Id is null || firstPageBook1.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPageBook1.Id, (int)user.Id);
                if (pageUser1 is null || pageUser1.Id is null || pageUser1.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser1.Id);
                await PageUserApi.PageUserMarkAsReadAsync(context, (int)pageUser1.Id);

                // read the first page of book 2
                var firstPageBook2 = await PageApi.PageReadFirstByBookIdAsync(context, bookIdEnglish);
                if (firstPageBook2 is null || firstPageBook2.Id is null || firstPageBook2.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPageBook2.Id, (int)user.Id);
                if (pageUser2 is null || pageUser2.Id is null || pageUser2.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser2.Id);
                await PageUserApi.PageUserMarkAsReadAsync(context, (int)pageUser2.Id);

                // re-run the word count query
                var list2 = await WordApi.WordsGetListOfReadCountAsync(context, (int)user.Id);
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
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int language = 1;
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
                Assert.IsNotNull(newBook.Id);
                bookId = (int)newBook.Id;

                var userService = CommonFunctions.CreateUserService();
                Assert.IsNotNull(userService);
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                Assert.IsTrue(user.Id >= 1);
                userId = (int)user.Id;
                var languageUser = LanguageUserApi.LanguageUserCreate(context, language, (int)user.Id);
                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);

                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId, (int)user.Id);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsNotNull(bookUser.LanguageUserId);

                var page = newBook.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(page);
                Assert.IsNotNull(page.Id);
                var dict = WordApi
                    .WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
                        context, (int)page.Id, (int)languageUser.Id);

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
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int language = 1;
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
                Assert.IsNotNull(newBook.Id);
                bookId = (int)newBook.Id;

                var userService = CommonFunctions.CreateUserService();
                Assert.IsNotNull(userService);
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                Assert.IsTrue(user.Id >= 1);
                userId = (int)user.Id;
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, language, (int)user.Id);
                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);

                var bookUser = await OrchestrationApi.OrchestrateBookUserCreationAndSubProcessesAsync(
                    context, bookId, (int)user.Id);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsNotNull(bookUser.LanguageUserId);

                var page = newBook.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(page);
                Assert.IsNotNull(page.Id);
                var dict = await WordApi
                    .WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdReadAsync(
                        context, (int)page.Id, (int)languageUser.Id);

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