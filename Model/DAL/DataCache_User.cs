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

namespace Model.DAL
{
    public static partial class DataCache
    {
        
        private static ConcurrentDictionary<string, User> UserByApplicationUserId = new ConcurrentDictionary<string, User>();

        public static User? UserCreate(User user, IdiomaticaContext context)
        {
            if (user.ApplicationUserId is null) throw new ArgumentNullException(nameof(user.ApplicationUserId));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                
                INSERT INTO [Idioma].[User]
                           ([Name]
                           ,[ApplicationUserId]
                           ,[LanguageCode]
                           ,[UniqueKey])
                     VALUES
                           ({user.Name}
                           ,{user.ApplicationUserId}
                           ,{user.Code} -- remember that C# Code maps to SQL LanguageCode
                           ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating User affected 0 rows");
            var newEntity = context.Users.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.UniqueKey is null)
            {
                throw new InvalidDataException("newEntity is null in UserCreate");
            }
            if (string.IsNullOrEmpty(newEntity.ApplicationUserId))
            {
                throw new InvalidDataException("ApplicationUserId is empty in UserCreate");
            }


            // add it to cache
            UserByApplicationUserId[newEntity.ApplicationUserId] = newEntity;

            return newEntity;
        }
        public static async Task<User?> UserCreateAsync(User value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return UserCreate(value, context); });
        }

        
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
    }
}
