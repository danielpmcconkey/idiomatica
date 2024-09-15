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
            var value = context.Books.Where(x => x.Id == key)
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
            int numRows = context.Database.ExecuteSql($"""
                                
                INSERT INTO [Idioma].[Book]
                           ([LanguageKey]
                           ,[Title]
                           ,[SourceURI]
                           ,[Id])
                     VALUES
                           ({book.LanguageId}
                           ,{book.Title}
                           ,{book.SourceURI}
                           ,{book.Id});
                """);
            if (numRows < 1) throw new InvalidDataException("Book create affected 0 rows");
            
            
            BookById[(Guid)book.Id] = book;
            return book;
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
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageKey = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageKey
                left join [Idioma].[ParagraphTranslation] ppt on pp.Id = ppt.ParagraphKey
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on ppt.Id = fcptb.ParagraphTranslationKey
                left join [Idioma].[Sentence] s on pp.Id = s.ParagraphKey
                left join [Idioma].[Token] t on s.Id = t.SentenceKey
                where b.Id = {bookId};

                delete s
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageKey = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageKey
                left join [Idioma].[ParagraphTranslation] ppt on pp.Id = ppt.ParagraphKey
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on ppt.Id = fcptb.ParagraphTranslationKey
                left join [Idioma].[Sentence] s on pp.Id = s.ParagraphKey
                where b.Id = {bookId};

                delete fcptb
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageKey = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageKey
                left join [Idioma].[ParagraphTranslation] ppt on pp.Id = ppt.ParagraphKey
                left join [Idioma].[FlashCardParagraphTranslationBridge] fcptb on ppt.Id = fcptb.ParagraphTranslationKey
                where b.Id = {bookId};

                delete ppt
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageKey = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageKey
                left join [Idioma].[ParagraphTranslation] ppt on pp.Id = ppt.ParagraphKey
                where b.Id = {bookId};

                delete pp
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageKey = p.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageKey
                where b.Id = {bookId};

                delete pu
                from Idioma.Book b
                left join [Idioma].[BookUser] bu on b.Id = bu.BookId
                left join [Idioma].[BookStat] bs on b.Id = bs.BookId
                left join [Idioma].[BookTag] bt on b.Id = bt.BookId
                left join [Idioma].[BookUserStat] bus on b.Id = bus.BookId
                left join [Idioma].[Page] p on b.Id = p.BookId
                left join [Idioma].[PageUser] pu on pu.PageKey = p.Id
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
