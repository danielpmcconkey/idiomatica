using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Model
{
    
    [Table("Sentence", Schema = "Idioma")]
    [PrimaryKey(nameof(Id))]
    [Index(nameof(ParagraphId), nameof(Ordinal), IsUnique = true)]
    public class Sentence
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required Guid ParagraphId { get; set; }
        [Required] public required int Ordinal { get; set; }
        [Column(TypeName = "TEXT")]
        [Required] public required string Text { get; set; }

        #endregion

        

        #region relationships
        public Paragraph? Paragraph { get; set; }
        public List<Token> Tokens { get; set; } = new List<Token>();
        #endregion


    }
}
