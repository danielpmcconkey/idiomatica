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
        
        #region create


        public static ParagraphTranslation? ParagraphTranslationCreate(
            ParagraphTranslation paragraphTranslation, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

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
        public static async Task<ParagraphTranslation?> ParagraphTranslationCreateAsync(ParagraphTranslation value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return ParagraphTranslationCreate(value, dbContextFactory); });
        }


        #endregion

        #region read
        public static ParagraphTranslation? ParagraphTranslationByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (ParagraphTranslationById.ContainsKey(key))
            {
                return ParagraphTranslationById[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = context.ParagraphTranslations.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            ParagraphTranslationById[key] = value;
            return value;
        }
        public static List<ParagraphTranslation> ParagraphTranslationsByParargraphIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (ParagraphTranslationsByParagraphId.ContainsKey(key))
            {
                return ParagraphTranslationsByParagraphId[key];
            }
            var context = dbContextFactory.CreateDbContext();

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
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<ParagraphTranslation>>.Run(() =>
            {
                return ParagraphTranslationsByParargraphIdRead(key, dbContextFactory);
            });
        }
        #endregion

        #region delete
        public static void ParagraphTranslationDeleteByParagraphIdAndLanguageCode(
            (Guid paragraphId, AvailableLanguageCode code) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            int numRows = context.Database.ExecuteSql($"""
                delete from [Idioma].[ParagraphTranslation]
                where [ParagraphId] = {key.paragraphId}
                and [LanguageCode] = {key.code.ToString()}
                """);
        }
        public static async Task ParagraphTranslationDeleteByParagraphIdAndLanguageCodeAsync(
            (Guid paragraphId, AvailableLanguageCode code) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            await Task.Run(() =>
            {
                ParagraphTranslationDeleteByParagraphIdAndLanguageCode(key, dbContextFactory);
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
