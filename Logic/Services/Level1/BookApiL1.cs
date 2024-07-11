using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Services.Level2;

namespace Logic.Services.Level1
{
    public static class BookApiL1
    {
        public static async Task<Book?> BookCreateAndSaveAsync(
            IdiomaticaContext context, string title, string languageCode, string? url, string text)
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
            string[] paragraphSplits = ParagraphApiL2.SplitTextToPotentialParagraphs(context, text, languageCode);
            

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

            var pageSplits = PageApiL2.CreatePageSplitsFromParagraphSplits(paragraphSplits);
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
                Page? page = await PageApiL2.CreatePageFromPageSplit(context,
                    pageSplit.pageNum, pageSplitTextTrimmed, 
                    (int)book.Id, (int)language.Id);
                if (page is not null && page.Id is not null or 0)
                {
                    book.Pages.Add(page);
                }
            }

            return book;
        }
    }
}
