using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic
{
    public interface LanguageParser
    {
        public string StripAllButWordCharacters(string input);
		public string[] GetWordsFromPage(Page page);
		public string[] GetWordsFromText(string text);
		public string[] SplitTextIntoSentences(string text);
		public string[] SplitTextIntoParagraphs(string text);
	}
    public class SpaceDelimitedLanguageParser : LanguageParser
    {
        private Language _language;
        const char _emptyChar = '\0';

        public SpaceDelimitedLanguageParser(Language language)
        {  
            _language = language; 
        }
        public string[] GetWordsFromPage(Page page)
		{
			return GetWordsFromText(page.OriginalText);
		}
		public string[] GetWordsFromText(string text)
		{
            // reduce all double spaces, line breaks, tabs, etc. to just a single space
            // then split into a words array
            // then replace punctuation with empty strings
            string whiteSpaceCleanup = Regex.Replace(text, @"[\s]{1,}", " ");
            string[] words = whiteSpaceCleanup.Split(' ');
            for (int i = 0; i < words.Length; i++) words[i] = StripAllButWordCharacters(words[i]).ToLower();
            return words;
        }
		public string[] SplitTextIntoSentences(string text)
		{
			Regex regex = new Regex(_language.RegexpSplitSentences);
			string[] sentences = regex.Split(text);
			return sentences;
		}
		public string[] SplitTextIntoParagraphs(string text)
		{
			// normalize all line breaks
			var lbSearch = "[\\r\\n]{1,}";
			string normalized = Regex.Replace(text, "[\\r\\n]{1,}", "\\n");
			// now split
			Regex regex = new Regex("\\n");
			string[] paragraphs = regex.Split(text);
			return paragraphs;
		}

		public string StripAllButWordCharacters(string input)
        {
            string pattern = $"[^{_language.RegexpWordCharacters}]+";
            string replacement = "";
            return Regex.Replace(input, pattern, replacement);
        }
    }
	
	/*
	 * WARNING
	 * due to the way blazor hosts multiple app sessions
	 * in the same process (https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-8.0)
	 * this static class should never persist anything
	 * all functions should have zero side-effects
	 * */
	internal static class LanguageParserFactory
    {
        internal static LanguageParser GetLanguageParser(Language language)
        {
            if (language.ParserType == "spacedel")
            {
                return new SpaceDelimitedLanguageParser(language);
            }
            else throw new NotImplementedException("other parser types not built yet");

        }
    }

        
        
        
    
}
