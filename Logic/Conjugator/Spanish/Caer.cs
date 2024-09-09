using Model;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    public class Caer : _Er
    {
        public Caer(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            
        }


        #region Present

        public override List<VerbConjugationPiece> GetStemPiecesPresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "o"
            }];
        }
        #endregion

        #region Preterite

        public override List<VerbConjugationPiece> GetStemPiecesPreteriteTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "í"
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
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ó"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "í"
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
                Piece = "í"
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
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eron"
            }];
        }
        #endregion



        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "as"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "áis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
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
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "era"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eras"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "era"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éramos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erais"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eran"
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
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ere"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eres"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ere"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éremos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ereis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eren"
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
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            }];
        }
        
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            }];
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "as"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "áis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ig"
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            }];
        }
        #endregion


    }
}

