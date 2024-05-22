using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class ErrorHandler
    {
        /*
         * WARNING
         * this class cannot hold data that is unique to a user or session
         * 
         * */

        public static Dictionary<int, string> ErrorCodes = new Dictionary<int, string>() 
        {
            // 1000 errors are invalid arguments

            { 1040, "Title may not be null when creating a new book." },
            { 1050, "Language code may not be null when creating a new book." },
            { 1060, "Text may not be null when creating a new book." },
            { 1100, "Book ID cannot be less than 1 when creating BookStats" },
            { 1120, "bookUserId cannot be 0 when saving current page." },
            { 1130, "currentPageId cannot be 0 when saving current page." },
            { 1140, "Page user cannot be null when clearing page" },
            { 1150, "id cannot be 0 when saving word user." },
            { 1160, "" },
            { 1170, "languageUser cannot be null when fetching flash cards from the DB" },
            { 1180, "languageUser.Id cannot be null or 0 when fetching flash cards from the DB" },
            { 1190, "userId cannot be less that 1 when fetching language users from the DB" },
            { 1200, "" },
            { 1210, "" },
            { 1220, "" },
            { 1230, "" },
            { 1240, "" },
            { 1250, "" },
            { 1260, "" },
            { 1270, "" },
            { 1280, "" },
            { 1290, "" },

            // 2000 errors are invalid data states

            { 2000, "Book query returned null when trying to create book user" },
            { 2010, "First page returned null or 0 when trying to create book user" },
            { 2020, "Language user query returned null or 0 when trying to create book user" },
            { 2030, "BookUser.Id returned as 0 when trying to create book user" },
            { 2040, "Saving a page returned a null ID from DB while creating a new book." },
            { 2050, "no BookUser found with that Id. Cannot update bookmark" },
            { 2060, "provided ID doesn't match a word user in the database" },
            { 2070, "Language pull from DB returned null while creating a new book." },
            { 2080, "Paragraph splits returned null or empty while creating a new book." },
            { 2090, "Saving the book returned a null ID from DB while creating a new book." },
            { 2110, "Bookstats insert query updated no rows" },            
            { 2100, "PageUser returned null from DB when trying to mark it as 'read'" },
            { 5070, "Underlying Page does not exist when trying to retrieve PageUser." },
            { 2130, "" },
            { 2140, "" },
            { 2150, "" },
            { 2160, "There is no card in the database that matches the provided ID while updating flash card" },
            { 5080, "BookUser.Id returned 0 after create and save in Read page." },
            { 5090, "bookUserStatsFromDb returned null after in Read page" },
            { 5110, "WordUser from is null or Id is 0 when trying to retrieve DB WordUser for new flash cards." },
            { 2120, "FlashCard.Id returned null or 0 when creating." },
            { 2170, "" },
            { 2180, "" },
            { 2190, "" },
            { 2200, "" },
            { 2210, "" },
            { 2220, "" },
            { 2230, "" },
            { 2240, "" },
            { 2250, "" },
            { 2260, "" },
            { 2270, "" },
            { 2280, "" },
            { 2290, "" },

            // 3000 errors are other .net errors

            { 3000, "Generic exception generated while creating and saving page user" },
            { 3010, "Generic exception generated while translating paragraph in DeepL service" },
            { 3020, "" },
            { 3030, "" },
            { 3040, "" },
            { 3050, "" },
            { 3060, "" },
            { 3070, "" },
            { 3080, "" },
            { 3090, "" },
            { 3100, "" },
            { 3110, "" },
            { 3120, "" },
            { 3130, "" },
            { 3140, "" },
            { 3150, "" },
            { 3160, "" },
            { 3170, "" },
            { 3180, "" },
            { 3190, "" },
        };
        public static void LogAndThrow(int code)
        {
            LogError(code, [], null);
            ThrowError(code);
        }
        public static void LogAndThrow(int code, string[] args)
        {
            LogError(code, args, null);
            ThrowError(code);
        }
        public static void LogAndThrow(int code, string[] args, Exception ex)
        {
            LogError(code, args, ex);
            ThrowError(code);
        }
        private static void LogError(int code, string[] args, Exception ex)
        {
            // todo: log errors
        }
        private static void ThrowError(int code)
        {
            var errorCode = ErrorCodes[code];
            throw new IdiomaticaException(errorCode) { code = code };
        }
    }
    public class IdiomaticaException : Exception
    {
        public int code;
        public IdiomaticaException()
        {
        }

        public IdiomaticaException(string message)
            : base(message)
        {
        }

        public IdiomaticaException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    /// <summary>
    /// ErrorState is used so parent and child components can share a single
    /// error state between one another
    /// </summary>
    public class ErrorState
    {
        public bool isError;
        public string errorMessage;
    }
}
