using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Model
{
	[Table("texts")]
    public class Page
    {
        [Column("TxID")] public int Id { get; set; }

        #region relationships
        [Column("TxBkID")] public int BookId { get; set; }
        public Book Book { get; set; }
        public List<Sentence> Sentences { get; set; } = new List<Sentence>(); 
        #endregion

        [Column("TxOrder")] public int Order { get; set; }
        [Column("TxText")] public string OriginalText { get; set; }
        [Column("TxReadDate")] public DateTime? ReadDate { get; set; }

    }
}
