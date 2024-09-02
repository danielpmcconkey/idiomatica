using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{

    public class Ver : _Er
    {
        public Ver(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            
        }

        internal override List<VerbConjugationPiece> GetCorePiece2()
        {
            // v -> ve
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core2))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "e";
            return ReplaceRightOfCore(_sourceLanguageInfinitive.Core2, seek);
        }
        

        #region Present

        public override List<VerbConjugationPiece> GetRootPiecesPresentYo()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentVosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
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
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "i"
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
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "o"
            }];
        }

        #endregion

        #region Imperfect
        public override List<VerbConjugationPiece> GetRootPiecesImperfectYo()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectTu()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectEl()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectNosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectVosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectEllos()
        {
            return GetCorePiece2();
        }

        #endregion

        

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentYo()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentTu()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEl()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentNosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentVosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEllos()
        {
            return GetCorePiece2();
        }
        #endregion

        

        #region AffirmativeImperative


        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUsted()
        {
            return GetCorePiece2();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeNosotros()
        {
            return GetCorePiece2();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUstedes()
        {
            return GetCorePiece2();
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeTu()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUsted()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeNosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeVosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUstedes()
        {
            return GetCorePiece2();
        }
        #endregion



    }
}
