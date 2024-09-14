using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;

namespace Logic.Services.API
{
    public static class LanguageUserApi
    {
        #region create
        public static LanguageUser? LanguageUserCreate(
            IdiomaticaContext context, Language language, User user)
        {
            var languageUser = new LanguageUser()
            {
                UniqueKey = Guid.NewGuid(),
                LanguageKey = language.UniqueKey,
                Language = language,
                UserKey = user.UniqueKey,
                User = user,
                TotalWordsRead = 0
            };
            languageUser = DataCache.LanguageUserCreate(languageUser, context);
            if (languageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return languageUser;
        }
        public static async Task<LanguageUser?> LanguageUserCreateAsync(
            IdiomaticaContext context, Language language, User user)
        {
            return await Task<LanguageUser?>.Run(() => { 
                return LanguageUserCreate(context, language, user); });
        }


        #endregion
        public static LanguageUser? LanguageUserGet(
            IdiomaticaContext context, Guid languageKey, Guid userKey)
        {
            return DataCache.LanguageUserByLanguageIdAndUserIdRead((languageKey, userKey), context);
        }
        public static async Task<LanguageUser?> LanguageUserGetAsync(
            IdiomaticaContext context, Guid languageKey, Guid userKey)
        {
            return await DataCache.LanguageUserByLanguageIdAndUserIdReadAsync((languageKey, userKey), context);
        }


        public static List<LanguageUser>? LanguageUsersAndLanguageGetByUserId(
            IdiomaticaContext context, Guid userKey)
        {
            return DataCache.LanguageUsersAndLanguageByUserIdRead((Guid)userKey, context);
        }
        public static async Task<List<LanguageUser>?> LanguageUsersAndLanguageGetByUserIdAsync(
            IdiomaticaContext context, Guid userKey)
        {
            return await DataCache.LanguageUsersAndLanguageByUserIdReadAsync((Guid)userKey, context);
        }
    }
}
