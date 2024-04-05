using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	/// BookHelper is a static class containing useful functions, commonly used to interact with book objects
	/// </summary>
	public static class BookHelper
	{
        /// <summary>
        /// includes book stats an languageuser, but not pages or content
        /// because including them would make the result set too large
        /// and the query would take too long
        /// </summary>
        public static Book? GetBookById(IdiomaticaContext context, int id)
        {
            Func<Book, bool> filter = (x => x.Id == id);
            return context.Books
                .Include(b => b.BookStats)
                .Include(l => l.LanguageUser).ThenInclude(lu => lu.Language)
                .Where(filter)
                .FirstOrDefault();
        }
        /// <summary>
        /// doesn't include the pages, paragraphs, etc
        /// </summary>
        public static List<Book> GetBooks(IdiomaticaContext context, Func<Book, bool> filter)
        {
            return context.Books
                .Include(b => b.BookStats)
                .Include(b => b.LanguageUser).ThenInclude(lu => lu.Language)
                .Where(filter)
                .ToList();
            
        }
        /// <summary>
        /// includes book stats an languageuser, but not pages or content
        /// </summary>
		public static List<Book> GetBooksForUserId(IdiomaticaContext context, int userId)
		{
			Func<Book, bool> filter = (x => x.LanguageUser.UserId == userId);
			return GetBooks(context, filter);
		}
		public static (int lastPageRead, int totalPages) GetPageStats(Book book)
		{
			var totalPages = (book.Pages is null) ? 0 : book.Pages.Count();
			var lastPageRead = (book.Pages is null) ? null : book.Pages
				.Where(x => x.ReadDate is not null)
				.OrderByDescending(x => x.ReadDate)
				.FirstOrDefault();
			var currentPageNumber = (lastPageRead is null) ? 0 : lastPageRead.Ordinal;

			return(currentPageNumber, totalPages);
		}
		public static bool IsBookComplete(Book book)
		{
			// a book is complete if its last page has been read
			if (book.Pages is null || book.Pages.Count == 0) return false;
			else
			{
				var lastPage = book.Pages.OrderByDescending(x => x.Ordinal).First();
				if (lastPage.ReadDate is not null) return true;
			}
			return false;
		}
	}
}
