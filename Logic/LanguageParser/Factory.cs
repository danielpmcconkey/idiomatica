using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.LanguageParser
{

    /*
	 * WARNING
	 * due to the way blazor hosts multiple app sessions
	 * in the same process (https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-8.0)
	 * this static class should never persist anything
	 * all functions should have zero side-effects
	 * */
    internal static class Factory
    {
        internal static ILanguageParser GetLanguageParser(Language language)
        {
            if (language.ParserType == "spacedel")
            {
                return new SpaceDelimited(language);
            }
            else throw new NotImplementedException("other parser types not built yet");

        }
    }
}
