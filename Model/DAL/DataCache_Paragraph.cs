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
        private static ConcurrentDictionary<Guid, Paragraph> ParagraphById = [];
        private static ConcurrentDictionary<Guid, List<Paragraph>> ParagraphsByPageId = [];


        #region read
        public static Paragraph? ParagraphByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (ParagraphById.ContainsKey(key))
            {
                return ParagraphById[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Paragraphs.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            ParagraphById[key] = value;
            return value;
        }
        public static async Task<Paragraph?> ParagraphByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Paragraph?>.Run(() =>
            {
                return ParagraphByIdRead(key, dbContextFactory);
            });
        }
        public static List<Paragraph> ParagraphsByPageIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (ParagraphsByPageId.ContainsKey(key))
            {
                return ParagraphsByPageId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = context.Paragraphs.Where(x => x.PageId == key).OrderBy(x => x.Ordinal)
                .ToList();

            // write the whole list to cache
            ParagraphsByPageId[key] = value;
           
            // write each item to cache
            foreach (var item in value) 
            {
                ParagraphById[(Guid)item.Id] = item;
            }

            return value;
        }
        public static async Task<List<Paragraph>> ParagraphsByPageIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<Paragraph>>.Run(() =>
            {
                return ParagraphsByPageIdRead(key, dbContextFactory);
            });
        }
        #endregion

        #region create


        public static Paragraph? ParagraphCreate(Paragraph paragraph, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Paragraph]
                      ([PageId]
                      ,[Ordinal]
                      ,[Id])
                VALUES
                      ({paragraph.PageId}
                      ,{paragraph.Ordinal}
                      ,{paragraph.Id})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Paragraph affected 0 rows");
            
            // add it to cache
            ParagraphById[paragraph.Id] = paragraph;

            return paragraph;
        }
        public static async Task<Paragraph?> ParagraphCreateAsync(Paragraph value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return ParagraphCreate(value, dbContextFactory); });
        }
        
        #endregion
    }
}
