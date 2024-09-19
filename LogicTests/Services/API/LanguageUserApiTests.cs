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
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUsersBefore = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (Guid)user.Id, dbContextFactory);
                Assert.IsNotNull(languageUsersBefore);

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    dbContextFactory, learningLanguage, user);
                var languageUsersAfter = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (Guid)user.Id, dbContextFactory);
                Assert.IsNotNull(languageUsersAfter);

                int actualCountAfter = languageUsersAfter.Count;


                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task LanguageUserCreateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            
            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUsersBefore = await DataCache.LanguageUsersAndLanguageByUserIdReadAsync(
                    (Guid)user.Id, dbContextFactory);
                Assert.IsNotNull(languageUsersBefore);

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    dbContextFactory, learningLanguage, user);
                var languageUsersAfter = await DataCache.LanguageUsersAndLanguageByUserIdReadAsync(
                    (Guid)user.Id, dbContextFactory);
                Assert.IsNotNull(languageUsersAfter);

                int actualCountAfter = languageUsersAfter.Count;


                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void LanguageUserGetTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUserMake = LanguageUserApi.LanguageUserCreate(
                    dbContextFactory, learningLanguage, user);
                if (languageUserMake is null)
                { ErrorHandler.LogAndThrow(); return; }
                var expectedResultId = languageUserMake.Id;
                var expectedResultLanguageId = languageUserMake.LanguageId;
                var expectedResultUserId = languageUserMake.UserId;


                // act
                var languageUserRead = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)user.Id);
                Assert.IsNotNull(languageUserRead);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task LanguageUserGetAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                var languageUserMake = await LanguageUserApi.LanguageUserCreateAsync(
                    dbContextFactory, learningLanguage, user);
                if (languageUserMake is null)
                { ErrorHandler.LogAndThrow(); return; }
                var expectedResultId = languageUserMake.Id;
                var expectedResultLanguageId = languageUserMake.LanguageId;
                var expectedResultUserId = languageUserMake.UserId;


                // act
                var languageUserRead = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)user.Id);
                Assert.IsNotNull(languageUserRead);
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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void LanguageUsersAndLanguageGetByUserIdTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Language secondLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            string expectedResult = "English";
            int expectedCount = 2;

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = user.Id;

                // this should pull the initial languageUser (spanish)
                var languageUsersBefore = LanguageUserApi
                    .LanguageUsersAndLanguageGetByUserId(dbContextFactory, user.Id);
                Assert.IsNotNull(languageUsersBefore);

                // add a second language
                var languageUser2 = LanguageUserApi.LanguageUserCreate(
                    dbContextFactory, secondLanguage, user);

                // act
                var languageUsersAfter = LanguageUserApi
                    .LanguageUsersAndLanguageGetByUserId(dbContextFactory, user.Id);
                Assert.IsNotNull(languageUsersAfter);

                Assert.AreEqual(expectedCount, languageUsersAfter.Count);


                var matchingLanguageUser = languageUsersAfter
                    .Where(x => x.LanguageId == secondLanguage.Id)
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task LanguageUsersAndLanguageGetByUserIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Language secondLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            string expectedResult = "English";
            int expectedCount = 2;

            try
            {
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = user.Id;

                // this should pull the initial languageUser (spanish)
                var languageUsersBefore = await LanguageUserApi
                    .LanguageUsersAndLanguageGetByUserIdAsync(dbContextFactory, user.Id);
                Assert.IsNotNull(languageUsersBefore);

                // add a second language
                var languageUser2 = await LanguageUserApi.LanguageUserCreateAsync(
                    dbContextFactory, secondLanguage, user);

                // act
                var languageUsersAfter = await LanguageUserApi
                    .LanguageUsersAndLanguageGetByUserIdAsync(dbContextFactory, user.Id);
                Assert.IsNotNull(languageUsersAfter);

                Assert.AreEqual(expectedCount, languageUsersAfter.Count);


                var matchingLanguageUser = languageUsersAfter
                    .Where(x => x.LanguageId == secondLanguage.Id)
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
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }
    }
}