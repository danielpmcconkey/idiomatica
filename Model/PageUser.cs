using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("PageUser", Schema = "Idioma")]
    public class PageUser
    {
        public int? Id { get; set; }

        #region relationships
        public int BookUserId { get; set; }
        public BookUser BookUser { get; set; }
        public int PageId { get; set; }
        public Page Page { get; set; }
        
        #endregion

        
        public DateTime? ReadDate { get; set; }

    }
}
