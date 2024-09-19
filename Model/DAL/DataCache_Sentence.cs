using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<Guid, Sentence> SentenceById = [];
        private static ConcurrentDictionary<Guid, List<Sentence>> SentencesByPageId = [];
        private static ConcurrentDictionary<Guid, List<Sentence>> SentencesByParagraphId = [];


        #region read
        public static Sentence? SentenceByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (SentenceById.ContainsKey(key))
            {
                return SentenceById[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Sentences.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            SentenceById[key] = value;
            return value;
        }
        public static async Task<Sentence?> SentenceByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Sentence?>.Run(() =>
            {
                return SentenceByIdRead(key, dbContextFactory);
            });
        }


        public static List<Sentence> SentencesByPageIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (SentencesByPageId.ContainsKey(key))
            {
                return SentencesByPageId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = (from p in context.Pages
                         join pp in context.Paragraphs on p.Id equals pp.PageId
                         join s in context.Sentences on pp.Id equals s.ParagraphId
                         orderby pp.Ordinal, s.Ordinal
                         where (p.Id == key)
                         select s

                          ).ToList();


            // write to cache
            SentencesByPageId[key] = value;
            return value;
        }
        public static async Task<List<Sentence>> SentencesByPageIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<Sentence>>.Run(() =>
            {
                return SentencesByPageIdRead(key, dbContextFactory);
            });
        }


        public static List<Sentence> SentencesByParagraphIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (SentencesByParagraphId.ContainsKey(key))
            {
                return SentencesByParagraphId[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Sentences
                .Where(x => x.ParagraphId == key)
                .ToList();
            // write to cache
            SentencesByParagraphId[key] = value;
            return value;
        }
        public static async Task<List<Sentence>> SentencesByParagraphIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<Sentence>>.Run(() =>
            {
                return SentencesByParagraphIdRead(key, dbContextFactory);
            });
        }


        #endregion

        #region create


        public static Sentence? SentenceCreate(Sentence sentence, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Sentence]
                      ([ParagraphId]
                      ,[Ordinal]
                      ,[Text]
                      ,[Id])
                VALUES
                      ({sentence.ParagraphId}
                      ,{sentence.Ordinal}
                      ,{sentence.Text}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Sentence affected 0 rows");
            var newEntity = context.Sentences.Where(x => x.Id == guid).FirstOrDefault();
            if (newEntity is null)
            {
                throw new InvalidDataException("newEntity is null in SentenceCreate");
            }


            // add it to cache
            SentenceById[(Guid)newEntity.Id] = newEntity;

            return newEntity;
        }
        public static async Task<Sentence?> SentenceCreateAsync(Sentence value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return SentenceCreate(value, dbContextFactory); });
        }

        #endregion
    }
}
