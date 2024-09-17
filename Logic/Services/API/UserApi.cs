﻿using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepL;
using Model.Enums;

namespace Logic.Services.API
{
    public static class UserApi
    {
        #region create

        public static User? UserCreate(
            string applicationUserId, string name, IdiomaticaContext context)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = applicationUserId,
                Name = name,
            };
            user = DataCache.UserCreate(user, context);
            if (user is null) { ErrorHandler.LogAndThrow(); return null; }

            return user;
        }
        public static async Task<User?> UserCreateAsync(
            string applicationUserId, string name, IdiomaticaContext context)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = applicationUserId,
                Name = name,
            };
            user = await DataCache.UserCreateAsync(user, context);
            return user;
        }


        public static UserBreadCrumb? UserBreadCrumbCreate(IdiomaticaContext context,
            User user, Page page)
        {
            UserBreadCrumb crumb = new() {
                Id = Guid.NewGuid(),
                UserId = user.Id, 
                User = user,
                PageId = page.Id,
                Page = page,
                ActionDateTime = DateTime.Now
            };
            return DataCache.UserBreadCrumbCreate(crumb, context);
        }
        public static async Task<UserBreadCrumb?> UserBreadCrumbCreateAsync(
            IdiomaticaContext context, User user, Page page)
        {
            return await Task.Run(() =>
            {
                return UserBreadCrumbCreate(context, user, page);
            });
        }


        public static UserSetting? UserSettingCreate(IdiomaticaContext context,
            AvailableUserSetting key, Guid userId, string value)
        {
            return DataCache.UserSettingCreate(key, userId, value, context);
        }
        public static async Task<UserSetting?> UserSettingCreateAsync(
            IdiomaticaContext context, AvailableUserSetting key, Guid userId, string value)
        {
            return await Task.Run(() =>
            {
                return UserSettingCreate(context, key, userId, value);
            });
        }

        #endregion


        #region read

        public static UserBreadCrumb? UserBreadCrumbReadLatest(IdiomaticaContext context, Guid userId)
        {
            var list = DataCache.UserBreadCrumbReadByFilter((x => x.UserId == userId), 1, context);
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

        public static Language? UserSettingUiLanguagReadByUserId(
            IdiomaticaContext context, Guid userId)
        {
            return DataCache.UserSettingUiLanguageByUserIdRead(userId, context);
        }
        public static async Task<Language?> UserSettingUiLanguagReadByUserIdAsync(
            IdiomaticaContext context, Guid userId)
        {
            return await DataCache.UserSettingUiLanguageByUserIdReadAsync(userId, context);
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
