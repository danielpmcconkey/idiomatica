﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Guid bookId = CommonFunctions.GetBook11Id(context);


            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }

        [TestMethod()]
        public async Task UserAndAllChildrenDeleteAsyncTest()
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
            Guid bookId = CommonFunctions.GetBook11Id(context);


            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }

        [TestMethod()]
        public void UserCreateTest()
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
            string code = "HU";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = UserApi.UserCreate(applicationUserId, name, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                Assert.IsNotNull(user.Id);
                var ukString = user.Id.ToString();
                Assert.IsTrue(ukString.Trim().Length > 0);
            }
            finally
            {
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }

        [TestMethod()]
        public async Task UserCreateAsyncTest()
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
            string code = "HU";
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 3";

            try
            {
                // create the user
                var user = await UserApi.UserCreateAsync(applicationUserId, name, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                Assert.IsNotNull(user.Id);
                var ukString = user.Id.ToString();
                Assert.IsTrue(ukString.Trim().Length > 0);
            }
            finally
            {
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }

        [TestMethod()]
        public void UserBreadCrumbReadLatestTest()
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
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);
                Guid languageUserId = (Guid)languageUser.Id;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); }

                // take a reading of time before you make the first crumb
                var time1 = DateTime.Now;
                Thread.Sleep(500);

                // get the first page
                var page1 = PageApi.PageReadByOrdinalAndBookId(
                    context, 1, bookId);

                // create the first breadcrumb
                if (page1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                UserApi.UserBreadCrumbCreate(context, user, page1);

                // read it back
                var crumb1 = UserApi.UserBreadCrumbReadLatest(context, userId);

                Assert.IsNotNull(crumb1);
                Assert.IsNotNull(crumb1.Id);
                Assert.IsTrue(crumb1.PageId == page1.Id);
                Assert.IsTrue(crumb1.ActionDateTime > time1);

                // take a reading of time before you make the first crumb
                var time2 = DateTime.Now;
                Thread.Sleep(500);

                // get the first page
                var page2 = PageApi.PageReadByOrdinalAndBookId(
                    context, 2, bookId);

                // create the first breadcrumb
                if (page2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                UserApi.UserBreadCrumbCreate(context, user, page2);

                // read it back
                var crumb2 = UserApi.UserBreadCrumbReadLatest(context, userId);

                Assert.IsNotNull(crumb2);
                Assert.IsNotNull(crumb2.Id);
                Assert.IsTrue(crumb2.PageId == page2.Id);
                Assert.IsTrue(crumb2.ActionDateTime > time2);
            }
            finally
            {
                // shouldn't really need to, but just in case
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }

        [TestMethod()]
        public async Task UserBreadCrumbReadLatestAsyncTest()
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
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);
                Guid languageUserId = (Guid)languageUser.Id;

                // craete a bookUser
                var bookUser = await BookUserApi.BookUserCreateAsync(
                    context, bookId, (Guid)user.Id);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); }

                // take a reading of time before you make the first crumb
                var time1 = DateTime.Now;
                await Task.Delay(500);

                // get the first page
                var page1 = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 1, bookId);

                // create the first breadcrumb
                if (page1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                await UserApi.UserBreadCrumbCreateAsync(context, user, page1);

                // read it back
                var crumb1 = await UserApi.UserBreadCrumbReadLatestAsync(context, userId);

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

                // create the first breadcrumb
                if (page2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                await UserApi.UserBreadCrumbCreateAsync(context, user, page2);

                // read it back
                var crumb2 = await UserApi.UserBreadCrumbReadLatestAsync(context, userId);

                Assert.IsNotNull(crumb2);
                Assert.IsNotNull(crumb2.Id);
                Assert.IsTrue(crumb2.PageId == page2.Id);
                Assert.IsTrue(crumb2.ActionDateTime > time2);
            }
            finally
            {
                // shouldn't really need to, but just in case
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }

        [TestMethod()]
        public void UserBreadCrumbCreateTest()
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
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);
                Guid languageUserId = (Guid)languageUser.Id;

                // craete a bookUser
                var bookUser = BookUserApi.BookUserCreate(
                    context, bookId, (Guid)user.Id);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); }

                // get the first page
                var page = PageApi.PageReadByOrdinalAndBookId(
                    context, 1, bookId);

                // create the breadcrumb
                if (page is null)
                { ErrorHandler.LogAndThrow(); return; }
                var crumb = UserApi.UserBreadCrumbCreate(context, user, page);

                Assert.IsNotNull(crumb);
                Assert.IsNotNull(crumb.Id);
                Assert.IsNotNull(crumb.ActionDateTime);
            }
            finally
            {
                // shouldn't really need to, but just in case
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }

        [TestMethod()]
        public async Task UserBreadCrumbCreateAsyncTest()
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
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook11Id(context);

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = await CommonFunctions.CreateNewTestUserAsync(userService, context);
                if (user is null) { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.Id;

                // pull the languageUser
                Assert.IsNotNull(userId);
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);
                Guid languageUserId = (Guid)languageUser.Id;

                // craete a bookUser
                var bookUser = await BookUserApi.BookUserCreateAsync(
                    context, bookId, (Guid)user.Id);
                if (bookUser is null)
                { ErrorHandler.LogAndThrow(); }

                // get the first page
                var page = await PageApi.PageReadByOrdinalAndBookIdAsync(
                    context, 1, bookId);

                // create the breadcrumb
                if (page is null)
                    { ErrorHandler.LogAndThrow(); return; }
                var crumb = await UserApi.UserBreadCrumbCreateAsync(context, user, page);

                Assert.IsNotNull(crumb);
                Assert.IsNotNull(crumb.Id);
                Assert.IsNotNull(crumb.ActionDateTime);
            }
            finally
            {
                // shouldn't really need to, but just in case
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }
    }
}