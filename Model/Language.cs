using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class Language
	{
		public int Id { get; set; }

        #region relationships
        public List<LanguageUser> LanguageUsers { get; set; }
        
        #endregion

        public string Name { get; set; }
        public string Dict1URI { get; set; }
        public string? Dict2URI { get; set; }
        public string? GoogleTranslateURI { get; set; }
        public string CharacterSubstitutions { get; set; }
        public string RegexpSplitSentences { get; set; }
        public string ExceptionsSplitSentences { get; set; }
        public string RegexpWordCharacters { get; set; }
        public int RemoveSpaces { get; set; }
        public int SplitEachChar { get; set; }
        public int RightToLeft { get; set; }
        public int ShowRomanization { get; set; } = 0;
        public string ParserType { get; set; } = "spacedel";
        public int TotalWordsRead { get; set; } = 0;
    }
}
