using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
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
        public static string? GetBookStat(List<BookStat> bookStats, AvailableBookStat key)
        {
            if (bookStats == null) return null;
            var existingStat = bookStats.Where(x => x.Key == key).FirstOrDefault();
            if (existingStat == null) return null;
            return existingStat.Value;
        }
        public static string? GetBookStat(Book book, AvailableBookStat key)
        {
            if (book.BookStats == null) return null;
            return GetBookStat(book.BookStats, key);
        }
        public static int GetBookStat_int(Book book, AvailableBookStat key)
        {
            if (book == null) return 0;
            if (book.BookStats == null) return 0;
            return GetBookStat_int(book.BookStats, key);
        }
        public static int GetBookStat_int(List<BookStat> bookStats, AvailableBookStat key)
        {
            if (bookStats == null) return 0;
            var existingStat = GetBookStat(bookStats, key);
            int parsedStat = 0;
            int.TryParse(existingStat, out parsedStat);
            return parsedStat;
        }

        public static string? GetBookUserStat(List<BookUserStat> bookUserStats, AvailableBookUserStat key)
        {
            if (bookUserStats == null) return null;
            var existingStat = bookUserStats.Where(x => x.Key == key).FirstOrDefault();
            if (existingStat == null) return null;
            return existingStat.Value;
        }
        //public static string? GetBookUserStat(BookUser bookUser, AvailableBookUserStat key)
        //{
        //    if (bookUser.BookUserStats == null) return null;
        //    return GetBookUserStat(bookUser.BookUserStats, key);
        //}
        //public static int GetBookUserStat_int(BookUser bookUser, AvailableBookUserStat key)
        //{
        //    if (bookUser.BookUserStats == null) return 0;
        //    return GetBookUserStat_int(bookUser.BookUserStats, key);
        //}
        public static int GetBookUserStat_int(List<BookUserStat> bookUserStats, AvailableBookUserStat key)
        {
            if (bookUserStats == null) return 0;
            var existingStat = GetBookUserStat(bookUserStats, key);
            int parsedStat = 0;
            int.TryParse(existingStat, out parsedStat);
            return parsedStat;
        }
        //public static bool GetBookUserStat_bool(BookUser bookUser, AvailableBookUserStat key)
        //{
        //    if (bookUser.BookUserStats == null) return false;
        //    return GetBookUserStat_bool(bookUser.BookUserStats, key);
        //}
        public static bool GetBookUserStat_bool(List<BookUserStat> bookUserStats, AvailableBookUserStat key)
        {
            if (bookUserStats == null) return false;
            var existingStat = GetBookUserStat(bookUserStats, key);
            bool parsedStat = false;
            bool.TryParse(existingStat, out parsedStat);
            return parsedStat;
        }


        

        #endregion

        #region private methods

        
        
        

        #endregion
    }
}
