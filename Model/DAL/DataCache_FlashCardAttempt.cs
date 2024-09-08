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
        private static ConcurrentDictionary<Guid, FlashCardAttempt?> FlashCardAttemptById = new ConcurrentDictionary<Guid, FlashCardAttempt?>();
        private static ConcurrentDictionary<Guid, List<FlashCardAttempt>> FlashCardAttemptsByFlashCardId = new ConcurrentDictionary<Guid, List<FlashCardAttempt>>();


        #region create


        public static FlashCardAttempt? FlashCardAttemptCreate(FlashCardAttempt flashCardAttempt, IdiomaticaContext context)
        {
            if (flashCardAttempt.FlashCardKey is null) throw new ArgumentNullException(nameof(flashCardAttempt.FlashCardKey));
            
            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[FlashCardAttempt]
                      ([FlashCardKey]
                      ,[Status]
                      ,[AttemptedWhen]
                      ,[UniqueKey])
                VALUES
                      ({flashCardAttempt.FlashCardKey}
                      ,{flashCardAttempt.Status}
                      ,{flashCardAttempt.AttemptedWhen}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating FlashCardAttempt affected 0 rows");
            var newEntity = context.FlashCardAttempts.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.UniqueKey is null)
            {
                throw new InvalidDataException("newEntity is null in FlashCardAttemptCreate");
            }


            // add it to cache
            FlashCardAttemptById[(Guid)newEntity.UniqueKey] = newEntity;

            return newEntity;
        }
        public static async Task<FlashCardAttempt?> FlashCardAttemptCreateAsync(FlashCardAttempt value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return FlashCardAttemptCreate(value, context); });
        }


        #endregion

        #region read
        public static FlashCardAttempt? FlashCardAttemptByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardAttemptById.TryGetValue(key, out FlashCardAttempt? value))
            {
                return value;
            }

            // read DB
            value = context.FlashCardAttempts.Where(x => x.FlashCardKey == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardAttemptById[key] = value;
            return value;
        }
        public static async Task<FlashCardAttempt?> FlashCardAttemptByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<FlashCardAttempt>.Run(() =>
            {
                return FlashCardAttemptByIdRead(key, context);
            });
        }

        #endregion

        #region update

        #endregion

    }
}
