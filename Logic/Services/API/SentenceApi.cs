using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;
using DeepL;
using System.Net;

namespace Logic.Services.API
{
    public static class SentenceApi
    {
        public static string[] PotentialSentencesSplitFromText(
            IdiomaticaContext context, string text, int languageId)
        {
            if (languageId < 1) ErrorHandler.LogAndThrow();
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();

            var language = DataCache.LanguageByIdRead(languageId, context);
            if (language is null ||
                language.Id is null or 0 ||
                string.IsNullOrEmpty(language.Code))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            return parser.SegmentTextBySentences(text);
        }
        public static async Task<string[]> PotentialSentencesSplitFromTextAsync(
            IdiomaticaContext context, string text, int languageId)
        {
            return await Task<string[]>.Run(() =>
            {
                return PotentialSentencesSplitFromText(context, text, languageId);
            });
        }


        public static Sentence? SentenceCreate(
            IdiomaticaContext context, string text, int languageId, int ordinal,
            int paragraphId)
        {
            if (paragraphId < 1) ErrorHandler.LogAndThrow();
            if (languageId < 1) ErrorHandler.LogAndThrow();
            if (ordinal < 0) ErrorHandler.LogAndThrow();
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();
            var newSentence = new Sentence()
            {
                ParagraphId = paragraphId,
                Text = text,
                Ordinal = ordinal,
            };
            newSentence = DataCache.SentenceCreate(newSentence, context);
            if (newSentence is null || newSentence.Id is null || newSentence.Id < 1)
            {
                ErrorHandler.LogAndThrow(2280);
                return null;
            }
            newSentence.Tokens = TokenApi.TokensCreateFromSentence(context,
                (int)newSentence.Id, languageId);
            return newSentence;
        }
        public static async Task<Sentence?> SentenceCreateAsync(
            IdiomaticaContext context, string text, int languageId, int ordinal,
            int paragraphId)
        {
            return await Task<Sentence?>.Run(() =>
            {
                return SentenceCreate(context, text, languageId, ordinal, paragraphId);
            });
        }


        public static List<Sentence>? SentencesReadByPageId(IdiomaticaContext context, int pageId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            return DataCache.SentencesByPageIdRead(pageId, context);
        }
        public static async Task<List<Sentence>?> SentencesReadByPageIdAsync(IdiomaticaContext context, int pageId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.SentencesByPageIdReadAsync(pageId, context);
        }
        
    }
}
