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
                Id = Guid.NewGuid(),
                LanguageId = language.Id,
                Language = language,
                UserId = user.Id,
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
            IdiomaticaContext context, Guid languageId, Guid userId)
        {
            return DataCache.LanguageUserByLanguageIdAndUserIdRead((languageId, userId), context);
        }
        public static async Task<LanguageUser?> LanguageUserGetAsync(
            IdiomaticaContext context, Guid languageId, Guid userId)
        {
            return await DataCache.LanguageUserByLanguageIdAndUserIdReadAsync((languageId, userId), context);
        }


        public static List<LanguageUser>? LanguageUsersAndLanguageGetByUserId(
            IdiomaticaContext context, Guid userId)
        {
            return DataCache.LanguageUsersAndLanguageByUserIdRead((Guid)userId, context);
        }
        public static async Task<List<LanguageUser>?> LanguageUsersAndLanguageGetByUserIdAsync(
            IdiomaticaContext context, Guid userId)
        {
            return await DataCache.LanguageUsersAndLanguageByUserIdReadAsync((Guid)userId, context);
        }
    }
}
