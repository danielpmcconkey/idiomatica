using Microsoft.EntityFrameworkCore;
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
        private static ConcurrentDictionary<Guid, FlashCard?> FlashCardById = new ConcurrentDictionary<Guid, FlashCard?>();
        private static ConcurrentDictionary<Guid, FlashCard?> FlashCardAndFullRelationshipsById = new ConcurrentDictionary<Guid, FlashCard?>();
        private static ConcurrentDictionary<Guid, FlashCard?> FlashCardByWordUserId = new ();
        //private static ConcurrentDictionary<(Guid languageUserId, int take), List<FlashCard>> FlashCardsActiveAndFullRelationshipsByLanguageUserId = new ();


        #region create

        public static FlashCard? FlashCardCreate(FlashCard flashCard, IdiomaticaContext context)
        {
            if (flashCard.WordUserKey is null) throw new ArgumentNullException(nameof(flashCard.WordUserKey));
            
            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[FlashCard]
                      ([WordUserKey]
                      ,[Status]
                      ,[NextReview]
                      ,[UniqueKey])
                VALUES
                      ({flashCard.WordUserKey}
                      ,{flashCard.Status}
                      ,{flashCard.NextReview}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating FlashCard affected 0 rows");
            var newEntity = context.FlashCards.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.UniqueKey is null)
            {
                throw new InvalidDataException("newEntity is null in FlashCardCreate");
            }


            // add it to cache
            FlashCardById[(Guid)newEntity.UniqueKey] = newEntity; ;

            return newEntity;
        }
        public static async Task<FlashCard?> FlashCardCreateAsync(FlashCard value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return FlashCardCreate(value, context); });
        }



        #endregion

        #region read
        public static FlashCard? FlashCardByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardById.ContainsKey(key))
            {
                return FlashCardById[key];
            }

            // read DB
            var value = context.FlashCards.Where(x => x.UniqueKey == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardById[key] = value;
            return value;
        }
        public static async Task<FlashCard?> FlashCardByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardByIdRead(key, context);
            });
        }

        public static FlashCard? FlashCardByWordUserIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardByWordUserId.ContainsKey(key))
            {
                return FlashCardByWordUserId[key];
            }

            // read DB
            var value = context.FlashCards.Where(x => x.WordUserKey == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardByWordUserId[key] = value;
            return value;
        }
        public static async Task<FlashCard?> FlashCardByWordUserIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardByWordUserIdRead(key, context);
            });
        }


        public static FlashCard? FlashCardAndFullRelationshipsByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardAndFullRelationshipsById.TryGetValue(key, out FlashCard? value))
            {
                return value;
            }

            // read DB
            value = context.FlashCards.Where(x => x.UniqueKey == key)
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
        public static async Task<FlashCard?> FlashCardAndFullRelationshipsByIdReadAsync(Guid key, IdiomaticaContext context)
        {
            return await Task<FlashCard?>.Run(() =>
            {
                return FlashCardAndFullRelationshipsByIdRead(key, context);
            });
        }
        public static List<FlashCard>? FlashCardsActiveAndFullRelationshipsByPredicateRead(
            Expression<Func<FlashCard, bool>> predicate, int take, IdiomaticaContext context)
        {
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

        #endregion

        #region update

        public static void FlashCardUpdate(FlashCard flashCard, IdiomaticaContext context)
        {
            if (flashCard.UniqueKey == null)
                throw new ArgumentException("ID cannot be null or 0 when updating FlashCard");

            int numRows = context.Database.ExecuteSql($"""
                        UPDATE [Idioma].[FlashCard]
                        SET [WordUserKey] = {flashCard.WordUserKey}
                           ,[Status] = {flashCard.Status}
                           ,[NextReview] = {flashCard.NextReview}
                           ,[UniqueKey] = {flashCard.UniqueKey}
                        where UniqueKey = {flashCard.UniqueKey}
                        ;
                        """);
            if (numRows < 1)
            {
                throw new InvalidDataException("FlashCard update affected 0 rows");
            };
            // now update the cache
            FlashCardUpdateAllCaches(flashCard);

            return;
        }
        public static async Task FlashCardUpdateAsync(FlashCard value, IdiomaticaContext context)
        {
            await Task.Run(() =>
            {
                FlashCardUpdate(value, context);
            });
        }



        
        #endregion

        private static bool doesFlashCardListContainId(List<FlashCard> list, Guid key)
        {
            return list.Where(x => x.UniqueKey == key).Any();
        }
        private static List<FlashCard> FlashCardsListGetUpdated(List<FlashCard> list, FlashCard value)
        {
            List<FlashCard> newList = new List<FlashCard>();
            foreach (var fc in list)
            {
                if (fc.UniqueKey == value.UniqueKey) newList.Add(value);
                else newList.Add(fc);
            }
            return newList;
        }
        private static void FlashCardUpdateAllCaches(FlashCard value)
        {
            if (value.UniqueKey is null) throw new ArgumentNullException(nameof(value));

            FlashCardById[(Guid)value.UniqueKey] = value;

            // FlashCardAndFullRelationshipsById
            FlashCard? cachedFull = null;
            FlashCardAndFullRelationshipsById.TryGetValue((Guid)value.UniqueKey, out cachedFull);
            if (cachedFull != null)
            {
                cachedFull.NextReview = value.NextReview;
                cachedFull.WordUserKey = value.WordUserKey;
                cachedFull.Status = value.Status;
            }
            
            return;
        }
    }
}
