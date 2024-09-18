using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Logic.Telemetry;
using Model.Enums;
using Model;
using Microsoft.EntityFrameworkCore;
using Model.DAL;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class WordUserApiTests
    {
        [TestMethod()]
        public void WordUserCreateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid wordId = CommonFunctions.GetWordId(context, "dice", learningLanguage.Id);
            Word? word = WordApi.WordGetById(context, wordId);
            Assert.IsNotNull(word);
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

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

                // create word user
                var wordUser = WordUserApi.WordUserCreate(
                    context, word, languageUser, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task WordUserCreateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid wordId = CommonFunctions.GetWordId(context, "dice", learningLanguage.Id);
            Word? word = WordApi.WordGetById(context, wordId);
            Assert.IsNotNull(word);
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

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

                // create word user
                var wordUser = await WordUserApi.WordUserCreateAsync(
                    context, word, languageUser, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void WordUsersCreateAllForBookIdAndUserIdTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook11Id(context);
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(
                    context, bookId, (Guid)user.Id);
                // now read them back for one of the pages
                var dict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    context, pageId, (Guid)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task WordUsersCreateAllForBookIdAndUserIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook11Id(context);
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);
                
                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(
                    context, bookId, (Guid)user.Id);
                // now read them back for one of the pages
                var dict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    context, pageId, (Guid)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void WordUsersDictByPageIdAndUserIdReadTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook11Id(context);
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;
            string wordToCheck = "exploradores";

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

                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(
                    context, bookId, (Guid)user.Id);

                // now read them back for one of the pages
                var dict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    context, pageId, (Guid)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
                var wordUser = dict[wordToCheck];
                Assert.IsNotNull(wordUser);
                Assert.IsNotNull(wordUser.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task WordUsersDictByPageIdAndUserIdReadAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid bookId = CommonFunctions.GetBook11Id(context);
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;
            string wordToCheck = "exploradores";

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

                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(
                    context, bookId, (Guid)user.Id);
                
                // now read them back for one of the pages
                var dict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    context, pageId, (Guid)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
                var wordUser = dict[wordToCheck];
                Assert.IsNotNull(wordUser);
                Assert.IsNotNull(wordUser.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void WordUserUpdateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);
            Guid wordId = CommonFunctions.GetWordId(context, "dice", language.Id);
            Word? word = WordApi.WordGetById(context, wordId);
            Assert.IsNotNull(word);
            AvailableWordUserStatus statusBefore = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfter = AvailableWordUserStatus.LEARNING3;
            string translation = "he/she says";

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
                    context, language.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var wordUserBefore = WordUserApi.WordUserCreate(
                    context, word, languageUser, null, statusBefore);
                Assert.IsNotNull(wordUserBefore); Assert.IsNotNull(wordUserBefore.Id);

                // update it
                WordUserApi.WordUserUpdate(context, wordUserBefore.Id, statusAfter, translation);

                // read it back
                var wordUserAfter = WordUserApi.WordUserReadById(context, wordUserBefore.Id);
                Assert.IsNotNull(wordUserAfter); Assert.IsNotNull(wordUserAfter.Id);
                Assert.AreEqual(statusAfter, wordUserAfter.Status);
                Assert.AreEqual(translation, wordUserAfter.Translation);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
            }
        }
        [TestMethod()]
        public async Task WordUserUpdateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(context);
            Guid wordId = CommonFunctions.GetWordId(context, "dice", language.Id);
            Word? word = WordApi.WordGetById(context, wordId);
            Assert.IsNotNull(word);
            AvailableWordUserStatus statusBefore = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfter = AvailableWordUserStatus.LEARNING3;
            string translation = "he/she says";

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
                    context, language.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var wordUserBefore = await WordUserApi.WordUserCreateAsync(
                    context, word, languageUser, null, statusBefore);
                Assert.IsNotNull(wordUserBefore); Assert.IsNotNull(wordUserBefore.Id);

                // update it
                await WordUserApi.WordUserUpdateAsync(context, wordUserBefore.Id, statusAfter, translation);

                // read it back
                var wordUserAfter = await WordUserApi.WordUserReadByIdAsync(context, wordUserBefore.Id);
                Assert.IsNotNull(wordUserAfter); Assert.IsNotNull(wordUserAfter.Id);
                Assert.AreEqual(statusAfter, wordUserAfter.Status);
                Assert.AreEqual(translation, wordUserAfter.Translation);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
            }
        }


        [TestMethod()]
        public void WordUserReadByIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(context);
            Guid wordUserId = CommonFunctions.GetWordUser(context, languageUserId, "vivían");
            AvailableWordUserStatus status = AvailableWordUserStatus.LEARNED;
            string translation = "they were living (imperfect)";

            var wordUser = WordUserApi.WordUserReadById(context, wordUserId);
            Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
            Assert.AreEqual(status, wordUser.Status);
            Assert.AreEqual(translation, wordUser.Translation);
        }
        [TestMethod()]
        public async Task WordUserReadByIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(context);
            Guid wordUserId = CommonFunctions.GetWordUser(context, languageUserId, "vivían");
            AvailableWordUserStatus status = AvailableWordUserStatus.LEARNED;
            string translation = "they were living (imperfect)";

            var wordUser = await WordUserApi.WordUserReadByIdAsync(context, wordUserId);
            Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
            Assert.AreEqual(status, wordUser.Status);
            Assert.AreEqual(translation, wordUser.Translation);
        }
    }
}