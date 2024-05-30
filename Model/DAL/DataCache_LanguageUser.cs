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
        private static ConcurrentDictionary<int, LanguageUser> LanguageUserById = new ConcurrentDictionary<int, LanguageUser>();
        private static ConcurrentDictionary<(int languageId, int userId), LanguageUser> LanguageUserByLanguageIdAndUserId = new ConcurrentDictionary<(int languageId, int userId), LanguageUser>();

        public static async Task<LanguageUser> LanguageUserByIdReadAsync(
            int key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageUserById.ContainsKey(key))
            {
                return LanguageUserById[key];
            }

            // read DB
            var value = context.LanguageUsers
                .Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            LanguageUserById[key] = value;
            return value;
        }
        public static async Task<LanguageUser> LanguageUserByLanguageIdAndUserIdReadAsync(
            (int languageId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (LanguageUserByLanguageIdAndUserId.ContainsKey(key))
            {
                return LanguageUserByLanguageIdAndUserId[key];
            }

            // read DB
            var value = context.LanguageUsers
                .Where(x => x.LanguageId == key.languageId && x.UserId == key.userId)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            LanguageUserByLanguageIdAndUserId[key] = value;
            return value;
        }
    }
}