using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic.LanguageParser
{
    public class SpaceDelimited : ILanguageParser
    {
        private Language _language;
        const char _emptyChar = '\0';

        public SpaceDelimited(Language language)
        {
            _language = language;
        }
        public string[] GetWordsFromPage(Page page)
        {
            return GetWordsFromText(page.OriginalText);
        }
        public string[] GetWordsFromText(string text)
        {
            return GetWordsFromText(text, false);
        }
        public string[] GetWordsFromText(string text, bool keepPunctuation = false)
        {
            // reduce all double spaces, line breaks, tabs, etc. to just a single space
            // then split into a words array
            // then replace punctuation with empty strings
            string whiteSpaceCleanup = Regex.Replace(text, @"[\s]{1,}", " ");
            string[] words = whiteSpaceCleanup.Split(' ');
            if(!keepPunctuation) 
                for (int i = 0; i < words.Length; i++) 
                    words[i] = StripAllButWordCharacters(words[i]).ToLower();
            return words;
        }
        public string[] SplitTextIntoSentences(string text)
        {
            Regex regex = new Regex(_language.RegexpSplitSentences);
            return regex.Split(text);
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
}
