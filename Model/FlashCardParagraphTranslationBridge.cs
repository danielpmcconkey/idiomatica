using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("FlashCardParagraphTranslationBridge", Schema = "Idioma")]
    public class FlashCardParagraphTranslationBridge
    {
        public int Id { get; set; }
        public int FlashCardId { get; set; }
        public FlashCard FlashCard { get; set; }
        public int ParagraphTranslationId { get; set; }
        public ParagraphTranslation ParagraphTranslation { get; set; }
    }
}
