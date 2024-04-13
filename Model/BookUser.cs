using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("BookUser", Schema = "Idioma")]
    public class BookUser
    {
        public int Id { get; set; }

        #region relationships

        public int BookId { get; set; }
        public Book Book { get; set; }
        public int LanguageUserId { get; set; }
        public LanguageUser LanguageUser { get; set; }
        public List<BookUserStat> BookUserStats { get; set; } = new List<BookUserStat>();
        public List<PageUser> PageUsers { get; set; } = new List<PageUser>();

        #endregion

        #region properties

        public bool IsArchived { get; set; } = false;
        public int CurrentPageID { get; set; } = 0;
        public float AudioCurrentPos { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string? AudioBookmarks { get; set; }

        #endregion
    }
}
