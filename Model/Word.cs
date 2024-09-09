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
    
    [Table("Word", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    [Index(nameof(LanguageKey), nameof(TextLowerCase), IsUnique = true)]
    public class Word
    {
        #region required data
        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid LanguageKey { get; set; }
        public required Language Language { get; set; }

        [StringLength(250)]
        [Required] public required string TextLowerCase { get; set; }


        #endregion


        
        
        public List<Token> Tokens { get; set; } = [];
        public List<WordUser> WordUsers { get; set; } = [];
        public List<WordTranslation> WordTranslations { get; set; } = [];
        public WordRank? WordRank { get; set; }

        


        [StringLength(250)]
        public string? Text { get; set; }
        
        [StringLength(250)]
        public string? Romanization { get; set; }

    }
}
