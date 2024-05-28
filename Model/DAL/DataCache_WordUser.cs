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
        private static ConcurrentDictionary<int, WordUser> WordUserById = new ConcurrentDictionary<int, WordUser>();
        private static ConcurrentDictionary<(int wordId, int userId), WordUser> WordUserByWordIdAndUserId = new ConcurrentDictionary<(int wordId, int userId), WordUser>();
        private static ConcurrentDictionary<(int bookId, int userId), Dictionary<string, WordUser>> WordUsersDictByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), Dictionary<string, WordUser>>();
        private static ConcurrentDictionary<(int pageId, int userId), Dictionary<string, WordUser>> WordUsersDictByPageIdAndUserId = new ConcurrentDictionary<(int pageId, int userId), Dictionary<string, WordUser>>();
        private static ConcurrentDictionary<(int userId, int languageId), List<WordUser>> WordUsersByUserIdAndLanguageId = new ConcurrentDictionary<(int userId, int languageId), List<WordUser>>();


        #region create
        public static async Task<bool> WordUserCreateAsync(WordUser value, IdiomaticaContext context)
        {
            context.WordUsers.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            // add to id cache
            WordUserById[value.Id] = value;
            return true;
        }
        #endregion

        #region read
        public static async Task<WordUser?> WordUserByWordIdAndUserIdReadAsync(
            (int wordId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (WordUserByWordIdAndUserId.ContainsKey(key))
            {
                return WordUserByWordIdAndUserId[key];
            }

            // read DB
            var value = context.WordUsers
                .Where(x => x.WordId == key.wordId && x.LanguageUser.UserId == key.userId)
                .FirstOrDefault();
            if (value == null) { return null; }
            // write to cache
            WordUserByWordIdAndUserId[key] = value;
            // also write each worduser to cache
            WordUserById[(int)value.Id] = value;
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
        #endregion

        #region update
        public static async Task WordUserUpdateAsync(WordUser value, IdiomaticaContext context)
        {
            if (value.Id == null || value.Id < 1) throw new ArgumentException("ID cannot be null or 0 when updating");

            var valueFromDb = context.WordUsers.Where(pu => pu.Id == value.Id).FirstOrDefault();
            if (valueFromDb == null) throw new InvalidDataException("Value does not exist in the DB to update");

            valueFromDb.Status = value.Status;
            valueFromDb.StatusChanged = value.StatusChanged;
            valueFromDb.WordId = value.WordId;
            valueFromDb.Created = value.Created;
            valueFromDb.LanguageUserId = value.LanguageUserId;
            valueFromDb.Translation = value.Translation;
            context.SaveChanges();

            // now update the cache
            WordUserById[(int)value.Id] = value;
            
            var cachedList1 = WordUserByWordIdAndUserId.Where(x => x.Value.Id == value.Id).ToList();
            foreach (var item in cachedList1) WordUserByWordIdAndUserId[item.Key] = value;

            return;
        }
        public static async Task WordUsersUpdateStatusByPageIdAndUserIdAndStatus(
            int pageId, int userId, AvailableWordUserStatus statusToEdit, 
            AvailableWordUserStatus newStatus, IdiomaticaContext context)
        {
            var wordUsers = (from pu in context.PageUsers
                             join p in context.Pages on pu.PageId equals p.Id
                             join pp in context.Paragraphs on p.Id equals pp.PageId
                             join s in context.Sentences on pp.Id equals s.ParagraphId
                             join t in context.Tokens on s.Id equals t.SentenceId
                             join w in context.Words on t.WordId equals w.Id
                             join wu in context.WordUsers on w.Id equals wu.WordId
                             where (pu.PageId == pageId && wu.LanguageUser.UserId == userId
                                 && wu.Status == statusToEdit)
                             select wu)
                             .ToList();
            foreach (var wordUser in wordUsers)
            {
                wordUser.Status = newStatus;
                // update cache
                var cachedList1 = WordUserById.Where(x => x.Value.Id == wordUser.Id).ToList();
                foreach (var item in cachedList1) WordUserById[item.Key] = wordUser;

                var cachedList2 = WordUserByWordIdAndUserId.Where(x => x.Value.Id == wordUser.Id).ToList();
                foreach (var item in cachedList2) WordUserByWordIdAndUserId[item.Key] = wordUser;

                var dictsByUser3 = WordUsersDictByBookIdAndUserId.Where(x => x.Key.userId == userId);
                foreach(var kvpIntIntDict in  dictsByUser3)
                {
                    var cacheList3 = kvpIntIntDict.Value.Where(x => x.Value.Id == wordUser.Id);
                    foreach (var kvpStringWordUser in cacheList3)
                    {
                        kvpStringWordUser.Value.Status = newStatus;
                    }
                }
                var dictsByUser4 = WordUsersDictByPageIdAndUserId.Where(x => x.Key.userId == userId);
                foreach (var kvpIntIntDict in dictsByUser4)
                {
                    var cacheList4 = kvpIntIntDict.Value.Where(x => x.Value.Id == wordUser.Id);
                    foreach (var kvpStringWordUser in cacheList4)
                    {
                        kvpStringWordUser.Value.Status = newStatus;
                    }
                }
                var dictsByUser5 = WordUsersByUserIdAndLanguageId.Where(x => x.Key.userId == userId);
                foreach (var kvpIntIntList in dictsByUser5)
                {
                    var cacheList5 = kvpIntIntList.Value.Where(x => x.Id == wordUser.Id);
                    foreach (var wu in cacheList5)
                    {
                        wu.Status = newStatus;
                    }
                }
            }
            await context.SaveChangesAsync();
        }
        
        #endregion

    }
}
