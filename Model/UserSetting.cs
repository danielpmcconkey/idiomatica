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
        #region required data
        [Required] public required int Key { get; set; }
        [Required] public required Guid UserKey { get; set; }
        public required User User { get; set; }

        [StringLength(1000)]
        [Required] public required string Value { get; set; }


        #endregion





    }
}
