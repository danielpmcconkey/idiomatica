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
        
        #region create

        public static Token? TokenCreate(Token token, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Token]
                      ([WordId]
                      ,[SentenceId]
                      ,[Display]
                      ,[Ordinal]
                      ,[Id])
                VALUES
                      ({token.WordId}
                      ,{token.SentenceId}
                      ,{token.Display}
                      ,{token.Ordinal}
                      ,{token.Id})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating Token affected 0 rows");
            
            // add it to cache
            TokenById[token.Id] = token;

            return token;
        }
        public static async Task<Token?> TokenCreateAsync(Token value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return TokenCreate(value, dbContextFactory); });
        }
        #endregion

        #region read
        public static Token? TokenByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (TokenById.ContainsKey(key))
            {
                return TokenById[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Tokens.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            TokenById[key] = value;
            return value;
        }
        public static async Task<Token?> TokenByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Token?>.Run(() =>
            {
                return TokenByIdRead(key, dbContextFactory);
            });
        }
        public static List<Token> TokensByPageIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (TokensByPageId.ContainsKey(key))
            {
                return TokensByPageId[key];
            }
            var context = dbContextFactory.CreateDbContext();

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
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensByPageIdRead(key, dbContextFactory);
            });
        }
        public static List<Token> TokensBySentenceIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (TokensBySentenceId.ContainsKey(key))
            {
                return TokensBySentenceId[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Tokens
                .Where(x => x.SentenceId == key)
                .OrderBy(x => x.Ordinal)
                .ToList();
            // write to cache
            TokensBySentenceId[key] = value;
            return value;
        }
        public static async Task<List<Token>> TokensBySentenceIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensBySentenceIdRead(key, dbContextFactory);
            });
        }
        public static List<Token> TokensAndWordsBySentenceIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (TokensAndWordsBySentenceId.ContainsKey(key))
            {
                return TokensAndWordsBySentenceId[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Tokens
                .Include(x => x.Word)
                .Where(x => x.SentenceId == key)
                .OrderBy(x => x.Ordinal)
                .ToList();
            // write to cache
            TokensAndWordsBySentenceId[key] = value;
            TokensBySentenceId[key] = value;
            foreach (var t in value)
            {
                TokenById[(Guid)t.Id] = t;
            }
            return value;
        }
        public static async Task<List<Token>> TokensAndWordsBySentenceIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensAndWordsBySentenceIdRead(key, dbContextFactory);
            });
        }
        #endregion

        #region delete
        public static void TokenBySentenceIdDelete(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

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
        public static async Task TokenBySentenceIdDeleteAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            await Task.Run(() =>
            {
                TokenBySentenceIdDelete(key, dbContextFactory);
            });
        }

        #endregion
    }
}
