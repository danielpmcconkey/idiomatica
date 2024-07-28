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
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            int languageId = 2;
            string expectedCode = "EN-US";


            try
            {
                // act
                var language = LanguageApi.LanguageRead(context, languageId);
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

                transaction.Rollback();
            }
        }
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

        [TestMethod()]
        public void LanguageReadByCodeTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            string code = "EN-US";
            int expectedId = 2;

            try
            {
                var language = LanguageApi.LanguageReadByCode(context, code);
                if (language == null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                Assert.AreEqual(expectedId, language.Id);
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }

        [TestMethod()]
        public async Task LanguageReadByCodeAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            string code = "EN-US";
            int expectedId = 2;

            try
            {
                var language = await LanguageApi.LanguageReadByCodeAsync(context, code);
                if (language == null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                Assert.AreEqual(expectedId, language.Id);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }
    }
}