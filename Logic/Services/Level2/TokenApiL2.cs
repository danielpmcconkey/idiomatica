using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using Model;
using Model.DAL;
using static System.Net.Mime.MediaTypeNames;

namespace Logic.Services.Level2
{
    public static class TokenApiL2
    {
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
            List<(Word word, int ordinal)> words = await WordApiL2.CreateOrderedWordsFromSentenceIdAsync(
                context, languageId, sentenceId);

            if (words.Count < 1) return new List<Token>();

            // now make the tokens
            var tokenTasks = words.Select(async x => {
                if(x.word is null || x.word.Id is null or 0 || x.word.Text is null)
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
    }
}
