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
    [Table("BookStat", Schema = "Idioma")]
    [PrimaryKey(nameof(BookId), nameof(Key))]
    public class BookStat
    {
        #region required data

        [Required] public required Guid BookId { get; set; }
        public required Book Book { get; set; }
        [Required] public required AvailableBookStat Key { get; set; }


        #endregion
        

        [StringLength(250)]
        public required string Value { get; set; }
    }
}
