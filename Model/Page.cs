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
        public int? Id { get; set; }

        #region relationships
        public int BookId { get; set; }
        public Book Book { get; set; }
        public List<Paragraph> Paragraphs { get; set; } = new List<Paragraph>();
        public List<PageUser> PageUsers { get; set; } = new List<PageUser>();
        #endregion

        public int Ordinal { get; set; }
        [Column(TypeName = "TEXT")]
        public string OriginalText { get; set; }

    }
}
