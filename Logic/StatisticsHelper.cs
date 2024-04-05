using System.Text;
using System.Text.RegularExpressions;
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
        public static string? GetBookStat(List<BookStat> bookStats, string key)
        {
            if (bookStats == null) return null;
            var existingStat = bookStats.Where(x => x.Key == key).FirstOrDefault();
            if (existingStat == null) return null;
            return existingStat.Value;
        }
        public static string? GetBookStat(Book book, string key)
        {
            if (book.BookStats == null) return null;
            return GetBookStat(book.BookStats, key);
        }
        public static int GetBookStat_int(Book book, string key)
        {
            if (book.BookStats == null) return 0;            
            return GetBookStat_int(book.BookStats, key);
        }
        public static int GetBookStat_int(List<BookStat> bookStats, string key)
        {
            if (bookStats == null) return 0;
            var existingStat = GetBookStat(bookStats, key);
            int parsedStat = 0;
            int.TryParse(existingStat, out parsedStat);
            return parsedStat;
        }
        /// <summary>
        /// Updates the bookstats table for all books for a given user and saves the database context
        /// </summary>
        /// <param name="context">the database context</param>
        /// <param name="userId">the ID of the user whose books are to be updated</param>
        public static void UpdateAllBookStatsForUserId(IdiomaticaContext context, int userId)
        {
            Func<Book, bool> allBooksFilter = (x => x.LanguageUser.UserId == userId);
            var books = BookHelper.GetBooks(context, allBooksFilter);
            foreach (var book in books)
            {
                UpdateSingleBookStats(context, book);
            }
            context.SaveChanges();
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
        /// <summary>
        /// Updates the bookstats table for a single book and saves the database context
        /// </summary>
        /// <param name="context">the database context</param>
        /// <param name="book">the book to be updated</param>
        public static void UpdateSingleBookStats(IdiomaticaContext context, Book book)
        {
            // first, check whether the book is complete
            book.IsComplete = BookHelper.IsBookComplete(book);

            // next update its page stats
            var pageStats = BookHelper.GetPageStats(book);
            book.TotalPages = pageStats.totalPages;
            book.LastPageRead = pageStats.lastPageRead;

            // finally get its word stats


            Dictionary<String, (AvailableStatus status, int count)> wordDict =
                new Dictionary<string, (AvailableStatus status, int count)>();

            Func<Word, bool> filter = (x => x.LanguageUser.LanguageId == book.LanguageUser.LanguageId);
            var wordsInLanguage = WordHelper.FetchWordDictForLanguageUser(book.LanguageUser);

            if (book.BookStats != null)
            {
                // delete these and build new
                foreach (var bs in book.BookStats)
                {
                    context.BookStats.Remove(bs);
                }
                book.BookStats = null;
            }
            List<BookStat> tempStats = new List<BookStat>();

            int totalWordCount = 0; // set this from null to 0 so you can increment it

            foreach (var t in book.Pages)
            {
                var wordsInText = PageHelper.GetWordsOfPage(t, book.LanguageUser);
                totalWordCount += wordsInText.Count();
                foreach (var word in wordsInText)
                {
                    if (wordsInLanguage.ContainsKey(word))
                    {
                        Word foundWord = wordsInLanguage[word];
                        if (wordDict.ContainsKey(word))
                        {
                            wordDict[word] = (foundWord.Status, wordDict[word].count + 1);
                        }
                        else
                        {
                            wordDict.Add(word, (foundWord.Status, 1));
                        }
                    }
                }
            }
            AddStatToBookStats(book, tempStats, "TOTALWORDCOUNT", totalWordCount.ToString());
            AddStatToBookStats(book, tempStats, "DISTINCTWORDCOUNT", wordDict.Count.ToString());

            var status0Stat = GetStatsByStatus(AvailableStatus.UNKNOWN, wordDict);
            var status1Stat = GetStatsByStatus(AvailableStatus.NEW1, wordDict);
            var status2Stat = GetStatsByStatus(AvailableStatus.NEW2, wordDict);
            var status3Stat = GetStatsByStatus(AvailableStatus.LEARNING3, wordDict);
            var status4Stat = GetStatsByStatus(AvailableStatus.LEARNING4, wordDict);
            var status5Stat = GetStatsByStatus(AvailableStatus.LEARNED, wordDict);
            var status99Stat = GetStatsByStatus(AvailableStatus.WELLKNOWN, wordDict);
            var status98Stat = GetStatsByStatus(AvailableStatus.IGNORED, wordDict);

            tempStats = AddStatToBookStats(book, tempStats, "TOTALUNKNOWNCOUNT", status0Stat.total.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "DISTINCTUNKNOWNCOUNT", status0Stat.distinct.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "TOTALNEW1COUNT", status1Stat.total.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "DISTINCTNEW1COUNT", status1Stat.distinct.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "TOTALNEW2COUNT", status2Stat.total.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "DISTINCTNEW2COUNT", status2Stat.distinct.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "TOTALLEARNING3COUNT", status3Stat.total.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "DISTINCTLEARNING3COUNT", status3Stat.distinct.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "TOTALLEARNING4COUNT", status4Stat.total.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "DISTINCTLEARNING4COUNT", status4Stat.distinct.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "TOTALLEARNEDCOUNT", status5Stat.total.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "DISTINCTLEARNEDCOUNT", status5Stat.distinct.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "TOTALWELLKNOWNCOUNT", status99Stat.total.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "DISTINCTWELLKNOWNCOUNT", status99Stat.distinct.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "TOTALIGNOREDCOUNT", status98Stat.total.ToString());
            tempStats = AddStatToBookStats(book, tempStats, "DISTINCTIGNOREDCOUNT", status98Stat.distinct.ToString());

            int totallearnedCount_int = GetBookStat_int(tempStats, "TOTALLEARNEDCOUNT");
            int totalwellknownCount_int = GetBookStat_int(tempStats, "TOTALWELLKNOWNCOUNT");
            int totalignoredCount_int = GetBookStat_int(tempStats, "TOTALIGNOREDCOUNT");
            int TotalWordCount_int = GetBookStat_int(tempStats, "TOTALWORDCOUNT");
            int distinctlearnedCount_int = GetBookStat_int(tempStats, "DISTINCTLEARNEDCOUNT");
            int distinctwellknownCount_int = GetBookStat_int(tempStats, "DISTINCTWELLKNOWNCOUNT");
            int distinctignoredCount_int = GetBookStat_int(tempStats, "DISTINCTIGNOREDCOUNT");
            int DistinctWordCount_int = GetBookStat_int(tempStats, "DISTINCTWORDCOUNT");

            tempStats = AddStatToBookStats(book, tempStats, "TOTALKNOWNPERCENT",
                GetKnownPercent(totallearnedCount_int, totalwellknownCount_int,
                totalignoredCount_int, TotalWordCount_int).ToString());

            tempStats = AddStatToBookStats(book, tempStats, "DISTINCTKNOWNPERCENT",
                GetKnownPercent(distinctlearnedCount_int, distinctwellknownCount_int,
                distinctignoredCount_int, DistinctWordCount_int).ToString());

            Console.WriteLine($"{book.Title}: {GetBookStat(book, "TOTALKNOWNPERCENT")} | {GetBookStat(book, "DISTINCTKNOWNPERCENT")}");
            foreach (var bs in tempStats)
            {
                var existingStat = context.BookStats
                    .Where(x => x.BookId == bs.BookId && x.Key == bs.Key)
                    .FirstOrDefault();
                try
                {
                    context.BookStats.Add(bs);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            context.SaveChanges();
        }
        #endregion
        #region private methods
        private static int GetKnownPercent(int? learnedCount, int? wellknownCount,
            int? ignoredCount, int? wordCount)
        {
            if (wordCount == 0) return 0;

            int totalKnown = (int)learnedCount + (int)wellknownCount + (int)ignoredCount;
            if (totalKnown == 0) return 0;

            decimal percentKnown = (decimal)totalKnown / (decimal)wordCount;
            return (int)Math.Round(percentKnown * 100M, 0);

        }
        private static (int total, int distinct) GetStatsByStatus
            (AvailableStatus status, Dictionary<String, (AvailableStatus status, int count)> wordDict)
        {
            int total = wordDict
                            .Where(x => x.Value.status == status)
                            .Sum(x => x.Value.count);
            int distinct = wordDict
                            .Where(x => x.Value.status == status)
                            .Count();
            return (total, distinct);
        }
        private static List<BookStat> AddStatToBookStats(Book book, List<BookStat> stats, string key, string value)
        {
            var existingStat = stats.Where(x => x.Key == key).FirstOrDefault();
            if (existingStat == null)
            {
                stats.Add(new BookStat() { BookId = book.Id, Key = key, Value = value });
            }
            else
            {
                existingStat.Value = value;
            }
            return stats;
        }


        #endregion
    }
}
