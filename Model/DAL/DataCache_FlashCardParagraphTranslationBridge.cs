
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
        private static ConcurrentDictionary<Guid, FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridgeById = [];
        private static ConcurrentDictionary<(Guid flashCardId, Guid uiLanguageKey), List<FlashCardParagraphTranslationBridge>>
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode = [];

        #region read
        public static List<FlashCardParagraphTranslationBridge>?
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCodeRead(
            (Guid flashCardId, Guid uiLanguageKey) key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode.ContainsKey(key))
            {
                return FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode[key];
            }

            // read DB
            var bridges = (from fcptb in context.FlashCardParagraphTranslationBridges
                          join pt in context.ParagraphTranslations on fcptb.ParagraphTranslationKey equals pt.UniqueKey
                          where pt.LanguageKey == key.uiLanguageKey && fcptb.FlashCardKey == key.flashCardId
                          select fcptb).ToList();
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode[key] = bridges;
            return bridges;
        }

        public static FlashCardParagraphTranslationBridge? FlashCardParagraphTranslationBridgeByIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardParagraphTranslationBridgeById.ContainsKey(key))
            {
                return FlashCardParagraphTranslationBridgeById[key];
            }

            // read DB
            var value = context.FlashCardParagraphTranslationBridges
                .Where(x => x.UniqueKey == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardParagraphTranslationBridgeById[key] = value;
            return value;
        }
        public static async Task<FlashCardParagraphTranslationBridge?> 
            FlashCardParagraphTranslationBridgeByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<FlashCardParagraphTranslationBridge?>.Run(() =>
            {
                return FlashCardParagraphTranslationBridgeByIdRead(key, context);
            });
        }
        #endregion

        #region create

        public static FlashCardParagraphTranslationBridge? FlashCardParagraphTranslationBridgeCreate(
            FlashCardParagraphTranslationBridge fcptb, IdiomaticaContext context)
        {
            int numRows = context.Database.ExecuteSql($"""
            INSERT INTO [Idioma].[FlashCardParagraphTranslationBridge]
                        ([FlashCardKey]
                        ,[ParagraphTranslationKey]
                        ,[UniqueKey])
                    VALUES
                        ({fcptb.FlashCardKey}
                        ,{fcptb.ParagraphTranslationKey}
                        ,{fcptb.UniqueKey})
            """);
            
            if (numRows < 1) throw new InvalidDataException("creating FlashCardParagraphTranslationBridge affected 0 rows");
            
            // add it to cache
            FlashCardParagraphTranslationBridgeById[fcptb.UniqueKey] = fcptb;

            return fcptb;
        }
        public static async Task<FlashCardParagraphTranslationBridge?> FlashCardParagraphTranslationBridgeCreateAsync(
            FlashCardParagraphTranslationBridge value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return FlashCardParagraphTranslationBridgeCreate(value, context); });
        }
        #endregion
    }
}
