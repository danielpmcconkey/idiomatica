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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Logic.Services.API
{
    public static class LanguageApi
    {
        public static Language? LanguageRead(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageId)
        {
            return DataCache.LanguageByIdRead(languageId, dbContextFactory);
        }
        public static async Task<Language?> LanguageReadAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid languageId)
        {
            return await DataCache.LanguageByIdReadAsync(languageId, dbContextFactory);
        }
        public static Language? LanguageReadByCode(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, AvailableLanguageCode code)
        {
            return DataCache.LanguageByCodeRead(code, dbContextFactory);
        }
        public static async Task<Language?> LanguageReadByCodeAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, AvailableLanguageCode code)
        {
            return await DataCache.LanguageByCodeReadAsync(code, dbContextFactory);
        }

        public static Dictionary<Guid, Language> LanguageOptionsRead(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Expression<Func<Language, bool>> filter)
        {
            var context = dbContextFactory.CreateDbContext();
            var options = context.Languages
                .Where(filter).OrderBy(x => x.Name).ToList();
            var returnDict = new Dictionary<Guid, Language>();
            if (options is not null)
            {
                foreach (var l in options)
                {
                    if (l is null) continue;
                    returnDict.Add(l.Id, l);
                }
            }
            return returnDict;
        }
        public static async Task<Dictionary<Guid, Language>> LanguageOptionsReadAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Expression<Func<Language, bool>> filter)
        {
            return await Task<Dictionary<string, Language>>.Run(() =>
            {
                return LanguageOptionsRead(dbContextFactory, filter);
            });

        }

    }
}
