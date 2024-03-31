using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Logic
{
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
	}
}
