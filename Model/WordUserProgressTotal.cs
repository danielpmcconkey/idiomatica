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
    /// <summary>
    /// this class represents a snapshot in time of a language user's total
    /// number of wordUsers in each status category
    /// </summary>
    [Table("WordUserProgressTotal", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    public class WordUserProgressTotal
    {
        #region required data
        [Required] public required Guid Id { get; set; }
        [Required] public required Guid LanguageUserId { get; set; }
        [Required] public required AvailableWordUserStatus Status { get; set; }
        [Required] public required DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        [Required] public required int Total { get; set; }

        #endregion

        public LanguageUser? LanguageUser { get; set; }
    }
}
