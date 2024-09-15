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
using System.Net;
using Model.Enums;

namespace Logic.Services.API
{
    public static class ParagraphApi
    {
        public static Paragraph? ParagraphCreateFromSplit(
            IdiomaticaContext context, string splitText, Page page,
            int ordinal, Language language)
        {
            if (splitText.Trim() == string.Empty) return null;
            if (ordinal < 0) ErrorHandler.LogAndThrow();

            var paragraph = new Paragraph()
            {
                Id = Guid.NewGuid(),
                Ordinal = ordinal,
                PageId = page.Id,
                Page = page,
            };
            paragraph = DataCache.ParagraphCreate(paragraph, context);
            if (paragraph is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // now create the sentences
            var sentenceSplits = SentenceApi.PotentialSentencesSplitFromText(
                context, splitText, language);
            int sentenceOrdinal = 0;
            foreach (var sentenceSplit in sentenceSplits)
            {
                if (string.IsNullOrEmpty(sentenceSplit)) continue;
                var trimmedSentenceSplit = sentenceSplit.Trim();
                if (string.IsNullOrEmpty(trimmedSentenceSplit)) continue;
                var sentence = SentenceApi.SentenceCreate(
                    context, trimmedSentenceSplit, language, sentenceOrdinal, paragraph);
                if (sentence is null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                // the EFCore context is already appending the sentence to
                // the paragraph when it creates the sentence with that
                // paragraph ID. so only add it if not there already
                if (paragraph.Sentences is null) paragraph.Sentences = [];
                else if (paragraph.Sentences
                    .Where(x => x.Id == sentence.Id)
                    .FirstOrDefault() is null)
                {
                    paragraph.Sentences.Add(sentence);
                }

                sentenceOrdinal++;
            }
            return paragraph;
        }
        public static async Task<Paragraph?> ParagraphCreateFromSplitAsync(
            IdiomaticaContext context, string splitText, Page page,
            int ordinal, Language language)
        {
            return await Task<Paragraph?>.Run(() =>
            {
                return ParagraphCreateFromSplit(context, splitText, page, ordinal, language);
            });
        }

        /// <summary>
        ///  chooses a random ParagraphTranslationBridge from a card, translated to the user's UI preferred language
        /// </summary>
        public static (string example, string translation) ParagraphExamplePullRandomByFlashCardId(
            IdiomaticaContext context, Guid flashCardId, AvailableLanguageCode uiLanguageCode)
        {
            var example = string.Empty;
            var translation = string.Empty;

            var uiLanguage = LanguageApi.LanguageReadByCode(context, uiLanguageCode);
            if (uiLanguage is null) { ErrorHandler.LogAndThrow(); return (example, translation); }
            
            // pull all bridges for this card with this language code
            var bridges = DataCache
                .FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCodeRead(
                    (flashCardId, uiLanguage.Id), context);
            if (bridges is null || bridges.Count == 0) return (example, translation);
            
            // select a random bridge
            Random rng = new ();
            int position = rng.Next(0, bridges.Count);
            var bridge = bridges[position];

            // pull the paragraphtranslation
            var paragraphTranslation = DataCache.ParagraphTranslationByIdRead(
                (Guid)bridge.ParagraphTranslationId, context);
            if (paragraphTranslation is null || paragraphTranslation.TranslationText is null) 
                return (example, translation);
            translation = paragraphTranslation.TranslationText;

            // pull the orig text
            var paragraph = DataCache.ParagraphByIdRead((Guid)paragraphTranslation.ParagraphId, context);
            if (paragraph is null) return (example, translation);
            example = ParagraphApi.ParagraphReadAllText(context, (Guid) paragraph.Id);

            return (example, translation);
        }
        public static async Task<(string example, string translation)> ParagraphExamplePullRandomByFlashCardIdAsync(
            IdiomaticaContext context, Guid flashCardId, AvailableLanguageCode uiLanguageCode)
        {
            return await Task<(string example, string translation)>.Run(() =>
            {
                return ParagraphExamplePullRandomByFlashCardId(context, flashCardId, uiLanguageCode);
            });
        }


        public static string ParagraphReadAllText(
            IdiomaticaContext context, Guid paragraphId)
        {
            return ParagraphReadAllTextAsync(context, paragraphId).Result;
        }
        public static async Task<string> ParagraphReadAllTextAsync(
            IdiomaticaContext context, Guid paragraphId)
        {
            var sentences = await DataCache.SentencesByParagraphIdReadAsync(paragraphId, context);
            if (sentences is null)
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
            return String.Join(" ", sentenceTexts);
        }


        public static List<Paragraph> ParagraphsCreateFromPage(
            IdiomaticaContext context, Page page, Language language)
        {
            var paragraphs = new List<Paragraph>();
            var paragraphSplits = PotentialParagraphsSplitFromText(
                context, page.OriginalText, language);
            int paragraphOrdinal = 0;
            foreach (var paragraphSplit in paragraphSplits)
            {
                var paragraphSplitTrimmed = paragraphSplit.Trim();
                if (string.IsNullOrEmpty(paragraphSplitTrimmed)) continue;
                var paragraph = ParagraphCreateFromSplit(context,
                    paragraphSplitTrimmed, page, paragraphOrdinal, language);
                if (paragraph is not null)
                {
                    paragraphs.Add(paragraph);
                }
                paragraphOrdinal++;
            }
            return paragraphs;
        }
        public static async Task<List<Paragraph>> ParagraphsCreateFromPageAsync(
            IdiomaticaContext context, Page page, Language language)
        {
            return await Task<List<Paragraph>>.Run(() =>
            {
                return ParagraphsCreateFromPage(context, page, language);
            });
        }


        public static List<Paragraph>? ParagraphsReadByPageId(
            IdiomaticaContext context, Guid pageId)
        {
            return DataCache.ParagraphsByPageIdRead(pageId, context);
        }
        public static async Task<List<Paragraph>?> ParagraphsReadByPageIdAsync(
            IdiomaticaContext context, Guid pageId)
        {
            return await DataCache.ParagraphsByPageIdReadAsync(pageId, context);
        }


        public static (string input, string output) ParagraphTranslate(
            IdiomaticaContext context, Paragraph paragraph,
            AvailableLanguageCode fromCode, AvailableLanguageCode toCode)
        {
            return ParagraphTranslateAsync(context, paragraph, fromCode, toCode).Result;
        }
        public static async Task<(string input, string output)> ParagraphTranslateAsync(
            IdiomaticaContext context, Paragraph paragraph,
            AvailableLanguageCode fromCode, AvailableLanguageCode toCode)
        {
            string input = await ParagraphReadAllTextAsync(context, paragraph.Id);
            string output = "";


            var languageFrom = LanguageApi.LanguageReadByCode(context, fromCode);
            var languageTo = LanguageApi.LanguageReadByCode(context, toCode);

            if (languageFrom == null || languageTo == null) { ErrorHandler.LogAndThrow(); return (input, output); }

            // see if the translation already exists
            var existingTranslations = await DataCache.ParagraphTranslationsByParargraphIdReadAsync(
                    paragraph.Id, context);
            if (existingTranslations is not null && existingTranslations.Count > 0)
            {
                // are any in the right language?
                var currentTranslation = existingTranslations
                    .Where(x => x.LanguageId == languageTo.Id)
                    .FirstOrDefault();
                if (currentTranslation != null && currentTranslation.TranslationText != null)
                {
                    output = currentTranslation.TranslationText;
                    return (input, output);
                }
            }
            // nope, no existing translations will work
            
            var deeplResult = await DeepLService.TranslateAsync(input, fromCode, toCode);
            if (deeplResult is not null)
            {
                output = deeplResult;
                // add to the DB
                var ppt = new ParagraphTranslation()
                {
                    Id = Guid.NewGuid(),
                    ParagraphId = paragraph.Id,
                    Paragraph = paragraph,
                    LanguageId = languageTo.Id,
                    Language = languageTo,
                    TranslationText = deeplResult
                };
                ppt = await DataCache.ParagraphTranslationCreateAsync(ppt, context);
                if (ppt is null)
                {
                    ErrorHandler.LogAndThrow();
                    return (input, output);
                }
            }
            
            return (input, output);
        }


        /// <summary>
        /// Used to take the string contents of the book creation page and 
        /// split it out into paragraphs (text, not Model.Paragraph) ahead of 
        /// creating page and paragraph objects
        /// </summary>
        public static string[] PotentialParagraphsSplitFromText(
            IdiomaticaContext context, string text, Language language)
        {
            if (string.IsNullOrEmpty(text)) { ErrorHandler.LogAndThrow(); return []; }
            var textSanitized = text.Trim().Replace('\u00A0', ' ');
            

            // divide text into paragraphs
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            if (parser is null) { ErrorHandler.LogAndThrow(); return []; }
            var paragraphSplitsRaw = parser.SegmentTextByParagraphs(textSanitized);
            if(paragraphSplitsRaw is null || paragraphSplitsRaw.Length < 1)
                {  ErrorHandler.LogAndThrow(); return []; }
            return paragraphSplitsRaw;
        }
        public static async Task<string[]> PotentialParagraphsSplitFromTextAsync(
            IdiomaticaContext context, string text, Language language)
        {
            return await Task<string[]>.Run(() =>
            {
                return PotentialParagraphsSplitFromText(context, text, language);
            });

        }

    }
}
