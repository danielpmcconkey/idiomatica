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

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class LanguageUserApiTests
    {
        [TestMethod()]
        public void LanguageUserCreateTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null)
                    { ErrorHandler.LogAndThrow(); return; }
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
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task LanguageUserCreateAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null)
                { ErrorHandler.LogAndThrow(); return; }
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
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void LanguageUserGetTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null)
                { ErrorHandler.LogAndThrow(); return; }
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
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task LanguageUserGetAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null)
                { ErrorHandler.LogAndThrow(); return; }
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
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void LanguageUsersAndLanguageGetByUserIdTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetEnglishLanguageKey(context);
            Language language = CommonFunctions.GetSpanishLanguage(context);
            string expectedResult = "English";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null)
                { ErrorHandler.LogAndThrow(); return; }
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
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task LanguageUsersAndLanguageGetByUserIdAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetEnglishLanguageKey(context);
            Language language = CommonFunctions.GetSpanishLanguage(context);
            string expectedResult = "English";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                var languageUsersBefore = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, (Guid)user.Id);
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, language, user);

                // act
                var languageUsersAfter = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
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
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
    }
}