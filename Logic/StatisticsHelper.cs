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
        /// <summary>
        /// Updates the bookstats table for all books for a given user and saves the database
        /// </summary>
        public static void UpdateAllBookStatsForUserId(IdiomaticaContext context, int userId)
        {
            List<BookStat> newStatsTotal = new List<BookStat>();
            List<BookStat> newStatsDistinct = new List<BookStat>();
            List<Book> books = new List<Book>();
            newStatsTotal = context.BookStats.FromSqlRaw(FormAllTotalCountsQuery(userId)).ToList();
            newStatsDistinct = context.BookStats.FromSqlRaw(FormAllDistinctCountsQuery(userId)).ToList();
            books = Fetch.BooksAndBookStatsAndLanguage(
                context, (x => x.LanguageUser.UserId == userId)).ToList();
            
            foreach (var book in books)
            {
                book.BookStats = new List<BookStat>();
                List<BookStat> newStatsThisBookTotal = newStatsTotal.Where(x => x.BookId == book.Id).ToList();
                List<BookStat> newStatsThisBookDistinct = newStatsDistinct.Where(x => x.BookId == book.Id).ToList();
                
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookStat.TOTALNEW1COUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookStat.TOTALNEW2COUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookStat.TOTALLEARNING3COUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookStat.TOTALLEARNING4COUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookStat.TOTALLEARNEDCOUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookStat.TOTALIGNOREDCOUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookStat.TOTALWELLKNOWNCOUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookTotal, AvailableBookStat.TOTALUNKNOWNCOUNT));
                
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookStat.DISTINCTNEW1COUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookStat.DISTINCTNEW2COUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookStat.DISTINCTLEARNING3COUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookStat.DISTINCTLEARNING4COUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookStat.DISTINCTLEARNEDCOUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookStat.DISTINCTIGNOREDCOUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookStat.DISTINCTWELLKNOWNCOUNT));
                book.BookStats.Add(UpdateOrCreateDefaultStat(context, book, newStatsThisBookDistinct, AvailableBookStat.DISTINCTUNKNOWNCOUNT));

                // calculated stats
                int totalWordCount = newStatsThisBookTotal.Sum(x =>
                {
                    int parsedInt = 0;
                    int.TryParse(x.Value, out parsedInt);
                    return parsedInt;
                });
                book.BookStats.Add(Upsert.BookStat(context, new BookStat()
                {
                    BookId = book.Id,
                    Key = AvailableBookStat.TOTALWORDCOUNT,
                    Value = totalWordCount.ToString()
                }));

                int distinctWordCount = newStatsThisBookDistinct.Sum(x =>
                {
                    int parsedInt = 0;
                    int.TryParse(x.Value, out parsedInt);
                    return parsedInt;
                });
                book.BookStats.Add(Upsert.BookStat(context, new BookStat()
                {
                    BookId = book.Id,
                    Key = AvailableBookStat.DISTINCTWORDCOUNT,
                    Value = distinctWordCount.ToString()
                }));

                int totalKnownPercent = GetKnownPercent(
                    GetBookStat_int(book.BookStats, AvailableBookStat.TOTALLEARNEDCOUNT),
                    GetBookStat_int(book.BookStats, AvailableBookStat.TOTALWELLKNOWNCOUNT),
                    GetBookStat_int(book.BookStats, AvailableBookStat.TOTALIGNOREDCOUNT),
                    totalWordCount
                    );
                book.BookStats.Add(Upsert.BookStat(context, new BookStat()
                {
                    BookId = book.Id,
                    Key = AvailableBookStat.TOTALKNOWNPERCENT,
                    Value = totalKnownPercent.ToString()
                }));

                int distinctKnownPercent = GetKnownPercent(
                    GetBookStat_int(book.BookStats, AvailableBookStat.DISTINCTLEARNEDCOUNT),
                    GetBookStat_int(book.BookStats, AvailableBookStat.DISTINCTWELLKNOWNCOUNT),
                    GetBookStat_int(book.BookStats, AvailableBookStat.DISTINCTIGNOREDCOUNT),
                    distinctWordCount
                    );
                book.BookStats.Add(Upsert.BookStat(context, new BookStat()
                {
                    BookId = book.Id,
                    Key = AvailableBookStat.DISTINCTKNOWNPERCENT,
                    Value = distinctKnownPercent.ToString()
                }));
            }
        }
        
        public static void UpdateLanguageTotalWordsRead(IdiomaticaContext context, LanguageUser languageUser)
        {
            var readWords = languageUser.Books
                .SelectMany(x => x.Pages)
                .Where(p => p.ReadDate is not null)
                .Sum(p => PageHelper.GetWordCountOfPage(p, languageUser))
                ;
            languageUser.TotalWordsRead = readWords;
            context.SaveChanges();
        }

        #endregion

        #region private methods

        private static string FormAllDistinctCountsQuery(int userId)
        {
            string q =
$"""
            with allDistinctWordsByBook as (
	            select 
		                b.Id as BookId
		            , w.Id as WordId
		            , w.Status
	            from Book b
	            left join Page p on p.BookId = b.Id
	            left join Paragraph pp on pp.PageId = p.Id
	            left join Sentence s on s.ParagraphId = pp.id
	            left join Token t on t.SentenceId = s.Id
	            left join Word w on t.WordId = w.Id
	            left join LanguageUser lu on b.LanguageUserId = lu.Id
	            where lu.UserId = {userId}
	            group by 
		                b.Id
		            , w.Id
		            , w.Status
            ), distinctCounts as (
	            select 
		                ad.BookId
		            , ad.Status
		            , count(ad.WordId) as DistinctCount
	            from allDistinctWordsByBook ad
	            group by 
		            ad.BookId
		            , ad.Status
            )
            select 
	                dc.BookId
	            , (case
                    when dc.Status = {(int)AvailableStatus.NEW1} then {(int)AvailableBookStat.DISTINCTNEW1COUNT}
                    when dc.Status = {(int)AvailableStatus.NEW2} then {(int)AvailableBookStat.DISTINCTNEW2COUNT}
                    when dc.Status = {(int)AvailableStatus.LEARNING3} then {(int)AvailableBookStat.DISTINCTLEARNING3COUNT}
                    when dc.Status = {(int)AvailableStatus.LEARNING4} then {(int)AvailableBookStat.DISTINCTLEARNING4COUNT}
                    when dc.Status = {(int)AvailableStatus.LEARNED} then {(int)AvailableBookStat.DISTINCTLEARNEDCOUNT}
                    when dc.Status = {(int)AvailableStatus.IGNORED} then {(int)AvailableBookStat.DISTINCTIGNOREDCOUNT}
                    when dc.Status = {(int)AvailableStatus.WELLKNOWN} then {(int)AvailableBookStat.DISTINCTWELLKNOWNCOUNT}
                    when dc.Status = {(int)AvailableStatus.UNKNOWN} then {(int)AvailableBookStat.DISTINCTUNKNOWNCOUNT}
                    end ) as [Key]
	            , cast(dc.DistinctCount as varchar(250)) as [Value]
            from distinctCounts dc
""";
            return q;
        }
        private static string FormAllTotalCountsQuery(int userId)
        {
            string q =
$"""
            with allWords as (
	            select 
		              b.Id as BookId
		            , w.Id as WordId
		            , w.Status
	            from Book b
	            left join Page p on p.BookId = b.Id
	            left join Paragraph pp on pp.PageId = p.Id
	            left join Sentence s on s.ParagraphId = pp.id
	            left join Token t on t.SentenceId = s.Id
	            left join Word w on t.WordId = w.Id
	            left join LanguageUser lu on b.LanguageUserId = lu.Id
	            where lu.UserId = {userId}
            ), totalCounts as (
	            select 
		              aw.BookId
		            , aw.Status
		            , count(aw.WordId) as TotalCount
	            from allWords aw
	            group by 
		              aw.BookId
		            , aw.Status
            )
            select 
	              tc.BookId
	            , (case
		            when tc.Status = {(int)AvailableStatus.NEW1} then {(int)AvailableBookStat.TOTALNEW1COUNT}
		            when tc.Status = {(int)AvailableStatus.NEW2} then {(int)AvailableBookStat.TOTALNEW2COUNT}
		            when tc.Status = {(int)AvailableStatus.LEARNING3} then {(int)AvailableBookStat.TOTALLEARNING3COUNT}
		            when tc.Status = {(int)AvailableStatus.LEARNING4} then {(int)AvailableBookStat.TOTALLEARNING4COUNT}
		            when tc.Status = {(int)AvailableStatus.LEARNED} then {(int)AvailableBookStat.TOTALLEARNEDCOUNT}
		            when tc.Status = {(int)AvailableStatus.IGNORED} then {(int)AvailableBookStat.TOTALIGNOREDCOUNT}
		            when tc.Status = {(int)AvailableStatus.WELLKNOWN} then {(int)AvailableBookStat.TOTALWELLKNOWNCOUNT}
		            when tc.Status = {(int)AvailableStatus.UNKNOWN} then {(int)AvailableBookStat.TOTALUNKNOWNCOUNT}
		            end ) as [Key]
	            , cast(tc.TotalCount as varchar(250)) as [Value]
            from totalCounts tc
""";
            return q;
        }
        private static int GetKnownPercent(int? learnedCount, int? wellknownCount,
            int? ignoredCount, int? wordCount)
        {
            if (wordCount == 0) return 0;

            int totalKnown = (int)learnedCount + (int)wellknownCount + (int)ignoredCount;
            if (totalKnown == 0) return 0;

            decimal percentKnown = (decimal)totalKnown / (decimal)wordCount;
            return (int)Math.Round(percentKnown * 100M, 0);

        }
        private static BookStat UpdateOrCreateDefaultStat(IdiomaticaContext context, 
            Book book, List<BookStat> newStats, AvailableBookStat key, string defaultValue = "0")
        {
            // if it's in the new stats, upsert it
            // if it's not in the new stats, set default value then upsert it
            var bs = newStats.Where(x => x.Key == key).FirstOrDefault();
            if (bs != null)
            {
                return Upsert.BookStat(context, bs);
            }

            return Upsert.BookStat(context, new BookStat()
            {
                BookId = book.Id,
                Key = key,
                Value = defaultValue
            });
        }

        #endregion
    }
}
