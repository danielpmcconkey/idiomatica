using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Logic.Telemetry;
using DeepL;
using System.Linq.Expressions;
using Model.Enums;

namespace Logic.Services.API
{
    public static class LanguageApi
    {
        public static Language? LanguageRead(IdiomaticaContext context, Guid languageId)
        {
            return DataCache.LanguageByIdRead(languageId, context);
        }
        public static async Task<Language?> LanguageReadAsync(IdiomaticaContext context, Guid languageId)
        {
            return await DataCache.LanguageByIdReadAsync(languageId, context);
        }
        public static Language? LanguageReadByCode(
            IdiomaticaContext context, AvailableLanguageCode code)
        {
            return DataCache.LanguageByCodeRead(code, context);
        }
        public static async Task<Language?> LanguageReadByCodeAsync(
            IdiomaticaContext context, AvailableLanguageCode code)
        {
            return await DataCache.LanguageByCodeReadAsync(code, context);
        }

        public static Dictionary<Guid, Language> LanguageOptionsRead(
            IdiomaticaContext context, Expression<Func<Language, bool>> filter)
        {
            var options = context.Languages
                .Where(filter).OrderBy(x => x.Name).ToList();
            var returnDict = new Dictionary<Guid, Language>();
            if (options is not null)
            {
                foreach (var l in options)
                {
                    if (l is null) continue;
                    returnDict.Add(l.UniqueKey, l);
                }
            }
            return returnDict;
        }
        public static async Task<Dictionary<Guid, Language>> LanguageOptionsReadAsync(
            IdiomaticaContext context, Expression<Func<Language, bool>> filter)
        {
            return await Task<Dictionary<string, Language>>.Run(() =>
            {
                return LanguageOptionsRead(context, filter);
            });

        }

    }
}
