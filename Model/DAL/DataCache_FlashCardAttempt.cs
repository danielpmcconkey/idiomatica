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


        public static FlashCardAttempt? FlashCardAttemptCreate(FlashCardAttempt flashCardAttempt, IdiomaticaContext context)
        {
            if (flashCardAttempt.FlashCardId is null) throw new ArgumentNullException(nameof(flashCardAttempt.FlashCardId));
            
            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[FlashCardAttempt]
                      ([FlashCardId]
                      ,[Status]
                      ,[AttemptedWhen]
                      ,[UniqueKey])
                VALUES
                      ({flashCardAttempt.FlashCardId}
                      ,{flashCardAttempt.Status}
                      ,{flashCardAttempt.AttemptedWhen}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating FlashCardAttempt affected 0 rows");
            var newEntity = context.FlashCardAttempts.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in FlashCardAttemptCreate");
            }


            // add it to cache
            FlashCardAttemptById[(int)newEntity.Id] = newEntity;

            return newEntity;
        }
        public static async Task<FlashCardAttempt?> FlashCardAttemptCreateAsync(FlashCardAttempt value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return FlashCardAttemptCreate(value, context); });
        }


        #endregion

        #region read
        public static FlashCardAttempt? FlashCardAttemptByIdRead(int key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardAttemptById.TryGetValue(key, out FlashCardAttempt? value))
            {
                return value;
            }

            // read DB
            value = context.FlashCardAttempts.Where(x => x.FlashCardId == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardAttemptById[key] = value;
            return value;
        }
        public static async Task<FlashCardAttempt?> FlashCardAttemptByIdReadAsync(int key, IdiomaticaContext context)
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
