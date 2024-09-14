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
        #region required data
        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid UserKey { get; set; }
        public required User User { get; set; }
        [Required] public required Guid PageKey { get; set; }
        public required Page Page { get; set; }
        [Required] public required DateTimeOffset ActionDateTime { get; set; }



        #endregion

        


    }
}
