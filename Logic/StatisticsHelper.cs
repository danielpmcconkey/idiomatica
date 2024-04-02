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
        public static string? GetBookStat(Book book, string key)
        {
            var existingStat = book.BookStats.Where(x => x.Key == key).FirstOrDefault();
            if (existingStat == null) return null;
            return existingStat.Value;
        }
        /// <summary>
        /// Updates the bookstats table for all books for a given user and saves the database context
        /// </summary>
        /// <param name="context">the database context</param>
        /// <param name="userId">the ID of the user whose books are to be updated</param>
        public static void UpdateAllBookStatsForUserId(IdiomaticaContext context, int userId)
		{
			Func<Book, bool> allBooksFilter = (x => x.LanguageUser.UserId == userId);
			var books = Fetch.Books(context, allBooksFilter);
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
				.Sum(p => PageHelper.GetWordCount(p, languageUser.Language))				
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
			

			Dictionary<String, (int status, int count)> wordDict =
				new Dictionary<string, (int status, int count)>();

			Func<Word, bool> filter = (x => x.LanguageUser.LanguageId == book.LanguageUser.LanguageId);
			var wordsInLanguage = Fetch.Words(context, filter);

			if (book.BookStats == null) book.BookStats = new List<BookStat>();
			
			int totalWordCount = 0; // set this from null to 0 so you can increment it



			foreach (var t in book.Pages)
			{
				var wordsInText = PageHelper.GetWords(t, book.LanguageUser.Language);
                totalWordCount += wordsInText.Count();
				foreach (var word in wordsInText)
				{
					var foundWord = wordsInLanguage
						.Where(x => x.Text.ToLower() == word)
						.FirstOrDefault();

					if (foundWord != null)
					{
						if (wordDict.ContainsKey(word))
						{
							wordDict[word] = (foundWord.Status.Id, wordDict[word].count + 1);
						}
						else
						{
							wordDict.Add(word, (foundWord.Status.Id, 1));
						}
					}
				}
			}
            AddStatToBookStats(book, book.BookStats, "TotalWordCount", totalWordCount.ToString());

			/*
             *
                (0, '?', 'Unknown'),
                (1, '1', 'New (1)'),
                (2, '2', 'New (2)'),
                (3, '3', 'Learning (3)'),
                (4, '4', 'Learning (4)'),
                (5, '5', 'Learned'),
                (99, 'WKn', 'Well Known'),
                (98, 'Ign', 'Ignored');
             * */

			AddStatToBookStats(book, book.BookStats, "DistinctWordCount", wordDict.Count.ToString());

            var status0Stat = GetStatsByStatus(0, wordDict);
            var status1Stat = GetStatsByStatus(1, wordDict);
            var status2Stat = GetStatsByStatus(2, wordDict);
            var status3Stat = GetStatsByStatus(3, wordDict);
            var status4Stat = GetStatsByStatus(4, wordDict);
            var status5Stat = GetStatsByStatus(5, wordDict);
            var status99Stat = GetStatsByStatus(99, wordDict);
			var status98Stat = GetStatsByStatus(98, wordDict);

			AddStatToBookStats(book, book.BookStats, "TotalUnknownCount", status0Stat.total.ToString());
            AddStatToBookStats(book, book.BookStats, "DistinctUnknownCount", status0Stat.distinct.ToString());
			AddStatToBookStats(book, book.BookStats, "totalnew1Count", status1Stat.total.ToString());
            AddStatToBookStats(book, book.BookStats, "distinctnew1Count", status1Stat.distinct.ToString());
            AddStatToBookStats(book, book.BookStats, "totalnew2Count", status2Stat.total.ToString());
            AddStatToBookStats(book, book.BookStats, "distinctnew2Count", status2Stat.distinct.ToString());
			AddStatToBookStats(book, book.BookStats, "totallearning3Count", status3Stat.total.ToString());
            AddStatToBookStats(book, book.BookStats, "distinctlearning3Count", status3Stat.distinct.ToString());
			AddStatToBookStats(book, book.BookStats, "totallearning4Count", status4Stat.total.ToString());
            AddStatToBookStats(book, book.BookStats, "distinctlearning4Count", status4Stat.distinct.ToString());
			AddStatToBookStats(book, book.BookStats, "totallearnedCount", status5Stat.total.ToString()); 
            AddStatToBookStats(book, book.BookStats, "distinctlearnedCount", status5Stat.distinct.ToString());
            AddStatToBookStats(book, book.BookStats, "totalwellknownCount", status99Stat.total.ToString());
            AddStatToBookStats(book, book.BookStats, "distinctwellknownCount", status99Stat.distinct.ToString());
			AddStatToBookStats(book, book.BookStats, "totalignoredCount", status98Stat.total.ToString());
            AddStatToBookStats(book, book.BookStats, "distinctignoredCount", status98Stat.distinct.ToString());





            string? totallearnedCount = GetBookStat(book, "totallearnedCount");
            int totallearnedCount_int = 0;
            int.TryParse(totallearnedCount, out totallearnedCount_int);

            string? totalwellknownCount = GetBookStat(book, "totalwellknownCount");
            int totalwellknownCount_int = 0;
            int.TryParse(totalwellknownCount, out totalwellknownCount_int);

            string? totalignoredCount = GetBookStat(book, "totalignoredCount");
            int totalignoredCount_int = 0;
            int.TryParse(totalignoredCount, out totalignoredCount_int);

            string? TotalWordCount = GetBookStat(book, "TotalWordCount");
            int TotalWordCount_int = 0;
            int.TryParse(TotalWordCount, out TotalWordCount_int);

            string? distinctlearnedCount = GetBookStat(book, "distinctlearnedCount");
            int distinctlearnedCount_int = 0;
            int.TryParse(distinctlearnedCount, out distinctlearnedCount_int);

            string? distinctwellknownCount = GetBookStat(book, "distinctwellknownCount");
            int distinctwellknownCount_int = 0;
            int.TryParse(distinctwellknownCount, out distinctwellknownCount_int);

            string? distinctignoredCount = GetBookStat(book, "distinctignoredCount");
            int distinctignoredCount_int = 0;
            int.TryParse(distinctignoredCount, out distinctignoredCount_int);

            string? DistinctWordCount = GetBookStat(book, "DistinctWordCount");
            int DistinctWordCount_int = 0;
            int.TryParse(DistinctWordCount, out TotalWordCount_int);


            AddStatToBookStats(book, book.BookStats, "TotalKnownPercent", 
                GetKnownPercent(totallearnedCount_int, totalwellknownCount_int,
                totalignoredCount_int, TotalWordCount_int).ToString());

            AddStatToBookStats(book, book.BookStats, "DistinctKnownPercent",
                GetKnownPercent(distinctlearnedCount_int, distinctwellknownCount_int,
                distinctignoredCount_int, DistinctWordCount_int).ToString());
           


			Console.WriteLine($"{book.Title}: {GetBookStat(book, "TotalKnownPercent")} | {GetBookStat(book, "DistinctKnownPercent")}");

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
			(int status, Dictionary<String, (int status, int count)> wordDict)
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
                stats.Add(new BookStat() { Book = book, BookId = book.Id, Key = key, Value = value });
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
