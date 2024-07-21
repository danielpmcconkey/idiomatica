using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DeepL;
using k8s.KubeConfigModels;
using Logic.Telemetry;
using Model;
using Model.DAL;

namespace Logic.Services.API
{
    public static class PageApi
    {
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

            // mark the previous page as read before moving on

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
                readDataPacket.CurrentPage = PageReadByOrdinalAndBookId(context, targetPageNum, bookId);
                if (readDataPacket.CurrentPage is null || readDataPacket.CurrentPage.Id is null || readDataPacket.CurrentPage.Id < 1)
                {
                    ErrorHandler.LogAndThrow();
                    return null;
                }
                readDataPacket.CurrentPageUser = PageUserApi.PageUserCreateForPageIdAndUserId(
                    context, (int)readDataPacket.CurrentPage.Id, (int)readDataPacket.LoggedInUser.Id);
            }


            var newReadDataPacket = OrchestrateResetReadDataForNewPage(
                context, readDataPacket, (int)readDataPacket.CurrentPageUser.PageId);
            if (newReadDataPacket is null || newReadDataPacket.BookUser is null ||
                newReadDataPacket.BookUser.Id is null || newReadDataPacket.CurrentPageUser is null ||
                newReadDataPacket.CurrentPageUser.PageId is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            readDataPacket = newReadDataPacket;
            BookUserApi.BookUserUpdateBookmark(
                context, (int)readDataPacket.BookUser.Id, (int)readDataPacket.CurrentPageUser.PageId);

            return readDataPacket;
        }
        public static ReadDataPacket? OrchestrateResetReadDataForNewPage(IdiomaticaContext context, ReadDataPacket readDataPacket, int newPageId)
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
                p.Sentences = NullHandler.ThrowIfNullOrEmptyList<Sentence>(readDataPacket.Sentences)
                    .Where(s => s.ParagraphId == p.Id)
                    .OrderBy(s => s.Ordinal)
                    .ToList();
                foreach (var s in p.Sentences)
                {
                    s.Tokens = NullHandler.ThrowIfNullOrEmptyList<Token>(readDataPacket.Tokens)
                        .Where(t => t.SentenceId == s.Id)
                        .OrderBy(t => t.Ordinal)
                        .ToList();

                    foreach (var t in s.Tokens)
                    {
                        var wordEntry = NullHandler.ThrowIfNullOrEmptyDict(readDataPacket.AllWordsInPage)
                            .Where(w => w.Value.Id == t.WordId)
                            .FirstOrDefault();
                        if (wordEntry.Value != null)
                        {
                            t.Word = wordEntry.Value;
                        }
                    }
                }
            }
            return readDataPacket;
        }
        public static ReadDataPacket? OrchestrateReadDataInit(
            IdiomaticaContext context, UserService userService, int bookId)
        {
            ReadDataPacket readDataPacket = new();
            // tier 0 tasks, not dependent on anything
            readDataPacket.LoggedInUser = userService.GetLoggedInUser(context);
            readDataPacket.Book = BookApi.BookGet(context, bookId);
            readDataPacket.BookTotalPageCount = BookApi.BookGetPageCount(context, bookId);
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
                readDataPacket.BookUser = BookUserApi.OrchestrateBookUserCreationAndSubProcesses(
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
            return resetDataPacket;

        }
        public static Page? PageReadById(IdiomaticaContext context, int pageId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            return DataCache.PageByIdRead(pageId, context);
        }
        public static async Task<Page?> PageReadByIdAsync(IdiomaticaContext context, int pageId)
        {
            if (pageId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.PageByIdReadAsync(pageId, context);
        }
        public static Page? PageReadFirstByBookId(IdiomaticaContext context, int bookId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            return DataCache.PageByOrdinalAndBookIdRead((1, bookId), context);
        }
        public static async Task<Page?> PageReadFirstByBookIdAsync(IdiomaticaContext context, int bookId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.PageByOrdinalAndBookIdReadAsync((1, bookId), context);
        }
        public static Page? PageReadByOrdinalAndBookId(IdiomaticaContext context, int ordinal, int bookId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (ordinal < 1) ErrorHandler.LogAndThrow();
            return DataCache.PageByOrdinalAndBookIdRead((ordinal, bookId), context);
        }
        public static async Task<Page?> PageReadByOrdinalAndBookIdAsync(
            IdiomaticaContext context, int ordinal, int bookId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.PageByOrdinalAndBookIdReadAsync((ordinal, bookId), context);
        }
        public static Page? CreatePageFromPageSplit(
            IdiomaticaContext context, int ordinal, string text,
            int bookId, int languageId)
        {
            if (ordinal < 0) ErrorHandler.LogAndThrow();
            if (bookId < 0) ErrorHandler.LogAndThrow();
            if (languageId < 0) ErrorHandler.LogAndThrow();
            string textTrimmed = text.Trim();
            if (string.IsNullOrEmpty(textTrimmed)) ErrorHandler.LogAndThrow();

            // pull language from the db
            var language = DataCache.LanguageByIdRead(languageId, context);
            if (language is null ||
                language.Id is null or 0 ||
                string.IsNullOrEmpty(language.Code))
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var newPage = new Page()
            {
                BookId = bookId,
                Ordinal = ordinal,
                OriginalText = textTrimmed
            };
            newPage = DataCache.PageCreate(newPage, context);
            if (newPage is null || newPage.Id is null || newPage.Id < 1)
            {
                ErrorHandler.LogAndThrow(2040);
                return null;
            }
            // create paragraphs
            newPage.Paragraphs = ParagraphApi.CreateParagraphsFromPage(
                context, (int)newPage.Id, languageId);
            return newPage;
        }
        public static async Task<Page?> CreatePageFromPageSplitAsync(
            IdiomaticaContext context, int ordinal, string text,
            int bookId, int languageId)
        {
            return await Task<Page?>.Run(() =>
            {
                return CreatePageFromPageSplit(context, ordinal, text, bookId, languageId);
            });
        }
        /// <summary>
        ///  Used to take a string array (paragraph splits) made from the 
        ///  string provided in the book creation form and combine them into 
        ///  appropriately sized page splits
        /// </summary>
        /// <param name="paragraphSplits"></param>
        /// <returns></returns>
        public static List<(int pageNum, string pageText)> CreatePageSplitsFromParagraphSplits(
            string[] paragraphSplits)
        {
            const int _targetCharCountPerPage = 1378;// this was arrived at by DB query after conversion
            List<(int pageNum, string pageText)> pageSplits = new List<(int pageNum, string pageText)>();

            var currentPageCount = 1;
            var pageText = "";
            int currentCharCount = 0;
            bool isFirstPpOfPage = true;
            for (int i = 0; i < paragraphSplits.Length; i++)
            {
                string paragraph = paragraphSplits[i].Trim();
                if (string.IsNullOrEmpty(paragraph)) continue;
                int thisCharCount = paragraph.Length;
                if (currentCharCount + thisCharCount > _targetCharCountPerPage)
                {
                    if (isFirstPpOfPage)
                    {
                        // special weird case
                        // it's too big to fit on one page and it is the first pp of this page
                        // make it its own page
                        pageText = $"{pageText}{"\r\n"}{paragraph}";
                        currentCharCount += thisCharCount;
                        isFirstPpOfPage = false;
                    }
                    else
                    {
                        // too big, stick it on the next one
                        i -= 1; // go back one
                        // new page boundary
                        pageSplits.Add((currentPageCount, pageText));

                        // reset stuff
                        pageText = "";
                        currentCharCount = 0;
                        currentPageCount++;
                        isFirstPpOfPage = true;
                    }
                }
                else
                {
                    // add to the stack
                    pageText = $"{pageText}{"\r\n"}{paragraph}";
                    currentCharCount += thisCharCount;
                    isFirstPpOfPage = false;
                }
            }
            if (!string.IsNullOrEmpty(pageText))
            {
                // there's still text left. need to add it to a new page split
                pageSplits.Add((currentPageCount, pageText));
            }
            return pageSplits;
        }
        public static async Task<List<(int pageNum, string pageText)>> CreatePageSplitsFromParagraphSplitsAsync(
            string[] paragraphSplits)
        {
            return await Task<List<(int pageNum, string pageText)>>.Run(() =>
            {
                return CreatePageSplitsFromParagraphSplits(paragraphSplits);
            });
        }
    }
}
