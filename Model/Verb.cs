using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("Verb", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    public class Verb
    {
        #region required data
        [Required] public required Guid Id { get; set; }
        [Required] public required Guid LanguageId { get; set; }
        public required Language Language { get; set; }

        [StringLength(2000)]
        [Required] public required string Conjugator { get; set; }

        [StringLength(2000)]
        public required string Infinitive { get; set; }

        #endregion

        

        


        [NotMapped]
        public List<VerbConjugation> VerbConjugations { get; set; } = [];
        public List<WordTranslation> WordTranslations { get; set; } = [];

        [StringLength(2000)]
        public string? DisplayName { get; set; }
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
