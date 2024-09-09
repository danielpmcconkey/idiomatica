using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Logic.Telemetry;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using DeepL;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using Model.Enums;

namespace Logic.Services.API
{
    public static class TokenApi
    {
        public static Token? TokenCreate(IdiomaticaContext context,
           string display, Guid sentenceId, int ordinal, Guid wordId)
        {
            var token = new Token()
            {
                Display = display,
                SentenceKey = sentenceId,
                Ordinal = ordinal,
                WordKey = wordId
            };
            token =  DataCache.TokenCreate(token, context);
            if (token is null || token.UniqueKey == null)
            {
                ErrorHandler.LogAndThrow(2290);
                return null;
            }
            return token;
        }
        public static async Task<Token?> TokenCreateAsync(IdiomaticaContext context,
           string display, Guid sentenceId, int ordinal, Guid wordId)
        {
            return await Task<Token?>.Run(() =>
            {
                return TokenCreate(context, display, sentenceId, ordinal, wordId);
            });
        }


        public static (Token? t, WordUser? wu) TokenGetChildObjects(
            IdiomaticaContext context, Guid tokenId, Guid languageUserId)
        {
            return TokenGetChildObjectsAsync(context, tokenId, languageUserId).Result;
        }
        public static async Task<(Token? t, WordUser? wu)> TokenGetChildObjectsAsync(
            IdiomaticaContext context, Guid tokenId, Guid languageUserId)
        {
            Token? t = new();
            WordUser? wu = new ();
            var languageUser = await DataCache.LanguageUserByIdReadAsync(languageUserId, context);
            if (languageUser == null || languageUser.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return (t, wu);
            }
            if (languageUser.UserKey == null)
            {
                ErrorHandler.LogAndThrow();
                return (t, wu);
            }
            t = await DataCache.TokenByIdReadAsync(tokenId, context);
            if (t == null || t.WordKey == null)
            {
                ErrorHandler.LogAndThrow();
                return (t, wu);
            }
            t.Word = await DataCache.WordByIdReadAsync((Guid)t.WordKey, context);
            if (t.Word == null || t.Word.UniqueKey == null)
            {
                ErrorHandler.LogAndThrow();
                return (t, wu);
            }
            wu = await DataCache.WordUserByWordIdAndUserIdReadAsync(
                ((Guid)t.Word.UniqueKey, (Guid)languageUser.UserKey), context);
            if (wu is null)
            {
                // create it
                wu = await WordUserApi.WordUserCreateAsync(context, (Guid)t.Word.UniqueKey,
                    (Guid)languageUser.UniqueKey, string.Empty, AvailableWordUserStatus.UNKNOWN);
            }
            return (t, wu);
        }


        /// <summary>
        /// Returns the list of tokens plus child word
        /// </summary>
        public static async Task<List<Token>> TokensAndWordsReadBySentenceIdAsync(
            IdiomaticaContext context, Guid sentenceId)
        {
            return await DataCache.TokensAndWordsBySentenceIdReadAsync(sentenceId, context);
        }
        public static List<Token> TokensAndWordsReadBySentenceId(
            IdiomaticaContext context, Guid sentenceId)
        {
            return TokensAndWordsReadBySentenceIdAsync(context, sentenceId).Result;
        }


        public static List<Token> TokensCreateFromSentence(IdiomaticaContext context,
            Guid sentenceId, Guid languageId)
        {
            var sentence = DataCache.SentenceByIdRead(sentenceId, context);
            if (sentence == null || string.IsNullOrEmpty(sentence.Text))
            {
                ErrorHandler.LogAndThrow();
                return new List<Token>();
            }

            // check if any already exist. there shouldn't be any but whateves
            DataCache.TokenBySentenceIdDelete(sentenceId, context);

            var language = DataCache.LanguageByIdRead(languageId, context);
            if (language is null ||
                language.UniqueKey is null ||
                string.IsNullOrEmpty(language.Code))
            {
                ErrorHandler.LogAndThrow();
                return new List<Token>();
            }

            // create the words first
            List<(Word? word, int? ordinal, string? tokenDisplay)> words = WordApi.WordsCreateOrderedFromSentenceId(
                context, languageId, sentenceId);

            if (words.Count < 1) return new List<Token>();

            // now make the tokens
            List<Token> tokens = [];
            foreach (var x in words)
            {
                if (x.word is null || x.word.UniqueKey is null ||
                    x.word.Text is null || x.ordinal is null || x.tokenDisplay is null) continue;
                Token? newToken = TokenCreate(
                    context,
                    $"{x.tokenDisplay} ", // display; add the space that you previously took out
                    sentenceId,
                    (int)x.ordinal,
                    (Guid)x.word.UniqueKey
                    );
                if(newToken is null || newToken.UniqueKey is null)
                {
                    ErrorHandler.LogAndThrow();
                    return [];
                }
                newToken.Word = x.word;
                tokens.Add(newToken);
            }
            return tokens;
        }
        public static async Task<List<Token>> TokensCreateFromSentenceAsync(
            IdiomaticaContext context, Guid sentenceId, Guid languageId)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensCreateFromSentence(context, sentenceId, languageId);
            });
        }


        public static List<Token>? TokensReadByPageId(
            IdiomaticaContext context, Guid pageId)
        {
            return DataCache.TokensByPageIdRead(pageId, context);
        }
        public static async Task<List<Token>?> TokensReadByPageIdAsync(
            IdiomaticaContext context, Guid pageId)
        {
            return await DataCache.TokensByPageIdReadAsync(pageId, context);
        }
    }
}
