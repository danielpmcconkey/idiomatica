using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Model.DAL;
using Model;
using System.Net;

namespace TestsBench.Tests
{
    public class IntegrationTest1
    {
        BookService? _bookService = null;
        UserService? _userService = null;

        public IntegrationTest1()
        {
            _bookService = new BookService();
            _userService = new UserService(null);
#if DEBUG
            var context = CreateContext();
            var testUser = DataCache.UserByApplicationUserIdReadAsync("TESTER", context).Result;
            _userService.SetLoggedInUserForTestBench(testUser, context);
#endif
        }
        #region BookList
        private IdiomaticaContext CreateContext()
        {

            var connectionstring = "Server=localhost;Database=Idiomatica;Trusted_Connection=True;TrustServerCertificate=true;";
            var optionsBuilder = new DbContextOptionsBuilder<IdiomaticaContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            return new IdiomaticaContext(optionsBuilder.Options);
        }
        private User CreateNewTestUser()
        {
            var user = new User()
            {
                ApplicationUserId = Guid.NewGuid().ToString(),
                Name = "Auto gen tester",
                Code = "En-US"
            };
            var context = CreateContext();
            context.Users.Add(user);
            context.SaveChanges();
            var languageUser = new LanguageUser()
            {
                LanguageId = 1, // espAnish
                UserId = (int)user.Id,
                TotalWordsRead = 0
            };
            context.LanguageUsers.Add(languageUser);
            context.SaveChanges();

            return user;
        }
        [Fact]
        public async Task ICannotAddTheSameBookUserTwice()
        {
            // arrange
            var context = CreateContext();
            var user = CreateNewTestUser();
            int bookId = 7;
            int userId = (int)user.Id;
            int firstBookUserId = 999;
            try
            {
                // act
                firstBookUserId = await _bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
                int newBookId = await _bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
                // assert
                Assert.Equal(newBookId, firstBookUserId);
            }
            finally
            {
                // clean-up
                var firstBookUser = context.BookUsers.Where(x => x.Id == firstBookUserId).FirstOrDefault();
                if (firstBookUser != null) context.BookUsers.Remove(firstBookUser);
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }

        }

        [Fact]
        public async Task ICanAddABookUser()
        {
            // arrange
            var context = CreateContext();
            var user = CreateNewTestUser();
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = 999;
            try
            {
                // act
                bookUserId = await _bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
                // assert
                Assert.True(bookUserId != 999 && bookUserId > 0);
            }
            finally
            {
                // clean-up
                var bookUser = context.BookUsers.Where(x => x.Id == bookUserId).FirstOrDefault();
                if (bookUser != null) context.BookUsers.Remove(bookUser);
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task BookUsersWillShowBooksOnTheBookList()
        {
            // arrange
            var context = CreateContext();
            var user = CreateNewTestUser();
            int book1Id = 7;
            int book2Id = 6;
            int userId = (int)user.Id;
            int bookUser1Id = await _bookService.BookUserCreateAndSaveAsync(context, book1Id, userId);
            int bookUser2Id = await _bookService.BookUserCreateAndSaveAsync(context, book2Id, userId);
            try
            {
                // act

                List<BookListRow> bookListRows = await DataCache.BookListRowsByUserIdReadAsync(userId, context);
                var book1 = await DataCache.BookByIdReadAsync(book1Id, context);
                var book2 = await DataCache.BookByIdReadAsync(book2Id, context);
                List<string> titlesInList = new List<string>();
                titlesInList.Add(book1.Title);
                titlesInList.Add(book2.Title);
                var titlesInConcat = string.Join("|", titlesInList.Order());
                List<string> titlesOutList = new List<string>();
                titlesOutList.Add(bookListRows[0].Title);
                titlesOutList.Add(bookListRows[1].Title);
                var titlesOutConcat = string.Join("|", titlesOutList.Order());

                // assert
                Assert.Equal(titlesInConcat, titlesOutConcat);
            }
            finally
            {
                // clean-up
                var bookUser1 = context.BookUsers.Where(x => x.Id == bookUser1Id).FirstOrDefault();
                if (bookUser1 != null) context.BookUsers.Remove(bookUser1);
                var bookUser2 = context.BookUsers.Where(x => x.Id == bookUser2Id).FirstOrDefault();
                if (bookUser2 != null) context.BookUsers.Remove(bookUser2);
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
        #endregion

        #region Read
        [Fact]
        public async Task ReadingBook7Page1ShowsCorrectPageWordsAndSentences()
        {
            // arrange
            var context = CreateContext();
            var user = CreateNewTestUser();
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await _bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            BookService BookService = new BookService();
            try
            {
                // act

                if (BookService.IsDataInitRead == false)
                {
                    await BookService.InitDataRead(context, _userService, bookId);
                }

                var title = BookService.BookTitle;
                var pageNum = BookService.BookCurrentPageNum;
                var totalPages = BookService.BookTotalPageCount;
                var numParagraphs = BookService.Paragraphs.Count;
                var sentences = BookService.Paragraphs[3].Sentences;
                var sentenceCount = sentences.Count;
                var sentence = sentences[1];
                var tokenCount = sentence.Tokens.Count;
                var token = sentence.Tokens[2]; // administrativa
                var word = token.Word;
                var wordUser = BookService.AllWordUsersInPage[token.Word.TextLowerCase];
                var wordUserStatus = wordUser.Status.ToString();

                // assert
                Assert.Equal(title, "Laura, la mujer invisible");
                Assert.Equal(pageNum, 1);
                Assert.Equal(totalPages, 17);
                Assert.Equal(numParagraphs, 8);
                Assert.Equal(sentenceCount, 7);
                Assert.Equal(tokenCount, 12);
                Assert.Equal(word.TextLowerCase, "administrativa");
                Assert.Equal(wordUserStatus, "UNKNOWN");
            }
            finally
            {
                // clean-up
                var bookUser = context.BookUsers.Where(x => x.Id == bookUserId).FirstOrDefault();
                if (bookUser != null) context.BookUsers.Remove(bookUser);
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
        #endregion
    }
}