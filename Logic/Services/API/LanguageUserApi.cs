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
