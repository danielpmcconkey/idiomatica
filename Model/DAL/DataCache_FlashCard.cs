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
        public static async Task<bool> FlashCardCreateAsync(FlashCard value, IdiomaticaContext context)
        {
            context.FlashCards.Add(value);
            await context.SaveChangesAsync();
            if (value.Id == null || value.Id == 0)
            {
                return false;
            }
            FlashCardById[(int)value.Id] = value;
            return true;
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
            var value = await context.FlashCards.Where(x => x.Id == key).FirstOrDefaultAsync();
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
            var value = await context.FlashCards.Where(x => x.Id == key)
                .Include(fc => fc.WordUser).ThenInclude(wu => wu.Word)
                .Include(fc => fc.Attempts)
                .Include(fc => fc.FlashCardParagraphTranslationBridges)
                    .ThenInclude(fcptb => fcptb.ParagraphTranslation)
                        .ThenInclude(pt => pt.Paragraph).ThenInclude(pp => pp.Sentences)
                .FirstOrDefaultAsync();
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
            var value = await context.FlashCards
                .Where(fc => fc.WordUser.LanguageUserId == key.languageUserId
                    && fc.Status == AvailableFlashCardStatus.ACTIVE)
                .Include(fc => fc.WordUser).ThenInclude(wu => wu.Word)
                .Include(fc => fc.Attempts)
                .Include(fc => fc.FlashCardParagraphTranslationBridges)
                    .ThenInclude(fcptb => fcptb.ParagraphTranslation)
                        .ThenInclude(pt => pt.Paragraph).ThenInclude(pp => pp.Sentences)
                .OrderBy(fc => fc.NextReview)
                .Take(key.take)
                .ToListAsync();
            if (value == null) return new List<FlashCard>();
            // write to cache
            FlashCardsActiveAndFullRelationshipsByLanguageUserId[key] = value;
            foreach (var f in value)
            {
                await FlashCardUpdateAllCachesAsync(f, key);
            }
            return value;
        }

        #endregion

        #region update
        public static async Task FlashCardUpdateAsync(FlashCard value, IdiomaticaContext context)
        {
            if (value.Id == null || value.Id < 1) throw new ArgumentException("ID cannot be null or 0 when updating");

            var valueFromDb = await context.FlashCards.Where(pu => pu.Id == value.Id).FirstOrDefaultAsync();
            if (valueFromDb == null) throw new InvalidDataException("Value does not exist in the DB to update");

            valueFromDb.NextReview = value.NextReview;
            valueFromDb.WordUserId = value.WordUserId;
            valueFromDb.Status = value.Status;
            await context.SaveChangesAsync();

            await FlashCardUpdateAllCachesAsync(value);

            return;
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
        private static async Task FlashCardUpdateAllCachesAsync(FlashCard value, (int languageUserId, int take)? keyLanguageUser = null)
        {
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
