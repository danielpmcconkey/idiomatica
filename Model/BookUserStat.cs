using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enums;

namespace Model
{
    [Table("BookUserStat", Schema = "Idioma")]
    [PrimaryKey(nameof(BookKey), nameof(LanguageUserKey), nameof(Key))]
    public class BookUserStat
    {
        #region required data

        [Required] public required Guid BookKey { get; set; }
        public required Book Book { get; set; }
        [Required] public required Guid LanguageUserKey { get; set; }
        public required LanguageUser LanguageUser { get; set; }
        [Required] public required AvailableBookUserStat Key { get; set; }

        #endregion


        [StringLength(250)]
        public string? ValueString { get; set; }

        [Column(TypeName ="numeric(10,4)")]
        public decimal? ValueNumeric { get; set; }
    }
}
