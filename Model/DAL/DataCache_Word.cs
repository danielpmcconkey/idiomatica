﻿using Microsoft.EntityFrameworkCore;
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
        private static ConcurrentDictionary<int, List<Word>> WordsCommon1000ByLanguageId = new ConcurrentDictionary<int, List<Word>>();
        private static ConcurrentDictionary<int, Word> WordById = new ConcurrentDictionary<int, Word>();
        private static ConcurrentDictionary<(int languageId, string textLower), Word> WordByLanguageIdAndTextLower = new ConcurrentDictionary<(int languageId, string textLower), Word>();
        private static ConcurrentDictionary<int, List<Word>> WordsBySentenceId = new ConcurrentDictionary<int, List<Word>>();
        private static ConcurrentDictionary<int, List<Word>> WordsByBookId = new ConcurrentDictionary<int, List<Word>>();
        private static ConcurrentDictionary<int, Dictionary<string, Word>> WordsDictByBookId = new ConcurrentDictionary<int, Dictionary<string, Word>>();
        private static ConcurrentDictionary<int, Dictionary<string, Word>> WordsDictByPageId = new ConcurrentDictionary<int, Dictionary<string, Word>>();

        #region read
        public static async Task<Word?> WordByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (WordById.ContainsKey(key))
            {
                return WordById[key];
            }

            // read DB
            var value = context.Words.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            WordById[key] = value;
            return value;
        }
        public static async Task<Word?> WordByLanguageIdAndTextLowerReadAsync((int languageId, string textLower) key, IdiomaticaContext context)
        {
            // check cache
            if (WordByLanguageIdAndTextLower.ContainsKey(key))
            {
                return WordByLanguageIdAndTextLower[key];
            }

            // read DB
            var value = context.Words
                .Where(x => x.LanguageId == key.languageId && x.TextLowerCase == key.textLower)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            WordByLanguageIdAndTextLower[key] = value;
            WordById[(int)value.Id] = value;
            return value;
        }
        public static async Task<List<Word>> WordsBySentenceIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (WordsBySentenceId.ContainsKey(key))
            {
                return WordsBySentenceId[key];
            }
            // read DB
            var value = (from s in context.Sentences
                          join t in context.Tokens on s.Id equals t.SentenceId
                          join w in context.Words on t.WordId equals w.Id
                          where (s.Id == key)
                          select w).ToList();
            // write to cache
            WordsBySentenceId[key] = value;
            return value;
        }
        public static async Task<List<Word>> WordsByBookIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (WordsByBookId.ContainsKey(key))
            {
                return WordsByBookId[key];
            }
            // read DB
            var value = (from b in context.Books
                         join p in context.Pages on b.Id equals p.BookId
                         join pp in context.Paragraphs on p.Id equals pp.PageId
                         join s in context.Sentences on pp.Id equals s.ParagraphId
                         join t in context.Tokens on s.Id equals t.SentenceId
                         join w in context.Words on t.WordId equals w.Id
                         where (b.Id == key)
                         select w).Distinct().ToList();
            
            
            // write to cache
            WordsByBookId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, Word>> WordsDictByBookIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (WordsDictByBookId.ContainsKey(key))
            {
                return WordsDictByBookId[key];
            }
            // read DB
            var groups = (from b in context.Books
                          join p in context.Pages on b.Id equals p.BookId
                          join pp in context.Paragraphs on p.Id equals pp.PageId
                          join s in context.Sentences on pp.Id equals s.ParagraphId
                          join t in context.Tokens on s.Id equals t.SentenceId
                          join w in context.Words on t.WordId equals w.Id
                          where (b.Id == key)
                          group w by w into g //new { w.Id, w.LanguageId, w.Romanization, w.Text, w.TextLowerCase, w.TokenCount } into g
                          select new { word = g.Key });
            var value = new Dictionary<string, Word>();
            foreach (var g in groups)
            {
                value[g.word.TextLowerCase] = g.word;
                // add it to the word cache
                WordById[(int)g.word.Id] = g.word;
            }

            // write to cache
            WordsDictByBookId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, Word>> WordsDictByPageIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (WordsDictByPageId.ContainsKey(key))
            {
                return WordsDictByPageId[key];
            }
            // read DB
            var groups = (from p in context.Pages
                          join pp in context.Paragraphs on p.Id equals pp.PageId
                          join s in context.Sentences on pp.Id equals s.ParagraphId
                          join t in context.Tokens on s.Id equals t.SentenceId
                          join w in context.Words on t.WordId equals w.Id
                          where (p.Id == key)
                          group w by w into g
                          select new { word = g.Key });
            var value = new Dictionary<string, Word>();
            foreach (var g in groups)
            {
                value[g.word.TextLowerCase] = g.word;
                // add it to the word cache
                WordById[(int)g.word.Id] = g.word;
            }

            // write to cache
            WordsDictByPageId[key] = value;
            return value;
        }
        
        public static async Task<List<Word>> WordsCommon1000ByLanguageIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (WordsCommon1000ByLanguageId.ContainsKey(key))
            {
                return WordsCommon1000ByLanguageId[key];
            }
            // read DB
            var value = context.Words.FromSql($"""
                SELECT TOP (1000) 
                	    w.Id
                      , LanguageId
                      , Text
                      , TextLowerCase
                      , Romanization
                      , TokenCount
                	  , count(t.Id) as numberOfUsages
                from Idiomatica.Idioma.Word w
                join Idiomatica.Idioma.Token t on w.Id = t.WordId
                where w.LanguageId = {key}
                group by 
                	    w.Id
                      , LanguageId
                      , Text
                      , TextLowerCase
                      , Romanization
                      , TokenCount
                order by count(t.Id) desc
                """)
                .ToList();

            // write the list to cache
            WordsCommon1000ByLanguageId[key] = value;
            // also write each word to cache
            foreach (var word in value) { WordById[(int)word.Id] = word; }
            return value;
        }
        #endregion

        #region create
        public static async Task<bool> WordCreateAsync(Word value, IdiomaticaContext context)
        {
            context.Words.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            WordById[(int)value.Id] = value;
            return true;
        }
        #endregion

    }
}
