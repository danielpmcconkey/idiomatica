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
            IdiomaticaContext context, string text, Language language)
        {
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();

            var parser = LanguageParser.Factory.GetLanguageParser(language);
            if(parser is null) { ErrorHandler.LogAndThrow(); return []; }
            return parser.SegmentTextBySentences(text);
        }
        public static async Task<string[]> PotentialSentencesSplitFromTextAsync(
            IdiomaticaContext context, string text, Language language)
        {
            return await Task<string[]>.Run(() =>
            {
                return PotentialSentencesSplitFromText(context, text, language);
            });
        }


        public static Sentence? SentenceCreate(
            IdiomaticaContext context, string text, Language language, int ordinal,
            Paragraph paragraph)
        {
            if (ordinal < 0) ErrorHandler.LogAndThrow();
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();
            var newSentence = new Sentence()
            {
                Id = Guid.NewGuid(),
                ParagraphId = paragraph.Id,
                Paragraph = paragraph,
                Text = text,
                Ordinal = ordinal,
            };
            newSentence = DataCache.SentenceCreate(newSentence, context);
            if (newSentence is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            newSentence.Tokens = TokenApi.TokensCreateFromSentence(context,
                newSentence, language);
            return newSentence;
        }
        public static async Task<Sentence?> SentenceCreateAsync(
            IdiomaticaContext context, string text, Language language, int ordinal,
            Paragraph paragraph)
        {
            return await Task<Sentence?>.Run(() =>
            {
                return SentenceCreate(context, text, language, ordinal, paragraph);
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
