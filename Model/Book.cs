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
    public class Book
    {
        public int? Id { get; set; }

        #region relationships

        public int? LanguageId { get; set; }
        public Language? Language { get; set; }
        public List<Page> Pages { get; set; } = new List<Page>();
        public List<BookStat> BookStats { get; set; } = new List<BookStat>();
        public List<BookUserStat> BookUserStats { get; set; } = new List<BookUserStat>();
        public List<BookUser> BookUsers { get; set; } = new List<BookUser>();
        public List<BookTag> BookTags { get; set; } = new List<BookTag>();

        #endregion


        [StringLength(250)]
        public string? Title { get; set; }
        [StringLength(1000)]
        public string? SourceURI { get; set; }
        
        //public int WordCount { get; set; } // todo: get rid of Book.WordCount
        [StringLength(250)]
        public string? AudioFilename { get; set; }

        public Guid UniqueKey { get; set; } // used so you can insert and then retrieve it; because it's too late to use a GUID as the primary key

    }
}
