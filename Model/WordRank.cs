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
    [PrimaryKey(nameof(UniqueKey))]
    [Index(nameof(LanguageKey), nameof(WordKey), IsUnique = true)]
    public class WordRank
    {
        #region required data

        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid LanguageKey { get; set; }
        public required Language Language { get; set; }

        [Required] public required Guid WordKey { get; set; }
       public required Word Word { get; set; }

        [Column(TypeName = "numeric(8,2)")]
        [Required] public required decimal DifficultyScore { get; set; }
        [Required] public required int Ordinal {  get; set; }

        #endregion


    }
}
