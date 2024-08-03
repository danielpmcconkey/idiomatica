using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.API
{
    public static class UserApi
    {
        #region create
        public static User? UserCreate(string applicationUserId, string name,
            string uiLanguageCode, IdiomaticaContext context)
        {
            var user = new User()
            {
                ApplicationUserId = applicationUserId,
                Name = name,
                Code = uiLanguageCode,
            };
            user = DataCache.UserCreate(user, context);
            if (user is null || user.Id is null) { ErrorHandler.LogAndThrow(); return null; }

            return user;
        }
        public static async Task<User?> UserCreateAsync(string applicationUserId, string name,
            string uiLanguageCode, IdiomaticaContext context)
        {
            var user = new User()
            {
                ApplicationUserId = applicationUserId,
                Name = name,
                Code = uiLanguageCode,
            };
            user = await DataCache.UserCreateAsync(user, context);
            if (user is null || user.Id is null) { ErrorHandler.LogAndThrow(); return null; }

            return user;
        }
        #endregion
        public static void UserAndAllChildrenDelete(IdiomaticaContext context, int userId)
        {
            if (userId < 1) ErrorHandler.LogAndThrow();
            DataCache.UserAndAllChildrenDelete(userId, context);
        }
        public static async Task UserAndAllChildrenDeleteAsync(IdiomaticaContext context, int userId)
        {
            await Task.Run(() =>
            {
                UserAndAllChildrenDelete(context, userId);
            });
        }
    }
}
