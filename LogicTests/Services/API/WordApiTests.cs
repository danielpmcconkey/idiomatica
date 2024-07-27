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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class WordApiTests
    {
        [TestMethod()]
        public void WordsDictReadByPageIdTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int pageId = 3;
            int expectedCount = 142;
            string wordToCheck = "ciencia";
            int expectedId = 28;


            try
            {
                // act
                var dict = WordApi.WordsDictReadByPageId(context, pageId);
                if (dict is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int actualCount = dict.Count;
                var actualWord = dict[wordToCheck];
                if (actualWord is null || actualWord.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int actualId = (int)actualWord.Id;

                // assert
                Assert.AreEqual(expectedId, actualId);
                Assert.AreEqual(expectedCount, actualCount);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public async Task WordsDictReadByPageIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int pageId = 3;
            int expectedCount = 142;
            string wordToCheck = "ciencia";
            int expectedId = 28;


            try
            {
                // act
                var dict = await WordApi.WordsDictReadByPageIdAsync(context, pageId);
                if (dict is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int actualCount = dict.Count;
                var actualWord = dict[wordToCheck];
                if (actualWord is null || actualWord.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int actualId = (int)actualWord.Id;

                // assert
                Assert.AreEqual(expectedId, actualId);
                Assert.AreEqual(expectedCount, actualCount);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public void CreateWordTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            int languageId = 2;
            string guid = Guid.NewGuid().ToString();
            string text = guid.ToLower();
            string romanization = guid;

            try
            {
                // act
                var newWord = WordApi.WordCreate(context, languageId, text, romanization);


                // assert
                Assert.IsNotNull(newWord);
                Assert.IsNotNull(newWord.Id);
                Assert.AreEqual(text, newWord.TextLowerCase, text);
                Assert.AreEqual(romanization, newWord.Romanization);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public async Task CreateWordAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            int languageId = 2;
            string guid = Guid.NewGuid().ToString();
            string text = guid.ToLower();
            string romanization = guid;

            try
            {
                // act
                var newWord = await WordApi.WordCreateAsync(context, languageId, text, romanization);

                // assert
                Assert.IsNotNull(newWord);
                Assert.IsNotNull(newWord.Id);
                Assert.AreEqual(text, newWord.TextLowerCase, text);
                Assert.AreEqual(romanization, newWord.Romanization);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public void CreateOrderedWordsFromSentenceIdTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int sentenceId = 13956;
            int languageId = 1;
            int expectedCount = 18;
            int wordOrdinalToCheck = 15;
            string expectedWord = "impacto";



            try
            {
                // act
                var wordOrderPair = WordApi.WordsCreateOrderedFromSentenceId(
                    context, languageId, sentenceId);
                int actualCount = wordOrderPair.Count;
                var checkedWord = wordOrderPair.Where(x => x.ordinal == wordOrdinalToCheck).FirstOrDefault();
                if (checkedWord.word is null || checkedWord.word.TextLowerCase is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string actualWord = checkedWord.word.TextLowerCase;

                // assert
                Assert.AreEqual(expectedCount, actualCount);
                Assert.AreEqual(expectedWord, actualWord);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public async Task CreateOrderedWordsFromSentenceIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int sentenceId = 13956;
            int languageId = 1;
            int expectedCount = 18;
            int wordOrdinalToCheck = 15;
            string expectedWord = "impacto";



            try
            {
                // act
                var wordOrderPair = await WordApi.WordsCreateOrderedFromSentenceIdAsync(
                    context, languageId, sentenceId);
                int actualCount = wordOrderPair.Count;
                var checkedWord = wordOrderPair.Where(x => x.ordinal == wordOrdinalToCheck).FirstOrDefault();
                if (checkedWord.word is null || checkedWord.word.TextLowerCase is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                string actualWord = checkedWord.word.TextLowerCase;

                // assert
                Assert.AreEqual(expectedCount, actualCount);
                Assert.AreEqual(expectedWord, actualWord);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public void WordsGetListOfReadCountTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
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
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser1 = LanguageUserApi.LanguageUserCreate(context, language1, (int)user.Id);
                if (languageUser1 is null) ErrorHandler.LogAndThrow();

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdSpanish, (int)user.Id);
                if (bookUser1 is null || bookUser1.Id is null || bookUser1.LanguageUserId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // act

                // pull the count
                var list1 = WordApi.WordsGetListOfReadCount(context, (int)user.Id);
                if (list1 is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int countRows1Actual = list1.Count;
                int countSpanishWords1Actual = list1.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;

                // add the second language and bookUser
                var languageUser2 = LanguageUserApi.LanguageUserCreate(context, language2, (int)user.Id);
                if (languageUser2 is null) ErrorHandler.LogAndThrow();

                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdEnglish, (int)user.Id);
                if (bookUser2 is null || bookUser2.Id is null || bookUser2.LanguageUserId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // read the first page of book 1
                var firstPageBook1 = PageApi.PageReadFirstByBookId(context, bookIdSpanish);
                if (firstPageBook1 is null || firstPageBook1.Id is null || firstPageBook1.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPageBook1.Id, (int)user.Id);
                if (pageUser1 is null || pageUser1.Id is null || pageUser1.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser1.Id);
                PageUserApi.PageUserMarkAsRead(context, (int)pageUser1.Id);

                // read the first page of book 2
                var firstPageBook2 = PageApi.PageReadFirstByBookId(context, bookIdEnglish);
                if (firstPageBook2 is null || firstPageBook2.Id is null || firstPageBook2.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPageBook2.Id, (int)user.Id);
                if (pageUser2 is null || pageUser2.Id is null || pageUser2.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser2.Id);
                PageUserApi.PageUserMarkAsRead(context, (int)pageUser2.Id);

                // re-run the word count query
                var list2 = WordApi.WordsGetListOfReadCount(context, (int)user.Id);
                if (list2 is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int countRows2Actual = list2.Count;
                int countSpanishWords2Actual = list2.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;
                int countEnglishWords2Actual = list2.Where(x => x.language == "English").FirstOrDefault().wordCount;

                // act


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

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public async Task WordsGetListOfReadCountAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
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
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser1 = LanguageUserApi.LanguageUserCreate(context, language1, (int)user.Id);
                if (languageUser1 is null) ErrorHandler.LogAndThrow();

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdSpanish, (int)user.Id);
                if (bookUser1 is null || bookUser1.Id is null || bookUser1.LanguageUserId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // act

                // pull the count
                var list1 = await WordApi.WordsGetListOfReadCountAsync(context, (int)user.Id);
                if (list1 is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int countRows1Actual = list1.Count;
                int countSpanishWords1Actual = list1.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;

                // add the second language and bookUser
                var languageUser2 = LanguageUserApi.LanguageUserCreate(context, language2, (int)user.Id);
                if (languageUser2 is null) ErrorHandler.LogAndThrow();

                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookIdEnglish, (int)user.Id);
                if (bookUser2 is null || bookUser2.Id is null || bookUser2.LanguageUserId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // read the first page of book 1
                var firstPageBook1 = await PageApi.PageReadFirstByBookIdAsync(context, bookIdSpanish);
                if (firstPageBook1 is null || firstPageBook1.Id is null || firstPageBook1.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPageBook1.Id, (int)user.Id);
                if (pageUser1 is null || pageUser1.Id is null || pageUser1.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser1.Id);
                await PageUserApi.PageUserMarkAsReadAsync(context, (int)pageUser1.Id);

                // read the first page of book 2
                var firstPageBook2 = await PageApi.PageReadFirstByBookIdAsync(context, bookIdEnglish);
                if (firstPageBook2 is null || firstPageBook2.Id is null || firstPageBook2.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)firstPageBook2.Id, (int)user.Id);
                if (pageUser2 is null || pageUser2.Id is null || pageUser2.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)pageUser2.Id);
                await PageUserApi.PageUserMarkAsReadAsync(context, (int)pageUser2.Id);

                // re-run the word count query
                var list2 = await WordApi.WordsGetListOfReadCountAsync(context, (int)user.Id);
                if (list2 is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int countRows2Actual = list2.Count;
                int countSpanishWords2Actual = list2.Where(x => x.language == "Spanish").FirstOrDefault().wordCount;
                int countEnglishWords2Actual = list2.Where(x => x.language == "English").FirstOrDefault().wordCount;

                // act


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

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public void WordGetByIdTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }

        [TestMethod()]
        public void WordGetByIdAsyncTest()
        {
            Assert.Fail();
        }
    }
}