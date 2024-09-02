using Logic.Conjugator.Spanish;
using Logic.Conjugator;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    public class _Ger : _Er
    {
        public _Ger(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {
            throw new NotImplementedException();
            // see page 173. None of these verbs were in my initial list
        }
    }
}