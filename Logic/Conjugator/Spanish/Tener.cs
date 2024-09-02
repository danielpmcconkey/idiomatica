using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    public class Tener : _Er
    {
        public Tener(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }

        internal override List<VerbConjugationPiece> GetCorePiece2()
        {
            // tien
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
            // tuv
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core3))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "uv";
            return ReplaceRightOfCore(_sourceLanguageInfinitive.Core3, seek);
        }
        internal override List<VerbConjugationPiece> GetCorePiece4()
        {
            // teng
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core4))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "g";
            return ReplaceRightOfCore(_sourceLanguageInfinitive.Core4, seek);
        }


        #region Present

        public override List<VerbConjugationPiece> GetRootPiecesPresentYo()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentTu()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEl()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEllos()
        {
            return GetCorePiece2();
        }


        #endregion

        #region Preterite
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteYo()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteYo()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "e"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteTu()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEl()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "o"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteNosotros()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteVosotros()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEllos()
        {
            return GetCorePiece3();
        }

        #endregion

        #region Conditional

        public override List<VerbConjugationPiece> GetStemPiecesConditionalYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ía"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ías"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ía"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "íamos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "íais"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ían"
            }];
        }
        #endregion

        #region Future

        public override List<VerbConjugationPiece> GetStemPiecesFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "é"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ás"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "á"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "emos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "án"
            }];
        }

        #endregion

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentYo()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentTu()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEl()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentNosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentVosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEllos()
        {
            return GetCorePiece4();
        }
        #endregion

        #region SubjunctiveImperfect
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectYo()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectTu()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEl()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectNosotros()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectVosotros()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEllos()
        {
            return GetCorePiece3();
        }


        #endregion

        #region SubjunctiveFuture

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureYo()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureTu()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEl()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureNosotros()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureVosotros()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEllos()
        {
            return GetCorePiece3();
        }
        #endregion

        #region AffirmativeImperative


        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeTu()
        {
            return [];
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUsted()
        {
            return GetCorePiece4();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeNosotros()
        {
            return GetCorePiece4();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUstedes()
        {
            return GetCorePiece4();
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeTu()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUsted()
        {
            return GetCorePiece4();
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
            return GetCorePiece4();
        }
        #endregion



    }
}
