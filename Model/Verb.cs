using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public enum AvailableGrammaticalPerson
    {
        ANY = 0,
        FIRSTPERSON = 1,
        SECONDPERSON = 2,
        THIRDPERSON = 3,
        SECONDPERSON_FORMAL = 4,
    }
    public enum AvailableGrammaticalNumber
    {
        ANY = 0,
        SINGULAR = 1,
        PLURAL = 2,
    }
    public enum AvailableGrammaticalGender
    {
        ANY = 0,
        MASCULINE = 1,
        FEMININE = 2,
        NEUTER = 3,
        ANIMATE = 4,
        INANIMATE = 5,
    }
    public enum AvailableGrammaticalTense
    {
        ANY = 0,
        PAST = 1,
        PRESENT = 2,
        FUTURE = 3,
        NONPAST = 4,
        NONFUTURE = 5,
    }
    public enum AvailableGrammaticalAspect
    {
        ANY = 0,
        PERFECT = 1,
        IMPERFECT = 2,
        CONTINUOUS = 3,
        PROGRESSIVE = 4,
        HABITUAL = 5,
    }
    public enum AvailableGrammaticalMood
    {
        ANY = 0,
        INDICATIVE = 1,
        INTERROGATIVE = 2,
        IMPERATIVE = 3,
        SUBJUNCTIVE = 4,
        INJUNCTIVE = 5,
        OPTATIVE = 6,
        POTENTIAL = 7,
        CONDITIONAL = 8,
        NEGATIVE_IMPERATIVE = 9,
    }
    public enum AvailableGrammaticalVoice
    {
        ANY = 0,
        ACTIVE = 1,
        PASSIVE = 2,
        MIDDLE = 3,
    }
    public enum AvailableVerbConjugationPieceType
    {
        PRONOUN = 0,
        CORE = 1,
        REGULAR = 2,
        IRREGULAR = 3,
    }
    [Table("Verb", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    public class Verb
    {
        public Guid? UniqueKey { get; set; }


        #region relationships

        public int? LanguageId { get; set; }
        public Language? Language { get; set; }

        [NotMapped]
        public List<VerbConjugation> VerbConjugations { get; set; } = [];
        public List<WordTranslation> WordTranslations { get; set; } = [];

        #endregion
        [StringLength(2000)]
        public string? Conjugator { get; set; }
        [StringLength(2000)]
        public string? DisplayName { get; set; }
        [StringLength(2000)]
        public string? Infinitive { get; set; }
        [StringLength(2000)]
        public string? Core1 { get; set; }
        [StringLength(2000)]
        public string? Core2 { get; set; } // used in English as first person preterite
        [StringLength(2000)]
        public string? Core3 { get; set; } // used in English as third person present
        [StringLength(2000)]
        public string? Core4 { get; set; }
        [StringLength(2000)]
        public string? Gerund { get; set; }
        [StringLength(2000)]
        public string? PastParticiple { get; set; }

    }
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
