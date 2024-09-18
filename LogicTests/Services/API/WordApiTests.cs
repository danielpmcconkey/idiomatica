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
using Microsoft.EntityFrameworkCore;
using Model.Enums;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class WordApiTests
    {
        [TestMethod()]
        public void WordCreateTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();


            Word? word = null;

            Language language = CommonFunctions.GetSpanishLanguage(context);
            string guid = Guid.NewGuid().ToString();
            string text = guid.ToLower();
            string romanization = guid;
            try
            {
                word = WordApi.WordCreate(context, language, text, romanization);

                Assert.IsNotNull(word);
                Assert.IsNotNull(word.Id);
                Assert.AreEqual(text, word.TextLowerCase, text);
                Assert.AreEqual(romanization, word.Romanization);
            }
            finally
            {
                // clean-up
                if (word is not null)
                {
                    DataCache.WordDeleteById((Guid)word.Id, context);
                }
            }
        }
        [TestMethod()]
        public async Task WordCreateAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            

            Word? word = null;

            Language language = CommonFunctions.GetSpanishLanguage(context);
            string guid = Guid.NewGuid().ToString();
            string text = guid.ToLower();
            string romanization = guid;

            try
            {
                word = await WordApi.WordCreateAsync(context, language, text, romanization);
                // assert
                Assert.IsNotNull(word);
                Assert.IsNotNull(word.Id);
                Assert.AreEqual(text, word.TextLowerCase, text);
                Assert.AreEqual(romanization, word.Romanization);
            }
            finally
            {
                // clean-up
                if (word is not null)
                {
                    DataCache.WordDeleteById((Guid)word.Id, context);
                }
            }
        }


        [TestMethod()]
        public void WordGetByIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            string expectedText = "cuerpo";
            Guid expectedLanguageId = CommonFunctions.GetSpanishLanguageId(context);
            Guid wordId = CommonFunctions.GetWordId(context, expectedText, expectedLanguageId);

            var word = WordApi.WordGetById(context, wordId);

            Assert.IsNotNull(word);
            Assert.AreEqual(wordId, word.Id);
            Assert.AreEqual(expectedText, word.TextLowerCase);
            Assert.AreEqual(expectedLanguageId, word.LanguageId);
        }
        [TestMethod()]
        public async Task WordGetByIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            string expectedText = "cuerpo";
            Guid expectedLanguageId = CommonFunctions.GetSpanishLanguageId(context);
            Guid wordId = CommonFunctions.GetWordId(context, expectedText, expectedLanguageId);

            var word = await WordApi.WordGetByIdAsync(context, wordId);

            Assert.IsNotNull(word);
            Assert.AreEqual(wordId, word.Id);
            Assert.AreEqual(expectedText, word.TextLowerCase);
            Assert.AreEqual(expectedLanguageId, word.LanguageId);
        }


        [TestMethod()]
        public void WordReadByLanguageIdAndTextTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            string text = "cuerpo";
            Guid languageId = CommonFunctions.GetSpanishLanguageId(context);
            Guid expectedWordId = CommonFunctions.GetWordId(context, text, languageId);

            var word = WordApi.WordReadByLanguageIdAndText(context, languageId, text);

            Assert.IsNotNull(word);
            Assert.AreEqual(expectedWordId, word.Id);
        }
        [TestMethod()]
        public async Task WordReadByLanguageIdAndTextAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            
            string text = "cuerpo";
            Guid languageId = CommonFunctions.GetSpanishLanguageId(context);
            Guid expectedWordId = CommonFunctions.GetWordId(context, text, languageId);

            var word = await WordApi.WordReadByLanguageIdAndTextAsync(context, languageId, text);

            Assert.IsNotNull(word);
            Assert.AreEqual(expectedWordId, word.Id);
        }


        [TestMethod()]
        public void WordsCreateOrderedFromSentenceIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid? bookId = null;
            string sentence1 = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWord = "después";
            int wordOrdinalToCheck = 2;

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
                    LanguageId = language.Id,
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

                // create sentence
                var sentence1created = SentenceApi.SentenceCreate(
                    context, sentence1, language, 0, paragraph);
                if (sentence1created is null)
                { ErrorHandler.LogAndThrow(); return; }


                var wordOrderPair = WordApi.WordsCreateOrderedFromSentenceId(
                    context, language, sentence1created);
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
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task WordsCreateOrderedFromSentenceIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid? bookId = null;
            string sentence1 = "Cada tarde, después de la escuela.";
            int expectedCount = 6;
            string expectedWord = "después";
            int wordOrdinalToCheck = 2;

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
                    LanguageId = language.Id,
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

                // create sentence
                var sentence1created = await SentenceApi.SentenceCreateAsync(
                    context, sentence1, language, 0, paragraph);
                if (sentence1created is null)
                { ErrorHandler.LogAndThrow(); return; }


                var wordOrderPair = await WordApi.WordsCreateOrderedFromSentenceIdAsync(
                    context, language, sentence1created);
                int actualCount = wordOrderPair.Count;
                var checkedWord = wordOrderPair.Where(x => x.ordinal == wordOrdinalToCheck)
                    .FirstOrDefault();
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
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void WordsDictReadByPageIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;
            string wordToCheck = "nombre";
            Guid languageKey = CommonFunctions.GetSpanishLanguageId(context);
            Guid expectedId = CommonFunctions.GetWordId(context, wordToCheck, languageKey);

            var dict = WordApi.WordsDictReadByPageId(context, pageId);
            if (dict is null)
            { ErrorHandler.LogAndThrow(); return; }
            int actualCount = dict.Count;
            var actualWord = dict[wordToCheck];
            if (actualWord is null)
            { ErrorHandler.LogAndThrow(); return; }
            Guid actualId = (Guid)actualWord.Id;

            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedCount, actualCount);
        }
        [TestMethod()]
        public async Task WordsDictReadByPageIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;
            string wordToCheck = "nombre";
            Guid languageKey = CommonFunctions.GetSpanishLanguageId(context);
            Guid expectedId = CommonFunctions.GetWordId(context, wordToCheck, languageKey);

            var dict = await WordApi.WordsDictReadByPageIdAsync(context, pageId);
            if (dict is null)
            { ErrorHandler.LogAndThrow(); return; }
            int actualCount = dict.Count;
            var actualWord = dict[wordToCheck];
            if (actualWord is null)
            { ErrorHandler.LogAndThrow(); return; }
            Guid actualId = (Guid)actualWord.Id;

            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedCount, actualCount);
        }


        [TestMethod()]
        public void WordsGetListOfReadCountTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();

            Language language1 = CommonFunctions.GetSpanishLanguage(context);
            Language language2 = CommonFunctions.GetEnglishLanguage(context);
            Guid bookIdSpanish = CommonFunctions.GetBook11Id(context);
            Guid bookIdEnglish = Guid.NewGuid(); Assert.Fail(); // 22;
            int countRows1Expected = 1;
            int countRows2Expected = 2;
            int countSpanishWords1Expected = 0;
            int countSpanishWords2Expected = 240;
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

                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                var languageUser1 = LanguageUserApi.LanguageUserCreate(context, language1, user);
                if (languageUser1 is null) ErrorHandler.LogAndThrow();

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdSpanish, (Guid)user.Id);
                if (bookUser1 is null)
                { ErrorHandler.LogAndThrow(); return; }

                // pull the count
                var list1 = WordApi.WordsGetListOfReadCount(context, (Guid)user.Id);
                if (list1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                int countRows1Actual = list1.Count;
                int countSpanishWords1Actual = list1.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;

                // add the second language and bookUser
                var languageUser2 = LanguageUserApi.LanguageUserCreate(context, language2, user);
                if (languageUser2 is null) ErrorHandler.LogAndThrow();

                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdEnglish, (Guid)user.Id);
                if (bookUser2 is null)
                { ErrorHandler.LogAndThrow(); return; }

                // read the first page of book 1
                var firstPageBook1 = PageApi.PageReadFirstByBookId(context, bookIdSpanish);
                if (firstPageBook1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, firstPageBook1, user);
                if (pageUser1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser1.Id);
                PageUserApi.PageUserMarkAsRead(context, (Guid)pageUser1.Id);

                // read the first page of book 2
                var firstPageBook2 = PageApi.PageReadFirstByBookId(context, bookIdEnglish);
                if (firstPageBook2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, firstPageBook2, user);
                if (pageUser2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser2.Id);
                PageUserApi.PageUserMarkAsRead(context, (Guid)pageUser2.Id);

                // re-run the word count query
                var list2 = WordApi.WordsGetListOfReadCount(context, (Guid)user.Id);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task WordsGetListOfReadCountAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();

            Language language1 = CommonFunctions.GetSpanishLanguage(context);
            Language language2 = CommonFunctions.GetEnglishLanguage(context);
            Guid bookIdSpanish = CommonFunctions.GetBook11Id(context);
            Guid bookIdEnglish = Guid.NewGuid(); Assert.Fail(); // 22;
            int countRows1Expected = 1;
            int countRows2Expected = 2;
            int countSpanishWords1Expected = 0;
            int countSpanishWords2Expected = 240;
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

                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                var languageUser1 = LanguageUserApi.LanguageUserCreate(context, language1, user);
                if (languageUser1 is null) ErrorHandler.LogAndThrow();

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdSpanish, (Guid)user.Id);
                if (bookUser1 is null)
                { ErrorHandler.LogAndThrow(); return; }

                // pull the count
                var list1 = await WordApi.WordsGetListOfReadCountAsync(context, (Guid)user.Id);
                if (list1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                int countRows1Actual = list1.Count;
                int countSpanishWords1Actual = list1.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;

                // add the second language and bookUser
                var languageUser2 = LanguageUserApi.LanguageUserCreate(context, language2, user);
                if (languageUser2 is null) ErrorHandler.LogAndThrow();

                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdEnglish, (Guid)user.Id);
                if (bookUser2 is null)
                { ErrorHandler.LogAndThrow(); return; }

                // read the first page of book 1
                var firstPageBook1 = await PageApi.PageReadFirstByBookIdAsync(context, bookIdSpanish);
                if (firstPageBook1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, firstPageBook1, user);
                if (pageUser1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser1.Id);
                await PageUserApi.PageUserMarkAsReadAsync(context, (Guid)pageUser1.Id);

                // read the first page of book 2
                var firstPageBook2 = await PageApi.PageReadFirstByBookIdAsync(context, bookIdEnglish);
                if (firstPageBook2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, firstPageBook2, user);
                if (pageUser2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (Guid)pageUser2.Id);
                await PageUserApi.PageUserMarkAsReadAsync(context, (Guid)pageUser2.Id);

                // re-run the word count query
                var list2 = await WordApi.WordsGetListOfReadCountAsync(context, (Guid)user.Id);
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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }

        [TestMethod()]
        public void WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdReadTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);
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
                bookId = (Guid)newBook.Id;

                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, language.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsNotNull(bookUser.LanguageUserId);

                var page = newBook.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(page);
                Assert.IsNotNull(page.Id);
                var dict = WordApi
                    .WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
                        context, page.Id, languageUser.Id);

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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }

        [TestMethod()]
        public async Task WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdReadAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);
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
                bookId = (Guid)newBook.Id;

                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, language.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var bookUser = await OrchestrationApi.OrchestrateBookUserCreationAndSubProcessesAsync(
                    context, (Guid)bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsNotNull(bookUser.LanguageUserId);

                var page = newBook.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                Assert.IsNotNull(page);
                Assert.IsNotNull(page.Id);
                var dict = await WordApi
                    .WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdReadAsync(
                        context, page.Id, languageUser.Id);

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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }

        [TestMethod()]
        public void WordTranslationsReadByWordIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            var languageId = CommonFunctions.GetSpanishLanguageId(context);
            string expectedTranslation1 = "he  says: present \"él\" conjugation of decir";
            string expectedTranslation2 = "she  says: present \"ella\" conjugation of decir";
            string expectedTranslation3 = "you  say: present \"usted\" conjugation of decir";

            var wordId = CommonFunctions.GetWordIdByTextLower(context, languageId, "dice");
            Assert.IsNotNull(wordId);

            var translations = WordApi.WordTranslationsReadByWordId(context, wordId);
            Assert.IsNotNull(translations);

            var orderedTranslations = translations
                .OrderBy(x => x.Ordinal)
                .ToArray();

            string actualTranslation1 = orderedTranslations[1].Translation;
            string actualTranslation2 = orderedTranslations[2].Translation;
            string actualTranslation3 = orderedTranslations[3].Translation;

            Assert.AreEqual(expectedTranslation1, actualTranslation1);
            Assert.AreEqual(expectedTranslation2, actualTranslation2);
            Assert.AreEqual(expectedTranslation3, actualTranslation3);
        }
    }
}