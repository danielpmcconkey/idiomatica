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
                .AddTransient<UserService>()
                //.AddDbContext<IdiomaticaContext>(options => {
                //    options.UseSqlServer(connectionstring, b => b.MigrationsAssembly("IdiomaticaWeb"));
                //    })
                .BuildServiceProvider();
        }
        
        
        
        internal static UserService? CreateUserService()
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
        internal static User? CreateNewTestUser(UserService userService, IdiomaticaContext context)
        {
            var user = new User()
            {
                ApplicationUserId = Guid.NewGuid().ToString(),
                Name = "Auto gen tester",
                Code = "En-US"
            };
            user = DataCache.UserCreate(user, context);
            if (user is null || user.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return user;
            }
            context.Users.Add(user);
            var languageUser = new LanguageUser()
            {
                LanguageId = 1, // espAnish
                UserId = (Guid)user.UniqueKey,
                TotalWordsRead = 0
            };
            DataCache.LanguageUserCreate(languageUser, context);
            context.LanguageUsers.Add(languageUser);

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
