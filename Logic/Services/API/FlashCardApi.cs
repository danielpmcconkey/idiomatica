using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DeepL;
using System.Linq.Expressions;
using Model.Enums;

namespace Logic.Services.API
{
    public static class FlashCardApi
    {
        public static FlashCard? FlashCardCreate(
            DiContainer diContainer, Guid wordUserId, AvailableLanguageCode uiLanguageCode)
        {
            var context = diContainer.DbContextFactory.CreateDbContext();

            var wordUser = DataCache.WordUserAndLanguageUserAndLanguageByIdRead(wordUserId, context);
            if (wordUser == null) { ErrorHandler.LogAndThrow(); return null; }

            var uiLanguage = LanguageApi.LanguageReadByCode(context, uiLanguageCode);
            if (uiLanguage == null) { ErrorHandler.LogAndThrow(); return null; }

            FlashCard? card = new ()
            {
                Id = Guid.NewGuid(),
                WordUser = wordUser,
                WordUserId = wordUserId,
                Status = AvailableFlashCardStatus.ACTIVE,
            };
            card = DataCache.FlashCardCreate(card, context);
            if (card is null) { ErrorHandler.LogAndThrow(); return null; }

            ErrorHandler.LogMessage(
                AvailableLogMessageTypes.DEBUG, $"created flash card {card.Id} for wordUser.Id {wordUser.Id}.", context);
            
            List<Word> wordUsages = DataCache.WordsAndTokensAndSentencesAndParagraphsByWordIdRead(
                (Guid)wordUser.WordId, context);

            int maxPptPerCard = 5;
            int currentPptThisCard = 0;

            // we create a hashset of paragraphs already used in this card
            // because sometimes, the same word apprears multiple times in a
            // paragraph and that would cause a violation of the unique
            // constraint between a flashcard and a paragraph translation in
            // the bridge object
            HashSet<Guid> paragraphsAlreadyChecked = [];

            foreach (var wordUsage in wordUsages)
            {
                if (currentPptThisCard >= maxPptPerCard) continue;
                foreach (var token in wordUsage.Tokens)
                {
                    if (currentPptThisCard >= maxPptPerCard) continue;

                    if (token.Sentence == null) { ErrorHandler.LogAndThrow(); return null; }
                    var sentence = token.Sentence;
                    if (sentence.Paragraph is null) 
                        { ErrorHandler.LogAndThrow(); return null; }
                    var paragraph = sentence.Paragraph;
                    if (paragraphsAlreadyChecked.Contains(paragraph.Id)) continue;
                    ErrorHandler.LogMessage(
                        AvailableLogMessageTypes.DEBUG, $"paragraph {paragraph.Id} has been checked.", context);


                    // pull paragraph translations here in case the same
                    // word is used more than once in the same paragraph
                    var paragraphTranslations = DataCache.ParagraphTranslationsByParargraphIdRead(
                        (Guid)paragraph.Id, context);

                    ParagraphTranslation? ppts = null;
                    if (paragraphTranslations.Count > 0)
                    {
                        ErrorHandler.LogMessage(
                            AvailableLogMessageTypes.DEBUG, $"there are paragraph translations for {paragraph.Id}.", context);

                        ErrorHandler.LogMessage(
                            AvailableLogMessageTypes.DEBUG, $"UI language code is {uiLanguageCode}.", context);


                        // are they the right language?
                        ppts = paragraphTranslations
                            .Where(
                                ppt => ppt.LanguageId == uiLanguage.Id)
                            .FirstOrDefault();
                        if (ppts is null)
                        {
                            // not the right language
                            // reset to null so the code below will create it
                            ErrorHandler.LogMessage(
                                AvailableLogMessageTypes.DEBUG, "No paragraphs with the right language.", context);

                            ppts = null;
                        }
                    }
                    if (ppts == null)
                    {
                        if (wordUser.LanguageUser == null
                            || wordUser.LanguageUser.Language is null)
                        {
                            ErrorHandler.LogAndThrow();
                            return null;
                        }
                        // create it
                        ErrorHandler.LogMessage(
                            AvailableLogMessageTypes.DEBUG, $"Creating a paragraph translation for {paragraph.Id}.", context);

                        string input = ParagraphApi.ParagraphReadAllText(context, paragraph.Id);
                        Language? toLang = LanguageApi.LanguageReadByCode(context, uiLanguageCode);
                        if (toLang is null) { ErrorHandler.LogAndThrow(); return null; }
                        AvailableLanguageCode fromLangCode = wordUser.LanguageUser.Language.Code;
                        string translation = DeepLService.Translate(input, fromLangCode, toLang.Code);
                        ppts = new ()
                        {
                            Id = Guid.NewGuid(),
                            ParagraphId = paragraph.Id,
                            Paragraph = paragraph,
                            LanguageId = toLang.Id,
                            Language = toLang,
                            TranslationText = translation
                        };
                        ppts = DataCache.ParagraphTranslationCreate(ppts, context);
                    }

                    if (ppts is null) { ErrorHandler.LogAndThrow(); return null; }

                    // now bridge it to the flash card
                    FlashCardParagraphTranslationBridge? fcptb = new ()
                    {
                        Id = Guid.NewGuid(),
                        ParagraphTranslationId = ppts.Id,
                        ParagraphTranslation = ppts,
                        FlashCardId = card.Id,
                        FlashCard = card,
                    };
                    fcptb = DataCache.FlashCardParagraphTranslationBridgeCreate(fcptb, context);
                    if(fcptb is null) { ErrorHandler.LogAndThrow(); return null; }
                    paragraphsAlreadyChecked.Add(paragraph.Id); // keep it from adding this same PP twice
                    fcptb.FlashCard = card;
                    fcptb.ParagraphTranslation = ppts;
                    card.FlashCardParagraphTranslationBridges.Add(fcptb);
                    currentPptThisCard++;
                }
            }
            
            return card;
        }
        public static async Task<FlashCard?> FlashCardCreateAsync(
            DiContainer diContainer, Guid wordUserId, AvailableLanguageCode uiLanguageCode)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardCreate(diContainer, wordUserId, uiLanguageCode);
            });
        }


        public static List<FlashCard> FlashCardDeckShuffle(List<FlashCard> deck)
        {
            Random rng = new Random();
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
            return deck;
        }
        public static async Task<List<FlashCard>> FlashCardDeckShuffleAsync(List<FlashCard> deck)
        {
            return await Task<List<FlashCard>>.Run(() =>
            {
                return FlashCardDeckShuffle(deck);
            });
        }


        public static FlashCard? FlashCardReadById(
           IdiomaticaContext context, Guid Id)
        {
            return DataCache.FlashCardByIdRead(Id, context);
        }
        public static async Task<FlashCard?> FlashCardReadByIdAsync(
           IdiomaticaContext context, Guid Id)
        {
            return await DataCache.FlashCardByIdReadAsync(Id, context);
        }
        public static FlashCard? FlashCardReadByWordUserId(
          IdiomaticaContext context, Guid wordUserId)
        {
            return DataCache.FlashCardByWordUserIdRead(wordUserId, context);
        }
        public static async Task<FlashCard?> FlashCardReadByWordUserIdAsync(
           IdiomaticaContext context, Guid wordUserId)
        {
            return await DataCache.FlashCardByWordUserIdReadAsync(wordUserId, context);
        }


        public static List<FlashCard>? FlashCardsCreate(
            DiContainer diContainer, Guid languageUserId, int numCards,
            AvailableLanguageCode uiLanguageCode)
        {
            if (numCards < 1) { return new List<FlashCard>(); }
            var context = diContainer.DbContextFactory.CreateDbContext();
            
            List<FlashCard> cards = new List<FlashCard>();

            // get word users that don't already have a flash card
            // ordered by recent status change
            var wordUsers = (
                        from wu in context.WordUsers
                        join w in context.Words on wu.WordId equals w.Id
                        join fc in context.FlashCards on wu.Id equals fc.WordUserId into grouping
                        from fc in grouping.DefaultIfEmpty()
                        where (
                            wu.LanguageUserId == languageUserId
                            && fc == null
                            && wu.Status != AvailableWordUserStatus.LEARNED
                            && wu.Status != AvailableWordUserStatus.IGNORED
                            && wu.Status != AvailableWordUserStatus.WELLKNOWN
                            && wu.Status != AvailableWordUserStatus.UNKNOWN
                            && wu.Translation != null
                            && wu.Translation != string.Empty
                            )
                        select wu
                    )
                .OrderByDescending(x => x.StatusChanged)
                .Take(numCards)
                .ToList();



            foreach (var wordUser in wordUsers)
            {
                if(wordUser is null) continue;
                var card = FlashCardCreate(diContainer, (Guid)wordUser.Id, uiLanguageCode);
                if (card != null) cards.Add(card);
            }
            return cards;
        }
        public static async Task<List<FlashCard>?> FlashCardsCreateAsync(
            DiContainer diContainer, Guid languageUserId, int numCards, AvailableLanguageCode uiLanguageCode)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardsCreate(diContainer, languageUserId, numCards, uiLanguageCode);
            });
        }


        public static List<FlashCard>? FlashCardsFetchByNextReviewDateByPredicate(
            IdiomaticaContext context, Expression<Func<FlashCard, bool>> predicate, int take)
        {
            return DataCache.FlashCardsActiveAndFullRelationshipsByPredicateRead(
                predicate, take, context);
        }
        public static async Task<List<FlashCard>?> FlashCardsFetchByNextReviewDateByPredicateAsync(
            IdiomaticaContext context, Expression<Func<FlashCard, bool>> predicate, int take)
        {
            return await Task<List<FlashCard>?>.Run(() =>
            {
                return FlashCardsFetchByNextReviewDateByPredicate(context, predicate, take);
            });
        }


        public static FlashCard? FlashCardUpdate(
            IdiomaticaContext context, Guid cardId, Guid wordUserId, 
            AvailableFlashCardStatus status, DateTime nextReview, Guid id)
        {
            var card = DataCache.FlashCardByIdRead(cardId, context);
            if (card == null) { ErrorHandler.LogAndThrow(); return null; }
            card.WordUserId = wordUserId;
            card.NextReview = nextReview;
            card.Status = status;
            card.Id = id;
            DataCache.FlashCardUpdate(card, context);
            return card;
        }
        public static async Task<FlashCard?> FlashCardUpdateAsync(
            IdiomaticaContext context, Guid cardId, Guid wordUserId, 
            AvailableFlashCardStatus status, DateTime nextReview, Guid id)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardUpdate(context, cardId, wordUserId, status, nextReview, id);
            });
        }
    }
}
