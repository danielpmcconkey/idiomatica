using Logic.UILabels;
using Microsoft.IdentityModel.Tokens;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic.LanguageParser
{
    public class SpanishParser : ILanguageParser
    {
        public string[] SegmentTextByParagraphs(string text)
        {
            return CommonSpaceDelimetedFunctions.SegmentTextByParagraphs(text);
        }
        public string[] SegmentTextBySentences(string text)
        {
            return PragmaticSegmenterNet.Segmenter
                .Segment(text, PragmaticSegmenterNet.Language.Spanish)
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
            // spanish dict verb conjugations
            const string pattern = "(Preterite|Present|Affirmative imperative|Subjunctive|Imperfect|Conditional|Future|Future subjunctive|Imperfect subjunctive|Future subjunctive|Negative imperative)(nosotros|tú|usted|vosotros|él/ella/usted|ustedes|yo|ellos/ellas/ustedes|vos)conjugation of([\\w]{1,})\\.";
            const string replacement = "$1 $2 conjugation of $3";
            var formatted = Regex.Replace(text, pattern, replacement, RegexOptions.None);
            return formatted;
        }
    }
}
