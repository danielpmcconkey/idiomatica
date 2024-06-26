﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<string, Language> LanguageByCode = new ConcurrentDictionary<string, Language>();
        private static ConcurrentDictionary<int, Language> LanguageById = new ConcurrentDictionary<int, Language>();
        
        
        #region read
        public static async Task<Language?> LanguageByCodeReadAsync(string key, IdiomaticaContext context)
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

            if (value == null) return null;
            // write to cache
            LanguageByCode[key] = value;
            LanguageById[(int)value.Id] = value;
            return value;
        }
        public static async Task<Language?> LanguageByIdReadAsync(int key, IdiomaticaContext context)
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

            if (value == null) return null;
            // write to cache
            LanguageById[key] = value;
            return value;
        } 
        #endregion

    }
}
