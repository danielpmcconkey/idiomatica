using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	/// BookHelper is a static class containing useful functions, commonly used to interact with book objects
	/// </summary>
	public static class BookHelper
	{
		public static Book? GetBooksById(IdiomaticaContext context, int bookId)
		{
			Func<Book, bool> allBooksFilter = (x => x.Id == bookId);
			return Fetch.Books(context, allBooksFilter).FirstOrDefault();
		}
		public static List<Book> GetBooksForUserId(IdiomaticaContext context, int userId)
		{
			Func<Book, bool> filter = (x => x.LanguageUser.UserId == userId);
			return Fetch.Books(context, filter);
		}
		public static (int lastPageRead, int totalPages) GetPageStats(Book book)
		{
			var totalPages = (book.Pages is null) ? 0 : book.Pages.Count();
			var lastPageRead = (book.Pages is null) ? null : book.Pages
				.Where(x => x.ReadDate is not null)
				.OrderByDescending(x => x.ReadDate)
				.FirstOrDefault();
			var currentPageNumber = (lastPageRead is null) ? 0 : lastPageRead.Order;

			return(currentPageNumber, totalPages);
		}
		public static bool IsBookComplete(Book book)
		{
			// a book is complete if its last page has been read
			if (book.Pages is null || book.Pages.Count == 0) return false;
			else
			{
				var lastPage = book.Pages.OrderByDescending(x => x.Order).First();
				if (lastPage.ReadDate is not null) return true;
			}
			return false;
		}
	}
}
