using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Logic.Telemetry;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Logic.Services.Level1
{
    public static class TokenApiL1
    {
        public static async Task<List<Token>?> TokensReadByPageIdAsync(IdiomaticaContext context, int pageId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.TokensByPageIdReadAsync(pageId, context);
        }
        public static async Task<Token?> CreateTokenAsync(IdiomaticaContext context,
           string display, int sentenceId, int ordinal, int wordId)
        {
            Token token = new Token()
            {
                Display = display,
                SentenceId = sentenceId,
                Ordinal = ordinal,
                WordId = wordId
            };
            bool isSaved = await DataCache.TokenCreateAsync(token, context);
            if (!isSaved || token.Id == null || token.Id == 0)
            {
                ErrorHandler.LogAndThrow(2290);
                return null;
            }
            return token;
        }
        public static async Task<List<Token>> CreateTokensFromSentence(IdiomaticaContext context,
            int sentenceId, int languageId)
        {
            if (sentenceId < 1) ErrorHandler.LogAndThrow();
            var sentence = await DataCache.SentenceByIdReadAsync(sentenceId, context);
            if (sentence == null || string.IsNullOrEmpty(sentence.Text))
            {
                ErrorHandler.LogAndThrow();
                return new List<Token>();
            }

            // check if any already exist. there shouldn't be any but whateves
            await DataCache.TokenBySentenceIdDelete(sentenceId, context);

            var language = DataCache.LanguageByIdRead(languageId, context);
            if (language is null ||
                language.Id is null or 0 ||
                string.IsNullOrEmpty(language.Code))
            {
                ErrorHandler.LogAndThrow();
                return new List<Token>();
            }

            // create the words first
            List<(Word word, int ordinal)> words = await WordApiL1.CreateOrderedWordsFromSentenceIdAsync(
                context, languageId, sentenceId);

            if (words.Count < 1) return new List<Token>();

            // now make the tokens
            var tokenTasks = words.Select(async x => {
                if (x.word is null || x.word.Id is null or 0 || x.word.Text is null)
                    return null;
                Token? newToken = await CreateTokenAsync(
                    context,
                    $"{x.word.Text} ", // display; add the space that you previously took out
                    sentenceId,
                    x.ordinal,
                    (int)x.word.Id
                    );
                return newToken;
            });
            Task.WaitAll(tokenTasks.ToArray());
            var tokens = await DataCache.TokensBySentenceIdReadAsync(sentenceId, context);
            return tokens ?? new List<Token>();
        }
        /// <summary>
        /// Returns the list of tokens plus child word
        /// </summary>
        public static async Task<List<Token>> TokensAndWordsReadBySentenceId(
            IdiomaticaContext context, int sentenceId)
        {
            if (sentenceId < 1) ErrorHandler.LogAndThrow();

            return await DataCache.TokensAndWordsBySentenceIdReadAsync(sentenceId, context);
        }
        public static async Task<(Token? t, WordUser? wu)> TokenGetChildObjects(
            IdiomaticaContext context, int tokenId, int languageUserId)
        {
            Token? t = new();
            WordUser? wu = new ();
            if (tokenId < 1) ErrorHandler.LogAndThrow();
            if (languageUserId < 1) ErrorHandler.LogAndThrow();
            var languageUser = await DataCache.LanguageUserByIdReadAsync(languageUserId, context);
            if (languageUser == null || languageUser.Id is null || languageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return (t, wu);
            }
            if (languageUser.UserId == null || languageUser.UserId < 1)
            {
                ErrorHandler.LogAndThrow();
                return (t, wu);
            }
            t = await DataCache.TokenByIdReadAsync(tokenId, context);
            if (t == null || t.WordId == null || t.WordId < 1)
            {
                ErrorHandler.LogAndThrow();
                return (t, wu);
            }
            t.Word = await DataCache.WordByIdReadAsync((int)t.WordId, context);
            if (t.Word == null || t.Word.Id == null || t.Word.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return (t, wu);
            }
            wu = await DataCache.WordUserByWordIdAndUserIdReadAsync(
                ((int)t.Word.Id, (int)languageUser.UserId), context);
            if (wu is null)
            {
                // create it
                wu = await WordUserApiL1.WordUserCreate(context, (int)t.Word.Id,
                    (int)languageUser.Id, string.Empty, AvailableWordUserStatus.UNKNOWN);
            }
            return (t, wu);
        }
    }
}
