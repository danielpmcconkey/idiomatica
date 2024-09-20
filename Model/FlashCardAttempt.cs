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
    [Table("FlashCardAttempt", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    public class FlashCardAttempt
    {
        #region required data
        [Required] public required Guid Id { get; set; }
        [Required] public required Guid FlashCardId { get; set; }
        [Required] public required AvailableFlashCardAttemptStatus Status { get; set; }
        [Required] public required DateTimeOffset AttemptedWhen { get; set; }



        #endregion
        public FlashCard? FlashCard { get; set; }

    }
}
