﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Model.DAL;
using Model;
using Logic.Telemetry;
using System.Linq.Expressions;
using Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class FlashCardApiTests
    {
        [TestMethod()]
        public void FlashCardCreateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a word
                var word = context.Words.Where(x => x.LanguageId == learningLanguage.Id).Take(1).FirstOrDefault();
                if (word is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = WordUserApi.WordUserCreate(
                    dbContextFactory, word, languageUser, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }

                FlashCard? card = FlashCardApi.FlashCardCreate(dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);
                Assert.IsNotNull(card);
                Assert.IsNotNull(card.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task FlashCardCreateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;

            try
            {


                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a word
                var word = context.Words.Where(x => x.LanguageId == learningLanguage.Id)
                    .Take(1).FirstOrDefault();
                if (word is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word, languageUser, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }

                FlashCard? card = await FlashCardApi.FlashCardCreateAsync(dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);
                Assert.IsNotNull(card);
                Assert.IsNotNull(card.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void FlashCardDeckShuffleTest()
        {

            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            HashSet<string> keys = new();
            int numShuffles = 20;
            int numDuplicates = 0;
            float threshold = 0.2F;

            List<FlashCard> deck = context.FlashCards.Take(5).ToList();

            for (int i = 0; i < numShuffles; i++)
            {
                deck = FlashCardApi.FlashCardDeckShuffle(deck);
                string newKey = "";
                foreach (var card in deck)
                {
                    Assert.IsNotNull(card.Id);
                    newKey = $"{newKey}{card.Id}";
                }
                if (keys.Contains(newKey)) numDuplicates++;
                else keys.Add(newKey);
            }
            float actual = numDuplicates / (float)numShuffles;
            Assert.IsTrue(actual <= threshold);
        }
        [TestMethod()]
        public async Task FlashCardDeckShuffleAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            HashSet<string> keys = new();
            int numShuffles = 20;
            int numDuplicates = 0;
            float threshold = 0.2F;

            List<FlashCard> deck = context.FlashCards.Take(5).ToList();

            for (int i = 0; i < numShuffles; i++)
            {
                deck = await FlashCardApi.FlashCardDeckShuffleAsync(deck);
                string newKey = "";
                foreach (var card in deck)
                {
                    Assert.IsNotNull(card.Id);
                    newKey = $"{newKey}{card.Id}";
                }
                if (keys.Contains(newKey)) numDuplicates++;
                else keys.Add(newKey);
            }
            float actual = numDuplicates / (float)numShuffles;
            Assert.IsTrue(actual <= threshold);
        }


        [TestMethod()]
        public void FlashCardReadByIdTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode learningLanguageCode = learningLanguage.Code;
            Language uiLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = uiLanguage.Code;

            string expectedText = "de";

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
                    dbContextFactory, learningLanguage.Id, expectedText);
                Assert.IsNotNull(word1);

                // add the word user
                var wordUser1 = WordUserApi.WordUserCreate(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);

                var card1 = OrchestrationApi.OrchestratePullFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card1);

                var card2 = FlashCardApi.FlashCardReadById(dbContextFactory, card1.Id);

                Assert.IsNotNull(card2);
                var wordUserPulled = DataCache.WordUserByIdRead(
                    card2.WordUserId, dbContextFactory);
                Assert.IsNotNull(wordUserPulled);
                var wordPulled = DataCache.WordByIdRead(
                    wordUserPulled.WordId, dbContextFactory);
                Assert.IsNotNull(wordPulled);
                Assert.AreEqual(expectedText, wordPulled.TextLowerCase);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser(
                    (Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task FlashCardReadByIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode learningLanguageCode = learningLanguage.Code;
            Language uiLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = uiLanguage.Code;

            string expectedText = "de";

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
                    dbContextFactory, learningLanguage.Id, expectedText);
                Assert.IsNotNull(word1);

                // add the word user
                var wordUser1 = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);

                var card1 = await OrchestrationApi.OrchestratePullFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card1);

                var card2 = await FlashCardApi.FlashCardReadByIdAsync(dbContextFactory, card1.Id);

                Assert.IsNotNull(card2);
                var wordUserPulled = await DataCache.WordUserByIdReadAsync(
                    card2.WordUserId, dbContextFactory);
                Assert.IsNotNull(wordUserPulled);
                var wordPulled = await DataCache.WordByIdReadAsync(
                    wordUserPulled.WordId, dbContextFactory);
                Assert.IsNotNull(wordPulled);
                Assert.AreEqual(expectedText, wordPulled.TextLowerCase);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync(
                    (Guid)userId, dbContextFactory);
            }
        }


        


        [TestMethod()]
        public void FlashCardUpdateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 5;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLanguage.Id)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        dbContextFactory, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create card
                var card = OrchestrationApi.OrchestratePullFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguage.Code, uiLanguageCode);
                Assert.IsNotNull(card);

                DateTime futureTime = DateTime.Now.AddMinutes(7);


                // update card
                FlashCardApi.FlashCardUpdate(dbContextFactory, (Guid)card.Id,
                    (Guid)card.WordUserId, AvailableFlashCardStatus.DONTUSE,
                    futureTime, (Guid)card.Id);

                // pull the card again
                var cardAfter = FlashCardApi.FlashCardReadById(dbContextFactory, (Guid)card.Id);
                Assert.IsNotNull(cardAfter);
                Assert.IsNotNull(cardAfter.Id);
                Assert.AreEqual(futureTime, cardAfter.NextReview);
                Assert.AreEqual(AvailableFlashCardStatus.DONTUSE, cardAfter.Status);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task FlashCardUpdateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 5;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLanguage.Id)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        dbContextFactory, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create card
                var card = await OrchestrationApi.OrchestratePullFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguage.Code, uiLanguageCode);
                Assert.IsNotNull(card);

                DateTime futureTime = DateTime.Now.AddMinutes(7);

                
                // update card
                await FlashCardApi.FlashCardUpdateAsync(dbContextFactory, (Guid)card.Id,
                    (Guid)card.WordUserId, AvailableFlashCardStatus.DONTUSE,
                    futureTime, (Guid)card.Id);

                // pull the card again
                var cardAfter = await FlashCardApi.FlashCardReadByIdAsync(dbContextFactory, (Guid)card.Id);
                Assert.IsNotNull(cardAfter);
                Assert.IsNotNull(cardAfter.Id);
                Assert.AreEqual(futureTime, cardAfter.NextReview);
                Assert.AreEqual(AvailableFlashCardStatus.DONTUSE, cardAfter.Status);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void FlashCardReadByWordUserIdTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid languageId = language.Id;
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            Guid wordId = CommonFunctions.GetWordIdByTextLower(dbContextFactory, languageId, "dice");
            var word = WordApi.WordGetById(dbContextFactory, wordId);
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                Assert.IsNotNull(word);

                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, languageId, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the wordUser
                var wordUser = WordUserApi.WordUserCreate(
                    dbContextFactory, word, languageUser, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);

                // create the flashcard
                FlashCardApi.FlashCardCreate(dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);

                // read the flashcard
                var flashCard = FlashCardApi.FlashCardReadByWordUserId(dbContextFactory, (Guid)wordUser.Id);

                Assert.IsNotNull(flashCard);
                Assert.IsNotNull(flashCard.Id);
                Assert.AreEqual(wordUser.Id, flashCard.WordUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task FlashCardReadByWordUserIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid languageId = language.Id;
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            Guid wordId = CommonFunctions.GetWordIdByTextLower(dbContextFactory, languageId, "dice");
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;
            var word = WordApi.WordGetById(dbContextFactory, wordId);

            try
            {
                Assert.IsNotNull(word);

                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, languageId, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the wordUser
                var wordUser = WordUserApi.WordUserCreate(
                    dbContextFactory, word, languageUser, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);

                // create the flashcard
                await FlashCardApi.FlashCardCreateAsync(dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);

                // read the flashcard
                var flashCard = await FlashCardApi.FlashCardReadByWordUserIdAsync(dbContextFactory, (Guid)wordUser.Id);

                Assert.IsNotNull(flashCard);
                Assert.IsNotNull(flashCard.Id);
                Assert.AreEqual(wordUser.Id, flashCard.WordUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public async Task FlashCardReadNextReviewCardAsyncTest()
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


                var card1 = await OrchestrationApi.OrchestratePullFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card1);

                // update the card's review date to tomorrow so it won't come
                // up when we pull the next card
                card1.NextReview = DateTimeOffset.Now.AddDays(1);
                await FlashCardApi.FlashCardUpdateAsync(dbContextFactory, card1);

                var card2 = await OrchestrationApi.OrchestratePullFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card2);

                // update the card's review date to 10 minutes ago, so it'll come up next
                card2.NextReview = DateTimeOffset.Now.AddMinutes(-10);
                await FlashCardApi.FlashCardUpdateAsync(dbContextFactory, card2);

                // now pull the next review card...
                var card3 = await FlashCardApi.FlashCardReadNextReviewCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode);
                Assert.IsNotNull(card3);

                Assert.AreEqual(card2.Id, card3.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public void FlashCardReadNextReviewCardTest()
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


                var card1 = OrchestrationApi.OrchestratePullFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card1);

                // update the card's review date to tomorrow so it won't come
                // up when we pull the next card
                card1.NextReview = DateTimeOffset.Now.AddDays(1);
                FlashCardApi.FlashCardUpdate(dbContextFactory, card1);

                var card2 = OrchestrationApi.OrchestratePullFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);

                Assert.IsNotNull(card2);

                // update the card's review date to 10 minutes ago, so it'll come up next
                card2.NextReview = DateTimeOffset.Now.AddMinutes(-10);
                FlashCardApi.FlashCardUpdate(dbContextFactory, card2);

                // now pull the next review card...
                var card3 = FlashCardApi.FlashCardReadNextReviewCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode);
                Assert.IsNotNull(card3);

                Assert.AreEqual(card2.Id, card3.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }

        [TestMethod()]
        public async Task FlashCardUpdateAsyncTestSimplified()
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
                

                // add some word users
                var wordUser1 = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);
                


                var card = await OrchestrationApi.OrchestratePullFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);
                Assert.IsNotNull(card);

                card.Status = AvailableFlashCardStatus.DONTUSE;
                var future = DateTimeOffset.Now.AddHours(4);
                card.NextReview = future;

                await FlashCardApi.FlashCardUpdateAsync(dbContextFactory, card);

                // I don't know if this is really testing anything as the
                // context will likely hold on to the reference to the object
                // regardless. I could maybe delete the cache, but that might
                // mess with other tests being conducted at the same time
                var cardPulled = await FlashCardApi.FlashCardReadByIdAsync(
                    dbContextFactory, card.Id);
                Assert.IsNotNull(cardPulled);
                Assert.AreEqual(AvailableFlashCardStatus.DONTUSE, cardPulled.Status);
                Assert.AreEqual(future, cardPulled.NextReview);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }

        [TestMethod()]
        public void FlashCardUpdateTestSimplified()
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


                // add some word users
                var wordUser1 = WordUserApi.WordUserCreate(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);



                var card = OrchestrationApi.OrchestratePullFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);
                Assert.IsNotNull(card);

                card.Status = AvailableFlashCardStatus.DONTUSE;
                var future = DateTimeOffset.Now.AddHours(4);
                card.NextReview = future;

                FlashCardApi.FlashCardUpdate(dbContextFactory, card);

                // I don't know if this is really testing anything as the
                // context will likely hold on to the reference to the object
                // regardless. I could maybe delete the cache, but that might
                // mess with other tests being conducted at the same time
                var cardPulled = FlashCardApi.FlashCardReadById(
                    dbContextFactory, card.Id);
                Assert.IsNotNull(cardPulled);
                Assert.AreEqual(AvailableFlashCardStatus.DONTUSE, cardPulled.Status);
                Assert.AreEqual(future, cardPulled.NextReview);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
    }
}