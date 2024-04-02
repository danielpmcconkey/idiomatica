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
        public int Id { get; set; }

        #region relationships
        public List<Word> Words { get; set; } = new List<Word>();
        #endregion


        public string Text { get; set; }
        public string Abbreviation { get; set; }
    }
}
