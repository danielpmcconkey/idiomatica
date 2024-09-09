using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Model.Enums;

namespace Model
{

    [Table("WordTranslation", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    public class WordTranslation
    {
        #region required data

        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid LanguageToKey { get; set; }
        public required Language LanguageTo { get; set; }
        [Required] public required Guid WordKey { get; set; }
        public required Word Word { get; set; }
        [Required] public required Guid VerbKey { get; set; }
        [Required] public required string Translation { get; set; }
        [Required] public required AvailablePartOfSpeech PartOfSpeech { get; set; }

        [StringLength(2000)]
        [Required] public required int Ordinal { get; set; } = 0;


        #endregion


       

        public Verb? Verb { get; set; }

        



    }
}

