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
        private static ConcurrentDictionary<Guid, Book> BookById = [];


        #region read
        public static Book? BookByIdRead(Guid key, IdiomaticaContext context)
        {
            // check cache
            if (BookById.ContainsKey(key))
            {
                return BookById[key];
            }

            // read DB
            var value = context.Books.Where(x => x.UniqueKey == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookById[key] = value;
            return value;
        }
        public static async Task<Book?> BookByIdReadAsync(Guid key, IdiomaticaContext context)
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
                           ([LanguageKey]
                           ,[Title]
                           ,[SourceURI]
                           ,[AudioFilename]
                           ,[UniqueKey])
                     VALUES
                           ({book.LanguageKey}
                           ,{book.Title}
                           ,{book.SourceURI}
                           ,{book.AudioFilename}
                           ,{guid});
                """);
            if (numRows < 1) throw new InvalidDataException("Book create affected 0 rows");
            // now read it into context
            var newBook = context.Books.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newBook is null || newBook.UniqueKey is null) 
                throw new InvalidDataException("Reading new book after creation returned null");
            
            BookById[(Guid)newBook.UniqueKey] = newBook;
            return newBook;
        }
        public static async Task<Book?> BookCreateAsync(Book value, IdiomaticaContext context)
        {
            return await Task.Run(() => { return BookCreate(value, context); });
        }
        #endregion

        #region delete
        public static void BookAndAllChildrenDelete(Guid bookId, IdiomaticaContext context)
        {
            Guid guid = Guid.NewGuid();
            context.Database.ExecuteSql($"""

                delete t
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                left join [Idioma].[BookTag] bt on b.UniqueKey = bt.BookKey
                left join [Idioma].[BookUserStat] bus on b.UniqueKey = bus.BookKey
                left join [Idioma].[Page] p on b.UniqueKey = p.BookKey
                left join [Idioma].[PageUser] pu on pu.PageKey = p.UniqueKey
                left join [Idioma].[Paragraph] pp on p.UniqueKey = pp.PageKey
                left join [Idioma].[ParagraphTranslation] ppt on pp.UniqueKey = ppt.ParagraphKey
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on ppt.UniqueKey = fcptb.ParagraphTranslationKey
                left join [Idioma].[Sentence] s on pp.UniqueKey = s.ParagraphKey
                left join [Idioma].[Token] t on s.UniqueKey = t.SentenceKey
                where b.UniqueKey = {bookId};

                delete s
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                left join [Idioma].[BookTag] bt on b.UniqueKey = bt.BookKey
                left join [Idioma].[BookUserStat] bus on b.UniqueKey = bus.BookKey
                left join [Idioma].[Page] p on b.UniqueKey = p.BookKey
                left join [Idioma].[PageUser] pu on pu.PageKey = p.UniqueKey
                left join [Idioma].[Paragraph] pp on p.UniqueKey = pp.PageKey
                left join [Idioma].[ParagraphTranslation] ppt on pp.UniqueKey = ppt.ParagraphKey
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on ppt.UniqueKey = fcptb.ParagraphTranslationKey
                left join [Idioma].[Sentence] s on pp.UniqueKey = s.ParagraphKey
                where b.UniqueKey = {bookId};

                delete fcptb
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                left join [Idioma].[BookTag] bt on b.UniqueKey = bt.BookKey
                left join [Idioma].[BookUserStat] bus on b.UniqueKey = bus.BookKey
                left join [Idioma].[Page] p on b.UniqueKey = p.BookKey
                left join [Idioma].[PageUser] pu on pu.PageKey = p.UniqueKey
                left join [Idioma].[Paragraph] pp on p.UniqueKey = pp.PageKey
                left join [Idioma].[ParagraphTranslation] ppt on pp.UniqueKey = ppt.ParagraphKey
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on ppt.UniqueKey = fcptb.ParagraphTranslationKey
                where b.UniqueKey = {bookId};

                delete ppt
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                left join [Idioma].[BookTag] bt on b.UniqueKey = bt.BookKey
                left join [Idioma].[BookUserStat] bus on b.UniqueKey = bus.BookKey
                left join [Idioma].[Page] p on b.UniqueKey = p.BookKey
                left join [Idioma].[PageUser] pu on pu.PageKey = p.UniqueKey
                left join [Idioma].[Paragraph] pp on p.UniqueKey = pp.PageKey
                left join [Idioma].[ParagraphTranslation] ppt on pp.UniqueKey = ppt.ParagraphKey
                where b.UniqueKey = {bookId};

                delete pp
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                left join [Idioma].[BookTag] bt on b.UniqueKey = bt.BookKey
                left join [Idioma].[BookUserStat] bus on b.UniqueKey = bus.BookKey
                left join [Idioma].[Page] p on b.UniqueKey = p.BookKey
                left join [Idioma].[PageUser] pu on pu.PageKey = p.UniqueKey
                left join [Idioma].[Paragraph] pp on p.UniqueKey = pp.PageKey
                where b.UniqueKey = {bookId};

                delete pu
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                left join [Idioma].[BookTag] bt on b.UniqueKey = bt.BookKey
                left join [Idioma].[BookUserStat] bus on b.UniqueKey = bus.BookKey
                left join [Idioma].[Page] p on b.UniqueKey = p.BookKey
                left join [Idioma].[PageUser] pu on pu.PageKey = p.UniqueKey
                where b.UniqueKey = {bookId};

                delete p
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                left join [Idioma].[BookTag] bt on b.UniqueKey = bt.BookKey
                left join [Idioma].[BookUserStat] bus on b.UniqueKey = bus.BookKey
                left join [Idioma].[Page] p on b.UniqueKey = p.BookKey
                where b.UniqueKey = {bookId};

                delete bus
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                left join [Idioma].[BookTag] bt on b.UniqueKey = bt.BookKey
                left join [Idioma].[BookUserStat] bus on b.UniqueKey = bus.BookKey
                where b.UniqueKey = {bookId};

                delete bt
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                left join [Idioma].[BookTag] bt on b.UniqueKey = bt.BookKey
                where b.UniqueKey = {bookId};

                delete bs
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                left join [Idioma].[BookStat] bs on b.UniqueKey = bs.BookKey
                where b.UniqueKey = {bookId};

                delete bu
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.UniqueKey = bu.BookKey
                where b.UniqueKey = {bookId};

                delete b
                from Idioma.Book b
                where b.UniqueKey = {bookId};
                
        
                """);



            // delete caches
            var listCachedBooks = BookById.Where(x => x.Value.UniqueKey == bookId).ToList();
            foreach (var cachedEntry in listCachedBooks)
                BookById.Remove(cachedEntry.Key, out Book? deletedValue);

            var listCachedBookListRows = BookListRowByBookIdAndUserId
                .Where(x => x.Value != null && x.Value.BookKey == bookId)
                .ToList();
            foreach (var cachedEntry in listCachedBookListRows)
                BookListRowByBookIdAndUserId.Remove(cachedEntry.Key, out BookListRow? deletedValue);
        }
        #endregion

    }
}
