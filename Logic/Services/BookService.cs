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

        #region Book

        public async Task<int> BookCreateAndSaveAsync(string title, string languageCode, string url, string text)
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
                return -1;
            }
            if (string.IsNullOrEmpty(languageCodeT))
            {
                ErrorHandler.LogAndThrow(1050);
                return -1;
            }
            if (string.IsNullOrEmpty(textT))
            {
                ErrorHandler.LogAndThrow(1060);
                return -1;
            }


            var context = await _dbContextFactory.CreateDbContextAsync();

            // pull language from the db
            var language = await DataCache.LanguageByCodeReadAsync(languageCodeT, context);
            if (language == null || language.Id == null || language.Id == 0)
            {
                ErrorHandler.LogAndThrow(2070);
                return -1;
            }

            // divide text into paragraphs
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var paragraphSplits = parser.SegmentTextByParagraphs(textT);
            if (paragraphSplits is null || paragraphSplits.Length == 0)
            {
                ErrorHandler.LogAndThrow(2080);
                return -1;
            }

            // add the book to the DB so you can save pages using its ID
            Book book = new Book()
            {
                Title = titleT,
                SourceURI = urlT,
                LanguageId = (int)language.Id,
            };
            bool didSaveBook = await DataCache.BookCreateAsync(book, context);
            if(!didSaveBook || book.Id == null || book.Id < 1)
            {
                ErrorHandler.LogAndThrow(2090);
                return -1;
            }

            // pull a list of common words from the database and put it
            // in a dictionary so that we don't always have to go back to the
            // database just to get the word ID for every single word
            var commonWordsInLanguage = await WordFetchCommonDictForLanguageAsync((int)language.Id);
            if (commonWordsInLanguage == null)
            {
                ErrorHandler.LogAndThrow(2200);
                return -1;
            }
            

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
                        int newPageId = await PageCreateAndSaveDuringBookCreateAsync((int)book.Id, currentPageCount,
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
                int newPageId = await PageCreateAndSaveDuringBookCreateAsync((int)book.Id, currentPageCount,
                            pageText, language, commonWordsInLanguage);
            }

            return (int)book.Id;
        }
        
        #endregion

        #region BookListRow
        
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
        
        
        #endregion

        #region bookuser
        public async Task<int> BookUserCreateAndSaveAsync(int bookId, int userId)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
            var book = await DataCache.BookByIdReadAsync(bookId, context);
            if(book == null)
            {
                ErrorHandler.LogAndThrow(2000);
                return 0;
            }
            book.Pages = await DataCache.PagesByBookIdReadAsync(bookId, context);
            
            var firstPage = book.Pages.Where(x => x.Ordinal == 1).FirstOrDefault();
            if(firstPage == null || firstPage.Id == 0)
            {
                ErrorHandler.LogAndThrow(2010);
                return 0;
            }
            var languageUser = await DataCache.LanguageUserByLanguageIdAndUserIdReadAsync(
                (book.LanguageId, userId), context);

                
            if (languageUser == null || languageUser.Id == null || languageUser.Id == 0)
            {
                ErrorHandler.LogAndThrow(2020);
            }
            BookUser bookUser = new BookUser() { 
                BookId = bookId, 
                CurrentPageID = (int)firstPage.Id, 
                LanguageUserId = (int)languageUser.Id
            };
            bool didSaveBookUser = await DataCache.BookUserCreateAsync(bookUser, context);
            if (!didSaveBookUser || bookUser.Id < 1)
            {
                ErrorHandler.LogAndThrow(2250);
                return -1;
            }
            if (bookUser.Id == 0)
            {
                ErrorHandler.LogAndThrow(2030);
                return -1;
            }

            // now update BookUserStats
            await BookUserStatsUpdate(bookUser.Id);

            return bookUser.Id;
        }
        public async Task BookUserUpdateBookmarkAsync(int bookUserId, int currentPageId)
        {
            if (bookUserId == 0)
            {
                ErrorHandler.LogAndThrow(1120);
            }
            if (currentPageId == 0)
            {
                ErrorHandler.LogAndThrow(1130);
            }
            var context = await _dbContextFactory.CreateDbContextAsync();

            var bookUser = await DataCache.BookUserByIdReadAsync(bookUserId, context);
            
            if (bookUser == null)
            {
                ErrorHandler.LogAndThrow(2050, [$"bookUserId: {bookUserId}"]);
                return;
            }
            bookUser.CurrentPageID = currentPageId;
            await DataCache.BookUserUpdateAsync(bookUser, context);
        }


        #endregion

        #region BookUserStat

        private async Task BookUserStatsUpdate(int bookUserId)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
            var bookUser = await DataCache.BookUserByIdReadAsync(bookUserId, context);
            var languageUser = await DataCache.LanguageUserByIdReadAsync(bookUser.LanguageUserId, context);
            var allWordsInBook = await DataCache.WordsByBookIdReadAsync(bookUser.BookId, context);
            var allWordUsersInBook = await DataCache.WordUsersByBookIdAndLanguageUserIdReadAsync(
                (bookUser.BookId, bookUser.LanguageUserId), context);
            var pageUsers = await DataCache.PageUsersByBookUserIdReadAsync(bookUserId, context);
            var pages = await DataCache.PagesByBookIdReadAsync (bookUserId, context);
            var bookStats = await DataCache.BookStatsByBookIdReadAsync(bookUser.BookId, context);
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
            int lastPageRead = (readPages.Count > 0) ? readPages.First().Ordinal : 0;

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
            await DataCache.BookListRowsByUserIdDeleteAsync((int)languageUser.Id, context);
            // delete the actual stats from teh database and bookuserstats cache
            await DataCache.BookUserStatsByBookIdAndUserIdDelete((bookUser.BookId, languageUser.UserId), context);
            // add the new stats
            await DataCache.BookUserStatsByBookIdAndUserIdCreate(
                (bookUser.BookId, languageUser.UserId), stats, context);
        }

        #endregion

        #region LanguageCode
        public IQueryable<LanguageCode> LanguageCodeFetchOptionsDuringBookCreate()
        {
            // this isn't worth caching
            var context = _dbContextFactory.CreateDbContext();

            Expression<Func<LanguageCode, bool>> filter = (x => x.IsImplementedForLearning == true);
            return context.LanguageCodes
                .Where(filter).OrderBy(x => x.LanguageName);
        }
        #endregion

        #region LanguageUser
        
        
        #endregion

        #region Page
        


        public async Task<int> PageCreateAndSaveDuringBookCreateAsync(
            int bookId, int Ordinal, string pageText,
            Language language, Dictionary<string,Word> commonWordsInLanguage)
        {
            var newPage = new Page()
            {
                BookId = bookId,
                Ordinal = Ordinal,
                OriginalText = pageText
            };


            var context = await _dbContextFactory.CreateDbContextAsync();
            bool isSaved = await DataCache.PageCreateNewAsync(newPage, context);
            if(!isSaved || newPage.Id == null || newPage.Id == 0)
            {
                ErrorHandler.LogAndThrow(2040);
                return -1;
            }

            
            // do the actual page parsing
            bool areParagraphsParsed = await ParagraphsParseFromPageAndSaveAsync(
                newPage, language, commonWordsInLanguage);
            if(!areParagraphsParsed)
            {
                ErrorHandler.LogAndThrow(2260);
                return -1;
            }
            return (int)newPage.Id;
        }

        
        #endregion

        #region PageUser
        public async void PageUserClearPage(PageUser pageUser, LanguageUser languageUser)
        {
            if (pageUser == null) 
            {
                ErrorHandler.LogAndThrow(1140); 
            }
            var context = _dbContextFactory.CreateDbContext();
            await DataCache.WordUsersUpdateStatusByPageIdAndUserIdAndStatus(pageUser.PageId, languageUser.UserId,
                AvailableWordUserStatus.UNKNOWN, AvailableWordUserStatus.WELLKNOWN, context);
        }
        public async Task<int> PageUserCreateAndSaveAsync(
            Page page, BookUser bookUser, Dictionary<string, Word> commonWordDict,
            Dictionary<string, WordUser> allWordUsersInLanguage)
        {
            if (page == null)
            {
                ErrorHandler.LogAndThrow(1310);
                return 0;
            }
            if (page.Id == 0)
            {
                ErrorHandler.LogAndThrow(1320);
                return 0;
            }
            if (bookUser == null)
            {
                ErrorHandler.LogAndThrow(1330);
                return 0;
            }
            if (commonWordDict == null)
            {
                ErrorHandler.LogAndThrow(1360);
                return 0;
            }

            try
            {
                var context = await _dbContextFactory.CreateDbContextAsync();
                var language = await DataCache.LanguageByIdReadAsync(bookUser.LanguageUserId, context);
                if (language == null)
                {
                    ErrorHandler.LogAndThrow(2320);
                    return 0;
                }

                // has the page ever been parsed?
                var paragraphs = await DataCache.ParagraphsByPageIdReadAsync((int)page.Id, context);
                if (paragraphs == null)
                {
                    await ParagraphsParseFromPageAndSaveAsync(
                        page, language, commonWordDict);
                }
                var pageUser = new PageUser()
                {
                    BookUserId = bookUser.Id,
                    PageId = (int)page.Id
                };
                bool didSave = await DataCache.PageUserCreateAsync(pageUser, context);
                if(! didSave || pageUser.Id == null || pageUser.Id == 0)
                {
                    ErrorHandler.LogAndThrow(2310);
                    return 0;
                }

                // now we need to add wordusers as needed
                foreach (var kvp in commonWordDict)
                {
                    if (allWordUsersInLanguage.ContainsKey(kvp.Key)) continue;

                    // need to create
                    var newWordUser = await WordUserCreateAndSaveUnknownAsync(
                        bookUser.LanguageUserId, kvp.Value);
                    if (newWordUser == null || newWordUser.Id == 0)
                    {
                        ErrorHandler.LogAndThrow(2330);
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
                ErrorHandler.LogAndThrow(3000, args, ex);
                throw; // you'll never get here
            }
        }
        public async Task PageUserUpdateReadDateAsync(int id, DateTime readDate)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
            var pu = await DataCache.PageUserByIdReadAsync(id, context);
            if (pu == null) 
            {
                ErrorHandler.LogAndThrow(2100);
                return;
            }
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
        public async Task<bool> ParagraphsParseFromPageAndSaveAsync(
            Page page, Language language, Dictionary<string, Word> commonWordDict)
        {

            if (page == null)
            {
                ErrorHandler.LogAndThrow(1250);
                return false;
            }
            if (page.Id == 0)
            {
                ErrorHandler.LogAndThrow(1260);
                return false;
            }

            var context = await _dbContextFactory.CreateDbContextAsync();

            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var paragraphSplits = parser.SegmentTextByParagraphs(page.OriginalText);

            int paragraphOrder = 0;

            foreach (var pText in paragraphSplits)
            {
                if (pText.Trim() == string.Empty) continue;
                Paragraph paragraph = new Paragraph()
                {
                    Ordinal = paragraphOrder,
                    PageId = (int)page.Id
                };
                bool isSaved = await DataCache.ParagraphCreateAsync(paragraph, context);
                if (!isSaved || paragraph.Id == null || paragraph.Id == 0)
                {
                    ErrorHandler.LogAndThrow(2270);
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
                        ErrorHandler.LogAndThrow(2280);
                        return false;
                    }

                    var areSavedTokens = await CreateTokensFromSentenceAndSaveAsync(
                        newSentence, language, commonWordDict);
                    if (!areSavedTokens)
                    {
                        ErrorHandler.LogAndThrow(2300);
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #region ParagraphTranslation
        
        #endregion

        #region Sentence
        /// <summary>
        /// this will delete any existing DB tokens
        /// </summary>
        public async Task<bool> CreateTokensFromSentenceAndSaveAsync(
            Sentence sentence, Language language, Dictionary<string, Word> commonWordDict)
        {
            if (sentence == null)
            {
                ErrorHandler.LogAndThrow(1270);
                return false;
            }
            if (sentence.Id == null || sentence.Id < 1)
            {
                ErrorHandler.LogAndThrow(1280);
                return false;
            }
            if (sentence.Text == null)
            {
                ErrorHandler.LogAndThrow(1290);
                return false;
            }
            if (commonWordDict == null)
            {
                ErrorHandler.LogAndThrow(1300);
                return false;
            }

            var context = await _dbContextFactory.CreateDbContextAsync();

            // check if any already exist. there shouldn't be any but whateves
            await DataCache.TokenBySentenceIdDelete((int)sentence.Id, context);

            // parse new ones

            List<Token> tokens = new List<Token>();

            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var wordsSplits = parser.SegmentTextByWordsKeepingPunctuation(sentence.Text);

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
                        .WordByLanguageIdAndTextLowerReadAsync(((int)language.Id, cleanWord), context);
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
                        var emptyWord = await WordCreateAndSaveNewAsync(language, string.Empty, string.Empty);
                        commonWordDict[cleanWord] = emptyWord;
                        existingWord = emptyWord;
                    }
                    else
                    {
                        // this is a newly encountered word. save it to the DB
                        // todo: add actual romanization lookup here
                        var newWord = await WordCreateAndSaveNewAsync(language, cleanWord, cleanWord);
                        commonWordDict.Add(cleanWord, newWord);
                        existingWord = newWord;
                    }
                }
                Token token = new Token()
                {
                    Display = $"{wordSplit} ", // add the space that you previously took out
                    SentenceId = (int)sentence.Id,
                    Ordinal = i,
                    WordId = (int)existingWord.Id
                };
                bool isSaved = await DataCache.TokenCreateAsync(token, context);
                if (!isSaved || token.Id == null || token.Id == 0)
                {
                    ErrorHandler.LogAndThrow(2290);
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Word

        public async Task<Word> WordCreateAndSaveNewAsync(
            Language language, string text, string romanization)
        {
            if (language == null)
            {
                ErrorHandler.LogAndThrow(1340);
                return null;
            }
            if (language.Id == 0)
            {
                ErrorHandler.LogAndThrow(1350);
                return null;
            }
            var context = await _dbContextFactory.CreateDbContextAsync();
            Word newWord = new Word()
            {
                LanguageId = (int)language.Id,
                Romanization = romanization,
                Text = text.ToLower(),
                TextLowerCase = text.ToLower(),
            };
            var isSaved = await DataCache.WordCreateAsync(newWord, context);
            if(!isSaved || newWord.Id == null || newWord.Id == 0)
            {
                ErrorHandler.LogAndThrow(2350);
                return null;
            }
            return newWord;
        }
        
        public async Task<Dictionary<string, Word>?> WordFetchCommonDictForLanguageAsync(int languageId)
        {
            if (languageId == 0)
            {
                ErrorHandler.LogAndThrow(1160);
                return null;
            }
            var context = await _dbContextFactory.CreateDbContextAsync();

            var topWordsInLanguage = await DataCache.WordsCommon1000ByLanguageIdReadAsync(languageId, context);

            Dictionary<string, Word> wordDict = new Dictionary<string, Word>();
            foreach (var word in topWordsInLanguage)
            {

                // TextLowerCase and LanguageId are a unique key in the database
                // so no need to check if it already exists before adding
                wordDict.Add(word.TextLowerCase, word);
            }
            return wordDict;
        }
        
        #endregion

        #region WordUser
        
        public async Task<WordUser> WordUserCreateAndSaveUnknownAsync(int languageUserId, Word word)
        {
            if (languageUserId == 0)
            {
                ErrorHandler.LogAndThrow(1370);
                return null;
            }
            if (word == null)
            {
                ErrorHandler.LogAndThrow(1380);
                return null;
            }

            var context = await _dbContextFactory.CreateDbContextAsync();

            WordUser newWordUser = new WordUser()
            {
                LanguageUserId = (int)languageUserId,
                WordId = (int)word.Id,
                Status = AvailableWordUserStatus.UNKNOWN,
                Translation = string.Empty
            };
            var isSaved = await DataCache.WordUserCreateAsync(newWordUser, context);
            if (!isSaved || newWordUser.Id == 0)
            {
                ErrorHandler.LogAndThrow(2360);
                return null;
            }
            return newWordUser;
        }
        
        
        public async Task<Dictionary<string, WordUser>> WordUserFetchDictForLanguageUserAsync(
            int userId, int languageId)
        {
            var context = await _dbContextFactory.CreateDbContextAsync();

            Dictionary<string, WordUser> wordDict = new Dictionary<string, WordUser>();




            var allWordUsersInLanguage = await DataCache.WordUsersByUserIdAndLanguageIdReadAsync(
                (userId, languageId), context);

            foreach (var wordUser in allWordUsersInLanguage)
            {
                if (wordUser.Word is null)
                {
                    wordUser.Word = await DataCache.WordByIdReadAsync(wordUser.WordId, context);
                }

                // TextLowerCase and LanguageId are a unique key in the database
                // so no need to check if it already exists before adding
                wordDict.Add(wordUser.Word.TextLowerCase, wordUser);
            }

            return wordDict;
        }
        /// <summary>
        /// Updates the WordUser but doesn't update all the caches. reset any 
        /// caches that are important to you
        /// </summary>
        /// <returns>true if the word user is changed and has been updated. false if not</returns>
        public async Task WordUserUpdateStatusAndTranslationAsync(
            int id, AvailableWordUserStatus newStatus, string translation)
        {
            if (id == 0)
            {
                ErrorHandler.LogAndThrow(1150);
                return;
            }
            // first pull the existing one from the database
            var context = await _dbContextFactory.CreateDbContextAsync();
            var dbWordUser = await DataCache.WordUserByIdReadAsync(id, context);
            if (dbWordUser == null)
            {
                ErrorHandler.LogAndThrow(2060);
                return;
            }
            
            dbWordUser.Status = newStatus;
            dbWordUser.Translation = translation;
            dbWordUser.StatusChanged = DateTime.Now;
            await DataCache.WordUserUpdateAsync(dbWordUser, context);
        }
        #endregion
    }
}
