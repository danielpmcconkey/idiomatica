using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    public class _Gar : _Car
    {
        public _Gar(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }

        internal override List<VerbConjugationPiece> GetCorePiece2()
        {
            // pag -> pagu
            if (string.IsNullOrEmpty(_sourceLanguageInfinitive.Core2))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            string seek = "gu";
            return ReplaceRightOfCore(_sourceLanguageInfinitive.Core2, seek);
        }
    }
}
