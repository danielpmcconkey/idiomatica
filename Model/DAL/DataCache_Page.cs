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
        private static ConcurrentDictionary<int, Page> PageById = new ConcurrentDictionary<int, Page>();
        private static ConcurrentDictionary<(int ordinal, int bookId), Page> PageByOrdinalAndBookId = new ConcurrentDictionary<(int ordinal, int bookId), Page>();
        private static ConcurrentDictionary<int, List<Page>> PagesByBookId = new ConcurrentDictionary<int, List<Page>>();

        #region read
        public static Page? PageByIdRead(int key, IdiomaticaContext context)
        {
            // check cache
            if (PageById.ContainsKey(key))
            {
                return PageById[key];
            }

            // read DB
            var value = context.Pages.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            PageById[key] = value;
            return value;
        }
        public static async Task<Page?> PageByOrdinalAndBookIdReadAsync(
            (int ordinal, int bookId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageByOrdinalAndBookId.ContainsKey(key))
            {
                return PageByOrdinalAndBookId[key];
            }
            // read DB
            var value = context.Pages
                .Where(p => p.Ordinal == key.ordinal
                    && p.BookId == key.bookId)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            PageByOrdinalAndBookId[key] = value;
            PageById[(int)value.Id] = value;
            return value;
        }
        public static async Task<List<Page>> PagesByBookIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (PagesByBookId.ContainsKey(key))
            {
                return PagesByBookId[key];
            }
            // read DB
            var value = context.Pages.Where(x => x.BookId == key).OrderBy(x => x.Ordinal)
                .ToList();

            // write to cache
            PagesByBookId[key] = value;
            // write each item to cache
            foreach (var item in value) { PageById[(int)item.Id] = item; }

            return value;
        }
        #endregion

        #region create
        public static async Task<bool> PageCreateNewAsync(Page value, IdiomaticaContext context)
        {
            context.Pages.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            PageById[(int)value.Id] = value;
            return true;
        }
        #endregion
    }
}
