using DeepL;
using Logic.Telemetry;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class FlashCardService
    {
        public bool IsDataInit = false;
        private User? _loggedInUser = null;
        private UserService _userService;
        private Model.LanguageCode _uiLanguageCode;
        private ErrorHandler _errorHandler;
        private DeepLService _deepLService;
        private LanguageUser _languageUserLearning;
        public const int LoadingDelayMiliseconds = 500;


        #region deck properties

        List<FlashCard> Deck = new List<FlashCard>();
        private int _currentCardPosition = 0;
        private int _cardCount = 0;
        public bool IsDeckDefined = false;
        public bool IsDeckComplete = false;
        public FlashCard CurrentCard = new FlashCard();

        #endregion

        #region current card properties

        private FlashCardParagraphTranslationBridge _bridge;
        public string ExampleParagraph;
        public string ParagraphTranslation;        
        public string CardTitle
        {
            get
            {
                if (CurrentCard is null) return "";
                if (CurrentCard.WordUser is null) return "";
                if (CurrentCard.WordUser.Word is null) return "";
                return CurrentCard.WordUser.Word.TextLowerCase;
            }
            set { }
        }
        public string CardTranslation
        {
            get
            {
                if (CurrentCard is null) return "";
                if (CurrentCard.WordUser is null) return "";
                if (CurrentCard.WordUser.Translation is null) return "";
                return CurrentCard.WordUser.Translation;
            }
            set { }
        }

        #endregion


        #region deck building form fields

        public int LanguageInput;
        public int NumNewCardsInput;
        public int NumOldCardsInput;
        public Dictionary<int, LanguageUser> LanguageOptions = new Dictionary<int, LanguageUser>();
        public List<int> NumNewCardOptions = new List<int>();
        public List<int> NumOldCardOptions = new List<int>();
        public const int MaxNewCardsNew = 50;
        public const int MaxNewCardsReview = 50;

        #endregion


        #region thread checking bools

        private bool _isLoadingLoggedInUser = false;

        #endregion



        #region init functions
        public FlashCardService(ErrorHandler errorHandler, DeepLService deepLService, UserService userService)
        {
            _userService = userService;
            _errorHandler = errorHandler;
            _deepLService = deepLService;

        }
        public async Task InitDataAsync(IdiomaticaContext context)
        {
            _loggedInUser = await UserGetLoggedInAsync(context);
            _uiLanguageCode = _userService.GetUiLanguageCode();
            var dbLanguageUsers = await LanguageUsersFetchAsync(context, (int)_loggedInUser.Id);
            if (LanguageOptions.Count == 0)
            {
                foreach (var lu in dbLanguageUsers)
                {
                    LanguageOptions.Add((int)lu.Id, lu);
                }
            }
            if (NumNewCardOptions.Count == 0)
            {
                for (int i = 0; i <= MaxNewCardsNew; i += 5)
                {
                    NumNewCardOptions.Add(i);
                }
            }
            if (NumOldCardOptions.Count == 0)
            {
                for (int i = 0; i <= MaxNewCardsReview; i += 5)
                {
                    NumOldCardOptions.Add(i);
                }
            }
            _uiLanguageCode = _userService.GetUiLanguageCode();
            IsDataInit = true;
        }
        private async Task<List<LanguageUser>> LanguageUsersFetchAsync(IdiomaticaContext context, int userId)
        {
            if (userId < 1)
            {
                _errorHandler.LogAndThrow(1190);
            }

            return await context.LanguageUsers
                .Where(lu => lu.UserId == userId)
                .Include(lu => lu.Language)
                .OrderBy(x => x.Language.Name)
                .ToListAsync()
                ;
        }
        #endregion

        #region deck

        public async Task DeckCreateAsync(IdiomaticaContext context)
        {
            _languageUserLearning = LanguageOptions[LanguageInput];
            var oldCards = await FlashCardsFetchByNextReviewDateAsync(context);
            var newCards = await FlashCardsCreateAsync(context);

            Deck.AddRange(oldCards);
            Deck.AddRange(newCards);

            DeckShuffle();

            _cardCount = Deck.Count;
            _currentCardPosition = 0;
            CurrentCard = Deck[0];
            IsDeckDefined = true;

            FlashCardResetCurrentCard();
        }
        public async Task DeckAdvanceCardAsync(IdiomaticaContext context, AvailableFlashCardAttemptStatus previousCardsStatus)
        {
            await FlashCardAttemptCreateAsync(context, (int)CurrentCard.Id, previousCardsStatus);
            switch (previousCardsStatus)
            {
                default:
                case AvailableFlashCardAttemptStatus.GOOD:
                    CurrentCard.NextReview = DateTime.Now.AddDays(1);
                    break;
                case AvailableFlashCardAttemptStatus.WRONG:
                    CurrentCard.NextReview = DateTime.Now.AddMinutes(5);
                    break;
                case AvailableFlashCardAttemptStatus.HARD:
                    CurrentCard.NextReview = DateTime.Now.AddHours(1);
                    break;
                case AvailableFlashCardAttemptStatus.EASY:
                    CurrentCard.NextReview = DateTime.Now.AddDays(5);
                    break;
                case AvailableFlashCardAttemptStatus.STOP:
                    CurrentCard.Status = AvailableFlashCardStatus.DONTUSE;
                    CurrentCard.NextReview = DateTime.Now.AddYears(5);
                    break;
            }
            await FlashCardUpdateCurrentCardAfterReviewAsync(context);

            _currentCardPosition++;
            if (_currentCardPosition >= Deck.Count)
            {
                IsDeckComplete = true;
                IsDeckDefined = false;
                Deck = new List<FlashCard>();
                return;
            }
            CurrentCard = Deck[_currentCardPosition];
            FlashCardResetCurrentCard();
        }
        private void DeckShuffle()
        {
            Random rng = new Random();
            int n = Deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = Deck[k];
                Deck[k] = Deck[n];
                Deck[n] = value;
            }
        }

        #endregion

        #region FlashCard

        private async Task<FlashCard> FlashCardCreateAsync(IdiomaticaContext context, int wordUserId)
        {
            FlashCard card = new FlashCard();

            var wordUser = context.WordUsers
                .Where(wu => wu.Id == wordUserId)
                .Include(wu => wu.LanguageUser).ThenInclude(lu => lu.Language)
                .Include(wu => wu.Word)
                .FirstOrDefault();
            if (wordUser == null || wordUser.Id == 0)
            {
                _errorHandler.LogAndThrow(5110);
            }
            card.WordUser = wordUser;
            card.WordUserId = wordUserId;
            card.NextReview = DateTime.Now;
            card.Status = AvailableFlashCardStatus.ACTIVE;
            context.FlashCards.Add(card);
            await context.SaveChangesAsync();
            if (card.Id == null || card.Id == 0)
            {
                _errorHandler.LogAndThrow(2120);
            }
            List<Word> wordUsages = context.Words
                .Where(w => w.Id == wordUser.Word.Id)
                .Include(w => w.Tokens)
                    .ThenInclude(t => t.Sentence)
                    .ThenInclude(s => s.Paragraph)
                .ToList();
            foreach (var wordUsage in wordUsages)
            {
                foreach (var token in wordUsage.Tokens)
                {
                    var sentence = token.Sentence;
                    var paragraph = sentence.Paragraph;
                    // pull paragraph translations here in case the same
                    // word is used more than once in the same paragraph
                    var paragraphTranslations = context.ParagraphTranslations
                        .Where(ppt => ppt.ParagraphId == paragraph.Id)
                        .ToList();
                    ParagraphTranslation ppt = null;
                    if (paragraphTranslations.Count > 0)
                    {
                        // are they the right language?
                        ppt = paragraphTranslations
                            .Where(ppt => ppt.LanguageCode == wordUser.LanguageUser.Language.LanguageCode)
                            .FirstOrDefault();
                        if (ppt == null || ppt.Id == null || ppt.Id == 0)
                        {
                            // not the right language
                            // reset to null so the code below will create it
                            ppt = null;
                        }
                    }
                    if (ppt == null)
                    {
                        // create it
                        string input = ParagraphGetFullText(paragraph);
                        string toLang = _uiLanguageCode.Code;
                        string fromLang = wordUser.LanguageUser.Language?.Code;
                        string translation = _deepLService.Translate(input, fromLang, toLang);
                        ppt = new ParagraphTranslation()
                        {
                            ParagraphId = (int)paragraph.Id,
                            Code = toLang,
                            TranslationText = translation
                        };
                        context.ParagraphTranslations.Add(ppt);
                        await context.SaveChangesAsync();
                    }

                    // now bridge it to the flash card
                    FlashCardParagraphTranslationBridge fcptb = new FlashCardParagraphTranslationBridge()
                    {
                        ParagraphTranslationId = ppt.Id,
                        FlashCardId = (int)card.Id,
                    };
                    context.FlashCardParagraphTranslationBridges.Add(fcptb);
                    await context.SaveChangesAsync();
                }
            }
            // re-pull the card from the DB just to make sure we got it all
            var newCard = context.FlashCards
                .Where(fc => fc.Id == card.Id)
                .Include(fc => fc.WordUser).ThenInclude(wu => wu.Word)
                .Include(fc => fc.Attempts)
                .Include(fc => fc.FlashCardParagraphTranslationBridges)
                    .ThenInclude(fcptb => fcptb.ParagraphTranslation)
                        .ThenInclude(pt => pt.Paragraph).ThenInclude(pp => pp.Sentences)
                .FirstOrDefault();


            return newCard;
        }
        private void FlashCardResetCurrentCard()
        {
            ParagraphTranslation = "";
            ExampleParagraph = "";
            // note some paragraph translations will be into languages the user 
            // doesn't speak. That's not good. Filter it
            var bridges = CurrentCard.FlashCardParagraphTranslationBridges
                .Where(x => x.ParagraphTranslation.Code == _uiLanguageCode.Code)
                .ToList();
            var bridgeCount = bridges.Count;
            if (bridgeCount > 0)
            {
                Random rng = new Random();
                int position = rng.Next(0, bridgeCount);
                _bridge = bridges[position];
                if (_bridge.ParagraphTranslation != null
                    && _bridge.ParagraphTranslation.Paragraph != null
                    && _bridge.ParagraphTranslation.Paragraph.Sentences != null)
                {
                    ParagraphTranslation = _bridge.ParagraphTranslation.TranslationText;
                    ExampleParagraph = ParagraphGetFullText(_bridge.ParagraphTranslation.Paragraph);
                }
            }
            
        }
        public async Task FlashCardUpdateCurrentCardAfterReviewAsync(IdiomaticaContext context)
        {
            var card = await context.FlashCards.Where(x => x.Id == CurrentCard.Id).FirstOrDefaultAsync();
            if (card == null)
            {
                _errorHandler.LogAndThrow(2160);
            }
            card.NextReview = CurrentCard.NextReview;
            card.Status = CurrentCard.Status;
            await context.SaveChangesAsync();
        }
        private async Task<List<FlashCard>> FlashCardsCreateAsync(IdiomaticaContext context)
        {
            if (NumNewCardsInput < 0)
            {
                return new List<FlashCard>();
            }
            if (_languageUserLearning == null)
            {
                _errorHandler.LogAndThrow(1170);
                return new List<FlashCard>();
            }
            if (_languageUserLearning.Id == null || _languageUserLearning.Id < 1)
            {
                _errorHandler.LogAndThrow(1180);
                return new List<FlashCard>();
            }
            List<FlashCard> cards = new List<FlashCard>();

            // get word users that don't already have a flash card
            // ordered by recent status change
            string q = $"""
                select top {NumNewCardsInput}
                wu.Id, wu.WordId, wu.LanguageUserId, wu.Translation, wu.Status, wu.Created, wu.StatusChanged
                from [Idioma].[WordUser] wu 
                join [Idioma].[Word] w on wu.WordId = w.Id
                left join [Idioma].[FlashCard] fc on fc.[WordUserId] = wu.Id
                where wu.LanguageUserId = {_languageUserLearning.Id}
                and fc.Id is null
                and wu.Status not in (
                    {(int)AvailableWordUserStatus.LEARNED},
                    {(int)AvailableWordUserStatus.IGNORED},
                    {(int)AvailableWordUserStatus.WELLKNOWN},
                    {(int)AvailableWordUserStatus.UNKNOWN})
                and wu.Translation is not null
                and wu.Translation <> ''
                order by StatusChanged desc
                ;
                """;
            var wordUsers = await context.WordUsers.FromSqlRaw(q).ToListAsync();
            foreach (var wordUser in wordUsers)
            {
                cards.Add(await FlashCardCreateAsync(context, wordUser.Id));
            }
            return cards;
        }
        public async Task<List<FlashCard>> FlashCardsFetchByNextReviewDateAsync(IdiomaticaContext context)
        {
            if (NumOldCardsInput < 1)
            {
                return new List<FlashCard>();
            }
            if (_languageUserLearning == null)
            {
                _errorHandler.LogAndThrow(1170);
                return new List<FlashCard>();
            }
            if (_languageUserLearning.Id == null || _languageUserLearning.Id < 1)
            {
                _errorHandler.LogAndThrow(1180);
                return new List<FlashCard>();
            }

            return await context.FlashCards
                .Where(fc => fc.WordUser.LanguageUserId == _languageUserLearning.Id 
                    && fc.Status == AvailableFlashCardStatus.ACTIVE)
                .Include(fc => fc.WordUser).ThenInclude(wu => wu.Word)
                .Include(fc => fc.Attempts)
                .Include(fc => fc.FlashCardParagraphTranslationBridges)
                    .ThenInclude(fcptb => fcptb.ParagraphTranslation)
                        .ThenInclude(pt => pt.Paragraph).ThenInclude(pp => pp.Sentences)
                .OrderBy(fc => fc.NextReview)
                .Take(NumOldCardsInput)
                .ToListAsync();
        }
        
        #endregion

        #region FlashCardAttempt

        public async Task<int> FlashCardAttemptCreateAsync(IdiomaticaContext context, int FlashCardId, AvailableFlashCardAttemptStatus status)
        {
            var attempt = new FlashCardAttempt()
            {
                FlashCardId = FlashCardId,
                AttemptedWhen = DateTime.Now,
                Status = status,
            };
            await context.FlashCardsAttempts.AddAsync(attempt);
            await context.SaveChangesAsync();
            return attempt.Id;
        }

        #endregion



        #region Paragraph

        public string ParagraphGetFullText(Paragraph pp)
        {
            var sentences = pp.Sentences.OrderBy(x => x.Ordinal).Select(s => s.Text);
            return String.Join(" ", sentences);
        }

        #endregion

        #region User

        private async Task<User?> UserGetLoggedInAsync(IdiomaticaContext context)
        {
            if (_loggedInUser == null)
            {
                if (_isLoadingLoggedInUser == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return UserGetLoggedInAsync(context).Result;
                }
                _isLoadingLoggedInUser = true;
                _loggedInUser = await _userService.GetLoggedInUserAsync(context);
                _isLoadingLoggedInUser = false;
            }
            return _loggedInUser;
        }

        #endregion
    }
}
