using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Model
{
	
    [Table("Page", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    [Index(nameof(BookId), nameof(Ordinal), IsUnique = true)]
    public class Page
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required Guid BookId { get; set; }
        public required Book Book { get; set; }
        [Required] public required int Ordinal { get; set; }
        [Column(TypeName = "TEXT")]
        [Required] public required string OriginalText { get; set; }

        #endregion
        

        
        public List<Paragraph> Paragraphs { get; set; } = [];
        public List<PageUser> PageUsers { get; set; } = [];
        public List<UserBreadCrumb>? UserBreadCrumbs { get; set; } = [];
        public List<BookUser>? BookUsersBookMarks { get; set; } = [];
        


    }
}
