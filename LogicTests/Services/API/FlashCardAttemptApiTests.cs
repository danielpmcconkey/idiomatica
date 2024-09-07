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
    public class FlashCardAttemptApiTests
    {
        [TestMethod()]
        public void FlashCardAttemptCreateTest()
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

                // create the card
                FlashCard? card = FlashCardApi.FlashCardCreate(context, (Guid)wordUser.UniqueKey, uiLanguageCode);
                if (card is null || card.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // create the attempt
                var attempt = FlashCardAttemptApi.FlashCardAttemptCreate(
                    context, (Guid)card.UniqueKey, AvailableFlashCardAttemptStatus.HARD);
                Assert.IsNotNull(attempt);
                Assert.IsNotNull(attempt.UniqueKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }

        [TestMethod()]
        public async Task FlashCardAttemptCreateAsyncTest()
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

                // create the card
                FlashCard? card = await FlashCardApi.FlashCardCreateAsync(context, (Guid)wordUser.UniqueKey, uiLanguageCode);
                if (card is null || card.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }

                // create the attempt
                var attempt = await FlashCardAttemptApi.FlashCardAttemptCreateAsync(
                    context, (Guid)card.UniqueKey, AvailableFlashCardAttemptStatus.HARD);
                Assert.IsNotNull(attempt);
                Assert.IsNotNull(attempt.UniqueKey);
            }
            finally
            {
                // clean-up
                await CommonFunctions.CleanUpUserAsync(userId, context);
            }
        }
    }
}