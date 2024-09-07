using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    
    
    [Table("UserSetting", Schema = "Idioma")]
    [PrimaryKey(nameof(Key), nameof(UserKey))]
    public class UserSetting
    {

        public int? Key { get; set; }
        public Guid? UserKey { get; set; }


        #region relationships
        public User? User { get; set; }
        #endregion



        [StringLength(1000)]
        public string? Value { get; set; }
    }
}
