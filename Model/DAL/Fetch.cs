using Microsoft.EntityFrameworkCore;

namespace Model.DAL
{
    public static class Fetch
    {

        public static Language Language(IdiomaticaContext context, int id)
        {
            Func<Language, bool> filter = (x => x.Id == id);
            return Languages(context, filter).FirstOrDefault();
        }
        public static List<Language> Languages(IdiomaticaContext context, Func<Language, bool> filter)
        {
            return context.Languages
                .Include(l => l.Books).ThenInclude(b => b.Texts).ThenInclude(t => t.Sentences)
                .Where(filter)
                .ToList();
        }
        public static Book Book(IdiomaticaContext context, int id)
        {
            Func<Book, bool> filter = (x => x.Id == id);
            return Books(context, filter).FirstOrDefault();
        }
        public static List<Book> Books(IdiomaticaContext context, Func<Book, bool> filter)
        {

            try
            {
                return context.Books
                        .Include(b => b.BookStat)
                        .Include(b => b.BookTags).ThenInclude(t => t.Tag)
                        .Include(b => b.Texts).ThenInclude(t => t.Sentences)
                        .Include(b => b.Language)
                        .Where(filter)
                        .ToList();
            }
            catch (Exception)
            {
                //var burp = context.ContextId;
                throw;
            }
        }
        public static Text Text(IdiomaticaContext context, int id)
        {
            Func<Language, bool> filter = (x => x.Id == id);
            return Texts(context, id).FirstOrDefault();
        }
        public static List<Text> Texts(IdiomaticaContext context, int? id = null)
        {
            return context.Texts
                .Where(x => id == null || x.Id == id)
                .Include(t => t.Sentences)
                .Include(t => t.Book)
                .ToList();
        }
        public static Word Word(IdiomaticaContext context, int id)
        {
            Func<Word, bool> filter = (x => x.Id == id);
            return Words(context, filter).FirstOrDefault();
        }
        public static List<Word> Words(IdiomaticaContext context, Func<Word, bool> filter)
        {
            return context.Words
                .Include(w => w.ParentWords)
                .Include(w => w.ChildWords)
                .Include(w => w.Status)
                .Include(w => w.Language)
                .Where(filter)
                .ToList();
        }
    }
}
