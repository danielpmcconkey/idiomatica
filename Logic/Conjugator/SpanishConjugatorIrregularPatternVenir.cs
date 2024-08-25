using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public class SpanishConjugatorIrregularPatternVenir : SpanishConjugatorIrBase
    {
        public SpanishConjugatorIrregularPatternVenir(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }

        

        internal override List<VerbConjugationPiece> GetCorePiece2()
        {
            // ven -> vien
            if(string.IsNullOrEmpty(_sourceLanguageInfinitive.Core2))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "ie";
            return ReplaceMiddleOfCore(_sourceLanguageInfinitive.Core2, seek);
        }
        internal override List<VerbConjugationPiece> GetCorePiece3()
        {
            // ven -> vin
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core3))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "i";
            return ReplaceMiddleOfCore(_sourceLanguageInfinitive.Core2, seek);
        }


        #region Present

        public override VerbConjugation ConjugatePresentYo()
        {
            var conjugation = GetBasePresentYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "o"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentTu()
        {
            var conjugation = GetBasePresentTuConjugation(2);
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
        public override VerbConjugation ConjugatePresentUsted()
        {
            var conjugation = GetBasePresentUstedConjugation(2);
            conjugation.Pieces.AddRange(GetPresentElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentEl()
        {
            var conjugation = GetBasePresentElConjugation(2);
            conjugation.Pieces.AddRange(GetPresentElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentElla()
        {
            var conjugation = GetBasePresentEllaConjugation(2);
            conjugation.Pieces.AddRange(GetPresentElPieces());
            return conjugation;
        }
        // nosotros is regular
        // vosotros is regular
        public override List<VerbConjugationPiece> GetPresentEllosPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "en"
            }];
        }
        public override VerbConjugation ConjugatePresentUstedes()
        {
            var conjugation = GetBasePresentUstedesConjugation(2);
            conjugation.Pieces.AddRange(GetPresentEllosPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentEllos()
        {
            var conjugation = GetBasePresentEllosConjugation(2);
            conjugation.Pieces.AddRange(GetPresentEllosPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentEllas()
        {
            var conjugation = GetBasePresentEllasConjugation(2);
            conjugation.Pieces.AddRange(GetPresentEllosPieces());
            return conjugation;
        }
        #endregion


        #region Preterite

        public override VerbConjugation ConjugatePreteriteYo()
        {
            var conjugation = GetBasePreteriteYoConjugation(3);
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "e"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteTu()
        {
            var conjugation = GetBasePreteriteTuConjugation(3);
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
                Piece = "o"
            }];
        }
        public override VerbConjugation ConjugatePreteriteUsted()
        {
            var conjugation = GetBasePreteriteUstedConjugation(3);
            conjugation.Pieces.AddRange(GetPreteriteElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteEl()
        {
            var conjugation = GetBasePreteriteElConjugation(3);
            conjugation.Pieces.AddRange(GetPreteriteElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteElla()
        {
            var conjugation = GetBasePreteriteEllaConjugation(3);
            conjugation.Pieces.AddRange(GetPreteriteElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteNosotros()
        {
            var conjugation = GetBasePreteriteNosotrosConjugation(3);
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
            var conjugation = GetBasePreteriteVosotrosConjugation(3);
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
        public override VerbConjugation ConjugatePreteriteUstedes()
        {
            var conjugation = GetBasePreteriteUstedesConjugation(3);
            conjugation.Pieces.AddRange(GetPreteriteEllosPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteEllos()
        {
            var conjugation = GetBasePreteriteEllosConjugation(3);
            conjugation.Pieces.AddRange(GetPreteriteEllosPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteEllas()
        {
            var conjugation = GetBasePreteriteEllasConjugation(3);
            conjugation.Pieces.AddRange(GetPreteriteEllosPieces());
            return conjugation;
        }
        #endregion


        #region Imperfect
        // imperfect are all regular
        
        #endregion


        #region Conditional

        public override VerbConjugation ConjugateConditionalYo()
        {
            var conjugation = GetBaseConditionalYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ía"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalTu()
        {
            var conjugation = GetBaseConditionalTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ías"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetConditionalElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            }, new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ía"
            }];
        }
        public override VerbConjugation ConjugateConditionalNosotros()
        {
            var conjugation = GetBaseConditionalNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "íamos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalVosotros()
        {
            var conjugation = GetBaseConditionalVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "íais"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetConditionalEllosPieces()
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

        public override VerbConjugation ConjugateFutureYo()
        {
            var conjugation = GetBaseFutureYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "é"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureTu()
        {
            var conjugation = GetBaseFutureTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ás"
            });
            return conjugation;
        }

        public override List<VerbConjugationPiece> GetFutureElPieces()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            }, new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public override VerbConjugation ConjugateFutureNosotros()
        {
            var conjugation = GetBaseFutureNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "emos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureVosotros()
        {
            var conjugation = GetBaseFutureVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "dr"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éis"
            });
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetFutureEllosPieces()
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
                Piece = "an"
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            },new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            }];
        }

        #endregion


        #region SubjunctiveImperfect


        public override VerbConjugation ConjugateSubjunctiveImperfectYo()
        {
            var conjugation = GetBaseSubjunctiveImperfectYoConjugation(3);
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
            var conjugation = GetBaseSubjunctiveImperfectTuConjugation(3);
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
        public override VerbConjugation ConjugateSubjunctiveImperfectUsted()
        {
            var conjugation = GetBaseSubjunctiveImperfectUstedConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectEl()
        {
            var conjugation = GetBaseSubjunctiveImperfectElConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectElla()
        {
            var conjugation = GetBaseSubjunctiveImperfectEllaConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectNosotros()
        {
            var conjugation = GetBaseSubjunctiveImperfectNosotrosConjugation(3);
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
            var conjugation = GetBaseSubjunctiveImperfectVosotrosConjugation(3);
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
        public override VerbConjugation ConjugateSubjunctiveImperfectUstedes()
        {
            var conjugation = GetBaseSubjunctiveImperfectUstedesConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectEllosPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectEllos()
        {
            var conjugation = GetBaseSubjunctiveImperfectEllosConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectEllosPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectEllas()
        {
            var conjugation = GetBaseSubjunctiveImperfectEllasConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectEllosPieces());
            return conjugation;
        }
        #endregion



        #region SubjunctiveFuture


        public override VerbConjugation ConjugateSubjunctiveFutureYo()
        {
            var conjugation = GetBaseSubjunctiveFutureYoConjugation(3);
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
            var conjugation = GetBaseSubjunctiveFutureTuConjugation(3);
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
        public override VerbConjugation ConjugateSubjunctiveFutureUsted()
        {
            var conjugation = GetBaseSubjunctiveFutureUstedConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveFutureElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureEl()
        {
            var conjugation = GetBaseSubjunctiveFutureElConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveFutureElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureElla()
        {
            var conjugation = GetBaseSubjunctiveFutureEllaConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveFutureElPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureNosotros()
        {
            var conjugation = GetBaseSubjunctiveFutureNosotrosConjugation(3);
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
            var conjugation = GetBaseSubjunctiveFutureVosotrosConjugation(3);
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
        public override VerbConjugation ConjugateSubjunctiveFutureUstedes()
        {
            var conjugation = GetBaseSubjunctiveFutureUstedesConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveFutureEllosPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureEllos()
        {
            var conjugation = GetBaseSubjunctiveFutureEllosConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveFutureEllosPieces());
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureEllas()
        {
            var conjugation = GetBaseSubjunctiveFutureEllasConjugation(3);
            conjugation.Pieces.AddRange(GetSubjunctiveFutureEllosPieces());
            return conjugation;
        }
        #endregion


        #region AffirmativeImperative


        public override VerbConjugation ConjugateAffirmativeImperativeTu()
        {
            // I don't know if this is right but it works for venir. There is not stem. Just the root
            var conjugation = GetBaseAffirmativeImperativeTuConjugation();
            
            return conjugation;
        }
        public override VerbConjugation ConjugateAffirmativeImperativeUsted()
        {
            var conjugation = GetBaseAffirmativeImperativeUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            });
            return conjugation;
        }
        // vosotros is regular, for some reason
        public override VerbConjugation ConjugateAffirmativeImperativeUstedes()
        {
            var conjugation = GetBaseAffirmativeImperativeUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
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
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "g"
            });
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 210,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            });
            return conjugation;
        }
        #endregion



    }
}
