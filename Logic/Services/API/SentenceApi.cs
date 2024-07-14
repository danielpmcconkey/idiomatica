using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Telemetry;

namespace Logic.Services.API
{
    public static class SentenceApi
    {
        public static async Task<List<Sentence>?> SentencesReadByPageIdAsync(IdiomaticaContext context, int pageId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.SentencesByPageIdReadAsync(pageId, context);
        }
        public static async Task<Sentence?> CreateSentenceAsync(
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
            var isSavedSentence = await DataCache.SentenceCreateAsync(newSentence, context);
            if (!isSavedSentence || newSentence.Id is null || newSentence.Id < 1)
            {
                ErrorHandler.LogAndThrow(2280);
                return null;
            }
            newSentence.Tokens = await TokenApi.CreateTokensFromSentence(context,
                (int)newSentence.Id, languageId);
            return newSentence;
        }
        public static string[] SplitTextToPotentialSentences(
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
        
    }
}
