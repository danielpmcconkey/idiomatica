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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);


            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context,
                    CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);

                // now delete it all
                UserApi.UserAndAllChildrenDelete(context, userId);

                // now try to pull anything
                var userRead = context.Users.Where(x => x.UniqueKey == userId).FirstOrDefault();
                var languageUserRead = context.LanguageUsers.Where(x => x.UserKey == userId).FirstOrDefault();
                var bookUserRead = context.BookUsers.Where(x => x.LanguageUserKey == languageUserId).FirstOrDefault();
                var wordUserRead = context.WordUsers.Where(x => x.LanguageUserKey == languageUserId).FirstOrDefault();

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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);


            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context,
                    CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, userId);

                // now delete it all
                await UserApi.UserAndAllChildrenDeleteAsync(context, userId);

                // now try to pull anything
                var userRead = context.Users.Where(x => x.UniqueKey == userId).FirstOrDefault();
                var languageUserRead = context.LanguageUsers.Where(x => x.UserKey == userId).FirstOrDefault();
                var bookUserRead = context.BookUsers.Where(x => x.LanguageUserKey == languageUserId).FirstOrDefault();
                var wordUserRead = context.WordUsers.Where(x => x.LanguageUserKey == languageUserId).FirstOrDefault();

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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string code = "HU";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = UserApi.UserCreate(applicationUserId, name, code, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            string code = "HU";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = await UserApi.UserCreateAsync(applicationUserId, name, code, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // take a reading of time before you make the first crumb
                var time1 = DateTime.Now;
                Thread.Sleep(500);

                // get the first page
                var page1 = PageApi.PageReadByOrdinalAndBookId(
                    context, 1, bookId);

                // create the first breadcrumb
                if (page1 is null || page1.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                UserApi.UserBreadCrumbCreate(context, userId, (Guid)page1.UniqueKey);

                // read it back
                var crumb1 = UserApi.UserBreadCrumbReadLatest(context, userId);

                Assert.IsNotNull(crumb1);
                Assert.IsNotNull(crumb1.UniqueKey);
                Assert.IsTrue(crumb1.PageKey == page1.UniqueKey);
                Assert.IsTrue(crumb1.ActionDateTime > time1);

                // take a reading of time before you make the first crumb
                var time2 = DateTime.Now;
                Thread.Sleep(500);

                // get the first page
                var page2 = PageApi.PageReadByOrdinalAndBookId(
                    context, 2, bookId);

                // create the first breadcrumb
                if (page2 is null || page2.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                UserApi.UserBreadCrumbCreate(context, userId, (Guid)page2.UniqueKey);

                // read it back
                var crumb2 = UserApi.UserBreadCrumbReadLatest(context, userId);

                Assert.IsNotNull(crumb2);
                Assert.IsNotNull(crumb2.UniqueKey);
                Assert.IsTrue(crumb2.PageKey == page2.UniqueKey);
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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = await BookUserApi.BookUserCreateAsync(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // take a reading of time before you make the first crumb
                var time1 = DateTime.Now;
                await Task.Delay(500);

                // get the first page
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 1, bookId);

                // create the first breadcrumb
                if (page1 is null || page1.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                await UserApi.UserBreadCrumbCreateAsync(context, userId, (Guid)page1.UniqueKey);

                // read it back
                var crumb1 = await UserApi.UserBreadCrumbReadLatestAsync(context, userId);

                Assert.IsNotNull(crumb1);
                Assert.IsNotNull(crumb1.UniqueKey);
                Assert.IsTrue(crumb1.PageKey == page1.UniqueKey);
                Assert.IsTrue(crumb1.ActionDateTime > time1);

                // take a reading of time before you make the first crumb
                var time2 = DateTime.Now;
                await Task.Delay(500);

                // get the first page
                var page2 = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 2, bookId);

                // create the first breadcrumb
                if (page2 is null || page2.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                await UserApi.UserBreadCrumbCreateAsync(context, userId, (Guid)page2.UniqueKey);

                // read it back
                var crumb2 = await UserApi.UserBreadCrumbReadLatestAsync(context, userId);

                Assert.IsNotNull(crumb2);
                Assert.IsNotNull(crumb2.UniqueKey);
                Assert.IsTrue(crumb2.PageKey == page2.UniqueKey);
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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // get the first page
                var page = PageApi.PageReadByOrdinalAndBookId(
                    context, 1, bookId);

                // create the breadcrumb
                if (page is null || page.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                var crumb = UserApi.UserBreadCrumbCreate(context, userId, (Guid)page.UniqueKey);

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
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create a languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null || languageUser.UniqueKey is null) 
                    { ErrorHandler.LogAndThrow(); return; }
                Guid languageUserId = (Guid)languageUser.UniqueKey;

                // craete a bookUser
                var bookUser = await BookUserApi.BookUserCreateAsync(
                    context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); }

                // get the first page
                var page = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 1, bookId);

                // create the breadcrumb
                if (page is null || page.UniqueKey is null)
                    { ErrorHandler.LogAndThrow(); return; }
                var crumb = await UserApi.UserBreadCrumbCreateAsync(context, userId, (Guid)page.UniqueKey);

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