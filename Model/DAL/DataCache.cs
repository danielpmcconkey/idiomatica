using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        
        private static ConcurrentDictionary<string, User> UserByApplicationUserId = new ConcurrentDictionary<string, User>();


        
        
        
        
        
        
        
        
        public static async Task<User?> UserByApplicationUserIdReadAsync(string key, IdiomaticaContext context)
        {
            // check cache
            if (UserByApplicationUserId.ContainsKey(key))
            {
                return UserByApplicationUserId[key];
            }
            // read DB
            var value = context.Users
                .Where(u => u.ApplicationUserId == key)
                .FirstOrDefault();

            if (value == null) return null;
            // write to cache
            UserByApplicationUserId[key] = value;
            return value;
        }
    }
}
