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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class LanguageApiTests
    {
        [TestMethod()]
        public void LanguageReadTest()
        {
            var context = CommonFunctions.CreateContext();

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
            var context = CommonFunctions.CreateContext();
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
            var context = CommonFunctions.CreateContext();
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
            var context = CommonFunctions.CreateContext();
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

            var context = CommonFunctions.CreateContext();

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
            var context = CommonFunctions.CreateContext();

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


        [TestMethod()]
        public void LanguageReadByCodeTest2()
        {
            var context = CommonFunctions.CreateContext();
            AvailableLanguageCode code = AvailableLanguageCode.HU;
            string expectedName = "Hungarian";

            var Language = LanguageApi.LanguageReadByCode(context, code);
            if (Language == null)
            { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedName, Language.Name);
        }

        [TestMethod()]
        public async Task LanguageReadByCodeAsyncTest2()
        {
            var context = CommonFunctions.CreateContext();
            AvailableLanguageCode code = AvailableLanguageCode.HU;
            string expectedName = "Hungarian";

            var Language = await LanguageApi.LanguageReadByCodeAsync(context, code);
            if (Language == null)
            { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedName, Language.Name);
        }

        [TestMethod()]
        public void LanguageUserInterfacePreferenceReadByUserIdTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            AvailableLanguageCode code = AvailableLanguageCode.HU;
            var language = LanguageApi.LanguageReadByCode(context, code);
            Assert.IsNotNull(language);
            string expectedName = "Hungarian";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = UserApi.UserCreate(applicationUserId, name, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = user.Id;

                UserSetting Setting = new()
                {
                    Key = AvailableUserSetting.UILANGUAGE,
                    UserId = userId,
                    User = user,
                    Value = language.Id.ToString(),
                };

                var uiLanguageSetting = UserApi.UserSettingUiLanguagReadByUserId(context, userId);
                if (uiLanguageSetting is null) { ErrorHandler.LogAndThrow(); return; }

                Assert.AreEqual(expectedName, uiLanguageSetting.Name);
            }
            finally
            {
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public async Task LanguageUserInterfacePreferenceReadByUserIdAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            AvailableLanguageCode code = AvailableLanguageCode.HU;
            var language = await LanguageApi.LanguageReadByCodeAsync(context, code);
            Assert.IsNotNull(language);
            string expectedName = "Hungarian";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = await UserApi.UserCreateAsync(applicationUserId, name, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = user.Id;

                UserSetting Setting = new()
                {
                    Key = AvailableUserSetting.UILANGUAGE,
                    UserId = userId,
                    User = user,
                    Value = language.Id.ToString(),
                };

                var uiLanguageSetting = await UserApi.UserSettingUiLanguagReadByUserIdAsync(context, userId);
                if (uiLanguageSetting is null) { ErrorHandler.LogAndThrow(); return; }

                Assert.AreEqual(expectedName, uiLanguageSetting.Name);
            }
            finally
            {
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
    }
}