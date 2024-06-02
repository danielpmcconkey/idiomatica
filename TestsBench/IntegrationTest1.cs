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
        #region setup functions
        BookService? _bookService = null;

        public IntegrationTest1()
        {
            _bookService = new BookService();
        }
        private UserService CreateUserService()
        {
            return new UserService(null);
        }
        private IdiomaticaContext CreateContext()
        {

            var connectionstring = "Server=localhost;Database=Idiomatica;Trusted_Connection=True;TrustServerCertificate=true;";
            var optionsBuilder = new DbContextOptionsBuilder<IdiomaticaContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            return new IdiomaticaContext(optionsBuilder.Options);
        }
        private User CreateNewTestUser(UserService userService)
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

#if DEBUG
            SetLoggedInUser(user, userService);
#endif

            return user;
        }
#if DEBUG
        private void SetLoggedInUser(User user, UserService userService)
        {
            var context = CreateContext();
            userService.SetLoggedInUserForTestBench(user, context);
        }
#endif
        #endregion


        #region BookList
        
        [Fact]
        public async Task ICannotAddTheSameBookUserTwice()
        {
            // arrange
            var context = CreateContext();
            var userService = CreateUserService();
            var user = CreateNewTestUser(userService);
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
            var userService = CreateUserService();
            var user = CreateNewTestUser(userService);
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
            var userService = CreateUserService();
            var user = CreateNewTestUser(userService);
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
            var userService = CreateUserService();
            var user = CreateNewTestUser(userService);
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await _bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            BookService BookService = new BookService();
            try
            {
                // act

                if (BookService.IsDataInitRead == false)
                {
                    await BookService.InitDataRead(context, userService, bookId);
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
                if (bookUser != null)
                {
                    bookUser.LanguageUser = null;
                    bookUser.Book = null;
                    bookUser.PageUsers = new List<PageUser>();
                    context.BookUsers.Remove(bookUser);
                }
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task UpdatingWordStatusWorks()
        {
            // arrange
            var context = CreateContext();
            var userService = CreateUserService();
            var user = CreateNewTestUser(userService);
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await _bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            BookService BookService = new BookService();
            try
            {
                // act

                if (BookService.IsDataInitRead == false)
                {
                    await BookService.InitDataRead(context, userService, bookId);
                }
                var wordUser = BookService.AllWordUsersInPage["administrativa"];
                wordUser.Status = AvailableWordUserStatus.LEARNED;
                await BookService.WordUserSaveModalDataAsync(
                    context, wordUser.Id, wordUser.Status, wordUser.Translation);

                var wordUserFromDb = context.WordUsers.FirstOrDefault(x => x.Id == wordUser.Id);

                // assert
                Assert.Equal(wordUserFromDb.Status.ToString(), AvailableWordUserStatus.LEARNED.ToString());
            }
            finally
            {
                // clean-up
                var bookUser = context.BookUsers.Where(x => x.Id == bookUserId).FirstOrDefault();
                if (bookUser != null)
                {
                    bookUser.LanguageUser = null;
                    bookUser.Book = null;
                    bookUser.PageUsers = new List<PageUser>();
                    context.BookUsers.Remove(bookUser);
                }
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task UpdatingWordTranslationWorks()
        {
            // arrange
            var context = CreateContext();
            var userService = CreateUserService();
            var user = CreateNewTestUser(userService);
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await _bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            BookService BookService = new BookService();
            string newTranslation = "administrative";
            try
            {
                // act

                if (BookService.IsDataInitRead == false)
                {
                    await BookService.InitDataRead(context, userService, bookId);
                }
                var wordUser = BookService.AllWordUsersInPage["administrativa"];
                wordUser.Translation = newTranslation;
                await BookService.WordUserSaveModalDataAsync(
                    context, wordUser.Id, wordUser.Status, wordUser.Translation);

                var wordUserFromDb = context.WordUsers.FirstOrDefault(x => x.Id == wordUser.Id);

                // assert
                Assert.Equal(wordUserFromDb.Translation, newTranslation);
            }
            finally
            {
                // clean-up
                var bookUser = context.BookUsers.Where(x => x.Id == bookUserId).FirstOrDefault();
                if (bookUser != null)
                {
                    bookUser.LanguageUser = null;
                    bookUser.Book = null;
                    bookUser.PageUsers = new List<PageUser>();
                    context.BookUsers.Remove(bookUser);
                }
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task UpdatingWordStatusIsReflectedOnOtherPages()
        {
            // arrange
            var context = CreateContext();
            var userService = CreateUserService();
            var user = CreateNewTestUser(userService);
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await _bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            BookService BookService = new BookService();
            try
            {
                // act

                if (BookService.IsDataInitRead == false)
                {
                    await BookService.InitDataRead(context, userService, bookId);
                }
                
                var word1 = BookService.Paragraphs[2].Sentences[4].Tokens[4].Word; // de
                var wordUser1 = BookService.AllWordUsersInPage[word1.TextLowerCase];
                wordUser1.Status = AvailableWordUserStatus.NEW2;
                var wordUserStatus1 = wordUser1.Status.ToString();
                await BookService.WordUserSaveModalDataAsync(
                    context, wordUser1.Id, wordUser1.Status, wordUser1.Translation);

                await BookService.PageMove(context, 2);
                var word2 = BookService.Paragraphs[2].Sentences[0].Tokens[4].Word; // de
                var wordUser2 = BookService.AllWordUsersInPage[word2.TextLowerCase];
                var wordUserStatus2 = AvailableWordUserStatus.NEW2.ToString();

                // assert
                Assert.Equal(wordUserStatus1, wordUserStatus2);
            }
            finally
            {
                // clean-up
                var bookUser = context.BookUsers.Where(x => x.Id == bookUserId).FirstOrDefault();
                if (bookUser != null)
                {
                    bookUser.LanguageUser = null;
                    bookUser.Book = null;
                    bookUser.PageUsers = new List<PageUser>();
                    context.BookUsers.Remove(bookUser);
                }
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        #endregion
    }
}