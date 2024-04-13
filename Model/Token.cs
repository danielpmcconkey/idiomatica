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
    /// <summary>
    /// This is used to assist UI in writing sentences on the screen easier
    /// </summary>
    public class Token
    {
        public int? Id { get; set; }

        #region relationships

        public int WordId { get; set; }
        public Word Word { get; set; }
        public int SentenceId { get; set; }
        public Sentence Sentence { get; set; }

        #endregion

        /// <summary>
        /// the text only component, regardless of status, includes any punctuation
        /// </summary>
        [StringLength(250)]
        public string Display { get; set; }


        /// <summary>
        /// the order it appears within its sentence
        /// </summary>
        public int Ordinal { get; set; }
    }
}
