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
    [PrimaryKey(nameof(Id))]
    [Index(nameof(WordId), nameof(LanguageUserId), IsUnique = true)]
    public class WordUser
    {
        #region required data
        [Required] public required Guid Id { get; set; }
        [Required] public required Guid WordId { get; set; }
        [Required] public required Guid LanguageUserId { get; set; }
        [Required] public required AvailableWordUserStatus Status { get; set; }
        [Required] public required DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        [Required] public required DateTimeOffset StatusChanged { get; set; } = DateTimeOffset.Now;



        #endregion

       

        public Word? Word { get; set; }
        public LanguageUser? LanguageUser { get; set; }
        public FlashCard? FlashCard { get; set; }

       

        [StringLength(2000)]
        public string? Translation { get; set; }

    }
}
