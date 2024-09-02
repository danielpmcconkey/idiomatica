using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{

    public class RootsOnly : _Ir
    {
        public RootsOnly(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            throw new NotImplementedException("this is a template. do not use. when you do copy it into a real class, make sure to check which class it's inheriting from");
        }

        internal override List<VerbConjugationPiece> GetCorePiece2()
        {
            // ven -> vien
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core2))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "ie";
            return ReplaceMiddleOfCore(_sourceLanguageInfinitive.Core2, seek);
        }
        internal override List<VerbConjugationPiece> GetCorePiece3()
        {
            // ven -> vin
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core3))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "i";
            return ReplaceMiddleOfCore(_sourceLanguageInfinitive.Core3, seek);
        }

        #region Present

        public override List<VerbConjugationPiece> GetRootPiecesPresentYo()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEl()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEllos()
        {
            return GetCorePiece();
        }

        #endregion

        #region Preterite
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteYo()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEl()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEllos()
        {
            return GetCorePiece();
        }

        #endregion

        #region Imperfect
        public override List<VerbConjugationPiece> GetRootPiecesImperfectYo()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectEl()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectEllos()
        {
            return GetCorePiece();
        }

        #endregion

        #region Conditional

        public override List<VerbConjugationPiece> GetRootPiecesConditionalYo()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalEl()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalEllos()
        {
            return GetCorePiece();
        }
        #endregion

        #region Future
        public override List<VerbConjugationPiece> GetRootPiecesFutureYo()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureEl()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureEllos()
        {
            return GetCorePiece();
        }

        #endregion

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentYo()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEl()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEllos()
        {
            return GetCorePiece();
        }
        #endregion

        #region SubjunctiveImperfect
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectYo()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEl()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEllos()
        {
            return GetCorePiece();
        }


        #endregion

        #region SubjunctiveFuture

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureYo()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEl()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEllos()
        {
            return GetCorePiece();
        }
        #endregion

        #region AffirmativeImperative


        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeTu()
        {
            return GetCorePiece();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUsted()
        {
            return GetCorePiece();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeNosotros()
        {
            return GetCorePiece();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeVosotros()
        {
            return GetCorePiece();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUstedes()
        {
            return GetCorePiece();
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUsted()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUstedes()
        {
            return GetCorePiece();
        }
        #endregion



    }
}
