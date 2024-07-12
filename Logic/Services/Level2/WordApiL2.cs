using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model;
using Model.DAL;
using static System.Net.Mime.MediaTypeNames;

namespace Logic.Services.Level2
{
    public static class WordApiL2
    {
        public static async Task<Word> CreateWordAsync(IdiomaticaContext context,
            int languageId, string text, string romanization)
        {
            Word newWord = new Word()
            {
                LanguageId = languageId,
                Romanization = romanization,
                Text = text.ToLower(),
                TextLowerCase = text.ToLower(),
            };
            var isSaved = await DataCache.WordCreateAsync(newWord, context);
            if (!isSaved || newWord.Id == null || newWord.Id == 0)
            {
                ErrorHandler.LogAndThrow(2350);
                return newWord;
            }
            return newWord;
        }
        public static async Task<List<(Word word, int ordinal)>> CreateOrderedWordsFromSentenceIdAsync(
            IdiomaticaContext context, int languageId, int sentenceId)
        {
            if (sentenceId < 1) ErrorHandler.LogAndThrow();
            var sentence = await DataCache.SentenceByIdReadAsync(sentenceId, context);
            if (sentence == null || string.IsNullOrEmpty(sentence.Text))
            {
                ErrorHandler.LogAndThrow();
                return new List<(Word word, int ordinal)>();
            }

            // check if any already exist. there shouldn't be any but whateves
            await DataCache.TokenBySentenceIdDelete(sentenceId, context);

            var language = DataCache.LanguageByIdRead(languageId, context);
            if (language is null ||
                language.Id is null or 0 ||
                string.IsNullOrEmpty(language.Code))
            {
                ErrorHandler.LogAndThrow();
                return new List<(Word word, int ordinal)>();
            }

            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var wordsSplits = parser.SegmentTextByWordsKeepingPunctuation(sentence.Text);
            if (wordsSplits is null || wordsSplits.Length == 0) 
                return new List<(Word word, int ordinal)>();
            var orderedSplits = new List<(string split, int ordinal)>();
            for (int i = 0; i < wordsSplits.Length; i++)
            {
                orderedSplits.Add((wordsSplits[i], i));
            }
            
            List<(string word, int ordinal)> cleanWords = orderedSplits
                .Select(x => (parser.StipNonWordCharacters(x.split), x.ordinal))
                .ToList();
            var wordTasks = cleanWords.Select(async x => {
                Word? existingWord = DataCache.WordByLanguageIdAndTextLowerRead(
                    ((int)languageId, x.word), context);
                if (existingWord is not null) return ((Word)existingWord, x.ordinal);
                // word doesn't exist; create it
                Word? newWord = await WordApiL2.CreateWordAsync(
                    context, languageId, x.word, x.word);
                return (newWord, x.ordinal);
            });
            Task.WaitAll(wordTasks.ToArray());
            List<(Word word, int ordinal)> words = wordTasks.Select(x => x.Result).ToList();
            
            return words;
        }
    }
}
