using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [PrimaryKey(nameof(StKey))]
    public class Setting
    {
        [Column("StKey")] public string StKey { get; set; }


        #region relationships
        public int UserId { get; set; }
        public User User { get; set; }
        #endregion


        [Column("StKeyType")] public string StKeyType { get; set; }
        [Column("StValue")] public string StValue { get; set; }
    }
}
