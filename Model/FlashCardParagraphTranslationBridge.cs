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
        public Guid? UniqueKey { get; set; }
        //public int? Id { get; set; }
        public Guid? FlashCardKey { get; set; }
        public FlashCard? FlashCard { get; set; }
        public Guid? ParagraphTranslationKey { get; set; }
        public ParagraphTranslation? ParagraphTranslation { get; set; }

    }
}
