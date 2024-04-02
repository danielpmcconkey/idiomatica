using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

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
		public static int GetWordCount(Page page, Language language)
		{
			return GetWords(page, language).Length;
		}
		public static string[] GetWords(Page page, Language language)
		{
			var parser = LanguageParserFactory.GetLanguageParser(language);
			return parser.GetWordsFromPage(page);
		}
		public static List<Paragraph> GetParagraphs(Page page, Language language)
		{
			List<Paragraph> paragraphs = new List<Paragraph>();

			var parser = LanguageParserFactory.GetLanguageParser(language);
			var paragraphSplits = parser.SplitTextIntoParagraphs(page.OriginalText);

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
	}
}
