using DeepL;
using Logic.Telemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model;
using Model.DAL;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Logic.Services
{
    public class FlashCardService
    {
        public bool IsDataInit = false;
        private User? _loggedInUser = null;
        private UserService _userService;
        
        public const int LoadingDelayMiliseconds = 500;


       

        


        


        #region thread checking bools

        private bool _isLoadingLoggedInUser = false;

        #endregion



        #region init functions
        public FlashCardService(UserService userService)
        {
            _userService = userService;

        }
        //public async Task InitDataAsync(IdiomaticaContext context)
        //{
            //_loggedInUser = await UserGetLoggedInAsync(context);
            //datapacketUiLanguageCode = _userService.GetUiLanguageCode();
            //var dbLanguageUsers = await LanguageUsersFetchAsync(context, (int)_loggedInUser.Id);
            //if (datapacketLanguageOptions.Count == 0)
            //{
            //    foreach (var lu in dbLanguageUsers)
            //    {
            //        datapacketLanguageOptions.Add((int)lu.Id, lu);
            //    }
            //}
        //    if (datapacketNumNewCardOptions.Count == 0)
        //    {
        //        for (int i = 0; i <= datapacketMaxNewCardsNew; i += 5)
        //        {
        //            datapacketNumNewCardOptions.Add(i);
        //        }
        //    }
        //    if (datapacketNumOldCardOptions.Count == 0)
        //    {
        //        for (int i = 0; i <= datapacketMaxNewCardsReview; i += 5)
        //        {
        //            datapacketNumOldCardOptions.Add(i);
        //        }
        //    }
        //    IsDataInit = true;
        //}
        //private async Task<List<LanguageUser>> LanguageUsersFetchAsync(IdiomaticaContext context, int userId)
        //{
        //    if (userId < 1)
        //    {
        //        ErrorHandler.LogAndThrow(1190);
        //    }
        //    return await DataCache.LanguageUsersAndLanguageByUserIdReadAsync(userId, context);
        //}
        #endregion

        #region deck

        //public async Task DeckCreateAsync(IdiomaticaContext context)
        //{
            //datapacketLanguageUserLearning = datapacketLanguageOptions[datapacketLanguageInput];
            //var oldCards = await FlashCardsFetchByNextReviewDateAsync(context);
            //var newCards = await FlashCardsCreateAsync(context);

            //datapacketDeck.AddRange(oldCards);
            //datapacketDeck.AddRange(newCards);

            //DeckShuffle();

            //datapacketCardCount = datapacketDeck.Count;
            //datapacketCurrentCardPosition = 0;
            //datapacketCurrentCard = datapacketDeck[0];
            //datapacketIsDeckDefined = true;

            //FlashCardResetCurrentCard();
        //}
        //public async Task DeckAdvanceCardAsync(IdiomaticaContext context, 
        //    AvailableFlashCardAttemptStatus previousCardsStatus)
        //{
            //await FlashCardAttemptCreateAsync(context, (int)datapacketCurrentCard.Id, previousCardsStatus);
            //switch (previousCardsStatus)
            //{
            //    default:
            //    case AvailableFlashCardAttemptStatus.GOOD:
            //        datapacketCurrentCard.NextReview = DateTime.Now.AddDays(1);
            //        break;
            //    case AvailableFlashCardAttemptStatus.WRONG:
            //        datapacketCurrentCard.NextReview = DateTime.Now.AddMinutes(5);
            //        break;
            //    case AvailableFlashCardAttemptStatus.HARD:
            //        datapacketCurrentCard.NextReview = DateTime.Now.AddHours(1);
            //        break;
            //    case AvailableFlashCardAttemptStatus.EASY:
            //        datapacketCurrentCard.NextReview = DateTime.Now.AddDays(5);
            //        break;
            //    case AvailableFlashCardAttemptStatus.STOP:
            //        datapacketCurrentCard.Status = AvailableFlashCardStatus.DONTUSE;
            //        datapacketCurrentCard.NextReview = DateTime.Now.AddYears(5);
            //        break;
            //}
            //await FlashCardUpdateCurrentCardAfterReviewAsync(context);

            //datapacketCurrentCardPosition++;
            //if (datapacketCurrentCardPosition >= datapacketDeck.Count)
            //{
            //    datapacketIsDeckComplete = true;
            //    datapacketIsDeckDefined = false;
            //    datapacketDeck = new List<FlashCard>();
            //    return;
            //}
            //datapacketCurrentCard = datapacketDeck[datapacketCurrentCardPosition];
        //    FlashCardResetCurrentCard();
        //}
        //private void DeckShuffle()
        //{
        //    Random rng = new Random();
        //    int n = datapacketDeck.Count;
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = rng.Next(n + 1);
        //        var value = datapacketDeck[k];
        //        datapacketDeck[k] = datapacketDeck[n];
        //        datapacketDeck[n] = value;
        //    }
        //}

        #endregion

        #region FlashCard

        //private async Task<FlashCard?> FlashCardCreateAsync(IdiomaticaContext context, int wordUserId)
        //{
        //    if (datapacketUiLanguageCode == null || datapacketUiLanguageCode.Code == null)
        //    {
        //        ErrorHandler.LogAndThrow(2490);
        //        return null;
        //    }
        //    FlashCard? card = new FlashCard();
        //    var wordUser = await DataCache.WordUserAndLanguageUserAndLanguageByIdReadAsync(wordUserId, context);
            
        //    if (wordUser == null || wordUser.Id < 1)
        //    {
        //        ErrorHandler.LogAndThrow(5110);
        //        return null;
        //    }
        //    card.WordUser = wordUser;
        //    card.WordUserId = wordUserId;
        //    card.NextReview = DateTime.Now;
        //    card.Status = AvailableFlashCardStatus.ACTIVE;
        //    await DataCache.FlashCardCreateAsync(card, context);
        //    if (card.Id == null || card.Id < 1)
        //    {
        //        ErrorHandler.LogAndThrow(2120);
        //        return null;
        //    }
        //    if (wordUser.WordId == null || wordUser.WordId < 1)
        //    {
        //        ErrorHandler.LogAndThrow(2460);
        //        return null;
        //    }
        //    List<Word> wordUsages = await DataCache.WordsAndTokensAndSentencesAndParagraphsByWordIdReadAsync(
        //        (int)wordUser.WordId, context);
                
        //    foreach (var wordUsage in wordUsages)
        //    {
        //        foreach (var token in wordUsage.Tokens)
        //        {
        //            if (token.Sentence == null)
        //            {
        //                ErrorHandler.LogAndThrow(2470);
        //                return null;
        //            }
        //            var sentence = token.Sentence;
        //            if (sentence.Paragraph == null || sentence.Paragraph.Id == null)
        //            {
        //                ErrorHandler.LogAndThrow(2480);
        //                return null;
        //            }
        //            var paragraph = sentence.Paragraph;
        //            // pull paragraph translations here in case the same
        //            // word is used more than once in the same paragraph
        //            var paragraphTranslations = await DataCache.ParagraphTranslationsByParargraphIdReadAsync(
        //                (int)paragraph.Id, context);



                        
        //            ParagraphTranslation? ppts = null;
        //            if (paragraphTranslations.Count > 0)
        //            {
        //                // are they the right language?
        //                ppts = paragraphTranslations
        //                    .Where(
        //                        ppt => ppt != null 
        //                        && ppt.LanguageCode != null
        //                        && ppt.LanguageCode.Code == datapacketUiLanguageCode.Code)
        //                    .FirstOrDefault();
        //                if (ppts == null || ppts.Id == null || ppts.Id == 0)
        //                {
        //                    // not the right language
        //                    // reset to null so the code below will create it
        //                    ppts = null;
        //                }
        //            }
        //            if (ppts == null)
        //            {
        //                if (wordUser.LanguageUser == null
        //                    || wordUser.LanguageUser.Language == null
        //                    || wordUser.LanguageUser.Language.Code == null)
        //                {
        //                    ErrorHandler.LogAndThrow(2500);
        //                    return null;
        //                }
        //                // create it
        //                string input = ParagraphGetFullText(paragraph);
        //                string toLang = datapacketUiLanguageCode.Code;
        //                string fromLang = wordUser.LanguageUser.Language.Code;
        //                string translation = DeepLService.Translate(input, fromLang, toLang);
        //                ppts = new ParagraphTranslation()
        //                {
        //                    ParagraphId = (int)paragraph.Id,
        //                    Code = toLang,
        //                    TranslationText = translation
        //                };
        //                await DataCache.ParagraphTranslationCreateAsync(ppts, context);
        //            }

        //            // now bridge it to the flash card
        //            FlashCardParagraphTranslationBridge fcptb = new FlashCardParagraphTranslationBridge()
        //            {
        //                ParagraphTranslationId = ppts.Id,
        //                FlashCardId = (int)card.Id,
        //            };
        //            await DataCache.FlashCardParagraphTranslationBridgeCreateAsync(fcptb, context);
        //        }
        //    }
        //    // re-pull the card from the DB just to make sure we got it all
        //    var newCard = await DataCache.FlashCardAndFullRelationshipsByIdReadAsync((int)card.Id, context);
            
        //    return newCard;
        //}
        //private void FlashCardResetCurrentCard()
        //{
        //    datapacketParagraphTranslation = "";
        //    datapacketExampleParagraph = "";
        //    // note some paragraph translations will be into languages the user 
        //    // doesn't speak. That's not good. Filter it
        //    var bridges = datapacketCurrentCard.FlashCardParagraphTranslationBridges
        //        .Where(x => x.ParagraphTranslation.Code == datapacketUiLanguageCode.Code)
        //        .ToList();
        //    var bridgeCount = bridges.Count;
        //    if (bridgeCount > 0)
        //    {
        //        Random rng = new Random();
        //        int position = rng.Next(0, bridgeCount);
        //        datapacketBridge = bridges[position];
        //        if (datapacketBridge.ParagraphTranslation != null
        //            && datapacketBridge.ParagraphTranslation.Paragraph != null
        //            && datapacketBridge.ParagraphTranslation.Paragraph.Sentences != null)
        //        {
        //            datapacketParagraphTranslation = datapacketBridge.ParagraphTranslation.TranslationText;
        //            datapacketExampleParagraph = ParagraphGetFullText(datapacketBridge.ParagraphTranslation.Paragraph);
        //        }
        //    }
            
        //}
        //public async Task FlashCardUpdateCurrentCardAfterReviewAsync(IdiomaticaContext context)
        //{
        //    if (datapacketCurrentCard == null || datapacketCurrentCard.Id == null)
        //    {
        //        ErrorHandler.LogAndThrow(2510);
        //        return;
        //    }
        //    var card = await DataCache.FlashCardByIdReadAsync((int)datapacketCurrentCard.Id, context);
        //    if (card == null)
        //    {
        //        ErrorHandler.LogAndThrow(2160);
        //        return;
        //    }
        //    card.NextReview = datapacketCurrentCard.NextReview;
        //    card.Status = datapacketCurrentCard.Status;
        //    await DataCache.FlashCardUpdateAsync(card, context);
        //}
        //private async Task<List<FlashCard>> FlashCardsCreateAsync(IdiomaticaContext context)
        //{
        //    if (datapacketNumNewCardsInput < 0)
        //    {
        //        return new List<FlashCard>();
        //    }
        //    if (datapacketLanguageUserLearning == null)
        //    {
        //        ErrorHandler.LogAndThrow(1170);
        //        return new List<FlashCard>();
        //    }
        //    if (datapacketLanguageUserLearning.Id == null || datapacketLanguageUserLearning.Id < 1)
        //    {
        //        ErrorHandler.LogAndThrow(1180);
        //        return new List<FlashCard>();
        //    }
        //    List<FlashCard> cards = new List<FlashCard>();

        //    // get word users that don't already have a flash card
        //    // ordered by recent status change
        //    var wordUsers = (
        //                from wu in context.WordUsers
        //                join w in context.Words on wu.WordId equals w.Id
        //                join fc in context.FlashCards on wu.Id equals fc.WordUserId into grouping
        //                from fc in grouping.DefaultIfEmpty()
        //                where (
        //                    wu.LanguageUserId == datapacketLanguageUserLearning.Id
        //                    && fc.Id == null
        //                    && wu.Status != AvailableWordUserStatus.LEARNED
        //                    && wu.Status != AvailableWordUserStatus.IGNORED
        //                    && wu.Status != AvailableWordUserStatus.WELLKNOWN
        //                    && wu.Status != AvailableWordUserStatus.UNKNOWN
        //                    && wu.Translation != null
        //                    && wu.Translation != string.Empty
        //                    )
        //                select wu
        //            )
        //        .OrderByDescending(x => x.StatusChanged)
        //        .Take(datapacketNumNewCardsInput)
        //        .ToList();



        //    foreach (var wordUser in wordUsers)
        //    {
        //        var card = await FlashCardCreateAsync(context, (int)wordUser.Id);
        //        if (card != null) cards.Add(card);
        //    }
        //    return cards;
        //}
        //public async Task<List<FlashCard>> FlashCardsFetchByNextReviewDateAsync(IdiomaticaContext context)
        //{
        //    if (datapacketNumOldCardsInput < 1)
        //    {
        //        return new List<FlashCard>();
        //    }
        //    if (datapacketLanguageUserLearning == null)
        //    {
        //        ErrorHandler.LogAndThrow(1170);
        //        return new List<FlashCard>();
        //    }
        //    if (datapacketLanguageUserLearning.Id == null || datapacketLanguageUserLearning.Id < 1)
        //    {
        //        ErrorHandler.LogAndThrow(1180);
        //        return new List<FlashCard>();
        //    }

        //    return await DataCache.FlashCardsActiveAndFullRelationshipsByLanguageUserIdReadAsync(
        //        ((int)datapacketLanguageUserLearning.Id, datapacketNumOldCardsInput), context);
        //}
        
        #endregion

        #region FlashCardAttempt

        //public async Task<int> FlashCardAttemptCreateAsync(IdiomaticaContext context, int FlashCardId, AvailableFlashCardAttemptStatus status)
        //{
        //    var attempt = new FlashCardAttempt()
        //    {
        //        FlashCardId = FlashCardId,
        //        AttemptedWhen = DateTime.Now,
        //        Status = status,
        //    };
        //    await DataCache.FlashCardAttemptCreateAsync(attempt, context);
        //    if (attempt.Id == null || attempt.Id < 1)
        //    {
        //        ErrorHandler.LogAndThrow(2520);
        //        return -1;
        //    }
        //    return (int)attempt.Id;
        //}

        #endregion



        //#region Paragraph

        
        //public string ParagraphGetFullText(Paragraph pp)
        //{
        //    var sentences = pp.Sentences.OrderBy(x => x.Ordinal).Select(s => s.Text);
        //    return String.Join(" ", sentences);
        //}

        //#endregion

        //#region User

        //private async Task<User?> UserGetLoggedInAsync(IdiomaticaContext context)
        //{
        //    if (_loggedInUser == null)
        //    {
        //        if (_isLoadingLoggedInUser == true)
        //        {
        //            // hold up. some other thread is loading it
        //            Thread.Sleep(1000);
        //            return await UserGetLoggedInAsync(context);
        //        }
        //        _isLoadingLoggedInUser = true;
        //        _loggedInUser = await _userService.GetLoggedInUserAsync(context);
        //        _isLoadingLoggedInUser = false;
        //    }
        //    return _loggedInUser;
        //}

        //#endregion
    }
}
