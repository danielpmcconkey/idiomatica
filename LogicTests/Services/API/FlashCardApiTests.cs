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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class FlashCardApiTests
    {
        [TestMethod()]
        public void FlashCardCreateTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a word
                var word = context.Words.Where(x => x.LanguageKey == learningLangugeId).Take(1).FirstOrDefault();
                if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = WordUserApi.WordUserCreate(
                    context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                FlashCard? card = FlashCardApi.FlashCardCreate(context, (Guid)wordUser.UniqueKey, uiLanguageCode);
                Assert.IsNotNull(card);
                Assert.IsNotNull(card.UniqueKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardCreateAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a word
                var word = context.Words.Where(x => x.LanguageKey == learningLangugeId).Take(1).FirstOrDefault();
                if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = await WordUserApi.WordUserCreateAsync(
                    context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                FlashCard? card = await FlashCardApi.FlashCardCreateAsync(context, (Guid)wordUser.UniqueKey, uiLanguageCode);
                Assert.IsNotNull(card);
                Assert.IsNotNull(card.UniqueKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void FlashCardDeckShuffleTest()
        {
            var context = CommonFunctions.CreateContext();

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
                    Assert.IsNotNull(card.UniqueKey);
                    newKey = $"{newKey}{card.UniqueKey}";
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
            var context = CommonFunctions.CreateContext();
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
                    Assert.IsNotNull(card.UniqueKey);
                    newKey = $"{newKey}{card.UniqueKey}";
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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageKey == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = FlashCardApi.FlashCardsCreate(
                    context, (Guid)languageUser.UniqueKey, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                foreach (var card in cards)
                {
                    if (card.UniqueKey is null || card.WordUserKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                    // pull the card again
                    var cardAfter = FlashCardApi.FlashCardReadById(context, (Guid)card.UniqueKey);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.UniqueKey);
                    Assert.AreEqual(card.NextReview, cardAfter.NextReview);
                    Assert.AreEqual(card.Status, cardAfter.Status);
                    Assert.AreEqual(card.UniqueKey, cardAfter.UniqueKey);
                    Assert.AreEqual(card.UniqueKey, cardAfter.UniqueKey);
                    Assert.AreEqual(card.WordUserKey, cardAfter.WordUserKey);
                }
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardReadByIdAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageKey == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    context, (Guid)languageUser.UniqueKey, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                foreach (var card in cards)
                {
                    if (card.UniqueKey is null || card.WordUserKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                    // pull the card again
                    var cardAfter = await FlashCardApi.FlashCardReadByIdAsync(context, (Guid)card.UniqueKey);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.UniqueKey);
                    Assert.AreEqual(card.NextReview, cardAfter.NextReview);
                    Assert.AreEqual(card.Status, cardAfter.Status);
                    Assert.AreEqual(card.UniqueKey, cardAfter.UniqueKey);
                    Assert.AreEqual(card.UniqueKey, cardAfter.UniqueKey);
                    Assert.AreEqual(card.WordUserKey, cardAfter.WordUserKey);
                }
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void FlashCardsCreateTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words, but only those that have tokens. if
                // you pull a word without a token, there won't be a paragraph
                // to translate and the assertion on the
                // FlashCardParagraphTranslationBridges.Count will fail
                var words = context.Words
                    .Where(x => x.LanguageKey == learningLangugeId && x.Tokens.Count > 0)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                }


                var cards = FlashCardApi.FlashCardsCreate(
                    context, (Guid)languageUser.UniqueKey, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);
                foreach (var card in cards)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.UniqueKey);
                    Assert.IsNotNull(card.FlashCardParagraphTranslationBridges);
                    Assert.IsTrue(card.FlashCardParagraphTranslationBridges.Count > 0);
                    Assert.IsNotNull(card.Status);
                    Assert.IsNotNull(card.UniqueKey);
                }

            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardsCreateAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words, but only those that have tokens. if
                // you pull a word without a token, there won't be a paragraph
                // to translate and the assertion on the
                // FlashCardParagraphTranslationBridges.Count will fail
                var words = context.Words
                    .Where(x => x.LanguageKey == learningLangugeId && x.Tokens.Count > 0)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                }


                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    context, (Guid)languageUser.UniqueKey, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);
                foreach (var card in cards)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.UniqueKey);
                    Assert.IsNotNull(card.FlashCardParagraphTranslationBridges);
                    Assert.IsTrue(card.FlashCardParagraphTranslationBridges.Count > 0);
                    Assert.IsNotNull(card.Status);
                    Assert.IsNotNull(card.UniqueKey);
                }

            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void FlashCardsFetchByNextReviewDateByPredicateTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words, but only those that have tokens. if
                // you pull a word without a token, there won't be a paragraph
                // to translate and the assertion on the
                // FlashCardParagraphTranslationBridges.Count will fail
                var words = context.Words
                    .Where(x => x.LanguageKey == learningLangugeId && x.Tokens.Count > 0)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = FlashCardApi.FlashCardsCreate(
                    context, (Guid)languageUser.UniqueKey, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);
                foreach (var card in cards)
                {
                    if (card.UniqueKey is null || card.WordUserKey is null) { ErrorHandler.LogAndThrow(); return; }
                    // update next review to 5 mins ago, so we can pull them
                    FlashCardApi.FlashCardUpdate(
                        context, (Guid)card.UniqueKey, (Guid)card.WordUserKey,
                        AvailableFlashCardStatus.ACTIVE, DateTime.Now.AddMinutes(-5),
                        (Guid)card.UniqueKey);
                }

                // now pull them 
                Expression<Func<FlashCard, bool>> predicate = fc => fc.WordUser != null
                    && fc.WordUser.LanguageUserKey == languageUser.UniqueKey
                    && fc.Status == AvailableFlashCardStatus.ACTIVE
                    && fc.NextReview != null
                    && fc.NextReview <= DateTime.Now;

                var cardsPulled = FlashCardApi.FlashCardsFetchByNextReviewDateByPredicate(
                    context, predicate, numCards);

                Assert.IsNotNull(cardsPulled);
                Assert.IsTrue(cardsPulled.Count == numCards);
                foreach (var card in cardsPulled)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.UniqueKey);
                    Assert.IsNotNull(card.FlashCardParagraphTranslationBridges);
                    Assert.IsTrue(card.FlashCardParagraphTranslationBridges.Count > 0);
                    Assert.IsNotNull(card.Status);
                    Assert.IsNotNull(card.NextReview);
                    Assert.IsNotNull(card.UniqueKey);
                }

            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardsFetchByNextReviewDateByPredicateAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words, but only those that have tokens. if
                // you pull a word without a token, there won't be a paragraph
                // to translate and the assertion on the
                // FlashCardParagraphTranslationBridges.Count will fail
                var words = context.Words
                    .Where(x => x.LanguageKey == learningLangugeId && x.Tokens.Count > 0)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    context, (Guid)languageUser.UniqueKey, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);
                foreach (var card in cards)
                {
                    if (card.UniqueKey is null || card.WordUserKey is null) { ErrorHandler.LogAndThrow(); return; }
                    // update next review to 5 mins ago, so we can pull them
                    await FlashCardApi.FlashCardUpdateAsync(
                        context, (Guid)card.UniqueKey, (Guid)card.WordUserKey,
                        AvailableFlashCardStatus.ACTIVE, DateTime.Now.AddMinutes(-5),
                        (Guid)card.UniqueKey);
                }

                // now pull them 
                Expression<Func<FlashCard, bool>> predicate = fc => fc.WordUser != null
                    && fc.WordUser.LanguageUserKey == languageUser.UniqueKey
                    && fc.Status == AvailableFlashCardStatus.ACTIVE
                    && fc.NextReview != null
                    && fc.NextReview <= DateTime.Now;

                var cardsPulled = await FlashCardApi.FlashCardsFetchByNextReviewDateByPredicateAsync(
                    context, predicate, numCards);

                Assert.IsNotNull(cardsPulled);
                Assert.IsTrue(cardsPulled.Count == numCards);
                foreach (var card in cardsPulled)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.UniqueKey);
                    Assert.IsNotNull(card.FlashCardParagraphTranslationBridges);
                    Assert.IsTrue(card.FlashCardParagraphTranslationBridges.Count > 0);
                    Assert.IsNotNull(card.Status);
                    Assert.IsNotNull(card.NextReview);
                    Assert.IsNotNull(card.UniqueKey);
                }

            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void FlashCardUpdateTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageKey == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = FlashCardApi.FlashCardsCreate(
                    context, (Guid)languageUser.UniqueKey, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                DateTime futureTime = DateTime.Now.AddMinutes(7);

                foreach (var card in cards)
                {
                    if (card.UniqueKey is null || card.WordUserKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                    // update card
                    FlashCardApi.FlashCardUpdate(context, (Guid)card.UniqueKey,
                        (Guid)card.WordUserKey, AvailableFlashCardStatus.DONTUSE,
                        futureTime, (Guid)card.UniqueKey);

                    // pull the card again
                    var cardAfter = FlashCardApi.FlashCardReadById(context, (Guid)card.UniqueKey);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.UniqueKey);
                    Assert.AreEqual(futureTime, cardAfter.NextReview);
                    Assert.AreEqual(AvailableFlashCardStatus.DONTUSE, cardAfter.Status);
                }

            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardUpdateAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid learningLangugeId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageKey == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, (Guid)word.UniqueKey, (Guid)languageUser.UniqueKey, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    context, (Guid)languageUser.UniqueKey, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                DateTime futureTime = DateTime.Now.AddMinutes(7);

                foreach (var card in cards)
                {
                    if (card.UniqueKey is null || card.WordUserKey is null)
                    { ErrorHandler.LogAndThrow(); return; }

                    // update card
                    await FlashCardApi.FlashCardUpdateAsync(context, (Guid)card.UniqueKey,
                        (Guid)card.WordUserKey, AvailableFlashCardStatus.DONTUSE,
                        futureTime, (Guid)card.UniqueKey);

                    // pull the card again
                    var cardAfter = await FlashCardApi.FlashCardReadByIdAsync(context, (Guid)card.UniqueKey);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.UniqueKey);
                    Assert.AreEqual(futureTime, cardAfter.NextReview);
                    Assert.AreEqual(AvailableFlashCardStatus.DONTUSE, cardAfter.Status);
                }

            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void FlashCardReadByWordUserIdTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            Guid wordId = CommonFunctions.GetWordKeyByTextLower(context, languageId, "dice");
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, languageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);

                // create the wordUser
                var wordUser = WordUserApi.WordUserCreate(
                    context, wordId, (Guid)languageUser.UniqueKey, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.UniqueKey);
                Assert.AreEqual(wordId, wordUser.WordKey);

                // create the flashcard
                FlashCardApi.FlashCardCreate(context, (Guid)wordUser.UniqueKey, uiLanguageCode);

                // read the flashcard
                var flashCard = FlashCardApi.FlashCardReadByWordUserId(context, (Guid)wordUser.UniqueKey);

                Assert.IsNotNull(flashCard);
                Assert.IsNotNull(flashCard.UniqueKey);
                Assert.AreEqual(wordUser.UniqueKey, flashCard.WordUserKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task FlashCardReadByWordUserIdAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            string uiLanguageCode = "EN-US";
            Guid wordId = CommonFunctions.GetWordKeyByTextLower(context, languageId, "dice");
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, languageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);

                // create the wordUser
                var wordUser = WordUserApi.WordUserCreate(
                    context, wordId, (Guid)languageUser.UniqueKey, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.UniqueKey);
                Assert.AreEqual(wordId, wordUser.WordKey);

                // create the flashcard
                await FlashCardApi.FlashCardCreateAsync(context, (Guid)wordUser.UniqueKey, uiLanguageCode);

                // read the flashcard
                var flashCard = await FlashCardApi.FlashCardReadByWordUserIdAsync(context, (Guid)wordUser.UniqueKey);
                
                Assert.IsNotNull(flashCard);
                Assert.IsNotNull(flashCard.UniqueKey);
                Assert.AreEqual(wordUser.UniqueKey, flashCard.WordUserKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
    }
}