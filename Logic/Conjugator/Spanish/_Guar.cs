using Logic.Conjugator.Spanish;
using Logic.Conjugator;
using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{


    public class _Guar : _Ar
    {
        public _Guar(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            throw new NotImplementedException();
            // see page 173. None of these verbs were in my initial list
        }
    }
}