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
using Microsoft.EntityFrameworkCore;
using Model.DAL;
using Model.Enums;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class UserApiTests
    {
        [TestMethod()]
        public void UserAndAllChildrenDeleteTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook11Id(context);


            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, (Guid)userId);

                // now delete it all
                UserApi.UserAndAllChildrenDelete(context, (Guid)userId);

                // now try to pull anything
                var userRead = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                var languageUserRead = context.LanguageUsers.Where(x => x.UserId == userId).FirstOrDefault();
                var bookUserRead = context.BookUsers.Where(x => x.LanguageUserId == languageUser.Id).FirstOrDefault();
                var wordUserRead = context.WordUsers.Where(x => x.LanguageUserId == languageUser.Id).FirstOrDefault();

                Assert.IsNull(userRead);
                Assert.IsNull(languageUserRead);
                Assert.IsNull(bookUserRead);
                Assert.IsNull(wordUserRead);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task UserAndAllChildrenDeleteAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook11Id(context);


            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); }

                // create wordUsers
                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, (Guid)userId);

                // now delete it all
                await UserApi.UserAndAllChildrenDeleteAsync(context, (Guid)userId);

                // now try to pull anything
                var userRead = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                var languageUserRead = context.LanguageUsers.Where(x => x.UserId == userId).FirstOrDefault();
                var bookUserRead = context.BookUsers.Where(x => x.LanguageUserId == languageUser.Id).FirstOrDefault();
                var wordUserRead = context.WordUsers.Where(x => x.LanguageUserId == languageUser.Id).FirstOrDefault();

                Assert.IsNull(userRead);
                Assert.IsNull(languageUserRead);
                Assert.IsNull(bookUserRead);
                Assert.IsNull(wordUserRead);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void UserCreateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester";

            try
            {
                // create the user
                var user = UserApi.UserCreate(applicationUserId, name, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                Assert.IsNotNull(user.Id);
                var ukString = user.Id.ToString();
                Assert.IsTrue(ukString.Trim().Length > 0);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task UserCreateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester";

            try
            {
                // create the user
                var user = await UserApi.UserCreateAsync(applicationUserId, name, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                Assert.IsNotNull(user.Id);
                var ukString = user.Id.ToString();
                Assert.IsTrue(ukString.Trim().Length > 0);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void UserBreadCrumbReadLatestTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);


            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);

                // take a reading of time before you make the first crumb
                var time1 = DateTime.Now;
                Task.Delay(500);

                // get the first page
                var page1 = PageApi.PageReadByOrdinalAndBookId(
                    context, 1, bookId);
                Assert.IsNotNull(page1);

                // create the first breadcrumb
                UserApi.UserBreadCrumbCreate(context, user, page1);

                // read it back
                var crumb1 = UserApi.UserBreadCrumbReadLatest(context, (Guid)userId);

                Assert.IsNotNull(crumb1);
                Assert.IsNotNull(crumb1.Id);
                Assert.IsTrue(crumb1.PageId == page1.Id);
                Assert.IsTrue(crumb1.ActionDateTime > time1);

                // take a reading of time before you make the first crumb
                var time2 = DateTime.Now;
                Task.Delay(500);

                // get the first page
                var page2 = PageApi.PageReadByOrdinalAndBookId(
                    context, 2, bookId);
                Assert.IsNotNull(page2);

                // create the first breadcrumb
                UserApi.UserBreadCrumbCreate(context, user, page2);

                // read it back
                var crumb2 = UserApi.UserBreadCrumbReadLatest(context, (Guid)userId);

                Assert.IsNotNull(crumb2);
                Assert.IsNotNull(crumb2.Id);
                Assert.IsTrue(crumb2.PageId == page2.Id);
                Assert.IsTrue(crumb2.ActionDateTime > time2);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task UserBreadCrumbReadLatestAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            

            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = await BookUserApi.BookUserCreateAsync(
                    context, bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);

                // take a reading of time before you make the first crumb
                var time1 = DateTime.Now;
                await Task.Delay(500);

                // get the first page
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 1, bookId);
                Assert.IsNotNull(page1);

                // create the first breadcrumb
                await UserApi.UserBreadCrumbCreateAsync(context, user, page1);

                // read it back
                var crumb1 = await UserApi.UserBreadCrumbReadLatestAsync(context, (Guid)userId);

                Assert.IsNotNull(crumb1);
                Assert.IsNotNull(crumb1.Id);
                Assert.IsTrue(crumb1.PageId == page1.Id);
                Assert.IsTrue(crumb1.ActionDateTime > time1);

                // take a reading of time before you make the first crumb
                var time2 = DateTime.Now;
                await Task.Delay(500);

                // get the first page
                var page2 = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 2, bookId);
                Assert.IsNotNull(page2);

                // create the first breadcrumb
                await UserApi.UserBreadCrumbCreateAsync(context, user, page2);

                // read it back
                var crumb2 = await UserApi.UserBreadCrumbReadLatestAsync(context, (Guid)userId);

                Assert.IsNotNull(crumb2);
                Assert.IsNotNull(crumb2.Id);
                Assert.IsTrue(crumb2.PageId == page2.Id);
                Assert.IsTrue(crumb2.ActionDateTime > time2);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void UserBreadCrumbCreateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                /// fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);

                // get the first page
                var page = PageApi.PageReadByOrdinalAndBookId(
                    context, 1, bookId);
                Assert.IsNotNull(page);

                // create the breadcrumb
                var crumb = UserApi.UserBreadCrumbCreate(context, user, page);

                Assert.IsNotNull(crumb);
                Assert.IsNotNull(crumb.Id);
                Assert.IsNotNull(crumb.ActionDateTime);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task UserBreadCrumbCreateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                /// fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // craete a bookUser
                var bookUser = await BookUserApi.BookUserCreateAsync(
                    context, bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);

                // get the first page
                var page = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 1, bookId);
                Assert.IsNotNull(page);

                // create the breadcrumb
                var crumb = await UserApi.UserBreadCrumbCreateAsync(context, user, page);

                Assert.IsNotNull(crumb);
                Assert.IsNotNull(crumb.Id);
                Assert.IsNotNull(crumb.ActionDateTime);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }
    }
}