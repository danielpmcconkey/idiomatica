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
using Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class BookApi
    {
        public static Book? BookCreateAndSave(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string title, AvailableLanguageCode languageCode,
            string? url, string text)
        {
            // sanitize and validate input
            var titleT = title.Trim();
            var urlT = url == null ? "" : url.Trim();

            if (titleT == string.Empty) { ErrorHandler.LogAndThrow(); return null; }
            

            // pull language from the db
            var language = DataCache.LanguageByCodeRead(languageCode, dbContextFactory);
            if (language is null) { ErrorHandler.LogAndThrow(); return null; }

            // divide text into paragraphs
            string[] paragraphSplits = ParagraphApi.PotentialParagraphsSplitFromText(dbContextFactory, text, language);


            // add the book to the DB so you can save pages using its ID
            Book? book = new ()
            {
                Id = Guid.NewGuid(),
                Title = titleT,
                SourceURI = urlT,
                LanguageId = language.Id,
                //Language = language,
            };
            book = DataCache.BookCreate(book, dbContextFactory);
            if (book is null)
            {
                ErrorHandler.LogAndThrow();
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
                Page? page = PageApi.PageCreateFromPageSplit(dbContextFactory,
                    pageSplit.pageNum, pageSplitTextTrimmed, book, language);
                if (page is not null)
                {
                    book.Pages.Add(page);
                }
            }
            return book;
        }
        public static async Task<Book?> BookCreateAndSaveAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, string title, AvailableLanguageCode languageCode,
            string? url, string text)
        {
            return await Task<Book?>.Run(() =>
            {
                return BookCreateAndSave(dbContextFactory, title, languageCode, url, text);
            });
        }

        public static void BookAndAllChildrenDelete(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId)
        {
            DataCache.BookAndAllChildrenDelete(bookId, dbContextFactory);
        }
        public static async Task BookAndAllChildrenDeleteAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId)
        {
            await Task.Run(() =>
            {
                BookAndAllChildrenDelete(dbContextFactory, bookId);
            });
        }


        public static Book? BookRead(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId)
        {
            return DataCache.BookByIdRead(bookId, dbContextFactory);
        }
        public static async Task<Book?> BookReadAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId)
        {
            return await DataCache.BookByIdReadAsync(bookId, dbContextFactory);
        }


        public static int BookReadPageCount(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId)
        {
            var dbVal = DataCache.BookStatByBookIdAndStatKeyRead((bookId, AvailableBookStat.TOTALPAGES), dbContextFactory);
            int outVal = 0;
            if (dbVal != null) int.TryParse(dbVal.Value, out outVal);
            return outVal;
        }
        public static async Task<int> BookReadPageCountAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId)
        {
            return await Task<int>.Run(() =>
            {
                return BookReadPageCount(dbContextFactory, bookId);
            });
        }


        public static BookListDataPacket BookListRead(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid loggedInUserId, BookListDataPacket bookListDataPacket)
        {
            var powerQueryResults = DataCache.BookListRowsPowerQuery(
                    loggedInUserId,
                    bookListDataPacket.BookListRowsToDisplay,
                    bookListDataPacket.SkipRecords,
                    bookListDataPacket.ShouldShowOnlyInShelf,
                    bookListDataPacket.TagsFilter,
                    bookListDataPacket.LcFilter?.Id,
                    bookListDataPacket.TitleFilter,
                    bookListDataPacket.SortProperty,
                    bookListDataPacket.ShouldSortAscending,
                    dbContextFactory);

            bookListDataPacket.BookListRows = powerQueryResults.results;
            bookListDataPacket.BookListTotalRowsAtCurrentFilter = powerQueryResults.count;
            return bookListDataPacket;

        }
        public static async Task<BookListDataPacket> BookListReadAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid loggedInUserId, BookListDataPacket bookListDataPacket)
        {
            var powerQueryResults = await DataCache.BookListRowsPowerQueryAsync(
                    loggedInUserId,
                    bookListDataPacket.BookListRowsToDisplay,
                    bookListDataPacket.SkipRecords,
                    bookListDataPacket.ShouldShowOnlyInShelf,
                    bookListDataPacket.TagsFilter,
                    bookListDataPacket.LcFilter?.Id,
                    bookListDataPacket.TitleFilter,
                    bookListDataPacket.SortProperty,
                    bookListDataPacket.ShouldSortAscending,
                    dbContextFactory);

            bookListDataPacket.BookListRows = powerQueryResults.results;
            bookListDataPacket.BookListTotalRowsAtCurrentFilter = powerQueryResults.count;
            return bookListDataPacket;

        }


        public static BookListRow? BookListRowByBookIdAndUserIdRead(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
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
                    dbContextFactory,
                    bookId);

            return powerQueryResults.results.FirstOrDefault();
        }
        public static async Task<BookListRow?> BookListRowByBookIdAndUserIdReadAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            return await Task<BookListRow?>.Run(() =>
            {
                return BookListRowByBookIdAndUserIdRead(dbContextFactory, bookId, userId);
            });

        }
    }
}
