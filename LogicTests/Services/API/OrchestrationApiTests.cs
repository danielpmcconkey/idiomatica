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
using Model.Enums;
using Microsoft.EntityFrameworkCore;
using Model.DAL;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class OrchestrationApiTests
    {
        [TestMethod()]
        public void OrchestrateBookCreationAndSubProcessesTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
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

                if (user is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, CommonFunctions.GetSpanishLanguage(context), user);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                    context,
                    (Guid)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.Id;

                // read the book stats
                var totalPagesStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.TOTALPAGES)
                    .FirstOrDefault();
                if (totalPagesStat is null || totalPagesStat.Value is null)
                { ErrorHandler.LogAndThrow(); return; }
                string totalPagesActual = totalPagesStat.Value;

                var totalWordsStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                if (totalWordsStat is null || totalWordsStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string totalWordCountActual = totalWordsStat.Value;

                var distinctWordsStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.DISTINCTWORDCOUNT)
                    .FirstOrDefault();
                if (distinctWordsStat is null || distinctWordsStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string distinctWordCountActual = distinctWordsStat.Value;

                // pull the book user
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                    context, (Guid)book.Id, (Guid)user.Id);

                // assert
                Assert.IsNotNull(book.BookStats);
                Assert.AreEqual(totalPagesExpected, totalPagesActual);
                Assert.AreEqual(totalWordCountExpected, totalWordCountActual);
                Assert.AreEqual(distinctWordCountExpected, distinctWordCountActual);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateBookCreationAndSubProcessesAsyncTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
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

                if (user is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, CommonFunctions.GetSpanishLanguage(context), user);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                    context,
                    (Guid)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null)
                    { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.Id;

                // read the book stats
                var totalPagesStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.TOTALPAGES)
                    .FirstOrDefault();
                if (totalPagesStat is null || totalPagesStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string totalPagesActual = totalPagesStat.Value;

                var totalWordsStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                if (totalWordsStat is null || totalWordsStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string totalWordCountActual = totalWordsStat.Value;

                var distinctWordsStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.DISTINCTWORDCOUNT)
                    .FirstOrDefault();
                if (distinctWordsStat is null || distinctWordsStat.Value is null)
                    { ErrorHandler.LogAndThrow(); return; }
                string distinctWordCountActual = distinctWordsStat.Value;

                // pull the book user
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(
                    context, (Guid)book.Id, (Guid)user.Id);

                // assert
                Assert.IsNotNull(book.BookStats);
                Assert.AreEqual(totalPagesExpected, totalPagesActual);
                Assert.AreEqual(totalWordCountExpected, totalWordCountActual);
                Assert.AreEqual(distinctWordCountExpected, distinctWordCountActual);
                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateClearPageAndMoveTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int totalPageCount = 5;
            Guid languageId = CommonFunctions.GetSpanishLanguageId(context);
            Language language = CommonFunctions.GetSpanishLanguage(context);
            int wordCount = 137;
            int paragraphCount = 1;
            string wordToLookUp = "país";
            AvailableWordUserStatus statusBeforeExpected = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfterExpected = AvailableWordUserStatus.WELLKNOWN;

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUser = LanguageUserApi.LanguageUserCreate(context, language, user);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.Paragraphs is null || readDataPacket.LanguageUser is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // check the word and its status
                Word? foundWordBefore = null;
                if (!readDataPacket.AllWordsInPage.TryGetValue(wordToLookUp, out foundWordBefore))
                { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserBefore = foundWordBefore.WordUsers
                    .Where(x => x.LanguageUserId == languageUser.Id)
                    .FirstOrDefault();
                if (foundUserBefore is null)
                { ErrorHandler.LogAndThrow(); return; }
                var statusBeforeActual = foundUserBefore.Status;

                // pull the current page ID before moving so you can later look up its wordUsers
                Guid origPageId = (Guid)readDataPacket.CurrentPageUser.PageId;

                // clear the unknown words and move to the new page
                var newReadDataPacket = OrchestrationApi.OrchestrateClearPageAndMove(context, readDataPacket, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.Paragraphs is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // pull the previous page's wordUser list
                var priorPageWordDict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    context, origPageId, (Guid)user.Id);
                if (priorPageWordDict is null)
                { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserAfter = null;
                if (!priorPageWordDict.TryGetValue(wordToLookUp, out foundUserAfter))
                { ErrorHandler.LogAndThrow(); return; }
                if (foundUserAfter is null)
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateClearPageAndMoveAsyncTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int totalPageCount = 5;
            Guid languageId = CommonFunctions.GetSpanishLanguageId(context);
            Language language = CommonFunctions.GetSpanishLanguage(context);
            int wordCount = 137;
            int paragraphCount = 1;
            string wordToLookUp = "país";
            AvailableWordUserStatus statusBeforeExpected = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfterExpected = AvailableWordUserStatus.WELLKNOWN;

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, language, user);
                Assert.IsNotNull(languageUser);
                
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(context, userService, bookId);
                if (readDataPacket is null || readDataPacket.CurrentPageUser is null ||
                    readDataPacket.CurrentPageUser.Page is null || readDataPacket.AllWordsInPage is null ||
                    readDataPacket.Paragraphs is null ||
                    readDataPacket.LanguageUser is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // check the word and its status
                Word? foundWordBefore = null;
                if (!readDataPacket.AllWordsInPage.TryGetValue(wordToLookUp, out foundWordBefore))
                { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserBefore = foundWordBefore.WordUsers
                    .Where(x => x.LanguageUserId == languageUser.Id)
                    .FirstOrDefault();
                if (foundUserBefore is null)
                { ErrorHandler.LogAndThrow(); return; }
                var statusBeforeActual = foundUserBefore.Status;

                // pull the current page ID before moving so you can later look up its wordUsers
                Guid origPageId = (Guid)readDataPacket.CurrentPageUser.PageId;

                // clear the unknown words and move to the new page
                var newReadDataPacket = await OrchestrationApi.OrchestrateClearPageAndMoveAsync(context, readDataPacket, 2);
                if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                    newReadDataPacket.CurrentPageUser is null ||
                    newReadDataPacket.AllWordsInPage is null ||
                    newReadDataPacket.Paragraphs is null)
                    { ErrorHandler.LogAndThrow(); return; }

                // pull the previous page's wordUser list
                var priorPageWordDict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    context, origPageId, (Guid)user.Id);
                if (priorPageWordDict is null)
                    { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserAfter = null;
                if (!priorPageWordDict.TryGetValue(wordToLookUp, out foundUserAfter))
                    { ErrorHandler.LogAndThrow(); return; }
                if (foundUserAfter is null)
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateFlashCardDispositionAndAdvanceTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            TestDbContextFactory dbContextFactory = new TestDbContextFactory();
            DiContainer diContainer = new DiContainer(dbContextFactory);
            var context = diContainer.DbContextFactory.CreateDbContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int numNew = 4;
            int numOld = 3;
            AvailableLanguageCode learningLanguageCode = AvailableLanguageCode.ES;
            var attemptStatus = AvailableFlashCardAttemptStatus.EASY;
            var startTime = DateTime.Now;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);
                //WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);

                // need to update the wordUsers' status so they show up on flash cardS
                var wordUsers = context.WordUsers
                    .Where(x => x.LanguageUserId == languageUser.Id)
                    .Take(10)
                    .ToList();
                foreach (var wordUser in wordUsers)
                {
                    WordUserApi.WordUserUpdate(
                        context, (Guid)wordUser.Id, AvailableWordUserStatus.LEARNING3, "test");
                }

                // create deck 1
                var dataPacket1 = OrchestrationApi.OrchestrateFlashCardDeckCreation(
                    diContainer, userId, learningLanguageCode, numNew, numOld);

                Assert.IsNotNull(dataPacket1);
                Assert.IsNotNull(dataPacket1.Deck);
                Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
                Assert.IsNotNull(dataPacket1.CurrentCard);
                Assert.IsNotNull(dataPacket1.CurrentCard.Id);

                Guid firstCardId = (Guid)dataPacket1.CurrentCard.Id;

                // disposition the current card and advance
                var dataPacket2 = OrchestrationApi.OrchestrateFlashCardDispositionAndAdvance(
                    context, dataPacket1, attemptStatus);

                Assert.IsNotNull(dataPacket2);
                Assert.IsNotNull(dataPacket2.Deck);
                Assert.IsNotNull(dataPacket2.CurrentCard);
                Assert.IsNotNull(dataPacket2.CurrentCard.Id);

                Guid secondCardId = (Guid)dataPacket2.CurrentCard.Id;

                Assert.AreNotEqual(firstCardId, secondCardId);

                // pull card1 from the DB
                var firstCardRead = FlashCardApi.FlashCardReadById(context, firstCardId);
                // pull the attempt
                var attempt = context.FlashCardAttempts.Where(x => x.FlashCardId == firstCardId).FirstOrDefault();

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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateFlashCardDispositionAndAdvanceAsyncTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            TestDbContextFactory dbContextFactory = new TestDbContextFactory();
            DiContainer diContainer = new DiContainer(dbContextFactory);
            var context = diContainer.DbContextFactory.CreateDbContext();
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int numNew = 4;
            int numOld = 3;
            AvailableLanguageCode learningLanguageCode = AvailableLanguageCode.ES;
            var attemptStatus = AvailableFlashCardAttemptStatus.EASY;
            var startTime = DateTime.Now;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, userId);
                //WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);

                // need to update the wordUsers' status so they show up on flash cardS
                var wordUsers = context.WordUsers
                    .Where(x => x.LanguageUserId == languageUser.Id)
                    .Take(10)
                    .ToList();
                foreach (var wordUser in wordUsers)
                {
                    await WordUserApi.WordUserUpdateAsync(
                        context, (Guid)wordUser.Id, AvailableWordUserStatus.LEARNING3, "test");
                }

                // create deck 1
                var dataPacket1 = await OrchestrationApi.OrchestrateFlashCardDeckCreationAsync(
                    diContainer, userId, learningLanguageCode, numNew, numOld);

                Assert.IsNotNull(dataPacket1);
                Assert.IsNotNull(dataPacket1.Deck);
                Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
                Assert.IsNotNull(dataPacket1.CurrentCard);
                Assert.IsNotNull(dataPacket1.CurrentCard.Id);

                Guid firstCardId = (Guid)dataPacket1.CurrentCard.Id;

                // disposition the current card and advance
                var dataPacket2 = await OrchestrationApi.OrchestrateFlashCardDispositionAndAdvanceAsync(
                    context, dataPacket1, attemptStatus);

                Assert.IsNotNull(dataPacket2);
                Assert.IsNotNull(dataPacket2.Deck);
                Assert.IsNotNull(dataPacket2.CurrentCard);
                Assert.IsNotNull(dataPacket2.CurrentCard.Id);

                Guid secondCardId = (Guid)dataPacket2.CurrentCard.Id;

                Assert.AreNotEqual(firstCardId, secondCardId);

                // pull card1 from the DB
                var firstCardRead = await FlashCardApi.FlashCardReadByIdAsync(context, firstCardId);
                // pull the attempt
                var attempt = context.FlashCardAttempts.Where(x => x.FlashCardId == firstCardId).FirstOrDefault();

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
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int numNew = 4;
            int numOld = 3;
            AvailableLanguageCode learningLanguageCode = AvailableLanguageCode.ES;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);


                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);

                // create wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(
                    context, bookId, (Guid)userId);

                // need to update the wordUsers' status so they show up on flash cardS
                var wordUsers = context.WordUsers.Where(x => x.LanguageUserId == languageUser.Id)
                    .Take(10).ToList();
                foreach (var wordUser in wordUsers)
                {
                    WordUserApi.WordUserUpdate(
                        context, (Guid)wordUser.Id, AvailableWordUserStatus.LEARNING3, "test");
                }

                // create deck 1
                var dataPacket1 = OrchestrationApi.OrchestrateFlashCardDeckCreation(
                    dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

                Assert.IsNotNull(dataPacket1);
                Assert.IsNotNull(dataPacket1.Deck);
                Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
                Assert.IsNotNull(dataPacket1.CurrentCard);
                Assert.IsNotNull(dataPacket1.CurrentCard.Id);


                // manually update card 1 to do not use, and next review date in
                // the past so it would show up in the next deck creation had we
                // not set it to STOP
                Assert.IsNotNull(dataPacket1);
                var cardToUpdate1 = dataPacket1.Deck[1];
                Assert.IsNotNull(cardToUpdate1);
                Assert.IsNotNull(cardToUpdate1.Id);
                Assert.IsNotNull(cardToUpdate1.WordUserId);
                FlashCardApi.FlashCardUpdate(context, (Guid)cardToUpdate1.Id,
                    (Guid)cardToUpdate1.WordUserId, AvailableFlashCardStatus.DONTUSE,
                    DateTime.Now.AddHours(-1), (Guid)cardToUpdate1.Id);

                // manually update card 2's next review date so it shows up in the next deck creation
                Assert.IsNotNull(dataPacket1);
                var cardToUpdate = dataPacket1.Deck[1];
                Assert.IsNotNull(cardToUpdate);
                Assert.IsNotNull(cardToUpdate.Id);
                Assert.IsNotNull(cardToUpdate.WordUserId);
                FlashCardApi.FlashCardUpdate(context, (Guid)cardToUpdate.Id,
                    (Guid)cardToUpdate.WordUserId, AvailableFlashCardStatus.ACTIVE,
                    DateTime.Now.AddHours(-1), (Guid)cardToUpdate.Id);

                // create deck 2
                var dataPacket2 = OrchestrationApi.OrchestrateFlashCardDeckCreation(
                    dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateFlashCardDeckCreationAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook17Id(context);
            int numNew = 4;
            int numOld = 3;
            AvailableLanguageCode learningLanguageCode = AvailableLanguageCode.ES;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);


                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);

                // create wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(
                    context, bookId, (Guid)userId);

                // need to update the wordUsers' status so they show up on flash cardS
                var wordUsers = context.WordUsers.Where(x => x.LanguageUserId == languageUser.Id)
                    .Take(10).ToList();
                foreach (var wordUser in wordUsers)
                {
                    await WordUserApi.WordUserUpdateAsync(
                        context, (Guid)wordUser.Id, AvailableWordUserStatus.LEARNING3, "test");
                }

                // create deck 1
                var dataPacket1 = await OrchestrationApi.OrchestrateFlashCardDeckCreationAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

                Assert.IsNotNull(dataPacket1);
                Assert.IsNotNull(dataPacket1.Deck);
                Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
                Assert.IsNotNull(dataPacket1.CurrentCard);
                Assert.IsNotNull(dataPacket1.CurrentCard.Id);

                
                // manually update card 1 to do not use, and next review date in
                // the past so it would show up in the next deck creation had we
                // not set it to STOP
                Assert.IsNotNull(dataPacket1);
                var cardToUpdate1 = dataPacket1.Deck[1];
                Assert.IsNotNull(cardToUpdate1);
                Assert.IsNotNull(cardToUpdate1.Id);
                Assert.IsNotNull(cardToUpdate1.WordUserId);
                await FlashCardApi.FlashCardUpdateAsync(context, (Guid)cardToUpdate1.Id,
                    (Guid)cardToUpdate1.WordUserId, AvailableFlashCardStatus.DONTUSE,
                    DateTime.Now.AddHours(-1), (Guid)cardToUpdate1.Id);

                // manually update card 2's next review date so it shows up in the next deck creation
                Assert.IsNotNull(dataPacket1); 
                var cardToUpdate = dataPacket1.Deck[1];
                Assert.IsNotNull(cardToUpdate);
                Assert.IsNotNull(cardToUpdate.Id);
                Assert.IsNotNull(cardToUpdate.WordUserId);
                await FlashCardApi.FlashCardUpdateAsync(context, (Guid)cardToUpdate.Id,
                    (Guid)cardToUpdate.WordUserId, AvailableFlashCardStatus.ACTIVE,
                    DateTime.Now.AddHours(-1), (Guid)cardToUpdate.Id);

                // create deck 2
                var dataPacket2 = await OrchestrationApi.OrchestrateFlashCardDeckCreationAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateMovePageTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = CommonFunctions.GetSpanishLanguageId(context);
            Language language = CommonFunctions.GetSpanishLanguage(context);
            int wordCount = 137;
            int paragraphCount = 1;

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // go through ReadDataInit as if we were reading the book
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    context, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // move the page per orchestration API
                var newReadDataPacket = OrchestrationApi.OrchestrateMovePage(
                    context, readDataPacket, bookId, 2);
                Assert.IsNotNull(newReadDataPacket);
                Assert.IsNotNull(newReadDataPacket.BookUser);
                Assert.IsNotNull(newReadDataPacket.CurrentPageUser);
                Assert.IsNotNull(newReadDataPacket.AllWordsInPage);
                Assert.IsNotNull(newReadDataPacket.Paragraphs);


                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateMovePageAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = CommonFunctions.GetSpanishLanguageId(context);
            Language language = CommonFunctions.GetSpanishLanguage(context);
            int wordCount = 137;
            int paragraphCount = 1;

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // go through ReadDataInit as if we were reading the book
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    context, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // move the page per orchestration API
                var newReadDataPacket = await OrchestrationApi.OrchestrateMovePageAsync(
                    context, readDataPacket, bookId, 2);
                Assert.IsNotNull(newReadDataPacket);
                Assert.IsNotNull(newReadDataPacket.BookUser);
                Assert.IsNotNull(newReadDataPacket.CurrentPageUser);
                Assert.IsNotNull(newReadDataPacket.AllWordsInPage);
                Assert.IsNotNull(newReadDataPacket.Paragraphs);


                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateReadDataInitTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = learningLanguage.Id;
            int wordCount = 51; // distinct words on the first page
            int paragraphCount = 1;
            int bookUserStatsCount = 8;

            try
            {
                // create user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // create languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLanguage, user);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // simulate read data init
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    context, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.BookUserStats);


                Assert.IsTrue(readDataPacket.BookCurrentPageNum == 1);
                Assert.IsNotNull(readDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, readDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, readDataPacket.Paragraphs.Count);
                Assert.AreEqual(bookUserStatsCount, readDataPacket.BookUserStats.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateReadDataInitAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            var bookId = CommonFunctions.GetBook17Id(context);
            Guid languageId = learningLanguage.Id;
            int wordCount = 51; // distinct words on the first page
            int paragraphCount = 1;
            int bookUserStatsCount = 8;

            try
            {
                // create user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // create languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLanguage, user);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                
                // simulate read data init
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    context, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.BookUserStats);


                Assert.IsTrue(readDataPacket.BookCurrentPageNum == 1);
                Assert.IsNotNull(readDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, readDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, readDataPacket.Paragraphs.Count);
                Assert.AreEqual(bookUserStatsCount, readDataPacket.BookUserStats.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void OrchestrateResetReadDataForNewPageTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var bookId = CommonFunctions.GetBook17Id(context);
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            int wordCount = 137; // distinct words on the second page
            int paragraphCount = 1;

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // simulate the read init
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    context, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // now pretend to move the page forward and reset
                int targetPageNum = 2;

                PageUserApi.PageUserMarkAsRead(
                    context, (Guid)readDataPacket.CurrentPageUser.Id);

                // create the new page user

                // but first need to pull the page
                readDataPacket.CurrentPage = PageApi.PageReadByOrdinalAndBookId(
                    context, targetPageNum, bookId);
                Assert.IsNotNull(readDataPacket.CurrentPage);
                Assert.IsNotNull(readDataPacket.LoggedInUser);

                readDataPacket.CurrentPageUser = PageUserApi
                    .PageUserCreateForPageIdAndUserId(
                    context, readDataPacket.CurrentPage, readDataPacket.LoggedInUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);

                //finally, we get to reset the data
                var newReadDataPacket = OrchestrationApi
                    .OrchestrateResetReadDataForNewPage(
                    context, readDataPacket, (Guid)readDataPacket.CurrentPageUser.PageId);
                Assert.IsNotNull(newReadDataPacket);
                Assert.IsNotNull(newReadDataPacket.BookUser);
                Assert.IsNotNull(newReadDataPacket.CurrentPageUser);
                Assert.IsNotNull(newReadDataPacket.AllWordsInPage);
                Assert.IsNotNull(newReadDataPacket.Paragraphs);


                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task OrchestrateResetReadDataForNewPageAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var bookId = CommonFunctions.GetBook17Id(context);
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            int wordCount = 137; // distinct words on the second page
            int paragraphCount = 1;

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);
                
                // simulate the read init
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    context, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // now pretend to move the page forward and reset
                int targetPageNum = 2;

                await PageUserApi.PageUserMarkAsReadAsync(
                    context, (Guid)readDataPacket.CurrentPageUser.Id);

                // create the new page user

                // but first need to pull the page
                readDataPacket.CurrentPage = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, targetPageNum, bookId);
                Assert.IsNotNull(readDataPacket.CurrentPage);
                Assert.IsNotNull(readDataPacket.LoggedInUser);

                readDataPacket.CurrentPageUser = await PageUserApi
                    .PageUserCreateForPageIdAndUserIdAsync(
                    context, readDataPacket.CurrentPage, readDataPacket.LoggedInUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);

                //finally, we get to reset the data
                var newReadDataPacket = await OrchestrationApi
                    .OrchestrateResetReadDataForNewPageAsync(
                    context, readDataPacket, (Guid)readDataPacket.CurrentPageUser.PageId);
                Assert.IsNotNull(newReadDataPacket);
                Assert.IsNotNull(newReadDataPacket.BookUser);
                Assert.IsNotNull(newReadDataPacket.CurrentPageUser);
                Assert.IsNotNull(newReadDataPacket.AllWordsInPage);
                Assert.IsNotNull(newReadDataPacket.Paragraphs);


                Assert.IsTrue(newReadDataPacket.BookCurrentPageNum == 2);
                Assert.IsNotNull(newReadDataPacket.BookTotalPageCount == 12);
                Assert.AreEqual(wordCount, newReadDataPacket.AllWordsInPage.Count);
                Assert.AreEqual(paragraphCount, newReadDataPacket.Paragraphs.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }
    }
}