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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 1;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                    { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                var languageUsersBefore = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (int)user.Id, context);
                if(languageUsersBefore is null) {  ErrorHandler.LogAndThrow(); return; }

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, userId);
                var languageUsersAfter = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (int)user.Id, context);
                if (languageUsersAfter is null) { ErrorHandler.LogAndThrow(); return; }

                int actualCountAfter = languageUsersAfter.Count;


                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);
                Assert.IsTrue(languageUser.Id > 0);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 1;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                var languageUsersBefore = await DataCache.LanguageUsersAndLanguageByUserIdReadAsync(
                    (int)user.Id, context);
                if (languageUsersBefore is null) { ErrorHandler.LogAndThrow(); return; }

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, userId);
                var languageUsersAfter = await DataCache.LanguageUsersAndLanguageByUserIdReadAsync(
                    (int)user.Id, context);
                if (languageUsersAfter is null) { ErrorHandler.LogAndThrow(); return; }

                int actualCountAfter = languageUsersAfter.Count;


                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);
                Assert.IsTrue(languageUser.Id > 0);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                int languageId = 1;
                var languageUserMake = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                if (languageUserMake is null || languageUserMake.Id is null || languageUserMake.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var expectedResultId = languageUserMake.Id;
                var expectedResultLanguageId = languageUserMake.LanguageId;
                var expectedResultUserId = languageUserMake.UserId;


                // act
                var languageUserRead = LanguageUserApi.LanguageUserGet(
                    context, languageId, (int)user.Id);
                if (languageUserRead is null || languageUserRead.Id is null || languageUserRead.Id < 1)
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                int languageId = 1;
                var languageUserMake = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (int)user.Id);
                if (languageUserMake is null || languageUserMake.Id is null || languageUserMake.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                var expectedResultId = languageUserMake.Id;
                var expectedResultLanguageId = languageUserMake.LanguageId;
                var expectedResultUserId = languageUserMake.UserId;


                // act
                var languageUserRead = await LanguageUserApi.LanguageUserGetAsync(
                    context, languageId, (int)user.Id);
                if (languageUserRead is null || languageUserRead.Id is null || languageUserRead.Id < 1)
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 2;
            string expectedResult = "English";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                var languageUsersBefore = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(
                    context, (int)user.Id);
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);

                // act
                var languageUsersAfter = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(
                    context, (int)user.Id);

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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 2;
            string expectedResult = "English";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                var languageUsersBefore = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, (int)user.Id);
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (int)user.Id);

                // act
                var languageUsersAfter = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, (int)user.Id);

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