using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using LogicTests;
using System.Linq.Expressions;
using Model;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class LanguageCodeApiTests
    {
        [TestMethod()]
        public void LanguageCodeOptionsReadTest()
        {

            var context = CommonFunctions.CreateContext();

            Expression<Func<LanguageCode, bool>> filter1 = (x => x.IsImplementedForLearning == true);
            Expression<Func<LanguageCode, bool>> filter2 = (x => x.IsImplementedForUI == true);
            Expression<Func<LanguageCode, bool>> filter3 = (x => x.IsImplementedForTranslation == true);

            var languageOptions1 = LanguageCodeApi.LanguageCodeOptionsRead(context, filter1);
            var languageOptions2 = LanguageCodeApi.LanguageCodeOptionsRead(context, filter2);
            var languageOptions3 = LanguageCodeApi.LanguageCodeOptionsRead(context, filter3);

            var spanishFrom1 = languageOptions1["ES"];
            var englishFrom2 = languageOptions2["EN-US"];
            var englishFrom3 = languageOptions3["EN-US"];

            Assert.IsNotNull(spanishFrom1);
            Assert.IsNotNull(englishFrom2);
            Assert.IsNotNull(englishFrom3);
            Assert.AreEqual("Spanish", spanishFrom1.LanguageName);
            Assert.AreEqual("English (American)", englishFrom2.LanguageName);
            Assert.AreEqual("English (American)", englishFrom3.LanguageName);
        }
        [TestMethod()]
        public async Task LanguageCodeOptionsReadAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            
            Expression<Func<LanguageCode, bool>> filter1 = (x => x.IsImplementedForLearning == true);
            Expression<Func<LanguageCode, bool>> filter2 = (x => x.IsImplementedForUI == true);
            Expression<Func<LanguageCode, bool>> filter3 = (x => x.IsImplementedForTranslation == true);

            var languageOptions1 = await LanguageCodeApi.LanguageCodeOptionsReadAsync(context, filter1);
            var languageOptions2 = await LanguageCodeApi.LanguageCodeOptionsReadAsync(context, filter2);
            var languageOptions3 = await LanguageCodeApi.LanguageCodeOptionsReadAsync(context, filter3);

            var spanishFrom1 = languageOptions1["ES"];
            var englishFrom2 = languageOptions2["EN-US"];
            var englishFrom3 = languageOptions3["EN-US"];

            Assert.IsNotNull(spanishFrom1);
            Assert.IsNotNull(englishFrom2);
            Assert.IsNotNull(englishFrom3);
            Assert.AreEqual("Spanish", spanishFrom1.LanguageName);
            Assert.AreEqual("English (American)", englishFrom2.LanguageName);
            Assert.AreEqual("English (American)", englishFrom3.LanguageName);
        }


        [TestMethod()]
        public void LanguageCodeReadByCodeTest()
        {
            var context = CommonFunctions.CreateContext();
            string code = "HU";
            string expectedName = "Hungarian";

            var languageCode = LanguageCodeApi.LanguageCodeReadByCode(context, code);
            if (languageCode == null)
            { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedName, languageCode.LanguageName);
        }

        [TestMethod()]
        public async Task LanguageCodeReadByCodeAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            string code = "HU";
            string expectedName = "Hungarian";
            
            var languageCode = await LanguageCodeApi.LanguageCodeReadByCodeAsync(context, code);
            if (languageCode == null)
                { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedName, languageCode.LanguageName);
        }

        [TestMethod()]
        public void LanguageCodeUserInterfacePreferenceReadByUserIdTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            string code = "HU";
            string expectedName = "Hungarian";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = UserApi.UserCreate(applicationUserId, name, code, context);
                if (user is null || user.Id is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                var languageCode = LanguageCodeApi.LanguageCodeUserInterfacePreferenceReadByUserId(
                    context, userId);
                if (languageCode is null) { ErrorHandler.LogAndThrow(); return; }

                Assert.AreEqual(expectedName, languageCode.LanguageName);
            }
            finally
            {
                CommonFunctions.CleanUpUser(userId, context);
            }
        }

        [TestMethod()]
        public async Task LanguageCodeUserInterfacePreferenceReadByUserIdAsyncTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            string code = "HU";
            string expectedName = "Hungarian";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = await UserApi.UserCreateAsync(applicationUserId, name, code, context);
                if (user is null || user.Id is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                var languageCode = await LanguageCodeApi.LanguageCodeUserInterfacePreferenceReadByUserIdAsync(
                    context, userId);
                if (languageCode is null) { ErrorHandler.LogAndThrow(); return; }

                Assert.AreEqual(expectedName, languageCode.LanguageName);
            }
            finally
            {
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
    }
}