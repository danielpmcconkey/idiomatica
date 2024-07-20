using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;

namespace Logic.Services.API
{
    public static class BookApi
    {
        /// <summary>
        /// orchestrates the processes to create the book, book stats, book user 
        /// for the creating user, word users for the creating user, and the 
        /// book user stats for the creating user
        /// </summary>
        public static async Task<Book?> OrchestrateBookCreationAndSubProcesses(
            IdiomaticaContext context, int userId,  string title, string languageCode,
            string? url, string text)
        {
            Book? book = await BookApi.BookCreateAndSaveAsync(context, title, languageCode, url, text);
            if(book is null || book.Id is null || book.Id < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            int bookId = (int)book.Id;
            // add the book stats
            await BookStatApi.BookStatsCreateAndSaveAsync(context, bookId);
            // now create the book user for the logged in user
            if(userId < 1)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            var bookUser = BookUserApi.OrchestrateBookUserCreationAndSubProcesses(context, bookId, userId);
            

            return book;
        }
        
        public static async Task<Book?> BookCreateAndSaveAsync(
            IdiomaticaContext context, string title, string languageCode,
            string? url, string text)
        {
            // sanitize and validate input
            var titleT = NullHandler.ThrowIfNullOrEmptyString(title.Trim());
            var languageCodeT = NullHandler.ThrowIfNullOrEmptyString(languageCode.Trim());
            var urlT = url == null ? "" : url.Trim();
            

            // pull language from the db
            var language = DataCache.LanguageByCodeRead(languageCodeT, context);
            if (language is null || language.Id is null or 0)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // divide text into paragraphs
            string[] paragraphSplits = ParagraphApi.SplitTextToPotentialParagraphs(context, text, languageCodeT);
            

            // add the book to the DB so you can save pages using its ID
            Book? book = new Book()
            {
                Title = titleT,
                SourceURI = urlT,
                LanguageId = language.Id,
            };
            book = await DataCache.BookCreateAsync(book, context);
            if (book is null || book.Id is null || book.Id < 1)
            {
                ErrorHandler.LogAndThrow(2090);
                return null;
            }

            var pageSplits = PageApi.CreatePageSplitsFromParagraphSplits(paragraphSplits);
            if (pageSplits is null || pageSplits.Count == 0)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // create page objects
            foreach (var pageSplit in pageSplits)
            {
                string pageSplitTextTrimmed = pageSplit.pageText.Trim();
                if (string.IsNullOrEmpty(pageSplitTextTrimmed)) continue;
                Page? page = await PageApi.CreatePageFromPageSplitAsync(context,
                    pageSplit.pageNum, pageSplitTextTrimmed, 
                    (int)book.Id, (int)language.Id);
                if (page is not null && page.Id is not null or 0)
                {
                    book.Pages.Add(page);
                }
            }

            return book;
        }
        public static Book? BookGet(IdiomaticaContext context, int bookId)
        {
            return DataCache.BookByIdRead(bookId, context);
        }
        public static async Task<Book?> BookGetAsync(IdiomaticaContext context, int bookId)
        {
            return await DataCache.BookByIdReadAsync(bookId, context);
        }
        public static int BookGetPageCount(IdiomaticaContext context, int bookId)
        {
            var dbVal = DataCache.BookStatByBookIdAndStatKeyRead((bookId, AvailableBookStat.TOTALPAGES), context);
            int outVal = 0;
            if (dbVal != null) int.TryParse(dbVal.Value, out outVal);
            return outVal;
        }
        public static async Task<int> BookGetPageCountAsync(IdiomaticaContext context, int bookId)
        {
            return await Task<int>.Run(() =>
            {
                return BookGetPageCount(context, bookId);
            });
        }
        public static async Task<BookListDataPacket> BookListReadAsync(
            IdiomaticaContext context, int loggedInUserId, BookListDataPacket bookListDataPacket)
        {
            var powerQueryResults = await DataCache.BookListRowsPowerQueryAsync(
                    loggedInUserId,
                    bookListDataPacket.BookListRowsToDisplay,
                    bookListDataPacket.SkipRecords,
                    bookListDataPacket.ShouldShowOnlyInShelf,
                    bookListDataPacket.TagsFilter,
                    bookListDataPacket.LcFilter,
                    bookListDataPacket.TitleFilter,
                    bookListDataPacket.SortProperty,
                    bookListDataPacket.ShouldSortAscending,
                    context);

            bookListDataPacket.BookListRows = powerQueryResults.results;
            bookListDataPacket.BookListTotalRowsAtCurrentFilter = powerQueryResults.count;
            return bookListDataPacket;

        }
    }
}
