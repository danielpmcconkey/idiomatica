using Logic;
using Logic.UILabels;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using System.Linq.Expressions;
using System.Net;

namespace IdiomaticaWeb.Services
{
    public class BookService
    {
        // todo: move the logged in user ID to be a member of BookService class

        private IDbContextFactory<IdiomaticaContext> _dbContextFactory;
        public BookService(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public PageUser CreatePageUserAndSave(
            Page page, BookUser bookUser, Dictionary<string, Word> commonWordDict,
            Dictionary<string, WordUser> allWordUsersInLanguage)
        {
            var context = _dbContextFactory.CreateDbContext();
            return PageHelper.CreatePageUserAndSave(
                context, page, bookUser, commonWordDict, allWordUsersInLanguage);
        }
        public IQueryable<BookUser> FetchBookUsersWithoutStats(int loggedInUserId)
        {
            var context = _dbContextFactory.CreateDbContext();
            
            Expression<Func<BookUser, bool>> filter = (x => x.LanguageUser.UserId == loggedInUserId);
            return context.BookUsers
                .Where(filter)
                .Include(bu => bu.LanguageUser).ThenInclude(lu => lu.Language)
                .Include(bu => bu.Book).ThenInclude(b => b.BookStats);
        }
        public BookUser? FetchBookUser(int loggedInUserId, int bookId) 
        {
            var context = _dbContextFactory.CreateDbContext();
            return context.BookUsers
                .Where(bu => bu.LanguageUser.UserId == loggedInUserId && bu.BookId == bookId)
                .Include(bu => bu.LanguageUser).ThenInclude(lu => lu.Language)
                .Include(bu => bu.Book).ThenInclude(b => b.BookStats)
                .FirstOrDefault()
                ;
        }
        public IQueryable<BookUserStat> FetchBookUserStats(int loggedInUserId)
        {
            var context = _dbContextFactory.CreateDbContext();

            Expression<Func<BookUserStat, bool>> filter = (x => x.LanguageUser.UserId == loggedInUserId);
            return context.BookUserStats
                .Where(filter);

        }
        public IQueryable<BookUserStat> FetchBookUserStats(int loggedInUserId, int bookId)
        {
            var context = _dbContextFactory.CreateDbContext();

            return context.BookUserStats
                .Where(x => x.LanguageUser.UserId == loggedInUserId && x.BookId == bookId);

        }
        public PageUser? FetchPageUserById(int pageUserId, int loggedInUserId)
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
        public Page? FetchPageByOrdinalWithinBook(int ordinal, int bookId)
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
        public PageUser? FetchPageUserByOrdinalWithinBook(int ordinal, int bookId, int loggedInUserId)
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
        public Dictionary<string, WordUser> FetchWordUserDictForLanguageUser(LanguageUser languageUser)
        {
            var context = _dbContextFactory.CreateDbContext();
            return WordHelper.FetchWordUserDictForLanguageUser(context, languageUser);
        }
        public Dictionary<string, Word> FetchCommonWordDictForLanguage(Language language)
        {
            var context = _dbContextFactory.CreateDbContext();
            return WordHelper.FetchCommonWordDictForLanguage(context, language);
        }
    }
}
