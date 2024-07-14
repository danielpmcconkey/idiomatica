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
            string[] paragraphSplits = ParagraphApi.SplitTextToPotentialParagraphs(context, text, languageCode);
            

            // add the book to the DB so you can save pages using its ID
            Book book = new Book()
            {
                Title = titleT,
                SourceURI = urlT,
                LanguageId = language.Id,
            };
            bool didSaveBook = await DataCache.BookCreateAsync(book, context);
            if (!didSaveBook || book.Id == null || book.Id < 1)
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
                Page? page = await PageApi.CreatePageFromPageSplit(context,
                    pageSplit.pageNum, pageSplitTextTrimmed, 
                    (int)book.Id, (int)language.Id);
                if (page is not null && page.Id is not null or 0)
                {
                    book.Pages.Add(page);
                }
            }

            return book;
        }
        public static async Task<Book?> BookGetAsync(IdiomaticaContext context, int bookId)
        {
            return await DataCache.BookByIdReadAsync(bookId, context);
        }
        public static async Task<int> BookGetPageCountAsync(IdiomaticaContext context, int bookId)
        {
            var dbVal = await DataCache.BookStatByBookIdAndStatKeyReadAsync((bookId, AvailableBookStat.TOTALPAGES), context);
            int outVal = 0;
            if (dbVal != null) int.TryParse(dbVal.Value, out outVal);
            return outVal;
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
