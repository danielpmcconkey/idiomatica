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
        private static ConcurrentDictionary<int, ParagraphTranslation> ParagraphTranslationById = new ConcurrentDictionary<int, ParagraphTranslation>();
        private static ConcurrentDictionary<int, List<ParagraphTranslation>> ParagraphTranslationsByParagraphId = new ConcurrentDictionary<int, List<ParagraphTranslation>>();


        #region read
        public static async Task<ParagraphTranslation> ParagraphTranslationByIdReadAsync(int key, IdiomaticaContext context)
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
        public static async Task<List<ParagraphTranslation>> ParagraphTranslationsByParargraphIdReadAsync(
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
        #endregion

        #region create
        public static async Task<bool> ParagraphTranslationCreateAsync(ParagraphTranslation value, IdiomaticaContext context)
        {
            context.ParagraphTranslations.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            // write to the ID cache
            ParagraphTranslationById[(int)value.Id] = value;

            // are there any lists with this one's paragraph already cached?
            if (value.ParagraphId == null)
            {
                return true;
            }
            if (ParagraphTranslationsByParagraphId.ContainsKey((int)value.ParagraphId))
            {
                var cachedList = ParagraphTranslationsByParagraphId[(int)value.ParagraphId];
                if (cachedList != null)
                {
                    cachedList.Add(value);
                }
            }
            return true;
        }
        #endregion
    }
}
