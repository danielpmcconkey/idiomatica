using Microsoft.EntityFrameworkCore;
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
        private static ConcurrentDictionary<Guid, PageUser> PageUserById = [];
        private static ConcurrentDictionary<(Guid pageId, Guid languageUserId), PageUser> PageUserByPageIdAndLanguageUserId = [];
        private static ConcurrentDictionary<(Guid languageUserId, int ordinal, Guid bookId), PageUser> PageUserByLanguageUserIdOrdinalAndBookId = []; 
        private static ConcurrentDictionary<Guid, List<PageUser>> PageUsersByBookUserId = [];

        #region create


        public static PageUser? PageUserCreate(PageUser pageUser, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[PageUser]
                      ([BookUserId]
                      ,[PageId]
                      ,[ReadDate]
                      ,[Id])
                VALUES
                      ({pageUser.BookUserId}
                      ,{pageUser.PageId}
                      ,{pageUser.ReadDate}
                      ,{pageUser.Id})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating PageUser affected 0 rows");
            
            // add it to cache
            PageUserUpdateAllCaches(pageUser);

            return pageUser;
        }
        public static async Task<PageUser?> PageUserCreateAsync(PageUser value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return PageUserCreate(value, dbContextFactory); });
        }


        
        #endregion

        #region read
        public static PageUser? PageUserByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (PageUserById.ContainsKey(key))
            {
                return PageUserById[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.PageUsers.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // now update the cache
            PageUserUpdateAllCaches(value);
            return value;
        }
        public static async Task<PageUser?> PageUserByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserByIdRead(key, dbContextFactory);
            });
        }
        public static PageUser? PageUserByLanguageUserIdOrdinalAndBookIdRead(
            (Guid languageUserId, int ordinal, Guid bookId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (PageUserByLanguageUserIdOrdinalAndBookId.ContainsKey(key))
            {
                return PageUserByLanguageUserIdOrdinalAndBookId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = context.PageUsers
                .Where(pu => pu.BookUser != null 
                    && pu.BookUser.LanguageUserId == key.languageUserId
                    && pu.Page != null
                    && pu.Page.Ordinal == key.ordinal
                    && pu.Page.BookId == key.bookId)
                .FirstOrDefault();

            if (value == null) return null;

            // now update the cache
            PageUserUpdateAllCaches(value);
            return value;
        }
        public static async Task<PageUser?> PageUserByLanguageUserIdOrdinalAndBookIdReadAsync(
            (Guid languageUserId, int ordinal, Guid bookId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserByLanguageUserIdOrdinalAndBookIdRead(key, dbContextFactory);
            });
        }
        public static PageUser? PageUserByPageIdAndLanguageUserIdRead(
            (Guid pageId, Guid languageUserId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (PageUserByPageIdAndLanguageUserId.ContainsKey(key))
            {
                return PageUserByPageIdAndLanguageUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = context.PageUsers
                .Where(pu => pu.BookUser != null
                    && pu.BookUser.LanguageUserId == key.languageUserId
                    && pu.PageId == key.pageId)
                .FirstOrDefault();

            if (value == null) return null;
            // now update the cache
            PageUserUpdateAllCaches(value);
            return value;
        }
        public static async Task<PageUser?> PageUserByPageIdAndLanguageUserIdReadAsync(
            (Guid pageId, Guid languageUserId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserByPageIdAndLanguageUserIdRead(key, dbContextFactory);
            });
        }
        
        public static List<PageUser> PageUsersByBookUserIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory, bool shouldOverrideCache = false)
        {
            // check cache
            if (PageUsersByBookUserId.ContainsKey(key) && !shouldOverrideCache)
            {
                return PageUsersByBookUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = (from bu in context.BookUsers
                         join pu in context.PageUsers on bu.Id equals pu.BookUserId
                         where (bu.Id == key)
                         select pu)
                .ToList();


            // now update the cache
            foreach (var item in value)
            {
                PageUserUpdateAllCaches(item);
            }
            return value;
        }
        public static async Task<List<PageUser>> PageUsersByBookUserIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory, bool shouldOverrideCache = false)
        {
            return await Task<List<PageUser>>.Run(() =>
            {
                return PageUsersByBookUserIdRead(key, dbContextFactory, shouldOverrideCache);
            });
        }

        #endregion

        #region update

        public static void PageUserUpdate(PageUser value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            int numRows = context.Database.ExecuteSql($"""
                                
                UPDATE [Idioma].[PageUser]
                   SET [BookUserId] = {value.BookUserId}
                      ,[PageId] = {value.PageId}
                      ,[ReadDate] = {value.ReadDate}
                      ,[Id] = {value.Id}
                 WHERE Id = {value.Id}

                """);

            // now update the cache
            PageUserUpdateAllCaches(value);

            return;
        }

        public static async Task PageUserUpdateAsync(PageUser value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            await Task.Run(() => { PageUserUpdate(value, dbContextFactory); });
        }
        #endregion

        private static bool doesPageUserListContainPageUserId(List<PageUser> list, Guid key)
        {
            return list.Where(x => x.Id == key).Any();
        }
        private static List<PageUser> PageUsersListGetUpdated(List<PageUser> list, PageUser value)
        {
            List<PageUser> newList = new List<PageUser>();
            foreach (var pu in list)
            {
                if (pu.Id == value.Id) newList.Add(value);
                else newList.Add(pu);
            }
            return newList;
        }
        private static void PageUserUpdateAllCaches(PageUser value)
        {
            PageUserById[(Guid)value.Id] = value;

            var cachedItem1 = PageUserByPageIdAndLanguageUserId.Where(x => x.Value.Id == value.Id).FirstOrDefault();
            if (cachedItem1.Value != null)
            {
                PageUserByPageIdAndLanguageUserId[cachedItem1.Key] = value;
            }
            var cachedItem2 = PageUserByLanguageUserIdOrdinalAndBookId.Where(x => x.Value.Id == value.Id).FirstOrDefault();
            if (cachedItem2.Value != null)
            {
                PageUserByLanguageUserIdOrdinalAndBookId[cachedItem2.Key] = value;
            }

            var cachedList3 = PageUsersByBookUserId
                .Where(x => doesPageUserListContainPageUserId(x.Value, (Guid)value.Id));
            var cachedList3Array = cachedList3.ToArray();
            for (int i = 0; i < cachedList3Array.Length; i++)
            {
                var item = cachedList3Array[i];
                var newList = PageUsersListGetUpdated(item.Value, value);
                PageUsersByBookUserId[item.Key] = newList;
            }
        }
    }
}
