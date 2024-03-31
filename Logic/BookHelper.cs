using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.DAL;

namespace Logic
{
	/// <summary>
	/// BookHelper is a static class containing useful functions, commonly used to interact with book objects
	/// </summary>
	public static class BookHelper
	{
		public static List<Book> GetBooksForUserId(IdiomaticaContext context, int userId)
		{
			Func<Book, bool> allBooksFilter = (x => x.UserId == userId);
			return Fetch.Books(context, allBooksFilter);
		}
		public static (int lastPageRead, int totalPages) GetPageStats(Book book)
		{
			var totalPages = (book.Texts is null) ? 0 : book.Texts.Count();
			var lastPageRead = (book.Texts is null) ? null : book.Texts
				.Where(x => x.ReadDate is not null)
				.OrderByDescending(x => x.ReadDate)
				.FirstOrDefault();
			var currentPageNumber = (lastPageRead is null) ? 0 : lastPageRead.Order;

			return(currentPageNumber, totalPages);
		}
		public static bool IsBookComplete(Book book)
		{
			// a book is complete if its last page has been read
			if (book.Texts is null || book.Texts.Count == 0) return false;
			else
			{
				var lastPage = book.Texts.OrderByDescending(x => x.Order).First();
				if (lastPage.ReadDate is not null) return true;
			}
			return false;
		}
	}
}
