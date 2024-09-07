using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("FlashCard", Schema = "Idioma")]
    public class FlashCard
    {
        public Guid? UniqueKey { get; set; }
        //public int? Id { get; set; }

        #region relationships


        //public int? WordUserId { get; set; }
        public Guid? WordUserKey { get; set; }
        public WordUser? WordUser { get; set; }
        public List<FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridges 
            { get; set; }  = new List<FlashCardParagraphTranslationBridge>();
        public AvailableFlashCardStatus? Status { get; set; }
        public List<FlashCardAttempt> Attempts { get; set; } = new List<FlashCardAttempt>();


        #endregion
        public DateTime? NextReview {  get; set; }

    }
}
