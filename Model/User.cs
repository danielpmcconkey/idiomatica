using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }

        #region relationships
		public List<LanguageUser> LanguageUsers { get; set; }
        public List<UserSetting> UserSettings { get; set; }

        #endregion

        public string Name { get; set; }
    }
}
