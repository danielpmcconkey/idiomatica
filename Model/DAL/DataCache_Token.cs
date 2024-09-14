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
        private static ConcurrentDictionary<Guid, Token> TokenById = new ConcurrentDictionary<Guid, Token>(); 
        private static ConcurrentDictionary<Guid, List<Token>> TokensByPageId = new ConcurrentDictionary<Guid, List<Token>>();
        private static ConcurrentDictionary<Guid, List<Token>> TokensBySentenceId = new ConcurrentDictionary<Guid, List<Token>>();
        private static ConcurrentDictionary<Guid, List<Token>> TokensAndWordsBySentenceId = new ConcurrentDictionary<Guid, List<Token>>();

        #region create

        public static Token? TokenCreate(Token token, IdiomaticaContext context)
        {
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Token]
                      ([WordKey]
                      ,[SentenceKey]
                      ,[Display]
                      ,[Ordinal]
                      ,[UniqueKey])
                VALUES
                      ({token.WordKey}
                      ,{token.SentenceKey}
                      ,{token.Display}
                      ,{token.Ordinal}
                      ,{token.UniqueKey})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Token affected 0 rows");
            
            // add it to cache
            TokenById[token.UniqueKey] = token;

            return token;
        }
        public static async Task<Token?> TokenCreateAsync(Token value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return TokenCreate(value, context); });
        }
        #endregion

        #region read
        public static Token? TokenByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (TokenById.ContainsKey(key))
            {
                return TokenById[key];
            }

            // read DB
            var value = context.Tokens.Where(x => x.UniqueKey == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            TokenById[key] = value;
            return value;
        }
        public static async Task<Token?> TokenByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<Token?>.Run(() =>
            {
                return TokenByIdRead(key, context);
            });
        }
        public static List<Token> TokensByPageIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (TokensByPageId.ContainsKey(key))
            {
                return TokensByPageId[key];
            }
            // read DB
            var groups = (from p in context.Pages
                          join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                          join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                          join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                          where (p.UniqueKey == key)
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
            Guid key, IdiomaticaContext context)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensByPageIdRead(key, context);
            });
        }
        public static List<Token> TokensBySentenceIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (TokensBySentenceId.ContainsKey(key))
            {
                return TokensBySentenceId[key];
            }

            // read DB
            var value = context.Tokens
                .Where(x => x.SentenceKey == key)
                .OrderBy(x => x.Ordinal)
                .ToList();
            // write to cache
            TokensBySentenceId[key] = value;
            return value;
        }
        public static async Task<List<Token>> TokensBySentenceIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensBySentenceIdRead(key, context);
            });
        }
        public static List<Token> TokensAndWordsBySentenceIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (TokensAndWordsBySentenceId.ContainsKey(key))
            {
                return TokensAndWordsBySentenceId[key];
            }

            // read DB
            var value = context.Tokens
                .Include(x => x.Word)
                .Where(x => x.SentenceKey == key)
                .OrderBy(x => x.Ordinal)
                .ToList();
            // write to cache
            TokensAndWordsBySentenceId[key] = value;
            TokensBySentenceId[key] = value;
            foreach (var t in value)
            {
                TokenById[(Guid)t.UniqueKey] = t;
            }
            return value;
        }
        public static async Task<List<Token>> TokensAndWordsBySentenceIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensAndWordsBySentenceIdRead(key, context);
            });
        }
        #endregion

        #region delete
        public static void TokenBySentenceIdDelete(Guid key, IdiomaticaContext context)
        {
            var existingList = context.Tokens.Where(x => x.SentenceKey == key);
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
                      WHERE SentenceKey = {key}
                """);
        }
        public static async Task TokenBySentenceIdDeleteAsync(Guid key, IdiomaticaContext context)
        {
            await Task.Run(() =>
            {
                TokenBySentenceIdDelete(key, context);
            });
        }

        #endregion
    }
}
