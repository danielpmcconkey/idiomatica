using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
namespace Logic
{
    /*
	 * WARNING
	 * due to the way blazor hosts multiple app sessions
	 * in the same process (https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-8.0)
	 * this static class should never persist anything
	 * all functions should have zero side-effects
	 * */

    /// <summary>
    /// StatisticsHelper groups common functions used to derive analytics about users, languages, books, etc.
    /// </summary>
    public static class StatisticsHelper
    {
        #region public interface
        public static string? GetBookStat(List<BookStat> bookStats, AvailableBookStat key)
        {
            if (bookStats == null) return null;
            var existingStat = bookStats.Where(x => x.Key == key).FirstOrDefault();
            if (existingStat == null) return null;
            return existingStat.Value;
        }
        public static string? GetBookStat(Book book, AvailableBookStat key)
        {
            if (book.BookStats == null) return null;
            return GetBookStat(book.BookStats, key);
        }
        public static int GetBookStat_int(Book book, AvailableBookStat key)
        {
            if (book.BookStats == null) return 0;
            return GetBookStat_int(book.BookStats, key);
        }
        public static int GetBookStat_int(List<BookStat> bookStats, AvailableBookStat key)
        {
            if (bookStats == null) return 0;
            var existingStat = GetBookStat(bookStats, key);
            int parsedStat = 0;
            int.TryParse(existingStat, out parsedStat);
            return parsedStat;
        }

        public static string? GetBookUserStat(List<BookUserStat> bookUserStats, AvailableBookUserStat key)
        {
            if (bookUserStats == null) return null;
            var existingStat = bookUserStats.Where(x => x.Key == key).FirstOrDefault();
            if (existingStat == null) return null;
            return existingStat.Value;
        }
        public static string? GetBookUserStat(BookUser bookUser, AvailableBookUserStat key)
        {
            if (bookUser.BookUserStats == null) return null;
            return GetBookUserStat(bookUser.BookUserStats, key);
        }
        public static int GetBookUserStat_int(BookUser bookUser, AvailableBookUserStat key)
        {
            if (bookUser.BookUserStats == null) return 0;
            return GetBookUserStat_int(bookUser.BookUserStats, key);
        }
        public static int GetBookUserStat_int(List<BookUserStat> bookUserStats, AvailableBookUserStat key)
        {
            if (bookUserStats == null) return 0;
            var existingStat = GetBookUserStat(bookUserStats, key);
            int parsedStat = 0;
            int.TryParse(existingStat, out parsedStat);
            return parsedStat;
        }
        public static bool GetBookUserStat_bool(BookUser bookUser, AvailableBookUserStat key)
        {
            if (bookUser.BookUserStats == null) return false;
            return GetBookUserStat_bool(bookUser.BookUserStats, key);
        }
        public static bool GetBookUserStat_bool(List<BookUserStat> bookUserStats, AvailableBookUserStat key)
        {
            if (bookUserStats == null) return false;
            var existingStat = GetBookUserStat(bookUserStats, key);
            bool parsedStat = false;
            bool.TryParse(existingStat, out parsedStat);
            return parsedStat;
        }


        //        /// <summary>
        //        /// Updates the bookstats table for all books for a given user and saves the database
        //        /// </summary>
        //        public static void UpdateAllBookStatsForUserId(IdiomaticaContext context, int userId)
        //        {
        //            List<BookStat> newStatsTotal = new List<BookStat>();
        //            List<BookStat> newStatsDistinct = new List<BookStat>();
        //            List<Book> books = new List<Book>();
        //            newStatsTotal = context.BookStats.FromSqlRaw(FormAllTotalCountsQuery(userId)).ToList();
        //            newStatsDistinct = context.BookStats.FromSqlRaw(FormAllDistinctCountsQuery(userId)).ToList();
        //            books = Fetch.BooksAndBookStatsAndLanguage(
        //                context, (x => x.LanguageUser.UserId == userId)).ToList();

        //            foreach (var book in books)
        //            {
        //                book.BookStats = new List<BookStat>();
        //                List<BookStat> newStatsThisBookTotal = newStatsTotal.Where(x => x.BookId == book.Id).ToList();
        //                List<BookStat> newStatsThisBookDistinct = newStatsDistinct.Where(x => x.BookId == book.Id).ToList();

        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookUserStat.TOTALNEW1COUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookUserStat.TOTALNEW2COUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookUserStat.TOTALLEARNING3COUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookUserStat.TOTALLEARNING4COUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookUserStat.TOTALLEARNEDCOUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookUserStat.TOTALIGNOREDCOUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookUserStat.TOTALWELLKNOWNCOUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookUserStat.TOTALUNKNOWNCOUNT));

        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookUserStat.DISTINCTNEW1COUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookUserStat.DISTINCTNEW2COUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookUserStat.DISTINCTLEARNING3COUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookUserStat.DISTINCTLEARNING4COUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookUserStat.DISTINCTLEARNEDCOUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookUserStat.DISTINCTIGNOREDCOUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookUserStat.DISTINCTWELLKNOWNCOUNT));
        //                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookUserStat.DISTINCTUNKNOWNCOUNT));

        //                // calculated stats
        //                int totalWordCount = newStatsThisBookTotal.Sum(x =>
        //                {
        //                    int parsedInt = 0;
        //                    int.TryParse(x.Value, out parsedInt);
        //                    return parsedInt;
        //                });
        //                book.BookStats.Add(Upsert.BookStat(context, new BookStat()
        //                {
        //                    BookId = book.Id,
        //                    Key = AvailableBookUserStat.TOTALWORDCOUNT,
        //                    Value = totalWordCount.ToString()
        //                }));

        //                int distinctWordCount = newStatsThisBookDistinct.Sum(x =>
        //                {
        //                    int parsedInt = 0;
        //                    int.TryParse(x.Value, out parsedInt);
        //                    return parsedInt;
        //                });
        //                book.BookStats.Add(Upsert.BookStat(context, new BookStat()
        //                {
        //                    BookId = book.Id,
        //                    Key = AvailableBookUserStat.DISTINCTWORDCOUNT,
        //                    Value = distinctWordCount.ToString()
        //                }));

        //                int totalKnownPercent = GetKnownPercent(
        //                    GetBookStat_int(book.BookStats, AvailableBookUserStat.TOTALLEARNEDCOUNT),
        //                    GetBookStat_int(book.BookStats, AvailableBookUserStat.TOTALWELLKNOWNCOUNT),
        //                    GetBookStat_int(book.BookStats, AvailableBookUserStat.TOTALIGNOREDCOUNT),
        //                    totalWordCount
        //                    );
        //                book.BookStats.Add(Upsert.BookStat(context, new BookStat()
        //                {
        //                    BookId = book.Id,
        //                    Key = AvailableBookUserStat.TOTALKNOWNPERCENT,
        //                    Value = totalKnownPercent.ToString()
        //                }));

        //                int distinctKnownPercent = GetKnownPercent(
        //                    GetBookStat_int(book.BookStats, AvailableBookUserStat.DISTINCTLEARNEDCOUNT),
        //                    GetBookStat_int(book.BookStats, AvailableBookUserStat.DISTINCTWELLKNOWNCOUNT),
        //                    GetBookStat_int(book.BookStats, AvailableBookUserStat.DISTINCTIGNOREDCOUNT),
        //                    distinctWordCount
        //                    );
        //                book.BookStats.Add(Upsert.BookStat(context, new BookStat()
        //                {
        //                    BookId = book.Id,
        //                    Key = AvailableBookUserStat.DISTINCTKNOWNPERCENT,
        //                    Value = distinctKnownPercent.ToString()
        //                }));
        //            }
        //        }

        //        public static void UpdateLanguageTotalWordsRead(IdiomaticaContext context, LanguageUser languageUser)
        //        {
        //            var readWords = languageUser.Books
        //                .SelectMany(x => x.Pages)
        //                .Where(p => p.ReadDate is not null)
        //                .Sum(p => PageHelper.GetWordCountOfPage(p, languageUser))
        //                ;
        //            languageUser.TotalWordsRead = readWords;
        //            context.SaveChanges();
        //        }

        #endregion

        //        #region private methods

        //        private static string FormAllDistinctCountsQuery(int userId)
        //        {
        //            string q =
        //$"""
        //            with allDistinctWordsByBook as (
        //	            select 
        //		                b.Id as BookId
        //		            , w.Id as WordId
        //		            , w.Status
        //	            from Book b
        //	            left join Page p on p.BookId = b.Id
        //	            left join Paragraph pp on pp.PageId = p.Id
        //	            left join Sentence s on s.ParagraphId = pp.id
        //	            left join Token t on t.SentenceId = s.Id
        //	            left join Word w on t.WordId = w.Id
        //	            left join LanguageUser lu on b.LanguageUserId = lu.Id
        //	            where lu.UserId = {userId}
        //	            group by 
        //		                b.Id
        //		            , w.Id
        //		            , w.Status
        //            ), distinctCounts as (
        //	            select 
        //		                ad.BookId
        //		            , ad.Status
        //		            , count(ad.WordId) as DistinctCount
        //	            from allDistinctWordsByBook ad
        //	            group by 
        //		            ad.BookId
        //		            , ad.Status
        //            )
        //            select 
        //	                dc.BookId
        //	            , (case
        //                    when dc.Status = {(int)AvailableWordUserStatus.NEW1} then {(int)AvailableBookUserStat.DISTINCTNEW1COUNT}
        //                    when dc.Status = {(int)AvailableWordUserStatus.NEW2} then {(int)AvailableBookUserStat.DISTINCTNEW2COUNT}
        //                    when dc.Status = {(int)AvailableWordUserStatus.LEARNING3} then {(int)AvailableBookUserStat.DISTINCTLEARNING3COUNT}
        //                    when dc.Status = {(int)AvailableWordUserStatus.LEARNING4} then {(int)AvailableBookUserStat.DISTINCTLEARNING4COUNT}
        //                    when dc.Status = {(int)AvailableWordUserStatus.LEARNED} then {(int)AvailableBookUserStat.DISTINCTLEARNEDCOUNT}
        //                    when dc.Status = {(int)AvailableWordUserStatus.IGNORED} then {(int)AvailableBookUserStat.DISTINCTIGNOREDCOUNT}
        //                    when dc.Status = {(int)AvailableWordUserStatus.WELLKNOWN} then {(int)AvailableBookUserStat.DISTINCTWELLKNOWNCOUNT}
        //                    when dc.Status = {(int)AvailableWordUserStatus.UNKNOWN} then {(int)AvailableBookUserStat.DISTINCTUNKNOWNCOUNT}
        //                    end ) as [Key]
        //	            , cast(dc.DistinctCount as varchar(250)) as [Value]
        //            from distinctCounts dc
        //""";
        //            return q;
        //        }
        //        private static string FormAllTotalCountsQuery(int userId)
        //        {
        //            string q =
        //$"""
        //            with allWords as (
        //	            select 
        //		              b.Id as BookId
        //		            , w.Id as WordId
        //		            , w.Status
        //	            from Book b
        //	            left join Page p on p.BookId = b.Id
        //	            left join Paragraph pp on pp.PageId = p.Id
        //	            left join Sentence s on s.ParagraphId = pp.id
        //	            left join Token t on t.SentenceId = s.Id
        //	            left join Word w on t.WordId = w.Id
        //	            left join LanguageUser lu on b.LanguageUserId = lu.Id
        //	            where lu.UserId = {userId}
        //            ), totalCounts as (
        //	            select 
        //		              aw.BookId
        //		            , aw.Status
        //		            , count(aw.WordId) as TotalCount
        //	            from allWords aw
        //	            group by 
        //		              aw.BookId
        //		            , aw.Status
        //            )
        //            select 
        //	              tc.BookId
        //	            , (case
        //		            when tc.Status = {(int)AvailableWordUserStatus.NEW1} then {(int)AvailableBookUserStat.TOTALNEW1COUNT}
        //		            when tc.Status = {(int)AvailableWordUserStatus.NEW2} then {(int)AvailableBookUserStat.TOTALNEW2COUNT}
        //		            when tc.Status = {(int)AvailableWordUserStatus.LEARNING3} then {(int)AvailableBookUserStat.TOTALLEARNING3COUNT}
        //		            when tc.Status = {(int)AvailableWordUserStatus.LEARNING4} then {(int)AvailableBookUserStat.TOTALLEARNING4COUNT}
        //		            when tc.Status = {(int)AvailableWordUserStatus.LEARNED} then {(int)AvailableBookUserStat.TOTALLEARNEDCOUNT}
        //		            when tc.Status = {(int)AvailableWordUserStatus.IGNORED} then {(int)AvailableBookUserStat.TOTALIGNOREDCOUNT}
        //		            when tc.Status = {(int)AvailableWordUserStatus.WELLKNOWN} then {(int)AvailableBookUserStat.TOTALWELLKNOWNCOUNT}
        //		            when tc.Status = {(int)AvailableWordUserStatus.UNKNOWN} then {(int)AvailableBookUserStat.TOTALUNKNOWNCOUNT}
        //		            end ) as [Key]
        //	            , cast(tc.TotalCount as varchar(250)) as [Value]
        //            from totalCounts tc
        //""";
        //            return q;
        //        }
        //        private static int GetKnownPercent(int? learnedCount, int? wellknownCount,
        //            int? ignoredCount, int? wordCount)
        //        {
        //            if (wordCount == 0) return 0;

        //            int totalKnown = (int)learnedCount + (int)wellknownCount + (int)ignoredCount;
        //            if (totalKnown == 0) return 0;

        //            decimal percentKnown = (decimal)totalKnown / (decimal)wordCount;
        //            return (int)Math.Round(percentKnown * 100M, 0);

        //        }
        //        private static BookStat UpdateOrCreateDefaultStat(IdiomaticaContext context, 
        //            Book book, List<BookStat> newStats, AvailableBookUserStat key, string defaultValue = "0")
        //        {
        //            // if it's in the new stats, upsert it
        //            // if it's not in the new stats, set default value then upsert it
        //            var bs = newStats.Where(x => x.Key == key).FirstOrDefault();
        //            if (bs != null)
        //            {
        //                return Upsert.BookStat(context, bs);
        //            }

        //            return Upsert.BookStat(context, new BookStat()
        //            {
        //                BookId = book.Id,
        //                Key = key,
        //                Value = defaultValue
        //            });
        //        }

        //        #endregion
    }
}
