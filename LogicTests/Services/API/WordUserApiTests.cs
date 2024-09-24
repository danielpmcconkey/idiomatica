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
    }
}