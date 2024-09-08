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
        public Guid? UniqueKey { get; set; }


        #region relationships
        
        public Guid? LanguageKey { get; set; }
        public Language? Language { get; set; }
        public List<Token> Tokens { get; set; } = [];
        public List<WordUser> WordUsers { get; set; } = [];
        public List<WordTranslation> WordTranslations { get; set; } = [];
        public WordRank? WordRank { get; set; }

        #endregion


        [StringLength(250)]
        public string? Text { get; set; }
        [StringLength(250)]
        public string? TextLowerCase { get; set; }
        
        [StringLength(250)]
        public string? Romanization { get; set; }
        public int? TokenCount { get; set; } = 0; // todo: understand token vs word in original lute and make a multi-word learning phrase in idiomatica


    }
}
