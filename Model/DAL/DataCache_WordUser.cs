using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private static ConcurrentDictionary<int, WordUser> WordUserAndLanguageUserAndLanguageById = new ConcurrentDictionary<int, WordUser>();
        private static ConcurrentDictionary<(int wordId, int userId), WordUser> WordUserByWordIdAndUserId = new ConcurrentDictionary<(int wordId, int userId), WordUser>();
        private static ConcurrentDictionary<(int bookId, int uselanguageUserIdrId), List<WordUser>> WordUsersByBookIdAndLanguageUserId = new ConcurrentDictionary<(int bookId, int languageUserId), List<WordUser>>();
        private static ConcurrentDictionary<(int bookId, int userId), Dictionary<string, WordUser>> WordUsersDictByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), Dictionary<string, WordUser>>();
        private static ConcurrentDictionary<(int pageId, int userId), Dictionary<string, WordUser>> WordUsersDictByPageIdAndUserId = new ConcurrentDictionary<(int pageId, int userId), Dictionary<string, WordUser>>();
        private static ConcurrentDictionary<(int userId, int languageId), List<WordUser>> WordUsersByUserIdAndLanguageId = new ConcurrentDictionary<(int userId, int languageId), List<WordUser>>();


        #region create
        public static async Task<bool> WordUserCreateAsync(WordUser value, IdiomaticaContext context)
        {
            context.WordUsers.Add(value);
            context.SaveChanges();
            if (value.Id == null || value.Id < 1)
            {
                return false;
            }
            // add to id cache
            WordUserById[(int)value.Id] = value;
            return true;
        }
        #endregion

        #region read
        public static async Task<WordUser> WordUserByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (WordUserById.ContainsKey(key))
            {
                return WordUserById[key];
            }

            // read DB
            var value = context.WordUsers.Where(x => x.Id == key).FirstOrDefault();
                
            if (value == null) return null;
            // write to cache
            WordUserById[key] = value;
            return value;
        }
        public static async Task<WordUser> WordUserAndLanguageUserAndLanguageByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (WordUserAndLanguageUserAndLanguageById.ContainsKey(key))
            {
                return WordUserById[key];
            }

            // read DB
            var value = context.WordUsers
                .Where(x => x.Id == key)
                .Include(x => x.LanguageUser).ThenInclude(x=> x.Language)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            WordUserAndLanguageUserAndLanguageById[key] = value;
            return value;
        }
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
        public static async Task<List<WordUser>> WordUsersByBookIdAndLanguageUserIdReadAsync(
           (int bookId, int languageUserId) key, IdiomaticaContext context)
        {
            // check cache
            if (WordUsersByBookIdAndLanguageUserId.ContainsKey(key))
            {
                return WordUsersByBookIdAndLanguageUserId[key];
            }
            // read DB
            var value = (from b in context.Books
                          join p in context.Pages on b.Id equals p.BookId
                          join pp in context.Paragraphs on p.Id equals pp.PageId
                          join s in context.Sentences on pp.Id equals s.ParagraphId
                          join t in context.Tokens on s.Id equals t.SentenceId
                          join w in context.Words on t.WordId equals w.Id
                          join wu in context.WordUsers on w.Id equals wu.Id
                          where (b.Id == key.bookId && wu.LanguageUserId == key.languageUserId)
                          select wu)
                .Distinct()
                .ToList();
            

            // write to cache
            WordUsersByBookIdAndLanguageUserId[key] = value;
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
                int wordUserId = (int)g.wordUser.Id;
                value[wordText] = wordUser;
                // add it to the worduser cache
                WordUserById[wordUserId] = wordUser;
            }

            // write to cache
            WordUsersDictByBookIdAndUserId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, WordUser>?> WordUsersDictByPageIdAndUserIdReadAsync(
            (int pageId, int userId) key, IdiomaticaContext context)
        {
            if (key.pageId < 1) return null;
            if (key.userId < 1) return null;

            
            // check cache
            if (WordUsersDictByPageIdAndUserId.ContainsKey(key))
            {
                return WordUsersDictByPageIdAndUserId[key];
            }

            // first pull the words for the page
            var wordsDict = await DataCache.WordsDictByPageIdReadAsync(key.pageId, context);
            if (wordsDict is null || wordsDict.Count < 1) return null;
            // read DB
            var groups = (
                from wd in wordsDict 
                join w in context.Words on wd.Key equals w.TextLowerCase
                join wu in context.WordUsers on w.Id equals wu.WordId into grouping
                from wu in grouping.DefaultIfEmpty()
                 select new { word = w, wordUser = wu }
                 ).Distinct()
                 .ToList();
            
            //var groups = (from p in context.Pages
            //              join pp in context.Paragraphs on p.Id equals pp.PageId
            //              join s in context.Sentences on pp.Id equals s.ParagraphId
            //              join t in context.Tokens on s.Id equals t.SentenceId
            //              join w in context.Words on t.WordId equals w.Id
            //              join wu in context.WordUsers on w.Id equals wu.WordId
            //              join lu in context.LanguageUsers on wu.LanguageUserId equals lu.Id
            //              where (p.Id == key.pageId && lu.UserId == key.userId)
            //              select new { word = w, wordUser = wu }).Distinct()
            //    .ToList();
            var value = new Dictionary<string, WordUser>();
            int languageUserId = -1;
            foreach (var g in groups)
            {
                if (g.word is null || g.word.Id is null || string.IsNullOrEmpty(g.word.TextLowerCase)) continue;
                string wordText = g.word.TextLowerCase;
                WordUser wordUser = new();
                if (g.wordUser is not null) wordUser = g.wordUser;
                // if the word user is null, create it
                if (g.wordUser is null)
                {
                    if (languageUserId < 1)
                    {
                        // see if any other word users from the group would work
                        var otherWordUser = groups.Where(x => x.wordUser is not null)
                            .Select(x => x.wordUser).FirstOrDefault();
                        if (otherWordUser is not null && otherWordUser.LanguageUserId is not null) 
                            languageUserId = (int)otherWordUser.LanguageUserId;
                        else
                        {
                            // gotta go the long way round
                            var language = (from p in context.Pages
                                            join b in context.Books on p.BookId equals b.Id
                                            join l in context.Languages on b.LanguageId equals l.Id
                                            select l).FirstOrDefault();
                            if (language is null || language.Id is null)
                            {
                                return null;
                            }
                            var languageUser = await DataCache.LanguageUserByLanguageIdAndUserIdReadAsync(
                                ((int)language.Id, key.userId), context);
                            if (languageUser is null || languageUser.Id is null || languageUser.Id < 1)
                            {
                                return null;
                            }
                            languageUserId = (int)languageUser.Id;
                        }
                    }
                    wordUser = new WordUser()
                    {
                        LanguageUserId = languageUserId,
                        WordId = g.word.Id,
                        Status = AvailableWordUserStatus.UNKNOWN,
                        Translation = string.Empty
                    };
                    var isSaved = await WordUserCreateAsync(wordUser, context);
                    if (!isSaved || wordUser.Id < 1)
                    {
                        return null;
                    }
                }
                if (wordUser is null || wordUser.Id is null) continue;
                int wordUserId = (int)wordUser.Id;
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

            if (valueFromDb.LanguageUserId is null) return;

            // now update the cache
            var languageUser = await LanguageUserByIdReadAsync((int)valueFromDb.LanguageUserId, context);
            if (languageUser is null || languageUser.UserId is null) return;
            await WordUserUpdateAllCachesAsync(value, (int)languageUser.UserId);
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
                await WordUserUpdateAllCachesAsync(wordUser, userId);
            }
            await context.SaveChangesAsync();
        }
        public static async Task WordUsersUpdateStatusByPageUserIdAndStatus(
            int pageUserId, AvailableWordUserStatus statusToEdit,
            AvailableWordUserStatus newStatus, IdiomaticaContext context)
        {
            var rows = (from pu in context.PageUsers
                             join bu in context.BookUsers on pu.BookUserId equals bu.Id
                             join lu in context.LanguageUsers on bu.LanguageUserId equals lu.Id
                             join p in context.Pages on pu.PageId equals p.Id
                             join pp in context.Paragraphs on p.Id equals pp.PageId
                             join s in context.Sentences on pp.Id equals s.ParagraphId
                             join t in context.Tokens on s.Id equals t.SentenceId
                             join w in context.Words on t.WordId equals w.Id
                             join wu in context.WordUsers on w.Id equals wu.WordId
                             where (pu.PageId == pageUserId && 
                                wu != null &&
                                wu.LanguageUserId == bu.LanguageUserId &&
                                wu.Status == statusToEdit)
                             select new { wordUser = wu, userId = lu.UserId })
                             .ToList();
            foreach (var row in rows)
            {
                row.wordUser.Status = newStatus;
                if (row is null || row.userId is null) continue;
                // update cache
                await WordUserUpdateAllCachesAsync(row.wordUser, (int)row.userId);
            }
            await context.SaveChangesAsync();
        }

        #endregion

        private static bool doesWordUserListContainById(List<WordUser> list, int key)
        {
            return list.Where(x => x.Id == key).Any();
        }
        private static List<WordUser> WordUsersListGetUpdated(List<WordUser> list, WordUser value)
        {
            List<WordUser> newList = new List<WordUser>();
            foreach (var wu in list)
            {
                if (wu.Id == value.Id) newList.Add(value);
                else newList.Add(wu);
            }
            return newList;
        }
        private static Dictionary<string, WordUser> WordUsersDictGetUpdated(
            Dictionary<string, WordUser> dict, WordUser value)
        {
            Dictionary<string, WordUser> newDict = new Dictionary<string, WordUser>();
            foreach (var kvpStringWordUser in newDict)
            {
                if (kvpStringWordUser.Value.Id == value.Id) newDict[kvpStringWordUser.Key] = value;
                else newDict[kvpStringWordUser.Key] = kvpStringWordUser.Value;
            }
            return newDict;
        }
        private static async Task WordUserUpdateAllCachesAsync(WordUser value, int userId)
        {
            WordUserById[(int)value.Id] = value;

            var cachedList1 = WordUserByWordIdAndUserId.Where(x => x.Value.Id == value.Id).ToList();
            foreach (var item in cachedList1) WordUserByWordIdAndUserId[item.Key] = value;

            var cachedList2 = WordUsersByBookIdAndLanguageUserId
                .Where(x => doesWordUserListContainById(x.Value, (int)value.Id)).ToArray();
            for (int i = 0; i < cachedList2.Length; i++)
            {
                var item = cachedList2[i];
                var newList = WordUsersListGetUpdated(item.Value, value);
                WordUsersByBookIdAndLanguageUserId[item.Key] = newList;
            }
            var dictsByUser3 = WordUsersDictByBookIdAndUserId.Where(x => x.Key.userId == userId);
            foreach (var kvpIntIntDict in dictsByUser3)
            {
                var newDict = WordUsersDictGetUpdated(kvpIntIntDict.Value, value);
                WordUsersDictByBookIdAndUserId[kvpIntIntDict.Key] = newDict;
            }
            var dictsByUser4 = WordUsersDictByPageIdAndUserId.Where(x => x.Key.userId == userId);
            foreach (var kvpIntIntDict in dictsByUser4)
            {
                var newDict = WordUsersDictGetUpdated(kvpIntIntDict.Value, value);
                WordUsersDictByPageIdAndUserId[kvpIntIntDict.Key] = newDict;
            }
            var cachedList5 = WordUsersByUserIdAndLanguageId.Where(x => x.Key.userId == userId).ToArray();
            
            for (int i = 0; i < cachedList5.Length; i++)
            {
                var item = cachedList5[i];
                var newList = WordUsersListGetUpdated(item.Value, value);
                WordUsersByUserIdAndLanguageId[item.Key] = newList;
            }
        }

    }
}
