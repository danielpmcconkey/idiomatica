using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using k8s.KubeConfigModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Expressions;

namespace Logic.Services.API
{
    public static class OrchestrationApi
    {
        /// <summary>
        /// orchestrates the processes to create the book, book stats, book user 
        /// for the creating user, word users for the creating user, and the 
        /// book user stats for the creating user
        /// </summary>
        public static Book? OrchestrateBookCreationAndSubProcesses(
            IdiomaticaContext context, int userId, string title, string languageCode,
            string? url, string text)
        {
            Book? book = BookApi.BookCreateAndSave(context, title, languageCode, url, text);
            if (book is null || book.Id is null || book.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            int bookId = (int)book.Id;
            // add the book stats
            BookStatApi.BookStatsCreateAndSave(context, bookId);
            // now create the book user for the logged in user
            if (userId < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var bookUser = OrchestrateBookUserCreationAndSubProcesses(context, bookId, userId);


            return book;
        }
        public static async Task<Book?> OrchestrateBookCreationAndSubProcessesAsync(
            IdiomaticaContext context, int userId, string title, string languageCode,
            string? url, string text)
        {
            return await Task<Book?>.Run(() =>
            {
                return OrchestrateBookCreationAndSubProcesses(context, userId, title, languageCode, url, text);
            });
        }


        /// <summary>
        /// Create the book user, word users, and book user stats
        /// </summary>
        public static BookUser? OrchestrateBookUserCreationAndSubProcesses(
            IdiomaticaContext context, int bookId, int userId)
        {
            var bookUser = BookUserApi.BookUserCreate(context, bookId, userId);
            if (bookUser is null || bookUser.Id is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // create the wordUsers
            WordUserApi.WordUsersCreateAllForBookIdAndUserId(context, bookId, userId);
            // update bookUserStats
            BookUserStatApi.BookUserStatsUpdateByBookUserId(context, (int)bookUser.Id);

            return bookUser;
        }
        public static async Task<BookUser?> OrchestrateBookUserCreationAndSubProcessesAsync(
            IdiomaticaContext context, int bookId, int userId)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return OrchestrateBookUserCreationAndSubProcesses(context, bookId, userId);
            });
        }


        public static ReadDataPacket? OrchestrateClearPageAndMove(IdiomaticaContext context, ReadDataPacket readDataPacket, int targetPageNum)
        {
            ReadDataPacket? outPacket = readDataPacket;
            if (readDataPacket.CurrentPageUser is null || readDataPacket.CurrentPageUser.Id is null || readDataPacket.CurrentPageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // update all unknowns to well known
            PageUserApi.PageUserUpdateUnknowWordsToWellKnown(context, (int)readDataPacket.CurrentPageUser.Id);
            // now move forward, if there's another page
            if (targetPageNum <= readDataPacket.BookTotalPageCount) // remember pages are 1-indexed
            {
                if (readDataPacket.Book is null || readDataPacket.Book.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                outPacket = OrchestrateMovePage(context, readDataPacket, (int)readDataPacket.Book.Id, targetPageNum);
                if (outPacket is null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
            }
            else
            {
                // mark the previous page as read because you didn't do it in the PageMove function
                PageUserApi.PageUserMarkAsRead(context, (int)readDataPacket.CurrentPageUser.Id);
                // refresh the word user cache
                if (outPacket is null || outPacket.CurrentPageUser is null || outPacket.CurrentPageUser.Id is null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                outPacket = OrchestrateResetReadDataForNewPage(
                    context, readDataPacket, (int)outPacket.CurrentPageUser.Id);
            }
            return outPacket;
        }
        public static async Task<ReadDataPacket?> OrchestrateClearPageAndMoveAsync(
            IdiomaticaContext context, ReadDataPacket readDataPacket, int targetPageNum)
        {
            return await Task<ReadDataPacket?>.Run(() =>
            {
                return OrchestrateClearPageAndMove(context, readDataPacket, targetPageNum);
            });
        }

        /// <summary>
        ///  take the result of the last card, update the last card, and move the next card to the top
        /// </summary>
        public static FlashCardDataPacket? OrchestrateFlashCardDispositionAndAdvance(
            IdiomaticaContext context, FlashCardDataPacket? dataPacket, AvailableFlashCardAttemptStatus previousCardsStatus)
        {
            if (dataPacket is null || dataPacket.CurrentCard is null || 
                dataPacket.CurrentCard.Id is null || dataPacket.CurrentCard.WordUserId is null)
                { ErrorHandler.LogAndThrow(); return null; }

            // create the attempt object
            var attempt = FlashCardAttemptApi.FlashCardAttemptCreate(
                context, (int)dataPacket.CurrentCard.Id, previousCardsStatus);

            // update the current card
            DateTime nextReview = DateTime.Now;
            AvailableFlashCardStatus newStatus = AvailableFlashCardStatus.ACTIVE;
            switch (previousCardsStatus)
            {
                default:
                case AvailableFlashCardAttemptStatus.GOOD:
                    nextReview = DateTime.Now.AddDays(3);
                    break;
                case AvailableFlashCardAttemptStatus.WRONG:
                    nextReview = DateTime.Now.AddMinutes(5);
                    break;
                case AvailableFlashCardAttemptStatus.HARD:
                    nextReview = DateTime.Now.AddHours(1);
                    break;
                case AvailableFlashCardAttemptStatus.EASY:
                    nextReview = DateTime.Now.AddDays(7);
                    break;
                case AvailableFlashCardAttemptStatus.STOP:
                    newStatus = AvailableFlashCardStatus.DONTUSE;
                    nextReview = DateTime.Now.AddYears(5);
                    break;
            }
            var card = FlashCardApi.FlashCardUpdate(
                context, (int)dataPacket.CurrentCard.Id, (int)dataPacket.CurrentCard.WordUserId,
                newStatus, nextReview, dataPacket.CurrentCard.UniqueKey);

            if (card is not null && previousCardsStatus == AvailableFlashCardAttemptStatus.WRONG)
            {
                // add the card to the end of the deck
                dataPacket.Deck.Add(card);
                dataPacket.CardCount++;
            }

            // advance the deck
            dataPacket.CurrentCardPosition++;
            if (dataPacket.CurrentCardPosition < dataPacket.Deck.Count)
            {
                dataPacket.CurrentCard = dataPacket.Deck[dataPacket.CurrentCardPosition];
            }

            return dataPacket;
        }
        public static async Task<FlashCardDataPacket?> OrchestrateFlashCardDispositionAndAdvanceAsync(
            IdiomaticaContext context, FlashCardDataPacket? dataPacket, AvailableFlashCardAttemptStatus previousCardsStatus)
        {
            return await Task<FlashCardDataPacket?>.Run(() =>
            {
                return OrchestrateFlashCardDispositionAndAdvance(context, dataPacket, previousCardsStatus);
            });
        }


        public static FlashCardDataPacket? OrchestrateFlashCardDeckCreation(
            IdiomaticaContext context, int userId, string learningLanguageCode, int numNew, int numReview)
        {
            if (userId < 1) { ErrorHandler.LogAndThrow(); return null; }
            if (string.IsNullOrEmpty(learningLanguageCode)) { ErrorHandler.LogAndThrow(); return null; }
            if (numNew < 0) { ErrorHandler.LogAndThrow(); return null; }
            if (numReview < 0) { ErrorHandler.LogAndThrow(); return null; }

            FlashCardDataPacket newDataPacket = new ();
            newDataPacket.LearningLanguageCode = learningLanguageCode;
            var uiLanguageCode = LanguageCodeApi
                .LanguageCodeUserInterfacePreferenceReadByUserId(context, userId);
            if (uiLanguageCode is null || uiLanguageCode.Code is null) 
                { ErrorHandler.LogAndThrow(); return null; }
            newDataPacket.UILanguageCode = uiLanguageCode.Code;

            // pull the LanguageUserId
            var language = DataCache.LanguageByCodeRead(learningLanguageCode, context);
            if (language is null || language.Id is null) { ErrorHandler.LogAndThrow(); return null; }
            var languageUser = DataCache.LanguageUserByLanguageIdAndUserIdRead(((int)language.Id, userId), context);
            if (languageUser is null || languageUser.Id is null) { ErrorHandler.LogAndThrow(); return null; }

            // pull cards that are ready for their next review
            Expression<Func<FlashCard, bool>> predicate = fc => fc.WordUser != null
                    && fc.WordUser.LanguageUserId == languageUser.Id
                    && fc.Status == AvailableFlashCardStatus.ACTIVE
                    && fc.NextReview != null
                    && fc.NextReview <= DateTime.Now;

            var oldCards = FlashCardApi.FlashCardsFetchByNextReviewDateByPredicate(
                context, predicate, numReview);

            if (oldCards is not null && oldCards.Count > 0) 
                newDataPacket.Deck.AddRange(oldCards);

            // pull cards that have already been created, but never reviewed
            Expression<Func<FlashCard, bool>> predicate2 = fc => fc.WordUser != null
                    && fc.WordUser.LanguageUserId == languageUser.Id
                    && fc.Status == AvailableFlashCardStatus.ACTIVE
                    && fc.NextReview == null;

            var readyForReviewCards = FlashCardApi.FlashCardsFetchByNextReviewDateByPredicate(
                context, predicate2, numNew);

            int numReadyForReviewReturned = 0;
            if (readyForReviewCards is not null && readyForReviewCards.Count > 0)
            {
                newDataPacket.Deck.AddRange(readyForReviewCards);
                numReadyForReviewReturned = readyForReviewCards.Count;
            }

            // create enough new cards to fill out the numNew
            if(numReadyForReviewReturned < numNew)
            {
                int numToCreate = numNew - numReadyForReviewReturned;
                var newCards = FlashCardApi.FlashCardsCreate(
                context, (int)languageUser.Id, numToCreate, uiLanguageCode.Code);
                if (newCards is not null && newCards.Count > 0)
                    newDataPacket.Deck.AddRange(newCards);
            }
           
            // shuffle it
            newDataPacket.Deck = FlashCardApi.FlashCardDeckShuffle(newDataPacket.Deck);
            
            // initialize some deck properties
            newDataPacket.CardCount = newDataPacket.Deck.Count;
            newDataPacket.CurrentCardPosition = 0;
            newDataPacket.CurrentCard = newDataPacket.Deck[0];

            return newDataPacket;
        }
        public static async Task<FlashCardDataPacket?> OrchestrateFlashCardDeckCreationAsync(
            IdiomaticaContext context, int userId, string learningLanguageCode, int numNew, int numReview)
        {
            return await Task<FlashCardDataPacket?>.Run(() =>
            {
                return OrchestrateFlashCardDeckCreation(context, userId, learningLanguageCode, numNew, numReview);
            });
        }


        public static ReadDataPacket? OrchestrateMovePage(
            IdiomaticaContext context, ReadDataPacket readDataPacket, int bookId, int targetPageNum)
        {
            if (readDataPacket.LanguageUser is null || readDataPacket.LanguageUser.Id is null || readDataPacket.LanguageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.CurrentPageUser is null || readDataPacket.CurrentPageUser.Id is null || readDataPacket.CurrentPageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.BookUser is null || readDataPacket.BookUser.Id is null || readDataPacket.BookUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.LoggedInUser is null || readDataPacket.LoggedInUser.Id is null || readDataPacket.LoggedInUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.Id is null || readDataPacket.CurrentPage.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // mark the previous page as read if moving forward
            if (targetPageNum > readDataPacket.CurrentPage.Ordinal)
                PageUserApi.PageUserMarkAsRead(context, (int)readDataPacket.CurrentPageUser.Id);

            if (targetPageNum < 1) return null;
            if (targetPageNum > readDataPacket.BookTotalPageCount)
                return null;

            // reload the current page user with the new target
            readDataPacket.CurrentPageUser = PageUserApi.PageUserReadByOrderWithinBook(
                context, (int)readDataPacket.LanguageUser.Id, targetPageNum, bookId);

            // create the page user if it hasn't been created already
            if (readDataPacket.CurrentPageUser is null || readDataPacket.CurrentPageUser.PageId is null)
            {
                // but first need to pull the page
                readDataPacket.CurrentPage = PageApi.PageReadByOrdinalAndBookId(context, targetPageNum, bookId);
                if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.Id is null || readDataPacket.CurrentPage.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                readDataPacket.CurrentPageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)readDataPacket.CurrentPage.Id, (int)readDataPacket.LoggedInUser.Id);
            }
            if (readDataPacket.CurrentPageUser is null || readDataPacket.CurrentPageUser.PageId is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }


            var newReadDataPacket = OrchestrateResetReadDataForNewPage(
                context, readDataPacket, (int)readDataPacket.CurrentPageUser.PageId);
            if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                newReadDataPacket.CurrentPageUser.PageId is null )
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            readDataPacket = newReadDataPacket;
            BookUserApi.BookUserUpdateBookmark(
                context, (int)readDataPacket.BookUser.Id, (int)readDataPacket.CurrentPageUser.PageId);

            return readDataPacket;
        }
        public static async Task<ReadDataPacket?> OrchestrateMovePageAsync(
            IdiomaticaContext context, ReadDataPacket readDataPacket, int bookId, int targetPageNum)
        {
            return await Task<ReadDataPacket?>.Run(() =>
            {
                return OrchestrateMovePage(context, readDataPacket, bookId, targetPageNum);
            });
        }


        public static ReadDataPacket? OrchestrateReadDataInit(
            IdiomaticaContext context, UserService userService, int bookId)
        {
            ReadDataPacket readDataPacket = new();
            // tier 0 tasks, not dependent on anything
            readDataPacket.LoggedInUser = userService.GetLoggedInUser(context);
            readDataPacket.Book = BookApi.BookRead(context, bookId);
            readDataPacket.BookTotalPageCount = BookApi.BookReadPageCount(context, bookId);
            readDataPacket.LanguageToCode = userService.GetUiLanguageCode();

            if (readDataPacket.LoggedInUser is null || readDataPacket.LoggedInUser.Id is null || readDataPacket.LoggedInUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.LanguageToCode is null || string.IsNullOrEmpty(readDataPacket.LanguageToCode.Code))
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.Book is null || readDataPacket.Book.Id is null || readDataPacket.Book.Id < 1 || readDataPacket.Book.LanguageId is null || readDataPacket.Book.LanguageId < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // tier 1 tasks, dependent on tier 0
            readDataPacket.BookUser = BookUserApi.BookUserByBookIdAndUserIdRead(context, (int)readDataPacket.Book.Id, (int)readDataPacket.LoggedInUser.Id);
            readDataPacket.LanguageUser = LanguageUserApi.LanguageUserGet(context, (int)readDataPacket.Book.LanguageId, (int)readDataPacket.LoggedInUser.Id);
            readDataPacket.Language = LanguageApi.LanguageRead(context, (int)readDataPacket.Book.LanguageId);


            if (readDataPacket.BookUser is null)
            {
                // create it, I guess
                readDataPacket.BookUser = OrchestrateBookUserCreationAndSubProcesses(
                    context, bookId, (int)readDataPacket.LoggedInUser.Id);
            }
            if (readDataPacket.BookUser is null || readDataPacket.BookUser.Id is null || readDataPacket.BookUser.Id < 1)
            {
                // still null, something went wrong
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.LanguageUser is null || readDataPacket.LanguageUser.Id is null || readDataPacket.LanguageUser.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.Language is null || readDataPacket.Language.Id is null || readDataPacket.Language.Id < 1 || string.IsNullOrEmpty(readDataPacket.Language.Code))
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // tier 2, dependent on tier 1
            readDataPacket.BookUserStats = BookUserStatApi.BookUserStatsRead(context, (int)readDataPacket.Book.Id, (int)readDataPacket.LoggedInUser.Id);
            readDataPacket.CurrentPageUser = PageUserApi.PageUserReadBookmarkedOrFirst(context, (int)readDataPacket.BookUser.Id);
            readDataPacket.LanguageFromCode = LanguageCodeApi.LanguageCodeReadByCode(context, (string)readDataPacket.Language.Code);



            // create the page user if it hasn't been created already
            if (readDataPacket.CurrentPageUser is null)
            {
                // but first need to pull the page
                readDataPacket.CurrentPage = PageApi.PageReadFirstByBookId(context, bookId);
                if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.Id is null || readDataPacket.CurrentPage.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                readDataPacket.CurrentPageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)readDataPacket.CurrentPage.Id, (int)readDataPacket.LoggedInUser.Id);
            }
            else
            {
                // just pull the current page
                if (readDataPacket.CurrentPageUser.PageId is null || readDataPacket.CurrentPageUser.PageId < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                readDataPacket.CurrentPage = PageApi.PageReadById(context, (int)readDataPacket.CurrentPageUser.PageId);
            }
            if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.Id is null || readDataPacket.CurrentPage.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // tier 3, dependent on tier 2
            var resetDataPacket = OrchestrateResetReadDataForNewPage(
                context, readDataPacket, (int)readDataPacket.CurrentPage.Id);
            if (resetDataPacket is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // finally, update the breadcrumbs
            UserApi.UserBreadCrumbCreate(
                context, (int)readDataPacket.LoggedInUser.Id, (int)readDataPacket.CurrentPage.Id);
            
            return resetDataPacket;

        }
        public static async Task<ReadDataPacket?> OrchestrateReadDataInitAsync(
            IdiomaticaContext context, UserService userService, int bookId)
        {
            return await Task<ReadDataPacket?>.Run(() =>
            {
                return OrchestrateReadDataInit(context, userService, bookId);
            });
        }


        public static ReadDataPacket? OrchestrateResetReadDataForNewPage(
            IdiomaticaContext context, ReadDataPacket readDataPacket, int newPageId)
        {
            if (newPageId < 1)
            {
                ErrorHandler.LogAndThrow(1240);
                return null;
            }
            if (readDataPacket.LoggedInUser == null || readDataPacket.LoggedInUser.Id == null || readDataPacket.LoggedInUser.Id == 0)
            {
                ErrorHandler.LogAndThrow(2130);
                return null;
            }
            // wipe the old ones out
            readDataPacket.CurrentPage = null;
            readDataPacket.Paragraphs = null;
            readDataPacket.AllWordUsersInPage = null;
            readDataPacket.AllWordsInPage = null;
            readDataPacket.Sentences = null;
            readDataPacket.Tokens = null;

            // and rebuild
            readDataPacket.CurrentPage = PageApi.PageReadById(context, newPageId);
            readDataPacket.Paragraphs = ParagraphApi.ParagraphsReadByPageId(context, newPageId);
            readDataPacket.AllWordsInPage = WordApi.WordsDictReadByPageId(context, newPageId);
            readDataPacket.Sentences = SentenceApi.SentencesReadByPageId(context, newPageId);
            readDataPacket.Tokens = TokenApi.TokensReadByPageId(context, newPageId);

            if (readDataPacket.AllWordsInPage is null)
            {
                ErrorHandler.LogAndThrow(2130);
                return null;
            }

            // do not do this until you've already pulled the _allWordsInPage as 
            // that makes it way more efficient since it checks the words dict cache
            readDataPacket.AllWordUsersInPage = WordUserApi.WordUsersDictByPageIdAndUserIdRead(
                context, newPageId, (int)readDataPacket.LoggedInUser.Id);

            if (readDataPacket.AllWordUsersInPage is null)
            {
                ErrorHandler.LogAndThrow(2130);
                return null;
            }
            if (readDataPacket.Paragraphs is null)
            {
                ErrorHandler.LogAndThrow(2130);
                return null;
            }

            // now knit the paragraph data together

            foreach (var p in readDataPacket.Paragraphs)
            {
                if (readDataPacket.Sentences is null || readDataPacket.Sentences.Count < 1) continue;
                p.Sentences = [.. readDataPacket.Sentences
                    .Where(s => s.ParagraphId == p.Id)
                    .OrderBy(s => s.Ordinal)];

                foreach (var s in p.Sentences)
                {
                    if (readDataPacket.Tokens is null || readDataPacket.Tokens.Count < 1) continue;
                    s.Tokens = [.. readDataPacket.Tokens
                        .Where(t => t.SentenceId == s.Id)
                        .OrderBy(t => t.Ordinal)];

                    foreach (var t in s.Tokens)
                    {
                        var wordEntry = readDataPacket.AllWordsInPage
                            .Where(w => w.Value.Id == t.WordId)
                            .FirstOrDefault();
                        if (wordEntry.Value != null)
                        {
                            t.Word = wordEntry.Value;
                        }
                    }
                }
            }
            // finally, update the breadcrumbs
            if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.Id is null)
                { ErrorHandler.LogAndThrow(2130); return null; }
            UserApi.UserBreadCrumbCreate(
                context, (int)readDataPacket.LoggedInUser.Id, (int)readDataPacket.CurrentPage.Id);
            
            return readDataPacket;
        }
        public static async Task<ReadDataPacket?> OrchestrateResetReadDataForNewPageAsync(
            IdiomaticaContext context, ReadDataPacket readDataPacket, int newPageId)
        {
            return await Task<ReadDataPacket?>.Run(() =>
            {
                return OrchestrateResetReadDataForNewPage(context, readDataPacket, newPageId);
            });
        }
    }
}
