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
    [Table("PageUser", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    [Index(nameof(PageId), nameof(BookUserId), IsUnique = true)]
    public class PageUser
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required Guid BookUserId { get; set; }
        [Required] public required Guid PageId { get; set; }
        public required Page Page { get; set; }

        #endregion
        

        
        public BookUser? BookUser { get; set; }
        public DateTimeOffset? ReadDate { get; set; }
    }
}
