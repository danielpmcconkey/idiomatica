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
        

        #region read
        public static  Word? WordByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordById.ContainsKey(key))
            {
                return WordById[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Words.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            WordById[key] = value;
            WordByLanguageIdAndTextLower[((Guid)value.LanguageId, value.TextLowerCase)] = value;
            return value;
        }
        public static async Task<Word?> WordByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordByIdRead(key, dbContextFactory);
            });
        }        

        
        public static Word? WordByLanguageIdAndTextLowerRead((Guid languageId, string textLower) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordByLanguageIdAndTextLower.ContainsKey(key))
            {
                return WordByLanguageIdAndTextLower[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Words
                .Where(x => x.LanguageId == key.languageId && x.TextLowerCase == key.textLower)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            WordByLanguageIdAndTextLower[key] = value;
            WordById[(Guid)value.Id] = value;
            return value;
        }
        public static List<Word> WordsBySentenceIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordsBySentenceId.ContainsKey(key))
            {
                return WordsBySentenceId[key];
            }
            var context = dbContextFactory.CreateDbContext();

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
        public static List<Word> WordsByBookIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordsByBookId.ContainsKey(key))
            {
                return WordsByBookId[key];
            }
            var context = dbContextFactory.CreateDbContext();

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
        public static async Task<List<Word>> WordsByBookIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<Word>>.Run(() =>
            {
                return WordsByBookIdRead(key, dbContextFactory);
            });
        }

        public static Dictionary<string, Word> WordsDictByBookIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordsDictByBookId.ContainsKey(key))
            {
                return WordsDictByBookId[key];
            }
            var context = dbContextFactory.CreateDbContext();

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
                if (g.word is null || g.word.TextLowerCase is null) continue;
                value[g.word.TextLowerCase] = g.word;
                // add it to the word cache
                WordById[(Guid)g.word.Id] = g.word;
            }

            // write to cache
            WordsDictByBookId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, Word>> WordsDictByBookIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Dictionary<string, Word>>.Run(() =>
            {
                return WordsDictByBookIdRead(key, dbContextFactory);
            });
        }

        public static Dictionary<string, Word> WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
            (Guid pageId, Guid languageUserId) key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserId.ContainsKey(key))
            {
                return WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var groups = (from p in context.Pages
                          join pp in context.Paragraphs on p.Id equals pp.PageId
                          join s in context.Sentences on pp.Id equals s.ParagraphId
                          join t in context.Tokens on s.Id equals t.SentenceId
                          join w in context.Words on t.WordId equals w.Id
                          join wu in context.WordUsers on w.Id equals wu.WordId into grouping1
                          from wordUsers in grouping1.DefaultIfEmpty()
                          join wt in context.WordTranslations on w.Id equals wt.WordId into grouping2
                          from wordTranslations in grouping2.DefaultIfEmpty()
                          where (p.Id == key.pageId && 
                            (wordUsers == null || wordUsers.LanguageUserId == key.languageUserId))
                          select new { w, wordUsers, wordTranslations }
                          );
            var value = new Dictionary<string, Word>();
            foreach (var g in groups)
            {
                if (g.w is null || g.w.TextLowerCase is null) continue;
                value[g.w.TextLowerCase] = g.w;
                // add it to the word cache
                WordById[(Guid)g.w.Id] = g.w;
            }

            // write to cache
            WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserId[key] = value;
            return value;
        }
        public static Dictionary<string, Word> WordsDictByPageIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordsDictByPageId.ContainsKey(key))
            {
                return WordsDictByPageId[key];
            }
            var context = dbContextFactory.CreateDbContext();

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
                if (g.word is null || g.word.TextLowerCase is null) continue;
                value[g.word.TextLowerCase] = g.word;
                // add it to the word cache
                WordById[(Guid)g.word.Id] = g.word;
            }

            // write to cache
            WordsDictByPageId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, Word>> WordsDictByPageIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Dictionary<string, Word>>.Run(() =>
            {
                return WordsDictByPageIdRead(key, dbContextFactory);
            });
        }


        public static List<Word> WordsCommon1000ByLanguageIdReadAsync(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordsCommon1000ByLanguageId.ContainsKey(key))
            {
                return WordsCommon1000ByLanguageId[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = context.Words.FromSql($"""
                SELECT TOP (1000) 
                	    w.Id
                      , LanguageId
                      , Text
                      , TextLowerCase
                      , Romanization
                	  , count(t.Id) as numberOfUsages
                from Idioma.Word w
                join Idioma.Token t on w.Id = t.WordId
                where w.LanguageId = {key}
                group by 
                	    w.Id
                      , LanguageId
                      , Text
                      , TextLowerCase
                      , Romanization
                order by count(t.Id) desc
                """)
                .ToList();

            // write the list to cache
            WordsCommon1000ByLanguageId[key] = value;
            // also write each word to cache
            foreach (var word in value)
            {
                if (word is null) continue;
                WordById[(Guid)word.Id] = word;
            }
            return value;
        }


        public static List<Word> WordsAndTokensAndSentencesAndParagraphsByWordIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordsAndTokensAndSentencesAndParagraphsByWordId.ContainsKey(key))
            {
                return WordsAndTokensAndSentencesAndParagraphsByWordId[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.Words
                .Where(w => w.Id == key)
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
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<List<Word>>.Run(() =>
            {
                return WordsAndTokensAndSentencesAndParagraphsByWordIdRead(key, dbContextFactory);
            });
        }

        
        public static List<WordTranslation> WordTranslationsByWordIdRead(
            Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (WordTranslationsByWordId.ContainsKey(key))
            {
                return WordTranslationsByWordId[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.WordTranslations
                .Where(wt => wt.WordId == key)
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


        public static Word? WordCreate(Word word, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[Word]
                           ([LanguageId]
                           ,[Text]
                           ,[TextLowerCase]
                           ,[Romanization]
                           ,[Id])
                     VALUES
                           ({word.LanguageId}
                           ,{word.Text}
                           ,{word.TextLowerCase}
                           ,{word.Romanization}
                           ,{word.Id})
                """);
            if (numRows < 1) throw new InvalidDataException("creating Word affected 0 rows");
            
            // add it to cache
            WordById[word.Id] = word;

            return word;
        }
        public static async Task<Word?> WordCreateAsync(Word value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return WordCreate(value, dbContextFactory); });
        }

        #endregion

        #region delete

        public static void WordDeleteById(Guid wordId, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            context.Database.ExecuteSql($"""
                        
                delete from [Idioma].[Word]
                where Id = {wordId}
                """);
            // delete it from cache
            WordById[wordId] = null;
        }

        #endregion

    }
}
