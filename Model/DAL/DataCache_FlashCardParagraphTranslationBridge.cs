
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
            

        #region read
        public static async Task<FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridgeByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardParagraphTranslationBridgeById.ContainsKey(key))
            {
                return FlashCardParagraphTranslationBridgeById[key];
            }

            // read DB
            var value = await context.FlashCardParagraphTranslationBridges
                .Where(x => x.Id == key)
                .FirstOrDefaultAsync();
            if (value == null) return null;
            // write to cache
            FlashCardParagraphTranslationBridgeById[key] = value;
            return value;
        }
        #endregion

        #region create
        public static async Task<bool> FlashCardParagraphTranslationBridgeCreateAsync(FlashCardParagraphTranslationBridge value, IdiomaticaContext context)
        {
            context.FlashCardParagraphTranslationBridges.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            // write to the ID cache
            FlashCardParagraphTranslationBridgeById[(int)value.Id] = value;

                
            return true;
        }
        #endregion
    }
}
