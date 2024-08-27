using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    public class _Ar : SpanishConjugator
    {
        public _Ar(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }

        /*
         * the standard SpanishConjugator is set us as default for AR verbs. No
         * overrides here. I kept the class though for consistency with ER and
         * IR verbs
         * */


        #region Present

        #endregion


        #region Preterite


        #endregion


        #region Imperfect


        #endregion


        #region Conditional


        #endregion


        #region Future



        #endregion


        #region SubjunctivePresent


        #endregion


        #region SubjunctiveImperfect

        #endregion


        #region SubjunctiveFuture



        #endregion


        #region AffirmativeImperative



        #endregion


        #region NegativeImperative

        #endregion

    }
}
