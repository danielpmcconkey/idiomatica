using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{

    public class Oir : _Ir
    {
        public Oir(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            
        }

        internal override List<VerbConjugationPiece> GetCorePiece2()
        {
            // oig
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core2))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "ig";
            return ReplaceRightOfCore(_sourceLanguageInfinitive.Core2, seek);
        }
        internal override List<VerbConjugationPiece> GetCorePiece3()
        {
            // oy
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core3))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "y";
            return ReplaceRightOfCore(_sourceLanguageInfinitive.Core3, seek);
        }

        #region Present

        public override List<VerbConjugationPiece> GetRootPiecesPresentYo()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentTu()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEl()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentNosotros()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.IRREGULAR, Piece = "í"
            },new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "mos"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEllos()
        {
            return GetCorePiece3();
        }

        #endregion

        #region Preterite

        public override List<VerbConjugationPiece> GetRootPiecesPreteriteTu()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteTu()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.IRREGULAR, Piece = "í"
            },new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "ste"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEl()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "ó"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteNosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteNosotros()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.IRREGULAR, Piece = "í"
            },new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "mos"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteVosotros()
        {
            return GetCorePiece();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteVosotros()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.IRREGULAR, Piece = "í"
            },new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "steis"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEllos()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEllos()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "eron"
            }];
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


        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectYo()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "era"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectTu()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "eras"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEl()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "era"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectNosotros()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "éramos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectVosotros()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "erais"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEllos()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "eran"
            }];
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


        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureYo()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "ere"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureTu()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "eres"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEl()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "ere"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureNosotros()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "éremos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureVosotros()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "ereis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEllos()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "eren"
            }];
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
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeVosotros()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.IRREGULAR, Piece = "í"
            },new VerbConjugationPiece() {
                Ordinal = 200, Type = AvailableVerbConjugationPieceType.REGULAR, Piece = "d"
            }];
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
