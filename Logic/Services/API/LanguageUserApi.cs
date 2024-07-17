using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using k8s.KubeConfigModels;

namespace Logic.Services.API
{
    public static class LanguageUserApi
    {
        #region create
        public static async Task<LanguageUser?> LanguageUserCreateAsync(
            IdiomaticaContext context, int languageId, int userId)
        {
            if (languageId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            var languageUser = new LanguageUser()
            {
                LanguageId = languageId,
                UserId = userId,
                TotalWordsRead = 0
            };
            var isSaved = await DataCache.LanguageUserCreateAsync(languageUser, context);
            if (!isSaved || languageUser.Id is null || languageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return languageUser;
        }
        #endregion
        public static async Task<LanguageUser?> LanguageUserGetAsync(
            IdiomaticaContext context, int languageId, int userId)
        {
            if (languageId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.LanguageUserByLanguageIdAndUserIdReadAsync((languageId, userId), context);
        }
        public static async Task<List<LanguageUser>> LanguageUsersAndLanguageGetByUserIdAsync(
            IdiomaticaContext context, int userId)
        {
            if (userId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.LanguageUsersAndLanguageByUserIdReadAsync((int)userId, context);
        }
    }
}
