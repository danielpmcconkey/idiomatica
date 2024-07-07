using Microsoft.EntityFrameworkCore;
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
        private static ConcurrentDictionary<int, FlashCardAttempt?> FlashCardAttemptById = new ConcurrentDictionary<int, FlashCardAttempt?>();
        private static ConcurrentDictionary<int, List<FlashCardAttempt>> FlashCardAttemptsByFlashCardId = new ConcurrentDictionary<int, List<FlashCardAttempt>>();


        #region create
        public static async Task<bool> FlashCardAttemptCreateAsync(FlashCardAttempt value, IdiomaticaContext context)
        {
            context.FlashCardAttempts.Add(value);
            await context.SaveChangesAsync();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            FlashCardAttemptById[(int)value.Id] = value;
            return true;
        }
        #endregion

        #region read
        public static async Task<FlashCardAttempt?> FlashCardAttemptByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardAttemptById.ContainsKey(key))
            {
                return FlashCardAttemptById[key];
            }

            // read DB
            var value = context.FlashCardAttempts.Where(x => x.FlashCardId == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardAttemptById[key] = value;
            return value;
        }

        #endregion

        #region update
        
        #endregion

    }
}
