using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;
using Model;
using System.Net;
using Logic.Telemetry;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookUserApiTests
    {
        [TestMethod()]
        public void BookUserArchiveTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            int bookId1 = 6;
            int bookId2 = 7;
            int bookId3 = 8;
            int expectedCountBefore = 3;
            int expectedCountAfter = 2;

            try
            {
                var originalPacket = new BookListDataPacket(context, false);

                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // act
                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId1, (int)user.Id);
                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId2, (int)user.Id);
                var bookUser3 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId3, (int)user.Id);
                var newPacketBefore = BookApi.BookListRead(context, (int)user.Id, originalPacket);
                int actualCountBefore = newPacketBefore is null ? 0 :
                    newPacketBefore.BookListRows is null ? 0 : newPacketBefore.BookListRows.Count;
                int bookUser2Id = bookUser2 is null ? 0 : bookUser2.Id is null ? 0 : (int)bookUser2.Id;
                BookUserApi.BookUserArchive(context, bookUser2Id);
                var newPacketAfter = BookApi.BookListRead(context, (int)user.Id, originalPacket);
                int actualCountAfter = newPacketAfter is null ? 0 :
                    newPacketAfter.BookListRows is null ? 0 : newPacketAfter.BookListRows.Count;
                // assert

                Assert.AreEqual(expectedCountBefore, actualCountBefore);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);

            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task BookUserArchiveAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            int bookId1 = 6;
            int bookId2 = 7;
            int bookId3 = 8;
            int expectedCountBefore = 3;
            int expectedCountAfter = 2;

            try
            {
                var originalPacket = new BookListDataPacket(context, false);

                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // act
                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId1, (int)user.Id);
                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId2, (int)user.Id);
                var bookUser3 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId3, (int)user.Id);
                var newPacketBefore = await BookApi.BookListReadAsync(context, (int)user.Id, originalPacket);
                int actualCountBefore = newPacketBefore is null ? 0 :
                    newPacketBefore.BookListRows is null ? 0 : newPacketBefore.BookListRows.Count;
                int bookUser2Id = bookUser2 is null ? 0 : bookUser2.Id is null ? 0 : (int)bookUser2.Id;
                await BookUserApi.BookUserArchiveAsync(context, bookUser2Id);
                var newPacketAfter = await BookApi.BookListReadAsync(context, (int)user.Id, originalPacket);
                int actualCountAfter = newPacketAfter is null ? 0 :
                    newPacketAfter.BookListRows is null ? 0 : newPacketAfter.BookListRows.Count;
                // assert

                Assert.AreEqual(expectedCountBefore, actualCountBefore);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);

            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void BookUserByBookIdAndUserIdReadTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            // try to pick a book where the book ID, user ID, language user ID, and book user ID are all different
            int bookId = 7;
            int userId = 11;
            int expectedId = 114;
            int expectedLanguageUserId = 10;

            try
            {
                // act
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(context, bookId, userId);
                // assert
                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.AreEqual(expectedId, bookUser.Id);
                Assert.AreEqual(expectedLanguageUserId, bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task BookUserByBookIdAndUserIdReadAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            // try to pick a book where the book ID, user ID, language user ID, and book user ID are all different
            int bookId = 7;
            int userId = 11;
            int expectedId = 114;
            int expectedLanguageUserId = 10;

            try
            {
                // act
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(context, bookId, userId);
                // assert
                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.AreEqual(expectedId, bookUser.Id);
                Assert.AreEqual(expectedLanguageUserId, bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void BookUserCreateTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();


            int bookId = 6;
            //var firstPage = PageApi.PageReadFirstByBookId(context, bookId);
            //var firstPageId = firstPage is null ? 0 : firstPage.Id is null ? 0 : (int)firstPage.Id;


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);

                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // act
                var bookUser = BookUserApi.BookUserCreate(context, bookId, (int)user.Id);
                // assert

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.IsNotNull(bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task BookUserCreateAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            int bookId = 6;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);

                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // act
                var bookUser = await BookUserApi.BookUserCreateAsync(context, bookId, (int)user.Id);
                // assert

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.IsNotNull(bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void BookUserUpdateBookmarkTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            int bookId = 6;
            int secondPageId = 67;


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);


                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();


                // act
                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                BookUserApi.BookUserUpdateBookmark(context, (int)bookUser.Id, secondPageId);
                var secondBookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                    context, bookId, (int)user.Id);
                int newBookmark = secondBookUser is null ? 0 :
                    secondBookUser.CurrentPageID is null ? 0 : (int)secondBookUser.CurrentPageID;
                // assert

                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsTrue(bookUser.Id > 0);
                Assert.IsNotNull(secondBookUser);
                Assert.AreEqual(secondPageId, newBookmark);
            }
            finally
            {
                // clean-up

                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task BookUserUpdateBookmarkAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();

            int bookId = 6;
            int secondPageId = 67;


            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);


                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();


                // act
                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                await BookUserApi.BookUserUpdateBookmarkAsync(context, (int)bookUser.Id, secondPageId);
                var secondBookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(
                    context, bookId, (int)user.Id);
                int newBookmark = secondBookUser is null ? 0 :
                    secondBookUser.CurrentPageID is null ? 0 : (int)secondBookUser.CurrentPageID;
                // assert

                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsTrue(bookUser.Id > 0);
                Assert.IsNotNull(secondBookUser);
                Assert.AreEqual(secondPageId, newBookmark);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }



    }
}