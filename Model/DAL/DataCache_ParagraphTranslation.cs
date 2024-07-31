using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<int, ParagraphTranslation> ParagraphTranslationById = new ConcurrentDictionary<int, ParagraphTranslation>();
        private static ConcurrentDictionary<int, List<ParagraphTranslation>> ParagraphTranslationsByParagraphId = new ConcurrentDictionary<int, List<ParagraphTranslation>>();

        #region create


        public static ParagraphTranslation? ParagraphTranslationCreate(
            ParagraphTranslation paragraphTranslation, IdiomaticaContext context)
        {
            if (paragraphTranslation.ParagraphId is null) 
                throw new ArgumentNullException(nameof(paragraphTranslation.ParagraphId));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[ParagraphTranslation]
                      ([ParagraphId]
                      ,[LanguageCode]
                      ,[TranslationText]
                      ,[UniqueKey])
                VALUES
                      ({paragraphTranslation.ParagraphId}
                      ,{paragraphTranslation.Code}
                      ,{paragraphTranslation.TranslationText}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating ParagraphTranslation affected 0 rows");
            var newEntity = context.ParagraphTranslations.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in ParagraphTranslationCreate");
            }


            // add it to cache
            ParagraphTranslationUpdateCache(newEntity);

            return newEntity;
        }
        public static async Task<ParagraphTranslation?> ParagraphTranslationCreateAsync(ParagraphTranslation value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return ParagraphTranslationCreate(value, context); });
        }


        #endregion

        #region read
        public static ParagraphTranslation? ParagraphTranslationByIdRead(int key, IdiomaticaContext context)
        {
            // check cache
            if (ParagraphTranslationById.ContainsKey(key))
            {
                return ParagraphTranslationById[key];
            }

            // read DB
            var value = context.ParagraphTranslations.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            ParagraphTranslationById[key] = value;
            return value;
        }
        public static List<ParagraphTranslation> ParagraphTranslationsByParargraphIdRead(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (ParagraphTranslationsByParagraphId.ContainsKey(key))
            {
                return ParagraphTranslationsByParagraphId[key];
            }
            // read DB
            var value = context.ParagraphTranslations.Where(x => x.ParagraphId == key).ToList();

            // write to cache
            ParagraphTranslationsByParagraphId[key] = value;
            // write each item to cache
            foreach (var item in value) { ParagraphTranslationById[(int)item.Id] = item; }

            return value;
        }
        public static async Task<List<ParagraphTranslation>> ParagraphTranslationsByParargraphIdReadAsync(
            int key, IdiomaticaContext context)
        {
            return await Task<List<ParagraphTranslation>>.Run(() =>
            {
                return ParagraphTranslationsByParargraphIdRead(key, context);
            });
        }
        #endregion

        #region delete
        public static void ParagraphTranslationDeleteByParagraphIdAndLanguageCode(
            (int paragraphId, string code) key, IdiomaticaContext context)
        {
            int numRows = context.Database.ExecuteSql($"""
                delete from [Idioma].[ParagraphTranslation]
                where [ParagraphId] = {key.paragraphId}
                and [LanguageCode] = {key.code}
                """);
        }
        public static async Task ParagraphTranslationDeleteByParagraphIdAndLanguageCodeAsync(
            (int paragraphId, string code) key, IdiomaticaContext context)
        {
            await Task.Run(() =>
            {
                ParagraphTranslationDeleteByParagraphIdAndLanguageCode(key, context);
            });
        }
        #endregion

        #region cache
        private static void ParagraphTranslationUpdateCache(ParagraphTranslation value)
        {
            if (value.Id is null) return;
            // write to the ID cache
            ParagraphTranslationById[(int)value.Id] = value;

            // are there any lists with this one's paragraph already cached?
            if (value.ParagraphId == null)
            {
                return;
            }
            if (ParagraphTranslationsByParagraphId.ContainsKey((int)value.ParagraphId))
            {
                var cachedList = ParagraphTranslationsByParagraphId[(int)value.ParagraphId];
                if (cachedList != null)
                {
                    cachedList.Add(value);
                }
            }
        }
        #endregion

    }
}
