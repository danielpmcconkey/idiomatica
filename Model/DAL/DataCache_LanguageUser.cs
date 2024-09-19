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
        private static ConcurrentDictionary<Guid, LanguageUser> LanguageUserById = new ConcurrentDictionary<Guid, LanguageUser>();
        private static ConcurrentDictionary<(Guid languageId, Guid userId), LanguageUser> LanguageUserByLanguageIdAndUserId = new ConcurrentDictionary<(Guid languageId, Guid userId), LanguageUser>();
        private static ConcurrentDictionary<Guid, List<LanguageUser>> LanguageUsersAndLanguageByUserId = new ConcurrentDictionary<Guid, List<LanguageUser>>();

        #region create
        public static LanguageUser? LanguageUserCreate(LanguageUser languageUser, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            //int numRows = context.Database.ExecuteSql($"""
                                
            //    INSERT INTO [Idioma].[LanguageUser]
            //               ([LanguageId]
            //               ,[UserId]
            //               ,[TotalWordsRead]
            //               ,[Id])
            //         VALUES
            //               ({languageUser.LanguageId}
            //               ,{languageUser.UserId}
            //               ,{languageUser.TotalWordsRead}
            //               ,{guid})
                
            //    """);
            //if (numRows < 1) throw new InvalidDataException("creating LanguageUser affected 0 rows");
            //var newEntity = context.LanguageUsers.Where(x => x.Id == guid).FirstOrDefault();
            //if (newEntity is null)
            //{
            //    throw new InvalidDataException("newEntity is null in LanguageUserCreate");
            //}
            context.LanguageUsers.Add(languageUser);
            context.SaveChanges();
            

            // add it to cache
            LanguageUserUpdateCache(languageUser, dbContextFactory);

            return languageUser;
        }
        public static async Task<LanguageUser?> LanguageUserCreateAsync(LanguageUser value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return LanguageUserCreate(value, dbContextFactory); });
        }
        #endregion

        #region read
        public static LanguageUser? LanguageUserByIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (LanguageUserById.ContainsKey(key))
            {
                return LanguageUserById[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.LanguageUsers
                .Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            LanguageUserById[key] = value;
            return value;
        }
        public static async Task<LanguageUser?> LanguageUserByIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<LanguageUser?>.Run(() =>
            {
                return LanguageUserByIdRead(key, dbContextFactory);
            });
        }
        public static LanguageUser? LanguageUserByLanguageIdAndUserIdRead(
            (Guid languageId, Guid userId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (LanguageUserByLanguageIdAndUserId.ContainsKey(key))
            {
                return LanguageUserByLanguageIdAndUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.LanguageUsers
                .Where(x => x.LanguageId == key.languageId && x.UserId == key.userId)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            LanguageUserByLanguageIdAndUserId[key] = value;
            return value;
        }
        public static async Task<LanguageUser?> LanguageUserByLanguageIdAndUserIdReadAsync(
            (Guid languageId, Guid userId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<LanguageUser?>.Run(() =>
            {
                return LanguageUserByLanguageIdAndUserIdRead(key, dbContextFactory);
            });
        }        
        public static List<LanguageUser>? LanguageUsersAndLanguageByUserIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (LanguageUsersAndLanguageByUserId.ContainsKey(key))
            {
                return LanguageUsersAndLanguageByUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();


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
                LanguageUserUpdateCache(v, dbContextFactory);
            }
            return value;
        }
        public static async Task<List<LanguageUser>?> LanguageUsersAndLanguageByUserIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<LanguageUser>?>.Run(() =>
            {
                return LanguageUsersAndLanguageByUserIdRead(key, dbContextFactory);
            });
        }
        #endregion


        private static void LanguageUserUpdateCache(LanguageUser languageUser, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            Guid id = languageUser.Id;
            Guid userId = languageUser.UserId;
            Guid languageId = languageUser.LanguageId;
            LanguageUserById[id] = languageUser;
            LanguageUserByLanguageIdAndUserId[(languageId, userId)] = languageUser;
            var readLanguage = DataCache.LanguageByIdRead(languageId, dbContextFactory);
            if (readLanguage == null) return;
            languageUser.Language = readLanguage;
            if (LanguageUsersAndLanguageByUserId.ContainsKey(userId))
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
