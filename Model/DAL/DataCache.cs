using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static class DataCache
    {
        
        private static ConcurrentDictionary<int, Book> BookById = new ConcurrentDictionary<int, Book>();
        private static ConcurrentDictionary<int, List<BookListRow>> BookListRowsByUserId = new ConcurrentDictionary<int, List<BookListRow>>();
        private static ConcurrentDictionary<(int bookId, AvailableBookStat statKey), BookStat> BookStatByBookIdAndStatKey = new ConcurrentDictionary<(int bookId, AvailableBookStat statKey), BookStat>();
        private static ConcurrentDictionary<(int userId, int bookId), BookUser> BookUserByUserIdAndBookId = new ConcurrentDictionary<(int userId, int bookId), BookUser>();
        private static ConcurrentDictionary<(int bookId, int userId), List<BookUserStat>> BookUserStatsByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), List<BookUserStat>>();
        private static ConcurrentDictionary<string, Language> LanguageByCode = new ConcurrentDictionary<string, Language>();
        private static ConcurrentDictionary<int, Language> LanguageById = new ConcurrentDictionary<int, Language>();
        private static ConcurrentDictionary<string, LanguageCode> LanguageCodeByCode = new ConcurrentDictionary<string, LanguageCode>();
        private static ConcurrentDictionary<(int languageId, int userId), LanguageUser> LanguageUserByLanguageIdAndUserId = new ConcurrentDictionary<(int languageId, int userId), LanguageUser>();
        private static ConcurrentDictionary<int, Page> PageById = new ConcurrentDictionary<int, Page>(); 
        private static ConcurrentDictionary<(int ordinal, int bookId), Page> PageByOrdinalAndBookId = new ConcurrentDictionary<(int ordinal, int bookId), Page>();
        private static ConcurrentDictionary<int, PageUser> PageUserById = new ConcurrentDictionary<int, PageUser>();
        private static ConcurrentDictionary<(int pageId, int languageUserId), PageUser> PageUserByPageIdAndLanguageUserId = new ConcurrentDictionary<(int pageId, int languageUserId), PageUser>();
        private static ConcurrentDictionary<(int languageUserId, int ordinal, int bookId), PageUser> PageUserByLanguageUserIdOrdinalAndBookId = new ConcurrentDictionary<(int languageUserId, int ordinal, int bookId), PageUser>();
        private static ConcurrentDictionary<int, Paragraph> ParagraphById = new ConcurrentDictionary<int, Paragraph>();
        private static ConcurrentDictionary<int, List<Paragraph>> ParagraphsByPageId = new ConcurrentDictionary<int, List<Paragraph>>();
        private static ConcurrentDictionary<int, List<Sentence>> SentencesByPageId = new ConcurrentDictionary<int, List<Sentence>>();
        private static ConcurrentDictionary<int, List<Sentence>> SentencesByParagraphId = new ConcurrentDictionary<int, List<Sentence>>();
        private static ConcurrentDictionary<int, List<Token>> TokensByPageId = new ConcurrentDictionary<int, List<Token>>();
        private static ConcurrentDictionary<int, List<Token>> TokensBySentenceId = new ConcurrentDictionary<int, List<Token>>();
        private static ConcurrentDictionary<string, User> UserByApplicationUserId = new ConcurrentDictionary<string, User>();
        private static ConcurrentDictionary<int, List<Word>> WordsCommon1000ByLanguageId = new ConcurrentDictionary<int, List<Word>>();
        private static ConcurrentDictionary<int, Word> WordById = new ConcurrentDictionary<int, Word>();
        private static ConcurrentDictionary<int, Dictionary<string, Word>> WordsDictByBookId = new ConcurrentDictionary<int, Dictionary<string, Word>>();
        private static ConcurrentDictionary<int, Dictionary<string, Word>> WordsDictByPageId = new ConcurrentDictionary<int, Dictionary<string, Word>>();
        private static ConcurrentDictionary<(int bookId, int userId), Dictionary<string, WordUser>> WordUsersDictByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), Dictionary<string, WordUser>>();
        private static ConcurrentDictionary<(int pageId, int userId), Dictionary<string, WordUser>> WordUsersDictByPageIdAndUserId = new ConcurrentDictionary<(int pageId, int userId), Dictionary<string, WordUser>>();
        private static ConcurrentDictionary<int, WordUser> WordUserById = new ConcurrentDictionary<int, WordUser>();
        private static ConcurrentDictionary<(int userId, int languageId), List<WordUser>> WordUsersByUserIdAndLanguageId = new ConcurrentDictionary<(int userId, int languageId), List<WordUser>>();


        
        public static async Task<Book> BookByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (BookById.ContainsKey(key))
            {
                return BookById[key];
            }

            // read DB
            var value = context.Books.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookById[key] = value;
            return value;
        }
        public static async Task<List<BookListRow>> BookListRowsByUserIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (BookListRowsByUserId.ContainsKey(key))
            {
                return BookListRowsByUserId[key];
            }
            // read DB
            var value = context.BookListRows.Where(x => x.UserId == key)
                .ToList();

            // write to cache
            BookListRowsByUserId[key] = value;
            return value;
        }
        public static async Task<BookStat?> BookStatByBookIdAndStatKeyReadAsync(
            (int bookId, AvailableBookStat statKey) key, IdiomaticaContext context)
        {
            // check cache
            if (BookStatByBookIdAndStatKey.ContainsKey(key))
            {
                return BookStatByBookIdAndStatKey[key];
            }
            // read DB
            var value = context.BookStats
                .Where(x => x.BookId == key.bookId && x.Key == key.statKey)
                .FirstOrDefault();
            if(value == null) return null;
            // write to cache
            BookStatByBookIdAndStatKey[key] = value;
            return value;
        }
        public static async Task<BookUser> BookUserByUserIdAndBookIdReadAsync(
            (int userId, int bookId) key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserByUserIdAndBookId.ContainsKey(key))
            {
                return BookUserByUserIdAndBookId[key];
            }

            // read DB
            var value = context.BookUsers
                .Where(x => x.LanguageUser.UserId == key.userId && x.BookId == key.bookId)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookUserByUserIdAndBookId[key] = value;
            return value;
        }
        public static async Task<List<BookUserStat>> BookUserStatsByBookIdAndUserIdReadAsync(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserStatsByBookIdAndUserId.ContainsKey(key))
            {
                return BookUserStatsByBookIdAndUserId[key];
            }

            // read DB
            var value = context.BookUserStats
                .Where(x => x.LanguageUser.UserId == key.userId && x.BookId == key.bookId)
                .ToList();
            // write to cache
            BookUserStatsByBookIdAndUserId[key] = value;
            return value;
        }
        public static async Task<LanguageCode?> LanguageCodeByCodeReadAsync(string key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageCodeByCode.ContainsKey(key))
            {
                return LanguageCodeByCode[key];
            }
            // read DB
            var value = context.LanguageCodes
                .Where(lc => lc.Code == key)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            LanguageCodeByCode[key] = value;
            return value;
        }
        public static async Task<Language?> LanguageByCodeReadAsync(string key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageByCode.ContainsKey(key))
            {
                return LanguageByCode[key];
            }
            // read DB
            var value = context.Languages
                .Where(l => l.Code == key)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            LanguageByCode[key] = value;
            LanguageById[(int)value.Id] = value;
            return value;
        }
        public static async Task<Language?> LanguageByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageById.ContainsKey(key))
            {
                return LanguageById[key];
            }
            // read DB
            var value = context.Languages
                .Where(l => l.Id == key)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            LanguageById[key] = value;
            return value;
        }
        public static async Task<LanguageUser> LanguageUserByLanguageIdAndUserIdReadAsync(
            (int languageId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageUserByLanguageIdAndUserId.ContainsKey(key))
            {
                return LanguageUserByLanguageIdAndUserId[key];
            }

            // read DB
            var value = context.LanguageUsers
                .Where(x => x.LanguageId == key.languageId && x.UserId == key.userId)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            LanguageUserByLanguageIdAndUserId[key] = value;
            return value;
        }
        public static async Task<Page> PageByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (PageById.ContainsKey(key))
            {
                return PageById[key];
            }

            // read DB
            var value = context.Pages.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            PageById[key] = value;
            return value;
        }
        public static async Task<Page?> PageByOrdinalAndBookIdReadAsync(
            (int ordinal, int bookId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageByOrdinalAndBookId.ContainsKey(key))
            {
                return PageByOrdinalAndBookId[key];
            }
            // read DB
            var value = context.Pages
                .Where(p => p.Ordinal == key.ordinal
                    && p.BookId == key.bookId)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            PageByOrdinalAndBookId[key] = value;
            PageById[(int)value.Id] = value;
            return value;
        }
        public static async Task<PageUser> PageUserByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserById.ContainsKey(key))
            {
                return PageUserById[key];
            }

            // read DB
            var value = context.PageUsers.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            PageUserById[key] = value;
            return value;
        }
        public static async Task<PageUser?> PageUserByLanguageUserIdOrdinalAndBookIdReadAsync(
            (int languageUserId, int ordinal, int bookId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserByLanguageUserIdOrdinalAndBookId.ContainsKey(key))
            {
                return PageUserByLanguageUserIdOrdinalAndBookId[key];
            }
            // read DB
            var value = context.PageUsers
                .Where(pu => pu.BookUser.LanguageUserId == key.languageUserId
                    && pu.Page.Ordinal == key.ordinal
                    && pu.Page.BookId == key.bookId)
                .FirstOrDefault();

            if (value == null) return null;

            // write to cache
            PageUserByLanguageUserIdOrdinalAndBookId[key] = value;
            PageUserById[(int)value.Id] = value;
            return value;
        }
        public static async Task<PageUser?> PageUserByPageIdAndLanguageUserIdReadAsync(
            (int pageId, int languageUserId) key, IdiomaticaContext context)
        {
            // check cache
            if (PageUserByPageIdAndLanguageUserId.ContainsKey(key))
            {
                return PageUserByPageIdAndLanguageUserId[key];
            }
            // read DB
            var value = context.PageUsers
                .Where(pu => pu.BookUser.LanguageUserId == key.languageUserId
                    && pu.PageId == key.pageId)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            PageUserByPageIdAndLanguageUserId[key] = value;
            PageUserById[(int)value.Id] = value;
            return value;
        }
        public static async Task<Paragraph> ParagraphByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (ParagraphById.ContainsKey(key))
            {
                return ParagraphById[key];
            }

            // read DB
            var value = context.Paragraphs.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            ParagraphById[key] = value;
            return value;
        }
        public static async Task<List<Paragraph>> ParagraphsByPageIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (ParagraphsByPageId.ContainsKey(key))
            {
                return ParagraphsByPageId[key];
            }
            // read DB
            var value = context.Paragraphs.Where(x => x.PageId == key).OrderBy(x => x.Ordinal)
                .ToList();

            // write to cache
            ParagraphsByPageId[key] = value;
            // write each item to cache
            foreach ( var item in value ) { ParagraphById[(int)item.Id] = item; }
            
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
        public static async Task<User?> UserByApplicationUserIdReadAsync(string key, IdiomaticaContext context)
        {
            // check cache
            if (UserByApplicationUserId.ContainsKey(key))
            {
                return UserByApplicationUserId[key];
            }
            // read DB
            var value = context.Users
                .Where(u => u.ApplicationUserId == key)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            UserByApplicationUserId[key] = value;
            return value;
        }
        public static async Task<Word> WordByIdReadAsync(int key, IdiomaticaContext context)
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
                             select new { word = g.Key});
            var value = new Dictionary<string, Word>();
            foreach(var g in groups)
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
        public static async Task<WordUser> WordUserByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (WordUserById.ContainsKey(key))
            {
                return WordUserById[key];
            }

            // read DB
            var value = context.WordUsers.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            WordUserById[key] = value;
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
            foreach(var word in value) { WordById[(int)word.Id] = word; }
            return value;
        }
        public static async Task<Dictionary<string, WordUser>> WordUsersDictByBookIdAndUserIdReadAsync(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (WordUsersDictByBookIdAndUserId.ContainsKey(key))
            {
                return WordUsersDictByBookIdAndUserId[key];
            }
            // read DB
            var groups = (from b in context.Books
                          join p in context.Pages on b.Id equals p.BookId
                          join pp in context.Paragraphs on p.Id equals pp.PageId
                          join s in context.Sentences on pp.Id equals s.ParagraphId
                          join t in context.Tokens on s.Id equals t.SentenceId
                          join w in context.Words on t.WordId equals w.Id
                          join wu in context.WordUsers on w.Id equals wu.Id
                          where (b.Id == key.bookId && wu.LanguageUser.UserId == key.userId)
                          select new { word = w, wordUser = wu }).Distinct()
                .ToList();
            var value = new Dictionary<string, WordUser>();
            foreach (var g in groups)
            {
                string wordText = g.word.TextLowerCase;// g.wordWordUser.word.TextLowerCase;
                WordUser wordUser = g.wordUser;
                int wordUserId = g.wordUser.Id;
                value[wordText] = wordUser;
                // add it to the worduser cache
                WordUserById[wordUserId] = wordUser;
            }

            // write to cache
            WordUsersDictByBookIdAndUserId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, WordUser>> WordUsersDictByPageIdAndUserIdReadAsync(
            (int pageId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (WordUsersDictByPageIdAndUserId.ContainsKey(key))
            {
                return WordUsersDictByPageIdAndUserId[key];
            }
            // read DB
            var groups = (from p in context.Pages
                          join pp in context.Paragraphs on p.Id equals pp.PageId
                          join s in context.Sentences on pp.Id equals s.ParagraphId
                          join t in context.Tokens on s.Id equals t.SentenceId
                          join w in context.Words on t.WordId equals w.Id
                          join wu in context.WordUsers on w.Id equals wu.Id
                          where (p.Id == key.pageId && wu.LanguageUser.UserId == key.userId)
                          select new { word = w, wordUser = wu }).Distinct()
                .ToList();
            var value = new Dictionary<string, WordUser>();
            foreach (var g in groups)
            {
                string wordText = g.word.TextLowerCase;
                WordUser wordUser = g.wordUser;
                int wordUserId = g.wordUser.Id;
                value[wordText] = wordUser;
                // add it to the worduser cache
                WordUserById[wordUserId] = wordUser;
            }

            // write to cache
            WordUsersDictByPageIdAndUserId[key] = value;
            return value;
        }
        public static async Task<List<WordUser>> WordUsersByUserIdAndLanguageIdReadAsync(
            (int userId, int languageId) key, IdiomaticaContext context)
        {
            // check cache
            if (WordUsersByUserIdAndLanguageId.ContainsKey(key))
            {
                return WordUsersByUserIdAndLanguageId[key];
            }

            // read DB
            var value = context.WordUsers
                .Where(x => x.LanguageUser.UserId == key.userId && x.LanguageUser.LanguageId == key.languageId)
                .ToList();
            // write to cache
            WordUsersByUserIdAndLanguageId[key] = value;
            // also write each worduser to cache
            foreach (var wordUser in value) { WordUserById[(int)wordUser.Id] = wordUser; }
            return value;
        }
    }
}
