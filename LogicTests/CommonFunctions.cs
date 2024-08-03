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
            //setup our DI
            var connectionstring = "Server=localhost;Database=Idiomatica;Trusted_Connection=True;TrustServerCertificate=true;";

            _serviceProvider = new ServiceCollection()
                .AddLogging()
                //.AddCascadingAuthenticationState()
                //.AddScoped<IdentityUserAccessor>()
                //.AddScoped<IdentityRedirectManager>()
                .AddScoped<AuthenticationStateProvider, RevalidatingProviderForUnitTesting>()
                .AddTransient<UserService>()
                .AddTransient<FlashCardService>()
                //.AddDbContext<IdiomaticaContext>(options => {
                //    options.UseSqlServer(connectionstring, b => b.MigrationsAssembly("IdiomaticaWeb"));
                //    })
                .BuildServiceProvider();
        }


        internal static FlashCardService CreateFlashCardService()
        {
            return _serviceProvider.GetService<FlashCardService>();
        }
        internal static UserService CreateUserService()
        {
            //return new UserService(null);
            return _serviceProvider.GetService<UserService>();
        }
        internal static IdiomaticaContext CreateContext()
        {
            //var factory = _serviceProvider.GetService<DbContext<IdiomaticaContext>>();
            //var context = factory.CreateDbContext();
            //return context;
            var connectionstring = "Server=localhost;Database=Idiomatica;Trusted_Connection=True;TrustServerCertificate=true;";
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
        internal static void CleanUpBook(int bookId, IdiomaticaContext context)
        {
            BookApi.BookAndAllChildrenDelete(context, bookId);
        }
        internal static async Task CleanUpBookAsync(int bookId, IdiomaticaContext context)
        {
            await BookApi.BookAndAllChildrenDeleteAsync(context, bookId);
        }
        internal static void CleanUpUser(int userId, IdiomaticaContext context)
        {
            UserApi.UserAndAllChildrenDelete(context, userId);
        }
        internal static async Task CleanUpUserAsync(int userId, IdiomaticaContext context)
        {
            await UserApi.UserAndAllChildrenDeleteAsync(context, userId);
        }
        #endregion

        #region object create functions
        internal static Book? CreateBook(IdiomaticaContext context, int userId)
        {
            Book? book = OrchestrationApi.OrchestrateBookCreationAndSubProcesses(
                context,
                userId,
                TestConstants.NewBookTitle,
                TestConstants.NewBookLanguageCode,
                TestConstants.NewBookUrl,
                TestConstants.NewBookText);
            if (book is null || book.Id is null || book.Id < 1) { ErrorHandler.LogAndThrow(); return null; }
            return book;
        }
        internal static async Task <Book?> CreateBookAsync(IdiomaticaContext context, int userId)
        {
            Book? book = await OrchestrationApi.OrchestrateBookCreationAndSubProcessesAsync(
                context,
                userId,
                TestConstants.NewBookTitle,
                TestConstants.NewBookLanguageCode,
                TestConstants.NewBookUrl,
                TestConstants.NewBookText);
            if (book is null || book.Id is null || book.Id < 1) { ErrorHandler.LogAndThrow(); return null; }
            return book;
        }


        internal static (int userId, int bookId, int bookUserId) CreateUserAndBookAndBookUser(
            IdiomaticaContext context, UserService userService)
        {
            int userId = 0;
            int bookId = 0;
            int bookUserId = 0;

            var user = CreateNewTestUser(userService, context);

            if (user is null || user.Id is null || user.Id < 1)
            { ErrorHandler.LogAndThrow(); return (userId, bookId, bookUserId); }
            userId = (int)user.Id;
            var languageUser = LanguageUserApi.LanguageUserCreate(context, 1, (int)user.Id);
            if (languageUser is null) ErrorHandler.LogAndThrow();


            Book? book = CreateBook(context, (int)user.Id);
            if (book is null || book.Id is null || book.Id < 1)
            { ErrorHandler.LogAndThrow(); return (userId, bookId, bookUserId); }
            bookId = (int)book.Id;

            var bookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                context, (int)book.Id, (int)user.Id);
            if (bookUser is null || bookUser.Id is null || bookUser.Id < 1)
            { ErrorHandler.LogAndThrow(); return (userId, bookId, bookUserId); }
            bookUserId = (int)bookUser.Id;

            return (userId, bookId, bookUserId);
        }
        internal static async Task<(int userId, int bookId, int bookUserId)> CreateUserAndBookAndBookUserAsync(
            IdiomaticaContext context, UserService userService)
        {
            return await Task<(int userId, int bookId, int bookUserId)>.Run(() =>
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
            if (user is null || user.Id is null)
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
        #endregion

    }
    internal class RevalidatingProviderForUnitTesting(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> options)
        : RevalidatingServerAuthenticationStateProvider(loggerFactory)
    {
        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            return true;
        }

        private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
        {
            return true;
        }
    }
}
