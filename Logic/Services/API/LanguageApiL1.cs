using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Logic.Telemetry;

namespace Logic.Services.Level1
{
    public static class LanguageApiL1
    {
        public static async Task<Language?> LanguageReadAsync(IdiomaticaContext context, int languageId)
        {
            if (languageId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.LanguageByIdReadAsync((languageId), context);
        }
        public static async Task<LanguageCode?> LanguageReadByCodeAsync(IdiomaticaContext context, string code)
        {
            if (!string.IsNullOrEmpty(code)) ErrorHandler.LogAndThrow();
            return await DataCache.LanguageCodeByCodeReadAsync(code, context);
        }
    }
}
