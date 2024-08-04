using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Logic.Telemetry;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookTagApiTests
    {
        [TestMethod()]
        public void BookTagAddTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string tag = Guid.NewGuid().ToString();

            try
            {

                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                var originalTags = BookTagApi.BookTagsGetByBookIdAndUserId(context, bookId, userId);
                int originalCount = originalTags is null ? 0 : originalTags.Count;
                int expectedNewCount = originalCount + 1;

                BookTagApi.BookTagAdd(context, bookId, userId, tag);
                var newTags = BookTagApi.BookTagsGetByBookIdAndUserId(context, bookId, userId);
                int newCount = newTags is null ? 0 : newTags.Count;

                Assert.IsNotNull(originalTags);
                Assert.IsNotNull(newTags);
                Assert.IsTrue(newCount > 0);
                Assert.AreEqual(expectedNewCount, newCount);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookTagAddAsyncTest()
        {
            int bookId = 0;
            int userId = 0;
            var context = CommonFunctions.CreateContext();
            string tag = Guid.NewGuid().ToString();

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                var originalTags = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(context, bookId, userId);
                int originalCount = originalTags is null ? 0 : originalTags.Count;
                int expectedNewCount = originalCount + 1;

                await BookTagApi.BookTagAddAsync(context, bookId, userId, tag);
                var newTags = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(context, bookId, userId);
                int newCount = newTags is null ? 0 : newTags.Count;

                Assert.IsNotNull(originalTags);
                Assert.IsNotNull(newTags);
                Assert.IsTrue(newCount > 0);
                Assert.AreEqual(expectedNewCount, newCount);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void BookTagRemoveTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string tag1 = Guid.NewGuid().ToString();
            string tag2 = $"{Guid.NewGuid().ToString()}varbit"; // just adding an extra string in case the two are somehow identical

            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                var originalTags = BookTagApi.BookTagsGetByBookIdAndUserId(context, bookId, userId);
                int originalCount = originalTags is null ? 0 : originalTags.Count;
                int expectedNewCountFirst = originalCount + 2;
                int expectedNewCountSecond = originalCount + 1;

                BookTagApi.BookTagAdd(context, bookId, userId, tag1);
                BookTagApi.BookTagAdd(context, bookId, userId, tag2);
                var newTagsFirst = BookTagApi.BookTagsGetByBookIdAndUserId(context, bookId, userId);
                int newCountFirst = newTagsFirst is null ? 0 : newTagsFirst.Count;
                BookTagApi.BookTagRemove(context, bookId, userId, tag1);
                var newTagsSecond = BookTagApi.BookTagsGetByBookIdAndUserId(context, bookId, userId);
                int newCountSecond = newTagsFirst is null ? 0 : newTagsSecond.Count;

                // assert
                Assert.IsNotNull(originalTags);
                Assert.IsNotNull(newTagsFirst);
                Assert.IsTrue(newCountFirst > 0);
                Assert.AreEqual(expectedNewCountFirst, newCountFirst);
                Assert.IsNotNull(newTagsSecond);
                Assert.IsTrue(newCountSecond > 0);
                Assert.AreEqual(expectedNewCountSecond, newCountSecond);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }

        [TestMethod()]
        public async Task BookTagRemoveAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            string tag1 = Guid.NewGuid().ToString();
            string tag2 = $"{Guid.NewGuid().ToString()}varbit"; // just adding an extra string in case the two are somehow identical
            
            try
            {
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int bookUserId = createResult.bookUserId;

                var originalTags = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(context, bookId, userId);
                int originalCount = originalTags is null ? 0 : originalTags.Count;
                int expectedNewCountFirst = originalCount + 2;
                int expectedNewCountSecond = originalCount + 1;
                
                await BookTagApi.BookTagAddAsync(context, bookId, userId, tag1);
                await BookTagApi.BookTagAddAsync(context, bookId, userId, tag2);
                var newTagsFirst = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(context, bookId, userId);
                int newCountFirst = newTagsFirst is null ? 0 : newTagsFirst.Count;
                await BookTagApi.BookTagRemoveAsync(context, bookId, userId, tag1);
                var newTagsSecond = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(context, bookId, userId);
                int newCountSecond = newTagsFirst is null ? 0 : newTagsSecond.Count;

                // assert
                Assert.IsNotNull(originalTags);
                Assert.IsNotNull(newTagsFirst);
                Assert.IsTrue(newCountFirst > 0);
                Assert.AreEqual(expectedNewCountFirst, newCountFirst);
                Assert.IsNotNull(newTagsSecond);
                Assert.IsTrue(newCountSecond > 0);
                Assert.AreEqual(expectedNewCountSecond, newCountSecond);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
    }
}