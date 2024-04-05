using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DAL;
using Logic;
using Model;
using System.Data.SQLite;
using System.Drawing;
using System.Xml.Linq;
using System.Reflection.PortableExecutable;
using System.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace IdiomaticaCLI
{
    #region sqlite data types
    public record sqlite_book
    {
        public int BkID;
        public int UserId;
        public int BkLgID;
        public int BkArchived;
        public int BkCurrentTxID;
        public int BkWordCount;
        public int IsComplete;
        public int TotalPages;
        public int LastPageRead;
        public string BkTitle;
        public string? BkSourceURI;
        public string? BkAudioFilename;
        public float? BkAudioCurrentPos;
        public string? BkAudioBookmarks;
    }
    public record sqlite_language
    {
        public int LgID;
        public int UserId;
        public string LgName;
        public string LgDict1URI;
        public string LgDict2URI;
        public string LgGoogleTranslateURI;
        public string LgCharacterSubstitutions;
        public string LgRegexpSplitSentences;
        public string LgExceptionsSplitSentences;
        public string LgRegexpWordCharacters;
        public int LgRemoveSpaces;
        public int LgSplitEachChar;
        public int LgRightToLeft;
        public int LgShowRomanization;
        public string LgParserType;
        public int TotalWordsRead;
    }
    public record sqlite_word
    {
        public int WoID;
        public int UserId;
        public int WoLgID;
        public string WoText;
        public string WoTextLC;
        public int WoStatus;
        public string? WoTranslation;
        public string? WoRomanization;
        public int WoTokenCount;
        public DateTime WoCreated;
        public DateTime WoStatusChanged;
    }
    public record sqlite_text
    {
        public int TxID; 
        public int TxBkID; 
        public int TxOrder;
        public string TxText;
        public DateTime? TxReadDate;
        public int? TxWordCount;
    }
    #endregion
    internal class DataTransfer
    {
        private int _danId;
        private Dictionary<int, int> _wordMap;
        public DataTransfer() { }
        public void Transfer(IdiomaticaContext context)
        {
            _wordMap = new Dictionary<int, int>();

            AddUsers(context);
            AddLanguages(context);
            AddBooks(context);
            AddWords(context);
            AddWordParents(context);


        }
        private void AddBooks(IdiomaticaContext context)
        {
            //Func<LanguageUser, bool> filter = (x => true);
            //var languageUsers = LanguageHelper.GetLanguageUser(context, filter);
            //foreach (var languageUser in languageUsers)
            //{
            //    // note, this isn't reusable as it's just luck that my user and language IDs match
            //    foreach (var book_in in PullBooksForLanguageUser(languageUser.LanguageId, _danId))
            //    {
            //        int oldBookId = book_in.BkID;
            //        bool archived = (book_in.BkArchived == 1) ? true : false;
            //        bool completed = (book_in.IsComplete == 1) ? true : false;
            //        int lastPageRead_in = book_in.LastPageRead;
            //        int lastPageRead_out = 0;
            //        List<Page> pages = new List<Page>();

            //        // get pages first so you can add them to the book at create time
            //        foreach(var text_in in PullTextsForBook(oldBookId))
            //        {
            //            List<Paragraph> paragraphs = PageHelper.CreateParagraphsFromPageAndSave(
            //                text_in.TxText, languageUser);
            //            Page page = new Page() 
            //            {
            //                Paragraphs = paragraphs, Ordinal = text_in.TxOrder, OriginalText = text_in.TxText, 
            //                ReadDate = text_in.TxReadDate
            //            };
            //            pages.Add(page);
                        

            //            //if(text_in.TxID == lastPageRead_in)
            //            //{
            //            //    lastPageRead_out = page.Id;
            //            //}
            //        }
            //        Book book = new Book()
            //        {
            //            CurrentPageID = book_in.BkCurrentTxID, IsArchived = archived,
            //            IsComplete = completed, LanguageUserId = languageUser.LanguageId,
            //            LastPageRead = lastPageRead_out, Pages = pages, BookStats = new List<BookStat>(), 
            //            SourceURI = book_in.BkSourceURI, Title = book_in.BkTitle, TotalPages = book_in.TotalPages, 
            //            WordCount = book_in.BkWordCount
            //        };
            //        context.Books.Add(book);
            //        context.SaveChanges();
            //    }
            //}
        }
        private void AddLanguages(IdiomaticaContext context)
        {
            foreach(var language_in in PullLanguages())
            {

                Language language = new Language() {
                    Name = language_in.LgName,
                    Dict1URI = language_in.LgDict1URI,
                    Dict2URI = language_in.LgDict2URI,
                    GoogleTranslateURI = language_in.LgGoogleTranslateURI,
                    CharacterSubstitutions = language_in.LgCharacterSubstitutions,
                    RegexpSplitSentences = language_in.LgRegexpSplitSentences, // note I had to change the regex so this won't work
                    ExceptionsSplitSentences = language_in.LgExceptionsSplitSentences,
                    RegexpWordCharacters = language_in.LgRegexpWordCharacters,
                    RemoveSpaces = (language_in.LgRemoveSpaces == 1) ? true: false,
                    SplitEachChar = (language_in.LgSplitEachChar == 1) ? true : false,
                    RightToLeft = (language_in.LgRightToLeft == 1) ? true : false,
                    ShowRomanization = (language_in.LgShowRomanization == 1) ? true : false,
                    ParserType = language_in.LgParserType,
                };
                context.Languages.Add(language);
                context.SaveChanges();

                LanguageUser languageUser = new LanguageUser()
                {
                    LanguageId = (int)language.Id,
                    UserId = _danId
                };
                context.LanguageUsers.Add(languageUser);
                context.SaveChanges();

                
            }
        }
        private void AddUsers(IdiomaticaContext context)
        {
            User dan = new User() { Name = "Dan McConkey" };
            context.Users.Add(dan);
            context.SaveChanges();
            _danId = (int)dan.Id;
        }
        private void AddWords(IdiomaticaContext context)
        {
            foreach(var languageUser in context.LanguageUsers)
            {
                AddWordsForLanguageUser(context, languageUser);
            }
        }
        private void AddWordsForLanguageUser(IdiomaticaContext context, LanguageUser languageUser)
        {
            List<(sqlite_word word_in, Word word_out)> combinedList = 
                new List<(sqlite_word word_in, Word word_out)>();
            var words_in = PullWordsForLanguage(languageUser.LanguageId);
            foreach (var word_in in words_in)
            {
                var statusId = AvailableStatus.UNKNOWN;
                switch (word_in.WoStatus)
                {
                    case 0:
                        statusId = AvailableStatus.UNKNOWN;
                        break;
                    case 1:
                        statusId = AvailableStatus.NEW1;
                        break;
                    case 2:
                        statusId = AvailableStatus.NEW2;
                        break;
                    case 3:
                        statusId = AvailableStatus.LEARNING3;
                        break;
                    case 4:
                        statusId = AvailableStatus.LEARNING4;
                        break;
                    case 5:
                        statusId = AvailableStatus.LEARNED;
                        break;
                    case 98:
                        statusId = AvailableStatus.IGNORED;
                        break;
                    case 99:
                        statusId = AvailableStatus.WELLKNOWN;
                        break;
                }
                            
                Word word_out = new Word() 
                {
                    LanguageUserId = (int)languageUser.Id,
                    Status = statusId,
                    Text = word_in.WoText,
                    TextLowerCase = word_in.WoTextLC,
                    Translation = word_in.WoTranslation,
                    Romanization = word_in.WoRomanization,
                    TokenCount = word_in.WoTokenCount,
                    Created = word_in.WoCreated,
                    StatusChanged = word_in.WoStatusChanged
                };
                combinedList.Add((word_in, word_out));
                context.Words.Add(word_out);
            }
            context.SaveChanges();
            // now go back and create a map between new word IDs and old word IDs
            foreach(var c in combinedList)
            {
                _wordMap.Add(c.word_in.WoID, (int)c.word_out.Id);
            }
        }
        private void AddWordParents(IdiomaticaContext context)
        {
            var wordParents_in = PullWordParents();
            foreach(var wp_in in wordParents_in)
            {
                int child_id = _wordMap[wp_in.word_id];
                int parent_id = _wordMap[wp_in.parent_id];

                var rowsInserted = context.Database.ExecuteSql($"INSERT INTO [dbo].[WordParent]([Id],[ParentWordId]) VALUES({child_id},{parent_id})");
            }
            context.SaveChanges();
        }
        #region SQLite functions
        private SQLiteConnection CreateConnection()
        {
            const string db_path = "C:\\Users\\Dan\\AppData\\Local\\Packages\\PythonSoftwareFoundation.Python.3.12_qbz5n2kfra8p0\\LocalCache\\Local\\Lute3\\Lute3\\lute.db";
            return new SQLiteConnection($"Data Source={db_path}");
        }
        public DateTime? GetDateTime(SQLiteDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }
            return reader.GetDateTime(ordinal);
        }
        public int? GetInt(SQLiteDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }
            return reader.GetInt32(ordinal);
        }
        public string? GetString(SQLiteDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }
            return reader.GetString(ordinal);
        }
        private List<sqlite_book> PullBooksForLanguageUser(int languageId, int userId)
        {
            List<sqlite_book> books = new List<sqlite_book>();
            string q = """
                select 
                    BkID,
                    UserId,
                    BkLgID,
                    BkArchived,
                    BkCurrentTxID,
                    BkWordCount,
                    IsComplete,
                    TotalPages,
                    LastPageRead,
                    BkTitle,
                    BkSourceURI,
                    BkAudioFilename,
                    BkAudioCurrentPos,
                    BkAudioBookmarks
                from books
                where BkLgID = $languageId
                """;
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("$languageId", languageId);
                    cmd.CommandText = q;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sqlite_book book = new sqlite_book();
                            book.BkID = reader.GetInt32(reader.GetOrdinal("BkID"));
                            book.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                            book.BkLgID = reader.GetInt32(reader.GetOrdinal("BkLgID"));
                            book.BkArchived = reader.GetInt32(reader.GetOrdinal("BkArchived"));
                            book.BkCurrentTxID = reader.GetInt32(reader.GetOrdinal("BkCurrentTxID"));
                            book.BkWordCount = reader.GetInt32(reader.GetOrdinal("BkWordCount"));
                            book.IsComplete = reader.GetInt32(reader.GetOrdinal("IsComplete"));
                            book.TotalPages = reader.GetInt32(reader.GetOrdinal("TotalPages"));
                            book.LastPageRead = reader.GetInt32(reader.GetOrdinal("LastPageRead"));
                            book.BkTitle = reader.GetString(reader.GetOrdinal("BkTitle"));
                            book.BkSourceURI = GetString(reader,"BkSourceURI");                            
                            books.Add(book);
                        }
                    }
                }
            }
            return books;
        }
        private List<sqlite_language> PullLanguages()
        {
            List<sqlite_language> languages = new List<sqlite_language>();
            string q = """
                select LgID,UserId,LgName,LgDict1URI,LgDict2URI,LgGoogleTranslateURI,
                LgCharacterSubstitutions,LgRegexpSplitSentences,LgExceptionsSplitSentences,
                LgRegexpWordCharacters,LgRemoveSpaces,LgSplitEachChar,LgRightToLeft,
                LgShowRomanization,LgParserType,TotalWordsRead
                from languages
                """;
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = q;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sqlite_language language = new sqlite_language();
                            language.LgID = reader.GetInt32(reader.GetOrdinal("LgID"));
                            language.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                            language.LgName = reader.GetString(reader.GetOrdinal("LgName"));
                            language.LgDict1URI = reader.GetString(reader.GetOrdinal("LgDict1URI"));
                            language.LgDict2URI = reader.GetString(reader.GetOrdinal("LgDict2URI"));
                            language.LgGoogleTranslateURI = reader.GetString(reader.GetOrdinal("LgGoogleTranslateURI"));
                            language.LgCharacterSubstitutions = reader.GetString(reader.GetOrdinal("LgCharacterSubstitutions"));
                            language.LgRegexpSplitSentences = reader.GetString(reader.GetOrdinal("LgRegexpSplitSentences"));
                            language.LgExceptionsSplitSentences = reader.GetString(reader.GetOrdinal("LgExceptionsSplitSentences"));
                            language.LgRegexpWordCharacters = reader.GetString(reader.GetOrdinal("LgRegexpWordCharacters"));
                            language.LgRemoveSpaces = reader.GetInt32(reader.GetOrdinal("LgRemoveSpaces"));
                            language.LgSplitEachChar = reader.GetInt32(reader.GetOrdinal("LgSplitEachChar"));
                            language.LgRightToLeft = reader.GetInt32(reader.GetOrdinal("LgRightToLeft"));
                            language.LgShowRomanization = reader.GetInt32(reader.GetOrdinal("LgShowRomanization"));
                            language.LgParserType = reader.GetString(reader.GetOrdinal("LgParserType"));
                            language.TotalWordsRead = reader.GetInt32(reader.GetOrdinal("TotalWordsRead"));
                            languages.Add(language);
                        }
                    }

                }
            }
            return languages;
        }
        private List<sqlite_text> PullTextsForBook(int bookId)
        {
            List<sqlite_text> texts = new List<sqlite_text>();
            string q = """
                select 
                    TxID,
                    TxBkID,
                    TxOrder,
                    TxText,
                    TxReadDate,
                    TxWordCount
                from texts
                where TxBkID = $bookId
                """;
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("$bookId", bookId);
                    cmd.CommandText = q;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sqlite_text text = new sqlite_text();
                            text.TxID = reader.GetInt32(reader.GetOrdinal("TxID"));
                            text.TxBkID = reader.GetInt32(reader.GetOrdinal("TxBkID"));
                            text.TxOrder = reader.GetInt32(reader.GetOrdinal("TxOrder"));
                            text.TxText = reader.GetString(reader.GetOrdinal("TxText"));
                            text.TxReadDate = GetDateTime(reader, "TxReadDate");
                            text.TxWordCount = GetInt(reader, "TxWordCount");
                            texts.Add(text);
                        }
                    }
                }
            }
            return texts;
        }
        private List<sqlite_word> PullWordsForLanguage(int languageId)
        {
            List<sqlite_word> words = new List<sqlite_word>();
            string q = """
                select WoID,UserId,WoLgID,WoText,WoTextLC,WoStatus,WoTranslation,WoRomanization,
                WoTokenCount,WoCreated,WoStatusChanged
                from words where WoLgID = $languageId
                """;
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("$languageId", languageId);
                    cmd.CommandText = q;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sqlite_word word = new sqlite_word();
                            word.WoID = reader.GetInt32(reader.GetOrdinal("WoID"));
                            word.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                            word.WoLgID = reader.GetInt32(reader.GetOrdinal("WoLgID"));
                            word.WoText = reader.GetString(reader.GetOrdinal("WoText"));
                            word.WoTextLC = reader.GetString(reader.GetOrdinal("WoTextLC"));
                            word.WoStatus = reader.GetInt32(reader.GetOrdinal("WoStatus"));
                            word.WoTranslation = GetString(reader,"WoTranslation");
                            word.WoRomanization = GetString(reader, "WoRomanization");
                            word.WoTokenCount = reader.GetInt32(reader.GetOrdinal("WoTokenCount"));
                            word.WoCreated = reader.GetDateTime(reader.GetOrdinal("WoCreated"));
                            word.WoStatusChanged = reader.GetDateTime(reader.GetOrdinal("WoStatusChanged"));
                            words.Add(word);
                        }
                    }

                }
            }
            return words;
        }
        private List<(int word_id, int parent_id)> PullWordParents()
        {
            List<(int word_id, int parent_id)> wordParents = new List<(int word_id, int parent_id)>();
            string q = """
                select WpWoID,WpParentWoID
                from wordparents
                """;
            using (var conn = CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    
                    cmd.CommandText = q;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            int word_id = reader.GetInt32(reader.GetOrdinal("WpWoID"));
                            int parent_id = reader.GetInt32(reader.GetOrdinal("WpParentWoID"));
                            wordParents.Add((word_id, parent_id));
                        }
                    }
                }
            }
            return wordParents;
        }
        #endregion
    }
}
