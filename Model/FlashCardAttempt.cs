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
    [PrimaryKey(nameof(UniqueKey))]
    public class FlashCardAttempt
    {
        #region required data
        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid FlashCardKey { get; set; }
        public required FlashCard FlashCard { get; set; }
        [Required] public required AvailableFlashCardAttemptStatus Status { get; set; }
        [Required] public required DateTime AttemptedWhen { get; set; }



        #endregion

    }
}
