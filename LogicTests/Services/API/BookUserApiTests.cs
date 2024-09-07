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
            Guid userId = Guid.NewGuid();
            Guid? bookId1 = null;
            Guid? bookId2 = null;
            Guid? bookId3 = null;
            var context = CommonFunctions.CreateContext();

            int expectedCountBefore = 3;
            int expectedCountAfter = 2;

            try
            {
                var originalPacket = new BookListDataPacket(context, false);

                // create the user and the three books
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;
                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                Book? book1 = CommonFunctions.CreateBook(context, (Guid)user.UniqueKey);
                if (book1 is null || book1.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId1 = (Guid)book1.UniqueKey;

                Book? book2 = CommonFunctions.CreateBook(context, (Guid)user.UniqueKey);
                if (book2 is null || book2.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId2 = (Guid)book2.UniqueKey;

                Book? book3 = CommonFunctions.CreateBook(context, (Guid)user.UniqueKey);
                if (book3 is null || book3.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId3 = (Guid)book3.UniqueKey;

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId1, (Guid)user.UniqueKey);
                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId2, (Guid)user.UniqueKey);
                var bookUser3 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId3, (Guid)user.UniqueKey);
                var newPacketBefore = BookApi.BookListRead(context, (Guid)user.UniqueKey, originalPacket);
                int actualCountBefore = newPacketBefore is null ? 0 :
                    newPacketBefore.BookListRows is null ? 0 : newPacketBefore.BookListRows.Count;
                Guid? bookUser2Id = bookUser2 is null ? null : bookUser2.UniqueKey is null ? null : (Guid)bookUser2.UniqueKey;
                Assert.IsNotNull(bookUser2Id);
                BookUserApi.BookUserArchive(context, (Guid)bookUser2Id);
                var newPacketAfter = BookApi.BookListRead(context, (Guid)user.UniqueKey, originalPacket);
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
            Guid userId = Guid.NewGuid();
            Guid? bookId1 = null;
            Guid? bookId2 = null;
            Guid? bookId3 = null;
            var context = CommonFunctions.CreateContext();
            
            int expectedCountBefore = 3;
            int expectedCountAfter = 2;

            try
            {
                var originalPacket = new BookListDataPacket(context, false);

                // create the user and the three books
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;
                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();
                
                Book? book1 = CommonFunctions.CreateBook(context, (Guid)user.UniqueKey);
                if (book1 is null || book1.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId1 = (Guid)book1.UniqueKey;
               
                Book? book2 = CommonFunctions.CreateBook(context, (Guid)user.UniqueKey);
                if (book2 is null || book2.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId2 = (Guid)book2.UniqueKey;
                
                Book? book3 = CommonFunctions.CreateBook(context, (Guid)user.UniqueKey);
                if (book3 is null || book3.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId3 = (Guid)book3.UniqueKey;

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId1, (Guid)user.UniqueKey);
                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId2, (Guid)user.UniqueKey);
                var bookUser3 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId3, (Guid)user.UniqueKey);
                var newPacketBefore = await BookApi.BookListReadAsync(context, (Guid)user.UniqueKey, originalPacket);
                int actualCountBefore = newPacketBefore is null ? 0 :
                    newPacketBefore.BookListRows is null ? 0 : newPacketBefore.BookListRows.Count;
                Guid? bookUser2Id = bookUser2 is null ? null : bookUser2.UniqueKey is null ? null : (Guid)bookUser2.UniqueKey;
                await BookUserApi.BookUserArchiveAsync(context, (Guid)bookUser2Id);
                var newPacketAfter = await BookApi.BookListReadAsync(context, (Guid)user.UniqueKey, originalPacket);
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
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);


            try
            {
                // create the user and book
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid expectedId = createResult.bookUserId;
                // grab the expected language user ID
                var lu = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(context, userId);
                if (lu is null || lu.Count < 1) { ErrorHandler.LogAndThrow(); return; }
                var l = lu.Where(x => x.LanguageKey == languageId).FirstOrDefault();
                if (l is null || l.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                Guid expectedLanguageUserId = (Guid)l.UniqueKey;

                // now test the bookUser read
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(context, bookId, userId);

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookKey);
                Assert.AreEqual(expectedId, bookUser.UniqueKey);
                Assert.AreEqual(expectedLanguageUserId, bookUser.LanguageUserKey);
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
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();
            Guid languageId = CommonFunctions.GetSpanishLanguageKey(context);
            

            try
            {
                // create the user and book
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, userService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid expectedId = createResult.bookUserId;
                // grab the expected language user ID
                var lu = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(context, userId);
                if (lu is null || lu.Count < 1) { ErrorHandler.LogAndThrow(); return; }
                var l = lu.Where(x => x.LanguageKey == languageId).FirstOrDefault();
                if (l is null || l.UniqueKey is null) { ErrorHandler.LogAndThrow(); return; }
                Guid expectedLanguageUserId = (Guid)l.UniqueKey;

                // now test the bookUser read
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(context, bookId, userId);
                
                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookKey);
                Assert.AreEqual(expectedId, bookUser.UniqueKey);
                Assert.AreEqual(expectedLanguageUserId, bookUser.LanguageUserKey);
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
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, userId);
                if (book is null || book.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // act
                var bookUser = BookUserApi.BookUserCreate(context, bookId, userId);
                // assert

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookKey);
                Assert.IsNotNull(bookUser.LanguageUserKey);
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
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, userId);
                if (book is null || book.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                
                var bookUser = await BookUserApi.BookUserCreateAsync(context, bookId, userId);
                

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookKey);
                Assert.IsNotNull(bookUser.LanguageUserKey);
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
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, userId);
                if (book is null || book.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // find the second page's ID
                var secondPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (secondPage is null || secondPage.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                Guid secondPageId = (Guid)secondPage.UniqueKey;

                // create the bookUser
                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                BookUserApi.BookUserUpdateBookmark(context, (Guid)bookUser.UniqueKey, secondPageId);
                var secondBookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                    context, bookId, (Guid)user.UniqueKey);
                Guid? newBookmark = secondBookUser is null ? null :
                    secondBookUser.CurrentPageKey is null ? null :
                    (Guid)secondBookUser.CurrentPageKey;


                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.UniqueKey);
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
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            var context = CommonFunctions.CreateContext();

            try
            {
                // create the user
                var userService = CommonFunctions.CreateUserService();
                if (userService is null) { ErrorHandler.LogAndThrow(); return; }
                var user = CommonFunctions.CreateNewTestUser(userService, context);
                if (user is null || user.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                userId = (Guid)user.UniqueKey;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(context, CommonFunctions.GetSpanishLanguageKey(context), (Guid)user.UniqueKey);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, userId);
                if (book is null || book.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.UniqueKey;

                // find the second page's ID
                var secondPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (secondPage is null || secondPage.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                Guid secondPageId = (Guid)secondPage.UniqueKey;

                // create the bookUser
                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                context, bookId, (Guid)user.UniqueKey);
                if (bookUser is null || bookUser.UniqueKey is null)
                { ErrorHandler.LogAndThrow(); return; }
                await BookUserApi.BookUserUpdateBookmarkAsync(context, (Guid)bookUser.UniqueKey, secondPageId);
                var secondBookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(
                    context, bookId, (Guid)user.UniqueKey);
                Guid? newBookmark = secondBookUser is null ? null :
                    secondBookUser.CurrentPageKey is null ? null : (Guid)secondBookUser.CurrentPageKey;
                

                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.UniqueKey);
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