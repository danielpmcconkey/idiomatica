using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    //[Table("VerbConjugation", Schema = "Idioma")]
    //[PrimaryKey(nameof(UniqueKey))]
    public class VerbConjugation
    {
        public Guid? UniqueKey { get; set; }


        #region relationships

        public Verb? Verb { get; set; }
        public List<VerbConjugationPiece> Pieces { get; set; } = [];
        public int? WordId { get; set; }
        public Word? Word { get; set; }

        #endregion

        public AvailableGrammaticalPerson? Person { get; set; }
        public AvailableGrammaticalNumber? Number { get; set; }
        public AvailableGrammaticalGender? Gender { get; set; }
        public AvailableGrammaticalTense? Tense { get; set; }
        public AvailableGrammaticalAspect? Aspect { get; set; }
        public AvailableGrammaticalMood? Mood { get; set; }
        public AvailableGrammaticalVoice Voice { get; set; }
        [StringLength(2000)]
        public string? Translation { get; set; }

        public override string ToString()
        {
            return string
                .Join("",
                    Pieces
                        .Where(x => x.Type != AvailableVerbConjugationPieceType.PRONOUN)
                        .Select(x => x.Piece));
        }

    }
}
