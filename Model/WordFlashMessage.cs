using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WordFlashMessage
    {
        [Column("WfID")] public int Id { get; set; }

        #region relationships
        #endregion


        [Column("WfWoID")] public int WfWoID { get; set; }
        [Column("WfMessage")] public string WfMessage { get; set; }
    }
}
