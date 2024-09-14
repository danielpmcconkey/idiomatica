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
    [PrimaryKey(nameof(UniqueKey))]
    public class BookTag
    {
        #region required data

        [Required] public required Guid? UniqueKey { get; set; }
        [Required] public required Guid BookKey { get; set; }
        public required Book Book { get; set; }
        [Required] public required Guid UserKey { get; set; }
        public required User User { get; set; }
        [Required] public required DateTimeOffset Created { get; set; }

        [StringLength(250)]
        [Required] public required string Tag { get; set; }
        
        #endregion
    }
}
