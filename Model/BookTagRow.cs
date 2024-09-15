using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// BookTagRow exists so we can view tags at the aggregate level. If 7
    /// people add the same tag, we'd show it once with a count of 7. The
    /// IsPersonal flag allows us to know if the current user was one of those
    /// 7
    /// </summary>
    [NotMapped]
    public record BookTagRow
    {
        public Guid? BookId { get; set; }
        public Book? Book { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public DateTimeOffset? Created { get; set; }
        public string? Tag { get; set; }
        public int? Count { get; set; }
        public bool? IsPersonal { get; set; }
    }

}
