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
    public class Decir : _Ir
    {
        public Decir(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {


        }
        /*
         * decir is a mess
         * We have roots of dec, dig, dic, dij, and just plain d
         * A lot of the words that would otherwise be dijier... are just dijer...
         * And, for some reason, it's preterite yo and él forms don't have accents
         * 
         * */
        internal List<VerbConjugationPiece> GetCoreDig()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 100,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "d"
                },new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "ig"
                }];
        }
        internal List<VerbConjugationPiece> GetCoreDic()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 100,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "d"
                },new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "i"
                },new VerbConjugationPiece()
                {
                    Ordinal = 120,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "c"
                }];
        }
        internal List<VerbConjugationPiece> GetCoreDij()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 100,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "d"
                },new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "ij"
                }];
        }
        internal List<VerbConjugationPiece> GetCoreD()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "d"
                }];
        }

        #region Present

        public override List<VerbConjugationPiece> GetRootPiecesPresentYo()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentTu()
        {
            return GetCoreDic();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEl()
        {
            return GetCoreDic();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEllos()
        {
            return GetCoreDic();
        }

        #endregion

        #region Preterite
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteYo()
        {
            return GetCoreDij();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "e"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteTu()
        {
            return GetCoreDij();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEl()
        {
            return GetCoreDij();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "o"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteNosotros()
        {
            return GetCoreDij();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteVosotros()
        {
            return GetCoreDij();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEllos()
        {
            return GetCoreDij();
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


        #region Conditional

        public override List<VerbConjugationPiece> GetRootPiecesConditionalYo()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalTu()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalEl()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalNosotros()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalVosotros()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesConditionalEllos()
        {
            return GetCoreD();
        }
        #endregion

        #region Future
        public override List<VerbConjugationPiece> GetRootPiecesFutureYo()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureTu()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureEl()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureNosotros()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureVosotros()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetRootPiecesFutureEllos()
        {
            return GetCoreD();
        }

        #endregion

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentYo()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentTu()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEl()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentNosotros()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentVosotros()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEllos()
        {
            return GetCoreDig();
        }
        #endregion

        #region SubjunctiveImperfect
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectYo()
        {
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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
            return GetCoreDij();
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


        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeTu()
        {
            return GetCoreD();
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "i"
            }];
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUsted()
        {
            return GetCoreDig();
        }

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeNosotros()
        {
            return GetCoreDig();
        }

        

        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUstedes()
        {
            return GetCoreDig();
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeTu()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUsted()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeNosotros()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeVosotros()
        {
            return GetCoreDig();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUstedes()
        {
            return GetCoreDig();
        }
        #endregion


    }
}
