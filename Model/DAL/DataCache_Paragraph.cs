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
        private static ConcurrentDictionary<int, Paragraph> ParagraphById = new ConcurrentDictionary<int, Paragraph>();
        private static ConcurrentDictionary<int, List<Paragraph>> ParagraphsByPageId = new ConcurrentDictionary<int, List<Paragraph>>();


        #region read
        public static async Task<Paragraph> ParagraphByIdReadAsync(int key, IdiomaticaContext context)
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
        public static List<Paragraph> ParagraphsByPageIdRead(
            int key, IdiomaticaContext context)
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
                if (item.Id is null) continue;
                ParagraphById[(int)item.Id] = item;
            }

            return value;
        }
        public static async Task<List<Paragraph>> ParagraphsByPageIdReadAsync(
            int key, IdiomaticaContext context)
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
            if (paragraph.PageId is null) throw new ArgumentNullException(nameof(paragraph.PageId));
            if (paragraph.Ordinal is null) throw new ArgumentNullException(nameof(paragraph.Ordinal));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Paragraph]
                      ([PageId]
                      ,[Ordinal]
                      ,[UniqueKey])
                VALUES
                      ({paragraph.PageId}
                      ,{paragraph.Ordinal}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Paragraph affected 0 rows");
            var newEntity = context.Paragraphs.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in ParagraphCreate");
            }

            // add it to cache
            ParagraphById[(int)newEntity.Id] = newEntity;

            return newEntity;
        }
        public static async Task<Paragraph?> ParagraphCreateAsync(Paragraph value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return ParagraphCreate(value, context); });
        }
        
        #endregion
    }
}
