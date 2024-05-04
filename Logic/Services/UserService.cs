using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Logic.Services
{
    public class UserService
    {
        public User LoggedInUser;
        private IDbContextFactory<IdiomaticaContext> _dbContextFactory;
        private AuthenticationStateProvider _authenticationStateProvider;
        public UserService(IDbContextFactory<IdiomaticaContext> dbContextFactory, AuthenticationStateProvider AuthenticationStateProvider)
        {
            _dbContextFactory = dbContextFactory;
            _authenticationStateProvider = AuthenticationStateProvider;
        }

        public async Task GetLoggedInUserAsync()
        {
            var authState = await _authenticationStateProvider
               .GetAuthenticationStateAsync();
            var appUser = authState.User;

            if (appUser.Identity is not null && appUser.Identity.IsAuthenticated)
            {
                string appUserId = appUser.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var context = _dbContextFactory.CreateDbContext();
                LoggedInUser = context.Users.Where(x => x.ApplicationUserId == appUserId).FirstOrDefault();
            }
            else
            {
                LoggedInUser = null;
            }
        }
    }
}
