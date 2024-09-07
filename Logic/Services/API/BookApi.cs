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
            var titleT = title.Trim();
            var languageCodeT = languageCode.Trim();
            var urlT = url == null ? "" : url.Trim();

            if (titleT == string.Empty) { ErrorHandler.LogAndThrow(); return null; }
            if (languageCodeT == string.Empty) { ErrorHandler.LogAndThrow(); return null; }


            // pull language from the db
            var language = DataCache.LanguageByCodeRead(languageCodeT, context);
            if (language is null || language.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }

            // divide text into paragraphs
            string[] paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(context, text, languageCodeT);


            // add the book to the DB so you can save pages using its ID
            Book? book = new ()
            {
                Title = titleT,
                SourceURI = urlT,
                LanguageKey = language.UniqueKey,
            };
            book = DataCache.BookCreate(book, context);
            if (book is null || book.UniqueKey is null)
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
                    (Guid)book.UniqueKey, (Guid)language.UniqueKey);
                if (page is not null && page.UniqueKey is not null)
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

        public static void BookAndAllChildrenDelete(IdiomaticaContext context, Guid bookId)
        {
            DataCache.BookAndAllChildrenDelete(bookId, context);
        }
        public static async Task BookAndAllChildrenDeleteAsync(IdiomaticaContext context, Guid bookId)
        {
            await Task.Run(() =>
            {
                BookAndAllChildrenDelete(context, bookId);
            });
        }


        public static Book? BookRead(IdiomaticaContext context, Guid bookId)
        {
            return DataCache.BookByIdRead(bookId, context);
        }
        public static async Task<Book?> BookReadAsync(IdiomaticaContext context, Guid bookId)
        {
            return await DataCache.BookByIdReadAsync(bookId, context);
        }


        public static int BookReadPageCount(IdiomaticaContext context, Guid bookId)
        {
            var dbVal = DataCache.BookStatByBookIdAndStatKeyRead((bookId, AvailableBookStat.TOTALPAGES), context);
            int outVal = 0;
            if (dbVal != null) int.TryParse(dbVal.Value, out outVal);
            return outVal;
        }
        public static async Task<int> BookReadPageCountAsync(IdiomaticaContext context, Guid bookId)
        {
            return await Task<int>.Run(() =>
            {
                return BookReadPageCount(context, bookId);
            });
        }


        public static BookListDataPacket BookListRead(
            IdiomaticaContext context, Guid loggedInUserId, BookListDataPacket bookListDataPacket)
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
            IdiomaticaContext context, Guid loggedInUserId, BookListDataPacket bookListDataPacket)
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
            IdiomaticaContext context, Guid bookId, Guid userId)
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
            IdiomaticaContext context, Guid bookId, Guid userId)
        {
            return await Task<BookListRow?>.Run(() =>
            {
                return BookListRowByBookIdAndUserIdRead(context, bookId, userId);
            });

        }
    }
}
