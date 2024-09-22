using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Model.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        

        #region create

        public static FlashCard? FlashCardCreate(FlashCard flashCard, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            context.FlashCards.Add(flashCard);
            context.SaveChanges();
            // add it to cache
            FlashCardById[flashCard.Id] = flashCard; ;

            return flashCard;
        }
        public static async Task<FlashCard?> FlashCardCreateAsync(FlashCard value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return FlashCardCreate(value, dbContextFactory); });
        }



        #endregion

        #region read
        public static FlashCard? FlashCardByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (FlashCardById.ContainsKey(key))
            {
                return FlashCardById[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.FlashCards.Where(x => x.Id == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardById[key] = value;
            return value;
        }
        public static async Task<FlashCard?> FlashCardByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardByIdRead(key, dbContextFactory);
            });
        }

        public static FlashCard? FlashCardByWordUserIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (FlashCardByWordUserId.ContainsKey(key))
            {
                return FlashCardByWordUserId[key];
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            var value = context.FlashCards.Where(x => x.WordUserId == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardByWordUserId[key] = value;
            return value;
        }
        public static async Task<FlashCard?> FlashCardByWordUserIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardByWordUserIdRead(key, dbContextFactory);
            });
        }


        public static FlashCard? FlashCardAndFullRelationshipsByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // check cache
            if (FlashCardAndFullRelationshipsById.TryGetValue(key, out FlashCard? value))
            {
                return value;
            }
            var context = dbContextFactory.CreateDbContext();


            // read DB
            value = context.FlashCards.Where(x => x.Id == key)
                .Include(fc => fc.WordUser)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    .ThenInclude(wu => wu.Word)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .Include(fc => fc.Attempts)
                .Include(fc => fc.FlashCardParagraphTranslationBridges)
                    .ThenInclude(fcptb => fcptb.ParagraphTranslation)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        .ThenInclude(pt => pt.Paragraph).ThenInclude(pp => pp.Sentences)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardById[key] = value;
            return value;
        }
        public static async Task<FlashCard?> FlashCardAndFullRelationshipsByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardAndFullRelationshipsByIdRead(key, dbContextFactory);
            });
        }
        public static List<FlashCard>? FlashCardsActiveAndFullRelationshipsByPredicateRead(
            Expression<Func<FlashCard, bool>> predicate, int take, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            // don't check cache here. The statuses change and new cards get
            // created for the first time while this cache thinks there's a
            // null for the deck

            // read DB
            var value = context.FlashCards
                .Where(predicate)
                .Include(fc => fc.WordUser)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    .ThenInclude(wu => wu.Word)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .Include(fc => fc.Attempts)
                .Include(fc => fc.FlashCardParagraphTranslationBridges)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    .ThenInclude(fcptb => fcptb.ParagraphTranslation)
                        .ThenInclude(pt => pt.Paragraph)
                            .ThenInclude(pp => pp.Sentences)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                .OrderBy(fc => fc.NextReview)
                .Take(take)
                .ToList();
            if (value is null) return value;
            // write to cache
            //FlashCardsActiveAndFullRelationshipsByLanguageUserId[key] = value;
            foreach (var f in value)
            {
                FlashCardUpdateAllCaches(f);
            }
            return value;
        }


        public static FlashCard? FlashCardNextReviewCardRead(Guid userId,
            AvailableLanguageCode learningLanguageCode,
            IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            // do not cache this as it'll constantly change
            var context = dbContextFactory.CreateDbContext();

            // try to get a card whose next review is here or next review is
            // empty (meaning never reviewed)
            return context.FlashCards.Where(x =>
                x.WordUser != null &&
                x.WordUser.LanguageUser != null &&
                x.WordUser.LanguageUser.Language != null &&
                x.WordUser.LanguageUser.Language.Code == learningLanguageCode &&
                x.WordUser.LanguageUser.UserId == userId &&
                (x.NextReview == null || x.NextReview <= DateTimeOffset.Now)
            )
            .OrderBy(x => x.NextReview)
            .FirstOrDefault();
            // it's okay to return empty. The API above this should create a
            // new card if that's the case
        }
        public static async Task<FlashCard?> FlashCardNextReviewCardReadAsync(Guid userId,
            AvailableLanguageCode learningLanguageCode,
            IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardNextReviewCardRead(userId, learningLanguageCode,
                    dbContextFactory);
            });
        }

        #endregion

        #region update

        public static void FlashCardUpdate(FlashCard flashCard, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();
            context.FlashCards.Update(flashCard);
            context.SaveChanges();

            //int numRows = context.Database.ExecuteSql($"""
            //            UPDATE [Idioma].[FlashCard]
            //            SET [WordUserId] = {flashCard.WordUserId}
            //               ,[Status] = {flashCard.Status}
            //               ,[NextReview] = {flashCard.NextReview}
            //               ,[Id] = {flashCard.Id}
            //            where Id = {flashCard.Id}
            //            ;
            //            """);
            //if (numRows < 1)
            //{
            //    throw new InvalidDataException("FlashCard update affected 0 rows");
            //};
            // now update the cache
            FlashCardUpdateAllCaches(flashCard);

            return;
        }
        public static async Task FlashCardUpdateAsync(FlashCard value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            await Task.Run(() =>
            {
                FlashCardUpdate(value, dbContextFactory);
            });
        }



        
        #endregion

        private static bool doesFlashCardListContainId(List<FlashCard> list, Guid key)
        {
            return list.Where(x => x.Id == key).Any();
        }
        private static List<FlashCard> FlashCardsListGetUpdated(List<FlashCard> list, FlashCard value)
        {
            List<FlashCard> newList = new List<FlashCard>();
            foreach (var fc in list)
            {
                if (fc.Id == value.Id) newList.Add(value);
                else newList.Add(fc);
            }
            return newList;
        }
        private static void FlashCardUpdateAllCaches(FlashCard value)
        {
            FlashCardById[(Guid)value.Id] = value;

            // FlashCardAndFullRelationshipsById
            FlashCard? cachedFull = null;
            FlashCardAndFullRelationshipsById.TryGetValue((Guid)value.Id, out cachedFull);
            if (cachedFull != null)
            {
                cachedFull.NextReview = value.NextReview;
                cachedFull.WordUserId = value.WordUserId;
                cachedFull.Status = value.Status;
            }
            
            return;
        }
    }
}
