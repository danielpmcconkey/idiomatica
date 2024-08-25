using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public class SpanishConjugatorIrrulalarPatternEstar: SpanishConjugatorArBase
    {
        public SpanishConjugatorIrrulalarPatternEstar(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }


        #region Present

        public override VerbConjugation ConjugatePresentYo()
        {
            var conjugation = GetBasePresentYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "o"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentTu()
        {
            var conjugation = GetBasePresentTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "á"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "s"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetPresentElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "á"
            }];
        }

        // nosotros is regualr
        // vosotros is regular
        public override List<VerbConjugationPiece> GetPresentEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "án"
            }];
        }

        #endregion


        #region Preterite

        public override VerbConjugation ConjugatePreteriteYo()
        {
            var conjugation = GetBasePreteriteYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uve"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteTu()
        {
            var conjugation = GetBasePreteriteTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uv"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ste"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetPreteriteElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvo"
            }];
        }
        public override VerbConjugation ConjugatePreteriteNosotros()
        {
            var conjugation = GetBasePreteriteNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvi"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "mos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteVosotros()
        {
            var conjugation = GetBasePreteriteVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvi"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "steis"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetPreteriteEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "uvie"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ron"
            }];

        }
        #endregion


        #region Imperfect
        // imperfet are all regular
        #endregion


        #region Conditional

        // conditional are all regular
        #endregion


        #region Future

        // future are all regular

        #endregion


        #region SubjunctivePresent

        public override VerbConjugation ConjugateSubjunctivePresentYo()
        {
            var conjugation = GetBaseSubjunctivePresentYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "é"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctivePresentTu()
        {
            var conjugation = GetBaseSubjunctivePresentTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "é"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "s"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctivePresentElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "é"
            }];
        }
        // nosotros and vosotros are regular
        public override List<VerbConjugationPiece> GetSubjunctivePresentEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "e"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "n"
            }];
        }

        #endregion


        #region SubjunctiveImperfect

        public override VerbConjugation ConjugateSubjunctiveImperfectYo()
        {
            var conjugation = GetBaseSubjunctiveImperfectYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvie"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ra"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectTu()
        {
            var conjugation = GetBaseSubjunctiveImperfectTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvie"
            }); 
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ras"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctiveImperfectElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "uvie"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ra"
            }];
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectNosotros()
        {
            var conjugation = GetBaseSubjunctiveImperfectNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvié"
            }); conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ramos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectVosotros()
        {
            var conjugation = GetBaseSubjunctiveImperfectVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvie"
            }); 
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "rais"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctiveImperfectEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvie"
            } ,new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ran"
            }];
        }
        #endregion


        #region SubjunctiveFuture

        public override VerbConjugation ConjugateSubjunctiveFutureYo()
        {
            var conjugation = GetBaseSubjunctiveFutureYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvie"
            }); 
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "re"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureTu()
        {
            var conjugation = GetBaseSubjunctiveFutureTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvie"
            }); 
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "res"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctiveFutureElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvie"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "re"
            }];
        }
        public override VerbConjugation ConjugateSubjunctiveFutureNosotros()
        {
            var conjugation = GetBaseSubjunctiveFutureNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvié"
            }); 
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "remos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureVosotros()
        {
            var conjugation = GetBaseSubjunctiveFutureVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvie"
            }); 
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "reis"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctiveFutureEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "uvie"
            }, new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ren"
            }];
        }

        #endregion


        #region AffirmativeImperative


        public override VerbConjugation ConjugateAffirmativeImperativeTu()
        {
            var conjugation = GetBaseAffirmativeImperativeTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "á"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateAffirmativeImperativeUsted()
        {
            var conjugation = GetBaseAffirmativeImperativeUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            });
            return conjugation;
        }

        // nosotros and vosotros are regular
        public override VerbConjugation ConjugateAffirmativeImperativeUstedes()
        {
            var conjugation = GetBaseAffirmativeImperativeUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "én"
            });
            return conjugation;
        }

        #endregion


        #region NegativeImperative


        public override VerbConjugation ConjugateNegativeImperativeTu()
        {
            var conjugation = GetBaseNegativeImperativeTuConjugation();

            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "s"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateNegativeImperativeUsted()
        {
            var conjugation = GetBaseNegativeImperativeUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            });
            return conjugation;
        }
        // nosotros and vosotros are regular
        public override VerbConjugation ConjugateNegativeImperativeUstedes()
        {
            var conjugation = GetBaseNegativeImperativeUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "é"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "en"
            });
            return conjugation;
        }
        #endregion

    }
}
