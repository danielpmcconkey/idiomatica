using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Book")]
    public class Book
    {
        public int? Id { get; set; }

        #region relationships

        public int LanguageUserId { get; set; }
        public LanguageUser LanguageUser { get; set; }
        public List<Page> Pages { get; set; } = new List<Page>();
        public List<BookStat> BookStats { get; set; } = new List<BookStat>();

        #endregion
        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string? SourceURI { get; set; }
        public bool IsArchived { get; set; } = false;
        public int CurrentPageID { get; set; } = 0;
        public int WordCount { get; set; } // todo: get rid of Book.WordCount
        [StringLength(250)]
        public string? AudioFilename { get; set; }
        public float AudioCurrentPos { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string? AudioBookmarks { get; set; }
		public bool IsComplete { get; set; } = false; // todo: get rid of Book.IsComplete
		public int TotalPages { get; set; } = 0;  // todo: get rid of Book.TotalPages
		public int LastPageRead { get; set; } = 0; // todo: get rid of Book.LastPageRead		
	}
	
}
