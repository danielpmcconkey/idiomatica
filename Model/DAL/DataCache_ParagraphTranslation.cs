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
        private static ConcurrentDictionary<Guid, ParagraphTranslation> ParagraphTranslationById = [];
        private static ConcurrentDictionary<Guid, List<ParagraphTranslation>> ParagraphTranslationsByParagraphId = [];

        #region create


        public static ParagraphTranslation? ParagraphTranslationCreate(
            ParagraphTranslation paragraphTranslation, IdiomaticaContext context)
        {
            if (paragraphTranslation.ParagraphKey is null) 
                throw new ArgumentNullException(nameof(paragraphTranslation.ParagraphKey));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[ParagraphTranslation]
                      ([ParagraphKey]
                      ,[LanguageCode]
                      ,[TranslationText]
                      ,[UniqueKey])
                VALUES
                      ({paragraphTranslation.ParagraphKey}
                      ,{paragraphTranslation.Code}
                      ,{paragraphTranslation.TranslationText}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating ParagraphTranslation affected 0 rows");
            var newEntity = context.ParagraphTranslations.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.UniqueKey is null)
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
        public static ParagraphTranslation? ParagraphTranslationByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (ParagraphTranslationById.ContainsKey(key))
            {
                return ParagraphTranslationById[key];
            }

            // read DB
            var value = context.ParagraphTranslations.Where(x => x.UniqueKey == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            ParagraphTranslationById[key] = value;
            return value;
        }
        public static List<ParagraphTranslation> ParagraphTranslationsByParargraphIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (ParagraphTranslationsByParagraphId.ContainsKey(key))
            {
                return ParagraphTranslationsByParagraphId[key];
            }
            // read DB
            var value = context.ParagraphTranslations.Where(x => x.ParagraphKey == key).ToList();

            // write to cache
            ParagraphTranslationsByParagraphId[key] = value;
            // write each item to cache
            foreach (var item in value)
            {
                if (item.UniqueKey is null) continue;
                ParagraphTranslationById[(Guid)item.UniqueKey] = item;
            }

            return value;
        }
        public static async Task<List<ParagraphTranslation>> ParagraphTranslationsByParargraphIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<List<ParagraphTranslation>>.Run(() =>
            {
                return ParagraphTranslationsByParargraphIdRead(key, context);
            });
        }
        #endregion

        #region delete
        public static void ParagraphTranslationDeleteByParagraphIdAndLanguageCode(
            (Guid paragraphId, string code) key, IdiomaticaContext context)
        {
            int numRows = context.Database.ExecuteSql($"""
                delete from [Idioma].[ParagraphTranslation]
                where [ParagraphKey] = {key.paragraphId}
                and [LanguageCode] = {key.code}
                """);
        }
        public static async Task ParagraphTranslationDeleteByParagraphIdAndLanguageCodeAsync(
            (Guid paragraphId, string code) key, IdiomaticaContext context)
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
            if (value.UniqueKey is null) return;
            // write to the ID cache
            ParagraphTranslationById[(Guid)value.UniqueKey] = value;

            // are there any lists with this one's paragraph already cached?
            if (value.ParagraphKey == null)
            {
                return;
            }
            if (ParagraphTranslationsByParagraphId.ContainsKey((Guid)value.ParagraphKey))
            {
                var cachedList = ParagraphTranslationsByParagraphId[(Guid)value.ParagraphKey];
                if (cachedList != null)
                {
                    cachedList.Add(value);
                }
            }
        }
        #endregion

    }
}
