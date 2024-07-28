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
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int languageId = 1;


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUsersBefore = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (int)user.Id, context);

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                // act

                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                var languageUsersAfter = DataCache.LanguageUsersAndLanguageByUserIdRead(
                    (int)user.Id, context);
                int actualCountAfter = languageUsersAfter.Count;


                // assert
                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);
                Assert.IsTrue(languageUser.Id > 0);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);
            }
            finally
            {
                // clean-up

                transaction.RollbackAsync();
            }
        }
        [TestMethod()]
        public async Task LanguageUserCreateAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int languageId = 1;


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUsersBefore = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, (int)user.Id);

                int countBefore = languageUsersBefore.Count;
                int expectedCountAfter = countBefore + 1;

                // act

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (int)user.Id);
                var languageUsersAfter = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, (int)user.Id);
                int actualCountAfter = languageUsersAfter.Count;


                // assert
                Assert.IsNotNull(languageUser);
                Assert.IsNotNull(languageUser.Id);
                Assert.IsTrue(languageUser.Id > 0);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void LanguageUserGetTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int languageId = 1;
                var languageUserMake = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);
                if (languageUserMake is null || languageUserMake.Id is null || languageUserMake.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var expectedResultId = languageUserMake.Id;
                var expectedResultLanguageId = languageUserMake.LanguageId;
                var expectedResultUserId = languageUserMake.UserId;


                // act
                var languageUserRead = LanguageUserApi.LanguageUserGet(
                    context, languageId, (int)user.Id);
                if (languageUserRead is null || languageUserRead.Id is null || languageUserRead.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
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
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task LanguageUserGetAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int languageId = 1;
                var languageUserMake = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (int)user.Id);
                if (languageUserMake is null || languageUserMake.Id is null || languageUserMake.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var expectedResultId = languageUserMake.Id;
                var expectedResultLanguageId = languageUserMake.LanguageId;
                var expectedResultUserId = languageUserMake.UserId;


                // act
                var languageUserRead = await LanguageUserApi.LanguageUserGetAsync(
                    context, languageId, (int)user.Id);
                if (languageUserRead is null || languageUserRead.Id is null || languageUserRead.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
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
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void LanguageUsersAndLanguageGetByUserIdTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int languageId = 2;
            string expectedResult = "English";


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUsersBefore = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(
                    context, (int)user.Id);
                var languageUser = LanguageUserApi.LanguageUserCreate(context, languageId, (int)user.Id);

                // act
                var languageUsersAfter = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(
                    context, (int)user.Id);

                if(languageUsersAfter is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                var matchingLanguageUser = languageUsersAfter
                    .Where(x => x.LanguageId == languageId)
                    .FirstOrDefault();

                if (matchingLanguageUser is null || matchingLanguageUser.Language is null
                    || matchingLanguageUser.Language.Name is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                string actualResult = matchingLanguageUser.Language.Name;


                // assert
                Assert.AreEqual(expectedResult, actualResult);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task LanguageUsersAndLanguageGetByUserIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int languageId = 2;
            string expectedResult = "English";


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUsersBefore = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, (int)user.Id);
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, languageId, (int)user.Id);

                // act
                var languageUsersAfter = await LanguageUserApi.LanguageUsersAndLanguageGetByUserIdAsync(
                    context, (int)user.Id);

                if (languageUsersAfter is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                var matchingLanguageUser = languageUsersAfter
                    .Where(x => x.LanguageId == languageId)
                    .FirstOrDefault();

                if (matchingLanguageUser is null || matchingLanguageUser.Language is null
                    || matchingLanguageUser.Language.Name is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                string actualResult = matchingLanguageUser.Language.Name;


                // assert
                Assert.AreEqual(expectedResult, actualResult);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }
    }
}