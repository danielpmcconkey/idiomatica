using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Language")]
    public class Language
	{
		public int? Id { get; set; }

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
        public bool RemoveSpaces { get; set; }
        public bool SplitEachChar { get; set; }
        public bool RightToLeft { get; set; }
        public bool ShowRomanization { get; set; } = false;
        public string ParserType { get; set; } = "spacedel";
        public int TotalWordsRead { get; set; } = 0;
    }
}
