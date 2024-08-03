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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
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

                // create the card
                FlashCard? card = FlashCardApi.FlashCardCreate(context, (int)wordUser.Id, uiLanguageCode);
                if (card is null || card.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create the attempt
                var attempt = FlashCardAttemptApi.FlashCardAttemptCreate(
                    context, (int)card.Id, AvailableFlashCardAttemptStatus.HARD);
                Assert.IsNotNull(attempt);
                Assert.IsNotNull(attempt.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int learningLangugeId = 1;
            string uiLanguageCode = "EN-US";

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
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

                // create the card
                FlashCard? card = await FlashCardApi.FlashCardCreateAsync(context, (int)wordUser.Id, uiLanguageCode);
                if (card is null || card.Id is null) { ErrorHandler.LogAndThrow(); return; }

                // create the attempt
                var attempt = await FlashCardAttemptApi.FlashCardAttemptCreateAsync(
                    context, (int)card.Id, AvailableFlashCardAttemptStatus.HARD);
                Assert.IsNotNull(attempt);
                Assert.IsNotNull(attempt.Id);
            }
            finally
            {
                // clean-up
                await CommonFunctions.CleanUpUserAsync(userId, context);
            }
        }
    }
}