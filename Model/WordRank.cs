using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    [Table("WordRank", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    [Index(nameof(LanguageId), nameof(WordId), IsUnique = true)]
    [Index(nameof(LanguageId), nameof(Ordinal), IsUnique = true)]
    public class WordRank
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required Guid LanguageId { get; set; }
        [Required] public required Guid WordId { get; set; }

        [Column(TypeName = "numeric(8,2)")]
        [Required] public required decimal DifficultyScore { get; set; }
        [Required] public required int Ordinal {  get; set; }

        #endregion

        public Language? Language { get; set; }
        public Word? Word { get; set; }

    }
}
