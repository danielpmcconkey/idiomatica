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
                Word = word,
                LanguageUserId = languageUser.Id,
                LanguageUser = languageUser,
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
