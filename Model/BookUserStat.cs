using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("BookUserStat", Schema = "Idioma")]
    [PrimaryKey(nameof(BookId), nameof(LanguageUserId), nameof(Key))]
    public class BookUserStat
    {
        public int? BookId { get; set; }
        public int? LanguageUserId { get; set; }
        public AvailableBookUserStat Key { get; set; }

        #region relationships
        public Book Book { get; set; }
        public LanguageUser LanguageUser { get; set; }
        #endregion
        [StringLength(250)]
        public string? ValueString { get; set; }
        public decimal? ValueNumeric { get; set; }
    }
}
