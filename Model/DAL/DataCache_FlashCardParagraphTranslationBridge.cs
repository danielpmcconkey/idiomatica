
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
        

        #region read
        public static List<FlashCardParagraphTranslationBridge>?
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCodeRead(
            (Guid flashCardId, Guid uiLanguageId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode.ContainsKey(key))
            {
                return FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var bridges = (from fcptb in context.FlashCardParagraphTranslationBridges
                          join pt in context.ParagraphTranslations on fcptb.ParagraphTranslationId equals pt.Id
                          where pt.LanguageId == key.uiLanguageId && fcptb.FlashCardId == key.flashCardId
                          select fcptb).ToList();
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode[key] = bridges;
            return bridges;
        }

        public static FlashCardParagraphTranslationBridge? FlashCardParagraphTranslationBridgeByIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (FlashCardParagraphTranslationBridgeById.ContainsKey(key))
            {
                return FlashCardParagraphTranslationBridgeById[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.FlashCardParagraphTranslationBridges
                .Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardParagraphTranslationBridgeById[key] = value;
            return value;
        }
        public static async Task<FlashCardParagraphTranslationBridge?> 
            FlashCardParagraphTranslationBridgeByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<FlashCardParagraphTranslationBridge?>.Run(() =>
            {
                return FlashCardParagraphTranslationBridgeByIdRead(key, dbContextFactory);
            });
        }
        #endregion

        #region create

        public static FlashCardParagraphTranslationBridge? FlashCardParagraphTranslationBridgeCreate(
            FlashCardParagraphTranslationBridge fcptb, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            int numRows = context.Database.ExecuteSql($"""
            INSERT INTO [Idioma].[FlashCardParagraphTranslationBridge]
                        ([FlashCardId]
                        ,[ParagraphTranslationId]
                        ,[Id])
                    VALUES
                        ({fcptb.FlashCardId}
                        ,{fcptb.ParagraphTranslationId}
                        ,{fcptb.Id})
            """);
            
            if (numRows < 1) throw new InvalidDataException("creating FlashCardParagraphTranslationBridge affected 0 rows");
            
            // add it to cache
            FlashCardParagraphTranslationBridgeById[fcptb.Id] = fcptb;

            return fcptb;
        }
        public static async Task<FlashCardParagraphTranslationBridge?> FlashCardParagraphTranslationBridgeCreateAsync(
            FlashCardParagraphTranslationBridge value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return FlashCardParagraphTranslationBridgeCreate(value, dbContextFactory); });
        }
        #endregion
    }
}
