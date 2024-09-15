using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enums;

namespace Model
{

    //[Table("VerbConjugationPiece", Schema = "Idioma")]
    //[PrimaryKey(nameof(Id))]
    public class VerbConjugationPiece
    {
        public Guid? Id { get; set; }


        #region relationships

        public VerbConjugation? VerbConjugation { get; set; }

        #endregion

        public int? Ordinal { get; set; }
        public AvailableVerbConjugationPieceType Type { get; set; }
        [StringLength(2000)]
        public string? Piece { get; set; }
    }
}
