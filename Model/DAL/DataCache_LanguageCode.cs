using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<string, LanguageCode> LanguageCodeByCode = new ConcurrentDictionary<string, LanguageCode>();
        private static ConcurrentDictionary<int, LanguageCode> LanguageCodeUserInterfacePreferenceByUserId = new();

        #region read

        public static LanguageCode? LanguageCodeByCodeRead(
            string key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageCodeByCode.ContainsKey(key))
            {
                return LanguageCodeByCode[key];
            }
            // read DB
            var value = context.LanguageCodes
                .Where(lc => lc.Code == key)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            LanguageCodeByCode[key] = value;
            return value;
        }
        public static async Task<LanguageCode?> LanguageCodeByCodeReadAsync(
            string key, IdiomaticaContext context)
        {
            return await Task<LanguageCode?>.Run(() =>
            {
                return LanguageCodeByCodeRead(key, context);
            });
        }


        public static LanguageCode? LanguageCodeUserInterfacePreferenceByUserIdRead(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageCodeUserInterfacePreferenceByUserId.ContainsKey(key))
            {
                return LanguageCodeUserInterfacePreferenceByUserId[key];
            }
            // read DB
            var value = (from u in context.Users
                        join lc in context.LanguageCodes on u.Code equals lc.Code
                        where u.Id == key
                        select lc)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            LanguageCodeUserInterfacePreferenceByUserId[key] = value;
            if (value.Code is null) return value;
            LanguageCodeByCode[value.Code] = value;
            return value;
        }
        public static async Task<LanguageCode?> LanguageCodeUserInterfacePreferenceByUserIdReadAsync(
            int key, IdiomaticaContext context)
        {
            return await Task<LanguageCode?>.Run(() =>
            {
                return LanguageCodeUserInterfacePreferenceByUserIdRead(key, context);
            });
        }

        #endregion

    }
}
