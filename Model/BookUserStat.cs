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
    [PrimaryKey(nameof(BookUserId), nameof(Key))]
    public class BookUserStat
    {
        public int? BookUserId { get; set; }
        public AvailableBookUserStat Key { get; set; }

        #region relationships
        public BookUser BookUser { get; set; }
        #endregion
        [StringLength(250)]
        public string Value { get; set; }
    }
}
