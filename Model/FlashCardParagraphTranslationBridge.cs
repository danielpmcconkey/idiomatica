﻿using Azure;
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
    [PrimaryKey(nameof(Id))]
    [Index(nameof(ParagraphTranslationId), nameof(FlashCardId), IsUnique = true)]
    public class FlashCardParagraphTranslationBridge
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required Guid FlashCardId { get; set; }
        public required FlashCard FlashCard { get; set; }
        [Required] public required Guid ParagraphTranslationId { get; set; }
        public required ParagraphTranslation ParagraphTranslation { get; set; }


        #endregion

    }
}
