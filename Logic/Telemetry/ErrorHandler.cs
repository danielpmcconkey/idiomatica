using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Telemetry
{
    public static class ErrorHandler
    {
        //private static ILogger<IdiomaticaLogger> _logger;

        //public ErrorHandler(ILogger<IdiomaticaLogger> logger) => _logger = logger;

        private static Dictionary<int, string> _errorCodes = new Dictionary<int, string>()
        {
            // 1000 errors are invalid arguments

            { 1040, "Title may not be null when creating a new book." },
            { 1050, "Language code may not be null when creating a new book." },
            { 1060, "Text may not be null when creating a new book." },
            { 1100, "Book ID cannot be less than 1 when creating BookStats" },
            { 1120, "bookUserId cannot be 0 when saving current page." },
            { 1130, "currentPageId cannot be 0 when saving current page." },
            { 1140, "Page cannot be null and Page.UniqueKey cannot be null or 0 when clearing page" },
            { 1141, "_currentPageUser or _currentPageUser.UniqueKey is null or 0 when clearing page" },
            { 1142, "_currentPageUser.PageKey is null or 0 when clearing page" },
            { 1150, "id cannot be 0 when saving word user." },
            { 1160, "Language ID cannot be zero when fetching common words" },
            { 1170, "languageUser cannot be null when fetching flash cards from the DB" },
            { 1180, "languageUser.UniqueKey cannot be null or 0 when fetching flash cards from the DB" },
            { 1190, "userId cannot be less that 1 when fetching language users from the DB" },
            { 1200, "_bookUser is null when pulling pageUser" },
            { 1210, "LoggedInUser cannot be null and LoggedInUser.UniqueKey cannot be null or 0 when clearing page" },
            { 1220, "LoggedInUser cannot be null and LoggedInUser.UniqueKey cannot be null or 0 when archiving a book" },
            { 1221, "LoggedInUser cannot be null and LoggedInUser.UniqueKey cannot be null or 0 when adding a book" },
            { 1230, "bookId cannot be 0 when fetching words by book" },
            { 1240, "pageId cannot be 0 when resetting page data" },
            { 1250, "page cannot be null when parsing paragraphs" },
            { 1260, "Page must have a DB ID before adding children" },
            { 1270, "Sentence cannot be null when creating tokens" },
            { 1280, "Sentence must have a DB ID before adding tokens" },
            { 1290, "Sentence text cannot be null when creating tokens" },
            { 1300, "word dictionary cannot be null when creating tokens" },
            { 1310, "page cannot be null when creating new PageUser" },
            { 1320, "page ID cannot be 0 when creating new PageUser" },
            { 1330, "bookUser cannot be null when creating new PageUser" },
            { 1340, "language cannot be null when saving a new word" },
            { 1350, "language ID cannot be 0 when saving a new word" },
            { 1360, "commonWordDict cannot be null when creating new PageUser" },
            { 1370, "languageuser.UniqueKey cannot be 0 when saving new WordUser" },
            { 1380, "word cannot be null when saving new WordUser" },
            { 1390, "token cannot be null and token.UniqueKey must be greater than 0 in word modal" },
            { 1400, "Logged in user and its ID must be non-null in the word modal" },
            { 1410, "bookId cannot be 0 when archiving book" },
            { 1411, "bookId cannot be 0 when adding a book" },
            { 1420, "bookId cannot be null or 0 when saving a BookTag" },
            { 1430, "userId cannot be null or 0 when saving a BookTag" },
            { 1440, "tag cannot be empty when saving a BookTag" },
            { 1450, "bookId cannot be null or 0 when removing a BookTag" },
            { 1460, "userId cannot be null or 0 when removing a BookTag" },
            { 1470, "tag cannot be empty when removing a BookTag" },
            { 1480, "_bookId is null or 0 in the PageMove function" },
            { 1490, "_currentPageUser.PageKey is null or 0 in the PageMove function" },
            { 1500, "_currentPage or _currentPage.UniqueKey is null or 0 in the PageMove function" },
            { 1510, "sentence.UniqueKey is null or 0 in SentenceFillChildObjects" },
            { 1520, "_allWordsInPage is null in SentenceFillChildObjects" },
            { 1530, "_allWordUsersInPage is null in SentenceFillChildObjects" },
            { 1540, "token.Word was null while cycling through sentence tokens in SentenceFillChildObjects" },
            { 1550, "token.Word.TextLowerCase was null while cycling through sentence tokens in SentenceFillChildObjects" },
            { 1560, "_loggedInUser is null or _loggedInUser.UniqueKey is null or 0 in SentenceFillChildObjects" },
            { 1570, "_languageUser is null or _languageUser.UniqueKey is null or 0 in SentenceFillChildObjects" },
            { 1580, "token.WordKey is null or 0 in TokenGetChildObjects" },
            { 1590, "_allWordsInPage is null or empty in TokenGetChildObjects" },

            // 2000 errors are invalid data states

            { 2000, "Book query returned null when trying to create book user" },
            { 2001, "Book.LanguageKey returned null when trying to create book user" },
            { 2010, "First page returned null or 0 when trying to create book user" },
            { 2020, "Language user query returned null or 0 when trying to create book user" },
            { 2021, "languageUser.UserKey returned null or 0 when trying to create book user" },
            { 2030, "BookUser.UniqueKey returned as 0 when trying to create book user" },
            { 2040, "Saving a page returned a null ID from DB while creating a new book." },
            { 2050, "no BookUser found with that Id. Cannot update bookmark" },
            { 2060, "provided ID doesn't match a word user in the database" },
            { 2070, "Language pull from DB returned null while creating a new book." },
            { 2080, "Paragraph splits returned null or empty while creating a new book." },
            { 2090, "Saving the book returned a null ID from DB while creating a new book." },
            { 2110, "Bookstats insert query updated no rows" },
            { 2100, "PageUser returned null from DB when trying to mark it as 'read'" },
            { 5070, "Underlying Page does not exist when trying to retrieve PageUser." },
            { 2130, "Either Logged in user or logged in user ID are null" },
            { 2140, "Either book or book ID are null" },
            { 2141, "_book.LanguageKey is either null or 0 on Read data init" },
            { 2150, "BookUser is either null or BookUserId is 0 on Read data init" },
            { 2151, "_bookUser.CurrentPageKey is either null or 0 on Read data init" },
            { 2152, "_bookUser.LanguageUserKey is either null or 0 on Read data init" },
            { 2153, "_language or _language.Code is either null or 0 on Read data init" },
            { 2160, "There is no card in the database that matches the provided ID while updating flash card" },
            { 5080, "BookUser.UniqueKey returned 0 after create and save in Read page." },
            { 5090, "bookUserStatsFromDb returned null after in Read page" },
            { 5110, "WordUser from is null or Id is 0 when trying to retrieve DB WordUser for new flash cards." },
            { 2120, "FlashCard.UniqueKey returned null or 0 when creating." },
            { 2170, "Language User or Language User ID is null while moving pages" },
            { 2180, "_bookUser or _bookUser.UniqueKey is null while moving pages" },
            { 2190, "_currentPageUser or _currentPageUser.UniqueKey is null while moving pages" },
            { 2200, "commonWordsInLanguage returned null while creating book" },
            { 2210, "paragraph.UniqueKey cannot be null or 0 in paragraph view" },
            { 2220, "_currentPageUser cannot be null and _currentPageUser.PageKey cannot be 0 in read init data" },
            { 2230, "sentence ID is null or zero when trying to fetch tokens" },
            { 2240, "_languageToCode and _languageFromCode cannot be null when translating a paragraph" },
            { 2241, "pp.UniqueKey cannot be null or 0 when translating a paragraph" },
            { 2242, "_languageFromCode or _languageFromCode.Code is null or empty when translating a paragraph" },
            { 2243, "_languageToCode or _languageToCode.Code is null or empty when translating a paragraph" },
            { 2250, "Error saving the bookUser while creating a new BookUser." },
            { 2260, "Paragraph parsing failed during page save" },
            { 2270, "Paragraph ID returned null or 0 after saving" },
            { 2280, "Sentence ID returned null or 0 after saving" },
            { 2290, "Token ID returned null or 0 after saving" },
            { 2300, "Saving tokens while parsing paragraphs returned false" },
            { 2310, "PageUser.ID returned null or 0 after saving" },
            { 2320, "Unable to retrieve language from the bookUser.LanguageUserKey while creating PageUser" },
            { 2330, "newWordUser returned null during PageUser creation" },
            { 2340, "Paragraph translation ID returned null or 0 while saving" },
            { 2350, "newWord ID returned null or 0 while saving" },
            { 2360, "newWordUser ID returned null or 0 while saving" },
            { 2370, "word not found while getting sentence tokens" },
            { 2380, "wordUser ID returned null or 0 while saving" },
            { 2390, "word and word Id cannot be null in the word modal" },
            { 2400, "wordUser and wordUser Id cannot be null in the word modal" },
            { 2410, "_bookTotalPages is null or 0 while moving pages" },
            { 2420, "token.Word cannot be null and token.Word.UniqueKey must be greater than 0 in word modal" },
            { 2430, "wordUser cannot be null and wordUser.UniqueKey must be greater than 0 in word modal" },
            { 2440, "BookTag ID returned null or 0 while saving or saving failed" },
            { 2450, "LoggedInUser cannot be null when reading book list" },
            { 2460, "WordUser.WordKey is null or 0 when trying to create new flash cards." },
            { 2470, "Sentence is null when trying to create new flash cards." },
            { 2480, "Paragraph is null when trying to create new flash cards." },
            { 2490, "_uiLanguageCode or _uiLanguageCode.Code is null when trying to create new flash cards." },
            { 2500, "wordUser.LanguageUser is null when trying to create new flash cards." },
            { 2510, "CurrentCard or CurrentCard.UniqueKey is null when updating a flash card." },
            { 2520, "FlashCardAttempt.UniqueKey was null or zero after saving it." },
            { 2530, "_languageUser is null or _languageUser.LanguageKey is null when creating new PageUser" },
            { 2540, "bookUser returned null while trying to archive a book" },
            { 2550, "Language code from DB has a null for its Code value" },
            { 2560, "" },
            { 2570, "" },
            { 2580, "" },
            { 2590, "" },
            { 2600, "" },
            { 2610, "" },
            { 2620, "" },
            { 2630, "" },
            { 2640, "" },
            { 2650, "" },
            { 2660, "" },
            { 2670, "" },
            { 2680, "" },
            { 2690, "" },

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
        public static void LogAndThrow(
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            LogError(memberName, sourceFilePath, sourceLineNumber, [], null);
            ThrowError(memberName, sourceFilePath, sourceLineNumber);
        }
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
        public static void LogAndThrow(int code, string[] args, Exception? ex)
        {
            LogError(code, args, ex);
            ThrowError(code);
        }
        private static void LogError(int code, string[] args, Exception? ex)
        {
            // todo: log errors
        }
        private static void LogError(
            string memberName, string sourceFilePath, int sourceLineNumber, string[] args, Exception? ex)
        {
            // todo: log errors
        }
        private static void ThrowError(int code)
        {
            var errorCode = _errorCodes[code];
            var ex = new IdiomaticaException() { code = code };
            throw ex;
        }
        private static void ThrowError(string memberName, string sourceFilePath, int sourceLineNumber)
        {

            var ex = new IdiomaticaException()
            {
                memberName = memberName,
                sourceFilePath = sourceFilePath,
                sourceLineNumber = sourceLineNumber
            };
            throw ex;
        }
    }
}
