using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Model.Enums;

namespace Model.DAL
{
    public static partial class DataCache
    {
        
        private static ConcurrentDictionary<string, User> UserByApplicationUserId = new ConcurrentDictionary<string, User>();
        private static ConcurrentDictionary<Guid, Language?> UserSettingUiLanguageByUserId = new();
        private static ConcurrentDictionary<Guid, List<UserSetting>?> UserSettingsByUserId = new();

        #region create
        public static User? UserCreate(User user, IdiomaticaContext context)
        {
            if (user.ApplicationUserId is null) throw new ArgumentNullException(nameof(user.ApplicationUserId));

            int numRows = context.Database.ExecuteSql($"""
                
                INSERT INTO [Idioma].[User]
                           ([Name]
                           ,[ApplicationUserId]
                           ,[UniqueKey])
                     VALUES
                           ({user.Name}
                           ,{user.ApplicationUserId}
                           ,{user.UniqueKey})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating User affected 0 rows");
            


            // add it to cache
            UserByApplicationUserId[user.ApplicationUserId] = user;

            return user;
        }
        public static async Task<User?> UserCreateAsync(User value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return UserCreate(value, context); });
        }
        #endregion


        #region read
        public static User? UserByApplicationUserIdRead(string key, IdiomaticaContext context)
        {
            // check cache
            if (UserByApplicationUserId.ContainsKey(key))
            {
                return UserByApplicationUserId[key];
            }
            // read DB
            var value = context.Users
                .Where(u => u.ApplicationUserId == key)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            UserByApplicationUserId[key] = value;
            return value;
        }
        public static List<UserSetting>? UserSettingsByUserIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (UserSettingsByUserId.ContainsKey(key))
            {
                return UserSettingsByUserId[key];
            }
            // read DB
            var value = (from u in context.Users
                         join us in context.UserSettings on u.UniqueKey equals us.UserKey
                         where u.UniqueKey == key
                         select us)
                .ToList();

            if (value == null) return null;
            // write to cache
            UserSettingsByUserId[key] = value;
            return value;
        }
        public static async Task<List<UserSetting>?> UserSettingsByUserIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<List<UserSetting>?>.Run(() =>
            {
                return UserSettingsByUserIdRead(key, context);
            });
        }
        public static Language? UserSettingUiLanguageByUserIdRead(
            Guid key, IdiomaticaContext context)
        {
            var settings = DataCache.UserSettingsByUserIdRead(key, context);
            if (settings == null) return null;
            var uiPref = settings
                .Where(x => x.Key == AvailableUserSetting.UILANGUAGE)
                .FirstOrDefault();
            if (uiPref == null) return null;
            if (!Guid.TryParse(uiPref.Value, out var languageKey)) return null;
            return DataCache.LanguageByIdRead(languageKey, context);
        }
        public static async Task<Language?> UserSettingUiLanguageByUserIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<Language?>.Run(() =>
            {
                return UserSettingUiLanguageByUserIdRead(key, context);
            });
        }
        #endregion

        #region update
        #endregion

        #region delete

        public static void UserAndAllChildrenDelete(Guid userId, IdiomaticaContext context)
        {
            context.Database.ExecuteSql($"""

                delete us
                from [Idioma].[User] u
                left join [Idioma].[UserSetting] us on u.UniqueKey = us.UserKey
                where u.UniqueKey = {userId};

                delete fcptb
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.UniqueKey = lu.UserKey
                left join [Idioma].[WordUser] wu on lu.UniqueKey = wu.LanguageUserKey
                left join [Idioma].[BookUser] bu on lu.UniqueKey = bu.LanguageUserKey
                left join [Idioma].[PageUser] pu on bu.UniqueKey = pu.BookUserKey
                left join [Idioma].[BookUserStat] bus on lu.UniqueKey = bus.LanguageUserKey
                left join [Idioma].[FlashCard] fc on wu.UniqueKey = fc.WordUserKey
                left join [Idioma].[FlashCardAttempt] fca on fc.UniqueKey = fca.FlashCardId
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on fc.UniqueKey = fcptb.FlashCardId
                where u.UniqueKey = {userId};

                delete fca
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.UniqueKey = lu.UserKey
                left join [Idioma].[WordUser] wu on lu.UniqueKey = wu.LanguageUserKey
                left join [Idioma].[BookUser] bu on lu.UniqueKey = bu.LanguageUserKey
                left join [Idioma].[PageUser] pu on bu.UniqueKey = pu.BookUserKey
                left join [Idioma].[BookUserStat] bus on lu.UniqueKey = bus.LanguageUserKey
                left join [Idioma].[FlashCard] fc on wu.UniqueKey = fc.WordUserKey
                left join [Idioma].[FlashCardAttempt] fca on fc.UniqueKey = fca.FlashCardId
                where u.UniqueKey = {userId};

                delete fc
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.UniqueKey = lu.UserKey
                left join [Idioma].[WordUser] wu on lu.UniqueKey = wu.LanguageUserKey
                left join [Idioma].[BookUser] bu on lu.UniqueKey = bu.LanguageUserKey
                left join [Idioma].[PageUser] pu on bu.UniqueKey = pu.BookUserKey
                left join [Idioma].[BookUserStat] bus on lu.UniqueKey = bus.LanguageUserKey
                left join [Idioma].[FlashCard] fc on wu.UniqueKey = fc.WordUserKey
                where u.UniqueKey = {userId};

                delete bus
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.UniqueKey = lu.UserKey
                left join [Idioma].[WordUser] wu on lu.UniqueKey = wu.LanguageUserKey
                left join [Idioma].[BookUser] bu on lu.UniqueKey = bu.LanguageUserKey
                left join [Idioma].[PageUser] pu on bu.UniqueKey = pu.BookUserKey
                left join [Idioma].[BookUserStat] bus on lu.UniqueKey = bus.LanguageUserKey
                where u.UniqueKey = {userId};

                delete pu
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.UniqueKey = lu.UserKey
                left join [Idioma].[WordUser] wu on lu.UniqueKey = wu.LanguageUserKey
                left join [Idioma].[BookUser] bu on lu.UniqueKey = bu.LanguageUserKey
                left join [Idioma].[PageUser] pu on bu.UniqueKey = pu.BookUserKey
                where u.UniqueKey = {userId};

                delete bu
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.UniqueKey = lu.UserKey
                left join [Idioma].[WordUser] wu on lu.UniqueKey = wu.LanguageUserKey
                left join [Idioma].[BookUser] bu on lu.UniqueKey = bu.LanguageUserKey
                where u.UniqueKey = {userId};

                delete wu
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.UniqueKey = lu.UserKey
                left join [Idioma].[WordUser] wu on lu.UniqueKey = wu.LanguageUserKey
                where u.UniqueKey = {userId};

                delete lu
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.UniqueKey = lu.UserKey
                where u.UniqueKey = {userId};

                delete u
                from [Idioma].[User] u
                where u.UniqueKey = {userId};
        
                """);



            // delete caches
            var listCachedUsers = UserByApplicationUserId.Where(x => x.Value.UniqueKey == userId).ToList();
            foreach (var cachedEntry in listCachedUsers)
                UserByApplicationUserId.Remove(cachedEntry.Key, out User? deletedValue);

            var listCachedLanguageUsers = LanguageUserById.Where(x => x.Value.UniqueKey == userId).ToList();
            foreach (var cachedEntry in listCachedLanguageUsers)
                LanguageUserById.Remove(cachedEntry.Key, out LanguageUser? deletedValue);

            var listCachedLanguageUsers2 = LanguageUserByLanguageIdAndUserId.Where(x => x.Value.UniqueKey == userId).ToList();
            foreach (var cachedEntry in listCachedLanguageUsers2)
                LanguageUserByLanguageIdAndUserId.Remove(cachedEntry.Key, out LanguageUser? deletedValue);

        }

        #endregion
    }
}
