using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Logic.Telemetry;

namespace Logic.Services.API
{
    public static class LanguageApi
    {
        public static Language? LanguageRead(IdiomaticaContext context, int languageId)
        {
            if (languageId < 1) ErrorHandler.LogAndThrow();
            return DataCache.LanguageByIdRead(languageId, context);
        }
        public static async Task<Language?> LanguageReadAsync(IdiomaticaContext context, int languageId)
        {
            if (languageId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.LanguageByIdReadAsync(languageId, context);
        }
        
    }
}
