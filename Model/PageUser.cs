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
    [PrimaryKey(nameof(UniqueKey))]
    public class PageUser
    {
        #region required data

        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid BookUserKey { get; set; }
        public required BookUser BookUser { get; set; }
        [Required] public required Guid PageKey { get; set; }
        public required Page Page { get; set; }

        #endregion
        

        
        public DateTimeOffset? ReadDate { get; set; }
    }
}
