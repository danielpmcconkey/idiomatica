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
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);

            /*
             * this is supposed to create the book, the book stats, and the 
             * bookUser, all in one call
             * */

            string totalPagesExpected = "4";
            string totalWordCountExpected = "774"; // 784 before???
            string distinctWordCountExpected = "232"; // 239 befor??

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the book
                Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                    dbContextFactory,
                    (Guid)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // read the book stats
                var totalPagesStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.TOTALPAGES)
                    .FirstOrDefault();
                Assert.IsNotNull(totalPagesStat);
                Assert.IsNotNull(totalPagesStat.Value);
                string totalPagesActual = totalPagesStat.Value;

                var totalWordsStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                Assert.IsNotNull(totalWordsStat);
                Assert.IsNotNull(totalWordsStat.Value);
                string totalWordCountActual = totalWordsStat.Value;

                var distinctWordsStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.DISTINCTWORDCOUNT)
                    .FirstOrDefault();
                Assert.IsNotNull(distinctWordsStat);
                Assert.IsNotNull(distinctWordsStat.Value);
                string distinctWordCountActual = distinctWordsStat.Value;

                // pull the book user
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                    dbContextFactory, (Guid)book.Id, (Guid)user.Id);

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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task OrchestrateBookCreationAndSubProcessesAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);

            /*
             * this is supposed to create the book, the book stats, and the 
             * bookUser, all in one call
             * */

            string totalPagesExpected = "4";
            string totalWordCountExpected = "774"; // 784 before???
            string distinctWordCountExpected = "232"; // 239 befor??

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the book
                Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                    dbContextFactory,
                    (Guid)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // read the book stats
                var totalPagesStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.TOTALPAGES)
                    .FirstOrDefault();
                Assert.IsNotNull(totalPagesStat);
                Assert.IsNotNull(totalPagesStat.Value);
                string totalPagesActual = totalPagesStat.Value;

                var totalWordsStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.TOTALWORDCOUNT)
                    .FirstOrDefault();
                Assert.IsNotNull(totalWordsStat);
                Assert.IsNotNull(totalWordsStat.Value);
                string totalWordCountActual = totalWordsStat.Value;

                var distinctWordsStat = book.BookStats
                    .Where(x => x.Key == AvailableBookStat.DISTINCTWORDCOUNT)
                    .FirstOrDefault();
                Assert.IsNotNull(distinctWordsStat);
                Assert.IsNotNull(distinctWordsStat.Value);
                string distinctWordCountActual = distinctWordsStat.Value;

                // pull the book user
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(
                    dbContextFactory, (Guid)book.Id, (Guid)user.Id);

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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void OrchestrateClearPageAndMoveTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid bookId = CommonFunctions.GetBookEspañaId(dbContextFactory); // España
            int totalPageCount = 5;
            Guid languageId = learningLanguage.Id;
            int wordCount = 135; // distinct words in page 2
            int paragraphCount = 1;
            string wordToLookUp = "país";
            AvailableWordUserStatus statusBeforeExpected = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfterExpected = AvailableWordUserStatus.WELLKNOWN;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    dbContextFactory, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // check the word and its status
                Word? foundWordBefore = null;
                if (!readDataPacket.AllWordsInPage.TryGetValue(wordToLookUp, out foundWordBefore))
                { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserBefore = foundWordBefore.WordUsers
                    .Where(x => x.LanguageUserId == languageUser.Id)
                    .FirstOrDefault();
                Assert.IsNotNull(foundUserBefore);
                var statusBeforeActual = foundUserBefore.Status;

                // pull the current page ID before moving so you can later look up its wordUsers
                Guid origPageId = (Guid)readDataPacket.CurrentPageUser.PageId;

                // clear the unknown words and move to the new page
                var newReadDataPacket = OrchestrationApi.OrchestrateClearPageAndMove(
                    dbContextFactory, readDataPacket, 2);
                Assert.IsNotNull(newReadDataPacket);
                Assert.IsNotNull(newReadDataPacket.BookUser);
                Assert.IsNotNull(newReadDataPacket.CurrentPageUser);
                Assert.IsNotNull(newReadDataPacket.AllWordsInPage);
                Assert.IsNotNull(newReadDataPacket.Paragraphs);

                // pull the previous page's wordUser list
                var priorPageWordDict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    dbContextFactory, origPageId, (Guid)user.Id);
                Assert.IsNotNull(priorPageWordDict);
                WordUser? foundUserAfter = null;
                if (!priorPageWordDict.TryGetValue(wordToLookUp, out foundUserAfter))
                { ErrorHandler.LogAndThrow(); return; }
                Assert.IsNotNull(foundUserAfter);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task OrchestrateClearPageAndMoveAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid bookId = CommonFunctions.GetBookEspañaId(dbContextFactory); // España
            int totalPageCount = 5;
            Guid languageId = learningLanguage.Id;
            int wordCount = 135; // distinct words in page 2
            int paragraphCount = 1;
            string wordToLookUp = "país";
            AvailableWordUserStatus statusBeforeExpected = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfterExpected = AvailableWordUserStatus.WELLKNOWN;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    dbContextFactory, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // check the word and its status
                Word? foundWordBefore = null;
                if (!readDataPacket.AllWordsInPage.TryGetValue(wordToLookUp, out foundWordBefore))
                { ErrorHandler.LogAndThrow(); return; }
                WordUser? foundUserBefore = foundWordBefore.WordUsers
                    .Where(x => x.LanguageUserId == languageUser.Id)
                    .FirstOrDefault();
                Assert.IsNotNull(foundUserBefore);
                var statusBeforeActual = foundUserBefore.Status;

                // pull the current page ID before moving so you can later look up its wordUsers
                Guid origPageId = (Guid)readDataPacket.CurrentPageUser.PageId;

                // clear the unknown words and move to the new page
                var newReadDataPacket = await OrchestrationApi.OrchestrateClearPageAndMoveAsync(
                    dbContextFactory, readDataPacket, 2);
                Assert.IsNotNull(newReadDataPacket);
                Assert.IsNotNull(newReadDataPacket.BookUser);
                Assert.IsNotNull(newReadDataPacket.CurrentPageUser);
                Assert.IsNotNull(newReadDataPacket.AllWordsInPage);
                Assert.IsNotNull(newReadDataPacket.Paragraphs);

                // pull the previous page's wordUser list
                var priorPageWordDict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    dbContextFactory, origPageId, (Guid)user.Id);
                Assert.IsNotNull(priorPageWordDict);
                WordUser? foundUserAfter = null;
                if (!priorPageWordDict.TryGetValue(wordToLookUp, out foundUserAfter))
                { ErrorHandler.LogAndThrow(); return; }
                Assert.IsNotNull(foundUserAfter);
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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        //[TestMethod()]
        //public void OrchestrateFlashCardDispositionAndAdvanceTest()
        //{
        //    Guid? userId = null;
        //    var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
        //    var loginService = CommonFunctions.GetRequiredService<LoginService>();
        //    var context = dbContextFactory.CreateDbContext();
        //    Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
        //    AvailableLanguageCode learningLanguageCode = AvailableLanguageCode.ES;
        //    Guid bookId = CommonFunctions.GetBookEspañaId(dbContextFactory);
        //    int numNew = 4;
        //    int numOld = 3;
        //    var attemptStatus = AvailableFlashCardAttemptStatus.EASY;
        //    var startTime = DateTime.Now;

        //    try
        //    {
        //        // create a user
        //        Assert.IsNotNull(loginService);
        //        var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
        //        Assert.IsNotNull(user);
        //        userId = (Guid)user.Id;
        //        Assert.IsNotNull(userId);

        //        // fetch the languageUser
        //        var languageUser = LanguageUserApi.LanguageUserGet(
        //            dbContextFactory, learningLanguage.Id, (Guid)userId);
        //        Assert.IsNotNull(languageUser);

        //        // craete a bookUser
        //        var bookUser = BookUserApi.BookUserCreate(
        //            dbContextFactory, bookId, (Guid)user.Id);
        //        if (bookUser is null)
        //        { ErrorHandler.LogAndThrow(); }

        //        // create wordUsers
        //        WordUserApi.WordUsersCreateAllForBookIdAndUserId(
        //            dbContextFactory, bookId, (Guid)userId);

        //        // need to update the wordUsers' status so they show up on flash cardS
        //        var wordUsers = context.WordUsers
        //            .Where(x => x.LanguageUserId == languageUser.Id)
        //            .Take(10)
        //            .ToList();
        //        foreach (var wordUser in wordUsers)
        //        {
        //            WordUserApi.WordUserUpdate(
        //                dbContextFactory, (Guid)wordUser.Id, AvailableWordUserStatus.LEARNING3, "test");
        //        }

        //        // create deck 1
        //        var dataPacket1 = OrchestrationApi.OrchestrateFlashCardDeckCreation(
        //            dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

        //        Assert.IsNotNull(dataPacket1);
        //        Assert.IsNotNull(dataPacket1.Deck);
        //        Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
        //        Assert.IsNotNull(dataPacket1.CurrentCard);
        //        Assert.IsNotNull(dataPacket1.CurrentCard.Id);

        //        Guid firstCardId = (Guid)dataPacket1.CurrentCard.Id;

        //        // disposition the current card and advance
        //        var dataPacket2 = OrchestrationApi
        //            .OrchestrateFlashCardDispositionAndAdvance(
        //                dbContextFactory, dataPacket1, attemptStatus);

        //        Assert.IsNotNull(dataPacket2);
        //        Assert.IsNotNull(dataPacket2.Deck);
        //        Assert.IsNotNull(dataPacket2.CurrentCard);
        //        Assert.IsNotNull(dataPacket2.CurrentCard.Id);

        //        Guid secondCardId = (Guid)dataPacket2.CurrentCard.Id;

        //        Assert.AreNotEqual(firstCardId, secondCardId);

        //        // pull card1 from the DB
        //        var firstCardRead = FlashCardApi.FlashCardReadById(
        //            dbContextFactory, firstCardId);
        //        // pull the attempt
        //        var attempt = context.FlashCardAttempts
        //            .Where(x => x.FlashCardId == firstCardId)
        //            .FirstOrDefault();

        //        Assert.IsNotNull(firstCardRead);
        //        Assert.IsNotNull(firstCardRead.Status);
        //        Assert.IsNotNull(firstCardRead.NextReview);
        //        Assert.IsTrue(firstCardRead.NextReview > startTime);
        //        Assert.IsNotNull(attempt);
        //        Assert.IsNotNull(attempt.Status);
        //        Assert.AreEqual(attemptStatus, attempt.Status);

        //    }
        //    finally
        //    {
        //        if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
        //    }
        //}
        //[TestMethod()]
        //public async Task OrchestrateFlashCardDispositionAndAdvanceAsyncTest()
        //{
        //    Guid? userId = null;
        //    var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
        //    var loginService = CommonFunctions.GetRequiredService<LoginService>();
        //    var context = dbContextFactory.CreateDbContext();
        //    Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
        //    AvailableLanguageCode learningLanguageCode = AvailableLanguageCode.ES;
        //    Guid bookId = CommonFunctions.GetBookEspañaId(dbContextFactory);
        //    int numNew = 4;
        //    int numOld = 3;
        //    var attemptStatus = AvailableFlashCardAttemptStatus.EASY;
        //    var startTime = DateTime.Now;

        //    try
        //    {
        //        // create a user
        //        Assert.IsNotNull(loginService);
        //        var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
        //        Assert.IsNotNull(user);
        //        userId = (Guid)user.Id;
        //        Assert.IsNotNull(userId);

        //        // fetch the languageUser
        //        var languageUser = await LanguageUserApi.LanguageUserGetAsync(
        //            dbContextFactory, learningLanguage.Id, (Guid)userId);
        //        Assert.IsNotNull(languageUser);

        //        // craete a bookUser
        //        var bookUser = await BookUserApi.BookUserCreateAsync(
        //            dbContextFactory, bookId, (Guid)user.Id);
        //        if (bookUser is null)
        //        { ErrorHandler.LogAndThrow(); }

        //        // create wordUsers
        //        await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(
        //            dbContextFactory, bookId, (Guid)userId);

        //        // need to update the wordUsers' status so they show up on flash cardS
        //        var wordUsers = context.WordUsers
        //            .Where(x => x.LanguageUserId == languageUser.Id)
        //            .Take(10)
        //            .ToList();
        //        foreach (var wordUser in wordUsers)
        //        {
        //            await WordUserApi.WordUserUpdateAsync(
        //                dbContextFactory, (Guid)wordUser.Id, AvailableWordUserStatus.LEARNING3, "test");
        //        }

        //        // create deck 1
        //        var dataPacket1 = await OrchestrationApi.OrchestrateFlashCardDeckCreationAsync(
        //            dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

        //        Assert.IsNotNull(dataPacket1);
        //        Assert.IsNotNull(dataPacket1.Deck);
        //        Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
        //        Assert.IsNotNull(dataPacket1.CurrentCard);
        //        Assert.IsNotNull(dataPacket1.CurrentCard.Id);

        //        Guid firstCardId = (Guid)dataPacket1.CurrentCard.Id;

        //        // disposition the current card and advance
        //        var dataPacket2 = await OrchestrationApi
        //            .OrchestrateFlashCardDispositionAndAdvanceAsync(
        //                dbContextFactory, dataPacket1, attemptStatus);

        //        Assert.IsNotNull(dataPacket2);
        //        Assert.IsNotNull(dataPacket2.Deck);
        //        Assert.IsNotNull(dataPacket2.CurrentCard);
        //        Assert.IsNotNull(dataPacket2.CurrentCard.Id);

        //        Guid secondCardId = (Guid)dataPacket2.CurrentCard.Id;

        //        Assert.AreNotEqual(firstCardId, secondCardId);

        //        // pull card1 from the DB
        //        var firstCardRead = await FlashCardApi.FlashCardReadByIdAsync(
        //            dbContextFactory, firstCardId);
        //        // pull the attempt
        //        var attempt = context.FlashCardAttempts
        //            .Where(x => x.FlashCardId == firstCardId)
        //            .FirstOrDefault();

        //        Assert.IsNotNull(firstCardRead);
        //        Assert.IsNotNull(firstCardRead.Status);
        //        Assert.IsNotNull(firstCardRead.NextReview);
        //        Assert.IsTrue(firstCardRead.NextReview > startTime);
        //        Assert.IsNotNull(attempt);
        //        Assert.IsNotNull(attempt.Status);
        //        Assert.AreEqual(attemptStatus, attempt.Status);

        //    }
        //    finally
        //    {
        //        if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
        //    }
        //}


        //[TestMethod()]
        //public void OrchestrateFlashCardDeckCreationTest()
        //{
        //    Guid? userId = null;
        //    var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
        //    var loginService = CommonFunctions.GetRequiredService<LoginService>();
        //    var context = dbContextFactory.CreateDbContext();
        //    Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
        //    Guid bookId = CommonFunctions.GetBookEspañaId(dbContextFactory);
        //    int numNew = 4;
        //    int numOld = 3;
        //    AvailableLanguageCode learningLanguageCode = AvailableLanguageCode.ES;

        //    try
        //    {
        //        // create a user
        //        Assert.IsNotNull(loginService);
        //        var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
        //        Assert.IsNotNull(user);
        //        userId = (Guid)user.Id;
        //        Assert.IsNotNull(userId);


        //        // pull the languageUser
        //        Assert.IsNotNull(userId);
        //        var languageUser = LanguageUserApi.LanguageUserGet(
        //            dbContextFactory, learningLanguage.Id, (Guid)userId);
        //        Assert.IsNotNull(languageUser);

        //        // craete a bookUser
        //        var bookUser = BookUserApi.BookUserCreate(
        //            dbContextFactory, bookId, (Guid)user.Id);
        //        Assert.IsNotNull(bookUser);

        //        // create wordUsers
        //        WordUserApi.WordUsersCreateAllForBookIdAndUserId(
        //            dbContextFactory, bookId, (Guid)userId);

        //        // need to update the wordUsers' status so they show up on flash cardS
        //        var wordUsers = context.WordUsers.Where(x => x.LanguageUserId == languageUser.Id)
        //            .Take(10).ToList();
        //        foreach (var wordUser in wordUsers)
        //        {
        //            WordUserApi.WordUserUpdate(
        //                dbContextFactory, (Guid)wordUser.Id,
        //                AvailableWordUserStatus.LEARNING3, "test");
        //        }

        //        // create deck 1
        //        // requesting 4 new and 3 old
        //        // it'll have 4 new and 0 old, since there are no old cards yet
        //        var dataPacket1 = OrchestrationApi.OrchestrateFlashCardDeckCreation(
        //            dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

        //        Assert.IsNotNull(dataPacket1);
        //        Assert.IsNotNull(dataPacket1.Deck);
        //        Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
        //        Assert.IsNotNull(dataPacket1.CurrentCard);
        //        Assert.IsNotNull(dataPacket1.CurrentCard.Id);


        //        // manually update card 1 to do not use, and next review date in
        //        // the past so it would show up in the next deck creation had we
        //        // not set it to STOP
        //        Assert.IsNotNull(dataPacket1);
        //        var cardToUpdate1 = dataPacket1.Deck[1];
        //        Assert.IsNotNull(cardToUpdate1);
        //        Assert.IsNotNull(cardToUpdate1.Id);
        //        Assert.IsNotNull(cardToUpdate1.WordUserId);
        //        FlashCardApi.FlashCardUpdate(dbContextFactory, (Guid)cardToUpdate1.Id,
        //            (Guid)cardToUpdate1.WordUserId, AvailableFlashCardStatus.DONTUSE,
        //            DateTimeOffset.Now.AddHours(-1), (Guid)cardToUpdate1.Id);

        //        // manually update card 2's next review date so it shows up in the next deck creation
        //        Assert.IsNotNull(dataPacket1);
        //        var cardToUpdate = dataPacket1.Deck[1];
        //        Assert.IsNotNull(cardToUpdate);
        //        Assert.IsNotNull(cardToUpdate.Id);
        //        Assert.IsNotNull(cardToUpdate.WordUserId);
        //        FlashCardApi.FlashCardUpdate(dbContextFactory, (Guid)cardToUpdate.Id,
        //            (Guid)cardToUpdate.WordUserId, AvailableFlashCardStatus.ACTIVE,
        //            DateTimeOffset.Now.AddHours(-1), (Guid)cardToUpdate.Id);

        //        // create deck 2
        //        // requesting 4 new and 3 old again
        //        // it'll have 4 new and 1 old, since two cards meet the date
        //        // req's but one of those is deactivated
        //        var dataPacket2 = OrchestrationApi.OrchestrateFlashCardDeckCreation(
        //            dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

        //        int expectedCount = 1 // prior review card
        //            + 4 // numNew
        //            ;

        //        Assert.IsNotNull(dataPacket2);
        //        Assert.IsNotNull(dataPacket2.Deck);
        //        Assert.AreEqual(expectedCount, dataPacket2.CardCount);
        //        Assert.IsNotNull(dataPacket2.CurrentCard);
        //    }
        //    finally
        //    {
        //        if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
        //    }
        //}
        //[TestMethod()]
        //public async Task OrchestrateFlashCardDeckCreationAsyncTest()
        //{
        //    Guid? userId = null;
        //    var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
        //    var loginService = CommonFunctions.GetRequiredService<LoginService>();
        //    var context = dbContextFactory.CreateDbContext();
        //    Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
        //    Guid bookId = CommonFunctions.GetBookEspañaId(dbContextFactory);
        //    int numNew = 4;
        //    int numOld = 3;
        //    AvailableLanguageCode learningLanguageCode = AvailableLanguageCode.ES;

        //    try
        //    {
        //        // create a user
        //        Assert.IsNotNull(loginService);
        //        var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
        //        Assert.IsNotNull(user);
        //        userId = (Guid)user.Id;
        //        Assert.IsNotNull(userId);


        //        // pull the languageUser
        //        Assert.IsNotNull(userId);
        //        var languageUser = LanguageUserApi.LanguageUserGet(
        //            dbContextFactory, learningLanguage.Id, (Guid)userId);
        //        Assert.IsNotNull(languageUser);

        //        // craete a bookUser
        //        var bookUser = BookUserApi.BookUserCreate(
        //            dbContextFactory, bookId, (Guid)user.Id);
        //        Assert.IsNotNull(bookUser);

        //        // create wordUsers
        //        await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(
        //            dbContextFactory, bookId, (Guid)userId);

        //        // need to update the wordUsers' status so they show up on flash cardS
        //        var wordUsers = context.WordUsers.Where(x => x.LanguageUserId == languageUser.Id)
        //            .Take(10).ToList();
        //        foreach (var wordUser in wordUsers)
        //        {
        //            await WordUserApi.WordUserUpdateAsync(
        //                dbContextFactory, (Guid)wordUser.Id,
        //                AvailableWordUserStatus.LEARNING3, "test");
        //        }

        //        // create deck 1
        //        // requesting 4 new and 3 old
        //        // it'll have 4 new and 0 old, since there are no old cards yet
        //        var dataPacket1 = await OrchestrationApi.OrchestrateFlashCardDeckCreationAsync(
        //            dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

        //        Assert.IsNotNull(dataPacket1);
        //        Assert.IsNotNull(dataPacket1.Deck);
        //        Assert.IsTrue(dataPacket1.CardCount == numNew); // there are no review cards yet
        //        Assert.IsNotNull(dataPacket1.CurrentCard);
        //        Assert.IsNotNull(dataPacket1.CurrentCard.Id);


        //        // manually update card 1 to do not use, and next review date in
        //        // the past so it would show up in the next deck creation had we
        //        // not set it to STOP
        //        Assert.IsNotNull(dataPacket1);
        //        var cardToUpdate1 = dataPacket1.Deck[1];
        //        Assert.IsNotNull(cardToUpdate1);
        //        Assert.IsNotNull(cardToUpdate1.Id);
        //        Assert.IsNotNull(cardToUpdate1.WordUserId);
        //        await FlashCardApi.FlashCardUpdateAsync(dbContextFactory, (Guid)cardToUpdate1.Id,
        //            (Guid)cardToUpdate1.WordUserId, AvailableFlashCardStatus.DONTUSE,
        //            DateTimeOffset.Now.AddHours(-1), (Guid)cardToUpdate1.Id);

        //        // manually update card 2's next review date so it shows up in the next deck creation
        //        Assert.IsNotNull(dataPacket1);
        //        var cardToUpdate = dataPacket1.Deck[1];
        //        Assert.IsNotNull(cardToUpdate);
        //        Assert.IsNotNull(cardToUpdate.Id);
        //        Assert.IsNotNull(cardToUpdate.WordUserId);
        //        await FlashCardApi.FlashCardUpdateAsync(dbContextFactory, (Guid)cardToUpdate.Id,
        //            (Guid)cardToUpdate.WordUserId, AvailableFlashCardStatus.ACTIVE,
        //            DateTimeOffset.Now.AddHours(-1), (Guid)cardToUpdate.Id);

        //        // create deck 2
        //        // requesting 4 new and 3 old again
        //        // it'll have 4 new and 1 old, since two cards meet the date
        //        // req's but one of those is deactivated
        //        var dataPacket2 = await OrchestrationApi.OrchestrateFlashCardDeckCreationAsync(
        //            dbContextFactory, (Guid)userId, learningLanguageCode, numNew, numOld);

        //        int expectedCount = 1 // prior review card
        //            + 4 // numNew
        //            ;

        //        Assert.IsNotNull(dataPacket2);
        //        Assert.IsNotNull(dataPacket2.Deck);
        //        Assert.AreEqual(expectedCount, dataPacket2.CardCount);
        //        Assert.IsNotNull(dataPacket2.CurrentCard);
        //    }
        //    finally
        //    {
        //        if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
        //    }
        //}


        [TestMethod()]
        public void OrchestrateMovePageTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var bookId = CommonFunctions.GetBookEspañaId(dbContextFactory); // 'España'
            Guid languageId = CommonFunctions.GetSpanishLanguageId(dbContextFactory);
            Language language = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int wordCount = 135; // distinct words on page 2
            int paragraphCount = 1;

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // go through ReadDataInit as if we were reading the book
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    dbContextFactory, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // move the page per orchestration API
                var newReadDataPacket = OrchestrationApi.OrchestrateMovePage(
                    dbContextFactory, readDataPacket, bookId, 2);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task OrchestrateMovePageAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var bookId = CommonFunctions.GetBookEspañaId(dbContextFactory); // 'España'
            Guid languageId = CommonFunctions.GetSpanishLanguageId(dbContextFactory);
            Language language = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int wordCount = 135; // distinct words on page 2
            int paragraphCount = 1;

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // go through ReadDataInit as if we were reading the book
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    dbContextFactory, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // move the page per orchestration API
                var newReadDataPacket = await OrchestrationApi.OrchestrateMovePageAsync(
                    dbContextFactory, readDataPacket, bookId, 2);
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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void OrchestrateReadDataInitTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var bookId = CommonFunctions.GetBookEspañaId(dbContextFactory); // España
            Guid languageId = learningLanguage.Id;
            int wordCount = 49; // distinct words on the first page
            int paragraphCount = 1;
            int bookUserStatsCount = 8;

            try
            {
                // create user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // simulate read data init
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    dbContextFactory, loginService, bookId);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task OrchestrateReadDataInitAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var bookId = CommonFunctions.GetBookEspañaId(dbContextFactory); // España
            Guid languageId = learningLanguage.Id;
            int wordCount = 49; // distinct words on the first page
            int paragraphCount = 1;
            int bookUserStatsCount = 8;

            try
            {
                // create user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // simulate read data init
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    dbContextFactory, loginService, bookId);
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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void OrchestrateResetReadDataForNewPageTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var bookId = CommonFunctions.GetBookEspañaId(dbContextFactory);
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int wordCount = 135; // distinct words on the second page
            int paragraphCount = 1;

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // simulate the read init
                var readDataPacket = OrchestrationApi.OrchestrateReadDataInit(
                    dbContextFactory, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // now pretend to move the page forward and reset
                int targetPageNum = 2;

                PageUserApi.PageUserMarkAsRead(
                    dbContextFactory, (Guid)readDataPacket.CurrentPageUser.Id);

                // create the new page user

                // but first need to pull the page
                readDataPacket.CurrentPage = PageApi.PageReadByOrdinalAndBookId(
                    dbContextFactory, targetPageNum, bookId);
                Assert.IsNotNull(readDataPacket.CurrentPage);
                Assert.IsNotNull(readDataPacket.LoggedInUser);

                readDataPacket.CurrentPageUser = PageUserApi
                    .PageUserCreateForPageIdAndUserId(
                    dbContextFactory, readDataPacket.CurrentPage, readDataPacket.LoggedInUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);

                //finally, we get to reset the data
                var newReadDataPacket = OrchestrationApi
                    .OrchestrateResetReadDataForNewPage(
                    dbContextFactory, readDataPacket, (Guid)readDataPacket.CurrentPageUser.PageId);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task OrchestrateResetReadDataForNewPageAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var bookId = CommonFunctions.GetBookEspañaId(dbContextFactory);
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            int wordCount = 135; // distinct words on the second page
            int paragraphCount = 1;

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // simulate the read init
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    dbContextFactory, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // now pretend to move the page forward and reset
                int targetPageNum = 2;

                await PageUserApi.PageUserMarkAsReadAsync(
                    dbContextFactory, (Guid)readDataPacket.CurrentPageUser.Id);

                // create the new page user

                // but first need to pull the page
                readDataPacket.CurrentPage = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    dbContextFactory, targetPageNum, bookId);
                Assert.IsNotNull(readDataPacket.CurrentPage);
                Assert.IsNotNull(readDataPacket.LoggedInUser);

                readDataPacket.CurrentPageUser = await PageUserApi
                    .PageUserCreateForPageIdAndUserIdAsync(
                    dbContextFactory, readDataPacket.CurrentPage, readDataPacket.LoggedInUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);

                //finally, we get to reset the data
                var newReadDataPacket = await OrchestrationApi
                    .OrchestrateResetReadDataForNewPageAsync(
                    dbContextFactory, readDataPacket, (Guid)readDataPacket.CurrentPageUser.PageId);
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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }

        [TestMethod()]
        public async Task OrchestratePullFlashCardAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode learningLanguageCode = learningLanguage.Code;
            Language uiLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = uiLanguage.Code;

            string expectedText1 = "de";
            string expectedText2 = "la";
            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var word1 = await WordApi.WordReadByLanguageIdAndTextAsync(
                    dbContextFactory, learningLanguage.Id, expectedText1);
                Assert.IsNotNull(word1);
                var word2 = await WordApi.WordReadByLanguageIdAndTextAsync(
                    dbContextFactory, learningLanguage.Id, expectedText2);
                Assert.IsNotNull(word2);

                // add some word users
                var wordUser1 = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);
                var wordUser2 = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word2, languageUser, "the", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser2);

                
                var card = await OrchestrationApi.OrchestratePullFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card);
                var wordUserPulled1 = await DataCache.WordUserByIdReadAsync(card.WordUserId, dbContextFactory);
                Assert.IsNotNull(wordUserPulled1);
                var wordPulled1 = await DataCache.WordByIdReadAsync(
                    wordUserPulled1.WordId, dbContextFactory);
                Assert.IsNotNull(wordPulled1);
                Assert.AreEqual(expectedText1, wordPulled1.TextLowerCase);

                // disposition the card so it doesn't come up when we pull the
                // next card
                await OrchestrationApi.OrchestrateFlashCardDispositioningAsync(
                    dbContextFactory, card, AvailableFlashCardAttemptStatus.EASY);

                var card2 = await OrchestrationApi.OrchestratePullFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card2);
                var wordUserPulled2 = await DataCache.WordUserByIdReadAsync(card2.WordUserId, dbContextFactory);
                Assert.IsNotNull(wordUserPulled2);
                var wordPulled2 = await DataCache.WordByIdReadAsync(wordUserPulled2.WordId, dbContextFactory);
                Assert.IsNotNull(wordPulled2);
                Assert.AreEqual(expectedText2, wordPulled2.TextLowerCase);

            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }

        [TestMethod()]
        public void OrchestratePullFlashCardTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode learningLanguageCode = learningLanguage.Code;
            Language uiLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = uiLanguage.Code;

            string expectedText1 = "de";
            string expectedText2 = "la";
            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var word1 = WordApi.WordReadByLanguageIdAndText(
                    dbContextFactory, learningLanguage.Id, expectedText1);
                Assert.IsNotNull(word1);
                var word2 = WordApi.WordReadByLanguageIdAndText(
                    dbContextFactory, learningLanguage.Id, expectedText2);
                Assert.IsNotNull(word2);

                // add some word users
                var wordUser1 = WordUserApi.WordUserCreate(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);
                var wordUser2 = WordUserApi.WordUserCreate(
                    dbContextFactory, word2, languageUser, "the", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser2);


                var card = OrchestrationApi.OrchestratePullFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card);
                var wordUserPulled1 = DataCache.WordUserByIdRead(card.WordUserId, dbContextFactory);
                Assert.IsNotNull(wordUserPulled1);
                var wordPulled1 = DataCache.WordByIdRead(wordUserPulled1.WordId, dbContextFactory);
                Assert.IsNotNull(wordPulled1);
                Assert.AreEqual(expectedText1, wordPulled1.TextLowerCase);

                // disposition the card so it doesn't come up when we pull the
                // next card
                OrchestrationApi.OrchestrateFlashCardDispositioning(
                    dbContextFactory, card, AvailableFlashCardAttemptStatus.EASY);

                var card2 = OrchestrationApi.OrchestratePullFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card2);
                var wordUserPulled2 = DataCache.WordUserByIdRead(card2.WordUserId, dbContextFactory);
                Assert.IsNotNull(wordUserPulled2);
                var wordPulled2 = DataCache.WordByIdRead(wordUserPulled2.WordId, dbContextFactory);
                Assert.IsNotNull(wordPulled2);
                Assert.AreEqual(expectedText2, wordPulled2.TextLowerCase);

            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }

        [TestMethod()]
        public async Task OrchestrateFlashCardDispositioningAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode learningLanguageCode = learningLanguage.Code;
            Language uiLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = uiLanguage.Code;

            string expectedText1 = "de";
            try
            {
                // make a context because we need to do some stuff to the DB
                // that's not supported by the API
                var context = dbContextFactory.CreateDbContext();


                // create the user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var word1 = await WordApi.WordReadByLanguageIdAndTextAsync(
                    dbContextFactory, learningLanguage.Id, expectedText1);
                Assert.IsNotNull(word1);
               
                // add the word user
                var wordUser1 = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);

                var card1 = await OrchestrationApi.OrchestratePullFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card1);


                // disposition the card to wrong. the expectation is that it
                // would be moved to 5 minutes from now
                await OrchestrationApi.OrchestrateFlashCardDispositioningAsync(
                    dbContextFactory, card1, AvailableFlashCardAttemptStatus.WRONG);
                Assert.IsTrue(card1.NextReview > DateTimeOffset.Now.AddMinutes(4));
                Assert.IsTrue(card1.NextReview < DateTimeOffset.Now.AddMinutes(6));

                // disposition the card to easy. the expectation is that it
                // would be moved to now plus 1.5 times the timespan between the
                // last 2 attempts

                // but first, let's update the prior review to 1 hour ago, to
                // make this more detectible
                var attempts = FlashCardAttemptApi.FlashCardAttemptsByFlashCardIdRead(
                    dbContextFactory, card1.Id);
                // should only be 1
                var lastAttempt = attempts.FirstOrDefault();
                Assert.IsNotNull(lastAttempt);
                var hourAgo = DateTimeOffset.Now.AddMinutes(-60);
                lastAttempt.AttemptedWhen = hourAgo;
                context.FlashCardAttempts.Update(lastAttempt);
                context.SaveChanges();
                // finally, we can disposition the card
                await OrchestrationApi.OrchestrateFlashCardDispositioningAsync(
                    dbContextFactory, card1, AvailableFlashCardAttemptStatus.EASY);
                // calculate what duration we think it should be
                var now = DateTimeOffset.Now;
                var timeBetweenLast2 = now - hourAgo;
                var minutes = timeBetweenLast2.TotalMinutes;
                minutes = minutes * 1.5; // the expected multiplier
                // give us a lower and upper range buffer
                int minutesHigh = (int)Math.Round(minutes * 1.1, 0);
                int minutesLow = (int)Math.Round(minutes * 0.9, 0);

                Assert.IsTrue(card1.NextReview > now.AddMinutes(minutesLow));
                Assert.IsTrue(card1.NextReview < now.AddMinutes(minutesHigh));
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }

        [TestMethod()]
        public void OrchestrateFlashCardDispositioningTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode learningLanguageCode = learningLanguage.Code;
            Language uiLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = uiLanguage.Code;

            string expectedText1 = "de";
            try
            {
                // make a context because we need to do some stuff to the DB
                // that's not supported by the API
                var context = dbContextFactory.CreateDbContext();


                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var word1 = WordApi.WordReadByLanguageIdAndText(
                    dbContextFactory, learningLanguage.Id, expectedText1);
                Assert.IsNotNull(word1);

                // add the word user
                var wordUser1 = WordUserApi.WordUserCreate(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);

                var card1 = OrchestrationApi.OrchestratePullFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card1);


                // disposition the card to wrong. the expectation is that it
                // would be moved to 5 minutes from now
                OrchestrationApi.OrchestrateFlashCardDispositioning(
                    dbContextFactory, card1, AvailableFlashCardAttemptStatus.WRONG);
                Assert.IsTrue(card1.NextReview > DateTimeOffset.Now.AddMinutes(4));
                Assert.IsTrue(card1.NextReview < DateTimeOffset.Now.AddMinutes(6));

                // disposition the card to easy. the expectation is that it
                // would be moved to now plus 1.5 times the timespan between the
                // last 2 attempts

                // but first, let's update the prior review to 1 hour ago, to
                // make this more detectible
                var attempts = FlashCardAttemptApi.FlashCardAttemptsByFlashCardIdRead(
                    dbContextFactory, card1.Id);
                // should only be 1
                var lastAttempt = attempts.FirstOrDefault();
                Assert.IsNotNull(lastAttempt);
                var hourAgo = DateTimeOffset.Now.AddMinutes(-60);
                lastAttempt.AttemptedWhen = hourAgo;
                context.FlashCardAttempts.Update(lastAttempt);
                context.SaveChanges();
                // finally, we can disposition the card
                OrchestrationApi.OrchestrateFlashCardDispositioning(
                    dbContextFactory, card1, AvailableFlashCardAttemptStatus.EASY);
                // calculate what duration we think it should be
                var now = DateTimeOffset.Now;
                var timeBetweenLast2 = now - hourAgo;
                var minutes = timeBetweenLast2.TotalMinutes;
                minutes = minutes * 1.5; // the expected multiplier
                // give us a lower and upper range buffer
                int minutesHigh = (int)Math.Round(minutes * 1.1, 0);
                int minutesLow = (int)Math.Round(minutes * 0.9, 0);

                Assert.IsTrue(card1.NextReview > now.AddMinutes(minutesLow));
                Assert.IsTrue(card1.NextReview < now.AddMinutes(minutesHigh));
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
    }
}