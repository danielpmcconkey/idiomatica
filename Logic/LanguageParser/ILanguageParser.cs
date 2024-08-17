using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Logic.LanguageParser
{
    // todo: remove any language parsing elements from the DB
    public interface ILanguageParser
    {
        public string[] SegmentTextByParagraphs(string text);
        public string[] SegmentTextBySentences(string text);
        public string[] SegmentTextByWordsKeepingPunctuation(string text);
        public string StipNonWordCharacters(string text);
        public string TextToLower(string text);
        //public string StripAllButWordCharacters(string input);
        //public string[] GetWordsFromPage(Page page);
        //public string[] GetWordsFromText(string text);
        //public string[] GetWordsFromText(string text, bool keepPunctuation);
        //public string[] SplitTextIntoSentences(string text);
        //public string[] SplitTextIntoParagraphs(string text);
    }
}
