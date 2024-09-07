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


        public static PageUser? PageUserCreate(PageUser pageUser, IdiomaticaContext context)
        {
            if (pageUser.BookUserKey is null) throw new ArgumentNullException(nameof(pageUser.BookUserKey));
            if (pageUser.PageKey is null) throw new ArgumentNullException(nameof(pageUser.PageKey));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[PageUser]
                      ([BookUserId]
                      ,[PageId]
                      ,[ReadDate]
                      ,[UniqueKey])
                VALUES
                      ({pageUser.BookUserKey}
                      ,{pageUser.PageKey}
                      ,{pageUser.ReadDate}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating FlashCard affected 0 rows");
            var newEntity = context.PageUsers.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.UniqueKey is null)
            {
                throw new InvalidDataException("newEntity is null in FlashCardCreate");
            }


            // add it to cache
            PageUserUpdateAllCaches(newEntity);

            return newEntity;
        }
        public static async Task<PageUser?> PageUserCreateAsync(PageUser value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return PageUserCreate(value, context); });
        }


        
        #endregion

        #region read
        public static PageUser? PageUserByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserById.ContainsKey(key))
            {
                return PageUserById[key];
            }

            // read DB
            var value = context.PageUsers.Where(x => x.UniqueKey == key)
                .FirstOrDefault();
            if (value == null) return null;
            // now update the cache
            PageUserUpdateAllCaches(value);
            return value;
        }
        public static async Task<PageUser?> PageUserByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserByIdRead(key, context);
            });
        }
        public static PageUser? PageUserByLanguageUserIdOrdinalAndBookIdRead(
            (Guid languageUserId, int ordinal, Guid bookId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserByLanguageUserIdOrdinalAndBookId.ContainsKey(key))
            {
                return PageUserByLanguageUserIdOrdinalAndBookId[key];
            }
            // read DB
            var value = context.PageUsers
                .Where(pu => pu.BookUser != null 
                    && pu.BookUser.LanguageUserKey == key.languageUserId
                    && pu.Page != null
                    && pu.Page.Ordinal == key.ordinal
                    && pu.Page.BookKey == key.bookId)
                .FirstOrDefault();

            if (value == null) return null;

            // now update the cache
            PageUserUpdateAllCaches(value);
            return value;
        }
        public static async Task<PageUser?> PageUserByLanguageUserIdOrdinalAndBookIdReadAsync(
            (Guid languageUserId, int ordinal, Guid bookId) key, IdiomaticaContext context)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserByLanguageUserIdOrdinalAndBookIdRead(key, context);
            });
        }
        public static PageUser? PageUserByPageIdAndLanguageUserIdRead(
            (Guid pageId, Guid languageUserId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserByPageIdAndLanguageUserId.ContainsKey(key))
            {
                return PageUserByPageIdAndLanguageUserId[key];
            }
            // read DB
            var value = context.PageUsers
                .Where(pu => pu.BookUser != null
                    && pu.BookUser.LanguageUserKey == key.languageUserId
                    && pu.PageKey == key.pageId)
                .FirstOrDefault();

            if (value == null) return null;
            // now update the cache
            PageUserUpdateAllCaches(value);
            return value;
        }
        public static async Task<PageUser?> PageUserByPageIdAndLanguageUserIdReadAsync(
            (Guid pageId, Guid languageUserId) key, IdiomaticaContext context)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserByPageIdAndLanguageUserIdRead(key, context);
            });
        }
        
        public static List<PageUser> PageUsersByBookUserIdRead(
            Guid key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            // check cache
            if (PageUsersByBookUserId.ContainsKey(key) && !shouldOverrideCache)
            {
                return PageUsersByBookUserId[key];
            }
            // read DB
            var value = (from bu in context.BookUsers
                         join pu in context.PageUsers on bu.UniqueKey equals pu.BookUserKey
                         where (bu.UniqueKey == key)
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
            Guid key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            return await Task<List<PageUser>>.Run(() =>
            {
                return PageUsersByBookUserIdRead(key, context, shouldOverrideCache);
            });
        }

        #endregion

        #region update

        public static void PageUserUpdate(PageUser value, IdiomaticaContext context)
        {
            if (value.UniqueKey == null) 
                throw new ArgumentException("ID cannot be null or 0 when updating PageUser");


            int numRows = context.Database.ExecuteSql($"""
                                
                UPDATE [Idioma].[PageUser]
                   SET [BookUserId] = {value.BookUserKey}
                      ,[PageId] = {value.PageKey}
                      ,[ReadDate] = {value.ReadDate}
                      ,[UniqueKey] = {value.UniqueKey}
                 WHERE Id = {value.UniqueKey}

                """);

            // now update the cache
            PageUserUpdateAllCaches(value);

            return;
        }

        public static async Task PageUserUpdateAsync(PageUser value, IdiomaticaContext context)
        {
            await Task.Run(() => { PageUserUpdate(value, context); });
        }
        #endregion

        private static bool doesPageUserListContainPageUserId(List<PageUser> list, Guid key)
        {
            return list.Where(x => x.UniqueKey == key).Any();
        }
        private static List<PageUser> PageUsersListGetUpdated(List<PageUser> list, PageUser value)
        {
            List<PageUser> newList = new List<PageUser>();
            foreach (var pu in list)
            {
                if (pu.UniqueKey == value.UniqueKey) newList.Add(value);
                else newList.Add(pu);
            }
            return newList;
        }
        private static void PageUserUpdateAllCaches(PageUser value)
        {
            if (value.UniqueKey is null) return;
            PageUserById[(Guid)value.UniqueKey] = value;

            var cachedItem1 = PageUserByPageIdAndLanguageUserId.Where(x => x.Value.UniqueKey == value.UniqueKey).FirstOrDefault();
            if (cachedItem1.Value != null)
            {
                PageUserByPageIdAndLanguageUserId[cachedItem1.Key] = value;
            }
            var cachedItem2 = PageUserByLanguageUserIdOrdinalAndBookId.Where(x => x.Value.UniqueKey == value.UniqueKey).FirstOrDefault();
            if (cachedItem2.Value != null)
            {
                PageUserByLanguageUserIdOrdinalAndBookId[cachedItem2.Key] = value;
            }

            var cachedList3 = PageUsersByBookUserId
                .Where(x => doesPageUserListContainPageUserId(x.Value, (Guid)value.UniqueKey));
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
