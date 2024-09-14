using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("ParagraphTranslation", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    [Index(nameof(ParagraphKey), nameof(LanguageKey), IsUnique = true)]
    public class ParagraphTranslation
    {
        #region required data

        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid ParagraphKey { get; set; }
        public required Paragraph Paragraph { get; set; }
        [Required] public required string TranslationText { get; set; }
        [Required] public required Guid LanguageKey { get; set; }
        public required Language Language { get; set; }

        #endregion


        
        [StringLength(8000)]
        public List<FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridges { get; set; } = new List<FlashCardParagraphTranslationBridge>();

    }
}
