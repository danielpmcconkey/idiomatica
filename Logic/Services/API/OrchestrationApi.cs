using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Expressions;
using Model.Enums;
using Logic.Conjugator.English;
using Microsoft.EntityFrameworkCore;

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
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId, string title, AvailableLanguageCode languageCode,
            string? url, string text)
        {
            Book? book = BookApi.BookCreateAndSave(dbContextFactory, title, languageCode, url, text);
            if (book is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            Guid bookId = (Guid)book.Id;
            // add the book stats
            book.BookStats = BookStatApi.BookStatsCreateAndSave(dbContextFactory, bookId);
            // now create the book user for the logged in user
            var bookUser = OrchestrateBookUserCreationAndSubProcesses(dbContextFactory, bookId, userId);
            if (bookUser is null) { ErrorHandler.LogAndThrow(); return null; }
            book.BookUsers.Add(bookUser);

            return book;
        }
        public static async Task<Book?> OrchestrateBookCreationAndSubProcessesAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId, string title, AvailableLanguageCode languageCode,
            string? url, string text)
        {
            return await Task<Book?>.Run(() =>
            {
                return OrchestrateBookCreationAndSubProcesses(dbContextFactory, userId, title, languageCode, url, text);
            });
        }


        /// <summary>
        /// Create the book user, word users, and book user stats
        /// </summary>
        public static BookUser? OrchestrateBookUserCreationAndSubProcesses(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            var bookUser = BookUserApi.BookUserCreate(dbContextFactory, bookId, userId);
            if (bookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // create the wordUsers
            WordUserApi.WordUsersCreateAllForBookIdAndUserId(dbContextFactory, bookId, userId);
            // update bookUserStats
            BookUserStatApi.BookUserStatsUpdateByBookUserId(dbContextFactory, (Guid)bookUser.Id);

            return bookUser;
        }
        public static async Task<BookUser?> OrchestrateBookUserCreationAndSubProcessesAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return OrchestrateBookUserCreationAndSubProcesses(dbContextFactory, bookId, userId);
            });
        }


        public static ReadDataPacket? OrchestrateClearPageAndMove(IDbContextFactory<IdiomaticaContext> dbContextFactory, ReadDataPacket readDataPacket, int targetPageNum)
        {
            ReadDataPacket? outPacket = readDataPacket;
            if (readDataPacket.CurrentPageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // update all unknowns to well known
            PageUserApi.PageUserUpdateUnknowWordsToWellKnown(dbContextFactory, (Guid)readDataPacket.CurrentPageUser.Id);
            // now move forward, if there's another page
            if (targetPageNum <= readDataPacket.BookTotalPageCount) // remember pages are 1-indexed
            {
                if (readDataPacket.Book is null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                outPacket = OrchestrateMovePage(dbContextFactory, readDataPacket, (Guid)readDataPacket.Book.Id, targetPageNum);
                if (outPacket is null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
            }
            else
            {
                // mark the previous page as read because you didn't do it in the PageMove function
                PageUserApi.PageUserMarkAsRead(dbContextFactory, readDataPacket.CurrentPageUser.Id);
                // refresh the word user cache
                if (outPacket is null || outPacket.CurrentPageUser is null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                outPacket = OrchestrateResetReadDataForNewPage(
                    dbContextFactory, readDataPacket, (Guid)outPacket.CurrentPageUser.Id);
            }
            return outPacket;
        }
        public static async Task<ReadDataPacket?> OrchestrateClearPageAndMoveAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, ReadDataPacket readDataPacket, int targetPageNum)
        {
            return await Task<ReadDataPacket?>.Run(() =>
            {
                return OrchestrateClearPageAndMove(dbContextFactory, readDataPacket, targetPageNum);
            });
        }

        /// <summary>
        ///  take the result of the last card, update the last card, and move the next card to the top
        /// </summary>
        public static FlashCardDataPacket? OrchestrateFlashCardDispositionAndAdvance(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, FlashCardDataPacket? dataPacket,
            AvailableFlashCardAttemptStatus previousCardsStatus)
        {
            if (dataPacket is null || dataPacket.CurrentCard is null)
                { ErrorHandler.LogAndThrow(); return null; }

            // create the attempt object
            var attempt = FlashCardAttemptApi.FlashCardAttemptCreate(
                dbContextFactory, dataPacket.CurrentCard, previousCardsStatus);

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
                dbContextFactory, (Guid)dataPacket.CurrentCard.Id, (Guid)dataPacket.CurrentCard.WordUserId,
                newStatus, nextReview, (Guid)dataPacket.CurrentCard.Id);

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
            IDbContextFactory<IdiomaticaContext> dbContextFactory, FlashCardDataPacket? dataPacket,
            AvailableFlashCardAttemptStatus previousCardsStatus)
        {
            return await Task<FlashCardDataPacket?>.Run(() =>
            {
                return OrchestrateFlashCardDispositionAndAdvance(dbContextFactory, dataPacket, previousCardsStatus);
            });
        }


        public static FlashCardDataPacket? OrchestrateFlashCardDeckCreation(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId,
            AvailableLanguageCode learningLanguageCode,
            int numNew, int numReview)
        {
            if (numNew < 0) { ErrorHandler.LogAndThrow(); return null; }
            if (numReview < 0) { ErrorHandler.LogAndThrow(); return null; }

            var context = dbContextFactory.CreateDbContext();

            FlashCardDataPacket newDataPacket = new ();
            newDataPacket.LearningLanguageCode = learningLanguageCode;
            var uiLanguage = UserApi.UserSettingUiLanguagReadByUserId(dbContextFactory, userId);
            if (uiLanguage is null) 
                { ErrorHandler.LogAndThrow(); return null; }
            newDataPacket.UILanguageCode = uiLanguage.Code;

            // pull the LanguageUserId
            var language = DataCache.LanguageByCodeRead(learningLanguageCode, dbContextFactory);
            if (language is null) { ErrorHandler.LogAndThrow(); return null; }
            var languageUser = DataCache.LanguageUserByLanguageIdAndUserIdRead(
                (language.Id, userId), dbContextFactory);
            if (languageUser is null) { ErrorHandler.LogAndThrow(); return null; }

            // pull cards that are ready for their next review
            Expression<Func<FlashCard, bool>> predicate = fc => fc.WordUser != null
                    && fc.WordUser.LanguageUserId == languageUser.Id
                    && fc.Status == AvailableFlashCardStatus.ACTIVE
                    && fc.NextReview != null
                    && fc.NextReview <= DateTimeOffset.Now;

            var oldCards = FlashCardApi.FlashCardsFetchByNextReviewDateByPredicate(
                dbContextFactory, predicate, numReview);

            if (oldCards is not null && oldCards.Count > 0) 
                newDataPacket.Deck.AddRange(oldCards);

            // pull cards that have already been created, but never reviewed
            Expression<Func<FlashCard, bool>> predicate2 = fc => fc.WordUser != null
                    && fc.WordUser.LanguageUserId == languageUser.Id
                    && fc.Status == AvailableFlashCardStatus.ACTIVE
                    && fc.NextReview == null;

            var readyForReviewCards = FlashCardApi.FlashCardsFetchByNextReviewDateByPredicate(
                dbContextFactory, predicate2, numNew);

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
                    dbContextFactory, (Guid)languageUser.Id, numToCreate, uiLanguage.Code);
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
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId, AvailableLanguageCode learningLanguageCode,
            int numNew, int numReview)
        {
            return await Task<FlashCardDataPacket?>.Run(() =>
            {
                return OrchestrateFlashCardDeckCreation(dbContextFactory, userId,
                    learningLanguageCode, numNew, numReview);
            });
        }


        public static ReadDataPacket? OrchestrateMovePage(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, ReadDataPacket readDataPacket, Guid bookId, int targetPageNum)
        {
            if (readDataPacket.LanguageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.CurrentPageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.BookUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.LoggedInUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.CurrentPage is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // mark the previous page as read if moving forward
            if (targetPageNum > readDataPacket.CurrentPage.Ordinal)
                PageUserApi.PageUserMarkAsRead(dbContextFactory, (Guid)readDataPacket.CurrentPageUser.Id);

            if (targetPageNum < 1) return null;
            if (targetPageNum > readDataPacket.BookTotalPageCount)
                return null;

            // reload the current page user with the new target
            readDataPacket.CurrentPageUser = PageUserApi.PageUserReadByOrderWithinBook(
                dbContextFactory, (Guid)readDataPacket.LanguageUser.Id, targetPageNum, bookId);

            // create the page user if it hasn't been created already
            if (readDataPacket.CurrentPageUser is null)
            {
                // but first need to pull the page
                readDataPacket.CurrentPage = PageApi.PageReadByOrdinalAndBookId(dbContextFactory, targetPageNum, bookId);
                if (readDataPacket.CurrentPage is null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                readDataPacket.CurrentPageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, readDataPacket.CurrentPage, readDataPacket.LoggedInUser);
            }
            if (readDataPacket.CurrentPageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }


            var newReadDataPacket = OrchestrateResetReadDataForNewPage(
                dbContextFactory, readDataPacket, (Guid)readDataPacket.CurrentPageUser.PageId);
            if (newReadDataPacket is null || newReadDataPacket.BookUser is null
                || newReadDataPacket.CurrentPageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            readDataPacket = newReadDataPacket;
            BookUserApi.BookUserUpdateBookmark(
                dbContextFactory, (Guid)readDataPacket.BookUser.Id, (Guid)readDataPacket.CurrentPageUser.PageId);

            return readDataPacket;
        }
        public static async Task<ReadDataPacket?> OrchestrateMovePageAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, ReadDataPacket readDataPacket, Guid bookId, int targetPageNum)
        {
            return await Task<ReadDataPacket?>.Run(() =>
            {
                return OrchestrateMovePage(dbContextFactory, readDataPacket, bookId, targetPageNum);
            });
        }


        public static ReadDataPacket? OrchestrateReadDataInit(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, LoginService userService, Guid bookId)
        {
            ReadDataPacket readDataPacket = new();
            // tier 0 tasks, not dependent on anything
            readDataPacket.LoggedInUser = userService.GetLoggedInUser(dbContextFactory);
            readDataPacket.Book = BookApi.BookRead(dbContextFactory, bookId);
            readDataPacket.BookTotalPageCount = BookApi.BookReadPageCount(dbContextFactory, bookId);
            readDataPacket.LanguageToCode = userService.GetUiLanguageCode();

            if (readDataPacket.LoggedInUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.LanguageToCode is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.Book is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // tier 1 tasks, dependent on tier 0
            readDataPacket.BookUser = BookUserApi.BookUserByBookIdAndUserIdRead(
                dbContextFactory, (Guid)readDataPacket.Book.Id, (Guid)readDataPacket.LoggedInUser.Id);
            readDataPacket.LanguageUser = LanguageUserApi.LanguageUserGet(
                dbContextFactory, (Guid)readDataPacket.Book.LanguageId, (Guid)readDataPacket.LoggedInUser.Id);
            readDataPacket.Language = LanguageApi.LanguageRead(dbContextFactory, (Guid)readDataPacket.Book.LanguageId);


            if (readDataPacket.BookUser is null)
            {
                // create it, I guess
                readDataPacket.BookUser = OrchestrateBookUserCreationAndSubProcesses(
                    dbContextFactory, bookId, (Guid)readDataPacket.LoggedInUser.Id);
            }
            if (readDataPacket.BookUser is null)
            {
                // still null, something went wrong
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.LanguageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.Language is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // tier 2, dependent on tier 1
            readDataPacket.BookUserStats = BookUserStatApi.BookUserStatsRead(
                dbContextFactory, (Guid)readDataPacket.Book.Id, (Guid)readDataPacket.LoggedInUser.Id);
            readDataPacket.CurrentPageUser = PageUserApi.PageUserReadBookmarkedOrFirst(
                dbContextFactory, (Guid)readDataPacket.BookUser.Id);
            readDataPacket.LanguageFromCode = LanguageApi.LanguageReadByCode(
                dbContextFactory, readDataPacket.Language.Code);



            // create the page user if it hasn't been created already
            if (readDataPacket.CurrentPageUser is null)
            {
                // but first need to pull the page
                readDataPacket.CurrentPage = PageApi.PageReadFirstByBookId(dbContextFactory, bookId);
                if (readDataPacket.CurrentPage is null)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                readDataPacket.CurrentPageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    dbContextFactory, readDataPacket.CurrentPage, readDataPacket.LoggedInUser);
            }
            else
            {
                // just pull the current page
                readDataPacket.CurrentPage = PageApi.PageReadById(
                    dbContextFactory, readDataPacket.CurrentPageUser.PageId);
            }
            if (readDataPacket.CurrentPage is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // tier 3, dependent on tier 2
            var resetDataPacket = OrchestrateResetReadDataForNewPage(
                dbContextFactory, readDataPacket, readDataPacket.CurrentPage.Id);
            if (resetDataPacket is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // finally, update the breadcrumbs
            UserApi.UserBreadCrumbCreate(
                dbContextFactory, readDataPacket.LoggedInUser, readDataPacket.CurrentPage);
            
            return resetDataPacket;

        }
        public static async Task<ReadDataPacket?> OrchestrateReadDataInitAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, LoginService userService, Guid bookId)
        {
            return await Task<ReadDataPacket?>.Run(() =>
            {
                return OrchestrateReadDataInit(dbContextFactory, userService, bookId);
            });
        }


        public static ReadDataPacket? OrchestrateResetReadDataForNewPage(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, ReadDataPacket readDataPacket, Guid newPageId)
        {
            if (readDataPacket.LanguageUser is null )
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            if (readDataPacket.LoggedInUser is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // wipe the old ones out
            readDataPacket.CurrentPage = null;
            readDataPacket.Paragraphs = null;
            readDataPacket.AllWordsInPage = null;
            readDataPacket.Sentences = null;
            readDataPacket.Tokens = null;

            // and rebuild
            readDataPacket.CurrentPage = PageApi.PageReadById(dbContextFactory, newPageId);
            readDataPacket.Paragraphs = ParagraphApi.ParagraphsReadByPageId(dbContextFactory, newPageId);
            readDataPacket.AllWordsInPage = WordApi
                .WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserIdRead(
                dbContextFactory, newPageId, (Guid)readDataPacket.LanguageUser.Id);
            readDataPacket.Sentences = SentenceApi.SentencesReadByPageId(dbContextFactory, newPageId);
            readDataPacket.Tokens = TokenApi.TokensReadByPageId(dbContextFactory, newPageId);

            if (readDataPacket.AllWordsInPage is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            if (readDataPacket.Paragraphs is null)
            {
                ErrorHandler.LogAndThrow();
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
            if (readDataPacket.CurrentPage is null)
                { ErrorHandler.LogAndThrow(); return null; }
            UserApi.UserBreadCrumbCreate(
                dbContextFactory, readDataPacket.LoggedInUser, readDataPacket.CurrentPage);
            
            return readDataPacket;
        }
        public static async Task<ReadDataPacket?> OrchestrateResetReadDataForNewPageAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, ReadDataPacket readDataPacket, Guid newPageId)
        {
            return await Task<ReadDataPacket?>.Run(() =>
            {
                return OrchestrateResetReadDataForNewPage(dbContextFactory, readDataPacket, newPageId);
            });
        }

    }
}
