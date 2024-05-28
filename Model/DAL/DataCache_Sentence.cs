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
        private static ConcurrentDictionary<int, Sentence> SentenceById = new ConcurrentDictionary<int, Sentence>();
        private static ConcurrentDictionary<int, List<Sentence>> SentencesByPageId = new ConcurrentDictionary<int, List<Sentence>>();
        private static ConcurrentDictionary<int, List<Sentence>> SentencesByParagraphId = new ConcurrentDictionary<int, List<Sentence>>();


        #region read
        public static async Task<Sentence> SentenceByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (SentenceById.ContainsKey(key))
            {
                return SentenceById[key];
            }

            // read DB
            var value = context.Sentences.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            SentenceById[key] = value;
            return value;
        }
        public static async Task<List<Sentence>> SentencesByPageIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (SentencesByPageId.ContainsKey(key))
            {
                return SentencesByPageId[key];
            }
            // read DB
            var value = (from p in context.Pages
                         join pp in context.Paragraphs on p.Id equals pp.PageId
                         join s in context.Sentences on pp.Id equals s.ParagraphId
                         orderby s.Ordinal
                         where (p.Id == key)
                         select s

                          ).ToList();


            // write to cache
            SentencesByPageId[key] = value;
            return value;
        }
        public static async Task<List<Sentence>> SentencesByParagraphIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (SentencesByParagraphId.ContainsKey(key))
            {
                return SentencesByParagraphId[key];
            }

            // read DB
            var value = context.Sentences
                .Where(x => x.ParagraphId == key)
                .ToList();
            // write to cache
            SentencesByParagraphId[key] = value;
            return value;
        }


        #endregion

        #region create
        public static async Task<bool> SentenceCreateAsync(Sentence value, IdiomaticaContext context)
        {
            context.Sentences.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            SentenceById[(int)value.Id] = value;
            return true;
        }
        #endregion
    }
}
