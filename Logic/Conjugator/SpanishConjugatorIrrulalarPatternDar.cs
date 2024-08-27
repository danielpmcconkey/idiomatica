using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public class SpanishConjugatorIrrulalarPatternDar : SpanishConjugatorArBase
    {
        public SpanishConjugatorIrrulalarPatternDar(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            
        }


        #region Present

        public override List<VerbConjugationPiece> GetStemPiecesPresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "o"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "a"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "is"
            }];
        }
        #endregion

        #region Preterite

        public override List<VerbConjugationPiece> GetStemPiecesPreteriteYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "i"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "i"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ste"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "i"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "o"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "i"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "mos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "i"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "steis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ron"
            }];
        }
        #endregion

        #region Imperfect
        
        #endregion

        #region Conditional

        
        #endregion

        #region Future

        

        #endregion

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            }];
        }
        
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "e"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "is"
            }];
        }
        #endregion

        #region SubjunctiveImperfect


        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ra"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ras"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ra"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ié"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ramos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "rais"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ran"
            }];
        }
        #endregion

        #region SubjunctiveFuture

        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "re"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "res"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "re"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ié"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "remos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "reis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ie"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ren"
            }];
        }
        #endregion

        #region AffirmativeImperative



        
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            }];
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "e"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "is"
            }];
        }
        #endregion


    }
}

