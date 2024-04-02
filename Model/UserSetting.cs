using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [PrimaryKey(nameof(Key), nameof(UserId))]
    public class UserSetting
    {
        public string Key { get; set; }


        #region relationships
        public int UserId { get; set; }
        public User User { get; set; }
        #endregion


        public string KeyType { get; set; }
        public string Value { get; set; }
    }
}
