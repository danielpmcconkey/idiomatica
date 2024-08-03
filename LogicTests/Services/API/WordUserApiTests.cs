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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 1;
            int wordId = 39;
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (int)user.Id;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, languageId, (int)user.Id);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);

                var wordUser = WordUserApi.WordUserCreate(
                    context, wordId, (int)languageUser.Id, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 1;
            int wordId = 39;
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (int)user.Id;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, (int)user.Id);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);

                var wordUser = await WordUserApi.WordUserCreateAsync(
                    context, wordId, (int)languageUser.Id, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int bookId = 6;
            int pageId = 66;
            int expectedCount = 149;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (int)user.Id;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);
                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, (int)user.Id);
                // now read them back for one of the pages
                var dict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    context, pageId, (int)user.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int bookId = 6;
            int pageId = 66;
            int expectedCount = 149;

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (int)user.Id;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, 1, (int)user.Id);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);
                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, (int)user.Id);
                // now read them back for one of the pages
                var dict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    context, pageId, (int)user.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int bookId = 6;
            int pageId = 66;
            int expectedCount = 149;
            string wordToCheck = "exploradores";

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (int)user.Id;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);

                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, (int)user.Id);

                // now read them back for one of the pages
                var dict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    context, pageId, (int)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
                var wordUser = dict[wordToCheck];
                Assert.IsNotNull(wordUser);
                Assert.IsNotNull(wordUser.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int bookId = 6;
            int pageId = 66;
            int expectedCount = 149;
            string wordToCheck = "exploradores";

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (int)user.Id;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, 1, (int)user.Id);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);

                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, (int)user.Id);
                
                // now read them back for one of the pages
                var dict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    context, pageId, (int)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
                var wordUser = dict[wordToCheck];
                Assert.IsNotNull(wordUser);
                Assert.IsNotNull(wordUser.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 1;
            int wordId = 39;
            AvailableWordUserStatus statusBefore = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfter = AvailableWordUserStatus.LEARNING3;
            string translation = "he/she says";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (int)user.Id;

                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, languageId, (int)user.Id);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);

                var wordUserBefore = WordUserApi.WordUserCreate(
                    context, wordId, (int)languageUser.Id, null, statusBefore);
                Assert.IsNotNull(wordUserBefore); Assert.IsNotNull(wordUserBefore.Id);

                // update it
                WordUserApi.WordUserUpdate(context, (int)wordUserBefore.Id, statusAfter, translation);

                // read it back
                var wordUserAfter = WordUserApi.WordUserReadById(context, (int)wordUserBefore.Id);
                Assert.IsNotNull(wordUserAfter); Assert.IsNotNull(wordUserAfter.Id);
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
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 1;
            int wordId = 39;
            AvailableWordUserStatus statusBefore = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfter = AvailableWordUserStatus.LEARNING3;
            string translation = "he/she says";

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                Assert.IsNotNull(user); Assert.IsNotNull(user.Id);
                userId = (int)user.Id;

                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, languageId, (int)user.Id);
                Assert.IsNotNull(languageUser); Assert.IsNotNull(languageUser.Id);

                var wordUserBefore = await WordUserApi.WordUserCreateAsync(
                    context, wordId, (int)languageUser.Id, null, statusBefore);
                Assert.IsNotNull(wordUserBefore); Assert.IsNotNull(wordUserBefore.Id);

                // update it
                await WordUserApi.WordUserUpdateAsync(context, (int)wordUserBefore.Id, statusAfter, translation);

                // read it back
                var wordUserAfter = await WordUserApi.WordUserReadByIdAsync(context, (int)wordUserBefore.Id);
                Assert.IsNotNull(wordUserAfter); Assert.IsNotNull(wordUserAfter.Id);
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
            int wordUserId = 1545;
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
            var context = CommonFunctions.CreateContext();
            int wordUserId = 1545;
            AvailableWordUserStatus status = AvailableWordUserStatus.LEARNED;
            string translation = "they were living (imperfect)";

            var wordUser = await WordUserApi.WordUserReadByIdAsync(context, wordUserId);
            Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
            Assert.AreEqual(status, wordUser.Status);
            Assert.AreEqual(translation, wordUser.Translation);
        }
    }
}