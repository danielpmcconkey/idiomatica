using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using LogicTests;
using Model.DAL;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Model;
using Microsoft.EntityFrameworkCore;
using Model.Enums;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class LanguageUserApiTests
    {
        [TestMethod()]
        public void LanguageUserCreateTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            var context = CommonFunctions.CreateContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUsersBefore = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (Guid)user.Id, context);
                if(languageUsersBefore is null) {  ErrorHandler.LogAndThrow(); return; }

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                var languageUser = LanguageUserApi.LanguageUserCreate(context, language, user);
                var languageUsersAfter = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (Guid)user.Id, context);
                if (languageUsersAfter is null) { ErrorHandler.LogAndThrow(); return; }

                int actualCountAfter = languageUsersAfter.Count;


                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task LanguageUserCreateAsyncTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            var context = CommonFunctions.CreateContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUsersBefore = await DataCache.LanguageUsersAndLanguageByUserIdReadAsync(
                    (Guid)user.Id, context);
                if (languageUsersBefore is null) { ErrorHandler.LogAndThrow(); return; }

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, language, user);
                var languageUsersAfter = await DataCache.LanguageUsersAndLanguageByUserIdReadAsync(
                    (Guid)user.Id, context);
                if (languageUsersAfter is null) { ErrorHandler.LogAndThrow(); return; }

                int actualCountAfter = languageUsersAfter.Count;


                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void LanguageUserGetTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            var context = CommonFunctions.CreateContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUserMake = LanguageUserApi.LanguageUserCreate(context, language, user);
                if (languageUserMake is null)
                { ErrorHandler.LogAndThrow(); return; }
                var expectedResultId = languageUserMake.Id;
                var expectedResultLanguageId = languageUserMake.LanguageId;
                var expectedResultUserId = languageUserMake.UserId;


                // act
                var languageUserRead = LanguageUserApi.LanguageUserGet(
                    context, language.Id, (Guid)user.Id);
                if (languageUserRead is null)
                { ErrorHandler.LogAndThrow(); return; }
                var actualResultId = languageUserRead.Id;
                var actualResultLanguageId = languageUserRead.LanguageId;
                var actualResultUserId = languageUserRead.UserId;
                // assert

                Assert.IsNotNull(actualResultId);
                Assert.IsNotNull(actualResultLanguageId);
                Assert.IsNotNull(actualResultUserId);
                Assert.AreEqual(expectedResultId, actualResultId);
                Assert.AreEqual(expectedResultLanguageId, actualResultLanguageId);
                Assert.AreEqual(expectedResultUserId, actualResultUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task LanguageUserGetAsyncTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            var context = CommonFunctions.CreateContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUserMake = await LanguageUserApi.LanguageUserCreateAsync(context, language, user);
                if (languageUserMake is null)
                { ErrorHandler.LogAndThrow(); return; }
                var expectedResultId = languageUserMake.Id;
                var expectedResultLanguageId = languageUserMake.LanguageId;
                var expectedResultUserId = languageUserMake.UserId;


                // act
                var languageUserRead = await LanguageUserApi.LanguageUserGetAsync(
                    context, language.Id, (Guid)user.Id);
                if (languageUserRead is null)
                { ErrorHandler.LogAndThrow(); return; }
                var actualResultId = languageUserRead.Id;
                var actualResultLanguageId = languageUserRead.LanguageId;
                var actualResultUserId = languageUserRead.UserId;
                // assert

                Assert.IsNotNull(actualResultId);
                Assert.IsNotNull(actualResultLanguageId);
                Assert.IsNotNull(actualResultUserId);
                Assert.AreEqual(expectedResultId, actualResultId);
                Assert.AreEqual(expectedResultLanguageId, actualResultLanguageId);
                Assert.AreEqual(expectedResultUserId, actualResultUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void LanguageUsersAndLanguageGetByUserIdTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetEnglishLanguageId(context);
            Language language = CommonFunctions.GetSpanishLanguage(context);
            string expectedResult = "English";

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUsersBefore = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(
                    context, (Guid)user.Id);
                var languageUser = LanguageUserApi.LanguageUserCreate(context, language, user);

                // act
                var languageUsersAfter = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(
                    context, (Guid)user.Id);

                if (languageUsersAfter is null)
                { ErrorHandler.LogAndThrow(); return; }

                var matchingLanguageUser = languageUsersAfter
                    .Where(x => x.LanguageId == languageId)
                    .FirstOrDefault();

                if (matchingLanguageUser is null || matchingLanguageUser.Language is null
                    || matchingLanguageUser.Language.Name is null)
                { ErrorHandler.LogAndThrow(); return; }

                string actualResult = matchingLanguageUser.Language.Name;

                Assert.AreEqual(expectedResult, actualResult);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task LanguageUsersAndLanguageGetByUserIdAsyncTest()
        {
            // boilerplate begin
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            AvailableLanguageCode uiLanguageCode = AvailableLanguageCode.EN_US;
            // boilerplate end


            Guid? userId = null;
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetEnglishLanguageId(context);
            Language language = CommonFunctions.GetSpanishLanguage(context);
            string expectedResult = "English";

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = user.Id;

                var languageUsersBefore = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, user.Id);
                Assert.IsNotNull(languageUsersBefore);

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, language, user);

                // act
                var languageUsersAfter = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, user.Id);

                if (languageUsersAfter is null)
                { ErrorHandler.LogAndThrow(); return; }

                var matchingLanguageUser = languageUsersAfter
                    .Where(x => x.LanguageId == languageId)
                    .FirstOrDefault();

                Assert.IsNotNull(matchingLanguageUser);
                Assert.IsNotNull(matchingLanguageUser.Language);
                Assert.IsNotNull(matchingLanguageUser.Language.Name);

                string actualResult = matchingLanguageUser.Language.Name;
                
                Assert.AreEqual(expectedResult, actualResult);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
    }
}