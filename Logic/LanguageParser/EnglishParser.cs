using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic.LanguageParser
{
    public class EnglishParser : ILanguageParser
    {
        public string[] SegmentTextByParagraphs(string text)
        {
            return CommonSpaceDelimetedFunctions.SegmentTextByParagraphs(text);
        }
        public string[] SegmentTextBySentences(string text)
        {
            return PragmaticSegmenterNet.Segmenter
                .Segment(text, PragmaticSegmenterNet.Language.English)
                .ToArray();
        }
        public string[] SegmentTextByWordsKeepingPunctuation(string text)
        {
            return CommonSpaceDelimetedFunctions.SegmentTextByWordsKeepingPunctuation(text);
        }
        public string StipNonWordCharacters(string text)
        {
            return CommonSpaceDelimetedFunctions.StipNonWordCharacters(text);
        }
        public string TextToLower(string text)
        {
            return CommonSpaceDelimetedFunctions.TextToLower(text);
        }
        public string FormatTranslation(string text)
        {
            throw new NotImplementedException();
        }
    }
}
