// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Model.DAL;
using Logic;
using Model;
using IdiomaticaCLI;
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
    // delete a page's contents and start over. does not delete the page
    Expression<Func<Book, bool>> filter =
                (x => x.LanguageUser.UserId == 1 && x.Id == 14);
    Book? bookFromDb = Fetch.BookAndBookStatsAndLanguage(filter);

    var pages = Fetch.Pages((x => x.BookId == bookFromDb.Id));
    foreach (var page in pages)
    {
        PageHelper.RepairPage(page, bookFromDb.LanguageUser);
    }
}

