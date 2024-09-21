using Microsoft.EntityFrameworkCore;
using Model.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Model.DAL
{
    public static partial class DataCache
    {
        

        #region read
        public static Language? LanguageByCodeRead(
            AvailableLanguageCode key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            // check cache
            if (LanguageByCode.ContainsKey(key))
            {
                return LanguageByCode[key];
            }
            // read DB
            var value = context.Languages
                .Where(l => l.Code == key)
                .FirstOrDefault();

            if (value is null) return null;
            // write to cache
            LanguageByCode[key] = value;
            LanguageById[(Guid)value.Id] = value;
            return value;
        }
        public static async Task<Language?> LanguageByCodeReadAsync(
            AvailableLanguageCode key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Language?>.Run(() =>
            {
                return LanguageByCodeRead(key, dbContextFactory);
            });
        }

        public static Language? LanguageByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (LanguageById.ContainsKey(key))
            {
                return LanguageById[key];
            }
            var context = dbContextFactory.CreateDbContext();

            // read DB
            var value = context.Languages
                .Where(l => l.Id == key)
                .FirstOrDefault();

            if (value is null) return null;
            // write to cache
            LanguageById[key] = value;
            LanguageByCode[value.Code] = value;
            return value;
        }
        public static async Task<Language?> LanguageByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Language?>.Run(() =>
            {
                return LanguageByIdRead(key, dbContextFactory);
            });
        }
        #endregion

    }
}
