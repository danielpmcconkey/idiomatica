using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    
    [Table("Paragraph", Schema = "Idioma")]
    public class Paragraph
    {
        public int? Id { get; set; }

        #region relationships
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();
        public int PageId { get; set; }
        public Page Page { get; set; }

        // warning, do not auto create the ParagraphTranslations list or you'll
        // break the translation function's ability to look-up if we've already
        // pulled it
        public List<ParagraphTranslation> ParagraphTranslations { get; set; } 
        #endregion

        public int Ordinal { get; set; }
    }
}
