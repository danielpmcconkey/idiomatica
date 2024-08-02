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
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
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

        public static void UserAndAllChildrenDelete(int userId, IdiomaticaContext context)
        {
            if (userId < 1) throw new ArgumentException(nameof(userId));

            Guid guid = Guid.NewGuid();
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
    }
}
