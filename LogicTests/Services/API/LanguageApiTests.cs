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
        public async Task LanguageReadAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            int languageId = 2;
            string expectedCode = "EN-US";


            try
            {
                // act
                var language = await LanguageApi.LanguageReadAsync(context, languageId);
                if (language == null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // assert
                Assert.AreEqual(expectedCode, language.Code);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        
    }
}