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
        //public int? Id { get; set; }
        public Guid? UniqueKey { get; set; }

        #region relationships
        public List<Book> Books { get; set; } = [];
        public List<Word> Words { get; set; } = [];
        public List<LanguageUser> LanguageUsers { get; set; } = [];
        [StringLength(25)]
        [Column("LanguageCode")]
        public string? Code { get; set; }
        public LanguageCode? LanguageCode { get; set; }
        public List<WordTranslation> WordTranslations { get; set; } = [];
        public List<Verb> Verbs { get; set; } = [];
        public List<WordRank> WordRanks { get; set; } = [];

        #endregion

        [StringLength(250)]
        public string? Name { get; set; }
        [StringLength(250)]
        public string? ParserType { get; set; } = "spacedel";
        
    }
}
