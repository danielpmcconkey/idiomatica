using Logic.Telemetry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Model;
using Model.DAL;
using PragmaticSegmenterNet.Languages;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;

namespace Logic.Services
{
    public class BookService
    {
        private User? _loggedInUser = null;
        private UserService _userService;
        private ErrorHandler _errorHandler;
        private DeepLService _deepLService;
        private NullHandler _nullHandler;

        #region public read-only properties

        public Dictionary<string, WordUser> AllWordUsersInPage
        {
            get
            {
                if (_allWordUsersInPage == null) return new Dictionary<string, WordUser>();
                return _allWordUsersInPage;
            }
        }
        public string? BookTitle { get
            {
                if (_book == null) return "";
                return _book.Title;
            } }
        public int? BookTotalPageCount
        {
            get { return _bookTotalPageCount; }
        }
        public int? BookCurrentPageNum
        {
            get 
            {
                if(_currentPage == null) return 0;
                return _currentPage.Ordinal;
            }
        }
        public int BookCurrentPageId
        {
            get
            {
                if (_currentPage == null || _currentPage.Id == null) return 0;
                return (int)_currentPage.Id;
            }
        }
        public List<BookListRow> BookListRows
        {
            get
            {
                return _bookListRows == null ? new List<BookListRow>() : _bookListRows;
            }
        }
        public bool IsDataInitRead { get { return _isDataInitRead; } }
        public bool IsDataInitBookList { get { return _isDataInitBookList; } }
        public string? LanguageFromCode
        {
            get
            {
                return (_languageFromCode == null) ? "" : _languageFromCode.Code;
            }
        }
        public string? LanguageToCode
        {
            get
            {
                return (_languageToCode == null) ? "" : _languageToCode.Code;
            }
        }
        public const int LoadingDelayMiliseconds = 1000;
        public List<Paragraph> Paragraphs
        {
            get 
            { 
                if (_paragraphs != null) return _paragraphs;
                else return new List<Paragraph>();
            }
        }
        public long? BookListFirstRowShown 
        { 
            get {
                if (_bookListRows == null || !_bookListRows.Any()) return 0L;
                else return _bookListRows.Min(x => x.RowNumber);
            } 
        }
        public long? BookListLastRowShown
        {
            get
            {
                if (_bookListRows == null || !_bookListRows.Any()) return 0L;
                else return _bookListRows.Max(x => x.RowNumber);
            }
        }
        public long? BookListTotalRowsAtCurrentFilter
        {
            get
            {
                return _bookListTotalRowsAtCurrentFilter;
            }
        }


        #endregion

        #region thread checking bools

        private bool _isDataInitRead = false;
        private bool _isDataInitBookList = false;
        private bool _isLoadingBook = false;
        private bool _isLoadingBookUser = false;
        private bool _isLoadingLanguageUser = false;
        private bool _isLoadingBookUserStats = false;
        private bool _isLoadingLoggedInUser = false;
        private bool _isLoadingAllWordUsersInPage = false;
        private bool _isLoadingWordsInPage = false;
        private bool _isLoadingPageUser = false;
        private bool _isLoadingParagraphs = false;
        private bool _isLoadingCurrentPage = false;
        private bool _isLoadingTotalPageCount = false;
        private bool _isLoadingSentences = false;
        private bool _isLoadingTokens = false;
        private bool _isLoadingLanguage = false;
        private bool _isLoadingLanguageToCode = false;
        private bool _isLoadingLanguageFromCode = false;

        #endregion

        #region active book and page data

        /// <summary>
        ///     a list of common words from the database and put it in a dictionary 
        ///     so that we don't always have to go back to the database just to get
        ///     the word ID for every single word
        /// </summary>
        private Dictionary<string, Word>? _allWordsInPage = null;
        private Dictionary<string, WordUser>? _allWordUsersInPage = null;
        private Book? _book = null;
        private int? _bookId = null;
        private List<BookListRow>? _bookListRows = null;
        private long? _bookListTotalRowsAtCurrentFilter = null;
        private int? _bookTotalPageCount = null;
        private BookUser? _bookUser = null;
        private List<BookUserStat>? _bookUserStats = null;
        private Page? _currentPage = null;
        private PageUser? _currentPageUser = null;
        private Language? _language = null;
        /// <summary>
        /// the language the book is written in
        /// </summary>
        private LanguageCode? _languageFromCode = null;
        /// <summary>
        /// The user's preferred UI language
        /// </summary>
        private LanguageCode? _languageToCode = null;
        private LanguageUser? _languageUser = null;
        private List<Paragraph>? _paragraphs = null;
        private List<Sentence>? _sentences = null;
        private List<Token>? _tokens = null;

        #endregion

        #region properties for sorting the book lists

        public bool IsBrowse = true;
        public int SkipRecords = 0;
        public string? TagsFilter = null;
        public string? LcFilterCode = null;
        public string? TitleFilter = null;
        public int? OrderBy = 4;    // title
        public bool SortAscending = true;
        public const int BookListRowsToDisplay = 10;
        public Dictionary<string, LanguageCode> LanguageOptions = new Dictionary<string, LanguageCode>();
        public Dictionary<int, string> OrderByOptions = new Dictionary<int, string>();

        #endregion

        public BookService(ErrorHandler errorHandler, DeepLService deepLService, 
            UserService userService, NullHandler nullHandler)
        {
            _errorHandler = errorHandler;
            _deepLService = deepLService;
            _userService = userService;
            _nullHandler = nullHandler;
        }


        #region init methods

        public async Task InitDataBookList(IdiomaticaContext context, bool isBrowse)
        {
            try
            {
                IsBrowse = isBrowse;
                _loggedInUser = await UserGetLoggedInAsync(context);
                await BookListResetAsync(context);
                await LanguageCodeDictionaryPopulate(context);
                OrderByOptions[1] = "Book ID";
                OrderByOptions[2] = "Language";
                OrderByOptions[3] = "Completed";
                OrderByOptions[4] = "Title";
                OrderByOptions[5] = "Total Pages";
                OrderByOptions[6] = "Total Word Count";
                OrderByOptions[7] = "Distinct Word Count";

                _isDataInitBookList = true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Deleteme()
        {
            User loggedInUserDenulled = _nullHandler.ThrowIfNull<User>(_loggedInUser);
            int loggedInUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(loggedInUserDenulled.Id);
            var languageUserDenulled = _nullHandler.ThrowIfNull<LanguageUser>(_languageUser);
            int languageUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(languageUserDenulled.Id);
            var allWordsInPageDenulled = _nullHandler.ThrowIfNullOrEmptyDict(_allWordsInPage);
            var allWordUsersInPageDenulled = _nullHandler.ThrowIfNullOrEmptyDict(_allWordUsersInPage);
            BookUser bookUserDenulled = _nullHandler.ThrowIfNull<BookUser>(_bookUser);
            Language languageDenulled = _nullHandler.ThrowIfNull<Language>(_language);
            int languageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(languageDenulled.Id);
            int bookIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(_bookId);

        }

        public async Task InitDataRead(IdiomaticaContext context, UserService userService, int bookId)
        {
            _userService = userService;
            _bookId = bookId;

            // tier 0 tasks, not dependent on anything
            var t_loggedInUser = UserGetLoggedInAsync(context);
            var t_book = BookGetAsync(context, (int)_bookId);
            var t_bookTotalPageCount = BookGetTotalPagesAsync(context, (int)_bookId);

            Task.WaitAll([t_loggedInUser, t_book, t_bookTotalPageCount]);

            _loggedInUser = t_loggedInUser.Result;
            _languageToCode = _userService.GetUiLanguageCode();
            _book = t_book.Result;
            _bookTotalPageCount = t_bookTotalPageCount.Result;

            // create non-null values to use in later look-ups
            Book bookDenulled = _nullHandler.ThrowIfNull<Book>(_book);
            int bookIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(_bookId);
            User loggedInUserDenulled = _nullHandler.ThrowIfNull<User>(_loggedInUser);
            int loggedInUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(loggedInUserDenulled.Id);
            int languageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(bookDenulled.LanguageId);

            // tier 1 tasks, dependent on tier 0
            var t_bookUser = BookUserGetAsync(context, bookIdDenulled, loggedInUserIdDenulled);
            var t_bookUserStats = BookUserGetStatsAsync(context, bookIdDenulled, loggedInUserIdDenulled);
            var t_languageUser = LanguageUserGetAsync(context, languageIdDenulled, loggedInUserIdDenulled);
            var t_language = LanguageGetAsync(context, languageIdDenulled);

            Task.WaitAll([t_bookUser, t_bookUserStats, t_languageUser, t_language]);

            _bookUser = t_bookUser.Result;
            _bookUserStats = t_bookUserStats.Result;
            _languageUser = t_languageUser.Result;
            _language = t_language.Result;

            // create non-null values to use in later look-ups
            BookUser bookUserDenulled = _nullHandler.ThrowIfNull<BookUser>(_bookUser);
            int currentPageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(bookUserDenulled.CurrentPageID);
            int languageUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(bookUserDenulled.LanguageUserId);
            Language languageDenulled = _nullHandler.ThrowIfNull<Language>(_language);
            string languageCodeDenulled = _nullHandler.ThrowIfNullOrEmptyString(languageDenulled.Code);

            // tier 2, dependent on tier 1
            var t_currentPageUser = PageUserGetOnReadOpenAsync(
                context, currentPageIdDenulled, languageUserIdDenulled);
            var t_languageFromCode = LanguageGetByCodeAsync(context, languageCodeDenulled);

            Task.WaitAll([t_currentPageUser, t_languageFromCode]);

            _currentPageUser = t_currentPageUser.Result;
            _languageFromCode = t_languageFromCode.Result;

            // create non-null values to use in later look-ups
            PageUser currentPageUserDenulled = _nullHandler.ThrowIfNull<PageUser>(_currentPageUser);
            int pageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(currentPageUserDenulled.PageId);

            // tier 3, dependent on tier 2
            await PageResetDataForRead(context, pageIdDenulled);

            // fin
            _isDataInitRead = true;
        }

        #endregion

        #region public interface

        public async Task<bool> BookDoesContainTagLikeText(IdiomaticaContext context, int? bookId, string? text)
        {
            if (bookId == null || bookId < 1 || text == null) return false;
            // pull all tags
            var tags = await DataCache.BookTagsByBookIdReadAsync((int)bookId, context);
            var filteredTags = tags.Where(
                x => x.Tag != null && x.Tag.Contains(text, StringComparison.OrdinalIgnoreCase));
            return filteredTags.Any();
        }
        public async Task<int> BookCreateAndSaveAsync(
            IdiomaticaContext context, string title, string languageCode, string url, string text)
        {
            const int _targetCharCountPerPage = 1378;// this was arrived at by DB query after conversion
            // sanitize and validate input
            var titleT = _nullHandler.ThrowIfNullOrEmptyString(title.Trim());
            var languageCodeT = _nullHandler.ThrowIfNullOrEmptyString(languageCode.Trim());
            var urlT = (url == null) ? "" : url.Trim();
            var textT = _nullHandler.ThrowIfNullOrEmptyString(text.Trim().Replace('\u00A0', ' '));

            // pull language from the db
            var language = _nullHandler.ThrowIfNull<Language>(
                await DataCache.LanguageByCodeReadAsync(languageCodeT, context));
            

            // divide text into paragraphs
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var paragraphSplitsRaw = parser.SegmentTextByParagraphs(textT);
            string[] paragraphSplits = _nullHandler.ThrowIfNullOrEmptyArray<string>(paragraphSplitsRaw);

            // add the book to the DB so you can save pages using its ID
            Book book = new Book()
            {
                Title = titleT,
                SourceURI = urlT,
                LanguageId = _nullHandler.ThrowIfNullOrZeroInt(language.Id),
            };
            bool didSaveBook = await DataCache.BookCreateAsync(book, context);
            if (!didSaveBook || book.Id == null || book.Id < 1)
            {
                _errorHandler.LogAndThrow(2090);
                return -1;
            }

            // pull a list of common words from the database and put it
            // in a dictionary so that we don't always have to go back to the
            // database just to get the word ID for every single word
            var commonWordsInLanguage = _nullHandler.ThrowIfNullOrEmptyDict(
                await WordFetchCommonDictForLanguageAsync(context, 
                    _nullHandler.ThrowIfNullOrZeroInt(language.Id)));

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
                        int newPageId = await PageCreateAndSaveDuringBookCreateAsync(
                            context, (int)book.Id, currentPageCount,
                                    pageText, language, commonWordsInLanguage);


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
                int newPageId = await PageCreateAndSaveDuringBookCreateAsync(
                    context, (int)book.Id, currentPageCount,
                            pageText, language, commonWordsInLanguage);
            }

            return (int)book.Id;
        }
        public async Task BookListRowsFilterAndSort(IdiomaticaContext context)
        {
            SkipRecords = 0;
            await BookListResetAsync(context);
        }
        public async Task BookListRowsNext(IdiomaticaContext context)
        {
            SkipRecords += BookListRowsToDisplay;
            await BookListResetAsync(context);
        }
        public async Task BookListRowsPrevious(IdiomaticaContext context)
        {
            SkipRecords -= BookListRowsToDisplay;
            if (SkipRecords < 0) SkipRecords = 0;
            await BookListResetAsync(context);
        }
        public void BookStatsCreateAndSave(IdiomaticaContext context, int bookId)
        {
            if (bookId < 1)
            {
                _errorHandler.LogAndThrow(1100);
            }
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
                _errorHandler.LogAndThrow(2110);
            }
        }
        public async Task BookTagAdd(IdiomaticaContext context, int? bookId, int? userId, string? tag)
        {

            string trimmedTag = _nullHandler.ThrowIfNullOrEmptyString(tag).Trim().ToLower();
            if (trimmedTag == string.Empty) return;

            int bookIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(bookId);
            int userIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(userId);
            DateTimeOffset created = DateTimeOffset.UtcNow;
            // check if this user already saved this tag
            var existingTag = context.BookTags.Where(x =>
                    x.BookId == bookIdDenulled &&
                    x.UserId == userIdDenulled &&
                    x.Tag == trimmedTag)
                .FirstOrDefault();
            if (existingTag != null) return;
            var newTag = new BookTag()
            {
                BookId = bookId,
                UserId = userId,
                Tag = trimmedTag,
                Created = created,
                IsPersonal = true
            };
            bool didSave = await DataCache.BookTagCreateAsync(newTag, context);
            if (!didSave || newTag.Id == null || newTag.Id < 1)
            {
                _errorHandler.LogAndThrow(2440);
            }
        }
        public async Task BookTagRemove(IdiomaticaContext context, int? bookId, int? userId, string? tag)
        {
            
            string trimmedTag = _nullHandler.ThrowIfNullOrEmptyString(tag).Trim().ToLower();
            if (trimmedTag == string.Empty) return;
            int bookIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(bookId);
            int userIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(userId);
            await DataCache.BookTagDelete((bookIdDenulled, userIdDenulled, trimmedTag), context);
        }
        public async Task<List<BookTag>> BookTagsGetByBookIdAndUserId(
            IdiomaticaContext context, int? bookId, int? userId)
        {
            if (bookId == null || bookId < 1) return new List<BookTag>();
            if (userId == null || userId < 1) return new List<BookTag>();
            var tags = await DataCache.BookTagsByBookIdAndUserIdReadAsync(((int)bookId, (int)userId), context);
            return tags;
        }
        public async Task BookUserAddAsync(IdiomaticaContext context, int bookId)
        {
            User loggedInUserDenulled = _nullHandler.ThrowIfNull<User>(_loggedInUser);
            int loggedInUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(loggedInUserDenulled.Id);
            var bookUser = await BookUserGetAsync(context, bookId, loggedInUserIdDenulled);
            if (bookUser != null)
            {
                bookUser.IsArchived = false;
                await DataCache.BookUserUpdateAsync(bookUser, context);
            }
            else
            {
                await BookUserCreateAndSaveAsync(context, bookId, loggedInUserIdDenulled);
            }

            // now pull a fresh copy of the book list
            await BookListResetAsync(context);
        }
        public async Task BookUserArchiveAsync(IdiomaticaContext context, int bookId)
        {
            User loggedInUserDenulled = _nullHandler.ThrowIfNull<User>(_loggedInUser);
            int loggedInUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(loggedInUserDenulled.Id);
            
            if (bookId < 1)
            {
                _errorHandler.LogAndThrow(1410);
                return;
            }
            var bookUser = _nullHandler.ThrowIfNull<BookUser>(
                await BookUserGetAsync(context, bookId, loggedInUserIdDenulled));
            
            bookUser.IsArchived = true;
            await DataCache.BookUserUpdateAsync(bookUser, context);

            // now pull a fresh copy of the book list
            await BookListResetAsync(context);
        }
        public async Task<int> BookUserCreateAndSaveAsync(IdiomaticaContext context, int bookId, int userId)
        {
            Book book = _nullHandler.ThrowIfNull<Book>(
                await DataCache.BookByIdReadAsync(bookId, context));
            int languageIdDebulled = _nullHandler.ThrowIfNullOrZeroInt(book.LanguageId);
            
            book.Pages = await DataCache.PagesByBookIdReadAsync(bookId, context);

            var firstPage = _nullHandler.ThrowIfNull<Page>(
                book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault());
            
            var languageUser = _nullHandler.ThrowIfNull<LanguageUser>(
                await DataCache.LanguageUserByLanguageIdAndUserIdReadAsync(
                    (languageIdDebulled, userId), context));


            
            // make sure that bookUser doesn't already exist before creating it
            var existingBookUser = await DataCache.BookUserByBookIdAndUserIdReadAsync(
                (bookId, _nullHandler.ThrowIfNullOrZeroInt(languageUser.UserId)), context);
            if(existingBookUser != null && existingBookUser.Id != null) 
            {
                // dude already exists. Just return it. 
                // update the stats first, just in case
                await BookUserStatsUpdate(context, (int)existingBookUser.Id);
                return (int)existingBookUser.Id;
            }

            BookUser bookUser = new BookUser()
            {
                BookId = bookId,
                CurrentPageID = _nullHandler.ThrowIfNullOrZeroInt(firstPage.Id),
                LanguageUserId = _nullHandler.ThrowIfNullOrZeroInt(languageUser.Id)
            };

            bool didSaveBookUser = await DataCache.BookUserCreateAsync(bookUser, context);
            if (!didSaveBookUser || bookUser.Id == null || bookUser.Id < 1)
            {
                _errorHandler.LogAndThrow(2250);
                return -1;
            }

            // now update BookUserStats
            await BookUserStatsUpdate(context, _nullHandler.ThrowIfNullOrZeroInt(bookUser.Id));

            return (int)bookUser.Id;
        }
        public async Task BookUserUpdateStats(IdiomaticaContext context, int? bookId)
        {
            _bookListRows = new List<BookListRow>();
            if (bookId == null) return;
            if (_loggedInUser == null || _loggedInUser.Id == null || _loggedInUser.Id < 1) return;
            var bookUser = await DataCache.BookUserByBookIdAndUserIdReadAsync(
                    ((int)bookId, (int)_loggedInUser.Id), context);
            if (bookUser == null || bookUser.Id == null || bookUser.Id < 1) return;
            await BookUserStatsUpdate(context, (int)bookUser.Id);
            await BookListResetAsync(context);
        }

        public async Task<IQueryable<LanguageCode>> LanguageCodeFetchOptionsForLearning(IdiomaticaContext context)
        {
            // this isn't worth caching

            Expression<Func<LanguageCode, bool>> filter = (x => x.IsImplementedForLearning == true);
            return context.LanguageCodes
                .Where(filter).OrderBy(x => x.LanguageName);
        }
        public async Task LanguageCodeDictionaryPopulate(IdiomaticaContext context)
        {
            var dbLanguageCodes = await LanguageCodeFetchOptionsForLearning(context);
            foreach (var lc in dbLanguageCodes)
            {
                LanguageOptions.Add(_nullHandler.ThrowIfNullOrEmptyString(lc.Code), lc);
            }
        }
        public async Task PageUserClearPageAndMove(IdiomaticaContext context, int targetPageNum)
        {
            User loggedInUserDenulled = _nullHandler.ThrowIfNull<User>(_loggedInUser);
            int loggedInUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(loggedInUserDenulled.Id);
            Page currentPageDenulled = _nullHandler.ThrowIfNull<Page>(_currentPage);
            int currentPageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(currentPageDenulled.Id);
            PageUser currentPageUserDenulled = _nullHandler.ThrowIfNull<PageUser>(_currentPageUser);
            int currentPageUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(currentPageUserDenulled.Id);
            int currentPagIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(currentPageUserDenulled.PageId);

            await DataCache.WordUsersUpdateStatusByPageIdAndUserIdAndStatus(
                currentPageIdDenulled, loggedInUserIdDenulled,
                AvailableWordUserStatus.UNKNOWN, AvailableWordUserStatus.WELLKNOWN, context);

            // now move forward, if there's another page
            if (targetPageNum <= _bookTotalPageCount) // remember pages are 1-indexed
            {
                await PageMove(context, targetPageNum);
            }
            else
            {
                // mark the previous page as read because you didn't do it in the PageMove function
                await PageUserMarkAsReadAsync(context, currentPageUserIdDenulled);
                // refresh the word user cache
                await PageResetDataForRead(context, currentPagIdDenulled);
            }
        }
        public async Task PageMove(IdiomaticaContext context, int targetPageNum)
        {
            _isDataInitRead = false;

            LanguageUser languageUserDenulled = _nullHandler.ThrowIfNull<LanguageUser>(_languageUser);
            int languageUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(languageUserDenulled.Id);
            PageUser currentPageUserDenulled = _nullHandler.ThrowIfNull<PageUser>(_currentPageUser);
            int currentPageUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(currentPageUserDenulled.Id);
            BookUser bookUserDenulled = _nullHandler.ThrowIfNull<BookUser>(_bookUser);
            int bookUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(bookUserDenulled.Id);
            int bookIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(_bookId);


            // mark the previous page as read before moving on
            await PageUserMarkAsReadAsync(context, currentPageUserIdDenulled);

            if (targetPageNum < 1) return;
            if (targetPageNum > _bookTotalPageCount)
                return;

            // reload the current page user with the new target
            _currentPageUser = await PageUserGetByOrderWithinBookAsync(
                context, languageUserIdDenulled, targetPageNum, bookIdDenulled);
            if (_currentPageUser == null) return;

            int currentPageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(_currentPageUser.PageId);
            
            await PageResetDataForRead(context, currentPageIdDenulled);
            await BookUserUpdateBookmarkAsync(context, bookUserIdDenulled, currentPageIdDenulled);
            _isDataInitRead = true;
        }
        public async Task<(string input, string output)> ParagraphTranslate(
            IdiomaticaContext context, Paragraph pp, string fromCode, string toCode)
        {
            var ppIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(pp.Id);

            List<string> sentences = _nullHandler.ThrowIfNullOrEmptyList<Sentence>(pp.Sentences)
                .OrderBy(x => x.Ordinal)
                .Select(s => s.Text ?? "").ToList<string>();
            string input = String.Join(" ", sentences);
            string output = "";


            // see if the translation already exists
            if (pp.ParagraphTranslations == null)
            {
                pp.ParagraphTranslations = await DataCache.ParagraphTranslationsByParargraphIdReadAsync(
                    ppIdDenulled, context);
            }
            var currentTranslation = pp.ParagraphTranslations
                .Where(x => x.Code == toCode)
                .FirstOrDefault();
            if (currentTranslation != null && currentTranslation.TranslationText != null)
            {
                output = currentTranslation.TranslationText;
            }
            else
            {
                var deeplResult = _deepLService.Translate(input, fromCode, toCode);
                if (deeplResult is not null)
                {
                    output = deeplResult;
                    // add to the DB
                    ParagraphTranslation ppt = new ParagraphTranslation()
                    {
                        ParagraphId = ppIdDenulled,
                        Code = toCode,
                        TranslationText = deeplResult
                    };
                    bool didSave = await DataCache.ParagraphTranslationCreateAsync(ppt, context);
                    if (!didSave || ppt.Id == null || ppt.Id < 1)
                    {
                        _errorHandler.LogAndThrow(2340);
                        return ("", "");
                    }
                    pp.ParagraphTranslations.Add(ppt);
                }
            }
            return (input, output);
        }
        public async Task<Sentence> SentenceFillChildObjects(IdiomaticaContext context, Sentence sentence)
        {
            User loggedInUserDenulled = _nullHandler.ThrowIfNull<User>(_loggedInUser);
            int loggedInUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(loggedInUserDenulled.Id);
            var languageUserDenulled = _nullHandler.ThrowIfNull<LanguageUser>(_languageUser);
            int languageUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(languageUserDenulled.Id);
            var allWordsInPageDenulled = _nullHandler.ThrowIfNullOrEmptyDict(_allWordsInPage);
            var allWordUsersInPageDenulled = _nullHandler.ThrowIfNullOrEmptyDict(_allWordUsersInPage);
            if (sentence.Tokens is null || sentence.Tokens.Count == 0)
            {
                // we shouldn't be here because tokens should have been added in the read page
                sentence.Tokens = await DataCache.TokensBySentenceIdReadAsync(
                    _nullHandler.ThrowIfNullOrZeroInt(sentence.Id), context);
                // now get the words
                foreach (var t in sentence.Tokens)
                {
                    var dictEntry = allWordsInPageDenulled.Where(w => w.Value.Id == t.WordId).FirstOrDefault();
                    if (dictEntry.Value == null)
                    {
                        _errorHandler.LogAndThrow(2370);
                        return sentence;
                    }
                    t.Word = dictEntry.Value;
                }
            }
            // make sure each token has a word user and is in the dictionary
            // doing it here allows there to be continuity between the same word
            // in different paragraphs, so that, when you edit one, it updates
            // the other.
            foreach (var token in sentence.Tokens)
            {
                Token tokenDenulled = _nullHandler.ThrowIfNull<Token>(token);
                int tokenWordIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(tokenDenulled.WordId);
                Word tokenWordDenulled = _nullHandler.ThrowIfNull<Word>(token.Word);
                string tokenWordTextLCDenulled = _nullHandler.ThrowIfNullString(tokenWordDenulled.TextLowerCase);

                if (!allWordUsersInPageDenulled.ContainsKey(tokenWordTextLCDenulled))
                {
                    
                    // check if there's already a wordUser
                    var wordUser = DataCache.WordUserByWordIdAndUserIdReadAsync((
                        tokenWordIdDenulled,
                        loggedInUserIdDenulled
                        ), context).Result;
                    if (wordUser is null)
                    {
                        // need to create
                       
                        wordUser = new WordUser()
                        {
                            LanguageUserId = languageUserIdDenulled,
                            WordId = tokenWordIdDenulled,
                            Status = AvailableWordUserStatus.UNKNOWN,
                            Translation = string.Empty
                        };
                        var isSaved = DataCache.WordUserCreateAsync(wordUser, context).Result;
                        if (!isSaved || wordUser.Id < 1)
                        {
                            _errorHandler.LogAndThrow(2380);
                            return sentence;
                        }
                    }

                    allWordUsersInPageDenulled[tokenWordTextLCDenulled] = wordUser;
                }
            }
            return sentence;
        }
        public async Task<(Token? t, WordUser? wu)> TokenGetChildObjects(IdiomaticaContext context, Token token)
        {
            WordUser wu = new WordUser();
            if (token.WordId == null || token.WordId < 1)
            {
                _errorHandler.LogAndThrow(1580);
                return (token, wu);
            }
            if (_loggedInUser == null || _loggedInUser.Id == null || _loggedInUser.Id < 1)
            {
                _errorHandler.LogAndThrow();
                return (token, wu);
            }
            if (_allWordsInPage == null || !_allWordsInPage.Any())
            {
                _errorHandler.LogAndThrow(1580);
                return (token, wu);
            }
            if (_allWordUsersInPage == null || !_allWordUsersInPage.Any())
            {
                _errorHandler.LogAndThrow();
                return (token, wu);
            }
            if (token.Word == null)
            {
                // need to pull the word
                token.Word = await WordGetByIdAsync(context, (int)token.WordId);
                var wordEntry = _allWordsInPage.Where(w => w.Value.Id == token.WordId).FirstOrDefault();
                if (wordEntry.Value != null)
                {
                    token.Word = wordEntry.Value;
                }
            }
            if (token.Word == null || token.Word.Id == null || token.Word.Id < 1)
            {
                _errorHandler.LogAndThrow(2390);
                return (token, wu);
            }
            if (string.IsNullOrEmpty(token.Word.TextLowerCase))
            {
                _errorHandler.LogAndThrow();
                return (token, wu);
            }
            if (_allWordUsersInPage.ContainsKey(token.Word.TextLowerCase))
            {
                wu = AllWordUsersInPage[token.Word.TextLowerCase];
            }
            else
            {
                var wuFromDb = await DataCache.WordUserByWordIdAndUserIdReadAsync(
                    ((int)token.WordId, (int)_loggedInUser.Id), context);
                if (wuFromDb == null)
                {
                    _errorHandler.LogAndThrow();
                    return (token, wu);
                }
            }
            if (wu == null || wu.Id < 1)
            {
                _errorHandler.LogAndThrow(2400);
                return (token, wu);
            }
            return (token, wu);
        }
#if DEBUG
        /// <summary>
        /// this is only used for the test bench to provide a logged in user outside of the standard app flow
        /// </summary>
        public void SetLoggedInUser(User user)
        {
            _loggedInUser = user;
        }
#endif
        public async Task<List<(string language, int wordCount)>> WordsGetListOfReadCount(
            IdiomaticaContext context, int? userId)
        {
            List<(string language, int wordCount)> returnList = new List<(string language, int wordCount)>();
            var languageUsers = await LanguageUsersAndLanguageGetByUserIdAsync(context, userId);
            foreach (var languageUser in languageUsers)
            {
                if (languageUser.Language == null || languageUser.Language.Name == null) continue;
                var count = (from lu in context.LanguageUsers
                             join bu in context.BookUsers on lu.Id equals bu.LanguageUserId
                             join pu in context.PageUsers on bu.Id equals pu.BookUserId
                             join p in context.Pages on pu.PageId equals p.Id
                             join pp in context.Paragraphs on p.Id equals pp.PageId
                             join s in context.Sentences on pp.Id equals s.ParagraphId
                             join t in context.Tokens on s.Id equals t.SentenceId
                             where pu.ReadDate != null
                                && lu.Id == languageUser.Id
                             select t).Count();
                returnList.Add((languageUser.Language.Name, count));
            }
            return returnList;
        }
        /// <summary>
        /// Updates the WordUser but doesn't update all the caches. reset any 
        /// caches that are important to you
        /// </summary>
        /// <returns>true if the word user is changed and has been updated. false if not</returns>
        public async Task WordUserSaveModalDataAsync(IdiomaticaContext context,
            int id, AvailableWordUserStatus newStatus, string translation)
        {
            if (id == 0)
            {
                _errorHandler.LogAndThrow(1150);
                return;
            }
            // first pull the existing one from the database
            var dbWordUser = await DataCache.WordUserByIdReadAsync(id, context);
            if (dbWordUser == null)
            {
                _errorHandler.LogAndThrow(2060);
                return;
            }

            dbWordUser.Status = newStatus;
            dbWordUser.Translation = translation;
            dbWordUser.StatusChanged = DateTime.Now;
            await DataCache.WordUserUpdateAsync(dbWordUser, context);
        }


        #endregion

        #region Book
        
        private async Task<Book?> BookGetAsync(IdiomaticaContext context, int bookId)
        {
            if (_book == null)
            {
                if (_isLoadingBook == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return BookGetAsync(context, bookId).Result;
                }
                _isLoadingBook = true;
                _book = await DataCache.BookByIdReadAsync(bookId, context);
                _isLoadingBook = false;
            }
            return _book;
        }
        private async Task<int?> BookGetTotalPagesAsync(IdiomaticaContext context, int bookId)
        {
            if (_bookTotalPageCount == null)
            {
                if (_isLoadingTotalPageCount == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return BookGetTotalPagesAsync(context, bookId).Result;
                }
                _isLoadingTotalPageCount = true;
                var dbVal = await DataCache.BookStatByBookIdAndStatKeyReadAsync((bookId, AvailableBookStat.TOTALPAGES), context);
                int outVal = 0;
                if(dbVal != null) int.TryParse(dbVal.Value, out outVal);
                _bookTotalPageCount = outVal;
                _isLoadingTotalPageCount = false;
            }
            return _bookTotalPageCount;
        }

        #endregion

        #region BookListRow
        private async Task BookListResetAsync(IdiomaticaContext context)
        {
            try
            {
                LanguageCode? lcFilter = null;
                if (LcFilterCode != null) LanguageOptions.TryGetValue(LcFilterCode, out lcFilter);

                AvailableBookListSortProperties sortProperty = AvailableBookListSortProperties.TITLE;
                try
                {
                    if (OrderBy != null) sortProperty = (AvailableBookListSortProperties)(int)OrderBy;
                }
                catch
                {
                    // just swallow it, you already assigned sortProperty to default
                }

                if(_loggedInUser == null || _loggedInUser.Id == null || _loggedInUser.Id < 1)
                {
                    _errorHandler.LogAndThrow();
                    return;
                }

                var powerQueryResults = await DataCache.BookListRowsPowerQueryAsync(
                    (int)_loggedInUser.Id,
                    BookListRowsToDisplay,
                    SkipRecords,
                    !IsBrowse,      // shouldShowOnlyInShelf
                    TagsFilter,
                    lcFilter,
                    TitleFilter,
                    sortProperty,
                    SortAscending,
                    context);
                _bookListRows = powerQueryResults.results;
                _bookListTotalRowsAtCurrentFilter = powerQueryResults.count;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region BookStat



        #endregion

        #region bookuser 
        private async Task<BookUser?> BookUserGetAsync(IdiomaticaContext context, int bookId, int userId)
        {
            if (_loggedInUser == null || _loggedInUser.Id == null || _loggedInUser.Id < 1)
            {
                _errorHandler.LogAndThrow();
                return null;
            }
            if (_bookUser == null)
            {
                if (_isLoadingBookUser == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return BookUserGetAsync(context, bookId, userId).Result;
                }
                _isLoadingBookUser = true;

                var bookUserFromDb = await DataCache.BookUserByBookIdAndUserIdReadAsync(
                    (bookId, userId), context);
                if (bookUserFromDb is null)
                {
                    // try to create it
                    var bookUserId = await BookUserCreateAndSaveAsync(context, bookId, (int)_loggedInUser.Id);
                    if (bookUserId == 0)
                    {
                        _errorHandler.LogAndThrow(5080);
                        return null;
                    }
                    bookUserFromDb = await DataCache.BookUserByBookIdAndUserIdReadAsync(
                        (bookId, userId), context);
                }
                _bookUser = bookUserFromDb;

                _isLoadingBookUser = false;
            }
            return _bookUser;
        }
        private async Task BookUserUpdateBookmarkAsync(IdiomaticaContext context, int bookUserId, int currentPageId)
        {
            if (bookUserId == 0)
            {
                _errorHandler.LogAndThrow(1120);
            }
            if (currentPageId == 0)
            {
                _errorHandler.LogAndThrow(1130);
            }
            
            var bookUser = await DataCache.BookUserByIdReadAsync(bookUserId, context);
            
            if (bookUser == null)
            {
                _errorHandler.LogAndThrow(2050, [$"bookUserId: {bookUserId}"]);
                return;
            }
            bookUser.CurrentPageID = currentPageId;
            await DataCache.BookUserUpdateAsync(bookUser, context);
        }

        #endregion

        #region BookUserStat

        private async Task<List<BookUserStat>?> BookUserGetStatsAsync(
            IdiomaticaContext context, int bookId, int userId)
        {
            if (_bookUserStats == null)
            {
                if (_isLoadingBookUserStats == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return BookUserGetStatsAsync(context, bookId, userId).Result;
                }
                _isLoadingBookUserStats = true;

                var bookUserStatsFromDb = await DataCache.BookUserStatsByBookIdAndUserIdReadAsync(
                    (bookId, userId), context);
                if (bookUserStatsFromDb is null)
                {
                    _errorHandler.LogAndThrow(5090);
                }
                _bookUserStats = bookUserStatsFromDb;
                _isLoadingBookUserStats = false;
                return bookUserStatsFromDb;
            }
            return _bookUserStats;
        }
        private async Task BookUserStatsUpdate(IdiomaticaContext context, int bookUserId)
        {
            var bookUser = await DataCache.BookUserByIdReadAsync(bookUserId, context);

            if (bookUser == null || bookUser.LanguageUserId == null || bookUser.LanguageUserId < 1 
                || bookUser.BookId == null || bookUser.BookId < 1)
            {
                _errorHandler.LogAndThrow();
                return;
            }
            var languageUser = await DataCache.LanguageUserByIdReadAsync((int)bookUser.LanguageUserId, context);
            var allWordsInBook = await DataCache.WordsByBookIdReadAsync((int)bookUser.BookId, context);
            var allWordUsersInBook = await DataCache.WordUsersByBookIdAndLanguageUserIdReadAsync(
                ((int)bookUser.BookId, (int)bookUser.LanguageUserId), context);
            var pageUsers = await DataCache.PageUsersByBookUserIdReadAsync(bookUserId, context);
            var pages = await DataCache.PagesByBookIdReadAsync (bookUserId, context);
            var bookStats = await DataCache.BookStatsByBookIdReadAsync((int)bookUser.BookId, context);
            var totalPagesStatline = bookStats.Where(x => x.Key == AvailableBookStat.TOTALPAGES).FirstOrDefault();
            var totalWordsStatline = bookStats.Where(x => x.Key == AvailableBookStat.TOTALWORDCOUNT).FirstOrDefault();
            var totalDistinctWordsStatline = bookStats.Where(x => x.Key == AvailableBookStat.DISTINCTWORDCOUNT).FirstOrDefault();
            var readPages = (from pu in pageUsers
                             join p in pages on pu.PageId equals p.Id
                             where pu.ReadDate != null
                             orderby p.Ordinal descending
                             select p)
                            .ToList();

            //pageUsers.Where(x => x.ReadDate != null).OrderByDescending(x => x.or);
            int totalPages = 0;
            bool isComplete = false;

            // is complete
            if (totalPagesStatline != null)
            {
                int.TryParse(totalPagesStatline.Value, out totalPages);
                isComplete = (readPages.Count() == totalPages) ? true : false;
            }

            // last page read
            int lastPageRead = 0;
            var latestReadPage = readPages.FirstOrDefault();
            if ((int)readPages.Count > 0 && latestReadPage != null && latestReadPage.Ordinal != null)
                lastPageRead = (int)latestReadPage.Ordinal;

            // progress
            string progress = $"{lastPageRead} / {totalPages}";

            // progress percent
            decimal progressPercent = (totalPages == 0) ? 0 : lastPageRead / (decimal)totalPages;

            // AvailableBookUserStat.TOTALWORDCOUNT
            int totalWordCount = 0;
            if(totalWordsStatline != null)
            {
                int.TryParse(totalWordsStatline.Value, out totalWordCount);
            }

            // AvailableBookUserStat.DISTINCTWORDCOUNT
            int distinctWordCount = allWordsInBook.Count;

            // AvailableBookUserStat.DISTINCTKNOWNPERCENT
            int knownDistinct = allWordUsersInBook
                .Where(x =>
                    x.Status == AvailableWordUserStatus.WELLKNOWN ||
                    x.Status == AvailableWordUserStatus.LEARNED ||
                    x.Status == AvailableWordUserStatus.IGNORED)
                .Count();

            decimal distinctKnownPercent = knownDistinct / (decimal)distinctWordCount;


            // AvailableBookUserStat.TOTALKNOWNPERCENT
            List<BookUserStat> stats = new List<BookUserStat>();
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.ISCOMPLETE,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = isComplete.ToString(),
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.LASTPAGEREAD,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = lastPageRead,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESS,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = progress,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESSPERCENT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = progressPercent,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTKNOWNPERCENT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = distinctKnownPercent,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTWORDCOUNT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = distinctWordCount,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALWORDCOUNT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = totalWordCount,
            }); ;
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALKNOWNPERCENT,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = null,
                ValueNumeric = null,
            }); ;

            // delete the booklistrows cache for this user
            if(languageUser != null && languageUser.Id != null && languageUser.UserId != null)
            {
                DataCache.BookListRowsByUserIdDelete((int)languageUser.Id, context);
                // delete the actual stats from teh database and bookuserstats cache
                await DataCache.BookUserStatsByBookIdAndUserIdDelete(((int)bookUser.BookId, (int)languageUser.UserId), context);
                // add the new stats
                await DataCache.BookUserStatsByBookIdAndUserIdCreate(
                    ((int)bookUser.BookId, (int)languageUser.UserId), stats, context);
            }
           
        }

        #endregion

        #region Language

        private async Task<Language?> LanguageGetAsync(IdiomaticaContext context, int languageId)
        {
            if (_language == null)
            {
                if (_isLoadingLanguage == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return LanguageGetAsync(context, languageId).Result;
                }
                _isLoadingLanguage = true;
                _language = await DataCache.LanguageByIdReadAsync((languageId), context);
                _isLoadingLanguage = false;
            }
            return _language;
        }
        private async Task<LanguageCode?> LanguageGetByCodeAsync(IdiomaticaContext context, string code)
        {
            if (_languageFromCode == null)
            {
                if (_isLoadingLanguageFromCode == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return LanguageGetByCodeAsync(context, code).Result;
                }
                try
                {
                    _isLoadingLanguageFromCode = true;
                    _languageFromCode = await DataCache.LanguageCodeByCodeReadAsync(code, context);
                }
                catch
                {
                    throw;
                }
                _isLoadingLanguageFromCode = false;
            }
            return _languageFromCode;
        }

        #endregion

        #region LanguageCode
        
        #endregion

        #region LanguageUser

        private async Task<LanguageUser?> LanguageUserGetAsync(
            IdiomaticaContext context, int languageId, int userId)
        {
            if (_languageUser == null)
            {
                if (_isLoadingLanguageUser == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return LanguageUserGetAsync(context, languageId, userId).Result;
                }
                _isLoadingLanguageUser = true;
                _languageUser = await DataCache.LanguageUserByLanguageIdAndUserIdReadAsync((languageId, userId), context);
                _isLoadingLanguageUser = false;
            }
            return _languageUser;
        }
        private async Task<List<LanguageUser>> LanguageUsersAndLanguageGetByUserIdAsync(
            IdiomaticaContext context, int? userId)
        {
            if (userId == null || userId < 1) return new List<LanguageUser>();
            return await DataCache.LanguageUsersAndLanguageByUserIdReadAsync((int)userId, context);
        }

        #endregion

        #region Page

        private async Task<int> PageCreateAndSaveDuringBookCreateAsync(
            IdiomaticaContext context, int bookId, int Ordinal, string pageText,
            Language language, Dictionary<string,Word> commonWordsInLanguage)
        {
            var newPage = new Page()
            {
                BookId = bookId,
                Ordinal = Ordinal,
                OriginalText = pageText
            };


            bool isSaved = await DataCache.PageCreateNewAsync(newPage, context);
            if(!isSaved || newPage.Id == null || newPage.Id == 0)
            {
                _errorHandler.LogAndThrow(2040);
                return -1;
            }

            
            // do the actual page parsing
            bool areParagraphsParsed = await ParagraphsParseFromPageAndSaveAsync(
                context, newPage, language, commonWordsInLanguage);
            if(!areParagraphsParsed)
            {
                _errorHandler.LogAndThrow(2260);
                return -1;
            }
            return (int)newPage.Id;
        }
        private async Task<Page?> PageGetCurrentAsync(IdiomaticaContext context, int pageId)
        {
            if (_currentPage == null)
            {
                if (_isLoadingCurrentPage == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return PageGetCurrentAsync(context, pageId).Result;
                }
                _isLoadingCurrentPage = true;
                try
                {
                    _currentPage = await DataCache.PageByIdReadAsync(pageId, context);
                }
                catch
                {
                    throw;
                }
                _isLoadingCurrentPage = false;
            }
            return _currentPage;
        }
        private async Task PageResetDataForRead(IdiomaticaContext context, int pageId)
        {
            if (pageId == 0)
            {
                _errorHandler.LogAndThrow(1240);
                return;
            }
            if (_loggedInUser == null || _loggedInUser.Id == null || _loggedInUser.Id == 0)
            {
                _errorHandler.LogAndThrow(2130);
                return;
            }
            // wipe the old ones out
            _currentPage = null;
            _paragraphs = null;
            _allWordUsersInPage = null;
            _allWordsInPage = null;
            _sentences = null;
            _tokens = null;

            // and rebuild
            var t_currentPage = PageGetCurrentAsync(context, pageId);
            var t_paragraphs = ParagraphsGetByPageIdAsync(context, pageId);
            var t_wordsInPage = WordsGetInPageAsync(context, pageId);
            var t_allWordUsersInPage = WordUsersGetAllInPageAsync(context, pageId, (int)_loggedInUser.Id);
            var t_sentencesInPage = SentencesByPageIdAsync(context, pageId);
            var t_tokensInPage = TokensByPageIdAsync(context, pageId);

            Task.WaitAll([t_currentPage, t_paragraphs, t_wordsInPage, t_allWordUsersInPage, t_sentencesInPage, t_tokensInPage]);

            _currentPage = t_currentPage.Result;
            _paragraphs = t_paragraphs.Result;
            _allWordUsersInPage = t_allWordUsersInPage.Result;
            _allWordsInPage = t_wordsInPage.Result;
            _sentences = t_sentencesInPage.Result;
            _tokens = t_tokensInPage.Result;



            // now knit the paragraph data together
            _paragraphs = _nullHandler.ThrowIfNullOrEmptyList<Paragraph>(_paragraphs)
                .OrderBy(x => x.Ordinal)
                .ToList();
            var pox = _paragraphs[0];
            foreach (var p in _paragraphs)
            {
                p.Sentences = _nullHandler.ThrowIfNullOrEmptyList<Sentence>(_sentences)
                    .Where(s => s.ParagraphId == p.Id)
                    .OrderBy(s => s.Ordinal)
                    .ToList();
                foreach (var s in p.Sentences)
                {
                    s.Tokens = _nullHandler.ThrowIfNullOrEmptyList<Token>(_tokens)
                        .Where(t => t.SentenceId == s.Id)
                        .OrderBy(t => t.Ordinal)
                        .ToList();

                    foreach (var t in s.Tokens)
                    {
                        var wordEntry = _nullHandler.ThrowIfNullOrEmptyDict(_allWordsInPage)
                            .Where(w => w.Value.Id == t.WordId)
                            .FirstOrDefault();
                        if (wordEntry.Value != null)
                        {
                            t.Word = wordEntry.Value;
                        }
                    }
                }
            }


        }

        #endregion

        #region PageUser

        private async Task<int> PageUserCreateAndSaveAsync(IdiomaticaContext context,
            Page page, BookUser bookUser, Dictionary<string, Word> commonWordDict,
            Dictionary<string, WordUser> allWordUsersInLanguage)
        {

            BookUser bookUserDenulled = _nullHandler.ThrowIfNull<BookUser>(bookUser);
            var languageUserDenulled = _nullHandler.ThrowIfNull<LanguageUser>(bookUserDenulled.LanguageUser);
            int languageUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(languageUserDenulled.Id);
            int languageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(languageUserDenulled.LanguageId);
            int pageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(page.Id);

            try
            {
                var language = _nullHandler.ThrowIfNull<Language>(
                    await DataCache.LanguageByIdReadAsync(languageIdDenulled, context));
                

                // has the page ever been parsed?
                var paragraphs = await DataCache.ParagraphsByPageIdReadAsync(
                    pageIdDenulled, context);
                if (paragraphs == null)
                {
                    await ParagraphsParseFromPageAndSaveAsync(
                        context, page, language, commonWordDict);
                }
                var pageUser = new PageUser()
                {
                    BookUserId = bookUser.Id,
                    PageId = pageIdDenulled
                };
                bool didSave = await DataCache.PageUserCreateAsync(pageUser, context);
                if(! didSave || pageUser.Id == null || pageUser.Id == 0)
                {
                    _errorHandler.LogAndThrow(2310);
                    return 0;
                }

                // now we need to add wordusers as needed
                foreach (var kvp in commonWordDict)
                {
                    if (allWordUsersInLanguage.ContainsKey(kvp.Key)) continue;

                    // need to create
                    var newWordUser = await WordUserCreateAndSaveUnknownAsync(
                        context, languageUserIdDenulled, kvp.Value);
                    if (newWordUser == null || newWordUser.Id == 0)
                    {
                        _errorHandler.LogAndThrow(2330);
                        return 0;
                    }
                    // and add to the dict
                    allWordUsersInLanguage[kvp.Key] = newWordUser;
                }
                return (int)pageUser.Id;
            }
            catch (IdiomaticaException)
            {
                throw;
            }
            catch (Exception ex)
            {
                
                string[] args = [
                    $"page.Id = {page.Id}",
                    $"bookUser.Id = {bookUser.Id}",
                    ];
                _errorHandler.LogAndThrow(3000, args, ex);
                throw; // you'll never get here
            }
        }
        private async Task<PageUser?> PageUserGetByOrderWithinBookAsync(
            IdiomaticaContext context, int languageUserId, int pageOrdinal, int bookId)
        {
            var pageUserFromDb = await DataCache.PageUserByLanguageUserIdOrdinalAndBookIdReadAsync(
                (languageUserId, pageOrdinal, bookId), context);

            if (pageUserFromDb is not null) return pageUserFromDb;

            User loggedInUserDenulled = _nullHandler.ThrowIfNull<User>(_loggedInUser);
            int loggedInUserIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(loggedInUserDenulled.Id);

            // pageUser is not created yet. Is the page created?
            var existingPage = _nullHandler.ThrowIfNull<Page>(
                await DataCache.PageByOrdinalAndBookIdReadAsync((pageOrdinal, bookId), context));
            int existingPageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(existingPage.Id);
            
            if (_allWordsInPage is null)
            {
                // it shouldn't be null, but whenever you need to create the page
                // user, these wouldn't be loaded yet
                _allWordsInPage = await WordsGetInPageAsync(context, existingPageIdDenulled);
            }
            if (_allWordUsersInPage is null)
            {
                // it shouldn't be null, but whenever you need to create the page
                // user, these wouldn't be loaded yet
                _allWordUsersInPage = await WordUsersGetAllInPageAsync(context,
                    existingPageIdDenulled, loggedInUserIdDenulled);
            }
            
            int newPageUserId = await PageUserCreateAndSaveAsync(context, existingPage,
                _nullHandler.ThrowIfNull<BookUser>(_bookUser),
                _nullHandler.ThrowIfNullOrEmptyDict(_allWordsInPage),
                _nullHandler.ThrowIfNullDict(_allWordUsersInPage));

            return await DataCache.PageUserByIdReadAsync(newPageUserId, context);
        }
        private async Task<PageUser?> PageUserGetOnReadOpenAsync(IdiomaticaContext context, int currentPageID, int languageUserId)
        {
            if (_currentPageUser == null)
            {
                if (_isLoadingPageUser == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return PageUserGetOnReadOpenAsync(context, currentPageID, languageUserId).Result;
                }
                _isLoadingPageUser = true;

                var dbPageUser = await DataCache.PageUserByPageIdAndLanguageUserIdReadAsync(
                    (currentPageID, languageUserId), context);
                if (dbPageUser is not null)
                {
                    _currentPageUser = dbPageUser;
                }
                else _currentPageUser = await PageUserGetByOrderWithinBookAsync(
                    context, languageUserId, 1, _nullHandler.ThrowIfNullOrZeroInt(_bookId));

                _isLoadingPageUser = false;
            }
            return _currentPageUser;
        }
        private async Task PageUserMarkAsReadAsync(IdiomaticaContext context, int pageUserId)
        {
            var readDate = DateTime.Now;
            await PageUserUpdateReadDateAsync(context, pageUserId, readDate);
        }
        private async Task PageUserUpdateReadDateAsync(IdiomaticaContext context, int id, DateTime readDate)
        {
            var pu = await DataCache.PageUserByIdReadAsync(id, context);
            
            pu.ReadDate = readDate;
            await DataCache.PageUserUpdateAsync(pu, context);
            return;
        }

        #endregion

        #region Paragraph

        /// <summary>
        /// parses the text through the language parser and returns paragraphs, 
        /// sentences, and tokens. it saves everything in the DB
        /// </summary>
        private async Task<bool> ParagraphsParseFromPageAndSaveAsync(
            IdiomaticaContext context, Page page, Language language, Dictionary<string, Word> commonWordDict)
        {
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var paragraphSplits = parser.SegmentTextByParagraphs(
                _nullHandler.ThrowIfNullString(page.OriginalText));

            int paragraphOrder = 0;

            foreach (var pText in paragraphSplits)
            {
                if (pText.Trim() == string.Empty) continue;
                Paragraph paragraph = new Paragraph()
                {
                    Ordinal = paragraphOrder,
                    PageId = _nullHandler.ThrowIfNullOrZeroInt(page.Id)
                };
                bool isSaved = await DataCache.ParagraphCreateAsync(paragraph, context);
                if (!isSaved || paragraph.Id == null || paragraph.Id == 0)
                {
                    _errorHandler.LogAndThrow(2270);
                    return false;
                }
                paragraphOrder++;

                var sentenceSplits = parser.SegmentTextBySentences(pText);
                for (int i = 0; i < sentenceSplits.Length; i++)
                {
                    var sentenceSplit = sentenceSplits[i];

                    var newSentence = new Sentence()
                    {
                        ParagraphId = (int)paragraph.Id,
                        Text = sentenceSplit,
                        Ordinal = i,
                    };
                    var isSavedSentence = await DataCache.SentenceCreateAsync(newSentence, context);
                    if (!isSavedSentence || newSentence.Id == null || newSentence.Id == 0)
                    {
                        _errorHandler.LogAndThrow(2280);
                        return false;
                    }

                    var areSavedTokens = await CreateTokensFromSentenceAndSaveAsync(
                        context, newSentence, language, commonWordDict);
                    if (!areSavedTokens)
                    {
                        _errorHandler.LogAndThrow(2300);
                        return false;
                    }
                }
            }
            return true;
        }
        private async Task<List<Paragraph>?> ParagraphsGetByPageIdAsync(IdiomaticaContext context, int pageId)
        {
            if (_paragraphs == null)
            {
                if (_isLoadingParagraphs == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return ParagraphsGetByPageIdAsync(context, pageId).Result;
                }
                // are they already loaded and attached to the page user?
                if (_currentPage != null && _currentPage.Paragraphs != null && _currentPage.Paragraphs.Count > 0)
                {
                    _paragraphs = _currentPage.Paragraphs;
                }
                else
                {
                    try
                    {
                        _isLoadingParagraphs = true;
                        _paragraphs = await DataCache.ParagraphsByPageIdReadAsync(pageId, context);
                    }
                    catch
                    {
                        throw;
                    }
                    _isLoadingParagraphs = false;
                }
            }
            return _paragraphs;
        }

        #endregion

        #region ParagraphTranslation

        #endregion

        #region Sentence

        /// <summary>
        /// this will delete any existing DB tokens
        /// </summary>
        private async Task<bool> CreateTokensFromSentenceAndSaveAsync(IdiomaticaContext context,
            Sentence sentence, Language language, Dictionary<string, Word> commonWordDict)
        {
            int sentenceIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(sentence.Id);
            string sentenceTextDenulled = _nullHandler.ThrowIfNullOrEmptyString(sentence.Text);
            int languageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(language.Id);


            // check if any already exist. there shouldn't be any but whateves
            await DataCache.TokenBySentenceIdDelete(sentenceIdDenulled, context);

            // parse new ones

            List<Token> tokens = new List<Token>();

            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var wordsSplits = parser.SegmentTextByWordsKeepingPunctuation(sentenceTextDenulled);

            for (int i = 0; i < wordsSplits.Length; i++)
            {
                var wordSplit = wordsSplits[i];
                var cleanWord = parser.StipNonWordCharacters(wordSplit).ToLower();
                Word? existingWord = null;
                // check if the word is in the word dict
                if (commonWordDict.ContainsKey(cleanWord))
                {
                    existingWord = commonWordDict[cleanWord];
                }
                else
                {
                    // check if the word is already in the DB
                    existingWord = await DataCache
                        .WordByLanguageIdAndTextLowerReadAsync((languageIdDenulled, cleanWord), context);
                }
                if (existingWord == null)
                {
                    if (cleanWord == string.Empty)
                    {
                        // there shouldn't be any empty words. that would mean
                        // that all non-word characters were stripped out and
                        // only left an empty string. Something like a string
                        // of punctuation or a quotation from another language
                        // that uses different characters. either way, create
                        // an empty word and move on
                        var emptyWord = await WordCreateAndSaveNewAsync(
                            context, language, string.Empty, string.Empty);
                        commonWordDict[cleanWord] = emptyWord;
                        existingWord = emptyWord;
                    }
                    else
                    {
                        // this is a newly encountered word. save it to the DB
                        // todo: add actual romanization lookup here
                        var newWord = await WordCreateAndSaveNewAsync(
                            context, language, cleanWord, cleanWord);
                        commonWordDict.Add(cleanWord, newWord);
                        existingWord = newWord;
                    }
                }
                Token token = new Token()
                {
                    Display = $"{wordSplit} ", // add the space that you previously took out
                    SentenceId = sentenceIdDenulled,
                    Ordinal = i,
                    WordId = _nullHandler.ThrowIfNullOrZeroInt(existingWord.Id)
                };
                bool isSaved = await DataCache.TokenCreateAsync(token, context);
                if (!isSaved || token.Id == null || token.Id == 0)
                {
                    _errorHandler.LogAndThrow(2290);
                    return false;
                }
            }
            return true;
        }
        private async Task<List<Sentence>?> SentencesByPageIdAsync(IdiomaticaContext context, int pageId)
        {
            if (_sentences == null)
            {
                if (_isLoadingSentences == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return SentencesByPageIdAsync(context, pageId).Result;
                }
                try
                {
                    _isLoadingSentences = true;
                    _sentences = await DataCache.SentencesByPageIdReadAsync(pageId, context);
                }
                catch
                {
                    throw;
                }
                _isLoadingSentences = false;
            }
            return _sentences;
        }

        #endregion

        #region Token

        private async Task<List<Token>?> TokensByPageIdAsync(IdiomaticaContext context, int pageId)
        {
            if (_tokens == null)
            {
                if (_isLoadingTokens == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return TokensByPageIdAsync(context, pageId).Result;
                }
                try
                {
                    _isLoadingTokens = true;
                    _tokens = await DataCache.TokensByPageIdReadAsync(pageId, context);
                }
                catch
                {
                    throw;
                }
                _isLoadingTokens = false;
            }
            return _tokens;
        }

        #endregion

        #region User

        private async Task<User?> UserGetLoggedInAsync(IdiomaticaContext context)
        {
            if (_loggedInUser == null)
            {
                if (_isLoadingLoggedInUser == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return UserGetLoggedInAsync(context).Result;
                }
                _isLoadingLoggedInUser = true;
                _loggedInUser = await _userService.GetLoggedInUserAsync(context);
                _isLoadingLoggedInUser = false;
            }
            return _loggedInUser;
        }

        #endregion

        #region Word

        private async Task<Word> WordCreateAndSaveNewAsync(IdiomaticaContext context,
            Language language, string text, string romanization)
        {
            int languageIdDenulled = _nullHandler.ThrowIfNullOrZeroInt(language.Id);
            Word newWord = new Word()
            {
                LanguageId = languageIdDenulled,
                Romanization = romanization,
                Text = text.ToLower(),
                TextLowerCase = text.ToLower(),
            };
            var isSaved = await DataCache.WordCreateAsync(newWord, context);
            if(!isSaved || newWord.Id == null || newWord.Id == 0)
            {
                _errorHandler.LogAndThrow(2350);
                return newWord;
            }
            return newWord;
        }
        private async Task<Dictionary<string, Word>?> WordFetchCommonDictForLanguageAsync(
            IdiomaticaContext context, int languageId)
        {
            
            
            var topWordsInLanguage = await DataCache.WordsCommon1000ByLanguageIdReadAsync(
                 _nullHandler.ThrowIfNullOrZeroInt(languageId),
                context);

            Dictionary<string, Word> wordDict = new Dictionary<string, Word>();
            foreach (var word in topWordsInLanguage)
            {

                // TextLowerCase and LanguageId are a unique key in the database
                // so no need to check if it already exists before adding
                wordDict.Add(_nullHandler.ThrowIfNullString(word.TextLowerCase), word);
            }
            return wordDict;
        }
        private async Task<Word?> WordGetByIdAsync(IdiomaticaContext context, int id)
        {
            return await DataCache.WordByIdReadAsync(id, context);
        }
        private async Task<Dictionary<string, Word>?> WordsGetInPageAsync(IdiomaticaContext context, int pageId)
        {
            if (_allWordsInPage == null)
            {
                if (_isLoadingWordsInPage == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return WordsGetInPageAsync(context, pageId).Result;
                }
                try
                {
                    _isLoadingWordsInPage = true;
                    _allWordsInPage = await DataCache.WordsDictByPageIdReadAsync(pageId, context);
                }
                catch
                {
                    throw;
                }
                _isLoadingWordsInPage = false;
            }
            return _allWordsInPage;
        }
        


        #endregion

        #region WordUser

        private async Task<WordUser> WordUserCreateAndSaveUnknownAsync(
            IdiomaticaContext context, int languageUserId, Word word)
        {
            WordUser newWordUser = new WordUser()
            {
                LanguageUserId = _nullHandler.ThrowIfNullOrZeroInt(languageUserId),
                WordId = _nullHandler.ThrowIfNullOrZeroInt(word.Id),
                Status = AvailableWordUserStatus.UNKNOWN,
                Translation = string.Empty
            };
            var isSaved = await DataCache.WordUserCreateAsync(newWordUser, context);
            if (!isSaved || newWordUser.Id == 0)
            {
                _errorHandler.LogAndThrow(2360);
                return newWordUser;
            }
            return newWordUser;
        }
        private async Task<Dictionary<string, WordUser>> WordUserFetchDictForLanguageUserAsync(
            IdiomaticaContext context, int userId, int languageId)
        {
            
            Dictionary<string, WordUser> wordDict = new Dictionary<string, WordUser>();




            var allWordUsersInLanguage = await DataCache.WordUsersByUserIdAndLanguageIdReadAsync(
                (userId, languageId), context);

            foreach (var wordUser in allWordUsersInLanguage)
            {
                if (wordUser.Word is null)
                {
                    wordUser.Word = 
                        await DataCache.WordByIdReadAsync(
                            _nullHandler.ThrowIfNullOrZeroInt(wordUser.WordId), context)
                        ;
                }
                Word wordDenulled = _nullHandler.ThrowIfNull<Word>(wordUser.Word);
                string wordTextLcDenulled = _nullHandler.ThrowIfNullString(wordDenulled.TextLowerCase);
                // TextLowerCase and LanguageId are a unique key in the database
                // so no need to check if it already exists before adding
                wordDict.Add(wordTextLcDenulled, wordUser);
            }

            return wordDict;
        }
        private async Task<Dictionary<string, WordUser>?> WordUsersGetAllInPageAsync(
            IdiomaticaContext context, int pageId, int userId)
        {
            if (_allWordUsersInPage == null)
            {
                if (_isLoadingAllWordUsersInPage == true)
                {
                    // hold up. some other thread is loading it
                    Thread.Sleep(1000);
                    return WordUsersGetAllInPageAsync(context, pageId, userId).Result;
                }
                try
                {
                    _isLoadingAllWordUsersInPage = true;
                    _allWordUsersInPage = await DataCache.WordUsersDictByPageIdAndUserIdReadAsync((pageId, userId), context);
                }
                catch
                {
                    throw;
                }
                _isLoadingAllWordUsersInPage = false;
            }
            return _allWordUsersInPage;
        }

        #endregion
    }
}
