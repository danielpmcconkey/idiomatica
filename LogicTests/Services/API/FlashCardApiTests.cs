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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class FlashCardApiTests
    {
        [TestMethod()]
        public void FlashCardCreateTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a word
                var word = context.Words.Where(x => x.LanguageId == learningLangugeId).Take(1).FirstOrDefault();
                if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = WordUserApi.WordUserCreate(
                    context, (int)word.Id, (int)languageUser.Id, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                FlashCard? card = FlashCardApi.FlashCardCreate(context, (int)wordUser.Id, uiLanguageCode);
                Assert.IsNotNull(card);
                Assert.IsNotNull(card.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a word
                var word = context.Words.Where(x => x.LanguageId == learningLangugeId).Take(1).FirstOrDefault();
                if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = await WordUserApi.WordUserCreateAsync(
                    context, (int)word.Id, (int)languageUser.Id, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                FlashCard? card = await FlashCardApi.FlashCardCreateAsync(context, (int)wordUser.Id, uiLanguageCode);
                Assert.IsNotNull(card);
                Assert.IsNotNull(card.Id);
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
            HashSet<string> keys = new();
            int numShuffles = 20;
            int numDuplicates = 0;
            float threshold = 0.2F;

            List<FlashCard> deck = new();
            deck.Add(new FlashCard() { Id = 1 });
            deck.Add(new FlashCard() { Id = 2 });
            deck.Add(new FlashCard() { Id = 3 });
            deck.Add(new FlashCard() { Id = 4 });
            deck.Add(new FlashCard() { Id = 5 });

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
            HashSet<string> keys = new();
            int numShuffles = 20;
            int numDuplicates = 0;
            float threshold = 0.2F;

            List<FlashCard> deck = new();
            deck.Add(new FlashCard() { Id = 1 });
            deck.Add(new FlashCard() { Id = 2 });
            deck.Add(new FlashCard() { Id = 3 });
            deck.Add(new FlashCard() { Id = 4 });
            deck.Add(new FlashCard() { Id = 5 });

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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, (int)word.Id, (int)languageUser.Id, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = FlashCardApi.FlashCardsCreate(
                    context, (int)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                foreach (var card in cards)
                {
                    if (card.Id is null || card.WordUserId is null)
                    { ErrorHandler.LogAndThrow(); return; }

                    // pull the card again
                    var cardAfter = FlashCardApi.FlashCardReadById(context, (int)card.Id);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.Id);
                    Assert.AreEqual(card.NextReview, cardAfter.NextReview);
                    Assert.AreEqual(card.Status, cardAfter.Status);
                    Assert.AreEqual(card.UniqueKey, cardAfter.UniqueKey);
                    Assert.AreEqual(card.Id, cardAfter.Id);
                    Assert.AreEqual(card.WordUserId, cardAfter.WordUserId);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, (int)word.Id, (int)languageUser.Id, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    context, (int)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                foreach (var card in cards)
                {
                    if (card.Id is null || card.WordUserId is null)
                    { ErrorHandler.LogAndThrow(); return; }

                    // pull the card again
                    var cardAfter = await FlashCardApi.FlashCardReadByIdAsync(context, (int)card.Id);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.Id);
                    Assert.AreEqual(card.NextReview, cardAfter.NextReview);
                    Assert.AreEqual(card.Status, cardAfter.Status);
                    Assert.AreEqual(card.UniqueKey, cardAfter.UniqueKey);
                    Assert.AreEqual(card.Id, cardAfter.Id);
                    Assert.AreEqual(card.WordUserId, cardAfter.WordUserId);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, (int)word.Id, (int)languageUser.Id, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                }


                var cards = FlashCardApi.FlashCardsCreate(
                    context, (int)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);
                foreach (var card in cards)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.Id);
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
        public async Task FlashCardsCreateAsyncTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, (int)word.Id, (int)languageUser.Id, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                }


                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    context, (int)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);
                foreach (var card in cards)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.Id);
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
        public void FlashCardsFetchByNextReviewDateByLanguageUserTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, (int)word.Id, (int)languageUser.Id, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = FlashCardApi.FlashCardsCreate(
                    context, (int)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);

                // now pull them 
                var cardsPulled = FlashCardApi.FlashCardsFetchByNextReviewDateByLanguageUser(
                    context, (int)languageUser.Id, numCards);

                Assert.IsNotNull(cardsPulled);
                Assert.IsTrue(cardsPulled.Count == numCards);
                foreach (var card in cardsPulled)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.Id);
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
        public async Task FlashCardsFetchByNextReviewDateByLanguageUserAsyncTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";
            int numWords = 20;
            int numCards = 5;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, (int)word.Id, (int)languageUser.Id, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    context, (int)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);

                // now pull them 
                var cardsPulled = await FlashCardApi.FlashCardsFetchByNextReviewDateByLanguageUserAsync(
                    context, (int)languageUser.Id, numCards);

                Assert.IsNotNull(cardsPulled);
                Assert.IsTrue(cardsPulled.Count == numCards);
                foreach (var card in cardsPulled)
                {
                    Assert.IsNotNull(card);
                    Assert.IsNotNull(card.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = WordUserApi.WordUserCreate(
                        context, (int)word.Id, (int)languageUser.Id, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = FlashCardApi.FlashCardsCreate(
                    context, (int)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                DateTime futureTime = DateTime.Now.AddMinutes(7);

                foreach (var card in cards)
                {
                    if (card.Id is null || card.WordUserId is null)
                    { ErrorHandler.LogAndThrow(); return; }

                    // update card
                    FlashCardApi.FlashCardUpdate(context, (int)card.Id,
                        (int)card.WordUserId, AvailableFlashCardStatus.DONTUSE,
                        futureTime, card.UniqueKey);

                    // pull the card again
                    var cardAfter = FlashCardApi.FlashCardReadById(context, (int)card.Id);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";
            int numWords = 5;
            int numCards = 1;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLangugeId, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a bunch of words
                var words = context.Words
                    .Where(x => x.LanguageId == learningLangugeId)
                    .Take(numWords)
                    .ToList();
                if (words is null || words.Count != numWords) { ErrorHandler.LogAndThrow(); return; }

                // create WordUsers for each
                foreach (var word in words)
                {
                    if (word is null || word.Id is null) { ErrorHandler.LogAndThrow(); return; }
                    var wordUser = await WordUserApi.WordUserCreateAsync(
                        context, (int)word.Id, (int)languageUser.Id, "test", AvailableWordUserStatus.LEARNING3);
                    if (wordUser is null || wordUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                }

                // create cards
                var cards = await FlashCardApi.FlashCardsCreateAsync(
                    context, (int)languageUser.Id, numCards, uiLanguageCode);
                Assert.IsNotNull(cards);
                Assert.IsTrue(cards.Count == numCards);


                DateTime futureTime = DateTime.Now.AddMinutes(7);

                foreach (var card in cards)
                {
                    if (card.Id is null || card.WordUserId is null)
                    { ErrorHandler.LogAndThrow(); return; }

                    // update card
                    await FlashCardApi.FlashCardUpdateAsync(context, (int)card.Id,
                        (int)card.WordUserId, AvailableFlashCardStatus.DONTUSE,
                        futureTime, card.UniqueKey);

                    // pull the card again
                    var cardAfter = await FlashCardApi.FlashCardReadByIdAsync(context, (int)card.Id);
                    Assert.IsNotNull(cardAfter);
                    Assert.IsNotNull(cardAfter.Id);
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
            Assert.Fail();
        }

        [TestMethod()]
        public void FlashCardReadByWordUserIdAsyncTest()
        {
            Assert.Fail();
        }
    }
}