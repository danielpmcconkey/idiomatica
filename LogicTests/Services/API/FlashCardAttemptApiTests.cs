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
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLanguage, user);
                if (languageUser is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a word
                var word = context.Words.Where(x => x.LanguageId == learningLanguage.Id).Take(1).FirstOrDefault();
                if (word is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = WordUserApi.WordUserCreate(
                    context, word, languageUser, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }

                // create the card
                FlashCard? card = FlashCardApi.FlashCardCreate(context, (Guid)wordUser.Id, uiLanguageCode);
                if (card is null) { ErrorHandler.LogAndThrow(); return; }

                // create the attempt
                var attempt = FlashCardAttemptApi.FlashCardAttemptCreate(
                    context, card, AvailableFlashCardAttemptStatus.HARD);
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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            var learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;

            try
            {
                // create a user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLanguage, user);
                if (languageUser is null) { ErrorHandler.LogAndThrow(); return; }

                // pull a word
                var word = context.Words.Where(x => x.LanguageId == learningLanguage.Id).Take(1).FirstOrDefault();
                if (word is null) { ErrorHandler.LogAndThrow(); return; }

                // create a WordUser
                var wordUser = await WordUserApi.WordUserCreateAsync(
                    context, word, languageUser, null, AvailableWordUserStatus.UNKNOWN);
                if (wordUser is null) { ErrorHandler.LogAndThrow(); return; }

                // create the card
                FlashCard? card = await FlashCardApi.FlashCardCreateAsync(context, (Guid)wordUser.Id, uiLanguageCode);
                if (card is null) { ErrorHandler.LogAndThrow(); return; }

                // create the attempt
                var attempt = await FlashCardAttemptApi.FlashCardAttemptCreateAsync(
                    context, card, AvailableFlashCardAttemptStatus.HARD);
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