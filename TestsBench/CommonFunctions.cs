using Logic.Services;
using Logic.Telemetry;
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
using Logic;

namespace TestsBench
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
                .AddTransient<BookService>()
                .AddTransient<UserService>()
                .AddTransient<FlashCardService>()
                .AddTransient<DeepLService>()
                //.AddDbContext<IdiomaticaContext>(options => {
                //    options.UseSqlServer(connectionstring, b => b.MigrationsAssembly("IdiomaticaWeb"));
                //    })
                .BuildServiceProvider();
        }
        internal static BookService CreateBookService()
        {
            return _serviceProvider.GetService<BookService>();
        }
        internal static DeepLService CreateDeepLService()
        {
            return _serviceProvider.GetService<DeepLService>();
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
        internal static User CreateNewTestUser(UserService userService, IdiomaticaContext context)
        {
            var user = new User()
            {
                ApplicationUserId = Guid.NewGuid().ToString(),
                Name = "Auto gen tester",
                Code = "En-US"
            };
            context.Users.Add(user);
            context.SaveChanges();
            var languageUser = new LanguageUser()
            {
                LanguageId = 1, // espAnish
                UserId = (int)user.Id,
                TotalWordsRead = 0
            };
            context.LanguageUsers.Add(languageUser);
            context.SaveChanges();

#if DEBUG
            SetLoggedInUser(user, userService, context);
#endif

            return user;
        }
#if DEBUG
        internal static void SetLoggedInUser(User user, UserService userService, IdiomaticaContext context)
        {
            userService.SetLoggedInUserForTestBench(user, context);
        }
#endif
        #endregion

    }
}
