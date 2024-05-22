﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Model;
using Logic.UILabels;

namespace Logic.Services
{
    public class UserService
    {
        private ClaimsPrincipal? _loggedInUserClaimsPrincipal;
        private User? _loggedInUser;
        private UILabels.UILabels _uiLabels;
        private LanguageCode _uiLanguageCode;

        private IDbContextFactory<IdiomaticaContext> _dbContextFactory;
        private AuthenticationStateProvider _authenticationStateProvider;
        public UserService(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, 
            AuthenticationStateProvider AuthenticationStateProvider)
        {
            _dbContextFactory = dbContextFactory;
            _authenticationStateProvider = AuthenticationStateProvider;
            _uiLabels = UILabels.Factory.GetUILabels(LanguageCodeEnum.EN_US);
        }
        public User? GetLoggedInUser()
        {
            var t = Task.Run(() => GetAppUserClaimsPrincipalAsync());
            t.Wait();
            _loggedInUserClaimsPrincipal = t.Result;
            if (_loggedInUserClaimsPrincipal == null) return null;
            if (_loggedInUserClaimsPrincipal.Identity is null) return null;
            if (!_loggedInUserClaimsPrincipal.Identity.IsAuthenticated) return null;
            
            var appUserId = _loggedInUserClaimsPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (appUserId == null) return null;
            //Console.WriteLine($"appUserId = {appUserId}");
            var context = _dbContextFactory.CreateDbContext();
            var matchingUsers = context.Users.Where(x => x.ApplicationUserId == appUserId)
                .Include(u => u.LanguageCode)
                .ToList();
            if (matchingUsers.Count() > 0)
            {
                _loggedInUser = matchingUsers[0];
                _uiLanguageCode = _loggedInUser.LanguageCode;
                _uiLabels = UILabels.Factory.GetUILabels(_uiLanguageCode.LanguageCodeEnum);
                return _loggedInUser;
            }
            return null;
        }

        /// <summary>
        /// GetUILabel is used to return a UI lable, translated into the user's 
        /// UI language preference
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetUILabel(string name)
        {
            return _uiLabels.GetLabel(name);
        }
        public string GetUILabelF(string name, object?[] args)
        {
            return _uiLabels.GetLabelF(name, args);
        }
        public LanguageCode GetUiLanguageCode()
        {
            return _uiLanguageCode;
        }
        private async Task<ClaimsPrincipal?> GetAppUserClaimsPrincipalAsync()
        {
            var authState = await _authenticationStateProvider
               .GetAuthenticationStateAsync();
            return authState.User;
        }
    }
}
