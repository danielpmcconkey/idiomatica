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
    public class LanguageApiTests
    {
        [TestMethod()]
        public void LanguageReadTest()
        {
            var context = CommonFunctions.CreateContext();

            int languageId = 2;
            string expectedCode = "EN-US";
            
            var language = LanguageApi.LanguageRead(context, languageId);
            if (language == null)
            { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedCode, language.Code);
            
        }
        [TestMethod()]
        public async Task LanguageReadAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            int languageId = 2;
            string expectedCode = "EN-US";

            var language = await LanguageApi.LanguageReadAsync(context, languageId);
            if (language == null)
            { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedCode, language.Code);
            
        }

        [TestMethod()]
        public void LanguageReadByCodeTest()
        {
            var context = CommonFunctions.CreateContext();
            string code = "EN-US";
            int expectedId = 2;

            var language = LanguageApi.LanguageReadByCode(context, code);
            if (language == null)
            { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedId, language.Id);
            
        }

        [TestMethod()]
        public async Task LanguageReadByCodeAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            string code = "EN-US";
            int expectedId = 2;
            var language = await LanguageApi.LanguageReadByCodeAsync(context, code);
            if (language == null)
                { ErrorHandler.LogAndThrow(); return; }

            Assert.AreEqual(expectedId, language.Id);
        }
    }
}