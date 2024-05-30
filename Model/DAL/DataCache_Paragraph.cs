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
        public static async Task<List<Paragraph>> ParagraphsByPageIdReadAsync(
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

            // write to cache
            ParagraphsByPageId[key] = value;
            // write each item to cache
            foreach (var item in value) { ParagraphById[(int)item.Id] = item; }

            return value;
        }
        #endregion

        #region create
        public static async Task<bool> ParagraphCreateAsync(Paragraph value, IdiomaticaContext context)
        {
            context.Paragraphs.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            ParagraphById[(int)value.Id] = value;
            return true;
        }
        #endregion
    }
}
