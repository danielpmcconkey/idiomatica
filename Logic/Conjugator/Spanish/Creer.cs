using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{

    public class Creer : _Er
    {
        public Creer(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            
        }

        internal override List<VerbConjugationPiece> GetCorePiece2()
        {
            // cre -> crey
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core2))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "y";
            return ReplaceRightOfCore(_sourceLanguageInfinitive.Core2, seek);
        }




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
        
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEl()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece()
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
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEllos()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eron"
            }];
        }

        #endregion





        #region SubjunctiveImperfect
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectYo()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "era"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectTu()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eras"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEl()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "era"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectNosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éramos"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectVosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erais"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEllos()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eran"
            }];
        }


        #endregion

        #region SubjunctiveFuture

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureYo()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ere"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureTu()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eres"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEl()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ere"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureNosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éremos"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureVosotros()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ereis"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEllos()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eren"
            }];
        }
        #endregion

    }
}
