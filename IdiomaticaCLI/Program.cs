// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Model.DAL;
using Logic;
using Model;

using (var context = new IdiomaticaContext())
{
    BookAnalyzer.AnalyzeBooks(context);

    //var path = @"E:\Idiomatica\Idiomatica\Model\Migrations\dropOldTables_20240330.13.13.44.sql";
    //Execute.File(context, path);


}

