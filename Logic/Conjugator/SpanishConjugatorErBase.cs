using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public class SpanishConjugatorErBase : SpanishConjugator
    {
        public SpanishConjugatorErBase(
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
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "es"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentUsted()
        {
            var conjugation = GetBasePresentUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "e"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentEl()
        {
            var conjugation = GetBasePresentElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "e"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentElla()
        {
            var conjugation = GetBasePresentEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "e"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentNosotros()
        {
            var conjugation = GetBasePresentNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "emos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentVosotros()
        {
            var conjugation = GetBasePresentVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éis"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentUstedes()
        {
            var conjugation = GetBasePresentUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "en"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentEllos()
        {
            var conjugation = GetBasePresentEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "en"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentEllas()
        {
            var conjugation = GetBasePresentEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "en"
            });
            return conjugation;
        }
        #endregion


        #region Preterite

        public override VerbConjugation ConjugatePreteriteYo()
        {
            var conjugation = GetBasePreteriteYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iste"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteUsted()
        {
            var conjugation = GetBasePreteriteUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ió"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteEl()
        {
            var conjugation = GetBasePreteriteElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ió"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteElla()
        {
            var conjugation = GetBasePreteriteEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ió"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteNosotros()
        {
            var conjugation = GetBasePreteriteNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "isteis"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteUstedes()
        {
            var conjugation = GetBasePreteriteUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieron"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteEllos()
        {
            var conjugation = GetBasePreteriteEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieron"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteEllas()
        {
            var conjugation = GetBasePreteriteEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieron"
            });
            return conjugation;
        }
        #endregion


        #region Imperfect

        public override VerbConjugation ConjugateImperfectYo()
        {
            var conjugation = GetBaseImperfectYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ías"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectUsted()
        {
            var conjugation = GetBaseImperfectUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ía"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectEl()
        {
            var conjugation = GetBaseImperfectElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ía"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectElla()
        {
            var conjugation = GetBaseImperfectEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ía"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectNosotros()
        {
            var conjugation = GetBaseImperfectNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "íais"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectUstedes()
        {
            var conjugation = GetBaseImperfectUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ían"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectEllos()
        {
            var conjugation = GetBaseImperfectEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ían"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectEllas()
        {
            var conjugation = GetBaseImperfectEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ían"
            });
            return conjugation;
        }
        #endregion


        #region Conditional

        public override VerbConjugation ConjugateConditionalYo()
        {
            var conjugation = GetBaseConditionalYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ería"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalTu()
        {
            var conjugation = GetBaseConditionalTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erías"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalUsted()
        {
            var conjugation = GetBaseConditionalUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ería"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalEl()
        {
            var conjugation = GetBaseConditionalElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ería"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalElla()
        {
            var conjugation = GetBaseConditionalEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ería"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalNosotros()
        {
            var conjugation = GetBaseConditionalNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eríamos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalVosotros()
        {
            var conjugation = GetBaseConditionalVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eríais"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalUstedes()
        {
            var conjugation = GetBaseConditionalUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erían"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalEllos()
        {
            var conjugation = GetBaseConditionalEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erían"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalEllas()
        {
            var conjugation = GetBaseConditionalEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erían"
            });
            return conjugation;
        }
        #endregion


        #region Future

        public override VerbConjugation ConjugateFutureYo()
        {
            var conjugation = GetBaseFutureYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eré"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureTu()
        {
            var conjugation = GetBaseFutureTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erás"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureUsted()
        {
            var conjugation = GetBaseFutureUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erá"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureEl()
        {
            var conjugation = GetBaseFutureElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erá"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureElla()
        {
            var conjugation = GetBaseFutureEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erá"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureNosotros()
        {
            var conjugation = GetBaseFutureNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eremos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureVosotros()
        {
            var conjugation = GetBaseFutureVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "eréis"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureUstedes()
        {
            var conjugation = GetBaseFutureUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erán"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureEllos()
        {
            var conjugation = GetBaseFutureEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erán"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureEllas()
        {
            var conjugation = GetBaseFutureEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "erán"
            });
            return conjugation;
        }
        #endregion


        #region SubjunctivePresent

        public override VerbConjugation ConjugateSubjunctivePresentYo()
        {
            var conjugation = GetBaseSubjunctivePresentYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "as"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctivePresentUsted()
        {
            var conjugation = GetBaseSubjunctivePresentUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctivePresentEl()
        {
            var conjugation = GetBaseSubjunctivePresentElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctivePresentElla()
        {
            var conjugation = GetBaseSubjunctivePresentEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctivePresentNosotros()
        {
            var conjugation = GetBaseSubjunctivePresentNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "áis"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctivePresentUstedes()
        {
            var conjugation = GetBaseSubjunctivePresentUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctivePresentEllos()
        {
            var conjugation = GetBaseSubjunctivePresentEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctivePresentEllas()
        {
            var conjugation = GetBaseSubjunctivePresentEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            });
            return conjugation;
        }

        #endregion


        #region SubjunctiveImperfect

        public override VerbConjugation ConjugateSubjunctiveImperfectYo()
        {
            var conjugation = GetBaseSubjunctiveImperfectYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieras"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectUsted()
        {
            var conjugation = GetBaseSubjunctiveImperfectUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iera"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectEl()
        {
            var conjugation = GetBaseSubjunctiveImperfectElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iera"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectElla()
        {
            var conjugation = GetBaseSubjunctiveImperfectEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iera"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectNosotros()
        {
            var conjugation = GetBaseSubjunctiveImperfectNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ierais"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectUstedes()
        {
            var conjugation = GetBaseSubjunctiveImperfectUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieran"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectEllos()
        {
            var conjugation = GetBaseSubjunctiveImperfectEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieran"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveImperfectEllas()
        {
            var conjugation = GetBaseSubjunctiveImperfectEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieran"
            });
            return conjugation;
        }
        #endregion


        #region SubjunctiveFuture

        public override VerbConjugation ConjugateSubjunctiveFutureYo()
        {
            var conjugation = GetBaseSubjunctiveFutureYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieres"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureUsted()
        {
            var conjugation = GetBaseSubjunctiveFutureUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iere"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureEl()
        {
            var conjugation = GetBaseSubjunctiveFutureElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iere"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureElla()
        {
            var conjugation = GetBaseSubjunctiveFutureEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iere"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureNosotros()
        {
            var conjugation = GetBaseSubjunctiveFutureNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "iereis"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureUstedes()
        {
            var conjugation = GetBaseSubjunctiveFutureUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieren"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureEllos()
        {
            var conjugation = GetBaseSubjunctiveFutureEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieren"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateSubjunctiveFutureEllas()
        {
            var conjugation = GetBaseSubjunctiveFutureEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ieren"
            });
            return conjugation;
        }
        #endregion


        #region AffirmativeImperative


        public override VerbConjugation ConjugateAffirmativeImperativeTu()
        {
            var conjugation = GetBaseAffirmativeImperativeTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
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
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ed"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateAffirmativeImperativeUstedes()
        {
            var conjugation = GetBaseAffirmativeImperativeUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
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
                Ordinal = 2,
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
                Ordinal = 2,
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
                Ordinal = 2,
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
                Ordinal = 2,
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
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            });
            return conjugation;
        }
        #endregion

    }
}
