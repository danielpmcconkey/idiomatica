using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
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
        private IDbContextFactory<IdiomaticaContext> _dbContextFactory;
        private UserService _userService;
        private LanguageCode _uiLanguageCode;
        public FlashCardService(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        #region FlashCard
        public async Task InitializeAsync(UserService userService)
        {
            _userService = userService;
            _uiLanguageCode = userService.GetUiLanguageCode();
        }
        public FlashCard FlashCardCreate(int wordUserId)
        {
            var context = _dbContextFactory.CreateDbContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                FlashCard card = new FlashCard();

                var wordUser = context.WordUsers
                    .Where(wu => wu.Id == wordUserId)
                    .Include(wu => wu.LanguageUser).ThenInclude(lu => lu.Language)
                    .Include(wu => wu.Word)
                    .FirstOrDefault();
                if (wordUser == null || wordUser.Id == 0)
                {
                    ErrorHandler.LogAndThrow(5110);
                }
                card.WordUser = wordUser;
                card.WordUserId = wordUserId;
                card.NextReview = DateTime.Now;
                card.Status = AvailableFlashCardStatus.ACTIVE;
                context.FlashCards.Add(card);
                context.SaveChanges();
                if (card.Id == null || card.Id == 0)
                {
                    ErrorHandler.LogAndThrow(2120);
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
                            string translation = DeepLService.Translate(input, fromLang, toLang);
                            ppt = new ParagraphTranslation()
                            {
                                ParagraphId = (int)paragraph.Id,
                                Code = toLang,
                                TranslationText = translation
                            };
                            context.ParagraphTranslations.Add(ppt);
                            context.SaveChanges();
                        }

                        // now bridge it to the flash card
                        FlashCardParagraphTranslationBridge fcptb = new FlashCardParagraphTranslationBridge()
                        {
                            ParagraphTranslationId = ppt.Id,
                            FlashCardId = (int)card.Id,
                        };
                        context.FlashCardParagraphTranslationBridges.Add(fcptb);
                        context.SaveChanges();
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

                transaction.Commit();
                return newCard;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
        public List<FlashCard> FlashCardsCreate(int limit, LanguageUser LanguageUser_learning)
        {
            if (limit < 0)
            {
                return new List<FlashCard>();
            }
            if (LanguageUser_learning == null)
            {
                ErrorHandler.LogAndThrow(1170);
            }
            if (LanguageUser_learning.Id == null || LanguageUser_learning.Id == 0)
            {
                ErrorHandler.LogAndThrow(1180);
            }
            var context = _dbContextFactory.CreateDbContext();
            List<FlashCard> cards = new List<FlashCard>();

            // get word users that don't already have a flash card
            // ordered by recent status change
            string q = $"""
                select top {limit}
                wu.Id, wu.WordId, wu.LanguageUserId, wu.Translation, wu.Status, wu.Created, wu.StatusChanged
                from [Idioma].[WordUser] wu 
                join [Idioma].[Word] w on wu.WordId = w.Id
                left join [Idioma].[FlashCard] fc on fc.[WordUserId] = wu.Id
                where wu.LanguageUserId = {LanguageUser_learning.Id}
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
            var wordUsers = context.WordUsers.FromSqlRaw(q);
            foreach (var wordUser in wordUsers)
            {
                cards.Add(FlashCardCreate(wordUser.Id));
            }
            return cards;
        }
        public List<FlashCard> FlashCardsFetchByNextReviewDate(int limit, LanguageUser languageUser)
        {
            if (limit < 1)
            {
                return new List<FlashCard>();
            }
            if (languageUser == null)
            {
                ErrorHandler.LogAndThrow(1170);
            }
            if (languageUser.Id == null || languageUser.Id == 0)
            {
                ErrorHandler.LogAndThrow(1180);
            }

            var context = _dbContextFactory.CreateDbContext();
            return context.FlashCards
                .Where(fc => fc.WordUser.LanguageUserId == languageUser.Id 
                    && fc.Status == AvailableFlashCardStatus.ACTIVE)
                .Include(fc => fc.WordUser).ThenInclude(wu => wu.Word)
                .Include(fc => fc.Attempts)
                .Include(fc => fc.FlashCardParagraphTranslationBridges)
                    .ThenInclude(fcptb => fcptb.ParagraphTranslation)
                        .ThenInclude(pt => pt.Paragraph).ThenInclude(pp => pp.Sentences)
                .OrderBy(fc => fc.NextReview)
                .Take(limit)
                .ToList();
        }
        public async Task FlashCardUpdateAfterReviewAsync(int cardId, DateTime nextReview,
            AvailableFlashCardStatus status)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
            var card = context.FlashCards.Where(x => x.Id == cardId).FirstOrDefault();
            if (card == null)
            {
                ErrorHandler.LogAndThrow(2160);
            }
            card.NextReview = nextReview;
            card.Status = status;
            context.SaveChanges();
        }
        #endregion

        #region FlashCardAttempt
        public async Task<int> FlashCardAttemptCreateAsync(int FlashCardId, AvailableFlashCardAttemptStatus status)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
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

        public IQueryable<LanguageUser> LanguageUsersFetch(int userId)
        {
            if (userId < 1)
            {
                ErrorHandler.LogAndThrow(1190);
            }
            var context = _dbContextFactory.CreateDbContext();

            return context.LanguageUsers
                .Where(lu => lu.UserId == userId)
                .Include(lu => lu.Language)
                .OrderBy(x => x.Language.Name)
                ;
        }

        public string ParagraphGetFullText(Paragraph pp)
        {
            var sentences = pp.Sentences.OrderBy(x => x.Ordinal).Select(s => s.Text);
            return String.Join(" ", sentences);
        }
    }
}
