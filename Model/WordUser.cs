using Microsoft.EntityFrameworkCore;
using Model.Enums;
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
    [PrimaryKey(nameof(UniqueKey))]
    public class WordUser
    {
        #region required data
        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid WordKey { get; set; }
        public required Word Word { get; set; }
        [Required] public required Guid LanguageUserKey { get; set; }
        public required LanguageUser LanguageUser { get; set; }
        [Required] public required AvailableWordUserStatus Status { get; set; }
        [Required] public required DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        [Required] public required DateTimeOffset StatusChanged { get; set; } = DateTimeOffset.Now;



        #endregion

       

        public FlashCard? FlashCard { get; set; }

       

        [StringLength(2000)]
        public string? Translation { get; set; }

    }
}
