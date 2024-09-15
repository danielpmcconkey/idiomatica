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
    [PrimaryKey(nameof(Id))]
    [Index(nameof(ParagraphId), nameof(LanguageId), IsUnique = true)]
    public class ParagraphTranslation
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required Guid ParagraphId { get; set; }
        public required Paragraph Paragraph { get; set; }
        [Required] public required string TranslationText { get; set; }
        [Required] public required Guid LanguageId { get; set; }
        public required Language Language { get; set; }

        #endregion


        
        [StringLength(8000)]
        public List<FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridges { get; set; } = new List<FlashCardParagraphTranslationBridge>();

    }
}
