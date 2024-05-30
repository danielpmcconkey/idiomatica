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
        
        #region read
        public static async Task<LanguageCode?> LanguageCodeByCodeReadAsync(string key, IdiomaticaContext context)
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
        #endregion

    }
}
