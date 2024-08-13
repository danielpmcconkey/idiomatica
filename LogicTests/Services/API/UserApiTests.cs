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

        [TestMethod()]
        public void UserBreadCrumbReadLatestTest()
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
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, 1, (int)user.Id);
                if (languageUser is null || languageUser.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                int languageUserId = (int)languageUser.Id;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                { ErrorHandler.LogAndThrow(); }

                // take a reading of time before you make the first crumb
                var time1 = DateTime.Now;
                Thread.Sleep(500);

                // get the first page
                var page1 = PageApi.PageReadByOrdinalAndBookId(
                    context, 1, bookId);

                // create the first breadcrumb
                if (page1 is null || page1.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                UserApi.UserBreadCrumbCreate(context, userId, (int)page1.Id);

                // read it back
                var crumb1 = UserApi.UserBreadCrumbReadLatest(context, userId);

                Assert.IsNotNull(crumb1);
                Assert.IsNotNull(crumb1.UniqueKey);
                Assert.IsTrue(crumb1.PageId == page1.Id);
                Assert.IsTrue(crumb1.ActionDateTime > time1);

                // take a reading of time before you make the first crumb
                var time2 = DateTime.Now;
                Thread.Sleep(500);

                // get the first page
                var page2 = PageApi.PageReadByOrdinalAndBookId(
                    context, 2, bookId);

                // create the first breadcrumb
                if (page2 is null || page2.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                UserApi.UserBreadCrumbCreate(context, userId, (int)page2.Id);

                // read it back
                var crumb2 = UserApi.UserBreadCrumbReadLatest(context, userId);

                Assert.IsNotNull(crumb2);
                Assert.IsNotNull(crumb2.UniqueKey);
                Assert.IsTrue(crumb2.PageId == page2.Id);
                Assert.IsTrue(crumb2.ActionDateTime > time2);
            }
            finally
            {
                // shouldn't really need to, but just in case
                CommonFunctions.CleanUpUser(userId, context);
            }
        }

        [TestMethod()]
        public async Task UserBreadCrumbReadLatestAsyncTest()
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
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, 1, (int)user.Id);
                if (languageUser is null || languageUser.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                int languageUserId = (int)languageUser.Id;

                // craete a bookUser
                var bookUser = await BookUserApi.BookUserCreateAsync(
                    context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                { ErrorHandler.LogAndThrow(); }

                // take a reading of time before you make the first crumb
                var time1 = DateTime.Now;
                await Task.Delay(500);

                // get the first page
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 1, bookId);

                // create the first breadcrumb
                if (page1 is null || page1.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                await UserApi.UserBreadCrumbCreateAsync(context, userId, (int)page1.Id);

                // read it back
                var crumb1 = await UserApi.UserBreadCrumbReadLatestAsync(context, userId);

                Assert.IsNotNull(crumb1);
                Assert.IsNotNull(crumb1.UniqueKey);
                Assert.IsTrue(crumb1.PageId == page1.Id);
                Assert.IsTrue(crumb1.ActionDateTime > time1);

                // take a reading of time before you make the first crumb
                var time2 = DateTime.Now;
                await Task.Delay(500);

                // get the first page
                var page2 = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 2, bookId);

                // create the first breadcrumb
                if (page2 is null || page2.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                await UserApi.UserBreadCrumbCreateAsync(context, userId, (int)page2.Id);

                // read it back
                var crumb2 = await UserApi.UserBreadCrumbReadLatestAsync(context, userId);

                Assert.IsNotNull(crumb2);
                Assert.IsNotNull(crumb2.UniqueKey);
                Assert.IsTrue(crumb2.PageId == page2.Id);
                Assert.IsTrue(crumb2.ActionDateTime > time2);
            }
            finally
            {
                // shouldn't really need to, but just in case
                await CommonFunctions.CleanUpUserAsync(userId, context);
            }
        }

        [TestMethod()]
        public void UserBreadCrumbCreateTest()
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
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, 1, (int)user.Id);
                if (languageUser is null || languageUser.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                int languageUserId = (int)languageUser.Id;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                { ErrorHandler.LogAndThrow(); }

                // get the first page
                var page = PageApi.PageReadByOrdinalAndBookId(
                    context, 1, bookId);

                // create the breadcrumb
                if (page is null || page.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                var crumb = UserApi.UserBreadCrumbCreate(context, userId, (int)page.Id);

                Assert.IsNotNull(crumb);
                Assert.IsNotNull(crumb.UniqueKey);
                Assert.IsNotNull(crumb.ActionDateTime);
            }
            finally
            {
                // shouldn't really need to, but just in case
                CommonFunctions.CleanUpUser(userId, context);
            }
        }

        [TestMethod()]
        public async Task UserBreadCrumbCreateAsyncTest()
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
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, 1, (int)user.Id);
                if (languageUser is null || languageUser.Id is null) 
                    { ErrorHandler.LogAndThrow(); return; }
                int languageUserId = (int)languageUser.Id;

                // craete a bookUser
                var bookUser = await BookUserApi.BookUserCreateAsync(
                    context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                { ErrorHandler.LogAndThrow(); }

                // get the first page
                var page = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 1, bookId);

                // create the breadcrumb
                if (page is null || page.Id is null)
                    { ErrorHandler.LogAndThrow(); return; }
                var crumb = await UserApi.UserBreadCrumbCreateAsync(context, userId, (int)page.Id);

                Assert.IsNotNull(crumb);
                Assert.IsNotNull(crumb.UniqueKey);
                Assert.IsNotNull(crumb.ActionDateTime);
            }
            finally
            {
                // shouldn't really need to, but just in case
                await CommonFunctions.CleanUpUserAsync(userId, context);
            }
        }
    }
}