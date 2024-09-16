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
    [PrimaryKey(nameof(Id))]
    [Index(nameof(LanguageId), nameof(UserId), IsUnique = true)]
    public class LanguageUser
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required Guid LanguageId { get; set; }
        [Required] public required Guid UserId { get; set; }

        #endregion



       
        public Language? Language { get; set; }
        public User? User { get; set; }
        public List<BookUser> BookUsers { get; set; } = new List<BookUser>();
        public List<WordUser> WordUsers { get; set; } = new List<WordUser>();
        public List<BookUserStat> BookUsersStats { get; set; } = new List<BookUserStat>();

        

        public int? TotalWordsRead { get; set; }

    }
}
