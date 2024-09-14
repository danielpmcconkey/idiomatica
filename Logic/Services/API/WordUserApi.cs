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

namespace Logic.Services.API
{
    public static class WordUserApi
    {
        public static WordUser? WordUserCreate(
            IdiomaticaContext context, Word word, LanguageUser languageUser, string? translation,
            AvailableWordUserStatus status)
        {
            return WordUserCreateAsync(context, word, languageUser, translation, status).Result;
        }
        public static async Task<WordUser?> WordUserCreateAsync(
            IdiomaticaContext context, Word word, LanguageUser languageUser, string? translation,
            AvailableWordUserStatus status)
        {
            WordUser? wu = new()
            {
                UniqueKey = Guid.NewGuid(),
                WordKey = word.UniqueKey,
                Word = word,
                LanguageUserKey = languageUser.UniqueKey,
                LanguageUser = languageUser,
                Translation = translation,
                Status = status,
                Created = DateTime.Now,
                StatusChanged = DateTime.Now,
            };
            wu = await DataCache.WordUserCreateAsync(wu, context);
            if (wu is null)
            {
                ErrorHandler.LogAndThrow();
            }
            return wu;
        }

        public static WordUser? WordUserReadById(
            IdiomaticaContext context, Guid wordUserId)
        {
            return DataCache.WordUserByIdRead(wordUserId, context);
        }
        public static async Task<WordUser?> WordUserReadByIdAsync(
            IdiomaticaContext context, Guid wordUserId)
        {
            return await DataCache.WordUserByIdReadAsync(wordUserId, context);
        }


        public static void WordUsersCreateAllForBookIdAndUserId(
            IdiomaticaContext context, Guid bookId, Guid userId)
        {
            DataCache.WordUsersCreateAllForBookIdAndUserId((bookId, userId), context);
        }
        public static async Task WordUsersCreateAllForBookIdAndUserIdAsync(
            IdiomaticaContext context, Guid bookId, Guid userId)
        {
            await DataCache.WordUsersCreateAllForBookIdAndUserIdAsync((bookId, userId), context);
        }


        public static Dictionary<string, WordUser>? WordUsersDictByPageIdAndUserIdRead(
            IdiomaticaContext context, Guid pageId, Guid userId)
        {
            return DataCache.WordUsersDictByPageIdAndUserIdRead((pageId, userId), context);
        }
        public static async Task<Dictionary<string, WordUser>?> WordUsersDictByPageIdAndUserIdReadAsync(
            IdiomaticaContext context, Guid pageId, Guid userId)
        {
            return await DataCache.WordUsersDictByPageIdAndUserIdReadAsync((pageId, userId), context);
        }


        public static void WordUserUpdate(IdiomaticaContext context,
            Guid id, AvailableWordUserStatus newStatus, string translation)
        {
            // first pull the existing one from the database
            var dbWordUser = DataCache.WordUserByIdRead(id, context);
            if (dbWordUser == null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }

            dbWordUser.Status = newStatus;
            dbWordUser.Translation = translation;
            dbWordUser.StatusChanged = DateTime.Now;
            DataCache.WordUserUpdate(dbWordUser, context);

        }        
        public static async Task WordUserUpdateAsync(IdiomaticaContext context,
            Guid id, AvailableWordUserStatus newStatus, string translation)
        {
            // first pull the existing one from the database
            var dbWordUser = await DataCache.WordUserByIdReadAsync(id, context);
            if (dbWordUser == null)
            {
                ErrorHandler.LogAndThrow(2060);
                return;
            }

            dbWordUser.Status = newStatus;
            dbWordUser.Translation = translation;
            dbWordUser.StatusChanged = DateTime.Now;
            await DataCache.WordUserUpdateAsync(dbWordUser, context);
        }


        public static string? WordUserTranslationFormat(IdiomaticaContext context,
            string? translation, string? translationLanguageCode)
        {
            if (string.IsNullOrEmpty(translation) || string.IsNullOrEmpty(translationLanguageCode))
                return translation;
            // get the language so we can use the right parser
            var language = LanguageApi.LanguageReadByCode(context, translationLanguageCode);
            if (language is null) { ErrorHandler.LogAndThrow(); return translation; }
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            if (parser is null) { ErrorHandler.LogAndThrow(); return translation; }
            return parser.FormatTranslation(translation);

        }
        public static async Task<string?> WordUserTranslationFormatAsync(IdiomaticaContext context,
            string? translation, string? translationLanguageCode)
        {
            return await Task<WordUser>.Run(() =>
            {
                return WordUserTranslationFormat(context, translation, translationLanguageCode);
            });
        }
    }
}
