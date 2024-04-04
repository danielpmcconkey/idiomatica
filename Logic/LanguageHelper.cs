using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;

namespace Logic
{
    public static class LanguageHelper
    {
        public static LanguageUser GetLanguageUser(IdiomaticaContext context, int id)
        {
            Func<LanguageUser, bool> filter = (x => x.Id == id);
            return GetLanguageUser(context, filter).FirstOrDefault();
        }
        public static List<LanguageUser> GetLanguageUser(IdiomaticaContext context, Func<LanguageUser, bool> filter)
        {
            return context.LanguageUsers.Include(l => l.Books).ThenInclude(b => b.BookStats)
                .Include(l => l.Language)
                .Include(l => l.User).ThenInclude(u => u.UserSettings)
                .Where(filter)
                .ToList();
        }
    }
}
