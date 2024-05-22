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
    public class ParagraphTranslation
    {
        public int Id {  get; set; }
        public int ParagraphId { get; set; }
        public Paragraph Paragraph { get; set; }
        [Column("LanguageCode")]
        public string Code { get; set; }
        public LanguageCode LanguageCode { get; set; }
        [StringLength(8000)]
        public string TranslationText { get; set; }
        public List<FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridges { get; set; }
    }
}
