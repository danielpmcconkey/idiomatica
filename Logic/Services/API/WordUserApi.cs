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

namespace Logic.Services.API
{
    public static class WordUserApi
    {
        public static WordUser? WordUserCreate(
            IdiomaticaContext context, int wordId, int languageUserId, string? translation,
            AvailableWordUserStatus? status)
        {
            return WordUserCreateAsync(context, wordId, languageUserId, translation, status).Result;
        }
        public static async Task<WordUser?> WordUserCreateAsync(
            IdiomaticaContext context, int wordId, int languageUserId, string? translation,
            AvailableWordUserStatus? status)
        {
            if (wordId < 1) ErrorHandler.LogAndThrow();
            if (languageUserId < 1) ErrorHandler.LogAndThrow();
            if (status is null) status = AvailableWordUserStatus.UNKNOWN;
            if (wordId < 1) ErrorHandler.LogAndThrow();
            if (wordId < 1) ErrorHandler.LogAndThrow();
            if (wordId < 1) ErrorHandler.LogAndThrow();
            
            WordUser? wu = new()
            {
                WordId = wordId,
                LanguageUserId = languageUserId,
                Translation = translation,
                Status = status,
                Created = DateTime.Now,
                StatusChanged = DateTime.Now,
            };
            wu = await DataCache.WordUserCreateAsync(wu, context);
            if (wu is null || wu.Id is null || wu.Id < 1)
            {
                ErrorHandler.LogAndThrow();
            }
            return wu;
        }


        public static void WordUsersCreateAllForBookIdAndUserId(
            IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            DataCache.WordUsersCreateAllForBookIdAndUserId((bookId, userId), context);
        }
        public static async Task WordUsersCreateAllForBookIdAndUserIdAsync(
            IdiomaticaContext context, int bookId, int userId)
        {
            await Task.Run(() =>
            {
                return WordUsersCreateAllForBookIdAndUserIdAsync(context, bookId, userId);
            });
        }


        public static Dictionary<string, WordUser>? WordUsersDictByPageIdAndUserIdRead(
            IdiomaticaContext context, int pageId, int userId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            return DataCache.WordUsersDictByPageIdAndUserIdRead((pageId, userId), context);
        }
        public static async Task<Dictionary<string, WordUser>?> WordUsersDictByPageIdAndUserIdReadAsync(
            IdiomaticaContext context, int pageId, int userId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.WordUsersDictByPageIdAndUserIdReadAsync((pageId, userId), context);
        }


        public static void WordUserUpdate(IdiomaticaContext context,
            int id, AvailableWordUserStatus newStatus, string translation)
        {
            if (id < 1)
            {
                ErrorHandler.LogAndThrow(1150);
                return;
            }
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
            int id, AvailableWordUserStatus newStatus, string translation)
        {
            if (id < 1)
            {
                ErrorHandler.LogAndThrow(1150);
                return;
            }
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
    }
}
