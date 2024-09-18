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
using Logic.Telemetry;
using Logic.Services.API;
using Model.Enums;

namespace Logic.Services
{
    public class LoginService
    {
        private ClaimsPrincipal? _loggedInUserClaimsPrincipal;
        private User? _loggedInUser;
        private UILabels.UILabels _uiLabels;
        private Language? _uiLanguage;
#if DEBUG
        /// <summary>
        /// this is used so that the test bench can be used without needing to go through front-end login
        /// </summary>
        private bool _loggedInUserOverride = false;
#endif


        private AuthenticationStateProvider _authenticationStateProvider;
        public LoginService(
            AuthenticationStateProvider AuthenticationStateProvider)
        {
            _authenticationStateProvider = AuthenticationStateProvider;
            _uiLabels = UILabels.Factory.GetUILabels(AvailableLanguageCode.EN_US);
        }
#if DEBUG
        /// <summary>
        /// this is used so that the test bench can be used without needing to go through front-end login
        /// </summary>
        public void SetLoggedInUserForTestBench(User loggedInUser, IdiomaticaContext context)
        {
            if (loggedInUser is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            _loggedInUser = loggedInUser;
            _uiLanguage = UserApi.UserSettingUiLanguagReadByUserId(context, loggedInUser.Id);
            if (_uiLanguage is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            _uiLabels = UILabels.Factory.GetUILabels(_uiLanguage.Code);
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
            return ProcessUserFromClaimPrincipal(context);
        }
        private User? ProcessUserFromClaimPrincipal(IdiomaticaContext context)
        {
            if (_loggedInUserClaimsPrincipal == null) return null;
            if (_loggedInUserClaimsPrincipal.Identity is null) return null;
            if (!_loggedInUserClaimsPrincipal.Identity.IsAuthenticated) return null;

            var appUserId = _loggedInUserClaimsPrincipal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (appUserId == null) return null;
           
            var matchingUser = DataCache.UserByApplicationUserIdRead(appUserId, context);
            if (matchingUser is null) return null;

            _loggedInUser = matchingUser;
            _uiLanguage = UserApi.UserSettingUiLanguagReadByUserId(context, matchingUser.Id);
            if (_uiLanguage == null) _uiLanguage = GetDefaultUiLanguage(context);
            if (_uiLanguage == null) { ErrorHandler.LogAndThrow(); return null; }
                    
            _uiLabels = UILabels.Factory.GetUILabels(_uiLanguage.Code);
            return _loggedInUser;
        }

        private Language? GetDefaultUiLanguage(IdiomaticaContext context)
        {
            return LanguageApi.LanguageReadByCode(context, AvailableLanguageCode.EN_US);
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
        public Language? GetUiLanguageCode()
        {
            return _uiLanguage;
        }
        private async Task<ClaimsPrincipal?> GetAppUserClaimsPrincipalAsync()
        {
            var authState = await _authenticationStateProvider
               .GetAuthenticationStateAsync();
            return authState.User;
        }
    }
}
