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
           string display, Sentence sentence, int ordinal, Word word)
        {
            var token = new Token()
            {
                Id = Guid.NewGuid(),
                Display = display,
                SentenceId = sentence.Id,
                Sentence = sentence,
                Ordinal = ordinal,
                WordId = word.Id,
                Word = word,
            };
            token =  DataCache.TokenCreate(token, context);
            if (token is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return token;
        }
        public static async Task<Token?> TokenCreateAsync(IdiomaticaContext context,
           string display, Sentence sentence, int ordinal, Word word)
        {
            return await Task<Token?>.Run(() =>
            {
                return TokenCreate(context, display, sentence, ordinal, word);
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
            var languageUser = await DataCache.LanguageUserByIdReadAsync(languageUserId, context);
            if (languageUser == null)
            {
                ErrorHandler.LogAndThrow();
                return (null, null);
            }
            var t = await DataCache.TokenByIdReadAsync(tokenId, context);
            if (t == null)
            {
                ErrorHandler.LogAndThrow();
                return (null, null);
            }
            if (t.Word == null)
            {
                ErrorHandler.LogAndThrow();
                return (null, null);
            }
            var wu = await DataCache.WordUserByWordIdAndUserIdReadAsync(
                (t.WordId, languageUser.UserId), context);
            if (wu is null)
            {
                // create it
                wu = await WordUserApi.WordUserCreateAsync(context, t.Word,
                    languageUser, string.Empty, AvailableWordUserStatus.UNKNOWN);
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
            Sentence sentence, Language language)
        {
            // check if any already exist. there shouldn't be any but whateves
            DataCache.TokenBySentenceIdDelete(sentence.Id, context);

            // create the words first
            List<(Word? word, int? ordinal, string? tokenDisplay)> words = 
                WordApi.WordsCreateOrderedFromSentenceId(context, language, sentence);

            if (words.Count < 1) return new List<Token>();

            // now make the tokens
            List<Token> tokens = [];
            foreach (var x in words)
            {
                if (x.word is null ||
                    x.word.Text is null || x.ordinal is null || x.tokenDisplay is null) continue;
                Token? newToken = TokenCreate(
                    context,
                    $"{x.tokenDisplay} ", // add the space that you previously took out
                    sentence,
                    (int)x.ordinal,
                    x.word
                    );
                if(newToken is null)
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
            IdiomaticaContext context, Sentence sentence, Language language)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensCreateFromSentence(context, sentence, language);
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
