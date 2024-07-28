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
    }
}
