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
        public static Page? PageCreateFromPageSplit(
            IdiomaticaContext context, int ordinal, string text,
            Book book, Language language)
        {
            if (ordinal < 0) ErrorHandler.LogAndThrow();
            string textTrimmed = text.Trim();
            if (string.IsNullOrEmpty(textTrimmed)) ErrorHandler.LogAndThrow();

            
            var newPage = new Page()
            {
                UniqueKey = Guid.NewGuid(),
                BookKey = book.UniqueKey,
                Book = book,
                Ordinal = ordinal,
                OriginalText = textTrimmed
            };
            newPage = DataCache.PageCreate(newPage, context);
            if (newPage is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            // create paragraphs
            newPage.Paragraphs = ParagraphApi.ParagraphsCreateFromPage(
                context, newPage, language);
            return newPage;
        }
        public static async Task<Page?> PageCreateFromPageSplitAsync(
            IdiomaticaContext context, int ordinal, string text,
            Guid bookId, Guid languageId)
        {
            return await Task<Page?>.Run(() =>
            {
                return PageCreateFromPageSplit(context, ordinal, text, bookId, languageId);
            });
        }


        public static Page? PageReadById(IdiomaticaContext context, Guid pageId)
        {
            return DataCache.PageByIdRead(pageId, context);
        }
        public static async Task<Page?> PageReadByIdAsync(IdiomaticaContext context, Guid pageId)
        {
            return await DataCache.PageByIdReadAsync(pageId, context);
        }


        public static Page? PageReadByOrdinalAndBookId(IdiomaticaContext context, int ordinal, Guid bookId)
        {
            if (ordinal < 1) ErrorHandler.LogAndThrow();
            return DataCache.PageByOrdinalAndBookIdRead((ordinal, bookId), context);
        }
        public static async Task<Page?> PageReadByOrdinalAndBookIdAsync(
            IdiomaticaContext context, int ordinal, Guid bookId)
        {
            return await DataCache.PageByOrdinalAndBookIdReadAsync((ordinal, bookId), context);
        }


        public static Page? PageReadFirstByBookId(IdiomaticaContext context, Guid bookId)
        {
            return DataCache.PageByOrdinalAndBookIdRead((1, bookId), context);
        }
        public static async Task<Page?> PageReadFirstByBookIdAsync(IdiomaticaContext context, Guid bookId)
        {
            return await DataCache.PageByOrdinalAndBookIdReadAsync((1, bookId), context);
        }


        /// <summary>
        ///  Used to take a string array (paragraph splits) made from the 
        ///  string provided in the book creation form and combine them into 
        ///  appropriately sized page splits
        /// </summary>
        /// <param name="paragraphSplits"></param>
        /// <returns></returns>
        public static List<(int pageNum, string pageText)> PageSplitsCreateFromParagraphSplits(
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
        public static async Task<List<(int pageNum, string pageText)>> PageSplitsCreateFromParagraphSplitsAsync(
            string[] paragraphSplits)
        {
            return await Task<List<(int pageNum, string pageText)>>.Run(() =>
            {
                return PageSplitsCreateFromParagraphSplits(paragraphSplits);
            });
        }
    }
}
