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
    public class Suponer :Poner
    {
        public Suponer(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }


        

        #region AffirmativeImperative


        public override List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece() {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = "sup"
            },new VerbConjugationPiece() {
                Ordinal = 110,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "ón"}];
        }


        #endregion

        



    }
}
