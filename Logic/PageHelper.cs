using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;

namespace Logic
{
	/*
	 * WARNING
	 * due to the way blazor hosts multiple app sessions
	 * in the same process (https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-8.0)
	 * this static class should never persist anything
	 * all functions should have zero side-effects
	 * */

	public static class PageHelper
	{
		public static int GetWordCountOfPage(Page page, Language language)
		{
			return GetWordsOfPage(page, language).Length;
		}
		public static string[] GetWordsOfPage(Page page, Language language)
		{
			var parser = LanguageParser.Factory.GetLanguageParser(language);
			return parser.GetWordsFromPage(page);
		}
        public static List<Paragraph> GetPageParagraphs(Page page, Language language)
        {
            return GetPageParagraphs(page.OriginalText, language);
        }
        public static List<Paragraph> GetPageParagraphs(string originalText, Language language)
        {
			List<Paragraph> paragraphs = new List<Paragraph>();

			var parser = LanguageParser.Factory.GetLanguageParser(language);
			var paragraphSplits = parser.SplitTextIntoParagraphs(originalText);

			int paragraphOrder = 0;
			foreach (var pText in paragraphSplits)
			{
				if (pText == string.Empty) continue;
				Paragraph paragraph = new Paragraph();
				paragraph.Order = paragraphOrder;
				paragraphOrder++;
                paragraph.Sentences = new List<Sentence>();
				var sentenceSplits = parser.SplitTextIntoSentences(pText);
                for (int i = 0; i < sentenceSplits.Length; i++)
                {
                    var sentenceSplit = sentenceSplits[i];
                    paragraph.Sentences.Add(new Sentence() 
                        { Paragraph = paragraph, Text = sentenceSplit, Order = i});
                }
				paragraphs.Add(paragraph);
			}
			return paragraphs;
		}

        public static Page? GetPageById(IdiomaticaContext context, int id)
        {
            Func<Page, bool> filter = (x => x.Id == id);
            return GetPages(context, filter).FirstOrDefault();
        }
        public static Page? GetPageByBookOrdinal(IdiomaticaContext context, int bookId, int ordinal)
        {
            Func<Page, bool> filter = (x => x.BookId == bookId && x.Order == ordinal);
            return GetPages(context, filter).FirstOrDefault();
        }
        /// <summary>
        /// provides all page content but minimal book information
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Page> GetPages(IdiomaticaContext context, Func<Page, bool> filter)
        {
            return context.Pages
                .Include(p => p.Paragraphs).ThenInclude(s => s.Sentences).ThenInclude(s => s.Tokens)
                            .ThenInclude(t => t.Word).ThenInclude(w => w.Status)
                .Include(p => p.Book)
                .Where(filter)
                .ToList();
        }
    }
}
