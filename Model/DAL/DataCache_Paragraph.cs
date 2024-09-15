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
        public static Paragraph? ParagraphByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (ParagraphById.ContainsKey(key))
            {
                return ParagraphById[key];
            }

            // read DB
            var value = context.Paragraphs.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            ParagraphById[key] = value;
            return value;
        }
        public static async Task<Paragraph?> ParagraphByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<Paragraph?>.Run(() =>
            {
                return ParagraphByIdRead(key, context);
            });
        }
        public static List<Paragraph> ParagraphsByPageIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (ParagraphsByPageId.ContainsKey(key))
            {
                return ParagraphsByPageId[key];
            }
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
            Guid key, IdiomaticaContext context)
        {
            return await Task<List<Paragraph>>.Run(() =>
            {
                return ParagraphsByPageIdRead(key, context);
            });
        }
        #endregion

        #region create


        public static Paragraph? ParagraphCreate(Paragraph paragraph, IdiomaticaContext context)
        {
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
        public static async Task<Paragraph?> ParagraphCreateAsync(Paragraph value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return ParagraphCreate(value, context); });
        }
        
        #endregion
    }
}
