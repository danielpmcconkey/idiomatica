using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Model
{
	
    [Table("Page", Schema = "Idioma")]
    public class Page
    {
        public Guid? UniqueKey { get; set; } // used so you can insert and then retrieve it; because it's too late to use a GUID as the primary key
        //public int? Id { get; set; }

        #region relationships
        public Guid? BookKey { get; set; }
        public Book? Book { get; set; }
        public List<Paragraph> Paragraphs { get; set; } = new List<Paragraph>();
        public List<PageUser> PageUsers { get; set; } = new List<PageUser>();
        public List<UserBreadCrumb>? UserBreadCrumbs { get; set; }
        public List<BookUser>? BookUsersBookMarks { get; set; }
        #endregion

        public int? Ordinal { get; set; }
        [Column(TypeName = "TEXT")]
        public string? OriginalText { get; set; }

    }
}
