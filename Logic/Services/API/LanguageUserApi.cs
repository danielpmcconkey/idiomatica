using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class LanguageUserApi
    {
        #region create
        public static LanguageUser? LanguageUserCreate(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Language language, User user)
        {
            var languageUser = new LanguageUser()
            {
                Id = Guid.NewGuid(),
                LanguageId = language.Id,
                //Language = language,
                UserId = user.Id,
                //User = user,
                TotalWordsRead = 0
            };
            languageUser = DataCache.LanguageUserCreate(languageUser, dbContextFactory);
            if (languageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return languageUser;
        }
        public static async Task<LanguageUser?> LanguageUserCreateAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Language language, User user)
        {
            return await Task<LanguageUser?>.Run(() => { 
                return LanguageUserCreate(dbContextFactory, language, user); });
        }


        #endregion
        public static LanguageUser? LanguageUserGet(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageId, Guid userId)
        {
            return DataCache.LanguageUserByLanguageIdAndUserIdRead((languageId, userId), dbContextFactory);
        }
        public static async Task<LanguageUser?> LanguageUserGetAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageId, Guid userId)
        {
            return await DataCache.LanguageUserByLanguageIdAndUserIdReadAsync((languageId, userId), dbContextFactory);
        }


        public static List<LanguageUser>? LanguageUsersAndLanguageGetByUserId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            return DataCache.LanguageUsersAndLanguageByUserIdRead((Guid)userId, dbContextFactory);
        }
        public static async Task<List<LanguageUser>?> LanguageUsersAndLanguageGetByUserIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            return await DataCache.LanguageUsersAndLanguageByUserIdReadAsync((Guid)userId, dbContextFactory);
        }
    }
}
