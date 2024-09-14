using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Model.Enums;

namespace Logic.Services.API
{
    public static class FlashCardAttemptApi
    {
        public static FlashCardAttempt? FlashCardAttemptCreate(
            IdiomaticaContext context, FlashCard flashCard, AvailableFlashCardAttemptStatus status)
        {
            FlashCardAttempt? attempt = new ()
            {
                UniqueKey = Guid.NewGuid(),
                FlashCardKey = flashCard.UniqueKey,
                FlashCard = flashCard,
                AttemptedWhen = DateTime.Now,
                Status = status,
            };
            attempt = DataCache.FlashCardAttemptCreate(attempt, context);
            if (attempt is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return attempt;
        }
        public static async Task<FlashCardAttempt?> FlashCardAttemptCreateAsync(
            IdiomaticaContext context, FlashCard flashCard, AvailableFlashCardAttemptStatus status)
        {
            return await Task<FlashCardAttempt?>.Run(() =>
            {
                return FlashCardAttemptCreate(context, flashCard, status);
            });
        }
    }
}
