using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    [Table("UserBreadCrumb", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    public class UserBreadCrumb
    {
        public Guid? UniqueKey { get; set; } 

        #region relationships
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? PageId { get; set; }
        public Page? Page { get; set; }

        #endregion

        public DateTime? ActionDateTime { get; set; }

    }
}
