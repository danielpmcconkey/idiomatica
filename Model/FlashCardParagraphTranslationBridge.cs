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
    [Table("FlashCardParagraphTranslationBridge", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    public class FlashCardParagraphTranslationBridge
    {
        #region required data

        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid FlashCardKey { get; set; }
        public required FlashCard FlashCard { get; set; }
        [Required] public required Guid ParagraphTranslationKey { get; set; }
        public required ParagraphTranslation ParagraphTranslation { get; set; }


        #endregion

    }
}
