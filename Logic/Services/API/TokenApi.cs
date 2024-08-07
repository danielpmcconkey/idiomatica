﻿using Model.DAL;
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

namespace Logic.Services.API
{
    public static class TokenApi
    {
        public static Token? TokenCreate(IdiomaticaContext context,
           string display, int sentenceId, int ordinal, int wordId)
        {
            var token = new Token()
            {
                Display = display,
                SentenceId = sentenceId,
                Ordinal = ordinal,
                WordId = wordId
            };
            token =  DataCache.TokenCreate(token, context);
            if (token is null || token.Id == null || token.Id == 0)
            {
                ErrorHandler.LogAndThrow(2290);
                return null;
            }
            return token;
        }
        public static async Task<Token?> TokenCreateAsync(IdiomaticaContext context,
           string display, int sentenceId, int ordinal, int wordId)
        {
            return await Task<Token?>.Run(() =>
            {
                return TokenCreate(context, display, sentenceId, ordinal, wordId);
            });
        }


        public static (Token? t, WordUser? wu) TokenGetChildObjects(
            IdiomaticaContext context, int tokenId, int languageUserId)
        {
            return TokenGetChildObjectsAsync(context, tokenId, languageUserId).Result;
        }
        public static async Task<(Token? t, WordUser? wu)> TokenGetChildObjectsAsync(
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
                wu = await WordUserApi.WordUserCreateAsync(context, (int)t.Word.Id,
                    (int)languageUser.Id, string.Empty, AvailableWordUserStatus.UNKNOWN);
            }
            return (t, wu);
        }


        /// <summary>
        /// Returns the list of tokens plus child word
        /// </summary>
        public static async Task<List<Token>> TokensAndWordsReadBySentenceIdAsync(
            IdiomaticaContext context, int sentenceId)
        {
            if (sentenceId < 1) ErrorHandler.LogAndThrow();

            return await DataCache.TokensAndWordsBySentenceIdReadAsync(sentenceId, context);
        }
        public static List<Token> TokensAndWordsReadBySentenceId(
            IdiomaticaContext context, int sentenceId)
        {
            return TokensAndWordsReadBySentenceIdAsync(context, sentenceId).Result;
        }


        public static List<Token> TokensCreateFromSentence(IdiomaticaContext context,
            int sentenceId, int languageId)
        {
            if (sentenceId < 1) ErrorHandler.LogAndThrow();
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
                language.Id is null or 0 ||
                string.IsNullOrEmpty(language.Code))
            {
                ErrorHandler.LogAndThrow();
                return new List<Token>();
            }

            // create the words first
            List<(Word? word, int ordinal)> words = WordApi.WordsCreateOrderedFromSentenceId(
                context, languageId, sentenceId);

            if (words.Count < 1) return new List<Token>();

            // now make the tokens
            List<Token> tokens = [];
            foreach (var x in words)
            {
                if (x.word is null || x.word.Id is null || x.word.Id < 1 || x.word.Text is null) continue;
                Token? newToken = TokenCreate(
                    context,
                    $"{x.word.Text} ", // display; add the space that you previously took out
                    sentenceId,
                    x.ordinal,
                    (int)x.word.Id
                    );
                if(newToken is null || newToken.Id is null)
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
            IdiomaticaContext context, int sentenceId, int languageId)
        {
            return await Task<List<Token>>.Run(() =>
            {
                return TokensCreateFromSentence(context, sentenceId, languageId);
            });
        }


        public static List<Token>? TokensReadByPageId(
            IdiomaticaContext context, int pageId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            return DataCache.TokensByPageIdRead(pageId, context);
        }
        public static async Task<List<Token>?> TokensReadByPageIdAsync(
            IdiomaticaContext context, int pageId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.TokensByPageIdReadAsync(pageId, context);
        }
    }
}
