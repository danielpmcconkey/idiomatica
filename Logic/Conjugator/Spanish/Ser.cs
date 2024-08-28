using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{

    public class Ser : _Er
    {
        public Ser(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            
        }

        internal List<VerbConjugationPiece> GetCorePieceEr()
        {
            // s -> er
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core2))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            return [new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "er"
                }];
        }
        internal List<VerbConjugationPiece> GetCorePieceSo()
        {
            // s -> so
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core3))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            return [new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.REGULAR,
                    Piece = "s"
                },new VerbConjugationPiece()
                {
                    Ordinal = 120,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "o"
                }];
        }
        internal List<VerbConjugationPiece> GetCorePieceFu()
        {
            // s -> so
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core4))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            return [new VerbConjugationPiece()
                {
                    Ordinal = 120,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "fu"
                }];
        }
        internal List<VerbConjugationPiece> GetCorePieceSe()
        {
            // s -> se
           return [new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.REGULAR,
                    Piece = "s"
                },new VerbConjugationPiece()
                {
                    Ordinal = 120,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "e"
                }];
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
        public override List<VerbConjugationPiece> GetRootPiecesPresentTu()
        {
            return GetCorePieceEr();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEl()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "es"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentNosotros()
        {
            return GetCorePieceSo();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "mos"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentVosotros()
        {
            return GetCorePieceSo();
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
        public override List<VerbConjugationPiece> GetRootPiecesPresentEllos()
        {
            return GetCorePieceSo();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "n"
            }];
        }
        #endregion

        #region Preterite

        public override List<VerbConjugationPiece> GetRootPiecesPreteriteYo()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "i"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteTu()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEl()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "e"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteNosotros()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteVosotros()
        {
            return GetCorePieceFu();
        }public override List<VerbConjugationPiece> GetRootPiecesPreteriteEllos()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eron"
            }];
        }
        #endregion

        #region Imperfect

        public override List<VerbConjugationPiece> GetRootPiecesImperfectYo()
        {
            return GetCorePieceEr();
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectTu()
        {
            return GetCorePieceEr();
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "as"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectEl()
        {
            return GetCorePieceEr();
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ér"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectVosotros()
        {
            return GetCorePieceEr();
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ais"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectEllos()
        {
            return GetCorePieceEr();
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            }];
        }
        #endregion

        

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentYo()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentTu()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEl()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentNosotros()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentVosotros()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEllos()
        {
            return GetCorePieceSe();
        }
        #endregion

        #region SubjunctiveImperfect

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectYo()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "era"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectTu()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eras"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEl()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "era"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectNosotros()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éramos"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectVosotros()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erais"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEllos()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eran"
            }];
        }
        #endregion

        #region SubjunctiveFuture

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureYo()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ere"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureTu()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eres"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEl()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ere"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureNosotros()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éremos"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureVosotros()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ereis"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEllos()
        {
            return GetCorePieceFu();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eren"
            }];
        }
        #endregion

        #region AffirmativeImperative


        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUsted()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeNosotros()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUstedes()
        {
            return GetCorePieceSe();
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeTu()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUsted()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeNosotros()
        {
            return GetCorePieceSe();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeVosotros()
        {
            return GetCorePieceSe(); ;
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUstedes()
        {
            return GetCorePieceSe();
        }
        #endregion



    }
}
