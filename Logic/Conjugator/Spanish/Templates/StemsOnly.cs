using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    public class StemsOnly : _Ar
    {
        public StemsOnly(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            throw new NotImplementedException("this is a template. do not use. when you do copy it into a real class, make sure to check which class it's inheriting from");
        }


        #region Present

        public override List<VerbConjugationPiece> GetStemPiecesPresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        #endregion

        #region Preterite

        public override List<VerbConjugationPiece> GetStemPiecesPreteriteYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        #endregion

        #region Imperfect
        public override List<VerbConjugationPiece> GetStemPiecesImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        #endregion

        #region Conditional

        public override List<VerbConjugationPiece> GetStemPiecesConditionalYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesConditionalEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        #endregion

        #region Future

        public override List<VerbConjugationPiece> GetStemPiecesFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }

        #endregion

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        #endregion

        #region SubjunctiveImperfect


        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        #endregion

        #region SubjunctiveFuture

        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        #endregion

        #region AffirmativeImperative



        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        #endregion

        #region NegativeImperative

        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = ""
            },new VerbConjugationPiece()
            {
                Ordinal = 220,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = ""
            }];
        }
        #endregion


    }
}

