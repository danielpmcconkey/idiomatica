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

    [Table("User", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    public class User
    {
        #region required data
        [Required] public required Guid UniqueKey { get; set; }

        // not a strict EF Core relationship
        [Required] public required string ApplicationUserId { get; set; }

        [StringLength(250)]
        [Required] public required string Name { get; set; }

        #endregion

        
        public List<LanguageUser> LanguageUsers { get; set; } = new List<LanguageUser>();
        public List<UserSetting> UserSettings { get; set; } = new List<UserSetting>();
        public List<BookTag> BookTags { get; set; } = new List<BookTag>();
        public List<UserBreadCrumb>? UserBreadCrumbs { get; set; }



       

    }
}
