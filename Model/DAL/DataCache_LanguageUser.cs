using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<int, LanguageUser> LanguageUserById = new ConcurrentDictionary<int, LanguageUser>();
        private static ConcurrentDictionary<(int languageId, int userId), LanguageUser> LanguageUserByLanguageIdAndUserId = new ConcurrentDictionary<(int languageId, int userId), LanguageUser>();
        private static ConcurrentDictionary<int, List<LanguageUser>> LanguageUsersAndLanguageByUserId = new ConcurrentDictionary<int, List<LanguageUser>>();

        #region create
        public static LanguageUser? LanguageUserCreate(LanguageUser languageUser, IdiomaticaContext context)
        {
            if (languageUser.LanguageId is null) throw new ArgumentNullException(nameof(languageUser.LanguageId));
            if (languageUser.UserId is null) throw new ArgumentNullException(nameof(languageUser.UserId));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                                
                INSERT INTO [Idioma].[LanguageUser]
                           ([LanguageId]
                           ,[UserId]
                           ,[TotalWordsRead]
                           ,[UniqueKey])
                     VALUES
                           ({languageUser.LanguageId}
                           ,{languageUser.UserId}
                           ,{languageUser.TotalWordsRead}
                           ,{guid})
                
                """);
            if (numRows < 1) throw new InvalidDataException("creating LanguageUser affected 0 rows");
            var newEntity = context.LanguageUsers.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in LanguageUserCreate");
            }
            

            // add it to cache
            LanguageUserUpdateCache(newEntity, context);

            return newEntity;
        }
        public static async Task<LanguageUser?> LanguageUserCreateAsync(LanguageUser value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return LanguageUserCreate(value, context); });
        }
        #endregion

        #region read
        public static LanguageUser? LanguageUserByIdRead(
            int key, IdiomaticaContext context)
        {
            return LanguageUserByIdReadAsync(key, context).Result;
        }
        public static async Task<LanguageUser?> LanguageUserByIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageUserById.ContainsKey(key))
            {
                return LanguageUserById[key];
            }

            // read DB
            var value = context.LanguageUsers
                .Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            LanguageUserById[key] = value;
            return value;
        }
        public static LanguageUser? LanguageUserByLanguageIdAndUserIdRead(
            (int languageId, int userId) key, IdiomaticaContext context)
        {
            var task = LanguageUserByLanguageIdAndUserIdReadAsync(key, context);
            return task.Result;
        }
        public static async Task<LanguageUser?> LanguageUserByLanguageIdAndUserIdReadAsync(
            (int languageId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageUserByLanguageIdAndUserId.ContainsKey(key))
            {
                return LanguageUserByLanguageIdAndUserId[key];
            }

            // read DB
            var value = await context.LanguageUsers
                .Where(x => x.LanguageId == key.languageId && x.UserId == key.userId)
                .FirstOrDefaultAsync();
            if (value == null) return null;
            // write to cache
            LanguageUserByLanguageIdAndUserId[key] = value;
            return value;
        }        
        public static List<LanguageUser>? LanguageUsersAndLanguageByUserIdRead(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageUsersAndLanguageByUserId.ContainsKey(key))
            {
                return LanguageUsersAndLanguageByUserId[key];
            }

            // read DB
            var value = context.LanguageUsers
                .Where(x => x.UserId == key && x.Language != null)
                .Include(lu => lu.Language)
                .OrderBy(x => x.Language)
                .ToList();
            if (value == null) return null;
            // write to cache
            LanguageUsersAndLanguageByUserId[key] = value;
            foreach (var v in value)
            {
                LanguageUserUpdateCache(v, context);
            }
            return value;
        }
        public static async Task<List<LanguageUser>?> LanguageUsersAndLanguageByUserIdReadAsync(
            int key, IdiomaticaContext context)
        {
            return await Task<List<LanguageUser>?>.Run(() =>
            {
                return LanguageUsersAndLanguageByUserIdRead(key, context);
            });
        }
        #endregion


        private static void LanguageUserUpdateCache(LanguageUser languageUser, IdiomaticaContext context)
        {
            if (languageUser is null || languageUser.Id is null ||
                languageUser.UserId is null || languageUser.LanguageId is null ||
                languageUser.LanguageId < 1 || languageUser.UserId < 1)
            {
                return;
            }
            int id = (int)languageUser.Id;
            int userId = (int)languageUser.UserId;
            int languageId = (int)languageUser.LanguageId;
            LanguageUserById[id] = languageUser;
            LanguageUserByLanguageIdAndUserId[(languageId, userId)] = languageUser;
            languageUser.Language = DataCache.LanguageByIdRead(languageId, context);
            if(LanguageUsersAndLanguageByUserId.ContainsKey(userId))
            {
                var list = LanguageUsersAndLanguageByUserId[userId];
                // grab the entries in the list that are NOT this language
                var newList = list.Where(x => x.LanguageId != languageId).ToList();
                newList.Add(languageUser);
                LanguageUsersAndLanguageByUserId[userId] = newList;
            }
        }
    }
}
