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
        public static List<Book> BooksAndBookStatsAndLanguage(Expression<Func<Book, bool>> filter)
        {
            using (var context = new IdiomaticaContext())
            {
                return context.Books
                .Where(filter)
                .Include(b => b.BookStats)
                .Include(l => l.LanguageUser).ThenInclude(lu => lu.Language)
                .ToList();
            }
        }
        public static Book? BookAndBookStatsAndLanguage(Expression<Func<Book, bool>> filter)
        {
            using (var context = new IdiomaticaContext())
            {
                return context.Books
                .Where(filter)
                .Include(b => b.BookStats)
                .Include(l => l.LanguageUser).ThenInclude(lu => lu.Language)
                .FirstOrDefault();
            }
        }
        #endregion

        #region Pages
        public static List<Page> Pages(Expression<Func<Page, bool>> filter)
        {
            using (var context = new IdiomaticaContext())
            {
                return context.Pages
                .Where(filter)
                .ToList();
            }
        }
        public static Page? PageAndParagraphsAndSentencesAndTokensAndWords(Expression<Func<Page, bool>> filter)
        {
            using (var context = new IdiomaticaContext())
            {
                return context.Pages
                .Where(filter)
                .Include(p => p.Paragraphs)
                    .ThenInclude(s => s.Sentences)
                    .ThenInclude(s => s.Tokens)
                    .ThenInclude(t => t.Word)
                .FirstOrDefault();
            }
        }
        #endregion

        #region Paragraphs
        public static List<Paragraph> Paragraphs(Expression<Func<Paragraph, bool>> filter)
        {
            using (var context = new IdiomaticaContext())
            {
                return context.Paragraphs
                .Where(filter)
                .ToList();
            }
        }
        public static List<Paragraph> ParagraphsAndSentencesAndTokensAndWords(Expression<Func<Paragraph, bool>> filter)
        {
            using (var context = new IdiomaticaContext())
            {
                return context.Paragraphs
                .Where(filter)
                .Include(s => s.Sentences)
                    .ThenInclude(s => s.Tokens)
                        .ThenInclude(t => t.Word)
                .ToList();
            }
        }
        #endregion

        #region Words
        public static List<Word> WordsAndChildrenAndParents(Expression<Func<Word, bool>> filter)
        {
            using (var context = new IdiomaticaContext())
            {
                return context.Words
                .Where(filter)
                .Include(w => w.ParentWords)
                .Include(w => w.ChildWords)
                .ToList();
            }
        }

        #endregion
    }
}
