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
        public static async Task<List<BookUserStat>?> BookUserStatsReadAsync(
            IdiomaticaContext context, int bookId, int userId)
        {
            if (bookId < 1) ErrorHandler.LogAndThrow();
            if (userId < 1) ErrorHandler.LogAndThrow();
            return await DataCache.BookUserStatsByBookIdAndUserIdReadAsync(
                    (bookId, userId), context);
        }
        public static async Task BookUserStatsUpdateByBookUserIdAsync(IdiomaticaContext context, int bookUserId)
        {
            var bookUser = await DataCache.BookUserByIdReadAsync(bookUserId, context);

            if (bookUser == null || bookUser.LanguageUserId == null || bookUser.LanguageUserId < 1
                || bookUser.BookId == null || bookUser.BookId < 1)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            var languageUser = await DataCache.LanguageUserByIdReadAsync((int)bookUser.LanguageUserId, context);
            var allWordsInBook = await DataCache.WordsByBookIdReadAsync((int)bookUser.BookId, context);
            var allWordUsersInBook = await DataCache.WordUsersByBookIdAndLanguageUserIdReadAsync(
                ((int)bookUser.BookId, (int)bookUser.LanguageUserId), context);
            var pageUsers = await DataCache.PageUsersByBookUserIdReadAsync(bookUserId, context);
            var pages = await DataCache.PagesByBookIdReadAsync(bookUserId, context);
            var bookStats = await DataCache.BookStatsByBookIdReadAsync((int)bookUser.BookId, context);
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
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.LASTPAGEREAD,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = lastPageRead,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESS,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = progress,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESSPERCENT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = progressPercent,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTKNOWNPERCENT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = distinctKnownPercent,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTWORDCOUNT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = distinctWordCount,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALWORDCOUNT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = totalWordCount,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALKNOWNPERCENT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = null,
                ValueNumeric = null,
            }); ;

            // delete the booklistrows cache for this user
            if (languageUser != null && languageUser.Id != null && languageUser.UserId != null)
            {
                DataCache.BookListRowsByUserIdDelete((int)languageUser.Id, context);
                // delete the actual stats from teh database and bookuserstats cache
                await DataCache.BookUserStatsByBookIdAndUserIdDelete(((int)bookUser.BookId, (int)languageUser.UserId), context);
                // add the new stats
                await DataCache.BookUserStatsByBookIdAndUserIdCreate(
                    ((int)bookUser.BookId, (int)languageUser.UserId), stats, context);
            }

        }
    }
}
