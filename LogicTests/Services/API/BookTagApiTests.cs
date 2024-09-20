using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Logic.Telemetry;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using Model.Enums;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookTagApiTests
    {
        [TestMethod()]
        public void BookTagAddTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            string tag = Guid.NewGuid().ToString();

            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                var originalTags = BookTagApi.BookTagsGetByBookIdAndUserId(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                int originalCount = originalTags is null ? 0 : originalTags.Count;
                int expectedNewCount = originalCount + 1;

                var book = BookApi.BookRead(dbContextFactory, (Guid)bookId);
                Assert.IsNotNull(book);
                var user = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                Assert.IsNotNull(book);
                Assert.IsNotNull(user);

                BookTagApi.BookTagAdd(dbContextFactory, book, user, tag);
                var newTags = BookTagApi.BookTagsGetByBookIdAndUserId(dbContextFactory, (Guid)bookId, (Guid)userId);
                int newCount = newTags is null ? 0 : newTags.Count;

                Assert.IsNotNull(originalTags);
                Assert.IsNotNull(newTags);
                Assert.IsTrue(newCount > 0);
                Assert.AreEqual(expectedNewCount, newCount);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task BookTagAddAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            string tag = Guid.NewGuid().ToString();

            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                var originalTags = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                int originalCount = originalTags is null ? 0 : originalTags.Count;
                int expectedNewCount = originalCount + 1;

                var book = BookApi.BookRead(dbContextFactory, (Guid)bookId);
                Assert.IsNotNull(book);
                var user = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                Assert.IsNotNull(book);
                Assert.IsNotNull(user);

                await BookTagApi.BookTagAddAsync(dbContextFactory, book, user, tag);
                var newTags = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(dbContextFactory, (Guid)bookId, (Guid)userId);
                int newCount = newTags is null ? 0 : newTags.Count;

                Assert.IsNotNull(originalTags);
                Assert.IsNotNull(newTags);
                Assert.IsTrue(newCount > 0);
                Assert.AreEqual(expectedNewCount, newCount);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void BookTagRemoveTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            string tag1 = Guid.NewGuid().ToString();
            string tag2 = $"{Guid.NewGuid().ToString()}varbit"; // just adding an extra string in case the two are somehow identical

            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                var originalTags = BookTagApi.BookTagsGetByBookIdAndUserId(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                int originalCount = originalTags is null ? 0 : originalTags.Count;
                int expectedNewCountFirst = originalCount + 2;
                int expectedNewCountSecond = originalCount + 1;

                var book = BookApi.BookRead(dbContextFactory, (Guid)bookId);
                Assert.IsNotNull(book);
                var user = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                Assert.IsNotNull(book);
                Assert.IsNotNull(user);

                BookTagApi.BookTagAdd(dbContextFactory, book, user, tag1);
                BookTagApi.BookTagAdd(dbContextFactory, book, user, tag2);
                var newTagsFirst = BookTagApi.BookTagsGetByBookIdAndUserId(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                int newCountFirst = newTagsFirst is null ? 0 : newTagsFirst.Count;
                BookTagApi.BookTagRemove(dbContextFactory, (Guid)bookId, (Guid)userId, tag1);
                var newTagsSecond = BookTagApi.BookTagsGetByBookIdAndUserId(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }

        [TestMethod()]
        public async Task BookTagRemoveAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            string tag1 = Guid.NewGuid().ToString();
            string tag2 = $"{Guid.NewGuid().ToString()}varbit"; // just adding an extra string in case the two are somehow identical
            
            try
            {
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(
                    dbContextFactory, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid bookUserId = createResult.bookUserId;

                var originalTags = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                int originalCount = originalTags is null ? 0 : originalTags.Count;
                int expectedNewCountFirst = originalCount + 2;
                int expectedNewCountSecond = originalCount + 1;

                var book = BookApi.BookRead(dbContextFactory, (Guid)bookId);
                Assert.IsNotNull(book);
                var user = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                Assert.IsNotNull(book);
                Assert.IsNotNull(user);

                await BookTagApi.BookTagAddAsync(dbContextFactory, book, user, tag1);
                await BookTagApi.BookTagAddAsync(dbContextFactory, book, user, tag2);
                var newTagsFirst = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
                int newCountFirst = newTagsFirst is null ? 0 : newTagsFirst.Count;
                await BookTagApi.BookTagRemoveAsync(dbContextFactory, (Guid)bookId, (Guid)userId, tag1);
                var newTagsSecond = await BookTagApi.BookTagsGetByBookIdAndUserIdAsync(
                    dbContextFactory, (Guid)bookId, (Guid)userId);
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
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, dbContextFactory);
            }
        }
    }
}