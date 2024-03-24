using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Status
    {
        [Column("StID")] public int Id { get; set; }

        #region relationships
        public List<Word> Words { get; set; } = new List<Word>();
        #endregion


        [Column("StText")] public string Text { get; set; }
        [Column("StAbbreviation")] public string Abbreviation { get; set; }
    }
}
