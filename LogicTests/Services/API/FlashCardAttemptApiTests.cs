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
    public class FlashCardAttemptApiTests
    {
        [TestMethod()]
        public void FlashCardAttemptCreateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;



            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a word
                var word = context.Words
                    .Where(x => x.LanguageId == learningLanguage.Id)
                    .Take(1)
                    .FirstOrDefault();
                Assert.IsNotNull(word);

                // create a WordUser
                var wordUser = WordUserApi.WordUserCreate(
                    context, word, languageUser, null, AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser);

                // create the card
                FlashCard? card = FlashCardApi.FlashCardCreate(
                    dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);
                Assert.IsNotNull(card);

                // create the attempt
                var attempt = FlashCardAttemptApi.FlashCardAttemptCreate(
                    context, card, AvailableFlashCardAttemptStatus.HARD);
                Assert.IsNotNull(attempt);
                Assert.IsNotNull(attempt.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }

        [TestMethod()]
        public async Task FlashCardAttemptCreateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            

            
            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // pull a word
                var word = context.Words
                    .Where(x => x.LanguageId == learningLanguage.Id)
                    .Take(1)
                    .FirstOrDefault();
                Assert.IsNotNull(word);

                // create a WordUser
                var wordUser = await WordUserApi.WordUserCreateAsync(
                    context, word, languageUser, null, AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser);

                // create the card
                FlashCard? card = await FlashCardApi.FlashCardCreateAsync(
                    dbContextFactory, (Guid)wordUser.Id, uiLanguageCode);
                Assert.IsNotNull(card);

                // create the attempt
                var attempt = await FlashCardAttemptApi.FlashCardAttemptCreateAsync(
                    context, card, AvailableFlashCardAttemptStatus.HARD);
                Assert.IsNotNull(attempt);
                Assert.IsNotNull(attempt.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }
    }
}