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
        public static async Task<bool> PageUserCreateAsync(PageUser value, IdiomaticaContext context)
        {
            context.PageUsers.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            // now update the cache
            await PageUserUpdateAllCachesAsync(value);
            return true;
        }
        #endregion

        #region read
        public static async Task<PageUser> PageUserByIdReadAsync(int key, IdiomaticaContext context)
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
            await PageUserUpdateAllCachesAsync(value);
            return value;
        }
        public static async Task<PageUser?> PageUserByLanguageUserIdOrdinalAndBookIdReadAsync(
            (int languageUserId, int ordinal, int bookId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserByLanguageUserIdOrdinalAndBookId.ContainsKey(key))
            {
                return PageUserByLanguageUserIdOrdinalAndBookId[key];
            }
            // read DB
            var value = context.PageUsers
                .Where(pu => pu.BookUser.LanguageUserId == key.languageUserId
                    && pu.Page.Ordinal == key.ordinal
                    && pu.Page.BookId == key.bookId)
                .FirstOrDefault();

            if (value == null) return null;

            // now update the cache
            await PageUserUpdateAllCachesAsync(value);
            return value;
        }
        public static async Task<PageUser?> PageUserByPageIdAndLanguageUserIdReadAsync(
            (int pageId, int languageUserId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserByPageIdAndLanguageUserId.ContainsKey(key))
            {
                return PageUserByPageIdAndLanguageUserId[key];
            }
            // read DB
            var value = context.PageUsers
                .Where(pu => pu.BookUser.LanguageUserId == key.languageUserId
                    && pu.PageId == key.pageId)
                .FirstOrDefault();

            if (value == null) return null;
            // now update the cache
            await PageUserUpdateAllCachesAsync(value);
            return value;
        }
        public static async Task<List<PageUser>> PageUsersByBookUserIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (PageUsersByBookUserId.ContainsKey(key))
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
                await PageUserUpdateAllCachesAsync(item);
            }
            return value;
        }

        #endregion

        #region update
        public static async Task PageUserUpdateAsync(PageUser value, IdiomaticaContext context)
        {
            if (value.Id == null || value.Id < 1) throw new ArgumentException("ID cannot be null or 0 when updating");

            var valueFromDb = context.PageUsers.Where(pu => pu.Id == value.Id).FirstOrDefault();
            if (valueFromDb == null) throw new InvalidDataException("Value does not exist in the DB to update");

            valueFromDb.ReadDate = value.ReadDate;
            valueFromDb.BookUserId = value.BookUserId;
            valueFromDb.PageId = value.PageId;
            context.SaveChanges();

            // now update the cache
            await PageUserUpdateAllCachesAsync(value);

            return;
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
        private static async Task PageUserUpdateAllCachesAsync(PageUser value)
        {
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
