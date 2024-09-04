using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Model
{

    [Table("WordTranslation", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    public class WordTranslation
    {
        public Guid? UniqueKey { get; set; }


        #region relationships

        public int? LanguageToId { get; set; }
        public Language? LanguageTo { get; set; }
        public int? WordId { get; set; }
        public Word? Word { get; set; }
        public Guid? VerbKey { get; set; }
        public Verb? Verb { get; set; }

        #endregion


        [StringLength(2000)]
        public string? Translation { get; set; }
        public AvailablePartOfSpeech? PartOfSpeech { get; set; }

        public int? Ordinal {  get; set; }

    }
}

