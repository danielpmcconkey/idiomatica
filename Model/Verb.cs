using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("Verb", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    public class Verb
    {
        public Guid? UniqueKey { get; set; }


        #region relationships

        public int? LanguageId { get; set; }
        public Language? Language { get; set; }

        [NotMapped]
        public List<VerbConjugation> VerbConjugations { get; set; } = [];
        public List<WordTranslation> WordTranslations { get; set; } = [];

        #endregion
        [StringLength(2000)]
        public string? Conjugator { get; set; }
        [StringLength(2000)]
        public string? DisplayName { get; set; }
        [StringLength(2000)]
        public string? Infinitive { get; set; }
        [StringLength(2000)]
        public string? Core1 { get; set; }
        [StringLength(2000)]
        public string? Core2 { get; set; } // used in English as first person preterite
        [StringLength(2000)]
        public string? Core3 { get; set; } // used in English as third person present
        [StringLength(2000)]
        public string? Core4 { get; set; }
        [StringLength(2000)]
        public string? Gerund { get; set; }
        [StringLength(2000)]
        public string? PastParticiple { get; set; }

    }

    
    
}
