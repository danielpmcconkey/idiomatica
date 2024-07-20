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
        public async Task LanguageCodeReadByCodeAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            string code = "HU";
            string expectedName = "Hungarian";


            try
            {
                // act
                var languageCode = await LanguageCodeApi.LanguageCodeReadByCodeAsync(context, code);
                if (languageCode == null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // assert
                Assert.AreEqual(expectedName, languageCode.LanguageName);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [TestMethod()]
        public void LanguageCodeOptionsReadTest()
        {

            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            Expression<Func<LanguageCode, bool>> filter1 = (x => x.IsImplementedForLearning == true);
            Expression<Func<LanguageCode, bool>> filter2 = (x => x.IsImplementedForUI == true);
            Expression<Func<LanguageCode, bool>> filter3 = (x => x.IsImplementedForTranslation == true);


            try
            {
                // act
                var languageOptions1 = LanguageCodeApi.LanguageCodeOptionsRead(context, filter1);
                var languageOptions2 = LanguageCodeApi.LanguageCodeOptionsRead(context, filter2);
                var languageOptions3 = LanguageCodeApi.LanguageCodeOptionsRead(context, filter3);

                var spanishFrom1 = languageOptions1["ES"];
                var englishFrom2 = languageOptions2["EN-US"];
                var englishFrom3 = languageOptions3["EN-US"];

                // assert
                Assert.IsNotNull(spanishFrom1);
                Assert.IsNotNull(englishFrom2);
                Assert.IsNotNull(englishFrom3);
                Assert.AreEqual("Spanish", spanishFrom1.LanguageName);
                Assert.AreEqual("English (American)", englishFrom2.LanguageName);
                Assert.AreEqual("English (American)", englishFrom3.LanguageName);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }

        [TestMethod()]
        public void LanguageCodeReadByCodeTest()
        {
            Assert.Fail();
        }
    }
}