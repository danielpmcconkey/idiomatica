using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public class SpanishConjugatorIrrulalarPatternEstar: SpanishConjugatorIrrulalarPatternAndar
    {
        public SpanishConjugatorIrrulalarPatternEstar(
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
        public override List<VerbConjugationPiece> GetStemPiecesPresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "á"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "s"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "á"
            }];
        }
       public override List<VerbConjugationPiece> GetStemPiecesPresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "á"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "n"
            }];
        }
        #endregion

        #region Preterite

        
        #endregion

        #region Imperfect
        // imperfet are all regular
        #endregion


        #region Conditional

        // conditional are all regular
        #endregion


        #region Future

        // future are all regular

        #endregion


        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "s"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "n"
            }];
        }
        #endregion

        #region SubjunctiveImperfect

        
        #endregion

        #region SubjunctiveFuture

        #endregion

        #region AffirmativeImperative


        
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "á"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "n"
            }];
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            },new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "s"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "n"
            }];
        }
        #endregion


    }
}
