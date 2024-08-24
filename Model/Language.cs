using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    
    [Table("Language", Schema = "Idioma")]
    public class Language
    {
        public int? Id { get; set; }

        #region relationships
        public List<Book> Books { get; set; } = [];
        public List<Word> Words { get; set; } = [];
        public List<LanguageUser> LanguageUsers { get; set; } = [];
        [StringLength(25)]
        [Column("LanguageCode")]
        public string? Code { get; set; }
        public LanguageCode? LanguageCode { get; set; }
        public List<WordTranslation> WordTranslations { get; set; } = [];

        #endregion

        [StringLength(250)]
        public string? Name { get; set; }
        [StringLength(1000)]
        public string? Dict1URI { get; set; }
        [StringLength(1000)]
        public string? Dict2URI { get; set; }
        [StringLength(1000)]
        public string? GoogleTranslateURI { get; set; }
        [StringLength(250)]
        public string? CharacterSubstitutions { get; set; }
        [StringLength(250)]
        public string? RegexpSplitSentences { get; set; }
        [StringLength(250)]
        public string? ExceptionsSplitSentences { get; set; }
        [StringLength(250)]
        public string? RegexpWordCharacters { get; set; }
        public bool? RemoveSpaces { get; set; }
        public bool? SplitEachChar { get; set; }
        public bool? RightToLeft { get; set; }
        public bool? ShowRomanization { get; set; } = false;
        [StringLength(250)]
        public string? ParserType { get; set; } = "spacedel";
        
    }
}
