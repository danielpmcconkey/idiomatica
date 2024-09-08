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
        private static ConcurrentDictionary<Guid, List<Word>> WordsCommon1000ByLanguageId = new ConcurrentDictionary<Guid, List<Word>>();
        private static ConcurrentDictionary<Guid, Word?> WordById = new ConcurrentDictionary<Guid, Word?>();
        private static ConcurrentDictionary<(Guid languageId, string textLower), Word> WordByLanguageIdAndTextLower = new ConcurrentDictionary<(Guid languageId, string textLower), Word>();
        private static ConcurrentDictionary<Guid, List<Word>> WordsBySentenceId = new ConcurrentDictionary<Guid, List<Word>>();
        private static ConcurrentDictionary<Guid, List<Word>> WordsByBookId = new ConcurrentDictionary<Guid, List<Word>>();
        private static ConcurrentDictionary<Guid, Dictionary<string, Word>> WordsDictByBookId = new ConcurrentDictionary<Guid, Dictionary<string, Word>>();
        private static ConcurrentDictionary<Guid, Dictionary<string, Word>> WordsDictByPageId = new ConcurrentDictionary<Guid, Dictionary<string, Word>>();
        private static ConcurrentDictionary<Guid, List<Word>> WordsAndTokensAndSentencesAndParagraphsByWordId = new ConcurrentDictionary<Guid, List<Word>>();
        private static ConcurrentDictionary<(Guid pageId, Guid languageUserId), Dictionary<string, Word>> WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserId = [];
        private static ConcurrentDictionary<Guid, List<WordTranslation>> WordTranslationsByWordId = [];


        #region read
        public static  Word? WordByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordById.ContainsKey(key))
            {
                return WordById[key];
            }

            // read DB
            var value = context.Words.Where(x => x.UniqueKey == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            WordById[key] = value;
            if (value.Text is null || value.LanguageKey is null) return value;
            WordByLanguageIdAndTextLower[((Guid)value.LanguageKey, value.Text)] = value;
            return value;
        }
        public static async Task<Word?> WordByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordByIdRead(key, context);
            });
        }        

        
        public static Word? WordByLanguageIdAndTextLowerRead((Guid languageId, string textLower) key, IdiomaticaContext context)
        {
            // check cache
            if (WordByLanguageIdAndTextLower.ContainsKey(key))
            {
                return WordByLanguageIdAndTextLower[key];
            }

            // read DB
            var value = context.Words
                .Where(x => x.LanguageKey == key.languageId && x.TextLowerCase == key.textLower)
                .FirstOrDefault();
            if (value == null || value.UniqueKey is null) return null;
            // write to cache
            WordByLanguageIdAndTextLower[key] = value;
            WordById[(Guid)value.UniqueKey] = value;
            return value;
        }
        public static List<Word> WordsBySentenceIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordsBySentenceId.ContainsKey(key))
            {
                return WordsBySentenceId[key];
            }
            // read DB
            var value = (from s in context.Sentences
                          join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                          join w in context.Words on t.WordKey equals w.UniqueKey
                          where (s.UniqueKey == key)
                          select w).ToList();
            // write to cache
            WordsBySentenceId[key] = value;
            return value;
        }
        public static List<Word> WordsByBookIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordsByBookId.ContainsKey(key))
            {
                return WordsByBookId[key];
            }
            // read DB
            var value = (from b in context.Books
                         join p in context.Pages on b.UniqueKey equals p.BookKey
                         join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                         join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                         join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                         join w in context.Words on t.WordKey equals w.UniqueKey
                         where (b.UniqueKey == key)
                         select w).Distinct().ToList();


            // write to cache
            WordsByBookId[key] = value;
            return value;
        }
        public static async Task<List<Word>> WordsByBookIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<List<Word>>.Run(() =>
            {
                return WordsByBookIdRead(key, context);
            });
        }

        public static Dictionary<string, Word> WordsDictByBookIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordsDictByBookId.ContainsKey(key))
            {
                return WordsDictByBookId[key];
            }
            // read DB
            var groups = (from b in context.Books
                          join p in context.Pages on b.UniqueKey equals p.BookKey
                          join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                          join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                          join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                          join w in context.Words on t.WordKey equals w.UniqueKey
                          where (b.UniqueKey == key)
                          group w by w into g //new { w.UniqueKey, w.LanguageKey, w.Romanization, w.Text, w.TextLowerCase, w.TokenCount } into g
                          select new { word = g.Key });
            var value = new Dictionary<string, Word>();
            foreach (var g in groups)
            {
                if (g.word is null || g.word.TextLowerCase is null || g.word.UniqueKey is null) continue;
                value[g.word.TextLowerCase] = g.word;
                // add it to the word cache
                WordById[(Guid)g.word.UniqueKey] = g.word;
            }

            // write to cache
            WordsDictByBookId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, Word>> WordsDictByBookIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<Dictionary<string, Word>>.Run(() =>
            {
                return WordsDictByBookIdRead(key, context);
            });
        }

        public static Dictionary<string, Word> WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
            (Guid pageId, Guid languageUserId) key, IdiomaticaContext context)
        {
            // check cache
            if (WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserId.ContainsKey(key))
            {
                return WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserId[key];
            }
            // read DB
            var groups = (from p in context.Pages
                          join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                          join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                          join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                          join w in context.Words on t.WordKey equals w.UniqueKey
                          join wu in context.WordUsers on w.UniqueKey equals wu.WordKey into grouping1
                          from wordUsers in grouping1.DefaultIfEmpty()
                          join wt in context.WordTranslations on w.UniqueKey equals wt.WordKey into grouping2
                          from wordTranslations in grouping2.DefaultIfEmpty()
                          where (p.UniqueKey == key.pageId && 
                            (wordUsers == null || wordUsers.LanguageUserKey == key.languageUserId))
                          select new { w, wordUsers, wordTranslations }
                          );
            var value = new Dictionary<string, Word>();
            foreach (var g in groups)
            {
                if (g.w is null || g.w.UniqueKey is null || g.w.TextLowerCase is null) continue;
                value[g.w.TextLowerCase] = g.w;
                // add it to the word cache
                WordById[(Guid)g.w.UniqueKey] = g.w;
            }

            // write to cache
            WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserId[key] = value;
            return value;
        }
        public static Dictionary<string, Word> WordsDictByPageIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordsDictByPageId.ContainsKey(key))
            {
                return WordsDictByPageId[key];
            }
            // read DB
            var groups = (from p in context.Pages
                          join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                          join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                          join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                          join w in context.Words on t.WordKey equals w.UniqueKey
                          where (p.UniqueKey == key)
                          group w by w into g
                          select new { word = g.Key });
            var value = new Dictionary<string, Word>();
            foreach (var g in groups)
            {
                if (g.word is null || g.word.UniqueKey is null || g.word.TextLowerCase is null) continue;
                value[g.word.TextLowerCase] = g.word;
                // add it to the word cache
                WordById[(Guid)g.word.UniqueKey] = g.word;
            }

            // write to cache
            WordsDictByPageId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, Word>> WordsDictByPageIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<Dictionary<string, Word>>.Run(() =>
            {
                return WordsDictByPageIdRead(key, context);
            });
        }


        public static List<Word> WordsCommon1000ByLanguageIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordsCommon1000ByLanguageId.ContainsKey(key))
            {
                return WordsCommon1000ByLanguageId[key];
            }
            // read DB
            var value = context.Words.FromSql($"""
                SELECT TOP (1000) 
                	    w.UniqueKey
                      , LanguageKey
                      , Text
                      , TextLowerCase
                      , Romanization
                      , TokenCount
                	  , count(t.UniqueKey) as numberOfUsages
                from Idioma.Word w
                join Idioma.Token t on w.UniqueKey = t.WordKey
                where w.LanguageKey = {key}
                group by 
                	    w.UniqueKey
                      , LanguageKey
                      , Text
                      , TextLowerCase
                      , Romanization
                      , TokenCount
                order by count(t.UniqueKey) desc
                """)
                .ToList();

            // write the list to cache
            WordsCommon1000ByLanguageId[key] = value;
            // also write each word to cache
            foreach (var word in value)
            {
                if (word is null || word.UniqueKey is null) continue;
                WordById[(Guid)word.UniqueKey] = word;
            }
            return value;
        }


        public static List<Word> WordsAndTokensAndSentencesAndParagraphsByWordIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordsAndTokensAndSentencesAndParagraphsByWordId.ContainsKey(key))
            {
                return WordsAndTokensAndSentencesAndParagraphsByWordId[key];
            }

            // read DB
            var value = context.Words
                .Where(w => w.UniqueKey == key)
                .Include(w => w.Tokens)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    .ThenInclude(t => t.Sentence)
                    .ThenInclude(s => s.Paragraph)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .ToList();
            if (value == null) return [];
            // write to cache
            WordsAndTokensAndSentencesAndParagraphsByWordId[key] = value;
            return value;
        }
        public static async Task<List<Word>> WordsAndTokensAndSentencesAndParagraphsByWordIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<List<Word>>.Run(() =>
            {
                return WordsAndTokensAndSentencesAndParagraphsByWordIdRead(key, context);
            });
        }

        
        public static List<WordTranslation> WordTranslationsByWordIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordTranslationsByWordId.ContainsKey(key))
            {
                return WordTranslationsByWordId[key];
            }

            // read DB
            var value = context.WordTranslations
                .Where(wt => wt.WordKey == key)
                .OrderBy(x => x.Ordinal)
                .Include(wt => wt.Verb)
                .ToList();
            if (value == null) return [];
            // write to cache
            WordTranslationsByWordId[key] = value;
            return value;
        }
        #endregion

        #region create


        public static Word? WordCreate(Word word, IdiomaticaContext context)
        {
            if (word.LanguageKey is null) throw new ArgumentNullException(nameof(word.LanguageKey));
            if (word.TextLowerCase is null) throw new ArgumentNullException(nameof(word.TextLowerCase));

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Word]
                           ([LanguageKey]
                           ,[Text]
                           ,[TextLowerCase]
                           ,[Romanization]
                           ,[TokenCount]
                           ,[UniqueKey])
                     VALUES
                           ({word.LanguageKey}
                           ,{word.Text}
                           ,{word.TextLowerCase}
                           ,{word.Romanization}
                           ,{word.TokenCount}
                           ,{guid})
                """);
            if (numRows < 1) throw new InvalidDataException("creating Word affected 0 rows");
            var newEntity = context.Words.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.UniqueKey is null)
            {
                throw new InvalidDataException("newEntity is null in WordCreate");
            }


            // add it to cache
            WordById[(Guid)newEntity.UniqueKey] = newEntity;

            return newEntity;
        }
        public static async Task<Word?> WordCreateAsync(Word value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return WordCreate(value, context); });
        }

        #endregion

        #region delete

        public static void WordDeleteById(Guid wordId, IdiomaticaContext context)
        {
            context.Database.ExecuteSql($"""
                        
                delete from [Idioma].[Word]
                where UniqueKey = {wordId}
                """);
            // delete it from cache
            WordById[wordId] = null;
        }

        #endregion

    }
}
