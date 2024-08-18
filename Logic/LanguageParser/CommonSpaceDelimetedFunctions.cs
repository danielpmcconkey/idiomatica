using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic.LanguageParser
{
    internal static class CommonSpaceDelimetedFunctions
    {
        public static string[] SegmentTextByParagraphs(string text)
        {
            Regex regex = new Regex(@"[\n\r]{1,}");
            string[] splits = regex.Split(text);
            return splits.Where(s => String.IsNullOrEmpty(s) == false).ToArray();
        }
        public static string[] SegmentTextByWordsKeepingPunctuation(string text)
        {
            // reduce all double spaces, line breaks, tabs, etc. to just a single space
            // then split into a words array
            string whiteSpaceCleanup = Regex.Replace(text, @"[\s]{1,}", " ");
            string[] words = whiteSpaceCleanup.Split(' ');
            return words;
        }
        public static string StipNonWordCharacters(string text)
        {
            // choosing to count numbers as word characters even though that
            // could get me in trouble later
            string pattern = $"[^{"a-zA-ZáéíóúÁÉÍÓÚñÑüÜ0-9"}]+";
            string replacement = "";
            return Regex.Replace(text, pattern, replacement);
        }
        public static string TextToLower(string text)
        {
            return text.ToLower();
        }
    }
}
