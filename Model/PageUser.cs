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
        public Guid? UniqueKey { get; set; }
        //public int? Id { get; set; }

        #region relationships
        public Guid? BookUserKey { get; set; }
        public BookUser? BookUser { get; set; }
        public Guid? PageKey { get; set; }
        public Page? Page { get; set; }
        
        #endregion

        
        public DateTime? ReadDate { get; set; }
    }
}
