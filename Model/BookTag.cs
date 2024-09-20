using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    [Table("BookTag", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    [Index(nameof(UserId), nameof(BookId), nameof(Tag), IsUnique = true)]
    public class BookTag
    {
        #region required data

        [Required] public required Guid? Id { get; set; }
        [Required] public required Guid BookId { get; set; }
        [Required] public required Guid UserId { get; set; }
        [Required] public required DateTimeOffset Created { get; set; }

        [StringLength(250)]
        [Required] public required string Tag { get; set; }
        
        #endregion
        public Book? Book { get; set; }
        public User? User { get; set; }
    }
}
