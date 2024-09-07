using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("WordUser", Schema = "Idioma")]
    public class WordUser
    {
        public Guid? UniqueKey { get; set; }

        #region relationships

        public Guid? WordKey { get; set; }
        public Word? Word { get; set; }
        public Guid? LanguageUserKey { get; set; }
        public LanguageUser? LanguageUser { get; set; }
        public FlashCard? FlashCard { get; set; }

        #endregion

        [StringLength(2000)]
        public string? Translation { get; set; }
        public AvailableWordUserStatus? Status { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? StatusChanged { get; set; } = DateTime.Now;

    }
}
