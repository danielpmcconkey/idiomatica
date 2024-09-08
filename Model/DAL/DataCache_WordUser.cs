using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<Guid, WordUser> WordUserById = new ();
        private static ConcurrentDictionary<Guid, WordUser> WordUserAndLanguageUserAndLanguageById = new ();
        private static ConcurrentDictionary<(Guid wordId, Guid userId), WordUser> WordUserByWordIdAndUserId = new ();
        private static ConcurrentDictionary<(Guid bookId, Guid uselanguageUserIdrId), List<WordUser>> WordUsersByBookIdAndLanguageUserId = new ();
        private static ConcurrentDictionary<(Guid bookId, Guid userId), Dictionary<string, WordUser>> WordUsersDictByBookIdAndUserId = new ();
        private static ConcurrentDictionary<(Guid pageId, Guid userId), Dictionary<string, WordUser>> WordUsersDictByPageIdAndUserId = new ();
        private static ConcurrentDictionary<(Guid userId, Guid languageId), List<WordUser>> WordUsersByUserIdAndLanguageId = new ();


        #region create


        public static WordUser? WordUserCreate(WordUser wordUser, IdiomaticaContext context)
        {
            if (wordUser.WordKey is null)
            {
                var ex = new InvalidDataException(nameof(wordUser.WordKey));
                throw ex;
            }

            if (wordUser.LanguageUserKey is null)
            {
                var ex = new InvalidDataException(nameof(wordUser.LanguageUserKey));
                throw ex;
            }

            if (wordUser.Status is null)
            {
                var ex = new InvalidDataException(nameof(wordUser.Status));
                throw ex;
            }

            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                
                INSERT INTO [Idioma].[WordUser]
                      ([WordKey]
                      ,[LanguageUserKey]
                      ,[Translation]
                      ,[Status]
                      ,[Created]
                      ,[StatusChanged]
                      ,[UniqueKey])
                VALUES (
                      {wordUser.WordKey}
                      ,{wordUser.LanguageUserKey}
                      ,{wordUser.Translation}
                      ,{wordUser.Status}
                      ,{wordUser.Created}
                      ,{wordUser.StatusChanged}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating WordUser affected 0 rows");
            var newEntity = context.WordUsers.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.UniqueKey is null)
            {
                throw new InvalidDataException("newEntity is null in WordUserCreate");
            }


            // add it to cache
            WordUserById[(Guid)newEntity.UniqueKey] = newEntity;

            return newEntity;
        }
        public static async Task<WordUser?> WordUserCreateAsync(WordUser value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return WordUserCreate(value, context); });
        }

        public static void WordUsersCreateAllForBookIdAndUserId(
            (Guid bookId, Guid userId) key, IdiomaticaContext context)
        {
            // add all the worduser objects that might not exist
            context.Database.ExecuteSql($"""
                insert into [Idioma].[WordUser] 
                ([WordId],[LanguageUserId],[Translation],[Status],[Created],[StatusChanged],[UniqueKey])
                select 
                	 w.UniqueKey as wordId
                	, lu.UniqueKey as languageUserId
                	, null as translation
                	, {(int)AvailableWordUserStatus.UNKNOWN} as unknownStatus
                	, CURRENT_TIMESTAMP as created
                	, CURRENT_TIMESTAMP as statusChanged
                    , NEWID()
                from [Idioma].[Page] p
                left join [Idioma].[Book] b on p.BookKey = b.UniqueKey
                left join [Idioma].[Language] l on b.LanguageKey = l.UniqueKey
                left join [Idioma].[LanguageUser] lu on l.UniqueKey = lu.LanguageKey
                left join [Idioma].[Paragraph] pp on p.UniqueKey = pp.PageKey
                left join [Idioma].[Sentence] s on pp.UniqueKey = s.ParagraphKey
                left join [Idioma].[Token] t on s.UniqueKey = t.SentenceKey
                left join [Idioma].[Word] w on t.WordKey = w.UniqueKey
                left join [Idioma].[WordUser] wu on w.UniqueKey = wu.WordKey and wu.LanguageUserKey = lu.UniqueKey
                where b.UniqueKey = {key.bookId}
                and lu.UserKey = {key.userId}
                and wu.UniqueKey is null
                group by 
                	  w.UniqueKey
                	, lu.UniqueKey
                	, w.TextLowerCase
                	, wu.UniqueKey
                """);

            // now, to write the cache pull the wordusers

            var groups = from p in context.Pages
                         join b in context.Books on p.BookKey equals b.UniqueKey
                         join l in context.Languages on b.LanguageKey equals l.UniqueKey
                         join lu in context.LanguageUsers on l.UniqueKey equals lu.LanguageKey
                         join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                         join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                         join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                         join w in context.Words on t.WordKey equals w.UniqueKey
                         join wu in context.WordUsers on w.UniqueKey equals wu.WordKey
                         where wu.LanguageUserKey == lu.UniqueKey
                         && lu.UserKey == key.userId
                         && b.UniqueKey == key.bookId
                         select new { page = p, word = w, wordUser = wu };


            foreach (var g in groups)
            {
                if (g.word is null || g.word.TextLowerCase is null) continue;
                if (g.page is null || g.page.UniqueKey is null) continue;
                if (g.wordUser is null || g.wordUser.UniqueKey is null) continue;
                (Guid pageId, Guid userId) = ((Guid)g.page.UniqueKey, key.userId);

                try
                {
                    if (WordUsersDictByPageIdAndUserId.ContainsKey((pageId, userId)) == false)
                    {
                        // create the dict before you mess with it
                        WordUsersDictByPageIdAndUserId[(pageId, userId)] = [];
                    }
                    var wordUsersDict = WordUsersDictByPageIdAndUserId[(pageId, userId)];
                    if (wordUsersDict != null)
                    {
                        wordUsersDict[g.word.TextLowerCase] = g.wordUser;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
        public static async Task WordUsersCreateAllForBookIdAndUserIdAsync(
            (Guid bookId, Guid userId) key, IdiomaticaContext context)
        {
            await Task.Run(() =>
            {
                WordUsersCreateAllForBookIdAndUserId(key, context);
            });
        }
        #endregion

        #region read
        public static WordUser? WordUserByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordUserById.TryGetValue(key, out WordUser? value))
            {
                return value;
            }

            // read DB
            value = context.WordUsers.Where(x => x.UniqueKey == key).FirstOrDefault();

            if (value == null) return null;
            // write to cache
            WordUserById[key] = value;
            return value;
        }
        public static async Task<WordUser?> WordUserByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<WordUser?>.Run(() =>
            {
                return WordUserByIdRead(key, context);
            });
        }

        public static WordUser? WordUserAndLanguageUserAndLanguageByIdRead(
            Guid key, IdiomaticaContext context)
        {
            // check cache
            if (WordUserAndLanguageUserAndLanguageById.ContainsKey(key))
            {
                return WordUserById[key];
            }

            // read DB
            var value = context.WordUsers
                .Where(x => x.UniqueKey == key)
                .Include(x => x.LanguageUser)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                .ThenInclude(x => x.Language)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            WordUserAndLanguageUserAndLanguageById[key] = value;
            return value;
        }
        public static async Task<WordUser?> WordUserAndLanguageUserAndLanguageByIdReadAsync(
            Guid key, IdiomaticaContext context)
        {
            return await Task<WordUser?>.Run(() =>
            {
                return WordUserAndLanguageUserAndLanguageByIdRead(key, context);
            });
        }


        public static WordUser? WordUserByWordIdAndUserIdRead(
            (Guid wordId, Guid userId) key, IdiomaticaContext context)
        {
            // check cache
            if (WordUserByWordIdAndUserId.TryGetValue(key, out WordUser? value))
            {
                return value;
            }

            // read DB
            value = context.WordUsers
                .Where(x => x.WordKey == key.wordId &&
                x.LanguageUser != null &&
                x.LanguageUser.UserKey == key.userId)
                .FirstOrDefault();
            if (value == null) { return null; }
            // write to cache
            WordUserByWordIdAndUserId[key] = value;
            // also write each worduser to cache
            if (value.UniqueKey is null) { return value; }
            WordUserById[(Guid)value.UniqueKey] = value;
            return value;
        }
        public static async Task<WordUser?> WordUserByWordIdAndUserIdReadAsync(
            (Guid wordId, Guid userId) key, IdiomaticaContext context)
        {
            return await Task<WordUser?>.Run(() =>
            {
                return WordUserByWordIdAndUserIdRead(key, context);
            });
        }
        public static List<WordUser> WordUsersByBookIdAndLanguageUserIdRead(
           (Guid bookId, Guid languageUserId) key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            // check cache
            if (!shouldOverrideCache && WordUsersByBookIdAndLanguageUserId.TryGetValue(key, out List<WordUser>? value))
            {
                return value;
            }
            // read DB
            value = [.. (from b in context.Books
                         join p in context.Pages on b.UniqueKey equals p.BookKey
                         join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                         join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                         join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                         join w in context.Words on t.WordKey equals w.UniqueKey
                         join wu in context.WordUsers on w.UniqueKey equals wu.WordKey
                         where (b.UniqueKey == key.bookId && wu.LanguageUserKey == key.languageUserId)
                         select wu)
                .Distinct()];


            // write to cache
            WordUsersByBookIdAndLanguageUserId[key] = value;
            return value;
        }
        public static async Task<List<WordUser>> WordUsersByBookIdAndLanguageUserIdReadAsync(
           (Guid bookId, Guid languageUserId) key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            return await Task<List<WordUser>>.Run(() =>
            {
                return WordUsersByBookIdAndLanguageUserIdRead(key, context, shouldOverrideCache);
            });
        }

        public static Dictionary<string, WordUser> WordUsersDictByBookIdAndUserIdRead(
           (Guid bookId, Guid userId) key, IdiomaticaContext context)
        {
            // check cache
            if (WordUsersDictByBookIdAndUserId.TryGetValue(key, out Dictionary<string, WordUser>? value))
            {
                return value;
            }
            // read DB
            var groups = (from b in context.Books
                          join p in context.Pages on b.UniqueKey equals p.BookKey
                          join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                          join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                          join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                          join w in context.Words on t.WordKey equals w.UniqueKey
                          join wu in context.WordUsers on w.UniqueKey equals wu.UniqueKey
                          where (b.UniqueKey == key.bookId &&
                            wu.LanguageUser != null &&
                            wu.LanguageUser.UserKey == key.userId)
                          select new { word = w, wordUser = wu }).Distinct()
                .ToList();
            value = [];
            foreach (var g in groups)
            {
                if (g.word is null || g.word.TextLowerCase is null || g.wordUser is null || g.wordUser.UniqueKey is null)
                    continue;
                string wordText = g.word.TextLowerCase;// g.wordWordUser.word.TextLowerCase;
                WordUser wordUser = g.wordUser;
                Guid wordUserId = (Guid)g.wordUser.UniqueKey;
                value[wordText] = wordUser;
                // add it to the worduser cache
                WordUserById[wordUserId] = wordUser;
            }

            // write to cache
            WordUsersDictByBookIdAndUserId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, WordUser>> WordUsersDictByBookIdAndUserIdReadAsync(
           (Guid bookId, Guid userId) key, IdiomaticaContext context)
        {
            return await Task<Dictionary<string, WordUser>>.Run(() =>
            {
                return WordUsersDictByBookIdAndUserIdRead(key, context);
            });
        }


        public static Dictionary<string, WordUser>? WordUsersDictByPageIdAndUserIdRead(
            (Guid pageId, Guid userId) key, IdiomaticaContext context, bool shouldOverwriteCache = false)
        {
            // check cache
            if (WordUsersDictByPageIdAndUserId.TryGetValue(key, out Dictionary<string, WordUser>? value) 
                && shouldOverwriteCache == false)
            {
                return value;
            }

            // add all the worduser objects that might not exist
            context.Database.ExecuteSql($"""
                insert into [Idioma].[WordUser] 
                ([WordKey],[LanguageUserKey],[Translation],[Status],[Created],[StatusChanged],[UniqueKey])
                select 
                	 w.UniqueKey as wordKey
                	, lu.UniqueKey as languageUserKey
                	, null as translation
                	, {(int)AvailableWordUserStatus.UNKNOWN} as unknownStatus
                	, CURRENT_TIMESTAMP as created
                	, CURRENT_TIMESTAMP as statusChanged
                    , NEWID() as uniqueKey
                from [Idioma].[Page] p
                left join [Idioma].[Book] b on p.BookKey = b.UniqueKey
                left join [Idioma].[Language] l on b.LanguageKey = l.UniqueKey
                left join [Idioma].[LanguageUser] lu on l.UniqueKey = lu.LanguageKey
                left join [Idioma].[Paragraph] pp on p.UniqueKey = pp.PageKey
                left join [Idioma].[Sentence] s on pp.UniqueKey = s.ParagraphKey
                left join [Idioma].[Token] t on s.UniqueKey = t.SentenceKey
                left join [Idioma].[Word] w on t.WordKey = w.UniqueKey
                left join [Idioma].[WordUser] wu on w.UniqueKey = wu.WordKey and wu.LanguageUserKey = lu.UniqueKey
                where p.UniqueKey = {key.pageId}
                and lu.UserKey = {key.userId}
                and wu.UniqueKey is null
                and w.UniqueKey is not null
                group by 
                	  w.UniqueKey
                	, lu.UniqueKey
                	, w.TextLowerCase
                	, wu.UniqueKey
                """);

            // now pull the wordusers
            var groups = (from p in context.Pages
                join b in context.Books on p.BookKey equals b.UniqueKey
                join l in context.Languages on b.LanguageKey equals l.UniqueKey
                join lu in context.LanguageUsers on l.UniqueKey equals lu.LanguageKey
                join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                join s  in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                join t  in context.Tokens on s.UniqueKey equals t.SentenceKey
                join w  in context.Words on t.WordKey equals w.UniqueKey
                join wu in context.WordUsers on w.UniqueKey equals wu.WordKey
                where wu.LanguageUserKey == lu.UniqueKey
                && lu.UserKey == key.userId
                && p.UniqueKey == key.pageId
                select new { word = w, wordUser = wu })
                .Distinct()
                .ToList();

            value = [];
            foreach (var g in groups)
            {
                if (g.word.TextLowerCase is null) continue;
                value[g.word.TextLowerCase] = g.wordUser;
            }

            // write to cache
            WordUsersDictByPageIdAndUserId[key] = value;
            return value;
        }
        public static async Task<Dictionary<string, WordUser>?> WordUsersDictByPageIdAndUserIdReadAsync(
            (Guid pageId, Guid userId) key, IdiomaticaContext context, bool shouldOverwriteCache = false)
        {
            return await Task<Dictionary<string, WordUser>?>.Run(() =>
            {
                return WordUsersDictByPageIdAndUserIdRead(key, context, shouldOverwriteCache);
            });
        }
        public static List<WordUser> WordUsersByUserIdAndLanguageIdRead(
            (Guid userId, Guid languageId) key, IdiomaticaContext context)
        {
            // check cache
            if ( WordUsersByUserIdAndLanguageId.TryGetValue(key, out List<WordUser>? value))
            {
                return value;
            }

            // read DB
            value = [.. context.WordUsers
                .Where(x => x.LanguageUser != null &&
                    x.LanguageUser.UserKey == key.userId &&
                    x.LanguageUser.LanguageKey == key.languageId)
                ];
            // write to cache
            WordUsersByUserIdAndLanguageId[key] = value;
            // also write each worduser to cache
            foreach (var wordUser in value)
            {
                if (wordUser.UniqueKey is null) continue;
                WordUserById[(Guid)wordUser.UniqueKey] = wordUser;
            }
            return value;
        }
        public static async Task<List<WordUser>> WordUsersByUserIdAndLanguageIdReadAsync(
            (Guid userId, Guid languageId) key, IdiomaticaContext context)
        {
            return await Task<List<WordUser>>.Run(() =>
            {
                return WordUsersByUserIdAndLanguageIdRead(key, context);
            });
        }
        #endregion

        #region update

        public static void WordUserUpdate(WordUser wordUser, IdiomaticaContext context)
        {
            if (wordUser.UniqueKey == null)
                throw new ArgumentException("ID cannot be null or 0 when updating WordUser");

            int numRows = context.Database.ExecuteSql($"""
                        UPDATE [Idioma].[WordUser]
                        SET [WordKey] = {wordUser.WordKey}
                           ,[LanguageUserKey] = {wordUser.LanguageUserKey}
                           ,[Translation] = {wordUser.Translation}
                           ,[Status] = {wordUser.Status}
                           ,[Created] = {wordUser.Created}
                           ,[StatusChanged] = {wordUser.StatusChanged}
                        where [UniqueKey] = {wordUser.UniqueKey}
                        ;
                        """);
            if (numRows < 1)
            {
                throw new InvalidDataException("WordUser update affected 0 rows");
            };
            // now update the cache
            if (wordUser.LanguageUserKey is null) return;
            var languageUser = LanguageUserByIdRead((Guid)wordUser.LanguageUserKey, context);
            if (languageUser is null || languageUser.UserKey is null) return;
            WordUserUpdateAllCaches(wordUser, (Guid)languageUser.UserKey);

            return;
        }
        public static async Task WordUserUpdateAsync(WordUser value, IdiomaticaContext context)
        {
            await Task.Run(() =>
            {
                WordUserUpdate(value, context);
            });
        }
        public static void WordUsersUpdateStatusByPageIdAndUserIdAndStatus(
            Guid pageId, Guid userId, AvailableWordUserStatus statusToEdit,
            AvailableWordUserStatus newStatus, IdiomaticaContext context)
        {
            var wordUsers = (from pu in context.PageUsers
                             join p in context.Pages on pu.PageKey equals p.UniqueKey
                             join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                             join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                             join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                             join w in context.Words on t.WordKey equals w.UniqueKey
                             join wu in context.WordUsers on w.UniqueKey equals wu.WordKey
                             where (pu.PageKey == pageId &&
                                    wu.LanguageUser != null &&
                                    wu.LanguageUser.UserKey == userId &&
                                    wu.Status == statusToEdit)
                             select wu)
                             .ToList();
            foreach (var wordUser in wordUsers)
            {
                wordUser.Status = newStatus;
                // update cache
                WordUserUpdateAllCaches(wordUser, userId);
            }
        }
        public static void WordUsersUpdateStatusByPageUserIdAndStatus(
            Guid pageUserId, AvailableWordUserStatus statusToEdit,
            AvailableWordUserStatus newStatus, IdiomaticaContext context)
        {
            // update the DB
            int updateCount = context.Database.ExecuteSql($"""
                update [Idioma].[WordUser]
                set [Status] = {(int)newStatus}
                where UniqueKey in (
                	select wu.UniqueKey
                	from Idioma.PageUser pu
                	left join Idioma.BookUser bu on pu.BookUserKey = bu.UniqueKey
                	left join Idioma.LanguageUser lu on bu.LanguageUserKey = lu.UniqueKey
                	left join Idioma.Page p on pu.PageKey = p.UniqueKey
                	left join Idioma.Paragraph pp on p.UniqueKey = pp.PageKey
                	left join Idioma.Sentence s on pp.UniqueKey = s.ParagraphKey
                	left join Idioma.Token t on s.UniqueKey = t.SentenceKey
                	left join Idioma.Word w on t.WordKey = w.UniqueKey
                	left join Idioma.WordUser wu on w.UniqueKey = wu.WordKey
                	where 
                		pu.UniqueKey = {pageUserId}
                		and wu.UniqueKey is not null 
                		and wu.LanguageUserKey = bu.LanguageUserKey
                		and wu.Status = {(int)statusToEdit}
                )
                """);

            // pull all existing wordUsers to update cache
            var rows = (from pu in context.PageUsers
                             join bu in context.BookUsers on pu.BookUserKey equals bu.UniqueKey
                             join lu in context.LanguageUsers on bu.LanguageUserKey equals lu.UniqueKey
                             join p in context.Pages on pu.PageKey equals p.UniqueKey
                             join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                             join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                             join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                             join w in context.Words on t.WordKey equals w.UniqueKey
                             join wu in context.WordUsers on w.UniqueKey equals wu.WordKey
                             where (pu.UniqueKey == pageUserId && 
                                wu != null &&
                                wu.LanguageUserKey == bu.LanguageUserKey)
                             select new { wordUser = wu, userId = lu.UserKey })
                             .ToList();
            // iterate and update cache
            foreach (var row in rows)
            {
                if (row is null || row.userId is null) continue;
                // update the context, because the write operation above did not
                if (row.wordUser.Status == statusToEdit) row.wordUser.Status = newStatus;
                // update cache
                WordUserUpdateAllCaches(row.wordUser, (Guid)row.userId);
            }
        }

        #endregion

        private static bool DoesWordUserListContainById(List<WordUser> list, Guid key)
        {
            return list.Where(x => x.UniqueKey == key).Any();
        }
        private static List<WordUser> WordUsersListGetUpdated(List<WordUser> list, WordUser value)
        {
            List<WordUser> newList = [];
            foreach (var wu in list)
            {
                if (wu.UniqueKey == value.UniqueKey) newList.Add(value);
                else newList.Add(wu);
            }
            return newList;
        }
        /// <summary>
        /// iterate through every entry in dict and, where the WordUser in the 
        /// dict shares an ID with the updatedWordUser, then replace the dict 
        /// entry with the updatedWordUser
        /// </summary>
        private static Dictionary<string, WordUser> WordUsersDictGetUpdated(
            Dictionary<string, WordUser> dict, WordUser updatedWordUser)
        {
            Dictionary<string, WordUser> newDict = [];
            foreach (var kvpStringWordUser in dict)
            {
                if (kvpStringWordUser.Value.UniqueKey == updatedWordUser.UniqueKey) newDict[kvpStringWordUser.Key] = updatedWordUser;
                else newDict[kvpStringWordUser.Key] = kvpStringWordUser.Value;
            }
            return newDict;
        }
        private static void WordUserUpdateAllCaches(WordUser value, Guid userId)
        {
            if (value.UniqueKey is null) return;

            WordUserById[(Guid)value.UniqueKey] = value;

            var cachedList1 = WordUserByWordIdAndUserId.Where(x => x.Value.UniqueKey == value.UniqueKey).ToList();
            foreach (var item in cachedList1) WordUserByWordIdAndUserId[item.Key] = value;

            var cachedList2 = WordUsersByBookIdAndLanguageUserId
                .Where(x => DoesWordUserListContainById(x.Value, (Guid)value.UniqueKey)).ToArray();
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
