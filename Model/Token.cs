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
    [PrimaryKey(nameof(UniqueKey))]
    /// <summary>
    /// This is used to assist UI in writing sentences on the screen easier
    /// </summary>
    public class Token
    {
        #region required data
        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid WordKey { get; set; }
        public required Word Word { get; set; }
        [Required] public required Guid SentenceKey { get; set; }
        public required Sentence Sentence { get; set; }

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

    }
}
