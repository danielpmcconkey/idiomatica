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
        public static List<Sentence> SentencesByPageIdRead(
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
        public static async Task<List<Sentence>> SentencesByPageIdReadAsync(
            int key, IdiomaticaContext context)
        {
            return await Task<List<Sentence>>.Run(() =>
            {
                return SentencesByPageIdRead(key, context);
            });
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


        public static Sentence? SentenceCreate(Sentence sentence, IdiomaticaContext context)
        {
            if (sentence.ParagraphId is null) throw new ArgumentNullException(nameof(sentence.ParagraphId));
            if (sentence.Ordinal is null) throw new ArgumentNullException(nameof(sentence.Ordinal));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Sentence]
                      ([ParagraphId]
                      ,[Ordinal]
                      ,[Text]
                      ,[UniqueKey])
                VALUES
                      ({sentence.ParagraphId}
                      ,{sentence.Ordinal}
                      ,{sentence.Text}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Sentence affected 0 rows");
            var newEntity = context.Sentences.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in SentenceCreate");
            }


            // add it to cache
            SentenceById[(int)newEntity.Id] = newEntity;

            return newEntity;
        }
        public static async Task<Sentence?> SentenceCreateAsync(Sentence value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return SentenceCreate(value, context); });
        }

        #endregion
    }
}
