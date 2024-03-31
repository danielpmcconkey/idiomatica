using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Language
    {
        [Column("LgID")] public int Id { get; set; }

        #region relationships
        public List<Book> Books { get; set; }
        public List<Word> Words { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        #endregion

        [Column("LgName")] public string LgName { get; set; }
        [Column("LgDict1URI")] public string LgDict1URI { get; set; }
        [Column("LgDict2URI")] public string? LgDict2URI { get; set; }
        [Column("LgGoogleTranslateURI")] public string? LgGoogleTranslateURI { get; set; }
        [Column("LgCharacterSubstitutions")] public string LgCharacterSubstitutions { get; set; }
        [Column("LgRegexpSplitSentences")] public string LgRegexpSplitSentences { get; set; }
        [Column("LgExceptionsSplitSentences")] public string LgExceptionsSplitSentences { get; set; }
        [Column("LgRegexpWordCharacters")] public string LgRegexpWordCharacters { get; set; }
        [Column("LgRemoveSpaces")] public int LgRemoveSpaces { get; set; }
        [Column("LgSplitEachChar")] public int LgSplitEachChar { get; set; }
        [Column("LgRightToLeft")] public int LgRightToLeft { get; set; }
        [Column("LgShowRomanization")] public int LgShowRomanization { get; set; } = 0;
        [Column("LgParserType")] public string LgParserType { get; set; } = "spacedel";
		[Column("TotalWordsRead")] public int TotalWordsRead { get; set; } = 0;
	}
}
