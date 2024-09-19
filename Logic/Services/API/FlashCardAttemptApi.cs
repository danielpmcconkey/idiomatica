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
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class FlashCardAttemptApi
    {
        public static FlashCardAttempt? FlashCardAttemptCreate(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, FlashCard flashCard, AvailableFlashCardAttemptStatus status)
        {
            FlashCardAttempt? attempt = new ()
            {
                Id = Guid.NewGuid(),
                FlashCardId = flashCard.Id,
                FlashCard = flashCard,
                AttemptedWhen = DateTime.Now,
                Status = status,
            };
            attempt = DataCache.FlashCardAttemptCreate(attempt, dbContextFactory);
            if (attempt is null)
            {
                ErrorHandler.LogAndThrow();
                return null;
            }
            return attempt;
        }
        public static async Task<FlashCardAttempt?> FlashCardAttemptCreateAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, FlashCard flashCard, AvailableFlashCardAttemptStatus status)
        {
            return await Task<FlashCardAttempt?>.Run(() =>
            {
                return FlashCardAttemptCreate(dbContextFactory, flashCard, status);
            });
        }
    }
}
