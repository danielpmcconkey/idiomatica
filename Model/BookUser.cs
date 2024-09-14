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
    [Table("BookUser", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    [Index(nameof(BookKey), nameof(LanguageUserKey), IsUnique = true)]
    public class BookUser
    {
        #region required data

        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid BookKey { get; set; }
        public required Book Book { get; set; }
        [Required] public required Guid LanguageUserKey { get; set; }
        public required LanguageUser LanguageUser { get; set; }

        #endregion

        
        public List<PageUser> PageUsers { get; set; } = new List<PageUser>();
        public Guid? CurrentPageKey { get; set; } // bookmark to the current page
        public Page? CurrentPage { get; set; } // bookmark to the current page
        public bool? IsArchived { get; set; } = false;

        
    }
}
