﻿using Model.DAL;
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

namespace Logic.Services.API
{
    public static class ParagraphApi
    {
        public static Paragraph? ParagraphCreateFromSplit(
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
            var sentenceSplits = SentenceApi.PotentialSentencesSplitFromText(
                context, splitText, languageId);
            int sentenceOrdinal = 0;
            foreach (var sentenceSplit in sentenceSplits)
            {
                if (string.IsNullOrEmpty(sentenceSplit)) continue;
                var trimmedSentenceSplit = sentenceSplit.Trim();
                if (string.IsNullOrEmpty(trimmedSentenceSplit)) continue;
                var sentence = SentenceApi.SentenceCreate(
                    context, trimmedSentenceSplit, languageId, sentenceOrdinal, (int)paragraph.Id);
                if (sentence is null || sentence.Id is  null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                // the EFCore context is already appending the sentence to
                // the paragraph when it creates the sentence with that
                // paragraph ID. so only add it if not there already
                if (paragraph.Sentences is null) paragraph.Sentences = [];
                else if (paragraph.Sentences.Where(x => x.Id == sentence.Id).FirstOrDefault() is null)
                {
                    paragraph.Sentences.Add(sentence);
                }

                sentenceOrdinal++;
            }
            return paragraph;
        }
        public static async Task<Paragraph?> ParagraphCreateFromSplitAsync(
            IdiomaticaContext context, string splitText, int pageId,
            int ordinal, int languageId)
        {
            return await Task<Paragraph?>.Run(() =>
            {
                return ParagraphCreateFromSplit(context, splitText, pageId, ordinal, languageId);
            });
        }

        /// <summary>
        ///  chooses a random ParagraphTranslationBridge from a card, translated to the user's UI preferred language
        /// </summary>
        public static (string example, string translation) ParagraphExamplePullRandomByFlashCardId(
            IdiomaticaContext context, int flashCardId, string uiLanguageCode)
        {
            var example = string.Empty;
            var translation = string.Empty;
            
            // pull all bridges for this card with this language code
            var bridges = DataCache
                .FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCodeRead(
                    (flashCardId, uiLanguageCode), context);
            if (bridges is null || bridges.Count == 0) return (example, translation);
            
            // select a random bridge
            Random rng = new ();
            int position = rng.Next(0, bridges.Count);
            var bridge = bridges[position];

            // pull the paragraphtranslation
            if (bridge.ParagraphTranslationId is null) return (example, translation);
            var paragraphTranslation = DataCache.ParagraphTranslationByIdRead(
                (int)bridge.ParagraphTranslationId, context);
            if (paragraphTranslation is null || paragraphTranslation.TranslationText is null) 
                return (example, translation);
            translation = paragraphTranslation.TranslationText;

            // pull the orig text
            if (paragraphTranslation.ParagraphId is null) return (example, translation);
            var paragraph = DataCache.ParagraphByIdRead((int)paragraphTranslation.ParagraphId, context);
            if (paragraph is null || paragraph.Id is null) return (example, translation);
            example = ParagraphApi.ParagraphReadAllText(context, (int) paragraph.Id);

            return (example, translation);
        }
        public static async Task<(string example, string translation)> ParagraphExamplePullRandomByFlashCardIdAsync(
            IdiomaticaContext context, int flashCardId, string uiLanguageCode)
        {
            return await Task<(string example, string translation)>.Run(() =>
            {
                return ParagraphExamplePullRandomByFlashCardId(context, flashCardId, uiLanguageCode);
            });
        }


        public static string ParagraphReadAllText(
            IdiomaticaContext context, int paragraphId)
        {
            return ParagraphReadAllTextAsync(context, paragraphId).Result;
        }
        public static async Task<string> ParagraphReadAllTextAsync(
            IdiomaticaContext context, int paragraphId)
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
            return String.Join(" ", sentenceTexts);
        }


        public static List<Paragraph> ParagraphsCreateFromPage(
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
            var paragraphSplits = PotentialParagraphsSplitFromText(
                context, page.OriginalText, language.Code);
            int paragraphOrdinal = 0;
            foreach (var paragraphSplit in paragraphSplits)
            {
                var paragraphSplitTrimmed = paragraphSplit.Trim();
                if (string.IsNullOrEmpty(paragraphSplitTrimmed)) continue;
                var paragraph = ParagraphCreateFromSplit(context,
                    paragraphSplitTrimmed, pageId, paragraphOrdinal, languageId);
                if (paragraph is not null && paragraph.Id is not null or 0)
                {
                    paragraphs.Add(paragraph);
                }
                paragraphOrdinal++;
            }
            return paragraphs;
        }
        public static async Task<List<Paragraph>> ParagraphsCreateFromPageAsync(
            IdiomaticaContext context, int pageId, int languageId)
        {
            return await Task<List<Paragraph>>.Run(() =>
            {
                return ParagraphsCreateFromPage(context, pageId, languageId);
            });
        }


        public static List<Paragraph>? ParagraphsReadByPageId(
            IdiomaticaContext context, int pageId)
        {
            return DataCache.ParagraphsByPageIdRead(pageId, context);
        }
        public static async Task<List<Paragraph>?> ParagraphsReadByPageIdAsync(
            IdiomaticaContext context, int pageId)
        {
            return await DataCache.ParagraphsByPageIdReadAsync(pageId, context);
        }


        public static (string input, string output) ParagraphTranslate(
            IdiomaticaContext context, int paragraphId, string fromCode, string toCode)
        {
            return ParagraphTranslateAsync(context, paragraphId, fromCode, toCode).Result;
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
            
            var deeplResult = await DeepLService.TranslateAsync(input, fromCode, toCode);
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


        /// <summary>
        /// Used to take the string contents of the book creation page and 
        /// split it out into paragraphs (text, not Model.Paragraph) ahead of 
        /// creating page and paragraph objects
        /// </summary>
        public static string[] PotentialParagraphsSplitFromText(
            IdiomaticaContext context, string text, string languageCode)
        {
            if (string.IsNullOrEmpty(text)) { ErrorHandler.LogAndThrow(); return []; }
            if (string.IsNullOrEmpty(languageCode)) { ErrorHandler.LogAndThrow(); return []; }
            var textSanitized = text.Trim().Replace('\u00A0', ' ');
            

            // pull language from the db
            var language = DataCache.LanguageByCodeRead(languageCode, context);
            if (language is null)
            {
                ErrorHandler.LogAndThrow();
                return new string[] { };
            }

            // divide text into paragraphs
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            if (parser is null) { ErrorHandler.LogAndThrow(); return []; }
            var paragraphSplitsRaw = parser.SegmentTextByParagraphs(textSanitized);
            if(paragraphSplitsRaw is null || paragraphSplitsRaw.Length < 1)
                {  ErrorHandler.LogAndThrow(); return []; }
            return paragraphSplitsRaw;
        }
        public static async Task<string[]> PotentialParagraphsSplitFromTextAsync(
            IdiomaticaContext context, string text, string languageCode)
        {
            return await Task<string[]>.Run(() =>
            {
                return PotentialParagraphsSplitFromText(context, text, languageCode);
            });

        }

    }
}
