using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public interface IVerbTranslator
    {
        public string? Translate(Verb targetVerb, VerbConjugation conjugationType);
    }
}
