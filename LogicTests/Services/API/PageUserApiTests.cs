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
using System.Net;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class PageUserApiTests
    {
        [TestMethod()]
        public void PageUserCreateForPageIdAndUserIdTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            var startDateTime = DateTime.Now;

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
                if (languageUser is null || languageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the book
                Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // get the page Id
                var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (page is null || page.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var pageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)page.Id, (int)user.Id);

                // assert
                Assert.IsNotNull(pageUser);
                Assert.IsNotNull(pageUser.Id);
                Assert.AreEqual(page.Id, pageUser.PageId);
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task PageUserCreateForPageIdAndUserIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var startDateTime = DateTime.Now;

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
                if (languageUser is null || languageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the book
                Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // get the page Id
                var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (page is null || page.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var pageUser = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, (int)page.Id, (int)user.Id);

                // assert
                Assert.IsNotNull(pageUser);
                Assert.IsNotNull(pageUser.Id);
                Assert.AreEqual(page.Id, pageUser.PageId);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void PageUserMarkAsReadTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            var startDateTime = DateTime.Now;

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
                if (languageUser is null || languageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the book
                Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the  bookUser
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(context, (int)book.Id, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                {
                    // still null, something went wrong
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // create the page 1 and 2 page users first, because the read call doesn't
                var page1 = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (page1 is null || page1.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int page1Id = (int)page1.Id;
                var pageUserBefore = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page1Id, (int)user.Id);
                if (pageUserBefore is null || pageUserBefore.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // act
                PageUserApi.PageUserMarkAsRead(context, (int)pageUserBefore.Id);
                var pageUserAfter = PageUserApi.PageUserReadByPageIdAndLanguageUserId(
                    context, page1Id, (int)languageUser.Id);
                if (pageUserAfter is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }



                // assert
                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(startDateTime <= pageUserAfter.ReadDate);
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task PageUserMarkAsReadAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var startDateTime = DateTime.Now;

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
                if (languageUser is null || languageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the book
                Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the  bookUser
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(context, (int)book.Id, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                {
                    // still null, something went wrong
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // create the page 1 and 2 page users first, because the read call doesn't
                var page1 = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                if (page1 is null || page1.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int page1Id = (int)page1.Id;
                var pageUserBefore = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page1Id, (int)user.Id);
                if (pageUserBefore is null || pageUserBefore.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // act
                await PageUserApi.PageUserMarkAsReadAsync(context, (int)pageUserBefore.Id);
                var pageUserAfter = await PageUserApi.PageUserReadByPageIdAndLanguageUserIdAsync(
                    context, page1Id, (int)languageUser.Id);
                if (pageUserAfter is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }



                // assert
                Assert.IsNotNull(pageUserAfter.ReadDate);
                Assert.IsTrue(startDateTime <= pageUserAfter.ReadDate);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void PageUserReadBookmarkedOrFirstTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

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

                // create the book
                Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the  bookUser
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(context, (int)book.Id, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                {
                    // still null, something went wrong
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // create the page 1 and 2 page users first, because the read call doesn't
                var page1 = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                var page2 = book.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                if (page1 is null || page1.Id is null || page2 is null || page2.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int page1Id = (int)page1.Id;
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page1Id, (int)user.Id);

                int page2Id = (int)page2.Id;
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page2Id, (int)user.Id);


                // act
                // make the call before every doing anything
                var pageUserBefore = PageUserApi.PageUserReadBookmarkedOrFirst(context, (int)bookUser.Id);
                if (pageUserBefore is null || pageUserBefore.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // pull the page object associated to the pageUser
                var pageBefore = PageApi.PageReadById(context, (int)pageUserBefore.PageId);
                if (pageBefore is null || pageBefore.Ordinal is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                BookUserApi.BookUserUpdateBookmark(context, (int)bookUser.Id, page2Id);

                // now pull the page User again
                var pageUserAfter = PageUserApi.PageUserReadBookmarkedOrFirst(context, (int)bookUser.Id);
                if (pageUserAfter is null || pageUserAfter.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // pull the page object associated to the pageUser
                var pageAfter = PageApi.PageReadById(context, (int)pageUserAfter.PageId);
                if (pageAfter is null || pageAfter.Ordinal is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int ordinalAfterActual = (int)pageAfter.Ordinal;



                // assert
                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task PageUserReadBookmarkedOrFirstAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

            try
            {
                var userService = CommonFunctions.CreateUserService();
                var user = CommonFunctions.CreateNewTestUser(userService, context);

                if (user is null || user.Id is null || user.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                var languageUser = await LanguageUserApi.LanguageUserCreateAsync(
                    context, 1, (int)user.Id);
                if (languageUser is null) ErrorHandler.LogAndThrow();

                // create the book
                Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the  bookUser
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(
                    context, (int)book.Id, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                {
                    // still null, something went wrong
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // create the page 1 and 2 page users first, because the read call doesn't
                var page1 = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                var page2 = book.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                if (page1 is null || page1.Id is null || page2 is null || page2.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int page1Id = (int)page1.Id;
                var pageUser1 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page1Id, (int)user.Id);

                int page2Id = (int)page2.Id;
                var pageUser2 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page2Id, (int)user.Id);


                // act
                // make the call before every doing anything
                var pageUserBefore = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(
                    context, (int)bookUser.Id);
                if (pageUserBefore is null || pageUserBefore.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // pull the page object associated to the pageUser
                var pageBefore = await PageApi.PageReadByIdAsync(context, (int)pageUserBefore.PageId);
                if (pageBefore is null || pageBefore.Ordinal is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                await BookUserApi.BookUserUpdateBookmarkAsync(context, (int)bookUser.Id, page2Id);

                // now pull the page User again
                var pageUserAfter = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(
                    context, (int)bookUser.Id);
                if (pageUserAfter is null || pageUserAfter.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // pull the page object associated to the pageUser
                var pageAfter = await PageApi.PageReadByIdAsync(context, (int)pageUserAfter.PageId);
                if (pageAfter is null || pageAfter.Ordinal is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int ordinalAfterActual = (int)pageAfter.Ordinal;



                // assert
                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void PageUserReadByOrderWithinBookTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

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

                // create the book
                Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the  bookUser
                var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(context, (int)book.Id, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                {
                    // still null, something went wrong
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // create the page 1 and 2 page users first, because the read call doesn't
                var page1 = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                var page2 = book.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                if (page1 is null || page1.Id is null || page2 is null || page2.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int page1Id = (int)page1.Id;
                var pageUser1 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page1Id, (int)user.Id);

                int page2Id = (int)page2.Id;
                var pageUser2 = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, page2Id, (int)user.Id);


                // act
                // make the call before every doing anything
                var pageUserBefore = PageUserApi.PageUserReadBookmarkedOrFirst(context, (int)bookUser.Id);
                if (pageUserBefore is null || pageUserBefore.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // pull the page object associated to the pageUser
                var pageBefore = PageApi.PageReadById(context, (int)pageUserBefore.PageId);
                if (pageBefore is null || pageBefore.Ordinal is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                BookUserApi.BookUserUpdateBookmark(context, (int)bookUser.Id, page2Id);

                // now pull the page User again
                var pageUserAfter = PageUserApi.PageUserReadBookmarkedOrFirst(context, (int)bookUser.Id);
                if (pageUserAfter is null || pageUserAfter.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // pull the page object associated to the pageUser
                var pageAfter = PageApi.PageReadById(context, (int)pageUserAfter.PageId);
                if (pageAfter is null || pageAfter.Ordinal is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int ordinalAfterActual = (int)pageAfter.Ordinal;



                // assert
                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task PageUserReadByOrderWithinBookAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            int ordinalBeforeExpected = 1;
            int ordinalAfterExpected = 2;

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

                // create the book
                Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                    context,
                    (int)user.Id,
                    TestConstants.NewBookTitle,
                    TestConstants.NewBookLanguageCode,
                    TestConstants.NewBookUrl,
                    TestConstants.NewBookText);
                if (book is null || book.Id is null || book.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }

                // create the  bookUser
                var bookUser = await BookUserApi.BookUserByBookIdAndUserIdReadAsync(context, (int)book.Id, (int)user.Id);
                if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
                {
                    // still null, something went wrong
                    ErrorHandler.LogAndThrow();
                    return;
                }


                // create the page 1 and 2 page users first, because the read call doesn't
                var page1 = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
                var page2 = book.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
                if (page1 is null || page1.Id is null || page2 is null || page2.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int page1Id = (int)page1.Id;
                var pageUser1 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page1Id, (int)user.Id);

                int page2Id = (int)page2.Id;
                var pageUser2 = await PageUserApi.PageUserCreateForPageIdAndUserIdAsync(
                    context, page2Id, (int)user.Id);


                // act
                // make the call before every doing anything
                var pageUserBefore = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(context, (int)bookUser.Id);
                if (pageUserBefore is null || pageUserBefore.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // pull the page object associated to the pageUser
                var pageBefore = await PageApi.PageReadByIdAsync(context, (int)pageUserBefore.PageId);
                if (pageBefore is null || pageBefore.Ordinal is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int ordinalBeforeActual = (int)pageBefore.Ordinal;

                // update the bookmark as if we moved the page
                await BookUserApi.BookUserUpdateBookmarkAsync(context, (int)bookUser.Id, page2Id);

                // now pull the page User again
                var pageUserAfter = await PageUserApi.PageUserReadBookmarkedOrFirstAsync(context, (int)bookUser.Id);
                if (pageUserAfter is null || pageUserAfter.PageId is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                // pull the page object associated to the pageUser
                var pageAfter = await PageApi.PageReadByIdAsync(context, (int)pageUserAfter.PageId);
                if (pageAfter is null || pageAfter.Ordinal is null)
                {
                    ErrorHandler.LogAndThrow();
                    return;
                }
                int ordinalAfterActual = (int)pageAfter.Ordinal;



                // assert
                Assert.AreEqual(ordinalBeforeExpected, ordinalBeforeActual);
                Assert.AreEqual(ordinalAfterExpected, ordinalAfterActual);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void PageUserReadByPageIdAndLanguageUserIdTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task PageUserReadByPageIdAndLanguageUserIdAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();




            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void PageUserUpdateReadDateTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }
        [TestMethod()]
        public async Task PageUserUpdateReadDateAsyncTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();




            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }


        [TestMethod()]
        public void PageUserUpdateUnknowWordsToWellKnownAsyncTest()
        {
            Assert.Fail();
        }
        [TestMethod()]
        public void PageUserUpdateUnknowWordsToWellKnownTest()
        {
            // assemble
            var context = CommonFunctions.CreateContext();
            using var transaction = context.Database.BeginTransaction();

            try
            {
                // act


                // assert
                Assert.Fail();
            }
            finally
            {
                // clean-up
                transaction.Rollback();
            }
        }











    }
}