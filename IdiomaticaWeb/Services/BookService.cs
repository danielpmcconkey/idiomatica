using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using System.Linq.Expressions;

namespace IdiomaticaWeb.Services
{
    public class BookService
    {
        private IDbContextFactory<IdiomaticaContext> _dbContextFactory;
        public BookService(IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public List<BookUser> FetchBookUsersWithoutStats(int loggedInUserId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                Expression<Func<BookUser, bool>> filter = (x => x.LanguageUser.UserId == loggedInUserId);
                return context.BookUsers
                    .Where(filter)
                    .Include(bu => bu.LanguageUser).ThenInclude(lu => lu.Language)
                    .Include(bu => bu.Book).ThenInclude(b => b.BookStats)
                    .ToList();
            }
        }
        public List<BookUserStat> FetchBookUserStats(int loggedInUserId)
        {
            using (var context = _dbContextFactory.CreateDbContext())
            {
                Expression<Func<BookUserStat, bool>> filter = (x => x.LanguageUser.UserId == loggedInUserId);
                return context.BookUserStats
                    .Where(filter)
                    .ToList();
            }
        }
    }
}
