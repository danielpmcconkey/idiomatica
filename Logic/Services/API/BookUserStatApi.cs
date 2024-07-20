using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services.API
{
    public static class BookUserStatApi
    {
        public static List<BookUserStat>? BookUserStatsRead(
            IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            return DataCache.BookUserStatsByBookIdAndUserIdRead(
                    (bookId, userId), context);
        }
        public static async Task<List<BookUserStat>?> BookUserStatsReadAsync(
            IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.BookUserStatsByBookIdAndUserIdReadAsync(
                    (bookId, userId), context);
        }
        public static void BookUserStatsUpdateByBookUserId(IdiomaticaContext context, int bookUserId)
        {
            var bookUser = DataCache.BookUserByIdRead(bookUserId, context);

            if (bookUser == null || bookUser.LanguageUserId == null || bookUser.LanguageUserId < 1
                || bookUser.BookId == null || bookUser.BookId < 1)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            var languageUser = DataCache.LanguageUserByIdRead((int)bookUser.LanguageUserId, context);
            if (languageUser is null || languageUser.Id is null || languageUser.UserId is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            
            // delete the booklistrows cache for this user
            DataCache.BookListRowsByUserIdDelete((int)languageUser.Id, context);
            // delete the actual stats from teh database and bookuserstats cache
            DataCache.BookUserStatsByBookIdAndUserIdDelete(((int)bookUser.BookId, (int)languageUser.UserId), context);

            var allWordsInBook = DataCache.WordsByBookIdRead((int)bookUser.BookId, context);
           
            var allWordUsersInBook = DataCache.WordUsersByBookIdAndLanguageUserIdRead(
                ((int)bookUser.BookId, (int)bookUser.LanguageUserId), context, true);
            var pageUsers = DataCache.PageUsersByBookUserIdRead(bookUserId, context);
            var pages = DataCache.PagesByBookIdRead(bookUserId, context);
            var bookStats = DataCache.BookStatsByBookIdRead((int)bookUser.BookId, context);
            if (bookStats is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }

            var totalPagesStatline = bookStats.Where(x => x.Key == AvailableBookStat.TOTALPAGES).FirstOrDefault();
            var totalWordsStatline = bookStats.Where(x => x.Key == AvailableBookStat.TOTALWORDCOUNT).FirstOrDefault();
            var totalDistinctWordsStatline = bookStats.Where(x => x.Key == AvailableBookStat.DISTINCTWORDCOUNT).FirstOrDefault();
            var readPages = (from pu in pageUsers
                             join p in pages on pu.PageId equals p.Id
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
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = isComplete.ToString(),
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.LASTPAGEREAD,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = lastPageRead,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESS,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = progress,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESSPERCENT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = progressPercent,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTKNOWNPERCENT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = distinctKnownPercent,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTWORDCOUNT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = distinctWordCount,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALWORDCOUNT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = totalWordCount,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALKNOWNPERCENT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = null,
                ValueNumeric = null,
            });

            
            // add the new stats
            DataCache.BookUserStatsByBookIdAndUserIdCreate(
                ((int)bookUser.BookId, (int)languageUser.UserId), stats, context);
            

        }
    }
}
