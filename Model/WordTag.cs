using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [PrimaryKey(nameof(WtWoID), nameof(WtTgID))]
    public class WordTag
    {

        #region relationships
        [Column("WtWoID")] public int WtWoID { get; set; }
        [Column("WtTgID")] public int WtTgID { get; set; }
        #endregion

    }
}
