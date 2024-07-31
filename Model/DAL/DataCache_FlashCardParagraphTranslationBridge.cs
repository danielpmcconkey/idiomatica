
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
        private static ConcurrentDictionary<int, FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridgeById = new ConcurrentDictionary<int, FlashCardParagraphTranslationBridge>();
        private static ConcurrentDictionary<(int flashCardId, string uiCode), List<FlashCardParagraphTranslationBridge>>
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode = new();

        #region read
        public static List<FlashCardParagraphTranslationBridge>?
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCodeRead(
            (int flashCardId, string uiCode) key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode.ContainsKey(key))
            {
                return FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode[key];
            }

            // read DB
            var bridges = (from fcptb in context.FlashCardParagraphTranslationBridges
                          join pt in context.ParagraphTranslations on fcptb.ParagraphTranslationId equals pt.Id
                          where pt.Code == key.uiCode && fcptb.FlashCardId == key.flashCardId
                          select fcptb).ToList();
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode[key] = bridges;
            return bridges;
        }
        public static async Task<FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridgeByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardParagraphTranslationBridgeById.ContainsKey(key))
            {
                return FlashCardParagraphTranslationBridgeById[key];
            }

            // read DB
            var value = context.FlashCardParagraphTranslationBridges
                .Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardParagraphTranslationBridgeById[key] = value;
            return value;
        }
        #endregion

        #region create

        public static FlashCardParagraphTranslationBridge? FlashCardParagraphTranslationBridgeCreate(
            FlashCardParagraphTranslationBridge fcptb, IdiomaticaContext context)
        {
            if (fcptb.FlashCardId is null) throw new ArgumentNullException(nameof(fcptb.FlashCardId));
            if (fcptb.ParagraphTranslationId is null) throw new ArgumentNullException(nameof(fcptb.ParagraphTranslationId));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                INSERT INTO [Idioma].[FlashCardParagraphTranslationBridge]
                           ([FlashCardId]
                           ,[ParagraphTranslationId]
                           ,[UniqueKey])
                     VALUES
                           ({fcptb.FlashCardId}
                           ,{fcptb.ParagraphTranslationId}
                           ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating FlashCardParagraphTranslationBridge affected 0 rows");
            var newEntity = context.FlashCardParagraphTranslationBridges.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in FlashCardParagraphTranslationBridgeCreate");
            }

            // add it to cache
            FlashCardParagraphTranslationBridgeById[(int)newEntity.Id] = newEntity;

            return newEntity;
        }
        public static async Task<FlashCardParagraphTranslationBridge?> FlashCardParagraphTranslationBridgeCreateAsync(
            FlashCardParagraphTranslationBridge value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return FlashCardParagraphTranslationBridgeCreate(value, context); });
        }
        #endregion
    }
}
