using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LanguageParser
{
    internal static class Factory
    {
        internal static ILanguageParser GetLanguageParser(LanguageUser languageUser)
        {
            return GetLanguageParser(languageUser.Language);
        }
        internal static ILanguageParser GetLanguageParser(Language language)
        {
            if (language.ParserType == "spacedel")
            {
                return new SpanishParser();
            }
            else throw new NotImplementedException("other parser types not built yet");
        }
    }
}
