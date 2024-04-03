using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Status")]
    public class Status
    {
        public int Id { get; set; }

        #region relationships
        public List<Word> Words { get; set; } = new List<Word>();
        #endregion


        public string Text { get; set; }
        public string Abbreviation { get; set; }
    }
    public enum AvailableStatuses
    {
        NEW1 = 1,
        NEW2 = 2,
        LEARNING3 = 3,
        LEARNING4 = 4,
        LEARNED = 5,
        IGNORED = 6,
        WELLKNOWN = 7,
        UNKNOWN = 8
    }
}
