using Azure;
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
    
    [Table("LanguageUser", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    [Index(nameof(LanguageKey), nameof(UserKey), IsUnique = true)]
    public class LanguageUser
    {
        #region required data

        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid LanguageKey { get; set; }
        public required Language Language { get; set; }
        [Required] public required Guid UserKey { get; set; }
        public required User User { get; set; }

        #endregion



       
        public List<BookUser> BookUsers { get; set; } = new List<BookUser>();
        public List<WordUser> WordUsers { get; set; } = new List<WordUser>();
        public List<BookUserStat> BookUsersStats { get; set; } = new List<BookUserStat>();

        

        public int? TotalWordsRead { get; set; }

    }
}
