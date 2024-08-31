using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{

    public class Ir : _Ir
    {
        public Ir(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            
        }


        #region Present

        public override List<VerbConjugationPiece> GetRootPiecesPresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "voy"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentYo()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vas"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentTu()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "va"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEl()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vamos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentNosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vais"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentVosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "van"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEllos()
        {
            return [];
        }
        #endregion

        #region Preterite

        public override List<VerbConjugationPiece> GetRootPiecesPreteriteYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fui"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteYo()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuiste"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteTu()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fue"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuimos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteNosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuisteis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteVosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesPreteriteEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fueron"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPreteriteEllos()
        {
            return [];
        }
        #endregion

        #region Imperfect

        public override List<VerbConjugationPiece> GetRootPiecesImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "iba"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectYo()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ibas"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectTu()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "iba"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectEl()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "íbamos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectNosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ibais"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectVosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "iban"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesImperfectEllos()
        {
            return [];
        }
        #endregion

        #region SubjunctivePresent

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vaya"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentYo()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vayas"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentTu()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vaya"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEl()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vayamos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentNosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vayáis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentVosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vayan"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEllos()
        {
            return [];
        }
        #endregion

        #region SubjunctiveImperfect

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuera"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectYo()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fueras"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectTu()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuera"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEl()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuéramos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectNosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuerais"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectVosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fueran"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEllos()
        {
            return [];
        }
        #endregion

        #region SubjunctiveFuture

        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuere"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureYo()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fueres"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureTu()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuere"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEl()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuéremos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureNosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fuereis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureVosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "fueren"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEllos()
        {
            return [];
        }
        #endregion

        #region AffirmativeImperative

        
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ve"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeTu()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vaya"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUsted()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vamos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeNosotros()
        {
            return [];
        }
        
        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vayan"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUstedes()
        {
            return [];
        }
        #endregion

        #region NegaativeImperative

        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vayas"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeTu()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vaya"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUsted()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vayamos"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeNosotros()
        {
            return [];
        }
        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vayáis"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeVosotros()
        {
            return [];
        }

        public override List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "vayan"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUstedes()
        {
            return [];
        }
        #endregion





    }
}
