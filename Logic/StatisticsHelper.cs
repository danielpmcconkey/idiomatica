using System.Text;
using System.Text.RegularExpressions;
using Model;
using Model.DAL;
namespace Logic
{
	/// <summary>
	/// StatisticsHelper groups common functions used to derive analytics about users, languages, books, etc.
	/// </summary>
	public static class StatisticsHelper
    {
		#region public interface
		/// <summary>
		/// Updates the bookstats table for all books for a given user and saves the database context
		/// </summary>
		/// <param name="context">the database context</param>
		/// <param name="userId">the ID of the user whose books are to be updated</param>
		public static void UpdateAllBookStatsForUserId(IdiomaticaContext context, int userId)
		{
			Func<Book, bool> allBooksFilter = (x => x.UserId == userId);
			var books = Fetch.Books(context, allBooksFilter);
			foreach (var book in books)
			{
				UpdateSingleBookStats(context, book);
			}
			context.SaveChanges();
		}
		public static void UpdateLanguageTotalWordsRead(IdiomaticaContext context, Language language)
		{			
			var readWords = language.Books
				.SelectMany(x => x.Pages)
				.Where(p => p.ReadDate is not null)
				.Sum(p => PageHelper.GetWordCount(p, language))				
				;
			language.TotalWordsRead = readWords;
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

			Func<Word, bool> filter = (x => x.LanguageId == book.LanguageId);
			var wordsInLanguage = Fetch.Words(context, filter);

			if (book.BookStat == null)
			{
				book.BookStat = new BookStat();
				book.BookStat.BookId = book.Id;
				book.BookStat.Book = book;
			}
			book.BookStat.totalwordCount = 0; // set this from null to 0 so you can increment it



			foreach (var t in book.Pages)
			{
				var wordsInText = PageHelper.GetWords(t, book.Language);
				book.BookStat.totalwordCount += wordsInText.Count();
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

			book.BookStat.distinctwordCount = wordDict.Count;

			var status0Stat = GetStatsByStatus(0, wordDict);
			book.BookStat.totalunknownCount = status0Stat.total;
			book.BookStat.distinctunknownCount = status0Stat.distinct;

			var status1Stat = GetStatsByStatus(1, wordDict);
			book.BookStat.totalnew1Count = status1Stat.total;
			book.BookStat.distinctnew1Count = status1Stat.distinct;

			var status2Stat = GetStatsByStatus(2, wordDict);
			book.BookStat.totalnew2Count = status2Stat.total;
			book.BookStat.distinctnew2Count = status2Stat.distinct;

			var status3Stat = GetStatsByStatus(3, wordDict);
			book.BookStat.totallearning3Count = status3Stat.total;
			book.BookStat.distinctlearning3Count = status3Stat.distinct;

			var status4Stat = GetStatsByStatus(4, wordDict);
			book.BookStat.totallearning4Count = status4Stat.total;
			book.BookStat.distinctlearning4Count = status4Stat.distinct;

			var status5Stat = GetStatsByStatus(5, wordDict);
			book.BookStat.totallearnedCount = status5Stat.total;
			book.BookStat.distinctlearnedCount = status5Stat.distinct;

			var status99Stat = GetStatsByStatus(99, wordDict);
			book.BookStat.totalwellknownCount = status99Stat.total;
			book.BookStat.distinctwellknownCount = status99Stat.distinct;

			var status98Stat = GetStatsByStatus(98, wordDict);
			book.BookStat.totalignoredCount = status98Stat.total;
			book.BookStat.distinctignoredCount = status98Stat.distinct;


			book.BookStat.totalknownPercent = GetKnownPercent(
				book.BookStat.totallearnedCount,
				book.BookStat.totalwellknownCount,
				book.BookStat.totalignoredCount,
				book.BookStat.totalwordCount);
			book.BookStat.distinctknownPercent = GetKnownPercent(
				book.BookStat.distinctlearnedCount,
				book.BookStat.distinctwellknownCount,
				book.BookStat.distinctignoredCount,
				book.BookStat.distinctwordCount);

			// set the old stats to what I want them to be
			book.BookStat.wordcount = book.BookStat.totalwordCount;
			book.BookStat.unknownpercent = book.BookStat.totalknownPercent;
			book.BookStat.distinctterms = book.BookStat.distinctwordCount;
			book.BookStat.unknownpercent = 100 - book.BookStat.totalknownPercent;
			book.BkWordCount = book.BookStat.wordcount;

			Console.WriteLine($"{book.BkTitle}: {book.BookStat.totalknownPercent} | {book.BookStat.distinctknownPercent}");

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
		
		#endregion
	}
}
