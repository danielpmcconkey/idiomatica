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
        private IDbContextFactory<IdiomaticaContext> _dbContextFactory;
        private AuthenticationStateProvider _authenticationStateProvider;
        public UserService(IDbContextFactory<IdiomaticaContext> dbContextFactory, AuthenticationStateProvider AuthenticationStateProvider)
        {
            _dbContextFactory = dbContextFactory;
            _authenticationStateProvider = AuthenticationStateProvider;
        }
        public User? GetLoggedInUser()
        {
            var t = Task.Run(() => GetAppUserClaimsPrincipalAsync());
            t.Wait();
            var appUser = t.Result;
            if (appUser == null) return null;
            if (appUser.Identity is null) return null;
            if (!appUser.Identity.IsAuthenticated) return null;
            
            var appUserId = appUser.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (appUserId == null) return null;
            //Console.WriteLine($"appUserId = {appUserId}");
            var context = _dbContextFactory.CreateDbContext();
            var matchingUsers = context.Users.Where(x => x.ApplicationUserId == appUserId).ToList();
            if(matchingUsers.Count() > 0) return matchingUsers[0];
            return null;
        }
        private async Task<ClaimsPrincipal?> GetAppUserClaimsPrincipalAsync()
        {
            var authState = await _authenticationStateProvider
               .GetAuthenticationStateAsync();
            return authState.User;
        }
    }
}
