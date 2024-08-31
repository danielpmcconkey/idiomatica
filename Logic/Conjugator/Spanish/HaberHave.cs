using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    /// <summary>
    /// this is used to represent the version of haber that means "to have". 
    /// It's conjugation is different from haber "there is"
    /// </summary>
    public class HaberHave : _Er
    {
        public HaberHave(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            
        }

        internal override List<VerbConjugationPiece> GetCorePiece2()
        {
            // he
            return [new VerbConjugationPiece()
                {
                    Ordinal = 100,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "h"
                },new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "e"
                }];
        }
        internal override List<VerbConjugationPiece> GetCorePiece3()
        {
            // ha
            return [new VerbConjugationPiece()
                {
                    Ordinal = 100,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "ha"
                }];
        }
        internal override List<VerbConjugationPiece> GetCorePiece4()
        {
            // hub
            return [new VerbConjugationPiece()
                {
                    Ordinal = 100,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "h"
                },new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "u"
                }, new VerbConjugationPiece()
                {
                    Ordinal = 120,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "b"
                }];
        }
        internal List<VerbConjugationPiece> GetCorePiece5()
        {
            // hay
            return [new VerbConjugationPiece()
                {
                    Ordinal = 100,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "ha"
                },new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "y"
                }];
        }
        


        #region Present

        public override List<VerbConjugationPiece> GetRootPiecesPresentYo()
        {
            return GetCorePiece2();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentYo()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentTu()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "s"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEl()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEl()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentNosotros()
        {
            return GetCorePiece2();
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
        public override List<VerbConjugationPiece> GetRootPiecesPresentEllos()
        {
            return GetCorePiece3();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "n"
            }];
        }
        #endregion

        #region Preterite

        public override List<VerbConjugationPiece> GetRootPiecesPreteriteYo()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "e"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteTu()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEl()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "o"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteNosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteVosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEllos()
        {
            return GetCorePiece4();
        }
        #endregion        

        #region Conditional
        public override List<VerbConjugationPiece> GetStemPiecesConditionalYo()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "ía"
                }];
        }
        
        public override List<VerbConjugationPiece> GetStemPiecesConditionalTu()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "ías"
                }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalEl()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "ía"
                }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalNosotros()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "íamos"
                }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalVosotros()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "ías"
                }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalEllos()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "ían"
                }];
        }
        #endregion

        #region Future

        public override List<VerbConjugationPiece> GetStemPiecesFutureYo()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "é"
                }];
        }

        public override List<VerbConjugationPiece> GetStemPiecesFutureTu()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "ás"
                }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureEl()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "á"
                }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureNosotros()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "emos"
                }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureVosotros()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "éis"
                }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureEllos()
        {
            return [new VerbConjugationPiece()
                {
                    Ordinal = 210,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = "r"
                    }, new VerbConjugationPiece()
                {
                    Ordinal = 220,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = "rán"
                }];
        }
        #endregion

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentYo()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentTu()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEl()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentNosotros()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentVosotros()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEllos()
        {
            return GetCorePiece5();
        }
        #endregion

        #region SubjunctiveImperfect

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectYo()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectTu()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEl()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectNosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectVosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEllos()
        {
            return GetCorePiece4();
        }
        #endregion

        #region SubjunctiveFuture

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureYo()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureTu()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEl()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureNosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureVosotros()
        {
            return GetCorePiece4();
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEllos()
        {
            return GetCorePiece4();
        }
        #endregion

        #region AffirmativeImperative


        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = "h"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "e"
            }];
        }
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUsted()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeNosotros()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUstedes()
        {
            return GetCorePiece5();
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeTu()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUsted()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeNosotros()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeVosotros()
        {
            return GetCorePiece5();
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUstedes()
        {
            return GetCorePiece5();
        }
        #endregion



    }
}
