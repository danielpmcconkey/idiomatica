using Microsoft.EntityFrameworkCore;

namespace Model.DAL
{
    public static class Fetch
    {

        public static LanguageUser LanguageUser(IdiomaticaContext context, int id)
        {
            Func<LanguageUser, bool> filter = (x => x.Id == id);
            return LanguageUsers(context, filter).FirstOrDefault();
        }
        public static List<LanguageUser> LanguageUsers(IdiomaticaContext context, Func<LanguageUser, bool> filter)
        {
            return context.LanguageUsers
                .Include(l => l.Books).ThenInclude(b => b.Pages).ThenInclude(p => p.Paragraphs).ThenInclude(s => s.Sentences)
				.Include(l => l.Books).ThenInclude(b => b.BookStats)
                .Include(l => l.Language)
                .Include(l => l.User).ThenInclude(u => u.UserSettings)
                .Where(filter)
                .ToList();
        }
        public static Book? BookById(IdiomaticaContext context, int id)
        {
            Func<Book, bool> filter = (x => x.Id == id);
            return context.Books
                        .Include(b => b.BookStats)
                        .Include(b => b.Pages).ThenInclude(p => p.Paragraphs)
                            .ThenInclude(t => t.Sentences).ThenInclude(s => s.Tokens)
                            .ThenInclude(t => t.Word).ThenInclude(w => w.Status)
                        .Include(b => b.LanguageUser).ThenInclude(lu => lu.Language)
                        .Where(filter)
                        .FirstOrDefault();
        }
        /// <summary>
        /// doesn't include the pages, paragraphs, etc
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<Book> Books(IdiomaticaContext context, Func<Book, bool> filter)
        {
            try
            {
                return context.Books
                        .Include(b => b.BookStats).Include(b => b.LanguageUser).ThenInclude(lu => lu.Language)
                        .Where(filter)
                        .ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static Page Page(IdiomaticaContext context, int id)
        {
            Func<LanguageUser, bool> filter = (x => x.Id == id);
            return Pages(context, id).FirstOrDefault();
        }
        public static List<Page> Pages(IdiomaticaContext context, int? id = null)
        {
            return context.Pages
                .Where(x => id == null || x.Id == id)
                .Include(p => p.Paragraphs).ThenInclude(s => s.Sentences)
                .Include(t => t.Book).ThenInclude(b => b.BookStats)
                .ToList();
        }
        public static List<Status> Statuses(IdiomaticaContext context)
        {
            return context.Statuses.ToList();
        }
        public static Word Word(IdiomaticaContext context, int id)
        {
            Func<Word, bool> filter = (x => x.Id == id);
            return Words(context, filter).FirstOrDefault();
        }
        public static List<Word> WordsByLanguageUser(IdiomaticaContext context, LanguageUser languageUser)
        {
            Func<Word, bool> filter = (x => x.LanguageUserId == languageUser.Id);
            return Words(context, filter);            
        }
        public static List<Word> Words(IdiomaticaContext context, Func<Word, bool> filter)
        {
            return context.Words
                .Include(w => w.ParentWords)
                .Include(w => w.ChildWords)
                .Include(w => w.Status)
                .Include(w => w.LanguageUser)
                .Where(filter)
                .ToList();
        }
    }
}
