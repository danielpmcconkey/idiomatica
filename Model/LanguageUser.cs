using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    
    [Table("LanguageUser", Schema = "Idioma")]
    public class LanguageUser
    {
        public int? Id { get; set; }

        #region relationships
        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public List<BookUser> BookUsers { get; set; } = new List<BookUser>();
        public List<WordUser> WordUsers { get; set; } = new List<WordUser>();
        public List<BookUserStat> BookUsersStats { get; set; } = new List<BookUserStat>();

        #endregion

        public int? TotalWordsRead { get; set; } = 0;
        public Guid UniqueKey { get; set; } // used so you can insert and then retrieve it; because it's too late to use a GUID as the primary key

    }
}
