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
            if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return null; }

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
            if (user is null || user.UniqueKey is null) { ErrorHandler.LogAndThrow(); return null; }

            return user;
        }


        public static UserBreadCrumb? UserBreadCrumbCreate(IdiomaticaContext context, Guid userId, Guid pageId)
        {
            UserBreadCrumb? crumb = new() {
                UserKey = userId,
                PageKey = pageId,
                ActionDateTime = DateTime.Now
            };
            crumb = DataCache.UserBreadCrumbCreate(crumb, context);
            if (crumb is null || crumb.UniqueKey is null) { ErrorHandler.LogAndThrow(); return null; }
            return crumb;
        }
        public static async Task<UserBreadCrumb?> UserBreadCrumbCreateAsync(
            IdiomaticaContext context, Guid userId, Guid pageId)
        {
            return await Task.Run(() =>
            {
                return UserBreadCrumbCreate(context, userId, pageId);
            });
        }

        #endregion


        #region read

        public static UserBreadCrumb? UserBreadCrumbReadLatest(IdiomaticaContext context, Guid userId)
        {
            var list = DataCache.UserBreadCrumbReadByFilter((x => x.UserKey == userId), 1, context);
            if(list.Count < 1) return null;
            return list[0];
        }
        public static async Task<UserBreadCrumb?> UserBreadCrumbReadLatestAsync(
            IdiomaticaContext context, Guid userId)
        {
            return await Task.Run(() =>
            {
                return UserBreadCrumbReadLatest(context, userId);
            });
        }

        #endregion

        #region delete

        public static void UserAndAllChildrenDelete(IdiomaticaContext context, Guid userId)
        {
            DataCache.UserAndAllChildrenDelete(userId, context);
        }
        public static async Task UserAndAllChildrenDeleteAsync(IdiomaticaContext context, Guid userId)
        {
            await Task.Run(() =>
            {
                UserAndAllChildrenDelete(context, userId);
            });
        }

        #endregion
    }
}
