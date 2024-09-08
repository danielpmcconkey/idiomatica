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

namespace LogicTests
{
    internal static class CommonFunctions
    {
        private static ServiceProvider _serviceProvider;



        #region setup functions

        static CommonFunctions()
        {
            
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddScoped<AuthenticationStateProvider, RevalidatingProviderForUnitTesting>()
                .AddTransient<UserService>()
                .BuildServiceProvider();
        }


        
        internal static UserService? CreateUserService()
        {
            if(_serviceProvider is null) { ErrorHandler.LogAndThrow(); return null; }
            return _serviceProvider.GetService<UserService>();
        }
        internal static IdiomaticaContext CreateContext()
        {
            var connectionstring = "Server=localhost;Database=Idiomatica_test;Trusted_Connection=True;TrustServerCertificate=true;";
            var optionsBuilder = new DbContextOptionsBuilder<IdiomaticaContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            return new IdiomaticaContext(optionsBuilder.Options);
        }
        
#if DEBUG
        internal static void SetLoggedInUser(Model.User user, UserService userService, IdiomaticaContext context)
        {
            userService.SetLoggedInUserForTestBench(user, context);
        }
#endif
        #endregion

        #region cleanup functions
        internal static void CleanUpBook(Guid? bookId, IdiomaticaContext context)
        {
            if (bookId == null) return;
            BookApi.BookAndAllChildrenDelete(context, (Guid)bookId);
        }
        internal static async Task CleanUpBookAsync(Guid? bookId, IdiomaticaContext context)
        {
            if (bookId == null) return;
            await BookApi.BookAndAllChildrenDeleteAsync(context, (Guid)bookId);
        }
        internal static void CleanUpUser(Guid userId, IdiomaticaContext context)
        {
            UserApi.UserAndAllChildrenDelete(context, userId);
        }
        internal static async Task CleanUpUserAsync(Guid userId, IdiomaticaContext context)
        {
            await UserApi.UserAndAllChildrenDeleteAsync(context, userId);
        }
        #endregion

        #region object create functions
        internal static Book? CreateBook(IdiomaticaContext context, Guid userId)
        {
            Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                context,
                userId,
                TestConstants.NewBookTitle,
                TestConstants.NewBookLanguageCode,
                TestConstants.NewBookUrl,
                TestConstants.NewBookText);
            if (book is null || book.UniqueKey is null) { ErrorHandler.LogAndThrow(); return null; }
            return book;
        }
        internal static async Task <Book?> CreateBookAsync(IdiomaticaContext context, Guid userId)
        {
            Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                context,
                userId,
                TestConstants.NewBookTitle,
                TestConstants.NewBookLanguageCode,
                TestConstants.NewBookUrl,
                TestConstants.NewBookText);
            if (book is null || book.UniqueKey is null) { ErrorHandler.LogAndThrow(); return null; }
            return book;
        }
        internal static Guid GetLanguageKey(IdiomaticaContext context, string Code)
        {
            var lang = context.Languages.Where(x => x.Code == Code).FirstOrDefault();
            Assert.IsNotNull(lang);
            Assert.IsNotNull(lang.UniqueKey);
            return (Guid)lang.UniqueKey;
        }
        internal static Guid GetSpanishLanguageKey(IdiomaticaContext context)
        {
            return GetLanguageKey(context, "ES");
        }
        internal static Guid GetEnglishLanguageKey(IdiomaticaContext context)
        {
            return GetLanguageKey(context, "EN-US");
        }
        internal static Guid GetSpanishLanguageUserKey(IdiomaticaContext context)
        {
            Guid languageKey = GetLanguageKey(context, "ES");
            var languageUser = context.LanguageUsers
                .Where(x => x.LanguageKey == languageKey && x.User != null && x.User.Name == "Dev Test user")
                .FirstOrDefault();
            Assert.IsNotNull(languageUser);
            Assert.IsNotNull(languageUser.UniqueKey);
            return (Guid)languageUser.UniqueKey;
        }

        internal static (Guid userId, Guid bookId, Guid bookUserId) CreateUserAndBookAndBookUser(
            IdiomaticaContext context, UserService userService)
        {
            Guid userId = Guid.NewGuid();
            Guid bookId = Guid.NewGuid();
            Guid? bookUserId;

            


            var user = CreateNewTestUser(userService, context);

            if (user is null || user.UniqueKey is null)
            { ErrorHandler.LogAndThrow(); return (userId, bookId, Guid.NewGuid()); }
            userId = (Guid)user.UniqueKey;
            var languageUser = LanguageUserApi.LanguageUserCreate(context, GetSpanishLanguageKey(context),
                (Guid)user.UniqueKey);
            if (languageUser is null) ErrorHandler.LogAndThrow();


            Book? book = CreateBook(context, (Guid)user.UniqueKey);
            if (book is null || book.UniqueKey is null)
            { ErrorHandler.LogAndThrow(); return (userId, bookId, Guid.NewGuid()); }
            bookId = (Guid)book.UniqueKey;

            var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                context, (Guid)book.UniqueKey, (Guid)user.UniqueKey);
            if (bookUser is null || bookUser.UniqueKey is null)
            { ErrorHandler.LogAndThrow(); return (userId, bookId, Guid.NewGuid()); }
            bookUserId = (Guid)bookUser.UniqueKey;

            return (userId, bookId, (Guid)bookUserId);
        }
        internal static async Task<(Guid userId, Guid bookId, Guid bookUserId)> CreateUserAndBookAndBookUserAsync(
            IdiomaticaContext context, UserService userService)
        {
            return await Task<(Guid userId, Guid bookId, Guid bookUserId)>.Run(() =>
            {
                return CreateUserAndBookAndBookUser(context, userService);
            });
        }

        internal static User? CreateNewTestUser(UserService userService, IdiomaticaContext context)
        {
            var applicationUserId = Guid.NewGuid().ToString();
            var name = "Auto gen tester 2";
            var code = "En-US";
            var user = UserApi.UserCreate(applicationUserId, name, code, context);
            if (user is null || user.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            context.Users.Add(user);
#if DEBUG
            SetLoggedInUser(user, userService, context);
#endif
            return user;
        }
        internal static async Task<User?> CreateNewTestUserAsync(
            UserService userService, IdiomaticaContext context)
        {
            return await Task<User?>.Run(() =>
            {
                return CreateNewTestUser(userService, context);
            });
        }

        internal static Guid GetBook17Id(IdiomaticaContext context)
        {
            var book = context.Books.Where(x => x.Title == "España").FirstOrDefault();
            Assert.IsNotNull(book);
            Assert.IsNotNull(book.UniqueKey);
            return (Guid)book.UniqueKey;
        }
        internal static Guid GetBook11Id(IdiomaticaContext context)
        {
            var book = context.Books.Where(x => x.Title == "Rapunzel").FirstOrDefault();
            Assert.IsNotNull(book);
            Assert.IsNotNull(book.UniqueKey);
            return (Guid)book.UniqueKey;
        }

        internal static Guid GetWordKeyByTextLower(IdiomaticaContext context, Guid languageKey, string textLower)
        {
            var word = context.Words
                .Where(x => x.LanguageKey == languageKey && x.TextLowerCase == textLower)
                .FirstOrDefault();
            Assert.IsNotNull(word);
            Assert.IsNotNull(word.UniqueKey);
            return (Guid)word.UniqueKey;
        }

        internal static Guid GetPage392Id(IdiomaticaContext context)
        {
            var book = context.Books.Where(x => x.Title == "España").Include(x => x.Pages).FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 2).FirstOrDefault();
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.UniqueKey);
            return (Guid)page.UniqueKey;
        }
        internal static Guid GetPage400Id(IdiomaticaContext context)
        {
            var book = context.Books.Where(x => x.Title == "África del Norte").Include(x => x.Pages).FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.UniqueKey);
            return (Guid)page.UniqueKey;
        }
        internal static Guid GetPage384Id(IdiomaticaContext context)
        {
            var book = context.Books.Where(x => x.Title == "[GUION PARA VIDEO: EXPLICACIÓN DE \"COMPREHENSIBLE INPUT\" EN 5 MINUTOS]").Include(x => x.Pages).FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.UniqueKey);
            return (Guid)page.UniqueKey;
        }
        internal static Guid GetPage378Id(IdiomaticaContext context)
        {
            var book = context.Books
                .Where(x => x.Title == "Rapunzel")
                .Include(x => x.Pages)
                .FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.UniqueKey);
            return (Guid)page.UniqueKey;
        }
        internal static Guid GetParagraph14706Id(IdiomaticaContext context)
        {
            var book = context.Books
                .Where(x => x.Title == "África del Norte")
                .Include(x => x.Pages).ThenInclude(x => x.Paragraphs)
                .FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            var pp = page.Paragraphs.Where(x => x.Ordinal == 0).FirstOrDefault();
            Assert.IsNotNull(pp);
            Assert.IsNotNull(pp.UniqueKey);
            return (Guid)pp.UniqueKey;
        }
        internal static Guid GetParagraph14590Id(IdiomaticaContext context)
        {
            var book = context.Books
                .Where(x => x.Title == "Rapunzel")
                .Include(x => x.Pages).ThenInclude(x => x.Paragraphs)
                .FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            var pp = page.Paragraphs.Where(x => x.Ordinal == 0).FirstOrDefault();
            Assert.IsNotNull(pp);
            Assert.IsNotNull(pp.UniqueKey);
            return (Guid)pp.UniqueKey;
        }
        internal static Guid GetParagraph14594Id(IdiomaticaContext context)
        {
            var book = context.Books
                .Where(x => x.Title == "Rapunzel")
                .Include(x => x.Pages).ThenInclude(x => x.Paragraphs)
                .FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            var pp = page.Paragraphs.Where(x => x.Ordinal == 4).FirstOrDefault();
            Assert.IsNotNull(pp);
            Assert.IsNotNull(pp.UniqueKey);
            return (Guid)pp.UniqueKey;
        }

        internal static Guid GetPageUser380Id(IdiomaticaContext context, Guid languageUserKey)
        {
            var book = context.Books.Where(x => x.Title == "[GUION PARA VIDEO: EXPLICACIÓN DE \"COMPREHENSIBLE INPUT\" EN 5 MINUTOS]").Include(x => x.Pages).FirstOrDefault();
            Assert.IsNotNull(book);
            var page = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            Assert.IsNotNull(page);
            var BookUser = context.BookUsers
                .Where(x => x.BookKey == book.UniqueKey && x.LanguageUserKey == languageUserKey)
                .FirstOrDefault();
            Assert.IsNotNull(BookUser);
            var pageUser = context.PageUsers
                .Where(x => x.BookUserKey == book.UniqueKey && x.PageKey == page.UniqueKey)
                .FirstOrDefault();
            Assert.IsNotNull(pageUser);
            Assert.IsNotNull(pageUser.UniqueKey);
            return (Guid)pageUser.UniqueKey;
        }
        internal static Guid GetToken94322Id(IdiomaticaContext context)
        {
            var token = (
                from b in context.Books
                join p in context.Pages on b.UniqueKey equals p.BookKey
                join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                where (
                    b.Title == "Rapunzel"
                    && p.Ordinal == 1
                    && pp.Ordinal == 0
                    && s.Ordinal == 0
                    && t.Ordinal == 0)
                select t
                ).FirstOrDefault();
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.UniqueKey);
            return (Guid)token.UniqueKey;
        }


        internal static Guid GetFlashCard1Id(IdiomaticaContext context, Guid languageUserId)
        {
            // this is for the word "de" in spanish
            Guid languageKey = GetSpanishLanguageKey(context);
            var word = context.Words
                .Where(x => x.LanguageKey == languageKey && x.TextLowerCase == "de")
                .FirstOrDefault();
            Assert.IsNotNull (word);
            var wordUser = context.WordUsers
                .Where(x => x.WordKey == word.UniqueKey && x.LanguageUserKey == languageUserId)
                .FirstOrDefault();
            Assert.IsNotNull(wordUser);
            var flashCard = context.FlashCards
                .Where(x => x.WordUserKey == wordUser.UniqueKey)
                .FirstOrDefault();
            Assert.IsNotNull(flashCard);
            Assert.IsNotNull(flashCard.UniqueKey);
            return (Guid)flashCard.UniqueKey;
        }

        internal static Guid GetWordUser(IdiomaticaContext context, Guid languageUserId, string textLowerCase)
        {
            var wordUser = (
                from lu in context.LanguageUsers
                join wu in context.WordUsers on lu.UniqueKey equals wu.LanguageUserKey
                join w in context.Words on wu.WordKey equals w.UniqueKey
                where (
                    lu.UniqueKey == languageUserId && w.TextLowerCase == textLowerCase)
                select wu
                ).FirstOrDefault();
            Assert.IsNotNull(wordUser);
            Assert.IsNotNull(wordUser.UniqueKey);
            return (Guid)wordUser.UniqueKey;
        }

        internal static Guid GetSentence24379Id(IdiomaticaContext context)
        {
            var sentence = (
                from b in context.Books
                join p in context.Pages on b.UniqueKey equals p.BookKey
                join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                where (
                    b.Title == "Rapunzel"
                    && p.Ordinal == 1
                    && pp.Ordinal == 0
                    && s.Ordinal == 0)
                select s
                ).FirstOrDefault();
            Assert.IsNotNull(sentence);
            Assert.IsNotNull(sentence.UniqueKey);
            return (Guid)sentence.UniqueKey;
        }
        internal static Guid GetSentence24380Id(IdiomaticaContext context)
        {
            var sentence = (
                from b in context.Books
                join p in context.Pages on b.UniqueKey equals p.BookKey
                join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                where (
                    b.Title == "Rapunzel"
                    && p.Ordinal == 1
                    && pp.Ordinal == 0
                    && s.Ordinal == 1)
                select s
                ).FirstOrDefault();
            Assert.IsNotNull(sentence);
            Assert.IsNotNull(sentence.UniqueKey);
            return (Guid)sentence.UniqueKey;
        }

        internal static Guid GetWordId(IdiomaticaContext context, string textToLower, Guid languageKey)
        {
            var w = context.Words
                .Where(x => x.LanguageKey == languageKey && x.TextLowerCase == textToLower)
                .FirstOrDefault();
            Assert.IsNotNull(w);
            Assert.IsNotNull(w.UniqueKey);
            return (Guid)w.UniqueKey;
        }
        #endregion

    }
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
