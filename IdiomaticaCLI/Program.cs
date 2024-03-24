// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Model.DAL;
using (var context = new IdiomaticaContext())
{
    var results = Fetch.Word(context, 111);
    //foreach (var r in results)
    //{
    //    //Console.WriteLine($"{r.BookStat.wordcount}");

    //    foreach (var x in r.ParentWords)
    //    {
    //        //Console.WriteLine($"{r.Text}'s child is {x.Text}");
    //        Console.WriteLine($"{r.Text}'s Parent is {x.Text}");
    //        //foreach (var y in x.Sentences)
    //        //{
    //        //    Console.WriteLine($"{y.SentenceText}");
    //        //}
    //    }
    //}
    string burp = "h";
}

