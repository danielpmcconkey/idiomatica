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
        private static ConcurrentDictionary<AvailableLanguageCode, Language> LanguageByCode = [];
        private static ConcurrentDictionary<Guid, Language> LanguageById = [];


        #region read
        public static Language? LanguageByCodeRead(
            AvailableLanguageCode key, IdiomaticaContext context)
        {
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
            AvailableLanguageCode key, IdiomaticaContext context)
        {
            return await Task<Language?>.Run(() =>
            {
                return LanguageByCodeRead(key, context);
            });
        }

        public static Language? LanguageByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageById.ContainsKey(key))
            {
                return LanguageById[key];
            }
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
        public static async Task<Language?> LanguageByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<Language?>.Run(() =>
            {
                return LanguageByIdRead(key, context);
            });
        }
        #endregion

    }
}
