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
            var task = BookByIdReadAsync(key, context);
            return task.Result;
        }
        public static async Task<Book?> BookByIdReadAsync(int key, IdiomaticaContext context)
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

    }
}
