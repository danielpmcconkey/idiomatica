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
            int userId = 0;
            int bookId1 = 0;
            int bookId2 = 0;
            int bookId3 = 0;
            var context = CommonFunctions.CreateContext();

            int expectedCountBefore = 3;
            int expectedCountAfter = 2;

            try
            {
                var originalPacket = new BookListDataPacket(context, false);

                // create the user and the three books
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                Book? book1 = CommonFunctions.CreateBook(context, (int)user.Id);
                if (book1 is null || book1.Id is null || book1.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                bookId1 = (int)book1.Id;

                Book? book2 = CommonFunctions.CreateBook(context, (int)user.Id);
                if (book2 is null || book2.Id is null || book2.Id < 2)
                { ErrorHandler.LogAndThrow(); return; }
                bookId2 = (int)book2.Id;

                Book? book3 = CommonFunctions.CreateBook(context, (int)user.Id);
                if (book3 is null || book3.Id is null || book3.Id < 3)
                { ErrorHandler.LogAndThrow(); return; }
                bookId3 = (int)book3.Id;

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

                Assert.AreEqual(expectedCountBefore, actualCountBefore);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);

            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId1, context);
                CommonFunctions.CleanUpBook(bookId2, context);
                CommonFunctions.CleanUpBook(bookId3, context);
            }
        }
        [TestMethod()]
        public async Task BookUserArchiveAsyncTest()
        {
            int userId = 0;
            int bookId1 = 0;
            int bookId2 = 0;
            int bookId3 = 0;
            var context = CommonFunctions.CreateContext();
            
            int expectedCountBefore = 3;
            int expectedCountAfter = 2;

            try
            {
                var originalPacket = new BookListDataPacket(context, false);

                // create the user and the three books
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                
                Book? book1 = CommonFunctions.CreateBook(context, (int)user.Id);
                if (book1 is null || book1.Id is null || book1.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                bookId1 = (int)book1.Id;
               
                Book? book2 = CommonFunctions.CreateBook(context, (int)user.Id);
                if (book2 is null || book2.Id is null || book2.Id < 2)
                { ErrorHandler.LogAndThrow(); return; }
                bookId2 = (int)book2.Id;
                
                Book? book3 = CommonFunctions.CreateBook(context, (int)user.Id);
                if (book3 is null || book3.Id is null || book3.Id < 3)
                { ErrorHandler.LogAndThrow(); return; }
                bookId3 = (int)book3.Id;

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
                
                Assert.AreEqual(expectedCountBefore, actualCountBefore);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);

            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId1, context);
                CommonFunctions.CleanUpBook(bookId2, context);
                CommonFunctions.CleanUpBook(bookId3, context);
            }
        }


        [TestMethod()]
        public void BookUserByBookIdAndUserIdReadTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 1;


            try
            {
                // create the user and book
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int expectedId = createResult.bookUserId;
                // grab the expected language user ID
                var lu = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(context, userId);
                if (lu is null || lu.Count < 1) { ErrorHandler.LogAndThrow(); return; }
                var l = lu.Where(x => x.LanguageId == languageId).FirstOrDefault();
                if (l is null || l.Id is null || l.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                int expectedLanguageUserId = (int)l.Id;

                // now test the bookUser read
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(context, bookId, userId);

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.AreEqual(expectedId, bookUser.Id);
                Assert.AreEqual(expectedLanguageUserId, bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookUserByBookIdAndUserIdReadAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();
            int languageId = 1;
            

            try
            {
                // create the user and book
                var userService = CommonFunctions.CreateUserService();
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                int expectedId = createResult.bookUserId;
                // grab the expected language user ID
                var lu = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(context, userId);
                if (lu is null || lu.Count < 1) { ErrorHandler.LogAndThrow(); return; }
                var l = lu.Where(x => x.LanguageId == languageId).FirstOrDefault();
                if (l is null || l.Id is null || l.Id < 1) { ErrorHandler.LogAndThrow(); return; }
                int expectedLanguageUserId = (int)l.Id;

                // now test the bookUser read
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(context, bookId, userId);
                
                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.AreEqual(expectedId, bookUser.Id);
                Assert.AreEqual(expectedLanguageUserId, bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void BookUserCreateTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, userId);
                if (book is null || book.Id is null || book.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

                // act
                var bookUser = BookUserApi.BookUserCreate(context, bookId, userId);
                // assert

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.IsNotNull(bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookUserCreateAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, userId);
                if (book is null || book.Id is null || book.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

                
                var bookUser = await BookUserApi.BookUserCreateAsync(context, bookId, userId);
                

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.IsNotNull(bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }


        [TestMethod()]
        public void BookUserUpdateBookmarkTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, userId);
                if (book is null || book.Id is null || book.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

                // find the second page's ID
                var secondPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (secondPage is null || secondPage.Id is null || secondPage.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                int secondPageId = (int)secondPage.Id;

                // create the bookUser
                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                BookUserApi.BookUserUpdateBookmark(context, (int)bookUser.Id, secondPageId);
                var secondBookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                    context, bookId, (int)user.Id);
                int newBookmark = secondBookUser is null ? 0 :
                    secondBookUser.CurrentPageID is null ? 0 : (int)secondBookUser.CurrentPageID;


                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsTrue(bookUser.Id > 0);
                Assert.IsNotNull(secondBookUser);
                Assert.AreEqual(secondPageId, newBookmark);
            }
            finally
            {
                // clean-up
                CommonFunctions.CleanUpUser(userId, context);
                CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookUserUpdateBookmarkAsyncTest()
        {
            int userId = 0;
            int bookId = 0;
            var context = CommonFunctions.CreateContext();

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.Id is null || user.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (int)user.Id;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, userId);
                if (book is null || book.Id is null || book.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (int)book.Id;

                // find the second page's ID
                var secondPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (secondPage is null || secondPage.Id is null || secondPage.Id < 1)
                { ErrorHandler.LogAndThrow(); return; }
                int secondPageId = (int)secondPage.Id;

                // create the bookUser
                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                context, bookId, (int)user.Id);
                if (bookUser is null || bookUser.Id is null)
                { ErrorHandler.LogAndThrow(); return; }
                await BookUserApi.BookUserUpdateBookmarkAsync(context, (int)bookUser.Id, secondPageId);
                var secondBookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(
                    context, bookId, (int)user.Id);
                int newBookmark = secondBookUser is null ? 0 :
                    secondBookUser.CurrentPageID is null ? 0 : (int)secondBookUser.CurrentPageID;
                

                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsTrue(bookUser.Id > 0);
                Assert.IsNotNull(secondBookUser);
                Assert.AreEqual(secondPageId, newBookmark);
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