using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Logic.Telemetry;
using Model;
using k8s.KubeConfigModels;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class WordUserApiTests
    {
        [TestMethod()]
        public void WordUserCreateTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            Guid wordId = CommonFunctions.GetWordId(context, "dice", languageId);
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, languageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);

                var wordUser = WordUserApi.WordUserCreate(
                    context, wordId, (Guid)languageUser.UniqueKey, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.UniqueKey);
                Assert.AreEqual(wordId, wordUser.WordKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task WordUserCreateAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            Guid wordId = CommonFunctions.GetWordId(context, "dice", languageId);
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);

                var wordUser = await WordUserApi.WordUserCreateAsync(
                    context, wordId, (Guid)languageUser.UniqueKey, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.UniqueKey);
                Assert.AreEqual(wordId, wordUser.WordKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void WordUsersCreateAllForBookIdAndUserIdTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context,
                    CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);
                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, (Guid)user.UniqueKey);
                // now read them back for one of the pages
                var dict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    context, pageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task WordUsersCreateAllForBookIdAndUserIdAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context,
                    CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);
                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, (Guid)user.UniqueKey);
                // now read them back for one of the pages
                var dict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    context, pageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void WordUsersDictByPageIdAndUserIdReadTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;
            string wordToCheck = "exploradores";

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context,
                    CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);

                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, (Guid)user.UniqueKey);

                // now read them back for one of the pages
                var dict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    context, pageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
                var wordUser = dict[wordToCheck];
                Assert.IsNotNull(wordUser);
                Assert.IsNotNull(wordUser.UniqueKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task WordUsersDictByPageIdAndUserIdReadAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid bookId = CommonFunctions.GetBook11Id(context);
            Guid pageId = CommonFunctions.GetPage378Id(context);
            int expectedCount = 122;
            string wordToCheck = "exploradores";

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context,
                    CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);

                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, (Guid)user.UniqueKey);
                
                // now read them back for one of the pages
                var dict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    context, pageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
                var wordUser = dict[wordToCheck];
                Assert.IsNotNull(wordUser);
                Assert.IsNotNull(wordUser.UniqueKey);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void WordUserUpdateTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            Guid wordId = CommonFunctions.GetWordId(context, "dice", languageId);
            AvailableWordUserStatus statusBefore = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfter = AvailableWordUserStatus.LEARNING3;
            string translation = "he/she says";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, languageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);

                var wordUserBefore = WordUserApi.WordUserCreate(
                    context, wordId, (Guid)languageUser.UniqueKey, null, statusBefore);
                Assert.IsNotNull(wordUserBefore); Assert.IsNotNull(wordUserBefore.UniqueKey);

                // update it
                WordUserApi.WordUserUpdate(context, (Guid)wordUserBefore.UniqueKey, statusAfter, translation);

                // read it back
                var wordUserAfter = WordUserApi.WordUserReadById(context, (Guid)wordUserBefore.UniqueKey);
                Assert.IsNotNull(wordUserAfter); Assert.IsNotNull(wordUserAfter.UniqueKey);
                Assert.AreEqual(statusAfter, wordUserAfter.Status);
                Assert.AreEqual(translation, wordUserAfter.Translation);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }
        [TestMethod()]
        public async Task WordUserUpdateAsyncTest()
        {
            Guid userId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            Guid wordId = CommonFunctions.GetWordId(context, "dice", languageId);
            AvailableWordUserStatus statusBefore = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfter = AvailableWordUserStatus.LEARNING3;
            string translation = "he/she says";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.UniqueKey);
                userId = (Guid)user.UniqueKey;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, (Guid)user.UniqueKey);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.UniqueKey);

                var wordUserBefore = await WordUserApi.WordUserCreateAsync(
                    context, wordId, (Guid)languageUser.UniqueKey, null, statusBefore);
                Assert.IsNotNull(wordUserBefore); Assert.IsNotNull(wordUserBefore.UniqueKey);

                // update it
                await WordUserApi.WordUserUpdateAsync(context, (Guid)wordUserBefore.UniqueKey, statusAfter, translation);

                // read it back
                var wordUserAfter = await WordUserApi.WordUserReadByIdAsync(context, (Guid)wordUserBefore.UniqueKey);
                Assert.IsNotNull(wordUserAfter); Assert.IsNotNull(wordUserAfter.UniqueKey);
                Assert.AreEqual(statusAfter, wordUserAfter.Status);
                Assert.AreEqual(translation, wordUserAfter.Translation);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
            }
        }


        [TestMethod()]
        public void WordUserReadByIdTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserKey(context);
            Guid wordUserId = CommonFunctions.GetWordUser(context, languageUserId, "vivían");
            AvailableWordUserStatus status = AvailableWordUserStatus.LEARNED;
            string translation = "they were living (imperfect)";
            
            var wordUser = WordUserApi.WordUserReadById(context, wordUserId);
            Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.UniqueKey);
            Assert.AreEqual(status, wordUser.Status);
            Assert.AreEqual(translation, wordUser.Translation);
        }
        [TestMethod()]
        public async Task WordUserReadByIdAsyncTest()
        {
            var context = CommonFunctions.CreateContext();
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserKey(context);
            Guid wordUserId = CommonFunctions.GetWordUser(context, languageUserId, "vivían");
            AvailableWordUserStatus status = AvailableWordUserStatus.LEARNED;
            string translation = "they were living (imperfect)";

            var wordUser = await WordUserApi.WordUserReadByIdAsync(context, wordUserId);
            Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.UniqueKey);
            Assert.AreEqual(status, wordUser.Status);
            Assert.AreEqual(translation, wordUser.Translation);
        }
    }
}