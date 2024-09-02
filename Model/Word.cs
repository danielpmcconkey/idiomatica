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
    [PrimaryKey(nameof(Id))]
    [Index(nameof(LanguageId), nameof(TextLowerCase), IsUnique = true)]
    public class Word
    {
        public int? Id { get; set; }


        #region relationships
        
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
        public List<Token> Tokens { get; set; } = [];
        public List<WordUser> WordUsers { get; set; } = [];
        public List<WordTranslation> WordTranslations { get; set; } = [];

        #endregion


        [StringLength(250)]
        public string? Text { get; set; }
        [StringLength(250)]
        public string? TextLowerCase { get; set; }
        
        [StringLength(250)]
        public string? Romanization { get; set; }
        public int? TokenCount { get; set; } = 0; // todo: understand token vs word in original lute and make a multi-word learning phrase in idiomatica
        public Guid UniqueKey { get; set; } // used so you can insert and then retrieve it; because it's too late to use a GUID as the primary key


    }
}
