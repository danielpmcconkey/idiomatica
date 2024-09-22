using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using System.ComponentModel.DataAnnotations;
using Azure;
using Model.Enums;
using Microsoft.EntityFrameworkCore;
using DeepL;

namespace Logic.Services.API
{
    public static class WordUserApi
    {
        public static WordUser? WordUserCreate(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Word word, LanguageUser languageUser, string? translation,
            AvailableWordUserStatus status)
        {
            return WordUserCreateAsync(dbContextFactory, word, languageUser, translation, status).Result;
        }
        public static async Task<WordUser?> WordUserCreateAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Word word, LanguageUser languageUser, string? translation,
            AvailableWordUserStatus status)
        {
            WordUser? wu = new()
            {
                Id = Guid.NewGuid(),
                WordId = word.Id,
                //Word = word,
                LanguageUserId = languageUser.Id,
                //LanguageUser = languageUser,
                Translation = translation,
                Status = status,
                Created = DateTime.Now,
                StatusChanged = DateTime.Now,
            };
            wu = await DataCache.WordUserCreateAsync(wu, dbContextFactory);
            if (wu is null)
            {
                ErrorHandler.LogAndThrow();
            }
            return wu;
        }

        public static WordUser? WordUserReadById(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid wordUserId)
        {
            return DataCache.WordUserByIdRead(wordUserId, dbContextFactory);
        }
        public static async Task<WordUser?> WordUserReadByIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid wordUserId)
        {
            return await DataCache.WordUserByIdReadAsync(wordUserId, dbContextFactory);
        }



        /// <summary>
        /// find the next word user to make into a flash card based on word
        /// rank or status change
        /// </summary>
        public static WordUser? WordUserReadForNextFlashCard(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId,
            AvailableLanguageCode learningLanguageCode)
        {
            var context = dbContextFactory.CreateDbContext();

            /* 
             * the hierarchy...
             *   
             *   1. get word users that don't already have a flash card
             *   2. translation must be present (either use translation or 
             *      standard translation)
             *   3. first priority is the lowest ranking wordRank
             *   4. after that, find th card that has most recently changed its
             *      status
             *   
             * */

            var wordUser = (
                        from wu in context.WordUsers
                        join lu in context.LanguageUsers on wu.LanguageUserId equals lu.Id
                        join l in context.Languages on lu.LanguageId equals l.Id
                        join w in context.Words on wu.WordId equals w.Id
                        join wr in context.WordRanks on w.Id equals wr.WordId
                        join fc in context.FlashCards on wu.Id equals fc.WordUserId into grouping
                        from fc in grouping.DefaultIfEmpty()
                        where (
                            lu.UserId == userId &&
                            l.Code == learningLanguageCode &&
                            fc == null &&
                            (
                                !string.IsNullOrEmpty(wu.Translation) ||
                                w.WordTranslations.Count > 0)
                            )
                        orderby wr.Ordinal
                        select wu
                    )
                .FirstOrDefault();
            if (wordUser is not null) return wordUser;

            
            // no more word-ranked wordUsers. check for one that's changed
            // recently
            return (
                    from wu in context.WordUsers
                    join lu in context.LanguageUsers on wu.LanguageUserId equals lu.Id
                    join l in context.Languages on lu.LanguageId equals l.Id
                    join w in context.Words on wu.WordId equals w.Id
                    join fc in context.FlashCards on wu.Id equals fc.WordUserId into grouping
                    from fc in grouping.DefaultIfEmpty()
                    where (
                        lu.UserId == userId &&
                        l.Code == learningLanguageCode &&
                        fc == null &&
                        (
                            !string.IsNullOrEmpty(wu.Translation) ||
                            w.WordTranslations.Count > 0)
                        )
                    orderby wu.StatusChanged
                    select wu
                )
            .FirstOrDefault();
        }
        public static async Task<WordUser?> WordUserReadForNextFlashCardAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId,
            AvailableLanguageCode learningLanguageCode)
        {
            return await Task<WordUser?>.Run(() =>
            {
                return WordUserReadForNextFlashCard(dbContextFactory, userId,
                    learningLanguageCode);
            });
        }


        public static void WordUsersCreateAllForBookIdAndUserId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            DataCache.WordUsersCreateAllForBookIdAndUserId((bookId, userId), dbContextFactory);
        }
        public static async Task WordUsersCreateAllForBookIdAndUserIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            await DataCache.WordUsersCreateAllForBookIdAndUserIdAsync((bookId, userId), dbContextFactory);
        }


        public static Dictionary<string, WordUser>? WordUsersDictByPageIdAndUserIdRead(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId, Guid userId)
        {
            return DataCache.WordUsersDictByPageIdAndUserIdRead((pageId, userId), dbContextFactory);
        }
        public static async Task<Dictionary<string, WordUser>?> WordUsersDictByPageIdAndUserIdReadAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId, Guid userId)
        {
            return await DataCache.WordUsersDictByPageIdAndUserIdReadAsync((pageId, userId), dbContextFactory);
        }


        public static void WordUserUpdate(IDbContextFactory<IdiomaticaContext> dbContextFactory,
            Guid id, AvailableWordUserStatus newStatus, string translation)
        {
            // first pull the existing one from the database
            var dbWordUser = DataCache.WordUserByIdRead(id, dbContextFactory);
            if (dbWordUser == null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }

            dbWordUser.Status = newStatus;
            dbWordUser.Translation = translation;
            dbWordUser.StatusChanged = DateTime.Now;
            DataCache.WordUserUpdate(dbWordUser, dbContextFactory);

        }        
        public static async Task WordUserUpdateAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory,
            Guid id, AvailableWordUserStatus newStatus, string translation)
        {
            // first pull the existing one from the database
            var dbWordUser = await DataCache.WordUserByIdReadAsync(id, dbContextFactory);
            if (dbWordUser == null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }

            dbWordUser.Status = newStatus;
            dbWordUser.Translation = translation;
            dbWordUser.StatusChanged = DateTime.Now;
            await DataCache.WordUserUpdateAsync(dbWordUser, dbContextFactory);
        }


        public static string? WordUserTranslationFormat(IDbContextFactory<IdiomaticaContext> dbContextFactory,
            string? translation, AvailableLanguageCode translationLanguageCode)
        {
            if (string.IsNullOrEmpty(translation))
                return translation;
            // get the language so we can use the right parser
            var language = LanguageApi.LanguageReadByCode(dbContextFactory, translationLanguageCode);
            if (language is null) { ErrorHandler.LogAndThrow(); return translation; }
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            if (parser is null) { ErrorHandler.LogAndThrow(); return translation; }
            return parser.FormatTranslation(translation);

        }
        public static async Task<string?> WordUserTranslationFormatAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory,
            string? translation, AvailableLanguageCode translationLanguageCode)
        {
            return await Task<WordUser>.Run(() =>
            {
                return WordUserTranslationFormat(dbContextFactory, translation, translationLanguageCode);
            });
        }
    }
}
