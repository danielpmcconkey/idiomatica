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
    public class BookTag
    {
        public int? Id { get; set; }

        #region relationships

        public int? BookId { get; set; }
        public Book? Book { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }

        #endregion

        [StringLength(250)]
        public string? Tag { get; set; }
        public DateTimeOffset? Created { get; set; }
        
        /// <summary>
        /// Tags are pulled in aggregate. If 20 users all set the same tag, it would show 20, even though the DB saves them individually
        /// </summary>
        [NotMapped]
        public int? Count { get; set; }
        
        /// <summary>
        /// IsPersonal is used for aggregate pulls to note whether this tag is one the user pensonally set
        /// </summary>
        [NotMapped]
        public bool? IsPersonal { get; set; }
        public Guid UniqueKey { get; set; } // used so you can insert and then retrieve it; because it's too late to use a GUID as the primary key

    }
}
