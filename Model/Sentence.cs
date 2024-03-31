using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Model
{
    public class Sentence
    {
        [Column("SeID")] public int Id { get; set; }

        #region relationships
        [Column("SeTxID")] public int PageId { get; set; }
        public Page Text { get; set; }
        #endregion

        [Column("SeOrder")] public int SeOrder { get; set; }
        [Column("SeText")] public string? SentenceText { get; set; }
    }
}
