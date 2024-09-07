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
        private static ConcurrentDictionary<Guid, Page> PageById = [];
        private static ConcurrentDictionary<(int ordinal, Guid bookId), Page> PageByOrdinalAndBookId = [];
        private static ConcurrentDictionary<Guid, List<Page>> PagesByBookId = [];

        #region read
        public static Page? PageByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (PageById.ContainsKey(key))
            {
                return PageById[key];
            }

            // read DB
            var value = context.Pages.Where(x => x.UniqueKey == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            PageById[key] = value;
            return value;
        }
        public static async Task<Page?> PageByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<Page?>.Run(() =>
            {
                return PageByIdRead(key, context);
            });
        }
        public static Page? PageByOrdinalAndBookIdRead(
            (int ordinal, Guid bookId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageByOrdinalAndBookId.ContainsKey(key))
            {
                return PageByOrdinalAndBookId[key];
            }
            // read DB
            var value = context.Pages
                .Where(p => p.Ordinal == key.ordinal
                    && p.BookKey == key.bookId)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            PageByOrdinalAndBookId[key] = value;
            if (value.UniqueKey is null) return value;
            PageById[(Guid)value.UniqueKey] = value;
            return value;
        }
        public static async Task<Page?> PageByOrdinalAndBookIdReadAsync(
            (int ordinal, Guid bookId) key, IdiomaticaContext context)
        {
            return await Task<Page?>.Run(() =>
            {
                return PageByOrdinalAndBookIdRead(key, context);
            });
        }
        public static List<Page> PagesByBookIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (PagesByBookId.ContainsKey(key))
            {
                return PagesByBookId[key];
            }
            // read DB
            var value = context.Pages.Where(x => x.BookKey == key).OrderBy(x => x.Ordinal)
                .ToList();

            // write to cache
            PagesByBookId[key] = value;
            // write each item to cache
            foreach (var item in value)
            {
                if (item is null || item.UniqueKey is null) continue;
                PageById[(Guid)item.UniqueKey] = item;
            }

            return value;
        }        
        public static async Task<List<Page>> PagesByBookIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<List<Page>>.Run(() =>
            {
                return PagesByBookIdRead(key, context);
            });
        }
        #endregion

        #region create


        public static Page? PageCreate(Page page, IdiomaticaContext context)
        {
            if (page.BookKey is null) throw new ArgumentNullException(nameof(page.BookKey));
            if (page.Ordinal is null) throw new ArgumentNullException(nameof(page.Ordinal));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Page]
                      ([BookId]
                      ,[Ordinal]
                      ,[OriginalText]
                      ,[UniqueKey])
                VALUES
                      ({page.BookKey}
                      ,{page.Ordinal}
                      ,{page.OriginalText}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Page affected 0 rows");
            var newEntity = context.Pages.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.UniqueKey is null)
            {
                throw new InvalidDataException("newEntity is null in FlashCardCreate");
            }


            // add it to cache
            PageById[(Guid)newEntity.UniqueKey] = newEntity; ;

            return newEntity;
        }
        public static async Task<Page?> PageCreateAsync(Page value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return PageCreate(value, context); });
        }
        #endregion
    }
}
