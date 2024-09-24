using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepL;
using Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class UserApi
    {
        #region create

        public static User? UserCreate(
            string applicationUserId, string name, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = applicationUserId,
                Name = name,
            };
            user = DataCache.UserCreate(user, dbContextFactory);
            if (user is null) { ErrorHandler.LogAndThrow(); return null; }

            return user;
        }
        public static async Task<User?> UserCreateAsync(
            string applicationUserId, string name, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = applicationUserId,
                Name = name,
            };
            user = await DataCache.UserCreateAsync(user, dbContextFactory);
            return user;
        }


        public static UserBreadCrumb? UserBreadCrumbCreate(IDbContextFactory<IdiomaticaContext> dbContextFactory,
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
            return DataCache.UserBreadCrumbCreate(crumb, dbContextFactory);
        }
        public static async Task<UserBreadCrumb?> UserBreadCrumbCreateAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, User user, Page page)
        {
            return await Task.Run(() =>
            {
                return UserBreadCrumbCreate(dbContextFactory, user, page);
            });
        }


        public static UserSetting? UserSettingCreate(IDbContextFactory<IdiomaticaContext> dbContextFactory,
            AvailableUserSetting key, Guid userId, string value)
        {
            return DataCache.UserSettingCreate(key, userId, value, dbContextFactory);
        }
        public static async Task<UserSetting?> UserSettingCreateAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, AvailableUserSetting key, Guid userId, string value)
        {
            return await Task.Run(() =>
            {
                return UserSettingCreate(dbContextFactory, key, userId, value);
            });
        }

        #endregion


        #region read

        public static UserBreadCrumb? UserBreadCrumbReadLatest(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            var list = DataCache.UserBreadCrumbReadByFilter((x => x.UserId == userId), 1, dbContextFactory);
            if(list.Count < 1) return null;
            return list[0];
        }
        public static async Task<UserBreadCrumb?> UserBreadCrumbReadLatestAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            return await Task.Run(() =>
            {
                return UserBreadCrumbReadLatest(dbContextFactory, userId);
            });
        }

        public static Language? UserSettingUiLanguagReadByUserId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            return DataCache.UserSettingUiLanguageByUserIdRead(userId, dbContextFactory);
        }
        public static async Task<Language?> UserSettingUiLanguagReadByUserIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            return await DataCache.UserSettingUiLanguageByUserIdReadAsync(userId, dbContextFactory);
        }

        public static Language? UserSettingLearningLanguagReadByUserId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            return DataCache.UserSettingLearningLanguageByUserIdRead(userId, dbContextFactory);
        }
        public static async Task<Language?> UserSettingLearningLanguagReadByUserIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            return await DataCache.UserSettingLearningLanguageByUserIdReadAsync(userId, dbContextFactory);
        }

        #endregion

        #region delete

        public static void UserAndAllChildrenDelete(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            DataCache.UserAndAllChildrenDelete(userId, dbContextFactory);
        }
        public static async Task UserAndAllChildrenDeleteAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            await Task.Run(() =>
            {
                UserAndAllChildrenDelete(dbContextFactory, userId);
            });
        }

        #endregion
    }
}
