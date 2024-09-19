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
        public static User? UserCreate(User user, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            if (user.ApplicationUserId is null) throw new ArgumentNullException(nameof(user.ApplicationUserId));
            var context = dbContextFactory.CreateDbContext();

            //int numRows = context.Database.ExecuteSql($"""

            //    INSERT INTO [Idioma].[User]
            //               ([Name]
            //               ,[ApplicationUserId]
            //               ,[Id])
            //         VALUES
            //               ({user.Name}
            //               ,{user.ApplicationUserId}
            //               ,{user.Id})

            //    """);
            //if (numRows < 1) throw new InvalidDataException("creating User affected 0 rows");

            context.Users.Add(user);
            context.SaveChanges();
            


            // add it to cache
            UserByApplicationUserId[user.ApplicationUserId] = user;

            return user;
        }
        public static async Task<User?> UserCreateAsync(User value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return UserCreate(value, dbContextFactory); });
        }


        public static UserSetting? UserSettingCreate(
            AvailableUserSetting key, Guid userId, string value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            UserSetting userSettingUiLang = new()
            {
                Key = key,
                UserId = userId,
                Value = value
            };
            context.Add(userSettingUiLang);
            var saved = false;
            var retries = 0;
            while (!saved && retries < 3)
            {
                try
                {
                    context.SaveChanges();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    retries++;
                    foreach (var entry in ex.Entries)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // TODO: decide which value should be written to database
                            // proposedValues[property] = <value to be saved>;
                        }
                    }
                }
            }
            

            return userSettingUiLang;
        }
        public static async Task<UserSetting?> UserSettingCreateAsync(
            AvailableUserSetting key, Guid userId, string value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return UserSettingCreate(key, userId, value, dbContextFactory); });
        }
        #endregion


        #region read
        public static User? UserByApplicationUserIdRead(string key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (UserByApplicationUserId.ContainsKey(key))
            {
                return UserByApplicationUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();

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
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (UserSettingsByUserId.ContainsKey(key))
            {
                return UserSettingsByUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = (from u in context.Users
                         join us in context.UserSettings on u.Id equals us.UserId
                         where u.Id == key
                         select us)
                .ToList();

            if (value == null) return null;
            // write to cache
            UserSettingsByUserId[key] = value;
            return value;
        }
        public static async Task<List<UserSetting>?> UserSettingsByUserIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<UserSetting>?>.Run(() =>
            {
                return UserSettingsByUserIdRead(key, dbContextFactory);
            });
        }
        public static Language? UserSettingUiLanguageByUserIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var settings = DataCache.UserSettingsByUserIdRead(key, dbContextFactory);
            if (settings == null) return null;
            var uiPref = settings
                .Where(x => x.Key == AvailableUserSetting.UILANGUAGE)
                .FirstOrDefault();
            if (uiPref == null) return null;
            if (!Guid.TryParse(uiPref.Value, out var languageKey)) return null;
            return DataCache.LanguageByIdRead(languageKey, dbContextFactory);
        }
        public static async Task<Language?> UserSettingUiLanguageByUserIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Language?>.Run(() =>
            {
                return UserSettingUiLanguageByUserIdRead(key, dbContextFactory);
            });
        }
        #endregion

        #region update
        #endregion

        #region delete

        public static void UserAndAllChildrenDelete(Guid userId, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            context.Database.ExecuteSql($"""

                delete us
                from [Idioma].[User] u
                left join [Idioma].[UserSetting] us on u.Id = us.UserId
                where u.Id = {userId};

                delete fcptb
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.Id = lu.UserId
                left join [Idioma].[WordUser] wu on lu.Id = wu.LanguageUserId
                left join [Idioma].[BookUser] bu on lu.Id = bu.LanguageUserId
                left join [Idioma].[PageUser] pu on bu.Id = pu.BookUserId
                left join [Idioma].[BookUserStat] bus on lu.Id = bus.LanguageUserId
                left join [Idioma].[FlashCard] fc on wu.Id = fc.WordUserId
                left join [Idioma].[FlashCardAttempt] fca on fc.Id = fca.FlashCardId
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on fc.Id = fcptb.FlashCardId
                where u.Id = {userId};

                delete fca
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.Id = lu.UserId
                left join [Idioma].[WordUser] wu on lu.Id = wu.LanguageUserId
                left join [Idioma].[BookUser] bu on lu.Id = bu.LanguageUserId
                left join [Idioma].[PageUser] pu on bu.Id = pu.BookUserId
                left join [Idioma].[BookUserStat] bus on lu.Id = bus.LanguageUserId
                left join [Idioma].[FlashCard] fc on wu.Id = fc.WordUserId
                left join [Idioma].[FlashCardAttempt] fca on fc.Id = fca.FlashCardId
                where u.Id = {userId};

                delete fc
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.Id = lu.UserId
                left join [Idioma].[WordUser] wu on lu.Id = wu.LanguageUserId
                left join [Idioma].[BookUser] bu on lu.Id = bu.LanguageUserId
                left join [Idioma].[PageUser] pu on bu.Id = pu.BookUserId
                left join [Idioma].[BookUserStat] bus on lu.Id = bus.LanguageUserId
                left join [Idioma].[FlashCard] fc on wu.Id = fc.WordUserId
                where u.Id = {userId};

                delete bus
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.Id = lu.UserId
                left join [Idioma].[WordUser] wu on lu.Id = wu.LanguageUserId
                left join [Idioma].[BookUser] bu on lu.Id = bu.LanguageUserId
                left join [Idioma].[PageUser] pu on bu.Id = pu.BookUserId
                left join [Idioma].[BookUserStat] bus on lu.Id = bus.LanguageUserId
                where u.Id = {userId};

                delete pu
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.Id = lu.UserId
                left join [Idioma].[WordUser] wu on lu.Id = wu.LanguageUserId
                left join [Idioma].[BookUser] bu on lu.Id = bu.LanguageUserId
                left join [Idioma].[PageUser] pu on bu.Id = pu.BookUserId
                where u.Id = {userId};

                delete bu
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.Id = lu.UserId
                left join [Idioma].[WordUser] wu on lu.Id = wu.LanguageUserId
                left join [Idioma].[BookUser] bu on lu.Id = bu.LanguageUserId
                where u.Id = {userId};

                delete wu
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.Id = lu.UserId
                left join [Idioma].[WordUser] wu on lu.Id = wu.LanguageUserId
                where u.Id = {userId};

                delete lu
                from [Idioma].[User] u
                left join [Idioma].[LanguageUser] lu on u.Id = lu.UserId
                where u.Id = {userId};

                delete u
                from [Idioma].[User] u
                where u.Id = {userId};
        
                """);



            // delete caches
            var listCachedUsers = UserByApplicationUserId.Where(x => x.Value.Id == userId).ToList();
            foreach (var cachedEntry in listCachedUsers)
                UserByApplicationUserId.Remove(cachedEntry.Key, out User? deletedValue);

            var listCachedLanguageUsers = LanguageUserById.Where(x => x.Value.Id == userId).ToList();
            foreach (var cachedEntry in listCachedLanguageUsers)
                LanguageUserById.Remove(cachedEntry.Key, out LanguageUser? deletedValue);

            var listCachedLanguageUsers2 = LanguageUserByLanguageIdAndUserId.Where(x => x.Value.Id == userId).ToList();
            foreach (var cachedEntry in listCachedLanguageUsers2)
                LanguageUserByLanguageIdAndUserId.Remove(cachedEntry.Key, out LanguageUser? deletedValue);

        }

        #endregion
    }
}
