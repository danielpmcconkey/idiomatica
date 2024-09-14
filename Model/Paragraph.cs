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
    
    [Table("Paragraph", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    public class Paragraph
    {
        #region required data

        [Required] public required Guid UniqueKey { get; set; }
        [Required] public required Guid PageKey { get; set; }
        public required Page Page { get; set; }
        [Required] public required int Ordinal { get; set; }

        #endregion


        #region relationships
        public List<Sentence> Sentences { get; set; } = [];

        // warning, do not auto create the ParagraphTranslations list or you'll
        // break the translation function's ability to look-up if we've already
        // pulled it
        public List<ParagraphTranslation> ParagraphTranslations { get; set; } = [];
        #endregion


    }
}
