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
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Model.DAL;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class LanguageApiTests
    {
        [TestMethod()]
        public void LanguageReadTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            Guid languageId = CommonFunctions.GetEnglishLanguageId(context);
            AvailableLanguageCode expectedCode = AvailableLanguageCode.EN_US;
            
            var language = LanguageApi.LanguageRead(context, languageId);
            if (language == null)
            { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedCode, language.Code);
            
        }
        [TestMethod()]
        public async Task LanguageReadAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid languageId = CommonFunctions.GetEnglishLanguageId(context);
            AvailableLanguageCode expectedCode = AvailableLanguageCode.EN_US;

            var language = await LanguageApi.LanguageReadAsync(context, languageId);
            if (language == null)
            { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedCode, language.Code);
            
        }


        [TestMethod()]
        public void LanguageReadByCodeTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            AvailableLanguageCode code = AvailableLanguageCode.EN_US;
            Guid expectedId = CommonFunctions.GetEnglishLanguageId(context);

            var language = LanguageApi.LanguageReadByCode(context, code);
            if (language == null)
            { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedId, language.Id);
            
        }

        [TestMethod()]
        public async Task LanguageReadByCodeAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            AvailableLanguageCode code = AvailableLanguageCode.EN_US;
            Guid expectedId = CommonFunctions.GetEnglishLanguageId(context);
            var language = await LanguageApi.LanguageReadByCodeAsync(context, code);
            if (language == null)
                { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedId, language.Id);
        }


        [TestMethod()]
        public void LanguageOptionsReadTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();

            Expression<Func<Language, bool>> filter1 = (x => x.IsImplementedForLearning == true);
            Expression<Func<Language, bool>> filter2 = (x => x.IsImplementedForUI == true);
            Expression<Func<Language, bool>> filter3 = (x => x.IsImplementedForTranslation == true);

            var languageOptions1 = LanguageApi.LanguageOptionsRead(context, filter1);
            var languageOptions2 = LanguageApi.LanguageOptionsRead(context, filter2);
            var languageOptions3 = LanguageApi.LanguageOptionsRead(context, filter3);

            var spanishFrom1 = languageOptions1
                .Where(x => x.Value.Code == AvailableLanguageCode.ES)
                .FirstOrDefault();
            var englishFrom2 = languageOptions2
                .Where(x => x.Value.Code == AvailableLanguageCode.EN_US)
                .FirstOrDefault();
            var englishFrom3 = languageOptions3
                .Where(x => x.Value.Code == AvailableLanguageCode.EN_US)
                .FirstOrDefault();

            Assert.IsNotNull(spanishFrom1);
            Assert.IsNotNull(englishFrom2);
            Assert.IsNotNull(englishFrom3);
            Assert.AreEqual("Spanish", spanishFrom1.Value.Name);
            Assert.AreEqual("English (American)", englishFrom2.Value.Name);
            Assert.AreEqual("English (American)", englishFrom3.Value.Name);
        }
        [TestMethod()]
        public async Task LanguageOptionsReadAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            
            Expression<Func<Language, bool>> filter1 = (x => x.IsImplementedForLearning == true);
            Expression<Func<Language, bool>> filter2 = (x => x.IsImplementedForUI == true);
            Expression<Func<Language, bool>> filter3 = (x => x.IsImplementedForTranslation == true);

            var languageOptions1 = await LanguageApi.LanguageOptionsReadAsync(context, filter1);
            var languageOptions2 = await LanguageApi.LanguageOptionsReadAsync(context, filter2);
            var languageOptions3 = await LanguageApi.LanguageOptionsReadAsync(context, filter3);

            var spanishFrom1 = languageOptions1
                .Where(x => x.Value.Code == AvailableLanguageCode.ES)
                .FirstOrDefault();
            var englishFrom2 = languageOptions2
                .Where(x => x.Value.Code == AvailableLanguageCode.EN_US)
                .FirstOrDefault();
            var englishFrom3 = languageOptions3
                .Where(x => x.Value.Code == AvailableLanguageCode.EN_US)
                .FirstOrDefault();

            Assert.IsNotNull(spanishFrom1);
            Assert.IsNotNull(englishFrom2);
            Assert.IsNotNull(englishFrom3);
            Assert.AreEqual("Spanish", spanishFrom1.Value.Name);
            Assert.AreEqual("English (American)", englishFrom2.Value.Name);
            Assert.AreEqual("English (American)", englishFrom3.Value.Name);
        }

        /// <summary>
        /// the name of LanguageReadByCodeTest2 is an artifact of when I merged
        /// the language and language code classes. They each had separate read
        /// by code methods and their own tests. I decided to keep the second test.
        /// </summary>
        [TestMethod()]
        public void LanguageReadByCodeTest2()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            AvailableLanguageCode code = AvailableLanguageCode.HU;
            string expectedName = "Hungarian";
            var Language = LanguageApi.LanguageReadByCode(context, code);
            Assert.IsNotNull(Language);
            Assert.AreEqual(expectedName, Language.Name);
        }
        [TestMethod()]
        public async Task LanguageReadByCodeAsyncTest2()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            AvailableLanguageCode code = AvailableLanguageCode.HU;
            string expectedName = "Hungarian";
            var Language = await LanguageApi.LanguageReadByCodeAsync(context, code);
            Assert.IsNotNull(Language);
            Assert.AreEqual(expectedName, Language.Name);
        }


        [TestMethod()]
        public void LanguageUserInterfacePreferenceReadByUserIdTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            AvailableLanguageCode code = AvailableLanguageCode.HU;
            var language = LanguageApi.LanguageReadByCode(context, code);
            Assert.IsNotNull(language);
            string expectedName = "Hungarian";
            var applicationUserId = Guid.NewGuid().ToString();

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context,
                    AvailableLanguageCode.ES, AvailableLanguageCode.HU);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);


                // create the setting

                UserApi.UserSettingCreate(
                    context, AvailableUserSetting.UILANGUAGE, (Guid)userId, language.Id.ToString());

                var uiLanguageSetting = UserApi.UserSettingUiLanguagReadByUserId(
                    context, (Guid)userId);
                Assert.IsNotNull(uiLanguageSetting);

                Assert.AreEqual(expectedName, uiLanguageSetting.Name);
            }
            finally
            {
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task LanguageUserInterfacePreferenceReadByUserIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            AvailableLanguageCode code = AvailableLanguageCode.HU;
            var language = await LanguageApi.LanguageReadByCodeAsync(context, code);
            Assert.IsNotNull(language);
            string expectedName = "Hungarian";
            var applicationUserId = Guid.NewGuid().ToString();

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context,
                    AvailableLanguageCode.ES, AvailableLanguageCode.HU);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);


                // create the setting

                await UserApi.UserSettingCreateAsync(
                    context, AvailableUserSetting.UILANGUAGE, (Guid)userId, language.Id.ToString());

                var uiLanguageSetting = await UserApi.UserSettingUiLanguagReadByUserIdAsync(
                    context, (Guid)userId);
                Assert.IsNotNull(uiLanguageSetting);

                Assert.AreEqual(expectedName, uiLanguageSetting.Name);
            }
            finally
            {
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }
    }
}