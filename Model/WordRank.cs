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
        public Guid? UniqueKey { get; set; }


        #region relationships

        public Guid? LanguageKey { get; set; }
        public Language? Language { get; set; }

        public Guid? WordKey { get; set; }
        public Word? Word { get; set; }

        #endregion

        [Column(TypeName = "numeric(8,2)")]
        public decimal? DifficultyScore { get; set; }

    }
}
