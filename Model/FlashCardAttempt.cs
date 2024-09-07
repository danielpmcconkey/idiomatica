using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("FlashCardAttempt", Schema = "Idioma")]
    public class FlashCardAttempt
    {
        public Guid? UniqueKey { get; set; }
        #region relationships


        public Guid? FlashCardKey { get; set; }
        public FlashCard? FlashCard { get; set; }
        public AvailableFlashCardAttemptStatus? Status { get; set; }


        #endregion
        public DateTime? AttemptedWhen { get; set; }

    }
}
