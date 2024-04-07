using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static class Fetch
    {
        #region Books
        public static List<Book> BooksAndBookStatsAndLanguage(IdiomaticaContext context, 
            Expression<Func<Book, bool>> filter)
        {
            return context.Books
                .Where(filter)
                .Include(b => b.BookStats)
                .Include(l => l.LanguageUser).ThenInclude(lu => lu.Language)
                .ToList();
        }
        public static Book? BookAndBookStatsAndLanguage(IdiomaticaContext context, Expression<Func<Book, bool>> filter)
        {
            return context.Books
                .Where(filter)
                .Include(b => b.BookStats)
                .Include(l => l.LanguageUser).ThenInclude(lu => lu.Language)
                .FirstOrDefault();
        }
        #endregion

        #region Bookstats
        public static List<BookStat> BookStats(IdiomaticaContext context, 
            Expression<Func<BookStat, bool>> filter)
        {
            return context.BookStats
                .Where(filter)
                .ToList();
        }
        #endregion

        #region LanguageUser

        public static LanguageUser LanguageUserAndBooksAndBookStatsAndLanguageAndUserAndUserStats(
            IdiomaticaContext context, Expression<Func<LanguageUser, bool>> filter)
        {
            return context.LanguageUsers
                .Where(filter)
                .Include(lu => lu.Books).ThenInclude(b => b.BookStats)
                .Include(lu => lu.Language)
                .Include(lu => lu.User).ThenInclude(u => u.UserSettings)
                .FirstOrDefault();
        }
        public static List<LanguageUser> LanguageUsersAndBooksAndBookStatsAndLanguageAndUserAndUserStats(
            IdiomaticaContext context, Expression<Func<LanguageUser, bool>> filter)
        {
            return context.LanguageUsers
                .Where(filter)
                .Include(lu => lu.Books).ThenInclude(b => b.BookStats)
                .Include(lu => lu.Language)
                .Include(lu => lu.User).ThenInclude(u => u.UserSettings)
                .ToList();
        }

        #endregion

        #region Pages
        public static List<Page> Pages(IdiomaticaContext context, Expression<Func<Page, bool>> filter)
        {
            return context.Pages
                .Where(filter)
                .ToList();
        }
        public static Page? PageAndParagraphsAndSentencesAndTokensAndWords(
            IdiomaticaContext context, Expression<Func<Page, bool>> filter)
        {
            return context.Pages
                .Where(filter)
                .Include(p => p.Paragraphs)
                    .ThenInclude(s => s.Sentences)
                    .ThenInclude(s => s.Tokens)
                    .ThenInclude(t => t.Word)
                .FirstOrDefault();
        }
        #endregion

        #region Paragraphs
        public static List<Paragraph> Paragraphs(IdiomaticaContext context, Expression<Func<Paragraph, bool>> filter)
        {
            return context.Paragraphs
                .Where(filter)
                .ToList();
        }
        public static List<Paragraph> ParagraphsAndSentencesAndTokensAndWords(
            IdiomaticaContext context, Expression<Func<Paragraph, bool>> filter)
        {
            return context.Paragraphs
                .Where(filter)
                .Include(s => s.Sentences)
                    .ThenInclude(s => s.Tokens)
                        .ThenInclude(t => t.Word)
                .ToList();
        }
        #endregion

        #region Words
        public static List<Word> WordsAndChildrenAndParents(
            IdiomaticaContext context, Expression<Func<Word, bool>> filter)
        {
            return context.Words
                .Where(filter)
                .Include(w => w.ParentWords)
                .Include(w => w.ChildWords)
                .ToList();
        }

        #endregion
    }
}
