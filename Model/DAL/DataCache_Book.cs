using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        


        #region read
        public static Book? BookByIdRead(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            // check cache
            if (BookById.ContainsKey(key))
            {
                return BookById[key];
            }

            // read DB
            var value = context.Books.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookById[key] = value;
            return value;
        }
        public static async Task<Book?> BookByIdReadAsync(Guid key, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task<Book?>.Run(() =>
            {
                return BookByIdRead(key, dbContextFactory);
            });
        }

        #endregion

        #region create
        public static Book? BookCreate(Book book, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            //int numRows = context.Database.ExecuteSql($"""
                                
            //    INSERT INTO [Idioma].[Book]
            //               ([LanguageId]
            //               ,[Title]
            //               ,[SourceURI]
            //               ,[Id])
            //         VALUES
            //               ({book.LanguageId}
            //               ,{book.Title}
            //               ,{book.SourceURI}
            //               ,{book.Id});
            //    """);
            //if (numRows < 1) throw new InvalidDataException("Book create affected 0 rows");
            
            context.Books.Add(book);
            context.SaveChanges();
            BookById[(Guid)book.Id] = book;
            return book;
        }
        public static async Task<Book?> BookCreateAsync(Book value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return BookCreate(value, dbContextFactory); });
        }
        #endregion

        #region delete
        public static void BookAndAllChildrenDelete(Guid bookId, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            /* 
             * this makes sure we delete everything even though we have cascade
             * foreign keys set up. SQL server is very conservative about what 
             * you can delete via keys to avoid possible race conditions. So we
             * do it manually here and we do it in order
             * */

            var context = dbContextFactory.CreateDbContext();

            var tokens = context.Tokens.Where(x =>
                x.Sentence != null &&     
                x.Sentence.Paragraph != null &&
                x.Sentence.Paragraph.Page != null &&
                x.Sentence.Paragraph.Page.BookId == bookId);
            context.Tokens.RemoveRange(tokens);
            context.SaveChanges();

            var sentences = context.Sentences.Where(x => 
                x.Paragraph != null &&
                x.Paragraph.Page != null &&
                x.Paragraph.Page.BookId == bookId);
            context.Sentences.RemoveRange(sentences);
            context.SaveChanges();

            var fcptb = context.FlashCardParagraphTranslationBridges.Where(x =>
                x.ParagraphTranslation != null &&
                x.ParagraphTranslation.Paragraph != null &&
                x.ParagraphTranslation.Paragraph.Page != null &&
                x.ParagraphTranslation.Paragraph.Page.BookId == bookId
            );
            context.FlashCardParagraphTranslationBridges.RemoveRange(fcptb);
            context.SaveChanges();

            var paragraphTranslations = context.ParagraphTranslations.Where(x =>
                x.Paragraph != null &&
                x.Paragraph.Page != null &&
                x.Paragraph.Page.BookId == bookId
            );
            context.ParagraphTranslations.RemoveRange(paragraphTranslations);
            context.SaveChanges();

            var paragraphs = context.Paragraphs.Where(x =>
                x.Page != null &&
                x.Page.BookId == bookId
            );
            context.Paragraphs.RemoveRange(paragraphs);
            context.SaveChanges();

            var pageUsers = context.PageUsers.Where(x =>
                x.Page != null &&
                x.Page.BookId == bookId
            );
            context.PageUsers.RemoveRange(pageUsers);
            context.SaveChanges();

            var pages = context.Pages.Where(x => x.BookId == bookId);
            context.Pages.RemoveRange(pages);
            context.SaveChanges();

            var bookUserStats = context.BookUserStats.Where(x =>
                x.BookId == bookId
            );
            context.BookUserStats.RemoveRange(bookUserStats);
            context.SaveChanges();

            var bookTags = context.BookTags.Where(x =>
                x.BookId == bookId
            );
            context.BookTags.RemoveRange(bookTags);
            context.SaveChanges();

            var bookStats = context.BookStats.Where(x =>
                x.BookId == bookId
            );
            context.BookStats.RemoveRange(bookStats);
            context.SaveChanges();

            var bookUsers = context.BookUsers.Where(x =>
                x.BookId == bookId
            );
            context.BookUsers.RemoveRange(bookUsers);
            context.SaveChanges();

            var books = context.Books.Where(x => x.Id == bookId);
            context.Books.RemoveRange(books);
            context.SaveChanges();

            // delete caches
            var listCachedBooks = BookById.Where(x => x.Value.Id == bookId).ToList();
            foreach (var cachedEntry in listCachedBooks)
                BookById.Remove(cachedEntry.Key, out Book? deletedValue);

            var listCachedBookListRows = BookListRowByBookIdAndUserId
                .Where(x => x.Value != null && x.Value.BookId == bookId)
                .ToList();
            foreach (var cachedEntry in listCachedBookListRows)
                BookListRowByBookIdAndUserId.Remove(cachedEntry.Key, out BookListRow? deletedValue);
        }
        #endregion

    }
}
