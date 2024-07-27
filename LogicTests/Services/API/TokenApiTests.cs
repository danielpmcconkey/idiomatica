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
    public class TokenApiTests
    {
        [TestMethod()]
        public async Task TokensReadByPageIdAsyncTest()
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
        public async Task CreateTokenAsyncTest()
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
        public async Task CreateTokensFromSentenceAsyncTest()
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
        public async Task TokensAndWordsReadBySentenceIdAsyncTest()
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
        public async Task TokenGetChildObjectsAsyncTest()
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
        public void TokensReadByPageIdTest()
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
        public void CreateTokenTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTokensFromSentenceTest()
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
        public void TokenCreateAsyncTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TokenGetChildObjectsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void TokensAndWordsReadBySentenceIdTest()
        {
            Assert.Fail();
        }
    }
}