using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Model
{
    
    [Table("Sentence", Schema = "Idioma")]
    public class Sentence
    {
        public int? Id { get; set; }

        #region relationships
        public int ParagraphId { get; set; }
        public Paragraph Paragraph { get; set; }
        public List<Token> Tokens { get; set; } = new List<Token>();
        #endregion

        public int Ordinal { get; set; }
        [Column(TypeName = "TEXT")]
        public string? Text { get; set; }
    }
}
