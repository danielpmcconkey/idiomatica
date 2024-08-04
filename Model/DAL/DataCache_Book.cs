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
        private static ConcurrentDictionary<int, Book> BookById = new ConcurrentDictionary<int, Book>();


        #region read
        public static Book? BookByIdRead(int key, IdiomaticaContext context)
        {
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
        public static async Task<Book?> BookByIdReadAsync(int key, IdiomaticaContext context)
        {
            return await Task<Book?>.Run(() =>
            {
                return BookByIdRead(key, context);
            });
        }

        #endregion

        #region create
        public static Book? BookCreate(Book book, IdiomaticaContext context)
        {
            var guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                                
                INSERT INTO [Idioma].[Book]
                           ([LanguageId]
                           ,[Title]
                           ,[SourceURI]
                           ,[AudioFilename]
                           ,[UniqueKey])
                     VALUES
                           ({book.LanguageId}
                           ,{book.Title}
                           ,{book.SourceURI}
                           ,{book.AudioFilename}
                           ,{guid});
                """);
            if (numRows < 1) throw new InvalidDataException("Book create affected 0 rows");
            // now read it into context
            var newBook = context.Books.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newBook is null || newBook.Id is null || newBook.Id < 1) 
                throw new InvalidDataException("Reading new book after creation returned null");
            
            BookById[(int)newBook.Id] = newBook;
            return newBook;
        }
        public static async Task<Book?> BookCreateAsync(Book value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return BookCreate(value, context); });
        }
        #endregion

        #region delete
        public static void BookAndAllChildrenDelete(int bookId, IdiomaticaContext context)
        {
            if (bookId < 1) throw new ArgumentException(nameof(bookId));

            Guid guid = Guid.NewGuid();
            context.Database.ExecuteSql($"""

                delete t
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageId = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageId
                left join [Idioma].[ParagraphTranslation] ppt on pp.Id = ppt.ParagraphId
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on ppt.Id = fcptb.ParagraphTranslationId
                left join [Idioma].[Sentence] s on pp.Id = s.ParagraphId
                left join [Idioma].[Token] t on s.Id = t.SentenceId
                where b.Id = {bookId};

                delete s
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageId = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageId
                left join [Idioma].[ParagraphTranslation] ppt on pp.Id = ppt.ParagraphId
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on ppt.Id = fcptb.ParagraphTranslationId
                left join [Idioma].[Sentence] s on pp.Id = s.ParagraphId
                where b.Id = {bookId};

                delete fcptb
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageId = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageId
                left join [Idioma].[ParagraphTranslation] ppt on pp.Id = ppt.ParagraphId
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on ppt.Id = fcptb.ParagraphTranslationId
                where b.Id = {bookId};

                delete ppt
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageId = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageId
                left join [Idioma].[ParagraphTranslation] ppt on pp.Id = ppt.ParagraphId
                where b.Id = {bookId};

                delete pp
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageId = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageId
                where b.Id = {bookId};

                delete pu
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageId = p.Id
                where b.Id = {bookId};

                delete p
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                where b.Id = {bookId};

                delete bus
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                where b.Id = {bookId};

                delete bt
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                where b.Id = {bookId};

                delete bs
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                where b.Id = {bookId};

                delete bu
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                where b.Id = {bookId};

                delete b
                from Idioma.Book b
                where b.Id = {bookId};
                
        
                """);



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
