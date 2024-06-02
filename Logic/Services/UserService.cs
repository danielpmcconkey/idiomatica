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
using Logic.UILabels;
using Polly;

namespace Logic.Services
{
    public class UserService
    {
        private ClaimsPrincipal? _loggedInUserClaimsPrincipal;
        private User? _loggedInUser;
        private UILabels.UILabels _uiLabels;
        private LanguageCode? _uiLanguageCode;
#if DEBUG
        /// <summary>
        /// this is used so that the test bench can be used without needing to go through front-end login
        /// </summary>
        private bool _loggedInUserOverride = false;
#endif


        private AuthenticationStateProvider _authenticationStateProvider;
        public UserService(
            AuthenticationStateProvider AuthenticationStateProvider)
        {
            _authenticationStateProvider = AuthenticationStateProvider;
            _uiLabels = UILabels.Factory.GetUILabels(LanguageCodeEnum.EN_US);
        }
#if DEBUG
        /// <summary>
        /// this is used so that the test bench can be used without needing to go through front-end login
        /// </summary>
        public void SetLoggedInUserForTestBench(User loggedInUser, IdiomaticaContext context)
        {
            _loggedInUser = loggedInUser;
            _uiLanguageCode = DataCache.LanguageCodeByCodeReadAsync(_loggedInUser.Code, context).Result;
            _uiLabels = UILabels.Factory.GetUILabels(_uiLanguageCode.LanguageCodeEnum);
            _loggedInUserOverride = true;
        }
#endif
        public User? GetLoggedInUser(IdiomaticaContext context)
        {
#if DEBUG
            /// this is used so that the test bench can be used without needing to go through front-end login
            if ( _loggedInUserOverride) return _loggedInUser;
#endif
            var t = Task.Run(() => GetLoggedInUserAsync(context));
            t.Wait();
            return t.Result;
        }
        public async Task<User?> GetLoggedInUserAsync(IdiomaticaContext context)
        {

#if DEBUG
            /// this is used so that the test bench can be used without needing to go through front-end login
            if (_loggedInUserOverride) return _loggedInUser;
#endif
            _loggedInUserClaimsPrincipal = await GetAppUserClaimsPrincipalAsync();
            return await processUserFromClaimPrincipal(context);
        }
        private async Task<User?> processUserFromClaimPrincipal(IdiomaticaContext context)
        {
            if (_loggedInUserClaimsPrincipal == null) return null;
            if (_loggedInUserClaimsPrincipal.Identity is null) return null;
            if (!_loggedInUserClaimsPrincipal.Identity.IsAuthenticated) return null;

            var appUserId = _loggedInUserClaimsPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (appUserId == null) return null;
            //Console.WriteLine($"appUserId = {appUserId}");
           
            var matchingUser = await DataCache.UserByApplicationUserIdReadAsync(appUserId, context);
            if (matchingUser != null)
            {
                _loggedInUser = matchingUser;
                _uiLanguageCode = await DataCache.LanguageCodeByCodeReadAsync(_loggedInUser.Code, context);
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
