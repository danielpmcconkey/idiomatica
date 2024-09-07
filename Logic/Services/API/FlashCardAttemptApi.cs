using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Logic.Services.API
{
    public static class FlashCardAttemptApi
    {
        public static FlashCardAttempt? FlashCardAttemptCreate(
            IdiomaticaContext context, Guid flashCardId, AvailableFlashCardAttemptStatus status)
        {
            FlashCardAttempt? attempt = new ()
            {
                FlashCardKey = flashCardId,
                AttemptedWhen = DateTime.Now,
                Status = status,
            };
            attempt = DataCache.FlashCardAttemptCreate(attempt, context);
            if (attempt is null || attempt.UniqueKey is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return attempt;
        }
        public static async Task<FlashCardAttempt?> FlashCardAttemptCreateAsync(
            IdiomaticaContext context, Guid flashCardId, AvailableFlashCardAttemptStatus status)
        {
            return await Task<FlashCardAttempt?>.Run(() =>
            {
                return FlashCardAttemptCreate(context, flashCardId, status);
            });
        }
    }
}
