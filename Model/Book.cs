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
    
    [Table("Book", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    public class Book
    {
        #region required data

        [Required] public required Guid Id
        { get; set; }

        [Required] public required Guid LanguageId { get; set; }
        public required Language Language { get; set; }
        
        [StringLength(250)]
        [Required] public required string Title { get; set; }
        #endregion

        

        public List<Page> Pages { get; set; } = new List<Page>();
        public List<BookStat> BookStats { get; set; } = new List<BookStat>();
        public List<BookUserStat> BookUserStats { get; set; } = new List<BookUserStat>();
        public List<BookUser> BookUsers { get; set; } = new List<BookUser>();
        public List<BookTag> BookTags { get; set; } = new List<BookTag>();

        [StringLength(1000)]
        public string? SourceURI { get; set; }
    }
}
