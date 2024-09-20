using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Model.Enums;

namespace Model
{

    [Table("WordTranslation", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    public class WordTranslation
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required Guid LanguageToId { get; set; }
        [Required] public required Guid WordId { get; set; }
        [Required] public required Guid VerbId { get; set; }
        [Required] public required string Translation { get; set; }
        [Required] public required AvailablePartOfSpeech PartOfSpeech { get; set; }

        [StringLength(2000)]
        [Required] public required int Ordinal { get; set; } = 0;


        #endregion

        public Language? LanguageTo { get; set; }
        public Word? Word { get; set; }
        public Verb? Verb { get; set; }

       


        



    }
}

