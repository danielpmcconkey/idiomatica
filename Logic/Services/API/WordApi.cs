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
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Model.Enums;

namespace Logic.Services.API
{
    public static class WordApi
    {
        public static WordTranslation? VerbWordTranslationSave(IdiomaticaContext context, 
            Verb verb, Language fromLanguage, Language toLanguage,
            string wordTextLower, string translation, int ordinal)
        {
            // look up the existing word
            var word = context.Words
                .Where(x => x.TextLowerCase == wordTextLower &&
                    x.LanguageKey == fromLanguage.UniqueKey)
                .FirstOrDefault();
            if (word is null)
            {
                // create it
                word = WordCreate(context, fromLanguage, wordTextLower, wordTextLower);
            }
            if (word is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            WordTranslation? wordTranslation = new ()
            {
                UniqueKey = Guid.NewGuid(),
                LanguageToKey = toLanguage.UniqueKey,
                LanguageTo = toLanguage,
                WordKey = word.UniqueKey,
                Word = word,
                PartOfSpeech = AvailablePartOfSpeech.VERB,
                Translation = translation.Trim(),
                VerbKey = verb.UniqueKey,
                Verb = verb,
                Ordinal = ordinal,
            };
            context.WordTranslations.Add(wordTranslation);
            context.SaveChanges();
            return wordTranslation;
        }
        public static Verb? VerbCreateAndSaveTranslations(IdiomaticaContext context,
            Verb learningVerb, Verb translationVerb, List<VerbConjugation> conjugations)
        {

            // note, it's not right to directly query frmo the API. But this
            // isn't a normal method, used by the app (I hope). It should only
            // be used for adding new word translations manually by system admins;
            var englishLang = LanguageApi.LanguageReadByCode(context, AvailableLanguageCode.EN_US);
            if (englishLang == null) { ErrorHandler.LogAndThrow(); return null; }
            Guid englishLangId = englishLang.UniqueKey;


            // save the learning verb object
            // but only if it doesn't already exist
            Verb? verbToUse = context.Verbs
                .Where(x=>x.LanguageKey == learningVerb.LanguageKey &&
                    x.Conjugator == learningVerb.Conjugator &&
                    x.Infinitive == learningVerb.Infinitive)
                .FirstOrDefault();
            if (verbToUse is null)
            {
                verbToUse = learningVerb;
                verbToUse.UniqueKey = Guid.NewGuid();
                context.Verbs.Add(verbToUse);
                context.SaveChanges();
            }
            if (verbToUse is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            
            // save the infinitive translation
            VerbWordTranslationSave(context, verbToUse,
                verbToUse.Language, translationVerb.Language,
                verbToUse.Infinitive, translationVerb.Infinitive, 1);

            if (verbToUse.Gerund is not null && translationVerb.Gerund is not null)
            {
                // save the gerund translation
                var gerundTranslation = translationVerb.Gerund;
                if (translationVerb.LanguageKey == englishLangId)
                {
                    gerundTranslation = $"{translationVerb.Gerund}: gerund of {learningVerb.Infinitive}";
                }

                VerbWordTranslationSave(context, verbToUse,
                    verbToUse.Language, translationVerb.Language,
                    verbToUse.Gerund, gerundTranslation, 100);
            }
            if (verbToUse.PastParticiple is not null && translationVerb.PastParticiple is not null)
            {
                // save the past participle translation
                var participleTranslation = translationVerb.PastParticiple;
                if (translationVerb.LanguageKey == englishLangId)
                {
                    participleTranslation = $"{translationVerb.PastParticiple}: past participle of {learningVerb.Infinitive}";
                }
                VerbWordTranslationSave(context, verbToUse,
                    verbToUse.Language, translationVerb.Language,
                    verbToUse.PastParticiple, participleTranslation, 100);
            }
            // save the conjugation translations
            foreach (var conjugation in conjugations)
            {
                var wordInLearningLang = conjugation.ToString();
                var wordInKnownLang = conjugation.Translation;
                if (string.IsNullOrEmpty(wordInLearningLang))
                    { ErrorHandler.LogAndThrow(); return null; }
                if (string.IsNullOrEmpty(wordInKnownLang))
                    { ErrorHandler.LogAndThrow(); return null; }
                VerbWordTranslationSave(context, verbToUse,
                    verbToUse.Language, translationVerb.Language,
                    wordInLearningLang, wordInKnownLang, conjugation.Ordinal);
            }

            return verbToUse;
        }
        public static Word? WordCreate(IdiomaticaContext context,
            Language language, string text, string romanization)
        {
            var newWord = new Word()
            {
                UniqueKey = Guid.NewGuid(),
                LanguageKey = language.UniqueKey,
                Language = language,
                Romanization = romanization,
                Text = text.ToLower(),
                TextLowerCase = text.ToLower(),
            };
            newWord = DataCache.WordCreate(newWord, context);
            if (newWord is null)
            {
                ErrorHandler.LogAndThrow();
                return newWord;
            }
            return newWord;
        }        
        public static async Task<Word?> WordCreateAsync(IdiomaticaContext context,
            Language language, string text, string romanization)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordCreate(context, language, text, romanization);
            });
        }


        public static Word? WordGetById(IdiomaticaContext context, Guid wordId)
        {
            return DataCache.WordByIdRead(wordId, context);
        }
        public static async Task<Word?> WordGetByIdAsync(IdiomaticaContext context, Guid wordId)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordGetById(context, wordId);
            });
        }


        public static Word? WordReadByLanguageIdAndText(
            IdiomaticaContext context, Guid languageId, string text)
        {
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();
            return DataCache.WordByLanguageIdAndTextLowerRead((languageId, text), context);
        }
        public static async Task<Word?> WordReadByLanguageIdAndTextAsync(
            IdiomaticaContext context, Guid languageId, string text)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordReadByLanguageIdAndText(context, languageId, text);
            });
        }


        public static List<(Word? word, int? ordinal, string? tokenDisplay)> WordsCreateOrderedFromSentenceId(
            IdiomaticaContext context, Language language, Sentence sentence)
        {
            List<(Word? word, int? ordinal, string? tokenDisplay)> outList = [];

            // check if any already exist. there shouldn't be any but whateves
            DataCache.TokenBySentenceIdDelete(sentence.UniqueKey, context);
            
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            if (parser is null) { ErrorHandler.LogAndThrow(); return []; }
            var wordsSplits = parser.SegmentTextByWordsKeepingPunctuation(sentence.Text);
            if (wordsSplits is null || wordsSplits.Length == 0)
                return outList;
            
            for (int i = 0; i < wordsSplits.Length; i++)
            {
                Word? word = null;
                int? ordinal = i;
                string? tokenDisplay = wordsSplits[i];
                var cleanWord = parser.TextToLower(parser.StipNonWordCharacters(tokenDisplay));
                word = DataCache.WordByLanguageIdAndTextLowerRead(
                    (language.UniqueKey, cleanWord), context);
                if (word is null)
                {
                    // word doesn't exist; create it
                    word = WordApi.WordCreate(
                        context, language, cleanWord, cleanWord);
                }
                
                outList.Add((word, i, tokenDisplay));
            }
            return outList;
        }
        public static async Task<List<(Word? word, int? ordinal, string? tokenDisplay)>> 
            WordsCreateOrderedFromSentenceIdAsync(
                IdiomaticaContext context, Language language, Sentence sentence)
        {
            return await Task<List<(Word? word, int? ordinal, string? tokenDisplay)>>.Run(() =>
            {
                return WordsCreateOrderedFromSentenceId(context, language, sentence);
            });
        }

        
        public static Dictionary<string, Word>? WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
            IdiomaticaContext context, Guid pageId, Guid languageUserId)
        {
            return DataCache
                .WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
                (pageId, languageUserId), context);
        }
        public static async Task<Dictionary<string, Word>?> WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdReadAsync(
            IdiomaticaContext context, Guid pageId, Guid languageUserId)
        {
            return await Task<List<(Word? word, int? ordinal, string? tokenDisplay)>>.Run(() =>
            {
                return WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
                    context, pageId, languageUserId);
            });
        }

        public static Dictionary<string, Word>? WordsDictReadByPageId(
            IdiomaticaContext context, Guid pageId)
        {
            return DataCache.WordsDictByPageIdRead(pageId, context);
        }
        public static async Task<Dictionary<string, Word>?> WordsDictReadByPageIdAsync(
            IdiomaticaContext context, Guid pageId)
        {
            return await DataCache.WordsDictByPageIdReadAsync(pageId, context);
        }


        public static List<(string language, int wordCount)> WordsGetListOfReadCount(
            IdiomaticaContext context, Guid userId)
        {
            List<(string language, int wordCount)> returnList = new List<(string language, int wordCount)>();
            var languageUsers = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(context, userId);
            if (languageUsers is null)
            {
                return new();
            }
            foreach (var languageUser in languageUsers)
            {
                if (languageUser.Language == null || languageUser.Language.Name == null) continue;
                var count = (from lu in context.LanguageUsers
                             join bu in context.BookUsers on lu.UniqueKey equals bu.LanguageUserKey
                             join pu in context.PageUsers on bu.UniqueKey equals pu.BookUserKey
                             join p in context.Pages on pu.PageKey equals p.UniqueKey
                             join pp in context.Paragraphs on p.UniqueKey equals pp.PageKey
                             join s in context.Sentences on pp.UniqueKey equals s.ParagraphKey
                             join t in context.Tokens on s.UniqueKey equals t.SentenceKey
                             where pu.ReadDate != null
                                && lu.UniqueKey == languageUser.UniqueKey
                             select t).Count();
                returnList.Add((languageUser.Language.Name, count));
            }
            return returnList;
        }
        public static async Task<List<(string language, int wordCount)>> WordsGetListOfReadCountAsync(
           IdiomaticaContext context, Guid userId)
        {
            return await Task<List<(string language, int wordCount)>>.Run(() =>
            {
                return WordsGetListOfReadCount(context, userId);
            });
        }

        public static List<WordTranslation> WordTranslationsReadByWordId(
            IdiomaticaContext context, Guid wordId)
        {
            return DataCache.WordTranslationsByWordIdRead(wordId, context);
        }
    }
}
