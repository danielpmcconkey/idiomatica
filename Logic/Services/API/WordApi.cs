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
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Model.Enums;

namespace Logic.Services.API
{
    public static class WordApi
    {
        public static WordTranslation? VerbWordTranslationSave(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, 
            Verb verb, Language fromLanguage, Language toLanguage, string wordTextLower,
            string translation, int ordinal, bool saveContext = true)
        {
            var context = dbContextFactory.CreateDbContext();
            // look up the existing word
            var word = context.Words
                .Where(x => x.TextLowerCase == wordTextLower &&
                    x.LanguageId == fromLanguage.Id)
                .FirstOrDefault();
            if (word is null)
            {
                // create it
                word = WordCreate(dbContextFactory, fromLanguage, wordTextLower, wordTextLower);
            }
            if (word is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            WordTranslation? wordTranslation = new ()
            {
                Id = Guid.NewGuid(),
                LanguageToId = toLanguage.Id,
                //LanguageTo = toLanguage,
                WordId = word.Id,
                //Word = word,
                PartOfSpeech = AvailablePartOfSpeech.VERB,
                Translation = translation.Trim(),
                VerbId = verb.Id,
                //Verb = verb,
                Ordinal = ordinal,
            };
            context.WordTranslations.Add(wordTranslation);
            if (saveContext) context.SaveChanges(); // this gets called a bunch, so maybe don't save inside the function
            return wordTranslation;
        }
        public static Verb? VerbCreateAndSaveTranslations(IDbContextFactory<IdiomaticaContext> dbContextFactory,
            Verb learningVerb, Verb translationVerb, List<VerbConjugation> conjugations)
        {

            // note, it's not right to directly query frmo the API. But this
            // isn't a normal method, used by the app (I hope). It should only
            // be used for adding new word translations manually by system admins;
            var englishLang = LanguageApi.LanguageReadByCode(dbContextFactory, AvailableLanguageCode.EN_US);
            if (englishLang == null) { ErrorHandler.LogAndThrow(); return null; }
            Guid englishLangId = englishLang.Id;

            var context = dbContextFactory.CreateDbContext();

            context.Verbs.Add(learningVerb);
            context.SaveChanges();

            var languageLearning = LanguageApi.LanguageRead(dbContextFactory, learningVerb.LanguageId);
            if (languageLearning is null) { ErrorHandler.LogAndThrow(); return null; }
            var languageTranslation = LanguageApi.LanguageRead(dbContextFactory, translationVerb.LanguageId);
            if (languageTranslation is null) { ErrorHandler.LogAndThrow(); return null; }

            // save the infinitive translation
            VerbWordTranslationSave(dbContextFactory, learningVerb,
                languageLearning, languageTranslation,
                learningVerb.Infinitive, translationVerb.Infinitive, 1, false);

            if (learningVerb.Gerund is not null && translationVerb.Gerund is not null)
            {
                // save the gerund translation
                var gerundTranslation = translationVerb.Gerund;
                if (translationVerb.LanguageId == englishLangId)
                {
                    gerundTranslation = $"{translationVerb.Gerund}: gerund of {learningVerb.Infinitive}";
                }

                VerbWordTranslationSave(dbContextFactory, learningVerb,
                    languageLearning, languageTranslation,
                    learningVerb.Gerund, gerundTranslation, 100, false);
            }
            if (learningVerb.PastParticiple is not null && translationVerb.PastParticiple is not null)
            {
                // save the past participle translation
                var participleTranslation = translationVerb.PastParticiple;
                if (translationVerb.LanguageId == englishLangId)
                {
                    participleTranslation = $"{translationVerb.PastParticiple}: past participle of {learningVerb.Infinitive}";
                }
                VerbWordTranslationSave(dbContextFactory, learningVerb,
                    languageLearning, languageTranslation,
                    learningVerb.PastParticiple, participleTranslation, 100, false);
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
                VerbWordTranslationSave(dbContextFactory, learningVerb,
                    languageLearning, languageTranslation,
                    wordInLearningLang, wordInKnownLang, conjugation.Ordinal, false);
            }
            context.SaveChanges();
            return learningVerb;
        }
        public static Word? WordCreate(IDbContextFactory<IdiomaticaContext> dbContextFactory,
            Language language, string text, string romanization)
        {
            var newWord = new Word()
            {
                Id = Guid.NewGuid(),
                LanguageId = language.Id,
                Language = language,
                Romanization = romanization,
                Text = text.ToLower(),
                TextLowerCase = text.ToLower(),
            };
            newWord = DataCache.WordCreate(newWord, dbContextFactory);
            if (newWord is null)
            {
                ErrorHandler.LogAndThrow();
                return newWord;
            }
            return newWord;
        }        
        public static async Task<Word?> WordCreateAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory,
            Language language, string text, string romanization)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordCreate(dbContextFactory, language, text, romanization);
            });
        }


        public static Word? WordGetById(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid wordId)
        {
            return DataCache.WordByIdRead(wordId, dbContextFactory);
        }
        public static async Task<Word?> WordGetByIdAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid wordId)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordGetById(dbContextFactory, wordId);
            });
        }


        public static Word? WordReadByLanguageIdAndText(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageId, string text)
        {
            if (string.IsNullOrEmpty(text)) ErrorHandler.LogAndThrow();
            return DataCache.WordByLanguageIdAndTextLowerRead((languageId, text), dbContextFactory);
        }
        public static async Task<Word?> WordReadByLanguageIdAndTextAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageId, string text)
        {
            return await Task<Word?>.Run(() =>
            {
                return WordReadByLanguageIdAndText(dbContextFactory, languageId, text);
            });
        }


        public static List<(Word? word, int? ordinal, string? tokenDisplay)> WordsCreateOrderedFromSentenceId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Language language, Sentence sentence)
        {
            List<(Word? word, int? ordinal, string? tokenDisplay)> outList = [];

            // check if any already exist. there shouldn't be any but whateves
            DataCache.TokenBySentenceIdDelete(sentence.Id, dbContextFactory);
            
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
                    (language.Id, cleanWord), dbContextFactory);
                if (word is null)
                {
                    // word doesn't exist; create it
                    word = WordApi.WordCreate(
                        dbContextFactory, language, cleanWord, cleanWord);
                }
                
                outList.Add((word, i, tokenDisplay));
            }
            return outList;
        }
        public static async Task<List<(Word? word, int? ordinal, string? tokenDisplay)>> 
            WordsCreateOrderedFromSentenceIdAsync(
                IDbContextFactory<IdiomaticaContext> dbContextFactory, Language language, Sentence sentence)
        {
            return await Task<List<(Word? word, int? ordinal, string? tokenDisplay)>>.Run(() =>
            {
                return WordsCreateOrderedFromSentenceId(dbContextFactory, language, sentence);
            });
        }

        
        public static Dictionary<string, Word>? WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId, Guid languageUserId)
        {
            return DataCache
                .WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
                (pageId, languageUserId), dbContextFactory);
        }
        public static async Task<Dictionary<string, Word>?> WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdReadAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId, Guid languageUserId)
        {
            return await Task<List<(Word? word, int? ordinal, string? tokenDisplay)>>.Run(() =>
            {
                return WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
                    dbContextFactory, pageId, languageUserId);
            });
        }

        public static Dictionary<string, Word>? WordsDictReadByPageId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId)
        {
            return DataCache.WordsDictByPageIdRead(pageId, dbContextFactory);
        }
        public static async Task<Dictionary<string, Word>?> WordsDictReadByPageIdAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid pageId)
        {
            return await DataCache.WordsDictByPageIdReadAsync(pageId, dbContextFactory);
        }


        public static List<(string language, int wordCount)> WordsGetListOfReadCount(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            List<(string language, int wordCount)> returnList = new List<(string language, int wordCount)>();
            var languageUsers = LanguageUserApi.LanguageUsersAndLanguageGetByUserId(dbContextFactory, userId);
            if (languageUsers is null)
            {
                return new();
            }

            var context = dbContextFactory.CreateDbContext();

            foreach (var languageUser in languageUsers)
            {
                if (languageUser.Language == null || languageUser.Language.Name == null) continue;
                var count = (from lu in context.LanguageUsers
                             join bu in context.BookUsers on lu.Id equals bu.LanguageUserId
                             join pu in context.PageUsers on bu.Id equals pu.BookUserId
                             join p in context.Pages on pu.PageId equals p.Id
                             join pp in context.Paragraphs on p.Id equals pp.PageId
                             join s in context.Sentences on pp.Id equals s.ParagraphId
                             join t in context.Tokens on s.Id equals t.SentenceId
                             where pu.ReadDate != null
                                && lu.Id == languageUser.Id
                             select t).Count();
                returnList.Add((languageUser.Language.Name, count));
            }
            return returnList;
        }
        public static async Task<List<(string language, int wordCount)>> WordsGetListOfReadCountAsync(
           IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
        {
            return await Task<List<(string language, int wordCount)>>.Run(() =>
            {
                return WordsGetListOfReadCount(dbContextFactory, userId);
            });
        }

        public static List<WordTranslation> WordTranslationsReadByWordId(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid wordId)
        {
            return DataCache.WordTranslationsByWordIdRead(wordId, dbContextFactory);
        }
    }
}
