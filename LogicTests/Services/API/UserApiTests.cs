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
    public class UserApiTests
    {
        [TestMethod()]
        public void UserAndAllChildrenDeleteTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int bookId = 1;


            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                int languageUserId = (int)languageUser.Id;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);

                // now delete it all
                UserApi.UserAndAllChildrenDelete(context, userId);

                // now try to pull anything
                var userRead = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                var languageUserRead = context.LanguageUsers.Where(x => x.UserId == userId).FirstOrDefault();
                var bookUserRead = context.BookUsers.Where(x => x.LanguageUserId == languageUserId).FirstOrDefault();
                var wordUserRead = context.WordUsers.Where(x => x.LanguageUserId == languageUserId).FirstOrDefault();

                Assert.IsNull(userRead);
                Assert.IsNull(languageUserRead);
                Assert.IsNull(bookUserRead);
                Assert.IsNull(wordUserRead);
            }
            finally
            {
                // shouldn't really need to, but just in case
                CommonFunctions.CleanUpUser(userId, context);
            }
        }

        [TestMethod()]
        public async Task UserAndAllChildrenDeleteAsyncTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int bookId = 1;
            

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null || user.Id is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return; }
                int languageUserId = (int)languageUser.Id;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                    { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, userId);

                // now delete it all
                await UserApi.UserAndAllChildrenDeleteAsync(context, userId);

                // now try to pull anything
                var userRead = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                var languageUserRead = context.LanguageUsers.Where(x => x.UserId == userId).FirstOrDefault();
                var bookUserRead = context.BookUsers.Where(x => x.LanguageUserId == languageUserId).FirstOrDefault();
                var wordUserRead = context.WordUsers.Where(x => x.LanguageUserId == languageUserId).FirstOrDefault();

                Assert.IsNull(userRead);
                Assert.IsNull(languageUserRead);
                Assert.IsNull(bookUserRead);
                Assert.IsNull(wordUserRead);
            }
            finally
            {
                // shouldn't really need to, but just in case
                CommonFunctions.CleanUpUser(userId, context);
            }
        }

        [TestMethod()]
        public void UserCreateTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            string code = "HU";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = UserApi.UserCreate(applicationUserId, name, code, context);
                if (user is null || user.Id is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                Assert.IsNotNull(user.UniqueKey);
                var ukString = user.UniqueKey.ToString();
                Assert.IsTrue(ukString.Trim().Length > 0);
            }
            finally
            {
                CommonFunctions.CleanUpUser(userId, context);
            }
        }

        [TestMethod()]
        public async Task UserCreateAsyncTest()
        {
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            string code = "HU";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = await UserApi.UserCreateAsync(applicationUserId, name, code, context);
                if (user is null || user.Id is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                Assert.IsNotNull(user.UniqueKey);
                var ukString = user.UniqueKey.ToString();
                Assert.IsTrue(ukString.Trim().Length > 0);
            }
            finally
            {
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
    }
}