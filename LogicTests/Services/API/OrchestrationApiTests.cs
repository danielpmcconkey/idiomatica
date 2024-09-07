using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using LogicTests;
using Model;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class OrchestrationApiTests
    {
        [TestMethod()]
        public void OrchestrateBookCreationAndSubProcessesTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();

            /*
             * this is supposed to create the book, the book stats, and the 
             * bookUser, all in one call
             * */

            string totalPagesExpected = "3";
            string totalWordCountExpected = "784";
            string distinctWordCountExpected = "241";

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);

                if (user is null || user.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                    context,
                    (Guid)user.UniqueKey,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // read the book stats
                var totalPagesStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.TOTALPAGES)
                    .FirstOrDefault();
                if (totalPagesStat is null || totalPagesStat.Value is null)
                { ErrorHandler.LogAndThrow(); return; }
                string totalPagesActual = totalPagesStat.Value;

                var totalWordsStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                if (totalWordsStat is null || totalWordsStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string totalWordCountActual = totalWordsStat.Value;

                var distinctWordsStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.DISTINCTWORDCOUNT)
                    .FirstOrDefault();
                if (distinctWordsStat is null || distinctWordsStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string distinctWordCountActual = distinctWordsStat.Value;

                // pull the book user
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(context, (Guid)book.UniqueKey, (Guid)user.UniqueKey);

                // assert
                Assert.IsNotNull(book.BookStats);
                Assert.AreEqual(totalPagesExpected, totalPagesActual);
                Assert.AreEqual(totalWordCountExpected, totalWordCountActual);
                Assert.AreEqual(distinctWordCountExpected, distinctWordCountActual);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.UniqueKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateBookCreationAndSubProcessesAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();

            /*
             * this is supposed to create the book, the book stats, and the 
             * bookUser, all in one call
             * */

            string totalPagesExpected = "3";
            string totalWordCountExpected = "784";
            string distinctWordCountExpected = "241";

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);

                if (user is null || user.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                    context,
                    (Guid)user.UniqueKey,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // read the book stats
                var totalPagesStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.TOTALPAGES)
                    .FirstOrDefault();
                if (totalPagesStat is null || totalPagesStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string totalPagesActual = totalPagesStat.Value;

                var totalWordsStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                if (totalWordsStat is null || totalWordsStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string totalWordCountActual = totalWordsStat.Value;

                var distinctWordsStat = book.BookStats
                    .Where(x => x.Key != null && x.Key == AvailableBookStat.DISTINCTWORDCOUNT)
                    .FirstOrDefault();
                if (distinctWordsStat is null || distinctWordsStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string distinctWordCountActual = distinctWordsStat.Value;

                // pull the book user
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(context, (Guid)book.UniqueKey, (Guid)user.UniqueKey);

                // assert
                Assert.IsNotNull(book.BookStats);
                Assert.AreEqual(totalPagesExpected, totalPagesActual);
                Assert.AreEqual(totalWordCountExpected, totalWordCountActual);
                Assert.AreEqual(distinctWordCountExpected, distinctWordCountActual);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.UniqueKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateClearPageAndMoveTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int totalPageCount = 5;
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            int wordCount = 137;
            int paragraphCount = 1;
            string wordToLookUp = "país";
            AvailableWordUserStatus statusBeforeExpected = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfterExpected = AvailableWordUserStatus.WELLKNOWN;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    //readDataPacket.AllWordUsersInPage is null ||
                    readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.UniqueKey is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.UniqueKey is null || readDataPacket.CurrentPageUser.PageKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // check the word and its status
                Word? foundWordBefore = null;
                if (!readDataPacket.AllWordsInPage.TryGetValue(wordToLookUp, out foundWordBefore))
                { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserBefore = foundWordBefore.WordUsers
                    .Where(x => x.LanguageUserKey == languageUser.UniqueKey)
                    .FirstOrDefault();
                if (foundUserBefore is null || foundUserBefore.Status is null)
                { ErrorHandler.LogAndThrow(); return; }
                var statusBeforeActual = foundUserBefore.Status;

                // pull the current page ID before moving so you can later look up its wordUsers
                Guid origPageId = (Guid)readDataPacket.CurrentPageUser.PageKey;

                // clear the unknown words and move to the new page
                var newReadDataPacket = OrchestrationApi.OrchestrateClearPageAndMove(context, readDataPacket, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.UniqueKey is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageKey is null || newReadDataPacket.AllWordsInPage is null ||
                    //newReadDataPacket.AllWordUsersInPage is null ||
                    newReadDataPacket.Paragraphs is null)
                { ErrorHandler.LogAndThrow(); return; }

                // pull the previous page's wordUser list
                var priorPageWordDict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(context, origPageId, (Guid)user.UniqueKey);
                if (priorPageWordDict is null)
                { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserAfter = null;
                if (!priorPageWordDict.TryGetValue(wordToLookUp, out foundUserAfter))
                { ErrorHandler.LogAndThrow(); return; }
                if (foundUserAfter is null || foundUserAfter.Status is null)
                { ErrorHandler.LogAndThrow(); return; }
                var statusAfterActual = foundUserAfter.Status;

                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.AreEqual(totalPageCount, newReadDataPacket.BookTotalPageCount);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
                Assert.AreEqual(statusBeforeExpected, statusBeforeActual);
                Assert.AreEqual(statusAfterExpected, statusAfterActual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateClearPageAndMoveAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int totalPageCount = 5;
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            int wordCount = 137;
            int paragraphCount = 1;
            string wordToLookUp = "país";
            AvailableWordUserStatus statusBeforeExpected = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfterExpected = AvailableWordUserStatus.WELLKNOWN;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    //readDataPacket.AllWordUsersInPage is null ||
                    readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.UniqueKey is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.UniqueKey is null || readDataPacket.CurrentPageUser.PageKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // check the word and its status
                Word? foundWordBefore = null;
                if (!readDataPacket.AllWordsInPage.TryGetValue(wordToLookUp, out foundWordBefore))
                { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserBefore = foundWordBefore.WordUsers
                    .Where(x => x.LanguageUserKey == languageUser.UniqueKey)
                    .FirstOrDefault();
                if (foundUserBefore is null || foundUserBefore.Status is null)
                { ErrorHandler.LogAndThrow(); return; }
                var statusBeforeActual = foundUserBefore.Status;

                // pull the current page ID before moving so you can later look up its wordUsers
                Guid origPageId = (Guid)readDataPacket.CurrentPageUser.PageKey;

                // clear the unknown words and move to the new page
                var newReadDataPacket = await OrchestrationApi.OrchestrateClearPageAndMoveAsync(context, readDataPacket, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.UniqueKey is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageKey is null || newReadDataPacket.AllWordsInPage is null ||
                    //newReadDataPacket.AllWordUsersInPage is null ||
                    newReadDataPacket.Paragraphs is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // pull the previous page's wordUser list
                var priorPageWordDict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(context, origPageId, (Guid)user.UniqueKey);
                if (priorPageWordDict is null)
                    { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserAfter = null;
                if (!priorPageWordDict.TryGetValue(wordToLookUp, out foundUserAfter))
                    { ErrorHandler.LogAndThrow(); return; }
                if (foundUserAfter is null || foundUserAfter.Status is null)
                    { ErrorHandler.LogAndThrow(); return; }
                var statusAfterActual = foundUserAfter.Status;

                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.AreEqual(totalPageCount, newReadDataPacket.BookTotalPageCount);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
                Assert.AreEqual(statusBeforeExpected, statusBeforeActual);
                Assert.AreEqual(statusAfterExpected, statusAfterActual);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateFlashCardDispositionAndAdvanceTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int numNew = 4;
            int numOld = 3;
            string learningLanguageCode = "ES";
            var attemptStatus = AvailableFlashCardAttemptStatus.EASY;
            var startTime = DateTime.Now;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);
                //WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);

                // need to update the wordUsers' status so they show up on flash cardS
                var wordUsers = context.WordUsers.Where(x => x.LanguageUserKey == languageUser.UniqueKey).Take(10).ToList();
                foreach (var wordUser in wordUsers)
                {
                    if (wordUser.UniqueKey is null) continue;
                    WordUserApi.WordUserUpdate(
                        context, (Guid)wordUser.UniqueKey, AvailableWordUserStatus.LEARNING3, "test");
                }

                // create deck 1
                var dataPacket1 = OrchestrationApi.OrchestrateFlashCardDeckCreation(
                    context, userId, learningLanguageCode, numNew, numOld);

                Assert.IsNotNull(dataPacket1);
                Assert.IsNotNull(dataPacket1.Deck);
                Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
                Assert.IsNotNull(dataPacket1.CurrentCard);
                Assert.IsNotNull(dataPacket1.CurrentCard.UniqueKey);

                Guid firstCardId = (Guid)dataPacket1.CurrentCard.UniqueKey;

                // disposition the current card and advance
                var dataPacket2 = OrchestrationApi.OrchestrateFlashCardDispositionAndAdvance(
                    context, dataPacket1, attemptStatus);

                Assert.IsNotNull(dataPacket2);
                Assert.IsNotNull(dataPacket2.Deck);
                Assert.IsNotNull(dataPacket2.CurrentCard);
                Assert.IsNotNull(dataPacket2.CurrentCard.UniqueKey);

                Guid secondCardId = (Guid)dataPacket2.CurrentCard.UniqueKey;

                Assert.AreNotEqual(firstCardId, secondCardId);

                // pull card1 from the DB
                var firstCardRead = FlashCardApi.FlashCardReadById(context, firstCardId);
                // pull the attempt
                var attempt = context.FlashCardAttempts.Where(x => x.FlashCardKey == firstCardId).FirstOrDefault();

                Assert.IsNotNull(firstCardRead);
                Assert.IsNotNull(firstCardRead.Status);
                Assert.IsNotNull(firstCardRead.NextReview);
                Assert.IsTrue(firstCardRead.NextReview > startTime);
                Assert.IsNotNull(attempt);
                Assert.IsNotNull(attempt.Status);
                Assert.AreEqual(attemptStatus, attempt.Status);

            }
            finally
            {
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateFlashCardDispositionAndAdvanceAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int numNew = 4;
            int numOld = 3;
            string learningLanguageCode = "ES";
            var attemptStatus = AvailableFlashCardAttemptStatus.EASY;
            var startTime = DateTime.Now;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, userId);
                //WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);

                // need to update the wordUsers' status so they show up on flash cardS
                var wordUsers = context.WordUsers.Where(x => x.LanguageUserKey == languageUser.UniqueKey).Take(10).ToList();
                foreach (var wordUser in wordUsers)
                {
                    if (wordUser.UniqueKey is null) continue;
                    await WordUserApi.WordUserUpdateAsync(
                        context, (Guid)wordUser.UniqueKey, AvailableWordUserStatus.LEARNING3, "test");
                }

                // create deck 1
                var dataPacket1 = await OrchestrationApi.OrchestrateFlashCardDeckCreationAsync(
                    context, userId, learningLanguageCode, numNew, numOld);

                Assert.IsNotNull(dataPacket1);
                Assert.IsNotNull(dataPacket1.Deck);
                Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
                Assert.IsNotNull(dataPacket1.CurrentCard);
                Assert.IsNotNull(dataPacket1.CurrentCard.UniqueKey);

                Guid firstCardId = (Guid)dataPacket1.CurrentCard.UniqueKey;

                // disposition the current card and advance
                var dataPacket2 = await OrchestrationApi.OrchestrateFlashCardDispositionAndAdvanceAsync(
                    context, dataPacket1, attemptStatus);

                Assert.IsNotNull(dataPacket2);
                Assert.IsNotNull(dataPacket2.Deck);
                Assert.IsNotNull(dataPacket2.CurrentCard);
                Assert.IsNotNull(dataPacket2.CurrentCard.UniqueKey);

                Guid secondCardId = (Guid)dataPacket2.CurrentCard.UniqueKey;

                Assert.AreNotEqual(firstCardId, secondCardId);

                // pull card1 from the DB
                var firstCardRead = await FlashCardApi.FlashCardReadByIdAsync(context, firstCardId);
                // pull the attempt
                var attempt = context.FlashCardAttempts.Where(x => x.FlashCardKey == firstCardId).FirstOrDefault();

                Assert.IsNotNull(firstCardRead);
                Assert.IsNotNull(firstCardRead.Status);
                Assert.IsNotNull(firstCardRead.NextReview);
                Assert.IsTrue(firstCardRead.NextReview > startTime);
                Assert.IsNotNull(attempt);
                Assert.IsNotNull(attempt.Status);
                Assert.AreEqual(attemptStatus, attempt.Status);

            }
            finally
            {
                await CommonFunctions.CleanUpUserAsync(userId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateFlashCardDeckCreationTest()
        {
            //Guid userId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int numNew = 4;
            int numOld = 3;
            string learningLanguageCode = "ES";

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);
                //WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);

                // need to update the wordUsers' status so they show up on flash cardS
                var wordUsers = context.WordUsers.Where(x => x.LanguageUserKey == languageUser.UniqueKey).Take(10).ToList();
                foreach (var wordUser in wordUsers)
                {
                    if (wordUser.UniqueKey is null) continue;
                    WordUserApi.WordUserUpdate(
                        context, (Guid)wordUser.UniqueKey, AvailableWordUserStatus.LEARNING3, "test");
                }

                // create deck 1
                var dataPacket1 = OrchestrationApi.OrchestrateFlashCardDeckCreation(
                    context, userId, learningLanguageCode, numNew, numOld);

                Assert.IsNotNull(dataPacket1);
                Assert.IsNotNull(dataPacket1.Deck);
                Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
                Assert.IsNotNull(dataPacket1.CurrentCard);
                Assert.IsNotNull(dataPacket1.CurrentCard.UniqueKey);


                // manually update card 1 to do not use, and next review date in
                // the past so it would show up in the next deck creation had we
                // not set it to STOP
                Assert.IsNotNull(dataPacket1);
                var cardToUpdate1 = dataPacket1.Deck[1];
                Assert.IsNotNull(cardToUpdate1);
                Assert.IsNotNull(cardToUpdate1.UniqueKey);
                Assert.IsNotNull(cardToUpdate1.WordUserKey);
                FlashCardApi.FlashCardUpdate(context, (Guid)cardToUpdate1.UniqueKey,
                    (Guid)cardToUpdate1.WordUserKey, AvailableFlashCardStatus.DONTUSE,
                    DateTime.Now.AddHours(-1), (Guid)cardToUpdate1.UniqueKey);

                // manually update card 2's next review date so it shows up in the next deck creation
                Assert.IsNotNull(dataPacket1);
                var cardToUpdate = dataPacket1.Deck[1];
                Assert.IsNotNull(cardToUpdate);
                Assert.IsNotNull(cardToUpdate.UniqueKey);
                Assert.IsNotNull(cardToUpdate.WordUserKey);
                FlashCardApi.FlashCardUpdate(context, (Guid)cardToUpdate.UniqueKey,
                    (Guid)cardToUpdate.WordUserKey, AvailableFlashCardStatus.ACTIVE,
                    DateTime.Now.AddHours(-1), (Guid)cardToUpdate.UniqueKey);

                // create deck 2
                var dataPacket2 = OrchestrationApi.OrchestrateFlashCardDeckCreation(
                    context, userId, learningLanguageCode, numNew, numOld);

                int expectedCount = 1 // prior review card
                    + 4 // numNew
                    ;

                Assert.IsNotNull(dataPacket2);
                Assert.IsNotNull(dataPacket2.Deck);
                Assert.AreEqual(expectedCount, dataPacket2.CardCount);
                Assert.IsNotNull(dataPacket2.CurrentCard);
            }
            finally
            {
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateFlashCardDeckCreationAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int numNew = 4;
            int numOld = 3;
            string learningLanguageCode = "ES";

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, userId);
                //WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);

                // need to update the wordUsers' status so they show up on flash cardS
                var wordUsers = context.WordUsers.Where(x => x.LanguageUserKey == languageUser.UniqueKey).Take(10).ToList();
                foreach (var wordUser in wordUsers)
                {
                    if (wordUser.UniqueKey is null) continue;
                    await WordUserApi.WordUserUpdateAsync(
                        context, (Guid)wordUser.UniqueKey, AvailableWordUserStatus.LEARNING3, "test");
                }

                // create deck 1
                var dataPacket1 = await OrchestrationApi.OrchestrateFlashCardDeckCreationAsync(
                    context, userId, learningLanguageCode, numNew, numOld);

                Assert.IsNotNull(dataPacket1);
                Assert.IsNotNull(dataPacket1.Deck);
                Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
                Assert.IsNotNull(dataPacket1.CurrentCard);
                Assert.IsNotNull(dataPacket1.CurrentCard.UniqueKey);

                
                // manually update card 1 to do not use, and next review date in
                // the past so it would show up in the next deck creation had we
                // not set it to STOP
                Assert.IsNotNull(dataPacket1);
                var cardToUpdate1 = dataPacket1.Deck[1];
                Assert.IsNotNull(cardToUpdate1);
                Assert.IsNotNull(cardToUpdate1.UniqueKey);
                Assert.IsNotNull(cardToUpdate1.WordUserKey);
                await FlashCardApi.FlashCardUpdateAsync(context, (Guid)cardToUpdate1.UniqueKey,
                    (Guid)cardToUpdate1.WordUserKey, AvailableFlashCardStatus.DONTUSE,
                    DateTime.Now.AddHours(-1), (Guid)cardToUpdate1.UniqueKey);

                // manually update card 2's next review date so it shows up in the next deck creation
                Assert.IsNotNull(dataPacket1); 
                var cardToUpdate = dataPacket1.Deck[1];
                Assert.IsNotNull(cardToUpdate);
                Assert.IsNotNull(cardToUpdate.UniqueKey);
                Assert.IsNotNull(cardToUpdate.WordUserKey);
                await FlashCardApi.FlashCardUpdateAsync(context, (Guid)cardToUpdate.UniqueKey,
                    (Guid)cardToUpdate.WordUserKey, AvailableFlashCardStatus.ACTIVE,
                    DateTime.Now.AddHours(-1), (Guid)cardToUpdate.UniqueKey);

                // create deck 2
                var dataPacket2 = await OrchestrationApi.OrchestrateFlashCardDeckCreationAsync(
                    context, userId, learningLanguageCode, numNew, numOld);

                int expectedCount = 1 // prior review card
                    + 4 // numNew
                    ;

                Assert.IsNotNull(dataPacket2);
                Assert.IsNotNull(dataPacket2.Deck);
                Assert.AreEqual(expectedCount, dataPacket2.CardCount);
                Assert.IsNotNull(dataPacket2.CurrentCard);
            }
            finally
            {
                await CommonFunctions.CleanUpUserAsync(userId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateMovePageTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            int wordCount = 137;
            int paragraphCount = 1;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // go through ReadDataInit as if we were reading the book
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.UniqueKey is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // move the page per orchestration API
                var newReadDataPacket = OrchestrationApi.OrchestrateMovePage(context, readDataPacket, bookId, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.UniqueKey is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageKey is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.Paragraphs is null)
                { ErrorHandler.LogAndThrow(); return; }


                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateMovePageAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            int wordCount = 137;
            int paragraphCount = 1;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                
                // go through ReadDataInit as if we were reading the book
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.UniqueKey is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // move the page per orchestration API
                var newReadDataPacket = await OrchestrationApi.OrchestrateMovePageAsync(context, readDataPacket, bookId, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.UniqueKey is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageKey is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.Paragraphs is null)
                    { ErrorHandler.LogAndThrow(); return; }


                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateReadDataInitTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            int wordCount = 51; // distinct words on the first page
            int paragraphCount = 1;
            int bookUserStatsCount = 8;

            try
            {
                // create user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, languageId, (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // simulate read data init
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.Paragraphs is null ||
                    readDataPacket.BookUserStats is null)
                { ErrorHandler.LogAndThrow(); return; }


                Assert.IsTrue(readDataPacket.BookCurrentPageNum == 1);
                Assert.IsNotNull(readDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, readDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, readDataPacket.Paragraphs.Count);
                Assert.AreEqual(bookUserStatsCount, readDataPacket.BookUserStats.Count);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateReadDataInitAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            int wordCount = 51; // distinct words on the first page
            int paragraphCount = 1;
            int bookUserStatsCount = 8;

            try
            {
                // create user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                
                // simulate read data init
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    //readDataPacket.AllWordUsersInPage is null ||
                    readDataPacket.Paragraphs is null ||
                    readDataPacket.BookUserStats is null)
                    { ErrorHandler.LogAndThrow(); return; }


                Assert.IsTrue(readDataPacket.BookCurrentPageNum == 1);
                Assert.IsNotNull(readDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, readDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, readDataPacket.Paragraphs.Count);
                Assert.AreEqual(bookUserStatsCount, readDataPacket.BookUserStats.Count);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateResetReadDataForNewPageTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            int wordCount = 137; // distinct words on the second page
            int paragraphCount = 1;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, languageId, (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // simulate the read init
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.UniqueKey is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                // now pretend to move the page forward and reset
                int targetPageNum = 2;

                PageUserApi.PageUserMarkAsRead(context, (Guid)readDataPacket.CurrentPageUser.UniqueKey);

                // create the new page user

                // but first need to pull the page
                readDataPacket.CurrentPage = PageApi.PageReadByOrdinalAndBookId(
                    context, targetPageNum, bookId);
                if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.UniqueKey is null ||
                    readDataPacket.LoggedInUser is null ||
                    readDataPacket.LoggedInUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                readDataPacket.CurrentPageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (Guid)readDataPacket.CurrentPage.UniqueKey, (Guid)readDataPacket.LoggedInUser.UniqueKey);

                if (readDataPacket.CurrentPageUser is null || readDataPacket.CurrentPageUser.PageKey is null)
                { ErrorHandler.LogAndThrow(); return; }

                //finally, we get to reset the data
                var newReadDataPacket = OrchestrationApi.OrchestrateResetReadDataForNewPage(
                    context, readDataPacket, (Guid)readDataPacket.CurrentPageUser.PageKey);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.UniqueKey is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageKey is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.Paragraphs is null)
                { ErrorHandler.LogAndThrow(); return; }


                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateResetReadDataForNewPageAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            int wordCount = 137; // distinct words on the second page
            int paragraphCount = 1;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                
                // simulate the read init
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.Paragraphs is null ||
                    readDataPacket.CurrentPageUser.UniqueKey is null || readDataPacket.LanguageUser is null ||
                    readDataPacket.LanguageUser.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // now pretend to move the page forward and reset
                int targetPageNum = 2;

                await PageUserApi.PageUserMarkAsReadAsync(context, (Guid)readDataPacket.CurrentPageUser.UniqueKey);

                // create the new page user

                // but first need to pull the page
                readDataPacket.CurrentPage = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, targetPageNum, bookId);
                if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.UniqueKey is null ||
                    readDataPacket.LoggedInUser is null ||
                    readDataPacket.LoggedInUser.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                readDataPacket.CurrentPageUser = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, (Guid)readDataPacket.CurrentPage.UniqueKey, (Guid)readDataPacket.LoggedInUser.UniqueKey);

                if (readDataPacket.CurrentPageUser is null || readDataPacket.CurrentPageUser.PageKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                //finally, we get to reset the data
                var newReadDataPacket = await OrchestrationApi.OrchestrateResetReadDataForNewPageAsync(
                    context, readDataPacket, (Guid)readDataPacket.CurrentPageUser.PageKey);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.BookUser.UniqueKey is null || newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.CurrentPageUser.PageKey is null || newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.Paragraphs is null)
                    { ErrorHandler.LogAndThrow(); return; }


                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
    }
}