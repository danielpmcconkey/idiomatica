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
    public static class ParagraphApi
    {
        public static List<Paragraph>? ParagraphsReadByPageId(IdiomaticaContext context, int pageId)
        {
            return DataCache.ParagraphsByPageIdRead(pageId, context);
        }
        public static async Task<List<Paragraph>?> ParagraphsReadByPageIdAsync(IdiomaticaContext context, int pageId)
        {
            return await DataCache.ParagraphsByPageIdReadAsync(pageId, context);
        }
        public static List<Paragraph> CreateParagraphsFromPage(
            IdiomaticaContext context, int pageId, int languageId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            if (languageId < 1) ErrorHandler.LogAndThrow();
            var language = DataCache.LanguageByIdRead(languageId, context);
            if (language is null || language.Id is null || language.Id < 1 ||
                string.IsNullOrEmpty(language.Code))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            var page = DataCache.PageByIdRead(pageId, context);
            if (page is null || page.Id is null || page.Id < 1 ||
                string.IsNullOrEmpty(page.OriginalText))
            {
                ErrorHandler.LogAndThrow();
                return new List<Paragraph>();
            }
            var paragraphs = new List<Paragraph>();
            var paragraphSplits = SplitTextToPotentialParagraphs(
                context, page.OriginalText, language.Code);
            int paragraphOrdinal = 0;
            foreach (var paragraphSplit in paragraphSplits)
            {
                var paragraphSplitTrimmed = paragraphSplit.Trim();
                if (string.IsNullOrEmpty(paragraphSplitTrimmed)) continue;
                var paragraph = CreateParagraphFromSplit(context,
                    paragraphSplitTrimmed, pageId, paragraphOrdinal, languageId);
                if (paragraph is not null && paragraph.Id is not null or 0)
                {
                    paragraphs.Add(paragraph);
                }
                paragraphOrdinal++;
            }
            return paragraphs;
        }
        public static async Task<List<Paragraph>> CreateParagraphsFromPageAsync(
            IdiomaticaContext context, int pageId, int languageId)
        {
            return await Task<List<Paragraph>>.Run(() =>
            {
                return CreateParagraphsFromPage(context, pageId, languageId);
            });
        }
        public static Paragraph? CreateParagraphFromSplit(
            IdiomaticaContext context, string splitText, int pageId,
            int ordinal, int languageId)
        {
            if (splitText.Trim() == string.Empty) return null;
            if (pageId < 1) ErrorHandler.LogAndThrow();
            if (languageId < 1) ErrorHandler.LogAndThrow();
            if (ordinal < 0) ErrorHandler.LogAndThrow();

            var paragraph = new Paragraph()
            {
                Ordinal = ordinal,
                PageId = pageId
            };
            paragraph = DataCache.ParagraphCreate(paragraph, context);
            if (paragraph is null || paragraph.Id is null || paragraph.Id < 1)
            {
                ErrorHandler.LogAndThrow(2270);
                return null;
            }

            // now create the sentences
            var sentenceSplits = SentenceApi.SplitTextToPotentialSentences(
                context, splitText, languageId);
            int sentenceOrdinal = 0;
            foreach (var sentenceSplit in sentenceSplits)
            {
                if (string.IsNullOrEmpty(sentenceSplit)) continue;
                var trimmedSentenceSplit = sentenceSplit.Trim();
                if (string.IsNullOrEmpty(trimmedSentenceSplit)) continue;
                var sentence = SentenceApi.CreateSentence(
                    context, trimmedSentenceSplit, languageId, sentenceOrdinal, (int)paragraph.Id);
                if (sentence != null)
                {
                    paragraph.Sentences.Add(sentence);
                }
            }
            return paragraph;
        }
        public static async Task<Paragraph?> CreateParagraphFromSplitAsync(
            IdiomaticaContext context, string splitText, int pageId,
            int ordinal, int languageId)
        {
            return await Task<Paragraph?>.Run(() =>
            {
                return CreateParagraphFromSplit(context, splitText, pageId, ordinal, languageId);
            });
        }
        /// <summary>
        /// Used to take the string contents of the book creation page and 
        /// split it out into paragraphs (text, not Model.Paragraph) ahead of 
        /// creating page and paragraph objects
        /// </summary>
        public static string[] SplitTextToPotentialParagraphs(
            IdiomaticaContext context, string text, string languageCode)
        {
            var textDenulled = NullHandler.ThrowIfNullOrEmptyString(text.Trim().Replace('\u00A0', ' '));
            var lcDenulled = NullHandler.ThrowIfNullOrEmptyString(languageCode);

            // pull language from the db
            var language = DataCache.LanguageByCodeRead(lcDenulled, context);
            if (language is null)
            {
                ErrorHandler.LogAndThrow();
                return new string[] { };
            }

            // divide text into paragraphs
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var paragraphSplitsRaw = parser.SegmentTextByParagraphs(textDenulled);
            return NullHandler.ThrowIfNullOrEmptyArray(paragraphSplitsRaw);
        }
        public static async Task<string[]> SplitTextToPotentialParagraphsAsync(
            IdiomaticaContext context, string text, string languageCode)
        {
            return await Task<string[]>.Run(() =>
            {
                return SplitTextToPotentialParagraphs(context, text, languageCode);
            });

        }
        public static async Task<string> ParagraphReadAllTextAsync(IdiomaticaContext context, int paragraphId)
        {
            if (paragraphId < 1) ErrorHandler.LogAndThrow();
            var sentences = await DataCache.SentencesByParagraphIdReadAsync(paragraphId, context);
            if (sentences is null || sentences.Count < 1)
            {
                ErrorHandler.LogAndThrow();
                return string.Empty;
            }
            var sentenceTexts = sentences.OrderBy(x => x.Ordinal).Select(s => s.Text ?? "").ToList<string>();
            if (sentenceTexts is null)
            {
                ErrorHandler.LogAndThrow();
                return string.Empty;
            }
            return String.Join(" ", sentences);
        }
        public static async Task<(string input, string output)> ParagraphTranslateAsync(
            IdiomaticaContext context, int paragraphId, string fromCode, string toCode)
        {
            if (paragraphId < 1) ErrorHandler.LogAndThrow();
            if (string.IsNullOrEmpty(fromCode)) ErrorHandler.LogAndThrow();
            if (string.IsNullOrEmpty(toCode)) ErrorHandler.LogAndThrow();


            string input = await ParagraphReadAllTextAsync(context, paragraphId);
            string output = "";

            // see if the translation already exists
            var existingTranslations = await DataCache.ParagraphTranslationsByParargraphIdReadAsync(
                    paragraphId, context);
            if (existingTranslations is not null && existingTranslations.Count > 0)
            {
                // are any in the right language?
                var currentTranslation = existingTranslations
                .Where(x => x.Code == toCode)
                .FirstOrDefault();
                if (currentTranslation != null && currentTranslation.TranslationText != null)
                {
                    output = currentTranslation.TranslationText;
                    return (input, output);
                }
            }
            // nope, no existing translations will work
            
            var deeplResult = DeepLService.Translate(input, fromCode, toCode);
            if (deeplResult is not null)
            {
                output = deeplResult;
                // add to the DB
                var ppt = new ParagraphTranslation()
                {
                    ParagraphId = paragraphId,
                    Code = toCode,
                    TranslationText = deeplResult
                };
                ppt = await DataCache.ParagraphTranslationCreateAsync(ppt, context);
                if (ppt is null || ppt.Id is null || ppt.Id < 1)
                {
                    ErrorHandler.LogAndThrow(2340);
                    return ("", "");
                }
            }
            
            return (input, output);
        }

    }
}
