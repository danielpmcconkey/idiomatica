using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    [Table("User", Schema = "Idioma")]
    public class User
    {
        public int? Id { get; set; }

        #region relationships
        public List<LanguageUser> LanguageUsers { get; set; } = new List<LanguageUser>();
        public List<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
        // not a strict EF Core relationship
        public string? ApplicationUserId { get; set; }

        /// <summary>
        /// the DB link to the primary language of the user; used for UI rendering and for translations
        /// </summary>
        [Column("LanguageCode")]
        public string? Code { get; set; } = "ENG-US";
        /// <summary>
        /// the primary language of the user; used for UI rendering and for translations
        /// </summary>
        public LanguageCode? LanguageCode { get; set; }
        public List<BookTag> BookTags { get; set; } = new List<BookTag>();


        #endregion
        [StringLength(250)]
        public string? Name { get; set; }
    }
}
