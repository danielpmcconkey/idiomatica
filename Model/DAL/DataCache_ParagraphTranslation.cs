using Microsoft.EntityFrameworkCore;
using Model.Enums;
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
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[ParagraphTranslation]
                      ([ParagraphId]
                      ,[LanguageId]
                      ,[TranslationText]
                      ,[Id])
                VALUES
                      ({paragraphTranslation.ParagraphId}
                      ,{paragraphTranslation.LanguageId}
                      ,{paragraphTranslation.TranslationText}
                      ,{paragraphTranslation.Id})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating ParagraphTranslation affected 0 rows");
            
            // add it to cache
            ParagraphTranslationUpdateCache(paragraphTranslation);

            return paragraphTranslation;
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
            var value = context.ParagraphTranslations.Where(x => x.Id == key)
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
            var value = context.ParagraphTranslations
                .Where(x => x.ParagraphId == key)
                .ToList();

            // write to cache
            ParagraphTranslationsByParagraphId[key] = value;
            // write each item to cache
            foreach (var item in value)
            {
                ParagraphTranslationById[(Guid)item.Id] = item;
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
            (Guid paragraphId, AvailableLanguageCode code) key, IdiomaticaContext context)
        {
            int numRows = context.Database.ExecuteSql($"""
                delete from [Idioma].[ParagraphTranslation]
                where [ParagraphId] = {key.paragraphId}
                and [LanguageCode] = {key.code.ToString()}
                """);
        }
        public static async Task ParagraphTranslationDeleteByParagraphIdAndLanguageCodeAsync(
            (Guid paragraphId, AvailableLanguageCode code) key, IdiomaticaContext context)
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
            // write to the ID cache
            ParagraphTranslationById[(Guid)value.Id] = value;

            // are there any lists with this one's paragraph already cached?
            if (ParagraphTranslationsByParagraphId.ContainsKey((Guid)value.ParagraphId))
            {
                var cachedList = ParagraphTranslationsByParagraphId[(Guid)value.ParagraphId];
                if (cachedList != null)
                {
                    cachedList.Add(value);
                }
            }
        }
        #endregion

    }
}
