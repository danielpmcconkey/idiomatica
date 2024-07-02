using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Model.DAL;
using Model;
using System.Net;
using Logic.Telemetry;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TestsBench.Tests
{
    public class IntegrationTest1
    {
        //private ILogger<IdiomaticaLogger> _logger;
        //private ErrorHandler _errorHandler;
        //private DeepLService _deepLService;
        private ServiceProvider _serviceProvider;

        #region constants
        const string _newBookTitle = "El gigante egoísta";
        const string _newBookUrl = "https://www.youtube.com/watch?v=1iQh6IJwO9U";
        const string _newBookLanguageCode = "ES";
        const string _newBookText = @"El gigante egoísta
De Óscar Wilde
Versión en español por SpanishInput


Primera parte: voy a construir una pared

Cada tarde, después de la escuela
Los niños iban a jugar al jardín del gigante
Era un jardín grande y bonito
Por todos lados había flores
También había  árboles llenos  de flores
A los pájaros les gustaba  descansar en los árboles y cantar
A los niños, les gustaba oír a los pájaros
Todos eran muy felices

Un día, el gigante volvió 
Había estado con un amigo por siete años
Después de los siete años, volvió a su casa
Llegó y vio a los niños jugando en jardín
""¿Qué hacen aquí?"" dijo con voz fuerte
Los niños se fueron corriendo
""¡Mi jardín es mi jardín!"" dijo el gigante
""¡Nadie más  puede venir a jugar aquí!""
""¡Voy a construir una pared para que nadie venga aquí!""

Hizo una pared grande alrededor del jardín
Y puso un cartel que decía, ""no entrar""
Era un gigante muy egoísta

Segunda parte: corazón frío, jardín frío
Los niños ahora no tenían dónde  jugar
La calle estaba llena de polvo y piedras
Así que  no les gustó jugar ahí 
Un día, al salir de la escuela
Fueron hasta la pared del jardín
Se pusieron a recordar el jardín
Dijeron, ""ahí  dentro éramos felices""
Entonces vino la primavera
Y por todo el país, había flores y pájaros
Solo en el jardín del gigante, aún hacía frío
Los pájaros no vinieron a cantar
Porque no había niños
Y los árboles no dieron flores
Un día, salió un hermosa flor
Pero cuando la flor vio el cartel
Se puso triste por los niños, y volvió a suelo
Solo el hielo estaba feliz. Dijo
""La primavera no ha venido a este jardín,
Asi que estaré aquí todo el año""
El jardín se volvió frío y blanco.
El hielo invitó al viento del norte, y vino
El viento frió pasó todo el día en el jardín
Y rompió las ventanas
El gigante egoísta dijo
""No entiendo por qué  no viene la primavera""
""Quisiera que la primavera viniera  pronto""
Sentado cerca de la ventana
Vio  que su jardín estaba frío y blanco
Pero la primavera nunca vino
Así que en el jardín siempre hacía  frío
El viento del norte y el hielo eran felices ahí 

Tercera parte: el regreso de la primavera
Una mañana, el gigante estaba en su cama
Cuando escuchó una linda canción
En realidad, solo era un pájaro
Cantando afuera de  su ventana
Pero hace mucho tiempo no había oído 
A un pájaro cantar en su jardín
Así que el canto del pájaro
Le pareció la música más hermosa del mundo
Entonces el hielo y el viento frío se fueron
Y un agradable olor entró  por la ventana
El gigante dijo:
""Creo que por fin ha llegado la primavera""
Así que salió de la cama y vio hacia afuera
¿Qué vio?
Vio una escena maravillosa
Los niños habían entrado a través 
De un pequeño hueco en la pared
Cada niño estaba sentado en un árbol
Los árboles estaban muy felices
Así que cada árbol dio muchas flores
Los pájaros estaban volando y cantando
Era una escena hermosa
Solo in un esquina aún había  hielo
En este esquina había un niño pequeño
Era (tan?) pequeño que no podía subir al árbol
Y estaba llorando
El árbol aún  estaba frío y con hielo
El viento del norte aún estaba allí
""¡Sube pequeño!"" dijo el árbol
Pero el niño era demasiado pequeño

Quarta parte: un cambio de actitud
Cuando vio esto, el gigante se puso triste
El gigante pensó:
Ahora sé  por qué  no venía la primavera
Voy a poner a ese niño sobre el árbol
Y voy a destruir la pared
También, voy a dejar que los niños
Vengan a jugar en mi jardín para siempre
Estaba muy triste por lo que había hecho
Así que abrió la puerta  muy despacio
Y salió al jardín
Cuando los niños lo vieron
Tuvieron miedo y se fueron corriendo
El frío otra vez llegó al jardín
Solo el niño pequeño no se fue
Estaba llorando así que no vio al gigante
El gigante lo tomó con cuidado en su mano
Y lo puso sobre el árbol
De inmediato, el árbol dio muchas flores
Y los pájaros vinieron a cantar
El pequeño niño dio un beso al  gigante
Los otros niños se dieron  cuenta
De que el gigante ya no era egoísta
Los niños vinieron corriendo
Yo con ellos vino la primavera
El gigante les dijo los niños
""Este jardín ahora es de ustedes, niños""
Y empezó a destruir la pared
Cuando la gente pasó por ahí
Vieron al gigante jugando con los niños
Era el jardín más hermoso del mundo

Fin
";
        #endregion

        #region setup functions

        public IntegrationTest1()
        {
            //setup our DI
            var connectionstring = "Server=localhost;Database=Idiomatica;Trusted_Connection=True;TrustServerCertificate=true;";

            _serviceProvider = new ServiceCollection()
                .AddLogging()
                //.AddCascadingAuthenticationState()
                //.AddScoped<IdentityUserAccessor>()
                //.AddScoped<IdentityRedirectManager>()
                .AddScoped<AuthenticationStateProvider, RevalidatingProviderForUnitTesting>()
                .AddTransient<BookService>()
                .AddTransient<UserService>()
                .AddTransient<FlashCardService>()
                .AddTransient<ErrorHandler>()
                .AddTransient<DeepLService>()
                //.AddDbContext<IdiomaticaContext>(options => {
                //    options.UseSqlServer(connectionstring, b => b.MigrationsAssembly("IdiomaticaWeb"));
                //    })
                .BuildServiceProvider();
        }
        private BookService CreateBookService()
        {
            return _serviceProvider.GetService<BookService>();
            //return new BookService(_logger, _errorHandler, _deepLService);
        }
        private UserService CreateUserService()
        {
            //return new UserService(null);
            return _serviceProvider.GetService<UserService>();
        }
        private IdiomaticaContext CreateContext()
        {
            //var factory = _serviceProvider.GetService<DbContext<IdiomaticaContext>>();
            //var context = factory.CreateDbContext();
            //return context;
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
            var bookService = CreateBookService();
            int bookId = 7;
            int userId = (int)user.Id;
            int firstBookUserId = 999;
            try
            {
                // act
                firstBookUserId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
                int newBookId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
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
            var bookService = CreateBookService();
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = 999;
            try
            {
                // act
                bookUserId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
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
            var bookService = CreateBookService();
            int book1Id = 7;
            int book2Id = 6;
            int userId = (int)user.Id;
            int bookUser1Id = await bookService.BookUserCreateAndSaveAsync(context, book1Id, userId);
            int bookUser2Id = await bookService.BookUserCreateAndSaveAsync(context, book2Id, userId);
            try
            {
                // act

                List<BookListRow> bookListRows = new List<BookListRow>();// await DataCache.BookListRowsByUserIdReadAsync(userId, context);
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
        [Fact]
        public async Task AddingATagToABookWillIncrementItsCount()
        {
            // arrange
            var context = CreateContext();
            var userService = CreateUserService();
            var user1 = CreateNewTestUser(userService);
            var user2 = CreateNewTestUser(userService);
            int userId1 = (int)user1.Id;
            int userId2 = (int)user2.Id;
            var bookService = CreateBookService();
            int bookId = 7;
            string tag = Guid.NewGuid().ToString();
            int initialCount = 0;
            int secondCount = 0;
            BookTag initialTag = null;
            BookTag secondTag = null;

            try
            {
                // act
                await bookService.BookTagAdd(context, bookId, userId1, tag);
                var initialTags = await bookService.BookTagsGetByBookIdAndUserId(context, bookId, userId1);
                initialTag = initialTags.Where(x => x.Tag == tag).FirstOrDefault();
                initialCount = (int)initialTag.Count;

                // set the initial tags state before adding it for user 2
                var secondTags = await bookService.BookTagsGetByBookIdAndUserId(context, bookId, userId2);
                await bookService.BookTagAdd(context, bookId, userId2, tag);
                // pull the second user's tags again now
                secondTags = await bookService.BookTagsGetByBookIdAndUserId(context, bookId, userId2);
                secondTag = secondTags.Where(x => x.Tag == tag).FirstOrDefault();
                secondCount = (int)secondTag.Count;

                // assert
                Assert.True(initialCount == 1);
                Assert.True(secondCount == 2);
            }
            finally
            {
                // clean-up
                context.BookTags.RemoveRange(
                    context.BookTags.Where(x => x.UserId == userId1 || x.UserId == userId2));
                
                context.Users.Remove(user1);
                context.Users.Remove(user2);
                context.SaveChanges();
            }
        }
        [Fact]
        public async Task RemovingATagFromABookWillDecrementItsCount()
        {
            // arrange
            var context = CreateContext();
            var userService = CreateUserService();
            var user1 = CreateNewTestUser(userService);
            var user2 = CreateNewTestUser(userService);
            var user3 = CreateNewTestUser(userService);
            int userId1 = (int)user1.Id;
            int userId2 = (int)user2.Id; 
            int userId3 = (int)user3.Id;
            var bookService = CreateBookService();
            int bookId = 7;
            string tag = Guid.NewGuid().ToString();
            int secondCount = 0;
            // add the first two tags
            await bookService.BookTagAdd(context, bookId, userId1, tag);
            await bookService.BookTagAdd(context, bookId, userId2, tag);
            // pre-load the tags so a list exists in cache to update. This
            // mimics the online flow. You can only add tags from a page that's already pulled them
            await bookService.BookTagsGetByBookIdAndUserId(context, bookId, userId3);
            // add the user3 tag
            await bookService.BookTagAdd(context, bookId, userId3, tag);
            // now get the count for user 3 (should be 3)
            var initialTags = await bookService.BookTagsGetByBookIdAndUserId(context, bookId, userId3);
            var initialTag = initialTags.Where(x => x.Tag == tag).FirstOrDefault();
            int initialCount = (int)initialTag.Count;


            try
            {
                // act

                // remove the tag from user 1
                await bookService.BookTagRemove(context, bookId, userId1, tag);
                
                // get the tags from user 3 again
                var secondTags = await bookService.BookTagsGetByBookIdAndUserId(context, bookId, userId3);
                var secondTag = secondTags.Where(x => x.Tag == tag).FirstOrDefault();
                secondCount = (int)secondTag.Count;


                // assert
                Assert.True(initialCount == 3);
                Assert.True(secondCount == 2);
            }
            finally
            {
                // clean-up
                context.BookTags.RemoveRange(
                    context.BookTags.Where(x => x.UserId == userId1 || x.UserId == userId2 || x.UserId == userId3));

                context.Users.Remove(user1);
                context.Users.Remove(user2);
                context.Users.Remove(user3);
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
            var bookService = CreateBookService();
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            try
            {
                // act

                if (bookService.IsDataInitRead == false)
                {
                    await bookService.InitDataRead(context, userService, bookId);
                }

                var title = bookService.BookTitle;
                var pageNum = bookService.BookCurrentPageNum;
                var totalPages = bookService.BookTotalPageCount;
                var numParagraphs = bookService.Paragraphs.Count;
                var sentences = bookService.Paragraphs[3].Sentences;
                var sentenceCount = sentences.Count;
                var sentence = sentences[1];
                var tokenCount = sentence.Tokens.Count;
                var token = sentence.Tokens[2]; // administrativa
                var word = token.Word;
                var wordUser = bookService.AllWordUsersInPage[token.Word.TextLowerCase];
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
            var bookService = CreateBookService();
            var user = CreateNewTestUser(userService);
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            
            try
            {
                // act

                if (bookService.IsDataInitRead == false)
                {
                    await bookService.InitDataRead(context, userService, bookId);
                }
                var wordUser = bookService.AllWordUsersInPage["administrativa"];
                wordUser.Status = AvailableWordUserStatus.LEARNED;
                await bookService.WordUserSaveModalDataAsync(
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
            var bookService = CreateBookService();
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            string newTranslation = "administrative";
            try
            {
                // act

                if (bookService.IsDataInitRead == false)
                {
                    await bookService.InitDataRead(context, userService, bookId);
                }
                var wordUser = bookService.AllWordUsersInPage["administrativa"];
                wordUser.Translation = newTranslation;
                await bookService.WordUserSaveModalDataAsync(
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
            var bookService = CreateBookService();
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            try
            {
                // act

                if (bookService.IsDataInitRead == false)
                {
                    await bookService.InitDataRead(context, userService, bookId);
                }
                
                var word1 = bookService.Paragraphs[2].Sentences[4].Tokens[4].Word; // de
                var wordUser1 = bookService.AllWordUsersInPage[word1.TextLowerCase];
                wordUser1.Status = AvailableWordUserStatus.NEW2;
                var wordUserStatus1 = wordUser1.Status.ToString();
                await bookService.WordUserSaveModalDataAsync(
                    context, wordUser1.Id, wordUser1.Status, wordUser1.Translation);

                await bookService.PageMove(context, 2);
                var word2 = bookService.Paragraphs[2].Sentences[0].Tokens[4].Word; // de
                var wordUser2 = bookService.AllWordUsersInPage[word2.TextLowerCase];
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

        [Fact]
        public async Task ParagraphTranslationWorks()
        {
            // arrange
            var context = CreateContext();
            var userService = CreateUserService();
            var user = CreateNewTestUser(userService);
            var bookService = CreateBookService();
            // create the book
            int bookId = await bookService.BookCreateAndSaveAsync(context, 
                _newBookTitle, _newBookLanguageCode, _newBookUrl, _newBookText);
            // add the book stats
            bookService.BookStatsCreateAndSave(context, bookId);
            // now create the book user for teh logged in user
            int userId = (int)user.Id;
            int bookUserId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            
            try
            {
                // act

                if (bookService.IsDataInitRead == false)
                {
                    await bookService.InitDataRead(context, userService, bookId);
                }
                var pp = bookService.Paragraphs[0];
                var translation = await bookService.ParagraphTranslate(context, pp);
                var expectedTranslation = "The selfish giant";
                var actualTranslation = pp.ParagraphTranslations[0].TranslationText;

                // assert
                Assert.Equal(expectedTranslation, actualTranslation);
            }
            finally
            {
                // clean-up
                var book = context.Books.Where(x => x.Id == bookId).FirstOrDefault();
                if (book != null)
                {
                    book.Language = null;
                    book.Pages = null;
                    book.BookStats = null;
                    book.BookUserStats = null;
                    book.BookUsers = null;
                    context.Books.Remove(book);
                }
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task MovingPagesUpdatesTheBookmarkAndReadDate()
        {
            // arrange
            var context = CreateContext();
            var userService = CreateUserService();
            var user = CreateNewTestUser(userService);
            var bookService = CreateBookService();
            int bookId = 7;
            int userId = (int)user.Id;
            int bookUserId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            try
            {
                // act

                if (bookService.IsDataInitRead == false)
                {
                    await bookService.InitDataRead(context, userService, bookId);
                }
                int originalPageId = bookService.BookCurrentPageId;
                await bookService.PageMove(context, 2);
                var bookUserFromDb = await DataCache.BookUserByIdReadAsync(bookUserId, context);
                int bookmark = bookUserFromDb.CurrentPageID;
                int expected = 80;
                var pageUsers = await DataCache.PageUsersByBookUserIdReadAsync(bookUserId, context);
                var originalPageUser = pageUsers.Where(x => x.PageId == originalPageId).FirstOrDefault();
                var originalPageUserReadDate = originalPageUser.ReadDate;

                // assert
                Assert.Equal(bookmark, expected);
                Assert.NotNull(originalPageUserReadDate);
                Assert.True(originalPageUserReadDate >=  DateTime.Now.AddMinutes(-10));
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