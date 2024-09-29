using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Services.API;
using LogicTests;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using Model.Enums;

namespace Logic.Services.API.Tests
{
    [TestClass()]
    public class WordUserApiTests
    {
        [TestMethod()]
        public void WordUserCreateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid wordId = CommonFunctions.GetWordId(dbContextFactory, "dice", learningLanguage.Id);
            Word? word = WordApi.WordGetById(dbContextFactory, wordId);
            Assert.IsNotNull(word);
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create word user
                var wordUser = WordUserApi.WordUserCreate(
                    dbContextFactory, word, languageUser, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task WordUserCreateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid wordId = CommonFunctions.GetWordId(dbContextFactory, "dice", learningLanguage.Id);
            Word? word = WordApi.WordGetById(dbContextFactory, wordId);
            Assert.IsNotNull(word);
            AvailableWordUserStatus status = AvailableWordUserStatus.UNKNOWN;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create word user
                var wordUser = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word, languageUser, null, status);
                Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
                Assert.AreEqual(wordId, wordUser.WordId);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void WordUsersCreateAllForBookIdAndUserIdTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid bookId = CommonFunctions.GetBookRapunzelId(dbContextFactory);
            Guid pageId = CommonFunctions.GetRapunzelPage1Id(dbContextFactory);
            int expectedCount = 107;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(
                    dbContextFactory, bookId, (Guid)user.Id);
                // now read them back for one of the pages
                var dict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    dbContextFactory, pageId, (Guid)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task WordUsersCreateAllForBookIdAndUserIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid bookId = CommonFunctions.GetBookRapunzelId(dbContextFactory);
            Guid pageId = CommonFunctions.GetRapunzelPage1Id(dbContextFactory);
            int expectedCount = 107;

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(
                    dbContextFactory, bookId, (Guid)user.Id);
                // now read them back for one of the pages
                var dict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    dbContextFactory, pageId, (Guid)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void WordUsersDictByPageIdAndUserIdReadTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid bookId = CommonFunctions.GetBookRapunzelId(dbContextFactory);
            Guid pageId = CommonFunctions.GetRapunzelPage1Id(dbContextFactory);
            int expectedCount = 107;
            string wordToCheck = "cautivado";

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the wordUsers
                WordUserApi.WordUsersCreateAllForBookIdAndUserId(
                    dbContextFactory, bookId, (Guid)user.Id);

                // now read them back for one of the pages
                var dict = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                    dbContextFactory, pageId, (Guid)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
                var wordUser = dict[wordToCheck];
                Assert.IsNotNull(wordUser);
                Assert.IsNotNull(wordUser.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task WordUsersDictByPageIdAndUserIdReadAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid bookId = CommonFunctions.GetBookRapunzelId(dbContextFactory);
            Guid pageId = CommonFunctions.GetRapunzelPage1Id(dbContextFactory);
            int expectedCount = 107;
            string wordToCheck = "cautivado";

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // create the wordUsers
                await WordUserApi.WordUsersCreateAllForBookIdAndUserIdAsync(
                    dbContextFactory, bookId, (Guid)user.Id);

                // now read them back for one of the pages
                var dict = await WordUserApi.WordUsersDictByPageIdAndUserIdReadAsync(
                    dbContextFactory, pageId, (Guid)user.Id);
                Assert.IsNotNull(dict);
                Assert.AreEqual(expectedCount, dict.Count);
                var wordUser = dict[wordToCheck];
                Assert.IsNotNull(wordUser);
                Assert.IsNotNull(wordUser.Id);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void WordUserUpdateTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid wordId = CommonFunctions.GetWordId(dbContextFactory, "dice", language.Id);
            Word? word = WordApi.WordGetById(dbContextFactory, wordId);
            Assert.IsNotNull(word);
            AvailableWordUserStatus statusBefore = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfter = AvailableWordUserStatus.LEARNING3;
            string translation = "he/she says";

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, language.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var wordUserBefore = WordUserApi.WordUserCreate(
                    dbContextFactory, word, languageUser, null, statusBefore);
                Assert.IsNotNull(wordUserBefore); Assert.IsNotNull(wordUserBefore.Id);

                // update it
                WordUserApi.WordUserUpdate(dbContextFactory, wordUserBefore.Id, statusAfter, translation);

                // read it back
                var wordUserAfter = WordUserApi.WordUserReadById(dbContextFactory, wordUserBefore.Id);
                Assert.IsNotNull(wordUserAfter); Assert.IsNotNull(wordUserAfter.Id);
                Assert.AreEqual(statusAfter, wordUserAfter.Status);
                Assert.AreEqual(translation, wordUserAfter.Translation);
            }
            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }
        [TestMethod()]
        public async Task WordUserUpdateAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language language = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            Guid wordId = CommonFunctions.GetWordId(dbContextFactory, "dice", language.Id);
            Word? word = WordApi.WordGetById(dbContextFactory, wordId);
            Assert.IsNotNull(word);
            AvailableWordUserStatus statusBefore = AvailableWordUserStatus.UNKNOWN;
            AvailableWordUserStatus statusAfter = AvailableWordUserStatus.LEARNING3;
            string translation = "he/she says";

            try
            {
                // create a user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;
                Assert.IsNotNull(userId);

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, language.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var wordUserBefore = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word, languageUser, null, statusBefore);
                Assert.IsNotNull(wordUserBefore); Assert.IsNotNull(wordUserBefore.Id);

                // update it
                await WordUserApi.WordUserUpdateAsync(dbContextFactory, wordUserBefore.Id, statusAfter, translation);

                // read it back
                var wordUserAfter = await WordUserApi.WordUserReadByIdAsync(dbContextFactory, wordUserBefore.Id);
                Assert.IsNotNull(wordUserAfter); Assert.IsNotNull(wordUserAfter.Id);
                Assert.AreEqual(statusAfter, wordUserAfter.Status);
                Assert.AreEqual(translation, wordUserAfter.Translation);
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void WordUserReadByIdTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(dbContextFactory);
            Guid wordUserId = CommonFunctions.GetWordUser(dbContextFactory, languageUserId, "niña");
            AvailableWordUserStatus status = AvailableWordUserStatus.LEARNED;
            string translation = "girl";

            var wordUser = WordUserApi.WordUserReadById(dbContextFactory, wordUserId);
            Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
            Assert.AreEqual(status, wordUser.Status);
            Assert.AreEqual(translation, wordUser.Translation);
        }
        [TestMethod()]
        public async Task WordUserReadByIdAsyncTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            Guid languageUserId = CommonFunctions.GetSpanishLanguageUserId(dbContextFactory);
            Guid wordUserId = CommonFunctions.GetWordUser(dbContextFactory, languageUserId, "niña");
            AvailableWordUserStatus status = AvailableWordUserStatus.LEARNED;
            string translation = "girl";

            var wordUser = await WordUserApi.WordUserReadByIdAsync(dbContextFactory, wordUserId);
            Assert.IsNotNull(wordUser); Assert.IsNotNull(wordUser.Id);
            Assert.AreEqual(status, wordUser.Status);
            Assert.AreEqual(translation, wordUser.Translation);
        }

        [TestMethod()]
        public async Task WordUserReadForNextFlashCardAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode learningLanguageCode = learningLanguage.Code;
            Language uiLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = uiLanguage.Code;

            string expectedText1 = "de";
            string expectedText2 = "la";
            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = await CommonFunctions.CreateNewTestUserAsync(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var word1 = await WordApi.WordReadByLanguageIdAndTextAsync(
                    dbContextFactory, learningLanguage.Id, expectedText1);
                Assert.IsNotNull(word1);
                var word2 = await WordApi.WordReadByLanguageIdAndTextAsync(
                    dbContextFactory, learningLanguage.Id, expectedText2);
                Assert.IsNotNull(word2);

                // add some word users
                var wordUser1 = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);
                var wordUser2 = await WordUserApi.WordUserCreateAsync(
                    dbContextFactory, word2, languageUser, "the", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser2);

                // pull the first card and disposition
                var card = await OrchestrationApi.OrchestratePullFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);
                Assert.IsNotNull(card);

                // disposition the card so it doesn't come up when we pull the
                // next word user
                await OrchestrationApi.OrchestrateFlashCardDispositioningAsync(
                    dbContextFactory, card, AvailableFlashCardAttemptStatus.EASY);

                // now pull the next word user
                var wordUserPulled = await WordUserApi.WordUserReadForNextFlashCardAsync(
                    dbContextFactory, (Guid)userId, learningLanguageCode);
                Assert.IsNotNull(wordUserPulled);
                var wordPulled = await DataCache.WordByIdReadAsync(
                    wordUserPulled.WordId, dbContextFactory);
                Assert.IsNotNull(wordPulled);
                Assert.AreEqual(expectedText2, wordPulled.TextLowerCase);


            }

            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }


        [TestMethod()]
        public void WordUserReadForNextFlashCardTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            AvailableLanguageCode learningLanguageCode = learningLanguage.Code;
            Language uiLanguage = CommonFunctions.GetEnglishLanguage(dbContextFactory);
            AvailableLanguageCode uiLanguageCode = uiLanguage.Code;

            string expectedText1 = "de";
            string expectedText2 = "la";
            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = LanguageUserApi.LanguageUserGet(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                var word1 = WordApi.WordReadByLanguageIdAndText(
                    dbContextFactory, learningLanguage.Id, expectedText1);
                Assert.IsNotNull(word1);
                var word2 = WordApi.WordReadByLanguageIdAndText(
                    dbContextFactory, learningLanguage.Id, expectedText2);
                Assert.IsNotNull(word2);

                // add some word users
                var wordUser1 = WordUserApi.WordUserCreate(
                    dbContextFactory, word1, languageUser, "of", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser1);
                var wordUser2 = WordUserApi.WordUserCreate(
                    dbContextFactory, word2, languageUser, "the", AvailableWordUserStatus.UNKNOWN);
                Assert.IsNotNull(wordUser2);

                // pull the first card and disposition
                var card = OrchestrationApi.OrchestratePullFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode, uiLanguageCode);
                Assert.IsNotNull(card);

                // disposition the card so it doesn't come up when we pull the
                // next word user
                OrchestrationApi.OrchestrateFlashCardDispositioning(
                    dbContextFactory, card, AvailableFlashCardAttemptStatus.EASY);

                // now pull the next word user
                var wordUserPulled = WordUserApi.WordUserReadForNextFlashCard(
                    dbContextFactory, (Guid)userId, learningLanguageCode);
                Assert.IsNotNull(wordUserPulled);
                var wordPulled = DataCache.WordByIdRead(
                    wordUserPulled.WordId, dbContextFactory);
                Assert.IsNotNull(wordPulled);
                Assert.AreEqual(expectedText2, wordPulled.TextLowerCase);


            }

            finally
            {
                // clean-up
                if (userId is not null) CommonFunctions.CleanUpUser((Guid)userId, dbContextFactory);
            }
        }

        /// <summary>
        /// there is no synchronous version of this test because there's no
        /// synchronous version of the API call 
        /// </summary>
        [TestMethod()]
        public async Task WordUserProgressTotalsCreateForLanguageUserIdAsyncTest()
        {
            Guid? userId = null;
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var loginService = CommonFunctions.GetRequiredService<LoginService>();
            var context = dbContextFactory.CreateDbContext();
            Language learningLanguage = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            var bookId = CommonFunctions.GetBookEspañaId(dbContextFactory); // 'España'
            Guid languageId = CommonFunctions.GetSpanishLanguageId(dbContextFactory);
            Language language = CommonFunctions.GetSpanishLanguage(dbContextFactory);
            /*
             * 439 distinct words in the book
             * 49 distinct words on Page 1
             * 135 distinct words on Page 2
             * mark all 50 from page 1 as WELLKNOWN
             * mark 10 of the words from page 2 as NEW1
             * mark 9 of the words from page 2 as NEW2
             * mark 8 of the words from page 2 as LEARNING3
             * mark 7 of the words from page 2 as LEARNING4
             * mark 6 of the words from page 2 as LEARNED
             * mark 5 of the words from page 2 as IGNORED
             * remaining words leave as UNKNOWN (345)
             * */
            // map the AvailableWordUserStatus
            Dictionary<int, (int count, int first, int last)> expectations = [];
            expectations.Add(1, (10,    0,      9));
            expectations.Add(2, (9,    10,     18));
            expectations.Add(3, (8,    19,     26));
            expectations.Add(4, (7,    27,     33));
            expectations.Add(5, (6,    34,     39));
            expectations.Add(6, (5,    40,     44));
            expectations.Add(7, (49,   -1,     -1));
            expectations.Add(8, (345,  -1,     -1));

            try
            {
                // create the user
                Assert.IsNotNull(loginService);
                var user = CommonFunctions.CreateNewTestUser(loginService, dbContextFactory);
                Assert.IsNotNull(user);
                userId = (Guid)user.Id;

                // fetch the languageUser
                var languageUser = await LanguageUserApi.LanguageUserGetAsync(
                    dbContextFactory, learningLanguage.Id, (Guid)userId);
                Assert.IsNotNull(languageUser);

                // go through ReadDataInit as if we were reading the book
                var readDataPacket = await OrchestrationApi.OrchestrateReadDataInitAsync(
                    dbContextFactory, loginService, bookId);
                Assert.IsNotNull(readDataPacket);
                Assert.IsNotNull(readDataPacket.Book);
                Assert.IsNotNull(readDataPacket.CurrentPageUser);
                Assert.IsNotNull(readDataPacket.CurrentPageUser.Page);
                Assert.IsNotNull(readDataPacket.AllWordsInPage);
                Assert.IsNotNull(readDataPacket.Paragraphs);
                Assert.IsNotNull(readDataPacket.LanguageUser);

                // clear the unknown words from P1 and move to P2
                await OrchestrationApi.OrchestrateClearPageAndMoveAsync(
                    dbContextFactory, readDataPacket, 2);

                var wordUsers = (from b in context.Books
                                 join p in context.Pages on b.Id equals p.BookId
                                 join pp in context.Paragraphs on p.Id equals pp.PageId
                                 join s in context.Sentences on pp.Id equals s.ParagraphId
                                 join t in context.Tokens on s.Id equals t.SentenceId
                                 join w in context.Words on t.WordId equals w.Id
                                 join wu in context.WordUsers on w.Id equals wu.WordId
                                 where (
                                     b.Id == readDataPacket.Book.Id &&
                                     p.Ordinal == 2 &&
                                     wu.LanguageUserId == languageUser.Id &&
                                     wu.Status != AvailableWordUserStatus.WELLKNOWN // don't want to grab any of the words we just edited
                                     )
                                 select wu)
                                .Distinct()
                                .ToArray();

                for(int i = 1; i < 7; i++)
                {
                    var thisRow = expectations[i];
                    var wordUsersToUpdate = wordUsers[thisRow.first..(thisRow.last + 1)];
                    foreach (var x in wordUsersToUpdate) x.Status = (AvailableWordUserStatus)i;
                }
                context.SaveChanges();

                // finally run the API call to update the stats
                await WordUserApi.WordUserProgressTotalsCreateForLanguageUserIdAsync(
                    languageUser.Id, dbContextFactory);

                // because the update call is completely async, we don't want
                // to test the results until it's written. but the method will
                // return instantly
                await Task.Delay(5000);

                for (int i = 1; i <= 8; i++)
                {
                    var thisRow = expectations[i];
                    var thisResult = context.WordUserProgressTotals
                        .Where(
                            x => x.LanguageUserId == languageUser.Id &&
                            x.Status == (AvailableWordUserStatus)i)
                        .OrderByDescending(x => x.Created)
                        .FirstOrDefault();
                    Assert.IsNotNull(thisResult);
                    Assert.AreEqual(thisRow.count, thisResult.Total);
                }
            }
            finally
            {
                // clean-up
                if (userId is not null) await CommonFunctions.CleanUpUserAsync((Guid)userId, dbContextFactory);
            }
        }
    }
}