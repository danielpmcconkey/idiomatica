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
        public static async Task<Page?> PageByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (PageById.ContainsKey(key))
            {
                return PageById[key];
            }

            // read DB
            var value = await context.Pages.Where(x => x.Id == key).FirstOrDefaultAsync();
            if (value == null) return null;
            // write to cache
            PageById[key] = value;
            return value;
        }
        public static Page? PageByOrdinalAndBookIdRead(
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
            if (value.Id is null) return value;
            PageById[(int)value.Id] = value;
            return value;
        }
        public static async Task<Page?> PageByOrdinalAndBookIdReadAsync(
            (int ordinal, int bookId) key, IdiomaticaContext context)
        {
            return await Task<Page?>.Run(() =>
            {
                return PageByOrdinalAndBookIdRead(key, context);
            });
        }
        public static List<Page> PagesByBookIdRead(
            int key, IdiomaticaContext context)
        {
            var task = PagesByBookIdReadAsync(key, context);
            return task.Result;
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
            var value = await context.Pages.Where(x => x.BookId == key).OrderBy(x => x.Ordinal)
                .ToListAsync();

            // write to cache
            PagesByBookId[key] = value;
            // write each item to cache
            foreach (var item in value) 
            { 
                if(item is null || item.Id is null) continue;
                PageById[(int)item.Id] = item; 
            }

            return value;
        }
        #endregion

        #region create


        public static Page? PageCreate(Page page, IdiomaticaContext context)
        {
            if (page.BookId is null) throw new ArgumentNullException(nameof(page.BookId));
            if (page.Ordinal is null) throw new ArgumentNullException(nameof(page.Ordinal));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Page]
                      ([BookId]
                      ,[Ordinal]
                      ,[OriginalText]
                      ,[UniqueKey])
                VALUES
                      ({page.BookId}
                      ,{page.Ordinal}
                      ,{page.OriginalText}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Page affected 0 rows");
            var newEntity = context.Pages.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in FlashCardCreate");
            }


            // add it to cache
            PageById[(int)newEntity.Id] = newEntity; ;

            return newEntity;
        }
        public static async Task<Page?> PageCreateAsync(Page value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return PageCreate(value, context); });
        }
        #endregion
    }
}
