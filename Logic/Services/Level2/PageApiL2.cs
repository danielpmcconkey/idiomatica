using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepL;
using Logic.Telemetry;
using Model;
using Model.DAL;

namespace Logic.Services.Level2
{
    public static class PageApiL2
    {
        public static async Task<Page?> CreatePageFromPageSplit(
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
            bool isSaved = await DataCache.PageCreateNewAsync(newPage, context);
            if (!isSaved || newPage.Id == null || newPage.Id == 0)
            {
                ErrorHandler.LogAndThrow(2040);
                return null;
            }
            // parse paragraphs
            var paragraphSplits = ParagraphApiL2.SplitTextToPotentialParagraphs(
                context, text, language.Code);
            int paragraphOrdinal = 0;
            foreach (var paragraphSplit in paragraphSplits)
            {
                var paragraphSplitTrimmed = paragraphSplit.Trim();
                if (string.IsNullOrEmpty(paragraphSplitTrimmed)) continue;
                var paragraph = await ParagraphApiL2.CreateParagraphFromSplitAsync(context,
                    paragraphSplitTrimmed, (int)newPage.Id, paragraphOrdinal, languageId);
                if (paragraph is not null && paragraph.Id is not null or 0)
                {
                    newPage.Paragraphs.Add(paragraph);
                }
                paragraphOrdinal++;
            }
            return newPage;
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
    }
}
