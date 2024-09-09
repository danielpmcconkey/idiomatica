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
    public class Mantener : Tener
    {
        public Mantener(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }

        #region AffirmativeImperative


        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = "mant"
            },new VerbConjugationPiece()
            {
                Ordinal = 110,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "én"
            }];
        }
        #endregion

       



    }
}
