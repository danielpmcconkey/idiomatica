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
    [Table("FlashCard", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    [Index(nameof(WordUserId), IsUnique = true)]
    public class FlashCard
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required Guid WordUserId { get; set; }
        public required WordUser WordUser { get; set; }
        [Required] public required AvailableFlashCardStatus Status { get; set; }

        #endregion


        public List<FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridges 
            { get; set; }  = [];
        public List<FlashCardAttempt> Attempts { get; set; } = [];
        public DateTimeOffset? NextReview {  get; set; }

    }
}
