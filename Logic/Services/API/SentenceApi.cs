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
            IdiomaticaContext context, string text, Guid languageId)
        {
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();

            var language = DataCache.LanguageByIdRead(languageId, context);
            if (language is null ||
                language.UniqueKey is null ||
                string.IsNullOrEmpty(language.Code))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            if(parser is null) { ErrorHandler.LogAndThrow(); return []; }
            return parser.SegmentTextBySentences(text);
        }
        public static async Task<string[]> PotentialSentencesSplitFromTextAsync(
            IdiomaticaContext context, string text, Guid languageId)
        {
            return await Task<string[]>.Run(() =>
            {
                return PotentialSentencesSplitFromText(context, text, languageId);
            });
        }


        public static Sentence? SentenceCreate(
            IdiomaticaContext context, string text, Guid languageId, int ordinal,
            Guid paragraphId)
        {
            if (ordinal < 0) ErrorHandler.LogAndThrow();
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();
            var newSentence = new Sentence()
            {
                ParagraphKey = paragraphId,
                Text = text,
                Ordinal = ordinal,
            };
            newSentence = DataCache.SentenceCreate(newSentence, context);
            if (newSentence is null || newSentence.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow(2280);
                return null;
            }
            newSentence.Tokens = TokenApi.TokensCreateFromSentence(context,
                (Guid)newSentence.UniqueKey, languageId);
            return newSentence;
        }
        public static async Task<Sentence?> SentenceCreateAsync(
            IdiomaticaContext context, string text, Guid languageId, int ordinal,
            Guid paragraphId)
        {
            return await Task<Sentence?>.Run(() =>
            {
                return SentenceCreate(context, text, languageId, ordinal, paragraphId);
            });
        }


        public static List<Sentence>? SentencesReadByPageId(IdiomaticaContext context, Guid pageId)
        {
            return DataCache.SentencesByPageIdRead(pageId, context);
        }
        public static async Task<List<Sentence>?> SentencesReadByPageIdAsync(IdiomaticaContext context, Guid pageId)
        {
            return await DataCache.SentencesByPageIdReadAsync(pageId, context);
        }


        public static List<Sentence>? SentencesReadByParagraphId(IdiomaticaContext context, Guid paragraphId)
        {
            return DataCache.SentencesByParagraphIdRead(paragraphId, context);
        }
        public static async Task<List<Sentence>?> SentencesReadByParagraphIdAsync(
            IdiomaticaContext context, Guid paragraphId)
        {
            return await DataCache.SentencesByParagraphIdReadAsync(paragraphId, context);
        }

    }
}
