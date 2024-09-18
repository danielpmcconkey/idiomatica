using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a word
                var word = context.Words.Where(x => x.LanguageId == learningLanguage.Id).Take(1).FirstOrDefault();
                if (word is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = WordUserApi.WordUserCreate(
                    context, word, languageUser, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }

                FlashCard? card = FlashCardApi.FlashCardCreate(dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);
                Assert.IsNotNull(card);
                Assert.IsNotNull(card.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardCreateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;

            try
            {
                

                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a word
                var word = context.Words.Where(x => x.LanguageId == learningLanguage.Id).Take(1).FirstOrDefault();
                if (word is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = await WordUserApi.WordUserCreateAsync(
                    context, word, languageUser, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }

                FlashCard? card = await FlashCardApi.FlashCardCreateAsync(dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);
                Assert.IsNotNull(card);
                Assert.IsNotNull(card.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
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
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
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
                        context, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = FlashCardApi.FlashCardsCreate(
                    dbContextFactory, (Guid)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                foreach (var card in cards)
                {
                    // pull the card again
                    var cardAfter = FlashCardApi.FlashCardReadById(context, (Guid)card.Id);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.Id);
                    Assert.AreEqual(card.NextReview, cardAfter.NextReview);
                    Assert.AreEqual(card.Status, cardAfter.Status);
                    Assert.AreEqual(card.Id, cardAfter.Id);
                    Assert.AreEqual(card.Id, cardAfter.Id);
                    Assert.AreEqual(card.WordUserId, cardAfter.WordUserId);
                }
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardReadByIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
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
                        context, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    dbContextFactory, (Guid)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                foreach (var card in cards)
                {
                    // pull the card again
                    var cardAfter = await FlashCardApi.FlashCardReadByIdAsync(context, (Guid)card.Id);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.Id);
                    Assert.AreEqual(card.NextReview, cardAfter.NextReview);
                    Assert.AreEqual(card.Status, cardAfter.Status);
                    Assert.AreEqual(card.Id, cardAfter.Id);
                    Assert.AreEqual(card.Id, cardAfter.Id);
                    Assert.AreEqual(card.WordUserId, cardAfter.WordUserId);
                }
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void FlashCardsCreateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a bunch of words, but only those that have tokens. if
                // you pull a word without a token, there won't be a paragraph
                // to translate and the assertion on the
                // FlashCardParagraphTranslationBridges.Count will fail
                var words = context.Words
                    .Where(x => x.LanguageId == learningLanguage.Id && x.Tokens.Count > 0)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }


                var cards = FlashCardApi.FlashCardsCreate(
                    dbContextFactory, (Guid)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);
                foreach (var card in cards)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.Id);
                    Assert.IsNotNull(card.FlashCardParagraphTranslationBridges);
                    Assert.IsTrue(card.FlashCardParagraphTranslationBridges.Count > 0);
                    Assert.IsNotNull(card.Status);
                    Assert.IsNotNull(card.Id);
                }

            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardsCreateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a bunch of words, but only those that have tokens. if
                // you pull a word without a token, there won't be a paragraph
                // to translate and the assertion on the
                // FlashCardParagraphTranslationBridges.Count will fail
                var words = context.Words
                    .Where(x => x.LanguageId == learningLanguage.Id && x.Tokens.Count > 0)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }


                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    dbContextFactory, (Guid)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);
                foreach (var card in cards)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.Id);
                    Assert.IsNotNull(card.FlashCardParagraphTranslationBridges);
                    Assert.IsTrue(card.FlashCardParagraphTranslationBridges.Count > 0);
                    Assert.IsNotNull(card.Status);
                    Assert.IsNotNull(card.Id);
                }

            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void FlashCardsFetchByNextReviewDateByPredicateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a bunch of words, but only those that have tokens. if
                // you pull a word without a token, there won't be a paragraph
                // to translate and the assertion on the
                // FlashCardParagraphTranslationBridges.Count will fail
                var words = context.Words
                    .Where(x => x.LanguageId == learningLanguage.Id && x.Tokens.Count > 0)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = FlashCardApi.FlashCardsCreate(
                    dbContextFactory, (Guid)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.AreEqual(numCards, cards.Count);
                foreach (var card in cards)
                {
                    // update next review to 5 mins ago, so we can pull them
                    FlashCardApi.FlashCardUpdate(
                        context, (Guid)card.Id, (Guid)card.WordUserId,
                        AvailableFlashCardStatus.ACTIVE, DateTime.Now.AddMinutes(-5),
                        (Guid)card.Id);
                }

                // now pull them 
                Expression<Func<FlashCard, bool>> predicate = fc => fc.WordUser != null
                    && fc.WordUser.LanguageUserId == languageUser.Id
                    && fc.Status == AvailableFlashCardStatus.ACTIVE
                    && fc.NextReview != null
                    && fc.NextReview <= DateTimeOffset.Now;

                var cardsPulled = FlashCardApi.FlashCardsFetchByNextReviewDateByPredicate(
                    context, predicate, numCards);

                Assert.IsNotNull(cardsPulled);
                Assert.AreEqual(numCards, cardsPulled.Count);
                foreach (var card in cardsPulled)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.Id);
                    Assert.IsNotNull(card.FlashCardParagraphTranslationBridges);
                    Assert.IsTrue(card.FlashCardParagraphTranslationBridges.Count > 0);
                    Assert.IsNotNull(card.Status);
                    Assert.IsNotNull(card.NextReview);
                    Assert.IsNotNull(card.Id);
                }

            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardsFetchByNextReviewDateByPredicateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a bunch of words, but only those that have tokens. if
                // you pull a word without a token, there won't be a paragraph
                // to translate and the assertion on the
                // FlashCardParagraphTranslationBridges.Count will fail
                var words = context.Words
                    .Where(x => x.LanguageId == learningLanguage.Id && x.Tokens.Count > 0)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    dbContextFactory, (Guid)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.AreEqual(numCards, cards.Count);
                foreach (var card in cards)
                {
                    // update next review to 5 mins ago, so we can pull them
                    await FlashCardApi.FlashCardUpdateAsync(
                        context, (Guid)card.Id, (Guid)card.WordUserId,
                        AvailableFlashCardStatus.ACTIVE, DateTime.Now.AddMinutes(-5),
                        (Guid)card.Id);
                }

                // now pull them 
                Expression<Func<FlashCard, bool>> predicate = fc => fc.WordUser != null
                    && fc.WordUser.LanguageUserId == languageUser.Id
                    && fc.Status == AvailableFlashCardStatus.ACTIVE
                    && fc.NextReview != null
                    && fc.NextReview <= DateTimeOffset.Now;

                var cardsPulled = await FlashCardApi.FlashCardsFetchByNextReviewDateByPredicateAsync(
                    context, predicate, numCards);

                Assert.IsNotNull(cardsPulled);
                Assert.AreEqual(numCards, cardsPulled.Count);
                foreach (var card in cardsPulled)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.Id);
                    Assert.IsNotNull(card.FlashCardParagraphTranslationBridges);
                    Assert.IsTrue(card.FlashCardParagraphTranslationBridges.Count > 0);
                    Assert.IsNotNull(card.Status);
                    Assert.IsNotNull(card.NextReview);
                    Assert.IsNotNull(card.Id);
                }

            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void FlashCardUpdateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
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
                        context, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = FlashCardApi.FlashCardsCreate(
                    dbContextFactory, (Guid)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                DateTime futureTime = DateTime.Now.AddMinutes(7);

                foreach (var card in cards)
                {
                    // update card
                    FlashCardApi.FlashCardUpdate(context, (Guid)card.Id,
                        (Guid)card.WordUserId, AvailableFlashCardStatus.DONTUSE,
                        futureTime, (Guid)card.Id);

                    // pull the card again
                    var cardAfter = FlashCardApi.FlashCardReadById(context, (Guid)card.Id);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.Id);
                    Assert.AreEqual(futureTime, cardAfter.NextReview);
                    Assert.AreEqual(AvailableFlashCardStatus.DONTUSE, cardAfter.Status);
                }

            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardUpdateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                Assert.IsNotNull(user.Id);
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
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
                        context, word, languageUser, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    dbContextFactory, (Guid)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count() == numCards);


                DateTime futureTime = DateTime.Now.AddMinutes(7);

                foreach (var card in cards)
                {
                    // update card
                    await FlashCardApi.FlashCardUpdateAsync(context, (Guid)card.Id,
                        (Guid)card.WordUserId, AvailableFlashCardStatus.DONTUSE,
                        futureTime, (Guid)card.Id);

                    // pull the card again
                    var cardAfter = await FlashCardApi.FlashCardReadByIdAsync(context, (Guid)card.Id);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.Id);
                    Assert.AreEqual(futureTime, cardAfter.NextReview);
                    Assert.AreEqual(AvailableFlashCardStatus.DONTUSE, cardAfter.Status);
                }

            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void FlashCardReadByWordUserIdTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);
            Guid languageId = language.Id;
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            Guid wordId = CommonFunctions.GetWordIdByTextLower(context, languageId, "dice");
            var word = WordApi.WordGetById(context, wordId);
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                Assert.IsNotNull(word);

                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user); 
                Assert.IsNotNull(user.Id);
                userId = (Guid)user.Id;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, language, user);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);

                // create the wordUser
                var wordUser = WordUserApi.WordUserCreate(
                    context, word, languageUser, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);

                // create the flashcard
                FlashCardApi.FlashCardCreate(dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);

                // read the flashcard
                var flashCard = FlashCardApi.FlashCardReadByWordUserId(context, (Guid)wordUser.Id);

                Assert.IsNotNull(flashCard);
                Assert.IsNotNull(flashCard.Id);
                Assert.AreEqual(wordUser.Id, flashCard.WordUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardReadByWordUserIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);
            Guid languageId = language.Id;
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            Guid wordId = CommonFunctions.GetWordIdByTextLower(context, languageId, "dice");
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;
            var word = WordApi.WordGetById(context, wordId);

            try
            {
                Assert.IsNotNull(word);

                // create the user
                if (loginService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (Guid)user.Id;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, language, user);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);

                // create the wordUser
                var wordUser = WordUserApi.WordUserCreate(
                    context, word, languageUser, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);

                // create the flashcard
                await FlashCardApi.FlashCardCreateAsync(dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);

                // read the flashcard
                var flashCard = await FlashCardApi.FlashCardReadByWordUserIdAsync(context, (Guid)wordUser.Id);
                
                Assert.IsNotNull(flashCard);
                Assert.IsNotNull(flashCard.Id);
                Assert.AreEqual(wordUser.Id, flashCard.WordUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
    }
}