using Logic.Telemetry;
using Model;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    public class Jugar : _Car
    {
        public Jugar(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }

        internal override List<VerbConjugationPiece> GetCorePiece2()
        {
            // jug -> juegu
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core2))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "egu";
            return ReplaceRightOfCore(_sourceLanguageInfinitive.Core2, seek);
        }
        internal override List<VerbConjugationPiece> GetCorePiece3()
        {
            // jug -> jueg
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core3))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "e";
            return ReplaceMiddleOfCore(_sourceLanguageInfinitive.Core3, seek);
        }
        internal override List<VerbConjugationPiece> GetCorePiece4()
        {
            // jug -> jugu
            // can't use the standard ReplaceRightOfCore function because that
            // would find the first u and return something weird
            return [new VerbConjugationPiece()
                {
                    Ordinal = 100,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "jug"
                },new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "u"
                }];
        }

        #region Present

        public override List<VerbConjugationPiece> GetRootPiecesPresentYo()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentTu()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEl()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEllos()
        {
            return GetCorePiece3();
        }

        #endregion

        #region Preterite
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteYo()
        {
            return GetCorePiece4();
        }
        #endregion

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentNosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentVosotros()
        {
            return GetCorePiece4();
        }
        #endregion

        #region AffirmativeImperative


        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeTu()
        {
            return GetCorePiece3();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUsted()
        {
            return GetCorePiece2();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeNosotros()
        {
            return GetCorePiece4();
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
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeVosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUstedes()
        {
            return GetCorePiece2();
        }
        #endregion


    }
}
