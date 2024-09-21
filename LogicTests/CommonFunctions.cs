using Logic.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Logic.Services.API;
using Logic.Telemetry;
using System.Net;
using Logic;
using Model.Enums;
using Microsoft.EntityFrameworkCore.Internal;

namespace LogicTests
{
    internal static class CommonFunctions
    {
        

        #region setup functions

        static CommonFunctions()
        {
            
        }

        private static ServiceProvider GetServiceProvider()
        {
            var connectionstring = "Server=localhost;Database=Idiomatica_test;Trusted_Connection=True;TrustServerCertificate=true;";

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddScoped<AuthenticationStateProvider, RevalidatingProviderForUnitTesting>();
            services.AddScoped<LoginService>();
            services.AddDbContextFactory<IdiomaticaContext>(options => {
                options.UseSqlServer(connectionstring, b => b.MigrationsAssembly("TestDataPopulator"));
            });
            return services.BuildServiceProvider();
        }
        /// <summary>
        /// The GetRequiredService method was taken from this guide
        /// https://gkama.medium.com/dependency-injection-di-in-net-core-and-net-5-c-unit-tests-935651a99a2d .
        /// This will also be needed for the test data population project, too. So centralize it someday?
        /// </summary>
        public static T GetRequiredService<T>() where T : notnull
        {
            var provider = GetServiceProvider();
            var service = provider.GetRequiredService<T>();
            Assert.IsNotNull(service);
            return service;
        }
        


        
#if DEBUG
        internal static void SetLoggedInUser(Model.User user, LoginService loginService, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            loginService.SetLoggedInUserForTestBench(user, dbContextFactory);
        }
#endif
        #endregion

        #region cleanup functions
        internal static void CleanUpBook(Guid? bookId, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            if (bookId == null) return;
            BookApi.BookAndAllChildrenDelete(dbContextFactory, (Guid)bookId);
        }
        internal static async Task CleanUpBookAsync(Guid? bookId, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            if (bookId == null) return;
            await BookApi.BookAndAllChildrenDeleteAsync(dbContextFactory, (Guid)bookId);
        }
        internal static void CleanUpUser(Guid userId, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            UserApi.UserAndAllChildrenDelete(dbContextFactory, userId);
        }
        internal static async Task CleanUpUserAsync(Guid userId, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            await UserApi.UserAndAllChildrenDeleteAsync(dbContextFactory, userId);
        }
        #endregion

        #region object create functions
        internal static Book? CreateBook(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            var context = dbContextFactory.CreateDbContext();
            Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                dbContextFactory,
                userId,
                TestConstants.NewBookTitle,
                TestConstants.NewBookLanguageCode,
                TestConstants.NewBookUrl,
                TestConstants.NewBookText);
            if (book is null) { ErrorHandler.LogAndThrow(); return null; }
            return book;
        }
        internal static async Task <Book?> CreateBookAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            var context = dbContextFactory.CreateDbContext();
            Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                dbContextFactory,
                userId,
                TestConstants.NewBookTitle,
                TestConstants.NewBookLanguageCode,
                TestConstants.NewBookUrl,
                TestConstants.NewBookText);
            if (book is null) { ErrorHandler.LogAndThrow(); return null; }
            return book;
        }
        internal static Guid GetLanguageId(IDbContextFactory<IdiomaticaContext> dbContextFactory, AvailableLanguageCode Code)
        {
            var context = dbContextFactory.CreateDbContext();
            var lang = context.Languages.Where(x => x.Code == Code).FirstOrDefault();
            Assert.IsNotNull(lang);
            Assert.IsNotNull(lang.Id);
            return (Guid)lang.Id;
        }
        internal static Language GetSpanishLanguage(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var lang = LanguageApi.LanguageReadByCode(dbContextFactory, AvailableLanguageCode.ES);
            if (lang is null) ErrorHandler.LogAndThrow();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
            return (Language)lang;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        internal static Guid GetSpanishLanguageId(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return GetLanguageId(dbContextFactory, AvailableLanguageCode.ES);
        }
        internal static Language GetEnglishLanguage(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var lang = LanguageApi.LanguageReadByCode(dbContextFactory, AvailableLanguageCode.EN_US);
            if (lang is null) ErrorHandler.LogAndThrow();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
            return (Language)lang;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        internal static Guid GetEnglishLanguageId(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return GetLanguageId(dbContextFactory, AvailableLanguageCode.EN_US);
        }
        internal static Guid GetSpanishLanguageUserId(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            Guid languageId = GetLanguageId(dbContextFactory, AvailableLanguageCode.ES);
            var languageUser = context.LanguageUsers
                .Where(x => x.LanguageId == languageId && x.User != null && x.User.Name == "Test / Dev user")
                .FirstOrDefault();
            Assert.IsNotNull(languageUser);
            Assert.IsNotNull(languageUser.Id);
            return (Guid)languageUser.Id;
        }
        internal static Guid GetTestUserId(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var user = context.Users
                .Where(x => x.Name == "Test / Dev user")
                .FirstOrDefault();
            Assert.IsNotNull(user);
            Assert.IsNotNull(user.Id);
            return (Guid)user.Id;
        }

        internal static (Guid userId, Guid bookId, Guid bookUserId) CreateUserAndBookAndBookUser(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, LoginService loginService)
        {
            Guid? userId = null;
            Guid? bookId = null;
            Guid? bookUserId = null;
            //var context = dbContextFactory.CreateDbContext();

            var user = CreateNewTestUser(loginService, dbContextFactory);
            Assert.IsNotNull(user);
            userId = user.Id;

            var languageUser = LanguageUserApi.LanguageUserGet(
                dbContextFactory, GetSpanishLanguageId(dbContextFactory), (Guid)userId);
            Assert.IsNotNull(languageUser);

            Book? book = CreateBook(dbContextFactory, (Guid)user.Id);
            Assert.IsNotNull(book);
            bookId = (Guid)book.Id;

            var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                dbContextFactory, (Guid)book.Id, (Guid)user.Id);
            Assert.IsNotNull(bookUser);
            bookUserId = (Guid)bookUser.Id;

            return ((Guid)userId, (Guid)bookId, (Guid)bookUserId);
        }
        internal static async Task<(Guid userId, Guid bookId, Guid bookUserId)> CreateUserAndBookAndBookUserAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, LoginService loginService)
        {
            return await Task<(Guid userId, Guid bookId, Guid bookUserId)>.Run(() =>
            {
                return CreateUserAndBookAndBookUser(dbContextFactory, loginService);
            });
        }
        internal static User? CreateNewTestUser(
            LoginService loginService, IDbContextFactory<IdiomaticaContext> dbContextFactory,
            AvailableLanguageCode learningLanguageCode, AvailableLanguageCode uiLanguageCode)
        {
            var applicationUserId = Guid.NewGuid().ToString();

            var uiLang = LanguageApi.LanguageReadByCode(dbContextFactory, uiLanguageCode);
            Assert.IsNotNull(uiLang);
            var uiLanguageId = uiLang.Id;

            var learningLang = LanguageApi.LanguageReadByCode(dbContextFactory, learningLanguageCode);
            Assert.IsNotNull(learningLang);
            var learningLanguageId = learningLang.Id;

            var name = "Auto gen tester";
            var user = UserApi.UserCreate(applicationUserId, name, dbContextFactory);
            if (user is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            LanguageUserApi.LanguageUserCreate(dbContextFactory, learningLang, user);
            UserApi.UserSettingCreate(dbContextFactory, AvailableUserSetting.UILANGUAGE,
                user.Id, uiLanguageId.ToString());
            UserApi.UserSettingCreate(dbContextFactory, AvailableUserSetting.CURRENTLEARNINGLANGUAGE,
                user.Id, learningLanguageId.ToString());
#if DEBUG
            SetLoggedInUser(user, loginService, dbContextFactory);
#endif
            return user;
        }

        internal static User? CreateNewTestUser(LoginService loginService, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return CreateNewTestUser(
                loginService, dbContextFactory, AvailableLanguageCode.ES, AvailableLanguageCode.EN_US);
        }
        internal static async Task<User?> CreateNewTestUserAsync(
            LoginService loginService, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<User?>.Run(() =>
            {
                return CreateNewTestUser(loginService, dbContextFactory);
            });
        }

        internal static Guid GetBookEspañaId(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books.Where(x => x.Title == "España").FirstOrDefault();
            Assert.IsNotNull(book);
            Assert.IsNotNull(book.Id);
            return (Guid)book.Id;
        }
        internal static Guid GetBookRapunzelId(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books.Where(x => x.Title == "Rapunzel").FirstOrDefault();
            Assert.IsNotNull(book);
            Assert.IsNotNull(book.Id);
            return (Guid)book.Id;
        }
        internal static Guid GetBookSelfishGiantId(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books.Where(x => x.Title == "The Selfish Giant").FirstOrDefault();
            Assert.IsNotNull(book);
            Assert.IsNotNull(book.Id);
            return (Guid)book.Id;
        }
        internal static Guid GetBookCenicientaId(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books.Where(x => x.Title == "Cenicienta").FirstOrDefault();
            Assert.IsNotNull(book);
            Assert.IsNotNull(book.Id);
            return (Guid)book.Id;
        }

        internal static Guid GetWordIdByTextLower(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageId, string textLower)
        {
            var context = dbContextFactory.CreateDbContext();
            var word = context.Words
                .Where(x => x.LanguageId == languageId && x.TextLowerCase == textLower)
                .FirstOrDefault();
            Assert.IsNotNull(word);
            Assert.IsNotNull(word.Id);
            return (Guid)word.Id;
        }

        internal static Guid GetPage392Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books.Where(x => x.Title == "España").Include(x => x.Pages).FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.Id);
            return (Guid)page.Id;
        }
        internal static Guid GetPage400Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books.Where(x => x.Title == "África del Norte").Include(x => x.Pages).FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.Id);
            return (Guid)page.Id;
        }
        internal static Guid GetGuionParaPage1Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books.Where(x => x.Title == "[GUION PARA VIDEO: EXPLICACIÓN DE \"COMPREHENSIBLE INPUT\" EN 5 MINUTOS]").Include(x => x.Pages).FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.Id);
            return (Guid)page.Id;
        }
        internal static Guid GetRapunzelPage1Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books
                .Where(x => x.Title == "Rapunzel")
                .Include(x => x.Pages)
                .FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.Id);
            return (Guid)page.Id;
        }
        internal static Guid GetParagraph14706Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books
                .Where(x => x.Title == "África del Norte")
                .Include(x => x.Pages).ThenInclude(x => x.Paragraphs)
                .FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            var pp = page.Paragraphs.Where(x => x.Ordinal == 0).FirstOrDefault();
            Assert.IsNotNull(pp);
            Assert.IsNotNull(pp.Id);
            return (Guid)pp.Id;
        }
        internal static Guid GetParagraph14590Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books
                .Where(x => x.Title == "Rapunzel")
                .Include(x => x.Pages).ThenInclude(x => x.Paragraphs)
                .FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            var pp = page.Paragraphs.Where(x => x.Ordinal == 0).FirstOrDefault();
            Assert.IsNotNull(pp);
            Assert.IsNotNull(pp.Id);
            return (Guid)pp.Id;
        }
        internal static Guid GetParagraph14594Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books
                .Where(x => x.Title == "Rapunzel")
                .Include(x => x.Pages).ThenInclude(x => x.Paragraphs)
                .FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            var pp = page.Paragraphs.Where(x => x.Ordinal == 4).FirstOrDefault();
            Assert.IsNotNull(pp);
            Assert.IsNotNull(pp.Id);
            return (Guid)pp.Id;
        }

        internal static Guid GetPageUser380Id(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageUserId)
        {
            var context = dbContextFactory.CreateDbContext();
            var book = context.Books.Where(x => x.Title == "[GUION PARA VIDEO: EXPLICACIÓN DE \"COMPREHENSIBLE INPUT\" EN 5 MINUTOS]").Include(x => x.Pages).FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            var bookUser = context.BookUsers
                .Where(x => x.BookId == book.Id && x.LanguageUserId == languageUserId)
                .FirstOrDefault();
            Assert.IsNotNull(bookUser);
            var pageUser = context.PageUsers
                .Where(x => x.BookUserId == bookUser.Id && x.PageId == page.Id)
                .FirstOrDefault();
            Assert.IsNotNull(pageUser);
            Assert.IsNotNull(pageUser.Id);
            return (Guid)pageUser.Id;
        }
        internal static Guid GetToken94322Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var token = (
                from b in context.Books
                join p in context.Pages on b.Id equals p.BookId
                join pp in context.Paragraphs on p.Id equals pp.PageId
                join s in context.Sentences on pp.Id equals s.ParagraphId
                join t in context.Tokens on s.Id equals t.SentenceId
                where (
                    b.Title == "Rapunzel"
                    && p.Ordinal == 1
                    && pp.Ordinal == 0
                    && s.Ordinal == 0
                    && t.Ordinal == 0)
                select t
                ).FirstOrDefault();
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.Id);
            return (Guid)token.Id;
        }


        internal static Guid GetFlashCard1Id(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageUserId)
        {
            var context = dbContextFactory.CreateDbContext();
            // this is for the word "de" in spanish
            Guid languageId = GetSpanishLanguageId(dbContextFactory);
            var word = context.Words
                .Where(x => x.LanguageId == languageId && x.TextLowerCase == "de")
                .FirstOrDefault();
            Assert.IsNotNull (word);
            var wordUser = context.WordUsers
                .Where(x => x.WordId == word.Id && x.LanguageUserId == languageUserId)
                .FirstOrDefault();
            Assert.IsNotNull(wordUser);
            var flashCard = context.FlashCards
                .Where(x => x.WordUserId == wordUser.Id)
                .FirstOrDefault();
            Assert.IsNotNull(flashCard);
            Assert.IsNotNull(flashCard.Id);
            return (Guid)flashCard.Id;
        }

        internal static Guid GetWordUser(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageUserId, string textLowerCase)
        {
            var context = dbContextFactory.CreateDbContext();
            var wordUser = (
                from lu in context.LanguageUsers
                join wu in context.WordUsers on lu.Id equals wu.LanguageUserId
                join w in context.Words on wu.WordId equals w.Id
                where (
                    lu.Id == languageUserId && w.TextLowerCase == textLowerCase)
                select wu
                ).FirstOrDefault();
            Assert.IsNotNull(wordUser);
            Assert.IsNotNull(wordUser.Id);
            return (Guid)wordUser.Id;
        }

        internal static Guid GetSentence24379Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var sentence = (
                from b in context.Books
                join p in context.Pages on b.Id equals p.BookId
                join pp in context.Paragraphs on p.Id equals pp.PageId
                join s in context.Sentences on pp.Id equals s.ParagraphId
                where (
                    b.Title == "Rapunzel"
                    && p.Ordinal == 1
                    && pp.Ordinal == 0
                    && s.Ordinal == 0)
                select s
                ).FirstOrDefault();
            Assert.IsNotNull(sentence);
            Assert.IsNotNull(sentence.Id);
            return (Guid)sentence.Id;
        }
        internal static Guid GetSentence24380Id(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            var sentence = (
                from b in context.Books
                join p in context.Pages on b.Id equals p.BookId
                join pp in context.Paragraphs on p.Id equals pp.PageId
                join s in context.Sentences on pp.Id equals s.ParagraphId
                where (
                    b.Title == "Rapunzel"
                    && p.Ordinal == 1
                    && pp.Ordinal == 0
                    && s.Ordinal == 1)
                select s
                ).FirstOrDefault();
            Assert.IsNotNull(sentence);
            Assert.IsNotNull(sentence.Id);
            return (Guid)sentence.Id;
        }

        internal static Guid GetWordId(IDbContextFactory<IdiomaticaContext> dbContextFactory, string textToLower, Guid languageId)
        {
            var context = dbContextFactory.CreateDbContext();
            var w = context.Words
                .Where(x => x.LanguageId == languageId && x.TextLowerCase == textToLower)
                .FirstOrDefault();
            Assert.IsNotNull(w);
            Assert.IsNotNull(w.Id);
            return (Guid)w.Id;
        }
        #endregion

    }

    /// <summary>
    /// // this code is copied to the test data populator project. Really need to consolidate
    /// </summary>
    internal class RevalidatingProviderForUnitTesting(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> options)
        : RevalidatingServerAuthenticationStateProvider(loggerFactory)
    {
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task<bool> ValidateAuthenticationStateAsync(
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            return true;
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return true;
        }
    }
}
