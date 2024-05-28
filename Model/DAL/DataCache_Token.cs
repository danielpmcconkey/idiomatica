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
        public static async Task<List<Token>> TokensByPageIdReadAsync(
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
        public static async Task<List<Token>> TokensBySentenceIdReadAsync(int key, IdiomaticaContext context)
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
        #endregion

        #region delete
        public static async Task TokenBySentenceIdDelete(int key, IdiomaticaContext context)
        {
            var existingList = context.Tokens.Where(x => x.SentenceId == key);
            foreach (var existingItem in existingList)
            {
                context.Tokens.Remove(existingItem);
            }
            context.SaveChanges();
            if(!TokensBySentenceId.TryRemove(key, out var value))
            {
                throw new InvalidDataException($"Failed to remove TokenBySentenceId from cache where key = {key}");
            }
        }

        #endregion
        #region create
        public static async Task<bool> TokenCreateAsync(Token value, IdiomaticaContext context)
        {
            context.Tokens.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            TokenById[(int)value.Id] = value;
            return true;
        }
        #endregion
    }
}
