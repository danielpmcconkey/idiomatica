using Azure;
using Microsoft.EntityFrameworkCore;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    
    [Table("Language", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    [Index(nameof(Code), IsUnique = true)]
    public class Language
    {
        #region required data
        [Required] public required Guid UniqueKey { get; set; }

        [StringLength(250)]
        [Required] public required string Name { get; set; }

        [Required] public required AvailableLanguageCode Code { get; set; }

        [StringLength(250)]
        [Required] public required string ParserType { get; set; } = "spacedel";
        [Required] public required bool IsImplementedForLearning { get; set; }
        [Required] public required bool IsImplementedForUI { get; set; }
        [Required] public required bool IsImplementedForTranslation { get; set; }

        #endregion
        


        public List<Book> Books { get; set; } = [];
        public List<Word> Words { get; set; } = [];
        public List<LanguageUser> LanguageUsers { get; set; } = [];
        public List<WordTranslation> WordTranslations { get; set; } = [];
        public List<Verb> Verbs { get; set; } = [];
        public List<WordRank> WordRanks { get; set; } = [];
        public List<ParagraphTranslation> ParagraphTranslations { get; set; } = [];
        //public List<User> Users { get; set; } = [];

    }
}
