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
            // reduce all double spaces, line breaks, tabs, etc. to just a single space
            // then split into a words array
            // then replace punctuation with empty strings
            string whiteSpaceCleanup = Regex.Replace(page.OriginalText, @"[\s]{1,}", " ");
            string[] words = whiteSpaceCleanup.Split(' ');
            for (int i = 0; i < words.Length; i++) words[i] = StripAllButWordCharacters(words[i]).ToLower();
            return words;
        }
        public string StripAllButWordCharacters(string input)
        {
            string pattern = $"[^{_language.LgRegexpWordCharacters}]+";
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
            if (language.LgParserType == "spacedel")
            {
                return new SpaceDelimitedLanguageParser(language);
            }
            else throw new NotImplementedException("other parser types not built yet");

        }
    }

        
        
        
    
}
