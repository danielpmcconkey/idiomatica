using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public abstract class Conjugator
    {
        protected IVerbTranslator _targetTranslator;
        protected Verb _sourceLanguageInfinitive;
        protected Verb _targetLanguageInfinitive;

        public Conjugator(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive)
        {
            _targetTranslator = targetTranslator;
            _sourceLanguageInfinitive = sourceLanguageInfinitive;
            _targetLanguageInfinitive = targetLanguageInfinitive;
        }
        public abstract List<VerbConjugation> Conjugate();
    }
}
