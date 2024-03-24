using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [PrimaryKey(nameof(BookId))]
    public class BookStat
    {

        #region relationships
        [Column("BkID")] public int BookId { get; set; }
        public Book Book { get; set; }
        #endregion

        [Column("wordcount")] public int? wordcount { get; set; }
        [Column("distinctterms")] public int? distinctterms { get; set; }
        [Column("distinctunknowns")] public int? distinctunknowns { get; set; }
        [Column("unknownpercent")] public int? unknownpercent { get; set; }
    }
}
