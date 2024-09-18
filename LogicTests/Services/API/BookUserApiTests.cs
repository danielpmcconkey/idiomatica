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
using Microsoft.EntityFrameworkCore;
using Model.DAL;
using Model.Enums;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class BookUserApiTests
    {
        [TestMethod()]
        public void BookUserArchiveTest()
        {
            Guid? bookId1 = null;
            Guid? bookId2 = null;
            Guid? bookId3 = null;
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            int expectedCountBefore = 3;
            int expectedCountAfter = 2;

            try
            {
                var originalPacket = new BookListDataPacket(context, false);

                // create the user and the three books
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                Book? book1 = CommonFunctions.CreateBook(context, (Guid)user.Id);
                if (book1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId1 = (Guid)book1.Id;

                Book? book2 = CommonFunctions.CreateBook(context, (Guid)user.Id);
                if (book2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId2 = (Guid)book2.Id;

                Book? book3 = CommonFunctions.CreateBook(context, (Guid)user.Id);
                if (book3 is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId3 = (Guid)book3.Id;

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId1, (Guid)user.Id);
                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId2, (Guid)user.Id);
                var bookUser3 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId3, (Guid)user.Id);
                Assert.IsNotNull(bookUser1);
                Assert.IsNotNull(bookUser2);
                Assert.IsNotNull(bookUser3);
                var newPacketBefore = BookApi.BookListRead(
                    context, (Guid)user.Id, originalPacket);
                int actualCountBefore = newPacketBefore is null ? 0 :
                    newPacketBefore.BookListRows is null ? 0 : newPacketBefore.BookListRows.Count;
                Guid? bookUser2Id = bookUser2 is null ? null : (Guid)bookUser2.Id;
                Assert.IsNotNull(bookUser2Id);
                BookUserApi.BookUserArchive(context, (Guid)bookUser2Id);
                var newPacketAfter = BookApi.BookListRead(
                    context, (Guid)user.Id, originalPacket);
                int actualCountAfter = newPacketAfter is null ? 0 :
                    newPacketAfter.BookListRows is null ? 0 : newPacketAfter.BookListRows.Count;

                Assert.AreEqual(expectedCountBefore, actualCountBefore);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId1 is not null) CommonFunctions.CleanUpBook(bookId1, context);
                if (bookId2 is not null) CommonFunctions.CleanUpBook(bookId2, context);
                if (bookId3 is not null) CommonFunctions.CleanUpBook(bookId3, context);
            }
        }
        [TestMethod()]
        public async Task BookUserArchiveAsyncTest()
        {
            Guid? bookId1 = null;
            Guid? bookId2 = null;
            Guid? bookId3 = null;
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            int expectedCountBefore = 3;
            int expectedCountAfter = 2;

            try
            {
                var originalPacket = new BookListDataPacket(context, false);

                // create the user and the three books
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                var languageUser = LanguageUserApi.LanguageUserGet(
                    context, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);
                
                Book? book1 = CommonFunctions.CreateBook(context, (Guid)user.Id);
                if (book1 is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId1 = (Guid)book1.Id;
               
                Book? book2 = CommonFunctions.CreateBook(context, (Guid)user.Id);
                if (book2 is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId2 = (Guid)book2.Id;
                
                Book? book3 = CommonFunctions.CreateBook(context, (Guid)user.Id);
                if (book3 is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId3 = (Guid)book3.Id;

                var bookUser1 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId1, (Guid)user.Id);
                var bookUser2 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId2, (Guid)user.Id);
                var bookUser3 = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId3, (Guid)user.Id);
                Assert.IsNotNull(bookUser1);
                Assert.IsNotNull(bookUser2);
                Assert.IsNotNull(bookUser3);
                var newPacketBefore = await BookApi.BookListReadAsync(
                    context, (Guid)user.Id, originalPacket);
                int actualCountBefore = newPacketBefore is null ? 0 :
                    newPacketBefore.BookListRows is null ? 0 : newPacketBefore.BookListRows.Count;
                Guid? bookUser2Id = bookUser2 is null ? null : (Guid)bookUser2.Id;
                Assert.IsNotNull(bookUser2Id);
                await BookUserApi.BookUserArchiveAsync(context, (Guid)bookUser2Id);
                var newPacketAfter = await BookApi.BookListReadAsync(
                    context, (Guid)user.Id, originalPacket);
                int actualCountAfter = newPacketAfter is null ? 0 :
                    newPacketAfter.BookListRows is null ? 0 : newPacketAfter.BookListRows.Count;
                
                Assert.AreEqual(expectedCountBefore, actualCountBefore);
                Assert.AreEqual(expectedCountAfter, actualCountAfter);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId1 is not null) CommonFunctions.CleanUpBook(bookId1, context);
                if (bookId2 is not null) CommonFunctions.CleanUpBook(bookId2, context);
                if (bookId3 is not null) CommonFunctions.CleanUpBook(bookId3, context);
            }
        }


        [TestMethod()]
        public void BookUserByBookIdAndUserIdReadTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid languageId = learningLanguage.Id;


            try
            {
                // create the user and book
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid expectedId = createResult.bookUserId;
                // grab the expected language user ID
                var lu = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(context, (Guid)userId);
                Assert.IsNotNull(lu);
                Assert.IsTrue(lu.Count < 1);
                var l = lu.Where(x => x.LanguageId == languageId).FirstOrDefault();
                Assert.IsNotNull(l);
                Guid expectedLanguageUserId = (Guid)l.Id;

                // now test the bookUser read
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                    context, (Guid)bookId, (Guid)userId);

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.AreEqual(expectedId, bookUser.Id);
                Assert.AreEqual(expectedLanguageUserId, bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookUserByBookIdAndUserIdReadAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);
            Guid languageId = learningLanguage.Id;
            

            try
            {
                // create the user and book
                Assert.IsNotNull(loginService);
                var createResult = CommonFunctions.CreateUserAndBookAndBookUser(context, loginService);
                userId = createResult.userId;
                bookId = createResult.bookId;
                Guid expectedId = createResult.bookUserId;
                // grab the expected language user ID
                var lu = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(context, (Guid)userId);
                Assert.IsNotNull(lu);
                Assert.IsTrue(lu.Count < 1);
                var l = lu.Where(x => x.LanguageId == languageId).FirstOrDefault();
                Assert.IsNotNull(l);
                Guid expectedLanguageUserId = (Guid)l.Id;

                // now test the bookUser read
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(
                    context, (Guid)bookId, (Guid)userId);
                
                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.AreEqual(expectedId, bookUser.Id);
                Assert.AreEqual(expectedLanguageUserId, bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void BookUserCreateTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);

                userId = (Guid)user.Id;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLanguage, user);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, (Guid)userId);
                if (book is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.Id;


                var bookUser = BookUserApi.BookUserCreate(
                    context, (Guid)bookId, (Guid)userId);


                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.IsNotNull(bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookUserCreateAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);

                userId = (Guid)user.Id;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLanguage, user);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                var book = CommonFunctions.CreateBook(context, (Guid)userId);
                if (book is null)
                { ErrorHandler.LogAndThrow(); return; }
                bookId = (Guid)book.Id;

                
                var bookUser = await BookUserApi.BookUserCreateAsync(
                    context, (Guid)bookId, (Guid)userId);
                

                Assert.IsNotNull(bookUser);
                Assert.AreEqual(bookId, bookUser.BookId);
                Assert.IsNotNull(bookUser.LanguageUserId);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }


        [TestMethod()]
        public void BookUserUpdateBookmarkTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // create the languageUser
                var languageUser = LanguageUserApi.LanguageUserCreate(
                    context, learningLanguage, user);
                Assert.IsNotNull(languageUser);

                // create the book
                var book = CommonFunctions.CreateBook(context, (Guid)userId);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // find the second page's ID
                var secondPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                Assert.IsNotNull(secondPage);
                Guid secondPageId = (Guid)secondPage.Id;

                // create the bookUser
                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);
                BookUserApi.BookUserUpdateBookmark(context, (Guid)bookUser.Id, secondPageId);
                var secondBookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                    context, (Guid)bookId, (Guid)user.Id);
                Guid? newBookmark = secondBookUser is null ? null :
                    secondBookUser.CurrentPageId is null ? null : (Guid)secondBookUser.CurrentPageId;


                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsNotNull(secondBookUser);
                Assert.AreEqual(secondPageId, newBookmark);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, context);
                if (bookId is not null) CommonFunctions.CleanUpBook(bookId, context);
            }
        }
        [TestMethod()]
        public async Task BookUserUpdateBookmarkAsyncTest()
        {
            Guid? userId = null;
            Guid? bookId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(context);

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, context);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // create the languageUser
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, learningLanguage, user);
                Assert.IsNotNull(languageUser);

                // create the book
                var book = CommonFunctions.CreateBook(context, (Guid)userId);
                Assert.IsNotNull(book);
                bookId = (Guid)book.Id;

                // find the second page's ID
                var secondPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                Assert.IsNotNull(secondPage);
                Guid secondPageId = (Guid)secondPage.Id;

                // create the bookUser
                var bookUser = OrchestrationApi.OrchestrateBookUserCreationAndSubProcesses(
                    context, (Guid)bookId, (Guid)user.Id);
                Assert.IsNotNull(bookUser);
                await BookUserApi.BookUserUpdateBookmarkAsync(context, (Guid)bookUser.Id, secondPageId);
                var secondBookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(
                    context, (Guid)bookId, (Guid)user.Id);
                Guid? newBookmark = secondBookUser is null ? null :
                    secondBookUser.CurrentPageId is null ? null : (Guid)secondBookUser.CurrentPageId;
                

                Assert.IsNotNull(bookUser);
                Assert.IsNotNull(bookUser.Id);
                Assert.IsNotNull(secondBookUser);
                Assert.AreEqual(secondPageId, newBookmark);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, context);
                if (bookId is not null) await CommonFunctions.CleanUpBookAsync(bookId, context);
            }
        }
    }
}