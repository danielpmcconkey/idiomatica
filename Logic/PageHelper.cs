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
		public static int GetWordCountOfPage(Page page, LanguageUser languageUser)
		{
			return GetWordsOfPage(page, languageUser).Length;
		}
		public static string[] GetWordsOfPage(Page page, LanguageUser languageUser)
		{
			var parser = LanguageParser.Factory.GetLanguageParser(languageUser);
			return parser.GetWordsFromPage(page);
		}
        
        /// <summary>
        ///  parses the text through the language parser and returns paragraphs, 
        ///  sentences, and tokens. it saves everything in the DB
        /// </summary>
        public static List<Paragraph> CreateParagraphsFromPageAndSave(
            Page page, LanguageUser languageUser)
        {
            if(page.Id == 0)
            {
                throw new InvalidDataException("Page must have a DB ID before adding children");
            }

			List<Paragraph> paragraphs = new List<Paragraph>();

            var wordDict = WordHelper.FetchWordDictForLanguageUser(languageUser);

            var parser = LanguageParser.Factory.GetLanguageParser(languageUser);
			var paragraphSplits = parser.SplitTextIntoParagraphs(page.OriginalText);

			int paragraphOrder = 0;
			foreach (var pText in paragraphSplits)
			{
				if (pText == string.Empty) continue;
                Paragraph paragraph = new Paragraph()
                {
                    Ordinal = paragraphOrder,
                    PageId = (int)page.Id
                };
                Insert.Paragraph(paragraph);
                paragraph.Page = page;

				paragraphOrder++;
                paragraph.Sentences = new List<Sentence>();
				var sentenceSplits = parser.SplitTextIntoSentences(pText);
                for (int i = 0; i < sentenceSplits.Length; i++)
                {
                    var sentenceSplit = sentenceSplits[i];
                    
                    var newSentence = new Sentence()
                    {
                        ParagraphId = (int)paragraph.Id,
                        Text = sentenceSplit,
                        Ordinal = i,
                    };
                    Insert.Sentence(newSentence);
                    newSentence.Paragraph = paragraph;
                        
                    
                    newSentence.Tokens = SentenceHelper.CreateTokensFromSentenceAndSave(
                        newSentence, languageUser, wordDict);
                    paragraph.Sentences.Add(newSentence);
                }
				paragraphs.Add(paragraph);
			}
			return paragraphs;
		}

        
        

        public static void RepairPage(Page page, LanguageUser languageUser)
        {
            // delete any existing paragraphs
            DeleteParagraphsFromPage(page);

            // build them back
            page.Paragraphs = CreateParagraphsFromPageAndSave(page, languageUser);
        }
        public static void DeleteParagraphsFromPage(Page page)
        {
            var existingParagraphs = Fetch.Paragraphs((p => p.PageId == page.Id));
            if (existingParagraphs != null)
            {
                using (var context = new IdiomaticaContext())
                {
                    foreach (var paragraph in existingParagraphs)
                    {
                        context.Paragraphs.Remove(paragraph);
                    }
                    context.SaveChanges();
                }
            }
            
        }
    }
}
