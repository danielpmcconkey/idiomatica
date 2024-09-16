using Microsoft.EntityFrameworkCore;
using Model.Enums;
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
    [PrimaryKey(nameof(Key), nameof(UserId))]
    public class UserSetting
    {
        #region required data
        [Required] public required AvailableUserSetting Key { get; set; }
        [Required] public required Guid UserId { get; set; }

        [StringLength(1000)]
        [Required] public required string Value { get; set; }


        #endregion

        public User? User { get; set; }




    }
}
