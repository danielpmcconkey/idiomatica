using Microsoft.EntityFrameworkCore;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Polly;

namespace Logic.Services
{
    public class BookService
    {
        private IDbContextFactory<IdiomaticaContext> _dbContextFactory;
        public BookService(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        #region book

        public int BookCreateAndSave(string title, string languageCode, string url, string text)
        {
            const int _targetCharCountPerPage = 1378;// this was arrived at by DB query after conversion
            // sanitize and validate input
            var titleT = title.Trim();
            var languageCodeT = languageCode.Trim();
            var urlT = url.Trim();
            var textT = text.Trim().Replace('\u00A0', ' ');
            if (string.IsNullOrEmpty(titleT))
            {
                throw new ArgumentNullException("Title may not be null when creating a new book.");
            }
            if (string.IsNullOrEmpty(languageCodeT))
            {
                throw new ArgumentNullException("Language code may not be null when creating a new book.");
            }
            if (string.IsNullOrEmpty(textT))
            {
                throw new ArgumentNullException("Text may not be null when creating a new book.");
            }


            var context = _dbContextFactory.CreateDbContext();

            // pull language from the db
            var language = context.Languages.Where(x => x.LanguageCode.Code == languageCodeT).FirstOrDefault();
            if (language == null || language.Id == null)
            {
                throw new InvalidDataException("Language pull from DB returned null while creating a new book.");
            }

            // divide text into paragraphs
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var paragraphSplits = parser.SegmentTextByParagraphs(textT);
            if (paragraphSplits is null || paragraphSplits.Length == 0)
            {
                throw new InvalidDataException("Paragraph splits returned null or empty while creating a new book.");
            }

            // add the book to the DB so you can save pages using its ID
            Book book = new Book()
            {
                Title = titleT,
                SourceURI = urlT,
                LanguageId = (int)language.Id,
            };
            context.Books.Add(book);
            context.SaveChanges();
            //book.Language = language;

            if (book.Id == null || book.Id == 0)
            {
                throw new InvalidDataException("Saving the book returned a null ID from DB while creating a new book.");
            }

            // pull a list of common words from the database and put it
            // in a dictionary so that we don't always have to go back to the
            // database just to get the word ID for every single word
            var commonWordsInLanguage = WordFetchCommonDictForLanguage(language);

            var currentPageCount = 1;
            var pageText = "";
            int currentCharCount = 0;
            bool isFirstPpOfPage = true;

            // loop through the paragraphs and add a page for the closest character count
            for (int i = 0; i < paragraphSplits.Length; i++)
            {
                string paragraph = paragraphSplits[i];
                int thisCharCount = paragraph.Length;
                if (currentCharCount + thisCharCount > _targetCharCountPerPage)
                {
                    if (isFirstPpOfPage)
                    {
                        // special weird case
                        // it's too big to fit on one page and it is the first pp of this page
                        // make it its own page
                        pageText = $"{pageText}{"\r\n"}{paragraph}";
                        currentCharCount += thisCharCount;
                        isFirstPpOfPage = false;
                    }
                    else
                    {
                        // too big, stick it on the next one
                        i -= 1; // go back one
                        // make a new page
                        PageCreateAndSaveDuringBookCreate((int)book.Id, currentPageCount,
                                    pageText, context, language, commonWordsInLanguage);


                        // reset stuff
                        pageText = "";
                        currentCharCount = 0;
                        currentPageCount++;
                        isFirstPpOfPage = true;
                    }
                }
                else
                {
                    // add to the stack
                    pageText = $"{pageText}{"\r\n"}{paragraph}";
                    currentCharCount += thisCharCount;
                    isFirstPpOfPage = false;
                }
            }
            if (!string.IsNullOrEmpty(pageText))
            {
                // there's still text left. need to add it to a new page
                PageCreateAndSaveDuringBookCreate((int)book.Id, currentPageCount,
                            pageText, context, language, commonWordsInLanguage);
            }

            return (int)book.Id;
        }

        #endregion

        #region bookuser
        public int BookUserCreateAndSave(int bookId, int userId)
        {
            var context = _dbContextFactory.CreateDbContext();
            var book = context.Books
                .Where(b => b.Id == bookId)
                .Include(b => b.Pages)
                .FirstOrDefault();
            if (book == null)
            {
                throw new InvalidDataException("Book query returned null when trying to create book user");
            }
            var firstPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if(firstPage == null || firstPage.Id == 0)
            {
                throw new InvalidDataException("First page returned null or 0 when trying to create book user");
            }
            var languageUser = context.LanguageUsers
                .Where(lu => lu.LanguageId == book.LanguageId && lu.UserId == userId)
                .FirstOrDefault();
            if (firstPage == null || firstPage.Id == 0)
            {
                throw new InvalidDataException("Language user query returned null or 0 when trying to create book user");
            }
            BookUser bookUser = new BookUser() { 
                BookId = bookId, 
                CurrentPageID = (int)firstPage.Id, 
                LanguageUserId = (int)languageUser.Id
            };
            context.BookUsers.Add(bookUser);
            context.SaveChanges();
            if (bookUser.Id == 0)
            {
                throw new InvalidDataException("BookUser.Id returned as 0 when trying to create book user");
            }
            return bookUser.Id;
        }
        public BookUser? BookUserFetch(int loggedInUserId, int bookId)
        {
            var context = _dbContextFactory.CreateDbContext();
            return context.BookUsers
                .Where(bu => bu.LanguageUser.UserId == loggedInUserId && bu.BookId == bookId)
                .Include(bu => bu.LanguageUser).ThenInclude(lu => lu.Language).ThenInclude(l => l.LanguageCode)
                .Include(bu => bu.Book).ThenInclude(b => b.BookStats)
                .FirstOrDefault()
                ;
        }
        public IQueryable<BookUser> BookUsersFetchWithoutStats(int loggedInUserId)
        {
            var context = _dbContextFactory.CreateDbContext();

            Expression<Func<BookUser, bool>> filter = (x => x.LanguageUser.UserId == loggedInUserId);
            return context.BookUsers
                .Where(filter)
                .Include(bu => bu.LanguageUser).ThenInclude(lu => lu.Language)
                .Include(bu => bu.Book).ThenInclude(b => b.BookStats);
        }

        #endregion

        #region BookUserStat
        public IQueryable<BookUserStat> BookUserStatsFetch(int loggedInUserId)
        {
            var context = _dbContextFactory.CreateDbContext();

            Expression<Func<BookUserStat, bool>> filter = (x => x.LanguageUser.UserId == loggedInUserId);
            return context.BookUserStats
                .Where(filter);

        }
        public IQueryable<BookUserStat> BookUserStatsFetch(int loggedInUserId, int bookId)
        {
            var context = _dbContextFactory.CreateDbContext();

            return context.BookUserStats
                .Where(x => x.LanguageUser.UserId == loggedInUserId && x.BookId == bookId);

        }
        #endregion

        #region LanguageCode
        public IQueryable<LanguageCode> LanguageCodeFetchOptionsDuringBookCreate()
        {
            var context = _dbContextFactory.CreateDbContext();

            Expression<Func<LanguageCode, bool>> filter = (x => x.IsImplementedForLearning == true);
            return context.LanguageCodes
                .Where(filter).OrderBy(x => x.LanguageName);
        }
        #endregion

        #region LanguageUser
        public LanguageUser LanguageUserFetch(int userId, int languageId)
        {
            var context = _dbContextFactory.CreateDbContext();
            return context.LanguageUsers
                .Where(lu => lu.User.Id == userId && lu.LanguageId == languageId)
                .Include(lu => lu.Language).ThenInclude(l => l.LanguageCode)
                .FirstOrDefault();
        }
        public LanguageUser LanguageUserFetch(int userId, string languageCode)
        {
            var context = _dbContextFactory.CreateDbContext();
            return context.LanguageUsers
                .Where(lu => lu.User.Id == userId && lu.Language.Code == languageCode)
                .Include(lu => lu.Language).ThenInclude(l => l.LanguageCode)
                .FirstOrDefault();
        }
        #endregion

        #region Page
        public Page? PageFetchByOrdinalWithinBook(int ordinal, int bookId)
        {
            // pages are public. no need to check for a match to logged in user
            var context = _dbContextFactory.CreateDbContext();
            return context.Pages
                .Where(p => p.Ordinal == ordinal
                    && p.BookId == bookId)
                .Include(p => p.Paragraphs)
                    .ThenInclude(pp => pp.Sentences)
                    .ThenInclude(s => s.Tokens)
                    .ThenInclude(s => s.Word)
                .FirstOrDefault();
        }
        public int PageCreateAndSaveDuringBookCreate(
            int bookId, int Ordinal, string pageText, IdiomaticaContext context,
            Language language, Dictionary<string,Word> commonWordsInLanguage)
        {
            var newPage = new Page()
            {
                BookId = bookId,
                Ordinal = Ordinal,
                OriginalText = pageText
            };
            // save the page to the DB so you can save paragraphs
            context.Pages.Add(newPage);
            context.SaveChanges();
            if (newPage.Id == null || newPage.Id == 0)
            {
                throw new InvalidDataException("Saving a page returned a null ID from DB while creating a new book.");
            }
            // do the actual page parsing
            PageHelper.ParseParagraphsFromPageAndSave(
                context, newPage, language, commonWordsInLanguage);
            return (int)newPage.Id;
        }
        public void PageUpdateBookmark(int bookUserId, int currentPageId)
        {
            if (bookUserId == 0) throw new ArgumentException("bookUserId cannot be 0 when saving current page.");
            if (currentPageId == 0) throw new ArgumentException("currentPageId cannot be 0 when saving current page.");
            var context = _dbContextFactory.CreateDbContext();
            var bookUser = context.BookUsers.FirstOrDefault(x => x.Id == bookUserId);
            if (bookUser == null) throw new ArgumentException($"no BookUser found with Id {bookUserId}. Cannot update bookmark");
            bookUser.CurrentPageID = currentPageId;
            context.SaveChanges();
        }
        #endregion

        #region PageUser
        public int PageUserCreateAndSave(
            Page page, BookUser bookUser, Dictionary<string, Word> commonWordDict,
            Dictionary<string, WordUser> allWordUsersInLanguage)
        {
            var context = _dbContextFactory.CreateDbContext();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                int pageUserId = PageHelper.CreatePageUserAndSave(
                        context, page, bookUser, commonWordDict, allWordUsersInLanguage);
                transaction.Commit();
                return pageUserId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
        public PageUser? PageUserFetchById(int pageUserId, int loggedInUserId)
        {
            // note this doesn't fetch the word_user

            var context = _dbContextFactory.CreateDbContext();
            return context.PageUsers
                .Where(pu => pu.BookUser.LanguageUserId == loggedInUserId
                    && pu.Id == pageUserId)
                .Include(pu => pu.Page)
                    .ThenInclude(p => p.Paragraphs)
                    .ThenInclude(pp => pp.Sentences)
                    .ThenInclude(s => s.Tokens)
                    .ThenInclude(s => s.Word)
                .FirstOrDefault();
        }
        public PageUser? PageUserFetchByOrdinalWithinBook(int ordinal, int bookId, int loggedInUserId)
        {
            var context = _dbContextFactory.CreateDbContext();
            return context.PageUsers
                .Where(pu => pu.BookUser.LanguageUserId == loggedInUserId
                    && pu.Page.Ordinal == ordinal
                    && pu.Page.BookId == bookId)
                .Include(pu => pu.Page)
                    .ThenInclude(p => p.Paragraphs)
                    .ThenInclude(pp => pp.Sentences)
                    .ThenInclude(s => s.Tokens)
                    .ThenInclude(s => s.Word)
                .FirstOrDefault();
        }
        #endregion

        #region ParagraphTranslation
        public List<ParagraphTranslation> ParagraphTranslationsFetchByParagraph(Paragraph pp)
        {
            if (pp == null || pp.Id < 1) return null;
            var context = _dbContextFactory.CreateDbContext();
            return context.ParagraphTranslations.Where(ppt => ppt.ParagraphId == pp.Id)
                .Include(ppt => ppt.LanguageCode)
                .ToList();
        }
        public ParagraphTranslation ParagraphTranslationSave (ParagraphTranslation ppt)
        {
            var context = _dbContextFactory.CreateDbContext();
            context.ParagraphTranslations.Add(ppt);
            context.SaveChanges();
            return ppt;
        }
        #endregion

        #region Word
        public Dictionary<string, Word> WordFetchCommonDictForLanguage(Language language)
        {
            var context = _dbContextFactory.CreateDbContext();
            return WordHelper.FetchCommonWordDictForLanguage(context, language);
        }
        #endregion

        #region WordUser
        public int WordUserCreateAndSave(LanguageUser languageUser, Word word)
        {
            var context = _dbContextFactory.CreateDbContext();
            var wordUser = WordHelper.CreateAndSaveUnknownWordUser(context,
                    languageUser, word);
            return wordUser.Id;
        }
        public WordUser? WordUserFetch(LanguageUser languageUser, Word word)
        {
            var context = _dbContextFactory.CreateDbContext();
            
            return context.WordUsers
                .Where(x => x.LanguageUserId == languageUser.Id && x.WordId == word.Id)
                .Include(wu => wu.Word)
                .FirstOrDefault();
        }
        public Dictionary<string, WordUser> WordUserFetchDictForLanguageUser(LanguageUser languageUser)
        {
            var context = _dbContextFactory.CreateDbContext();
            return WordHelper.FetchWordUserDictForLanguageUser(context, languageUser);
        }
        public void WordUserUpdateStatusAndTranslation(int id, AvailableWordUserStatus newStatus, string translation)
        {
            if (id == 0) throw new ArgumentException("id cannot be 0 when saving word user.");
            // first pull the existing one from the database
            var context = _dbContextFactory.CreateDbContext();
            var dbWordUser = context.WordUsers.Where(x => x.Id == id).FirstOrDefault();
            if (dbWordUser == null) throw new InvalidDataException("provided ID doesn't match a word user in the database");
            // check if status has changed
            if (dbWordUser.Status != (AvailableWordUserStatus)newStatus)
            {
                dbWordUser.Status = (AvailableWordUserStatus)newStatus;
                dbWordUser.StatusChanged = DateTime.Now;
            }
            dbWordUser.Translation = translation;
            context.SaveChanges();
        }
        #endregion
    }
}
