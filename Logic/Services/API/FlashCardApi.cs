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

namespace Logic.Services.API
{
    public static class FlashCardApi
    {
        public static FlashCard? FlashCardCreate(IdiomaticaContext context, Guid wordUserId, string uiLanguageCode)
        {
            if (string.IsNullOrEmpty(uiLanguageCode)) { ErrorHandler.LogAndThrow(); return null; }
            FlashCard? card = new FlashCard();
            var wordUser = DataCache.WordUserAndLanguageUserAndLanguageByIdRead(wordUserId, context);

            if (wordUser == null) { ErrorHandler.LogAndThrow(); return null; }
            card.WordUser = wordUser;
            card.WordUserKey = wordUserId;
            card.NextReview = null;
            card.Status = AvailableFlashCardStatus.ACTIVE;
            card = DataCache.FlashCardCreate(card, context);
            if (card is null || card.UniqueKey is null) { ErrorHandler.LogAndThrow(); return null; }
            if (wordUser.WordKey is null) { ErrorHandler.LogAndThrow(); return null; }

            List<Word> wordUsages = DataCache.WordsAndTokensAndSentencesAndParagraphsByWordIdRead(
                (Guid)wordUser.WordKey, context);

            int maxPptPerCard = 5;
            int currentPptThisCard = 0;

            foreach (var wordUsage in wordUsages)
            {
                if (currentPptThisCard >= maxPptPerCard) continue;
                foreach (var token in wordUsage.Tokens)
                {
                    if (currentPptThisCard >= maxPptPerCard) continue;

                    if (token.Sentence == null) { ErrorHandler.LogAndThrow(); return null; }
                    var sentence = token.Sentence;
                    if (sentence.Paragraph == null || sentence.Paragraph.UniqueKey == null) 
                        { ErrorHandler.LogAndThrow(); return null; }
                    var paragraph = sentence.Paragraph;
                    // pull paragraph translations here in case the same
                    // word is used more than once in the same paragraph
                    var paragraphTranslations = DataCache.ParagraphTranslationsByParargraphIdRead(
                        (Guid)paragraph.UniqueKey, context);

                    ParagraphTranslation? ppts = null;
                    if (paragraphTranslations.Count > 0)
                    {
                        // are they the right language?
                        ppts = paragraphTranslations
                            .Where(
                                ppt => ppt != null
                                && ppt.LanguageCode != null
                                && ppt.LanguageCode.Code == uiLanguageCode)
                            .FirstOrDefault();
                        if (ppts == null || ppts.UniqueKey == null)
                        {
                            // not the right language
                            // reset to null so the code below will create it
                            ppts = null;
                        }
                    }
                    if (ppts == null)
                    {
                        if (wordUser.LanguageUser == null
                            || wordUser.LanguageUser.Language == null
                            || wordUser.LanguageUser.Language.Code == null)
                        {
                            ErrorHandler.LogAndThrow(2500);
                            return null;
                        }
                        // create it
                        string input = ParagraphApi.ParagraphReadAllText(context, (Guid)paragraph.UniqueKey);
                        string toLang = uiLanguageCode;
                        string fromLang = wordUser.LanguageUser.Language.Code;
                        string translation = DeepLService.Translate(input, fromLang, toLang);
                        ppts = new ()
                        {
                            ParagraphKey = (Guid)paragraph.UniqueKey,
                            Code = toLang,
                            TranslationText = translation
                        };
                        ppts = DataCache.ParagraphTranslationCreate(ppts, context);
                    }

                    if (ppts is null || ppts.UniqueKey is null) { ErrorHandler.LogAndThrow(); return null; }

                    // now bridge it to the flash card
                    FlashCardParagraphTranslationBridge? fcptb = new ()
                    {
                        ParagraphTranslationKey = ppts.UniqueKey,
                        FlashCardKey = (Guid)card.UniqueKey,
                    };
                    fcptb = DataCache.FlashCardParagraphTranslationBridgeCreate(fcptb, context);
                    if(fcptb is null) { ErrorHandler.LogAndThrow(); return null; }
                    fcptb.FlashCard = card;
                    fcptb.ParagraphTranslation = ppts;
                    card.FlashCardParagraphTranslationBridges.Add(fcptb);
                    currentPptThisCard++;
                }
            }
            
            return card;
        }
        public static async Task<FlashCard?> FlashCardCreateAsync(IdiomaticaContext context, Guid wordUserId, string uiLanguageCode)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardCreate(context, wordUserId, uiLanguageCode);
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
            IdiomaticaContext context, Guid languageUserId, int numCards, string uiLanguageCode)
        {
            if (numCards < 1) { return new List<FlashCard>(); }
            
            List<FlashCard> cards = new List<FlashCard>();

            // get word users that don't already have a flash card
            // ordered by recent status change
            var wordUsers = (
                        from wu in context.WordUsers
                        join w in context.Words on wu.WordKey equals w.UniqueKey
                        join fc in context.FlashCards on wu.UniqueKey equals fc.WordUserKey into grouping
                        from fc in grouping.DefaultIfEmpty()
                        where (
                            wu.LanguageUserKey == languageUserId
                            && fc.UniqueKey == null
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
                if(wordUser is null || wordUser.UniqueKey is null) continue;
                var card = FlashCardCreate(context, (Guid)wordUser.UniqueKey, uiLanguageCode);
                if (card != null) cards.Add(card);
            }
            return cards;
        }
        public static async Task<List<FlashCard>?> FlashCardsCreateAsync(
            IdiomaticaContext context, Guid languageUserId, int numCards, string uiLanguageCode)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardsCreate(context, languageUserId, numCards, uiLanguageCode);
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
            AvailableFlashCardStatus status, DateTime nextReview, Guid uniqueKey)
        {
            var card = DataCache.FlashCardByIdRead(cardId, context);
            if (card == null) { ErrorHandler.LogAndThrow(); return null; }
            card.WordUserKey = wordUserId;
            card.NextReview = nextReview;
            card.Status = status;
            card.UniqueKey = uniqueKey;
            DataCache.FlashCardUpdate(card, context);
            return card;
        }
        public static async Task<FlashCard?> FlashCardUpdateAsync(
            IdiomaticaContext context, Guid cardId, Guid wordUserId, 
            AvailableFlashCardStatus status, DateTime nextReview, Guid uniqueKey)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardUpdate(context, cardId, wordUserId, status, nextReview, uniqueKey);
            });
        }
    }
}
