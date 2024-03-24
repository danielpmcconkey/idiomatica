using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WordImage
    {
        [Column("WiID")] public int Id { get; set; }

        #region relationships
        #endregion


        [Column("WiWoID")] public int WiWoID { get; set; }
        [Column("WiSource")] public string WiSource { get; set; }
    }
}
