using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public class SpanishConjugatorIrBase : SpanishConjugator
    {
        public SpanishConjugatorIrBase(
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
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentTu()
        {
            var conjugation = GetBasePresentTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "es"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetPresentElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "e"
            }];
        }
        public override VerbConjugation ConjugatePresentNosotros()
        {
            var conjugation = GetBasePresentNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "imos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentVosotros()
        {
            var conjugation = GetBasePresentVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ís"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetPresentEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "en"
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
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "í"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteTu()
        {
            var conjugation = GetBasePreteriteTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iste"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetPreteriteElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ió"
            }];
        }

        public override VerbConjugation ConjugatePreteriteNosotros()
        {
            var conjugation = GetBasePreteriteNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "imos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteVosotros()
        {
            var conjugation = GetBasePreteriteVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "isteis"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetPreteriteEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieron"
            }];
        }

        #endregion


        #region Imperfect

        public override VerbConjugation ConjugateImperfectYo()
        {
            var conjugation = GetBaseImperfectYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ía"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectTu()
        {
            var conjugation = GetBaseImperfectTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ías"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetImperfectElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ía"
            }];
        }

        public override VerbConjugation ConjugateImperfectNosotros()
        {
            var conjugation = GetBaseImperfectNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "íamos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectVosotros()
        {
            var conjugation = GetBaseImperfectVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "íais"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetImperfectEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ían"
            }];
        }

        #endregion


        #region Conditional

        public override VerbConjugation ConjugateConditionalYo()
        {
            var conjugation = GetBaseConditionalYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iría"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalTu()
        {
            var conjugation = GetBaseConditionalTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "irías"
            });
            return conjugation;
        }

        public override List<VerbConjugationPiece> GetConditionalElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iría"
            }];
        }

        public override VerbConjugation ConjugateConditionalNosotros()
        {
            var conjugation = GetBaseConditionalNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iríamos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalVosotros()
        {
            var conjugation = GetBaseConditionalVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iríais"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetConditionalEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "irían"
            }];
        }

        #endregion


        #region Future

        public override VerbConjugation ConjugateFutureYo()
        {
            var conjugation = GetBaseFutureYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iré"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureTu()
        {
            var conjugation = GetBaseFutureTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "irás"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetFutureElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "irá"
            }];
        }

        public override VerbConjugation ConjugateFutureNosotros()
        {
            var conjugation = GetBaseFutureNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iremos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureVosotros()
        {
            var conjugation = GetBaseFutureVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iréis"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetFutureEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "irán"
            }];
        }
        #endregion


        #region SubjunctivePresent

        public override VerbConjugation ConjugateSubjunctivePresentYo()
        {
            var conjugation = GetBaseSubjunctivePresentYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
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
                Piece = "as"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctivePresentElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public override VerbConjugation ConjugateSubjunctivePresentNosotros()
        {
            var conjugation = GetBaseSubjunctivePresentNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctivePresentVosotros()
        {
            var conjugation = GetBaseSubjunctivePresentVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "áis"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctivePresentEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
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
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iera"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectTu()
        {
            var conjugation = GetBaseSubjunctiveImperfectTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieras"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctiveImperfectElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iera"
            }];
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectNosotros()
        {
            var conjugation = GetBaseSubjunctiveImperfectNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iéramos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectVosotros()
        {
            var conjugation = GetBaseSubjunctiveImperfectVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ierais"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctiveImperfectEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieran"
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
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iere"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureTu()
        {
            var conjugation = GetBaseSubjunctiveFutureTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieres"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctiveFutureElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iere"
            }];
        }
        public override VerbConjugation ConjugateSubjunctiveFutureNosotros()
        {
            var conjugation = GetBaseSubjunctiveFutureNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iéremos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureVosotros()
        {
            var conjugation = GetBaseSubjunctiveFutureVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iereis"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetSubjunctiveFutureEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieren"
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
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "e"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateAffirmativeImperativeUsted()
        {
            var conjugation = GetBaseAffirmativeImperativeUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            });
            return conjugation;
        }

        public override VerbConjugation ConjugateAffirmativeImperativeNosotros()
        {
            var conjugation = GetBaseAffirmativeImperativeNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateAffirmativeImperativeVosotros()
        {
            var conjugation = GetBaseAffirmativeImperativeVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "id"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateAffirmativeImperativeUstedes()
        {
            var conjugation = GetBaseAffirmativeImperativeUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
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
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "as"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateNegativeImperativeUsted()
        {
            var conjugation = GetBaseNegativeImperativeUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateNegativeImperativeNosotros()
        {
            var conjugation = GetBaseNegativeImperativeNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateNegativeImperativeVosotros()
        {
            var conjugation = GetBaseNegativeImperativeVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "áis"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateNegativeImperativeUstedes()
        {
            var conjugation = GetBaseNegativeImperativeUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            });
            return conjugation;
        }
        #endregion

    }
}
