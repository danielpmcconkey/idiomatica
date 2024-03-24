using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("tags2")]
    public class Tag
    {
        [Column("T2ID")] public int Id { get; set; }

        #region relationships
        public List<BookTag> BookTags { get; set; }
        #endregion


        [Column("T2Text")] public string Text { get; set; }
        [Column("T2Comment")] public string Comment { get; set; } = "";
    }
}
