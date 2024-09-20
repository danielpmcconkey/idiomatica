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
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class TokenApi
    {
        public static Token? TokenCreate(IDbContextFactory<IdiomaticaContext> dbContextFactory,
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
            token =  DataCache.TokenCreate(token, dbContextFactory);
            if (token is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return token;
        }
        public static async Task<Token?> TokenCreateAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory,
           string display, Sentence sentence, int ordinal, Word word)
        {
            return await Task<Token?>.Run(() =>
            {
                return TokenCreate(dbContextFactory, display, sentence, ordinal, word);
            });
        }


        public static (Token? t, WordUser? wu) TokenGetChildObjects(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid tokenId, Guid languageUserId)
        {
            return TokenGetChildObjectsAsync(dbContextFactory, tokenId, languageUserId).Result;
        }
        public static async Task<(Token? t, WordUser? wu)> TokenGetChildObjectsAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid tokenId, Guid languageUserId)
        {
            var languageUser = await DataCache.LanguageUserByIdReadAsync(languageUserId, dbContextFactory);
            if (languageUser == null)
            {
                ErrorHandler.LogAndThrow();
                return (null, null);
            }
            var t = await DataCache.TokenByIdReadAsync(tokenId, dbContextFactory);
            if (t == null)
            {
                ErrorHandler.LogAndThrow();
                return (null, null);
            }
            var w = await DataCache.WordByIdReadAsync(t.WordId, dbContextFactory);
            if (w is null) { ErrorHandler.LogAndThrow(); return (null, null); }
            t.Word = w;
            if (t.Word == null)
            {
                ErrorHandler.LogAndThrow();
                return (null, null);
            }
            var wu = await DataCache.WordUserByWordIdAndUserIdReadAsync(
                (t.WordId, languageUser.UserId), dbContextFactory);
            if (wu is null)
            {
                // create it
                wu = await WordUserApi.WordUserCreateAsync(dbContextFactory, t.Word,
                    languageUser, string.Empty, AvailableWordUserStatus.UNKNOWN);
            }
            return (t, wu);
        }


        /// <summary>
        /// Returns the list of tokens plus child word
        /// </summary>
        public static async Task<List<Token>> TokensAndWordsReadBySentenceIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid sentenceId)
        {
            return await DataCache.TokensAndWordsBySentenceIdReadAsync(sentenceId, dbContextFactory);
        }
        public static List<Token> TokensAndWordsReadBySentenceId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid sentenceId)
        {
            return TokensAndWordsReadBySentenceIdAsync(dbContextFactory, sentenceId).Result;
        }


        public static List<Token> TokensCreateFromSentence(IDbContextFactory<IdiomaticaContext> dbContextFactory,
            Sentence sentence, Language language)
        {
            // check if any already exist. there shouldn't be any but whateves
            DataCache.TokenBySentenceIdDelete(sentence.Id, dbContextFactory);

            // create the words first
            List<(Word? word, int? ordinal, string? tokenDisplay)> words = 
                WordApi.WordsCreateOrderedFromSentenceId(dbContextFactory, language, sentence);

            if (words.Count < 1) return new List<Token>();

            // now make the tokens
            List<Token> tokens = [];
            foreach (var x in words)
            {
                if (x.word is null ||
                    x.word.Text is null || x.ordinal is null || x.tokenDisplay is null) continue;
                Token? newToken = TokenCreate(
                    dbContextFactory,
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
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Sentence sentence, Language language)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensCreateFromSentence(dbContextFactory, sentence, language);
            });
        }


        public static List<Token>? TokensReadByPageId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId)
        {
            return DataCache.TokensByPageIdRead(pageId, dbContextFactory);
        }
        public static async Task<List<Token>?> TokensReadByPageIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId)
        {
            return await DataCache.TokensByPageIdReadAsync(pageId, dbContextFactory);
        }
    }
}
