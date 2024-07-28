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
        private static ConcurrentDictionary<int, PageUser> PageUserById = new ConcurrentDictionary<int, PageUser>();
        private static ConcurrentDictionary<(int pageId, int languageUserId), PageUser> PageUserByPageIdAndLanguageUserId = new ConcurrentDictionary<(int pageId, int languageUserId), PageUser>();
        private static ConcurrentDictionary<(int languageUserId, int ordinal, int bookId), PageUser> PageUserByLanguageUserIdOrdinalAndBookId = new ConcurrentDictionary<(int languageUserId, int ordinal, int bookId), PageUser>();
        private static ConcurrentDictionary<int, List<PageUser>> PageUsersByBookUserId = new ConcurrentDictionary<int, List<PageUser>>();

        #region create


        public static PageUser? PageUserCreate(PageUser pageUser, IdiomaticaContext context)
        {
            if (pageUser.BookUserId is null) throw new ArgumentNullException(nameof(pageUser.BookUserId));
            if (pageUser.PageId is null) throw new ArgumentNullException(nameof(pageUser.PageId));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[PageUser]
                      ([BookUserId]
                      ,[PageId]
                      ,[ReadDate]
                      ,[UniqueKey])
                VALUES
                      ({pageUser.BookUserId}
                      ,{pageUser.PageId}
                      ,{pageUser.ReadDate}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating FlashCard affected 0 rows");
            var newEntity = context.PageUsers.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
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
        public static PageUser? PageUserByIdRead(int key, IdiomaticaContext context)
        {
            return PageUserByIdReadAsync(key, context).Result;
        }
        public static async Task<PageUser?> PageUserByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserById.ContainsKey(key))
            {
                return PageUserById[key];
            }

            // read DB
            var value = context.PageUsers.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // now update the cache
            PageUserUpdateAllCaches(value);
            return value;
        }
        public static PageUser? PageUserByLanguageUserIdOrdinalAndBookIdRead(
            (int languageUserId, int ordinal, int bookId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserByLanguageUserIdOrdinalAndBookId.ContainsKey(key))
            {
                return PageUserByLanguageUserIdOrdinalAndBookId[key];
            }
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
            (int languageUserId, int ordinal, int bookId) key, IdiomaticaContext context)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserByLanguageUserIdOrdinalAndBookIdRead(key, context);
            });
        }
        public static PageUser? PageUserByPageIdAndLanguageUserIdRead(
            (int pageId, int languageUserId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserByPageIdAndLanguageUserId.ContainsKey(key))
            {
                return PageUserByPageIdAndLanguageUserId[key];
            }
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
            (int pageId, int languageUserId) key, IdiomaticaContext context)
        {
            return await Task<PageUser?>.Run(() =>
            {
                return PageUserByPageIdAndLanguageUserIdRead(key, context);
            });
        }
        
        public static List<PageUser> PageUsersByBookUserIdRead(
            int key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            // check cache
            if (PageUsersByBookUserId.ContainsKey(key) && !shouldOverrideCache)
            {
                return PageUsersByBookUserId[key];
            }
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
            int key, IdiomaticaContext context, bool shouldOverrideCache = false)
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
            if (value.Id == null || value.Id < 1) 
                throw new ArgumentException("ID cannot be null or 0 when updating PageUser");


            int numRows = context.Database.ExecuteSql($"""
                                
                UPDATE [Idioma].[PageUser]
                   SET [BookUserId] = {value.BookUserId}
                      ,[PageId] = {value.PageId}
                      ,[ReadDate] = {value.ReadDate}
                      ,[UniqueKey] = {value.UniqueKey}
                 WHERE Id = {value.Id}

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

        private static bool doesPageUserListContainPageUserId(List<PageUser> list, int key)
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
            if (value.Id is null || value.Id < 1) return;
            PageUserById[(int)value.Id] = value;

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
                .Where(x => doesPageUserListContainPageUserId(x.Value, (int)value.Id));
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
