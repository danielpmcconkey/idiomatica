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
using Microsoft.EntityFrameworkCore.Query.Internal;

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
            var titleT = (title == null) ? "" : title.Trim();
            var languageCodeT = (languageCode == null) ? "" : languageCode.Trim();
            var urlT = (url == null) ? "" : url.Trim();
            var textT = (text == null) ? "" : text.Trim().Replace('\u00A0', ' ');
            if (string.IsNullOrEmpty(titleT))
            {
                ErrorHandler.LogAndThrow(1040);
            }
            if (string.IsNullOrEmpty(languageCodeT))
            {
                ErrorHandler.LogAndThrow(1050);
            }
            if (string.IsNullOrEmpty(textT))
            {
                ErrorHandler.LogAndThrow(1060);
            }


            var context = _dbContextFactory.CreateDbContext();

            // pull language from the db
            var language = context.Languages.Where(x => x.LanguageCode.Code == languageCodeT).FirstOrDefault();
            if (language == null || language.Id == null)
            {
                ErrorHandler.LogAndThrow(2070);
            }

            // divide text into paragraphs
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var paragraphSplits = parser.SegmentTextByParagraphs(textT);
            if (paragraphSplits is null || paragraphSplits.Length == 0)
            {
                ErrorHandler.LogAndThrow(2080);
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
                ErrorHandler.LogAndThrow(2090);
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
                string paragraph = paragraphSplits[i].Trim();
                if (string.IsNullOrEmpty(paragraph)) continue;
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
        public Book BookFetch(BookUser bookUser)
        {
            var context = _dbContextFactory.CreateDbContext();

            return context.Books.Where(b => b.Id == bookUser.BookId)
                .Include(b => b.BookStats)
                .FirstOrDefault()
                ;
        }

        #endregion

        #region BookStat
        public void BookStatsCreateAndSave(int bookId)
        {
            if (bookId < 1)
            {
                ErrorHandler.LogAndThrow(1100);
            }
            var context = _dbContextFactory.CreateDbContext();
            string q = $"""
            with allPages as (
            	SELECT b.id as bookId, p.Id as pageId, p.Ordinal as pageOrdinal
            	FROM [Idiomatica].[Idioma].[Book] b
            	left join [Idioma].[Page] p on p.BookId = b.Id
            ), allParagraphs as (
            	select bookId, p.pageId, pageOrdinal, pp.Id as paragraphId, pp.Ordinal as paragraphOrdinal
            	from [Idioma].[Paragraph] pp
            	left join allPages p on pp.PageId = p.pageId
            ), allSentences as (
            	select bookId, pageId, pageOrdinal, pp.paragraphId, paragraphOrdinal, s.Id as sentenceId, s.Ordinal as sentenceOrdinal
            	from allParagraphs pp
            	join [Idioma].[Sentence] s on s.ParagraphId = pp.paragraphId
            ), allTokens as (
            	select bookId, pageId, pageOrdinal, paragraphId, paragraphOrdinal, s.sentenceId, sentenceOrdinal, t.Id as tokenId, t.Ordinal as tokenOrdinal, t.WordId as wordId
            	from allSentences s
            	left join [Idioma].[Token] t on t.SentenceId = s.sentenceId
            ), allWords as (
            	select bookId, pageId, pageOrdinal, paragraphId, paragraphOrdinal, sentenceId, sentenceOrdinal, t.tokenId, tokenOrdinal, w.TextLowerCase as wordText
            	from allTokens t
            	left join [Idioma].[Word] w on t.wordId = w.Id
            ), distinctWords as (
            	select bookId, wordText, count(*) as numInstances
            	from allWords
            	group by bookId, wordText
            ), totalPageCount as (
            	select bookId as BookId, {(int)AvailableBookStat.TOTALPAGES} as [Key], count(*) as [Value]
            	from allPages
            	group by bookId
            ), totalWordCount as (
            	select bookId as BookId, {(int)AvailableBookStat.TOTALWORDCOUNT} as [Key], sum(numInstances) as [Value]
            	from distinctWords
            	group by bookId
            ), distinctWordCount as (
            	select bookId as BookId, {(int)AvailableBookStat.DISTINCTWORDCOUNT} as [Key], count(wordText) as [Value]
            	from distinctWords
            	group by bookId
            ), bookStatQueries as (
            	select * from totalPageCount
            	union all
            	select * from totalWordCount
            	union all
            	select * from distinctWordCount
            )
            insert into [Idioma].[BookStat](BookId, [Key], [Value])
            select * from bookStatQueries
            where BookId = {bookId}
            """;
            int numRows = context.Database.ExecuteSqlRaw(q);
            if (numRows < 1)
            {
                ErrorHandler.LogAndThrow(2110);
            }
            //context.SaveChanges();
        }
        public List<BookStat> BookStatsFetch(int bookId)
        {
            var context = _dbContextFactory.CreateDbContext();
            return context.BookStats.Where(bs => bs.BookId == bookId).ToList();
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
                ErrorHandler.LogAndThrow(2000);
            }
            var firstPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if(firstPage == null || firstPage.Id == 0)
            {
                ErrorHandler.LogAndThrow(2010);
            }
            var languageUser = context.LanguageUsers
                .Where(lu => lu.LanguageId == book.LanguageId && lu.UserId == userId)
                .FirstOrDefault();
            if (firstPage == null || firstPage.Id == 0)
            {
                ErrorHandler.LogAndThrow(2020);
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
                ErrorHandler.LogAndThrow(2030);
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
        public BookUser? BookUserFetch(PageUser pageUser)
        {
            var context = _dbContextFactory.CreateDbContext();
            

            return (from pu in context.PageUsers
                    join p in context.Pages on pu.PageId equals p.Id
                    join b in context.Books on p.BookId equals b.Id
                    join bu in context.BookUsers on b.Id equals bu.BookId
                    where (pu.PageId == pageUser.PageId)
                    select bu)
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
                ErrorHandler.LogAndThrow(2040);
            }
            // do the actual page parsing
            PageHelper.ParseParagraphsFromPageAndSave(
                context, newPage, language, commonWordsInLanguage);
            return (int)newPage.Id;
        }
        public void PageUpdateBookmark(int bookUserId, int currentPageId)
        {
            if (bookUserId == 0)
            {
                ErrorHandler.LogAndThrow(1120);
            }
            if (currentPageId == 0)
            {
                ErrorHandler.LogAndThrow(1130);
            }
            var context = _dbContextFactory.CreateDbContext();
            var bookUser = context.BookUsers.FirstOrDefault(x => x.Id == bookUserId);
            if (bookUser == null)
            {
                ErrorHandler.LogAndThrow(2050, [$"bookUserId: {bookUserId}"]);
            }
            bookUser.CurrentPageID = currentPageId;
            context.SaveChanges();
        }
        #endregion

        #region PageUser
        public void PageUserClearPage(PageUser pageUser, LanguageUser languageUser)
        {
            if (pageUser == null) 
            {
                ErrorHandler.LogAndThrow(1140); 
            }
            var context = _dbContextFactory.CreateDbContext();
            
            var wordUsers = (from pu in context.PageUsers
                             join p in context.Pages on pu.PageId equals p.Id
                             join pp in context.Paragraphs on p.Id equals pp.PageId
                             join s in context.Sentences on pp.Id equals s.ParagraphId
                             join t in context.Tokens on s.Id equals t.SentenceId
                             join w in context.Words on t.WordId equals w.Id
                             join wu in context.WordUsers on w.Id equals wu.WordId
                             where (pu.PageId == pageUser.PageId && wu.LanguageUserId == languageUser.Id
                                 && wu.Status == AvailableWordUserStatus.UNKNOWN)
                             select wu).ToList();

            foreach (var wu in wordUsers)
            {
                wu.Status = AvailableWordUserStatus.WELLKNOWN;
            }
            context.SaveChanges();
        }
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
            catch (IdiomaticaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                string[] args = [
                    $"page.Id = {page.Id}",
                    $"bookUser.Id = {bookUser.Id}",
                    ];
                ErrorHandler.LogAndThrow(3000, args, ex);
                throw; // you'll never get here
            }
        }
        public PageUser? PageUserFetchById(int pageId, int languageUserId)
        {
            // note this doesn't fetch the word_user

            var context = _dbContextFactory.CreateDbContext();
            return context.PageUsers
                .Where(pu => pu.BookUser.LanguageUserId == languageUserId
                    && pu.PageId == pageId)
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
        public void PageUserUpdateReadDate(int id, DateTime readDate)
        {
            var context = _dbContextFactory.CreateDbContext();
            var pu = context.PageUsers.Where(pu => pu.Id == id).FirstOrDefault();
            if (pu == null) 
            {
                ErrorHandler.LogAndThrow(2100);
            }
            pu.ReadDate = readDate;
            context.SaveChanges();
            return;
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
            if (id == 0)
            {
                ErrorHandler.LogAndThrow(1150);
            }
            // first pull the existing one from the database
            var context = _dbContextFactory.CreateDbContext();
            var dbWordUser = context.WordUsers.Where(x => x.Id == id).FirstOrDefault();
            if (dbWordUser == null)
            {
                ErrorHandler.LogAndThrow(2060);
            }
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
