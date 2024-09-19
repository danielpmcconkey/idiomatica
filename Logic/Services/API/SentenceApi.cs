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
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class SentenceApi
    {
        public static string[] PotentialSentencesSplitFromText(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string text, Language language)
        {
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();

            var parser = LanguageParser.Factory.GetLanguageParser(language);
            if(parser is null) { ErrorHandler.LogAndThrow(); return []; }
            return parser.SegmentTextBySentences(text);
        }
        public static async Task<string[]> PotentialSentencesSplitFromTextAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string text, Language language)
        {
            return await Task<string[]>.Run(() =>
            {
                return PotentialSentencesSplitFromText(dbContextFactory, text, language);
            });
        }


        public static Sentence? SentenceCreate(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string text, Language language, int ordinal,
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
            newSentence = DataCache.SentenceCreate(newSentence, dbContextFactory);
            if (newSentence is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            newSentence.Tokens = TokenApi.TokensCreateFromSentence(dbContextFactory,
                newSentence, language);
            return newSentence;
        }
        public static async Task<Sentence?> SentenceCreateAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string text, Language language, int ordinal,
            Paragraph paragraph)
        {
            return await Task<Sentence?>.Run(() =>
            {
                return SentenceCreate(dbContextFactory, text, language, ordinal, paragraph);
            });
        }


        public static List<Sentence>? SentencesReadByPageId(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId)
        {
            return DataCache.SentencesByPageIdRead(pageId, dbContextFactory);
        }
        public static async Task<List<Sentence>?> SentencesReadByPageIdAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId)
        {
            return await DataCache.SentencesByPageIdReadAsync(pageId, dbContextFactory);
        }


        public static List<Sentence>? SentencesReadByParagraphId(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid paragraphId)
        {
            return DataCache.SentencesByParagraphIdRead(paragraphId, dbContextFactory);
        }
        public static async Task<List<Sentence>?> SentencesReadByParagraphIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid paragraphId)
        {
            return await DataCache.SentencesByParagraphIdReadAsync(paragraphId, dbContextFactory);
        }

    }
}
