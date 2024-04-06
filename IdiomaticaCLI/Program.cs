// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Model.DAL;
using Logic;
using Model;
using System.Linq.Expressions;

//using (var context = new IdiomaticaContext())
//{
//    StatisticsHelper.UpdateAllBookStatsForUserId(context, 1);

//    //Func<Book, bool> allBooksFilter = (x => x.Id == 12);
//    //var book = Fetch.Books(context, allBooksFilter).FirstOrDefault();
//    //StatisticsHelper.UpdateSingleBookStats(context, book);

//    //Func<Language, bool> languageFilter = (x => x.UserId == 1);
//    //foreach (Language language in Fetch.Languages(context, languageFilter))
//    //{
//    //	StatisticsHelper.UpdateLanguageTotalWordsRead(context, language);
//    //}


//    //var path = @"E:\Idiomatica\Idiomatica\Model\Migrations\20240331.08.21_AddHelperColumns.sql";
//    //Execute.File(context, path);


//    //DataTransfer seeder = new DataTransfer();
//    //seeder.Transfer(context);

//}

if (false)
{
    // delete a book's contents and start over. does not delete the pages
    Expression<Func<Book, bool>> filter =
                (x => x.Id != 14);
    var booksFromDb = Fetch.BooksAndBookStatsAndLanguage(filter);
    foreach (var bookFromDb in booksFromDb)
    {
        var pages = Fetch.Pages((x => x.BookId == bookFromDb.Id));
        foreach (var page in pages)
        {
            PageHelper.RepairPage(page, bookFromDb.LanguageUser);
        }
    }
}

