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
        public static Page? PageByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (PageById.ContainsKey(key))
            {
                return PageById[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Pages.Where(x => x.Id == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            PageById[key] = value;
            return value;
        }
        public static async Task<Page?> PageByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Page?>.Run(() =>
            {
                return PageByIdRead(key, dbContextFactory);
            });
        }
        public static Page? PageByOrdinalAndBookIdRead(
            (int ordinal, Guid bookId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (PageByOrdinalAndBookId.ContainsKey(key))
            {
                return PageByOrdinalAndBookId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = context.Pages
                .Where(p => p.Ordinal == key.ordinal
                    && p.BookId == key.bookId)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            PageByOrdinalAndBookId[key] = value;
            PageById[(Guid)value.Id] = value;
            return value;
        }
        public static async Task<Page?> PageByOrdinalAndBookIdReadAsync(
            (int ordinal, Guid bookId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Page?>.Run(() =>
            {
                return PageByOrdinalAndBookIdRead(key, dbContextFactory);
            });
        }
        public static List<Page> PagesByBookIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (PagesByBookId.ContainsKey(key))
            {
                return PagesByBookId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = context.Pages.Where(x => x.BookId == key).OrderBy(x => x.Ordinal)
                .ToList();

            // write to cache
            PagesByBookId[key] = value;
            // write each item to cache
            foreach (var item in value)
            {
                if (item is null) continue;
                PageById[(Guid)item.Id] = item;
            }

            return value;
        }        
        public static async Task<List<Page>> PagesByBookIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<Page>>.Run(() =>
            {
                return PagesByBookIdRead(key, dbContextFactory);
            });
        }
        #endregion

        #region create


        public static Page? PageCreate(Page page, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Page]
                      ([BookId]
                      ,[Ordinal]
                      ,[OriginalText]
                      ,[Id])
                VALUES
                      ({page.BookId}
                      ,{page.Ordinal}
                      ,{page.OriginalText}
                      ,{page.Id})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Page affected 0 rows");
            


            // add it to cache
            PageById[page.Id] = page; ;

            return page;
        }
        public static async Task<Page?> PageCreateAsync(Page value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return PageCreate(value, dbContextFactory); });
        }
        #endregion
    }
}
