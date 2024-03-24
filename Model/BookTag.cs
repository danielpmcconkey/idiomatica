using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [PrimaryKey(nameof(BookId), nameof(TagId))]
    public class BookTag
    {


        #region relationships
        [Column("BtBkID")] public int BookId { get; set; }
        public Book Book { get; set; }
        [Column("BtT2ID")] public int TagId { get; set; }
        public Tag Tag { get; set; }
        #endregion

    }
}
