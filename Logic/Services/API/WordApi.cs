using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using DeepL;
using static System.Net.Mime.MediaTypeNames;

namespace Logic.Services.API
{
    public static class WordApi
    {
        public static Word? WordCreate(IdiomaticaContext context,
            int languageId, string text, string romanization)
        {
            var newWord = new Word()
            {
                LanguageId = languageId,
                Romanization = romanization,
                Text = text.ToLower(),
                TextLowerCase = text.ToLower(),
            };
            newWord = DataCache.WordCreate(newWord, context);
            if (newWord is null || newWord.Id is null || newWord.Id < 1)
            {
                ErrorHandler.LogAndThrow(2350);
                return newWord;
            }
            return newWord;
        }        
        public static async Task<Word?> WordCreateAsync(IdiomaticaContext context,
            int languageId, string text, string romanization)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordCreate(context, languageId, text, romanization);
            });
        }


        public static Word? WordGetById(IdiomaticaContext context, int wordId)
        {
            if (wordId < 1) ErrorHandler.LogAndThrow();
            return DataCache.WordByIdRead(wordId, context);
        }
        public static async Task<Word?> WordGetByIdAsync(IdiomaticaContext context, int wordId)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordGetById(context, wordId);
            });
        }


        public static Word? WordReadByLanguageIdAndText(
            IdiomaticaContext context, int languageId, string text)
        {
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();
            if (languageId < 1) ErrorHandler.LogAndThrow();
            return DataCache.WordByLanguageIdAndTextLowerRead((languageId, text), context);
        }
        public static async Task<Word?> WordReadByLanguageIdAndTextAsync(
            IdiomaticaContext context, int languageId, string text)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordReadByLanguageIdAndText(context, languageId, text);
            });
        }


        public static List<(Word? word, int? ordinal, string? tokenDisplay)> WordsCreateOrderedFromSentenceId(
            IdiomaticaContext context, int languageId, int sentenceId)
        {
            List<(Word? word, int? ordinal, string? tokenDisplay)> outList = [];
            if (sentenceId < 1) ErrorHandler.LogAndThrow();
            var sentence = DataCache.SentenceByIdRead(sentenceId, context);
            if (sentence == null || string.IsNullOrEmpty(sentence.Text))
            {
                ErrorHandler.LogAndThrow();
                return outList;
            }

            // check if any already exist. there shouldn't be any but whateves
            DataCache.TokenBySentenceIdDelete(sentenceId, context);

            var language = DataCache.LanguageByIdRead(languageId, context);
            if (language is null ||
                language.Id is null or 0 ||
                string.IsNullOrEmpty(language.Code))
            {
                ErrorHandler.LogAndThrow();
                return outList;
            }

            var parser = LanguageParser.Factory.GetLanguageParser(language);
            if (parser is null) { ErrorHandler.LogAndThrow(); return []; }
            var wordsSplits = parser.SegmentTextByWordsKeepingPunctuation(sentence.Text);
            if (wordsSplits is null || wordsSplits.Length == 0)
                return outList;
            
            for (int i = 0; i < wordsSplits.Length; i++)
            {
                Word? word = null;
                int? ordinal = i;
                string? tokenDisplay = wordsSplits[i];
                var cleanWord = parser.TextToLower(parser.StipNonWordCharacters(tokenDisplay));
                word = DataCache.WordByLanguageIdAndTextLowerRead(
                    ((int)languageId, cleanWord), context);
                if (word is null)
                {
                    // word doesn't exist; create it
                    word = WordApi.WordCreate(
                        context, languageId, cleanWord, cleanWord);
                }
                
                outList.Add((word, i, tokenDisplay));
            }
            return outList;
        }
        public static async Task<List<(Word? word, int? ordinal, string? tokenDisplay)>> WordsCreateOrderedFromSentenceIdAsync(
            IdiomaticaContext context, int languageId, int sentenceId)
        {
            return await Task<List<(Word? word, int? ordinal, string? tokenDisplay)>>.Run(() =>
            {
                return WordsCreateOrderedFromSentenceId(context, languageId, sentenceId);
            });
        }


        public static Dictionary<string, Word>? WordsDictReadByPageId(
            IdiomaticaContext context, int pageId)
        {
            return DataCache.WordsDictByPageIdRead(pageId, context);
        }
        public static async Task<Dictionary<string, Word>?> WordsDictReadByPageIdAsync(
            IdiomaticaContext context, int pageId)
        {
            return await DataCache.WordsDictByPageIdReadAsync(pageId, context);
        }


        public static List<(string language, int wordCount)> WordsGetListOfReadCount(
            IdiomaticaContext context, int userId)
        {
            if (userId < 1) ErrorHandler.LogAndThrow();
            List<(string language, int wordCount)> returnList = new List<(string language, int wordCount)>();
            var languageUsers = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(context, userId);
            if (languageUsers is null)
            {
                return new();
            }
            foreach (var languageUser in languageUsers)
            {
                if (languageUser.Language == null || languageUser.Language.Name == null) continue;
                var count = (from lu in context.LanguageUsers
                             join bu in context.BookUsers on lu.Id equals bu.LanguageUserId
                             join pu in context.PageUsers on bu.Id equals pu.BookUserId
                             join p in context.Pages on pu.PageId equals p.Id
                             join pp in context.Paragraphs on p.Id equals pp.PageId
                             join s in context.Sentences on pp.Id equals s.ParagraphId
                             join t in context.Tokens on s.Id equals t.SentenceId
                             where pu.ReadDate != null
                                && lu.Id == languageUser.Id
                             select t).Count();
                returnList.Add((languageUser.Language.Name, count));
            }
            return returnList;
        }
        public static async Task<List<(string language, int wordCount)>> WordsGetListOfReadCountAsync(
           IdiomaticaContext context, int userId)
        {
            return await Task<List<(string language, int wordCount)>>.Run(() =>
            {
                return WordsGetListOfReadCount(context, userId);
            });
        }
    }
}
