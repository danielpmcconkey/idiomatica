using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;
using DeepL;
using static System.Net.Mime.MediaTypeNames;

namespace Logic.Services.API
{
    public static class BookApi
    {
        public static Book? BookCreateAndSave(
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
            string[] paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(context, text, languageCodeT);


            // add the book to the DB so you can save pages using its ID
            Book? book = new Book()
            {
                Title = titleT,
                SourceURI = urlT,
                LanguageId = language.Id,
            };
            book = DataCache.BookCreate(book, context);
            if (book is null || book.Id is null || book.Id < 1)
            {
                ErrorHandler.LogAndThrow(2090);
                return null;
            }

            var pageSplits = PageApi.PageSplitsCreateFromParagraphSplits(paragraphSplits);
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
                Page? page = PageApi.PageCreateFromPageSplit(context,
                    pageSplit.pageNum, pageSplitTextTrimmed,
                    (int)book.Id, (int)language.Id);
                if (page is not null && page.Id is not null or 0)
                {
                    book.Pages.Add(page);
                }
            }

            return book;
        }
        public static async Task<Book?> BookCreateAndSaveAsync(
            IdiomaticaContext context, string title, string languageCode,
            string? url, string text)
        {
            return await Task<Book?>.Run(() =>
            {
                return BookCreateAndSave(context, title, languageCode, url, text);
            });
        }


        public static Book? BookRead(IdiomaticaContext context, int bookId)
        {
            return DataCache.BookByIdRead(bookId, context);
        }
        public static async Task<Book?> BookReadAsync(IdiomaticaContext context, int bookId)
        {
            return await DataCache.BookByIdReadAsync(bookId, context);
        }


        public static int BookReadPageCount(IdiomaticaContext context, int bookId)
        {
            var dbVal = DataCache.BookStatByBookIdAndStatKeyRead((bookId, AvailableBookStat.TOTALPAGES), context);
            int outVal = 0;
            if (dbVal != null) int.TryParse(dbVal.Value, out outVal);
            return outVal;
        }
        public static async Task<int> BookReadPageCountAsync(IdiomaticaContext context, int bookId)
        {
            return await Task<int>.Run(() =>
            {
                return BookReadPageCount(context, bookId);
            });
        }


        public static BookListDataPacket BookListRead(
            IdiomaticaContext context, int loggedInUserId, BookListDataPacket bookListDataPacket)
        {
            var powerQueryResults = DataCache.BookListRowsPowerQuery(
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


        public static BookListRow? BookListRowByBookIdAndUserIdRead(
            IdiomaticaContext context, int bookId, int userId)
        {
            var powerQueryResults = DataCache.BookListRowsPowerQuery(
                    userId,
                    1,
                    0,
                    false,
                    null,
                    null,
                    null,
                    null,
                    true,
                    context,
                    bookId);

            return powerQueryResults.results.FirstOrDefault();
        }
        public static async Task<BookListRow?> BookListRowByBookIdAndUserIdReadAsync(
            IdiomaticaContext context, int bookId, int userId)
        {
            return await Task<BookListRow?>.Run(() =>
            {
                return BookListRowByBookIdAndUserIdRead(context, bookId, userId);
            });

        }
    }
}
