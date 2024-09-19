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
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class ParagraphApi
    {
        public static Paragraph? ParagraphCreateFromSplit(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string splitText, Page page,
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
            paragraph = DataCache.ParagraphCreate(paragraph, dbContextFactory);
            if (paragraph is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // now create the sentences
            var sentenceSplits = SentenceApi.PotentialSentencesSplitFromText(
                dbContextFactory, splitText, language);
            int sentenceOrdinal = 0;
            foreach (var sentenceSplit in sentenceSplits)
            {
                if (string.IsNullOrEmpty(sentenceSplit)) continue;
                var trimmedSentenceSplit = sentenceSplit.Trim();
                if (string.IsNullOrEmpty(trimmedSentenceSplit)) continue;
                var sentence = SentenceApi.SentenceCreate(
                    dbContextFactory, trimmedSentenceSplit, language, sentenceOrdinal, paragraph);
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
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string splitText, Page page,
            int ordinal, Language language)
        {
            return await Task<Paragraph?>.Run(() =>
            {
                return ParagraphCreateFromSplit(dbContextFactory, splitText, page, ordinal, language);
            });
        }

        /// <summary>
        ///  chooses a random ParagraphTranslationBridge from a card, translated to the user's UI preferred language
        /// </summary>
        public static (string example, string translation) ParagraphExamplePullRandomByFlashCardId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid flashCardId, AvailableLanguageCode uiLanguageCode)
        {
            var example = string.Empty;
            var translation = string.Empty;

            var uiLanguage = LanguageApi.LanguageReadByCode(dbContextFactory, uiLanguageCode);
            if (uiLanguage is null) { ErrorHandler.LogAndThrow(); return (example, translation); }
            
            // pull all bridges for this card with this language code
            var bridges = DataCache
                .FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCodeRead(
                    (flashCardId, uiLanguage.Id), dbContextFactory);
            if (bridges is null || bridges.Count == 0) return (example, translation);
            
            // select a random bridge
            Random rng = new ();
            int position = rng.Next(0, bridges.Count);
            var bridge = bridges[position];

            // pull the paragraphtranslation
            var paragraphTranslation = DataCache.ParagraphTranslationByIdRead(
                (Guid)bridge.ParagraphTranslationId, dbContextFactory);
            if (paragraphTranslation is null || paragraphTranslation.TranslationText is null) 
                return (example, translation);
            translation = paragraphTranslation.TranslationText;

            // pull the orig text
            var paragraph = DataCache.ParagraphByIdRead((Guid)paragraphTranslation.ParagraphId, dbContextFactory);
            if (paragraph is null) return (example, translation);
            example = ParagraphApi.ParagraphReadAllText(dbContextFactory, (Guid) paragraph.Id);

            return (example, translation);
        }
        public static async Task<(string example, string translation)> ParagraphExamplePullRandomByFlashCardIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid flashCardId, AvailableLanguageCode uiLanguageCode)
        {
            return await Task<(string example, string translation)>.Run(() =>
            {
                return ParagraphExamplePullRandomByFlashCardId(dbContextFactory, flashCardId, uiLanguageCode);
            });
        }


        public static string ParagraphReadAllText(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid paragraphId)
        {
            return ParagraphReadAllTextAsync(dbContextFactory, paragraphId).Result;
        }
        public static async Task<string> ParagraphReadAllTextAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid paragraphId)
        {
            var sentences = await DataCache.SentencesByParagraphIdReadAsync(paragraphId, dbContextFactory);
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
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Page page, Language language)
        {
            var paragraphs = new List<Paragraph>();
            var paragraphSplits = PotentialParagraphsSplitFromText(
                dbContextFactory, page.OriginalText, language);
            int paragraphOrdinal = 0;
            foreach (var paragraphSplit in paragraphSplits)
            {
                var paragraphSplitTrimmed = paragraphSplit.Trim();
                if (string.IsNullOrEmpty(paragraphSplitTrimmed)) continue;
                var paragraph = ParagraphCreateFromSplit(dbContextFactory,
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
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Page page, Language language)
        {
            return await Task<List<Paragraph>>.Run(() =>
            {
                return ParagraphsCreateFromPage(dbContextFactory, page, language);
            });
        }


        public static List<Paragraph>? ParagraphsReadByPageId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId)
        {
            return DataCache.ParagraphsByPageIdRead(pageId, dbContextFactory);
        }
        public static async Task<List<Paragraph>?> ParagraphsReadByPageIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId)
        {
            return await DataCache.ParagraphsByPageIdReadAsync(pageId, dbContextFactory);
        }


        public static (string input, string output) ParagraphTranslate(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Paragraph paragraph,
            AvailableLanguageCode fromCode, AvailableLanguageCode toCode)
        {
            return ParagraphTranslateAsync(dbContextFactory, paragraph, fromCode, toCode).Result;
        }
        public static async Task<(string input, string output)> ParagraphTranslateAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Paragraph paragraph,
            AvailableLanguageCode fromCode, AvailableLanguageCode toCode)
        {
            string input = await ParagraphReadAllTextAsync(dbContextFactory, paragraph.Id);
            string output = "";


            var languageFrom = LanguageApi.LanguageReadByCode(dbContextFactory, fromCode);
            var languageTo = LanguageApi.LanguageReadByCode(dbContextFactory, toCode);

            if (languageFrom == null || languageTo == null) { ErrorHandler.LogAndThrow(); return (input, output); }

            // see if the translation already exists
            var existingTranslations = await DataCache.ParagraphTranslationsByParargraphIdReadAsync(
                    paragraph.Id, dbContextFactory);
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
                ppt = await DataCache.ParagraphTranslationCreateAsync(ppt, dbContextFactory);
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
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string text, Language language)
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
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string text, Language language)
        {
            return await Task<string[]>.Run(() =>
            {
                return PotentialParagraphsSplitFromText(dbContextFactory, text, language);
            });

        }

    }
}
