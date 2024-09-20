using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    
    [Table("Token", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    [Index(nameof(SentenceId), nameof(Ordinal), IsUnique = true)]
    /// <summary>
    /// This is used to assist UI in writing sentences on the screen easier
    /// </summary>
    public class Token
    {
        #region required data
        [Required] public required Guid Id { get; set; }
        [Required] public required Guid WordId { get; set; }
        [Required] public required Guid SentenceId { get; set; }

        /// <summary>
        /// the text only component, regardless of status, includes any punctuation
        /// </summary>
        [StringLength(250)]
        [Required] public required string Display { get; set; }

        /// <summary>
        /// the order it appears within its sentence
        /// </summary>
        [Required] public required int Ordinal { get; set; }


        #endregion

        public Word? Word { get; set; }
        public Sentence? Sentence { get; set; }
    }
}
