using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    //[Table("VerbConjugationPiece", Schema = "Idioma")]
    //[PrimaryKey(nameof(UniqueKey))]
    public class VerbConjugationPiece
    {
        public Guid? UniqueKey { get; set; }


        #region relationships

        public VerbConjugation? VerbConjugation { get; set; }

        #endregion

        public int? Ordinal { get; set; }
        public AvailableVerbConjugationPieceType Type { get; set; }
        [StringLength(2000)]
        public string? Piece { get; set; }
    }
}
