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
    public class WordUserApiTests
    {
        [TestMethod()]
        public async Task WordUsersCreateAllForBookIdAndUserIdAsyncTest()
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
        public async Task WordUsersDictByPageIdAndUserIdReadAsyncTest()
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
        public async Task WordUserCreateAsyncTest()
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
        public async Task WordUserUpdateAsyncTest()
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
        public void WordUsersCreateAllForBookIdAndUserIdTest()
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
        public void WordUsersDictByPageIdAndUserIdReadTest()
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
        public void WordUserCreateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WordUsersCreateAllForBookIdAndUserIdAsyncTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WordUserUpdateTest()
        {
            Assert.Fail();
        }
    }
}