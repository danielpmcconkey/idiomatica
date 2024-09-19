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
    [PrimaryKey(nameof(Id))]
    public class UserBreadCrumb
    {
        #region required data
        [Required] public required Guid Id { get; set; }
        [Required] public required Guid UserId { get; set; }
        [Required] public required Guid PageId { get; set; }
        [Required] public required DateTimeOffset ActionDateTime { get; set; }



        #endregion

        public User? User { get; set; }
        public Page? Page { get; set; }
        


    }
}
