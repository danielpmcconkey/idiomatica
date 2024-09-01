using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public static class Factory
    {
        public static Conjugator? Get(string fullyQualifiedName, IVerbTranslator? targetTranslator,
            Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive)
        {
            Type? t = Type.GetType(fullyQualifiedName);
            if (t is null) { ErrorHandler.LogAndThrow(); return null; }

            var newObject = Activator.CreateInstance(t,
                [targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive]);

            if (newObject is null) { ErrorHandler.LogAndThrow(); return null; }

            return (Conjugator)newObject;
        }
    }
}
