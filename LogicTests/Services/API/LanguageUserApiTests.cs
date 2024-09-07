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
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUsersBefore = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (Guid)user.UniqueKey, context);
                if(languageUsersBefore is null) {  ErrorHandler.LogAndThrow(); return; }

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, userId);
                var languageUsersAfter = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (Guid)user.UniqueKey, context);
                if (languageUsersAfter is null) { ErrorHandler.LogAndThrow(); return; }

                int actualCountAfter = languageUsersAfter.Count;


                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.UniqueKey);
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
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUsersBefore = await DataCache.LanguageUsersAndLanguageByUserIdReadAsync(
                    (Guid)user.UniqueKey, context);
                if (languageUsersBefore is null) { ErrorHandler.LogAndThrow(); return; }

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, userId);
                var languageUsersAfter = await DataCache.LanguageUsersAndLanguageByUserIdReadAsync(
                    (Guid)user.UniqueKey, context);
                if (languageUsersAfter is null) { ErrorHandler.LogAndThrow(); return; }

                int actualCountAfter = languageUsersAfter.Count;


                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.UniqueKey);
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

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
                var languageUserMake = LanguageUserApi.LanguageUserCreate(context, languageId, (Guid)user.UniqueKey);
                if (languageUserMake is null || languageUserMake.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                var expectedResultId = languageUserMake.UniqueKey;
                var expectedResultLanguageId = languageUserMake.LanguageKey;
                var expectedResultUserId = languageUserMake.UserKey;


                // act
                var languageUserRead = LanguageUserApi.LanguageUserGet(
                    context, languageId, (Guid)user.UniqueKey);
                if (languageUserRead is null || languageUserRead.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                var actualResultId = languageUserRead.UniqueKey;
                var actualResultLanguageId = languageUserRead.LanguageKey;
                var actualResultUserId = languageUserRead.UserKey;
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

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
                var languageUserMake = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (Guid)user.UniqueKey);
                if (languageUserMake is null || languageUserMake.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                var expectedResultId = languageUserMake.UniqueKey;
                var expectedResultLanguageId = languageUserMake.LanguageKey;
                var expectedResultUserId = languageUserMake.UserKey;


                // act
                var languageUserRead = await LanguageUserApi.LanguageUserGetAsync(
                    context, languageId, (Guid)user.UniqueKey);
                if (languageUserRead is null || languageUserRead.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                var actualResultId = languageUserRead.UniqueKey;
                var actualResultLanguageId = languageUserRead.LanguageKey;
                var actualResultUserId = languageUserRead.UserKey;
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
            string expectedResult = "English";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUsersBefore = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(
                    context, (Guid)user.UniqueKey);
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (Guid)user.UniqueKey);

                // act
                var languageUsersAfter = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(
                    context, (Guid)user.UniqueKey);

                if (languageUsersAfter is null)
                { ErrorHandler.LogAndThrow(); return; }

                var matchingLanguageUser = languageUsersAfter
                    .Where(x => x.LanguageKey == languageId)
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
            string expectedResult = "English";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                var languageUsersBefore = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, (Guid)user.UniqueKey);
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (Guid)user.UniqueKey);

                // act
                var languageUsersAfter = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, (Guid)user.UniqueKey);

                if (languageUsersAfter is null)
                { ErrorHandler.LogAndThrow(); return; }

                var matchingLanguageUser = languageUsersAfter
                    .Where(x => x.LanguageKey == languageId)
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