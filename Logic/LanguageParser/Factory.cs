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
            if (languageUser.Language.ParserType == "spacedel")
            {
                return new SpaceDelimited(languageUser.Language);
            }
            else throw new NotImplementedException("other parser types not built yet");

        }
    }
}
