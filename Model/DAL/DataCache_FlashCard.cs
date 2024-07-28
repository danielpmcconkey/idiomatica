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
        private static ConcurrentDictionary<int, FlashCard?> FlashCardById = new ConcurrentDictionary<int, FlashCard?>();
        private static ConcurrentDictionary<int, FlashCard?> FlashCardAndFullRelationshipsById = new ConcurrentDictionary<int, FlashCard?>();
        private static ConcurrentDictionary<(int languageUserId, int take), List<FlashCard>> FlashCardsActiveAndFullRelationshipsByLanguageUserId = new ConcurrentDictionary<(int languageUserId, int take), List<FlashCard>>();


        #region create

        public static FlashCard? FlashCardCreate(FlashCard flashCard, IdiomaticaContext context)
        {
            if (flashCard.WordUserId is null) throw new ArgumentNullException(nameof(flashCard.WordUserId));
            
            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        
                INSERT INTO [Idioma].[FlashCard]
                      ([WordUserId]
                      ,[Status]
                      ,[NextReview]
                      ,[UniqueKey])
                VALUES
                      ({flashCard.WordUserId}
                      ,{flashCard.Status}
                      ,{flashCard.NextReview}
                      ,{guid})
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating FlashCard affected 0 rows");
            var newEntity = context.FlashCards.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in FlashCardCreate");
            }


            // add it to cache
            FlashCardById[(int)newEntity.Id] = newEntity; ;

            return newEntity;
        }
        public static async Task<FlashCard?> FlashCardCreateAsync(FlashCard value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return FlashCardCreate(value, context); });
        }


        
        #endregion

        #region read
        public static async Task<FlashCard?> FlashCardByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardById.ContainsKey(key))
            {
                return FlashCardById[key];
            }

            // read DB
            var value = context.FlashCards.Where(x => x.Id == key).FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardById[key] = value;
            return value;
        }
        public static async Task<FlashCard?> FlashCardAndFullRelationshipsByIdReadAsync(int key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardAndFullRelationshipsById.ContainsKey(key))
            {
                return FlashCardAndFullRelationshipsById[key];
            }

            // read DB
            var value = context.FlashCards.Where(x => x.Id == key)
                .Include(fc => fc.WordUser).ThenInclude(wu => wu.Word)
                .Include(fc => fc.Attempts)
                .Include(fc => fc.FlashCardParagraphTranslationBridges)
                    .ThenInclude(fcptb => fcptb.ParagraphTranslation)
#nullable disable
                        .ThenInclude(pt => pt.Paragraph).ThenInclude(pp => pp.Sentences)
#nullable restore
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            FlashCardById[key] = value;
            return value;
        }
        public static async Task<List<FlashCard>> FlashCardsActiveAndFullRelationshipsByLanguageUserIdReadAsync(
            (int languageUserId, int take) key, IdiomaticaContext context)
        {
            // check cache
            if (FlashCardsActiveAndFullRelationshipsByLanguageUserId.ContainsKey(key))
            {
                return FlashCardsActiveAndFullRelationshipsByLanguageUserId[key];
            }

            // read DB
            var value = context.FlashCards
                .Where(fc => fc.WordUser.LanguageUserId == key.languageUserId
                    && fc.Status == AvailableFlashCardStatus.ACTIVE)
                .Include(fc => fc.WordUser).ThenInclude(wu => wu.Word)
                .Include(fc => fc.Attempts)
                .Include(fc => fc.FlashCardParagraphTranslationBridges)
                    .ThenInclude(fcptb => fcptb.ParagraphTranslation)
                        .ThenInclude(pt => pt.Paragraph).ThenInclude(pp => pp.Sentences)
                .OrderBy(fc => fc.NextReview)
                .Take(key.take)
                .ToList();
            if (value == null) return new List<FlashCard>();
            // write to cache
            FlashCardsActiveAndFullRelationshipsByLanguageUserId[key] = value;
            foreach (var f in value)
            {
                FlashCardUpdateAllCaches(f, key);
            }
            return value;
        }

        #endregion

        #region update

        public static void FlashCardUpdate(FlashCard flashCard, IdiomaticaContext context)
        {
            if (flashCard.Id == null || flashCard.Id < 1)
                throw new ArgumentException("ID cannot be null or 0 when updating FlashCard");

            int numRows = context.Database.ExecuteSql($"""
                        UPDATE [Idioma].[FlashCard]
                        SET [WordUserId] = {flashCard.WordUserId}
                           ,[Status] = {flashCard.Status}
                           ,[NextReview] = {flashCard.NextReview}
                           ,[UniqueKey] = {flashCard.UniqueKey}
                        where Id = {flashCard.Id}
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

        private static bool doesFlashCardListContainId(List<FlashCard> list, int key)
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
        private static void FlashCardUpdateAllCaches(FlashCard value, (int languageUserId, int take)? keyLanguageUser = null)
        {
            if (value.Id is null) throw new ArgumentNullException(nameof(value));

            FlashCardById[(int)value.Id] = value;

            // FlashCardAndFullRelationshipsById
            FlashCard? cachedFull = null;
            FlashCardAndFullRelationshipsById.TryGetValue((int)value.Id, out cachedFull);
            if (cachedFull != null)
            {
                cachedFull.NextReview = value.NextReview;
                cachedFull.WordUserId = value.WordUserId;
                cachedFull.Status = value.Status;
            }
            if(keyLanguageUser != null)
            {
                // FlashCardsActiveAndFullRelationshipsByLanguageUserId
                var cachedList2 = FlashCardsActiveAndFullRelationshipsByLanguageUserId
                    .Where(x => doesFlashCardListContainId(x.Value, (int)value.Id)).ToArray();
                for (int i = 0; i < cachedList2.Length; i++)
                {
                    var item = cachedList2[i];
                    var newList = FlashCardsListGetUpdated(item.Value, value);
                    FlashCardsActiveAndFullRelationshipsByLanguageUserId[item.Key] = newList;
                }
            }
            return;
        }
    }
}
