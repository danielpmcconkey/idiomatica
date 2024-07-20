using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class PageUserApiTests
    {
        [TestMethod()]
        public async Task PageUserReadBookmarkedOrFirstAsyncTest()
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
        public async Task PageUserReadByOrderWithinBookAsyncTest()
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
        public async Task PageUserCreateForPageIdAndUserIdTest()
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
        public async Task PageUserMarkAsReadAsyncTest()
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
        public async Task PageUserUpdateUnknowWordsToWellKnownTest()
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
        public void PageUserReadBookmarkedOrFirstTest()
        {
            Assert.Fail();
        }

       

        [TestMethod()]
        public void PageUserReadByPageIdAndLanguageUserIdTest()
        {
            Assert.Fail();
        }

        

        [TestMethod()]
        public void PageUserReadByOrderWithinBookTest()
        {
            Assert.Fail();
        }

        

        

        [TestMethod()]
        public void PageUserCreateForPageIdAndUserIdAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PageUserMarkAsReadTest()
        {
            Assert.Fail();
        }

        

        [TestMethod()]
        public void PageUserUpdateReadDateTest()
        {
            Assert.Fail();
        }
    }
}