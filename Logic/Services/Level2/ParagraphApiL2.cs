using Logic.Telemetry;
using Model;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Logic.Services.Level2
{
    public static class ParagraphApiL2
    {
        public static async Task<Paragraph?> CreateParagraphFromSplitAsync(
            IdiomaticaContext context, string splitText, int pageId,
            int ordinal, int languageId)
        {
            if (splitText.Trim() == string.Empty) return null;
            if (pageId < 1) ErrorHandler.LogAndThrow();
            if (languageId < 1) ErrorHandler.LogAndThrow();
            if (ordinal < 1) ErrorHandler.LogAndThrow();
            
            Paragraph paragraph = new Paragraph()
            {
                Ordinal = ordinal,
                PageId = pageId
            };
            bool isSaved = await DataCache.ParagraphCreateAsync(paragraph, context);
            if (!isSaved || paragraph.Id == null || paragraph.Id == 0)
            {
                ErrorHandler.LogAndThrow(2270);
                return null;
            }

            // now create the sentences
            var sentenceSplits = SentenceApiL2.SplitTextToPotentialSentences(
                context, splitText, languageId);
            int sentenceOrdinal = 0;
            foreach (var sentenceSplit in sentenceSplits)
            {
                if (string.IsNullOrEmpty(sentenceSplit)) continue;
                var trimmedSentenceSplit = sentenceSplit.Trim();
                if (string.IsNullOrEmpty(trimmedSentenceSplit)) continue;
                var sentence = await SentenceApiL2.CreateSentenceAsync(
                    context, trimmedSentenceSplit, languageId, sentenceOrdinal, (int)paragraph.Id);
                if (sentence != null)
                {
                    paragraph.Sentences.Add(sentence);
                }
            }
            return paragraph;
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
    }
}
