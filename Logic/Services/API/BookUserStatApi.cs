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
    public static class BookUserStatApi
    {
        public static List<BookUserStat>? BookUserStatsRead(
            IdiomaticaContext context, Guid bookId, Guid userId)
        {
            return DataCache.BookUserStatsByBookIdAndUserIdRead(
                    (bookId, userId), context);
        }
        public static async Task<List<BookUserStat>?> BookUserStatsReadAsync(
            IdiomaticaContext context, Guid bookId, Guid userId)
        {
            return await DataCache.BookUserStatsByBookIdAndUserIdReadAsync(
                    (bookId, userId), context);
        }


        public static void BookUserStatsUpdateByBookUserId(IdiomaticaContext context, Guid bookUserId)
        {
            var bookUser = DataCache.BookUserByIdRead(bookUserId, context);

            if (bookUser == null || bookUser.LanguageUserKey == null
                || bookUser.BookKey == null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            var languageUser = DataCache.LanguageUserByIdRead((Guid)bookUser.LanguageUserKey, context);
            if (languageUser is null || languageUser.UniqueKey is null || languageUser.UserKey is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            
            // delete the booklistrows cache for this user
            DataCache.BookListRowsByUserIdDelete((Guid)languageUser.UniqueKey, context);
            // delete the actual stats from teh database and bookuserstats cache
            DataCache.BookUserStatsByBookIdAndUserIdDelete(((Guid)bookUser.BookKey, (Guid)languageUser.UserKey), context);

            var allWordsInBook = DataCache.WordsByBookIdRead((Guid)bookUser.BookKey, context);
           
            var allWordUsersInBook = DataCache.WordUsersByBookIdAndLanguageUserIdRead(
                ((Guid)bookUser.BookKey, (Guid)bookUser.LanguageUserKey), context, true);
            var pageUsers = DataCache.PageUsersByBookUserIdRead(bookUserId, context, true);
            var pages = DataCache.PagesByBookIdRead((Guid)bookUser.BookKey, context);
            var bookStats = DataCache.BookStatsByBookIdRead((Guid)bookUser.BookKey, context);
            if (bookStats is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }

            var totalPagesStatline = bookStats.Where(x => x.Key == AvailableBookStat.TOTALPAGES).FirstOrDefault();
            var totalWordsStatline = bookStats.Where(x => x.Key == AvailableBookStat.TOTALWORDCOUNT).FirstOrDefault();
            var totalDistinctWordsStatline = bookStats.Where(x => x.Key == AvailableBookStat.DISTINCTWORDCOUNT).FirstOrDefault();
            var readPages = (from pu in pageUsers
                             join p in pages on pu.PageKey equals p.UniqueKey
                             where pu.ReadDate != null
                             orderby p.Ordinal descending
                             select p)
                            .ToList();

            //pageUsers.Where(x => x.ReadDate != null).OrderByDescending(x => x.or);
            int totalPages = 0;
            bool isComplete = false;

            // is complete
            if (totalPagesStatline != null)
            {
                int.TryParse(totalPagesStatline.Value, out totalPages);
                isComplete = (readPages.Count() == totalPages) ? true : false;
            }

            // last page read
            int lastPageRead = 0;
            var latestReadPage = readPages.FirstOrDefault();
            if ((int)readPages.Count > 0 && latestReadPage != null && latestReadPage.Ordinal != null)
                lastPageRead = (int)latestReadPage.Ordinal;

            // progress
            string progress = $"{lastPageRead} / {totalPages}";

            // progress percent
            decimal progressPercent = (totalPages == 0) ? 0 : lastPageRead / (decimal)totalPages;

            // AvailableBookUserStat.TOTALWORDCOUNT
            int totalWordCount = 0;
            if (totalWordsStatline != null)
            {
                int.TryParse(totalWordsStatline.Value, out totalWordCount);
            }

            // AvailableBookUserStat.DISTINCTWORDCOUNT
            int distinctWordCount = allWordsInBook.Count;

            // AvailableBookUserStat.DISTINCTKNOWNPERCENT
            int knownDistinct = allWordUsersInBook
                .Where(x =>
                    x.Status == AvailableWordUserStatus.WELLKNOWN ||
                    x.Status == AvailableWordUserStatus.LEARNED ||
                    x.Status == AvailableWordUserStatus.IGNORED)
                .Count();

            decimal distinctKnownPercent = knownDistinct / (decimal)distinctWordCount;


            // AvailableBookUserStat.TOTALKNOWNPERCENT
            List<BookUserStat> stats = new List<BookUserStat>();
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.ISCOMPLETE,
                BookKey = bookUser.BookKey,
                LanguageUserKey = bookUser.LanguageUserKey,
                ValueString = isComplete.ToString(),
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.LASTPAGEREAD,
                BookKey = bookUser.BookKey,
                LanguageUserKey = bookUser.LanguageUserKey,
                ValueNumeric = lastPageRead,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESS,
                BookKey = bookUser.BookKey,
                LanguageUserKey = bookUser.LanguageUserKey,
                ValueString = progress,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESSPERCENT,
                BookKey = bookUser.BookKey,
                LanguageUserKey = bookUser.LanguageUserKey,
                ValueNumeric = progressPercent,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTKNOWNPERCENT,
                BookKey = bookUser.BookKey,
                LanguageUserKey = bookUser.LanguageUserKey,
                ValueNumeric = distinctKnownPercent,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTWORDCOUNT,
                BookKey = bookUser.BookKey,
                LanguageUserKey = bookUser.LanguageUserKey,
                ValueNumeric = distinctWordCount,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALWORDCOUNT,
                BookKey = bookUser.BookKey,
                LanguageUserKey = bookUser.LanguageUserKey,
                ValueNumeric = totalWordCount,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALKNOWNPERCENT,
                BookKey = bookUser.BookKey,
                LanguageUserKey = bookUser.LanguageUserKey,
                ValueString = null,
                ValueNumeric = null,
            });

            
            // add the new stats
            DataCache.BookUserStatsByBookIdAndUserIdCreate(
                ((Guid)bookUser.BookKey, (Guid)languageUser.UserKey), stats, context);
            

        }
        public static async Task BookUserStatsUpdateByBookUserIdAsync(IdiomaticaContext context, Guid bookUserId)
        {
            await Task.Run(() =>
            {
                BookUserStatsUpdateByBookUserId(context, bookUserId);
            });
        }
    }
}
