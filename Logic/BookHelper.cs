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
	
	public static class BookHelper
	{
        
        
        
		//public static (int lastPageRead, int totalPages) GetPageStats(Book book)
		//{
		//	var totalPages = (book.Pages is null) ? 0 : book.Pages.Count();
		//	var lastPageRead = (book.Pages is null) ? null : book.Pages
		//		.Where(x => x.ReadDate is not null)
		//		.OrderByDescending(x => x.ReadDate)
		//		.FirstOrDefault();
		//	var currentPageNumber = (lastPageRead is null) ? 0 : lastPageRead.Ordinal;

		//	return(currentPageNumber, totalPages);
		//}
		//public static bool IsBookComplete(Book book)
		//{
		//	// a book is complete if its last page has been read
		//	if (book.Pages is null || book.Pages.Count == 0) return false;
		//	else
		//	{
		//		var lastPage = book.Pages.OrderByDescending(x => x.Ordinal).First();
		//		if (lastPage.ReadDate is not null) return true;
		//	}
		//	return false;
		//}
	}
}
