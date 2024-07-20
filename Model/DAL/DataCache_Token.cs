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
        private static ConcurrentDictionary<int, Token> TokenById = new ConcurrentDictionary<int, Token>(); 
        private static ConcurrentDictionary<int, List<Token>> TokensByPageId = new ConcurrentDictionary<int, List<Token>>();
        private static ConcurrentDictionary<int, List<Token>> TokensBySentenceId = new ConcurrentDictionary<int, List<Token>>();
        private static ConcurrentDictionary<int, List<Token>> TokensAndWordsBySentenceId = new ConcurrentDictionary<int, List<Token>>();

        #region create

        public static Token? TokenCreate(Token token, IdiomaticaContext context)
        {
            if (token.WordId is null) throw new ArgumentNullException(nameof(token.WordId));
            if (token.SentenceId is null) throw new ArgumentNullException(nameof(token.SentenceId));
            if (token.Ordinal is null) throw new ArgumentNullException(nameof(token.Ordinal));


            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Token]
                      ([WordId]
                      ,[SentenceId]
                      ,[Display]
                      ,[Ordinal]
                      ,[UniqueKey])
                VALUES
                      ({token.WordId}
                      ,{token.SentenceId}
                      ,{token.Display}
                      ,{token.Ordinal}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Token affected 0 rows");
            var newEntity = context.Tokens.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in TokenCreate");
            }


            // add it to cache
            TokenById[(int)newEntity.Id] = newEntity;

            return newEntity;
        }
        public static async Task<Token?> TokenCreateAsync(Token value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return TokenCreate(value, context); });
        }
        #endregion

        #region read
        public static async Task<Token?> TokenByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (TokenById.ContainsKey(key))
            {
                return TokenById[key];
            }

            // read DB
            var value = context.Tokens.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            TokenById[key] = value;
            return value;
        }
        public static List<Token> TokensByPageIdRead(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (TokensByPageId.ContainsKey(key))
            {
                return TokensByPageId[key];
            }
            // read DB
            var groups = (from p in context.Pages
                          join pp in context.Paragraphs on p.Id equals pp.PageId
                          join s in context.Sentences on pp.Id equals s.ParagraphId
                          join t in context.Tokens on s.Id equals t.SentenceId
                          where (p.Id == key)
                          group t by t into g
                          select new { token = g.Key });
            List<Token> value = new List<Token>();
            foreach (var g in groups)
            {
                value.Add(g.token);
            }

            // write to cache
            TokensByPageId[key] = value;
            return value;
        }
        public static async Task<List<Token>> TokensByPageIdReadAsync(
            int key, IdiomaticaContext context)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensByPageIdRead(key, context);
            });
        }
        public static List<Token> TokensBySentenceIdRead(int key, IdiomaticaContext context)
        {
            // check cache
            if (TokensBySentenceId.ContainsKey(key))
            {
                return TokensBySentenceId[key];
            }

            // read DB
            var value = context.Tokens
                .Where(x => x.SentenceId == key)
                .OrderBy(x => x.Ordinal)
                .ToList();
            // write to cache
            TokensBySentenceId[key] = value;
            return value;
        }
        public static async Task<List<Token>> TokensBySentenceIdReadAsync(int key, IdiomaticaContext context)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensBySentenceIdRead(key, context);
            });
        }
        public static async Task<List<Token>> TokensAndWordsBySentenceIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (TokensAndWordsBySentenceId.ContainsKey(key))
            {
                return TokensAndWordsBySentenceId[key];
            }

            // read DB
            var value = await context.Tokens
                .Include(x => x.Word)
                .Where(x => x.SentenceId == key)
                .OrderBy(x => x.Ordinal)
                .ToListAsync();
            // write to cache
            TokensAndWordsBySentenceId[key] = value;
            TokensBySentenceId[key] = value;
            foreach(var t in value)
            {
                if(t.Id is null || t.Id < 1) continue;
                TokenById[(int)t.Id] = t;
            }
            return value;
        }
        #endregion

        #region delete
        public static void TokenBySentenceIdDelete(int key, IdiomaticaContext context)
        {
            var existingList = context.Tokens.Where(x => x.SentenceId == key);
            foreach (var existingItem in existingList)
            {
                context.Tokens.Remove(existingItem);
            }
            if (TokensBySentenceId.ContainsKey(key))
            {
                if (!TokensBySentenceId.TryRemove(key, out var value))
                {
                    throw new InvalidDataException($"Failed to remove TokenBySentenceId from cache where key = {key}");
                }
            }

            // now take them out of the DB
            context.Database.ExecuteSql($"""
                DELETE FROM [Idioma].[Token]
                      WHERE SentenceId = {key}
                """);
        }
        public static async Task TokenBySentenceIdDeleteAsync(int key, IdiomaticaContext context)
        {
            await Task.Run(() =>
            {
                TokenBySentenceIdDelete(key, context);
            });
        }

        #endregion
    }
}
