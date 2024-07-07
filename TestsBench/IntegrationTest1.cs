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
using IdentityModel.OidcClient.Browser;
using System.Text;

namespace TestsBench.Tests
{
    public class IntegrationTest1
    {
        //private ILogger<IdiomaticaLogger> _logger;
        //private ErrorHandler _errorHandler;
        //private DeepLService _deepLService;

        #region constants
        const string _newBookTitle = "El gigante ego�sta";
        const string _newBookUrl = "https://www.youtube.com/watch?v=1iQh6IJwO9U";
        const string _newBookLanguageCode = "ES";
        const string _newBookText = @"El gigante ego�sta
De �scar Wilde
Versi�n en espa�ol por SpanishInput


Primera parte: voy a construir una pared

Cada tarde, despu�s de la escuela
Los ni�os iban a jugar al jard�n del gigante
Era un jard�n grande y bonito
Por todos lados hab�a flores
Tambi�n hab�a  �rboles llenos  de flores
A los p�jaros les gustaba  descansar en los �rboles y cantar
A los ni�os, les gustaba o�r a los p�jaros
Todos eran muy felices

Un d�a, el gigante volvi� 
Hab�a estado con un amigo por siete a�os
Despu�s de los siete a�os, volvi� a su casa
Lleg� y vio a los ni�os jugando en jard�n
""�Qu� hacen aqu�?"" dijo con voz fuerte
Los ni�os se fueron corriendo
""�Mi jard�n es mi jard�n!"" dijo el gigante
""�Nadie m�s  puede venir a jugar aqu�!""
""�Voy a construir una pared para que nadie venga aqu�!""

Hizo una pared grande alrededor del jard�n
Y puso un cartel que dec�a, ""no entrar""
Era un gigante muy ego�sta

Segunda parte: coraz�n fr�o, jard�n fr�o
Los ni�os ahora no ten�an d�nde  jugar
La calle estaba llena de polvo y piedras
As� que  no les gust� jugar ah� 
Un d�a, al salir de la escuela
Fueron hasta la pared del jard�n
Se pusieron a recordar el jard�n
Dijeron, ""ah�  dentro �ramos felices""
Entonces vino la primavera
Y por todo el pa�s, hab�a flores y p�jaros
Solo en el jard�n del gigante, a�n hac�a fr�o
Los p�jaros no vinieron a cantar
Porque no hab�a ni�os
Y los �rboles no dieron flores
Un d�a, sali� un hermosa flor
Pero cuando la flor vio el cartel
Se puso triste por los ni�os, y volvi� a suelo
Solo el hielo estaba feliz. Dijo
""La primavera no ha venido a este jard�n,
Asi que estar� aqu� todo el a�o""
El jard�n se volvi� fr�o y blanco.
El hielo invit� al viento del norte, y vino
El viento fri� pas� todo el d�a en el jard�n
Y rompi� las ventanas
El gigante ego�sta dijo
""No entiendo por qu�  no viene la primavera""
""Quisiera que la primavera viniera  pronto""
Sentado cerca de la ventana
Vio  que su jard�n estaba fr�o y blanco
Pero la primavera nunca vino
As� que en el jard�n siempre hac�a  fr�o
El viento del norte y el hielo eran felices ah� 

Tercera parte: el regreso de la primavera
Una ma�ana, el gigante estaba en su cama
Cuando escuch� una linda canci�n
En realidad, solo era un p�jaro
Cantando afuera de  su ventana
Pero hace mucho tiempo no hab�a o�do 
A un p�jaro cantar en su jard�n
As� que el canto del p�jaro
Le pareci� la m�sica m�s hermosa del mundo
Entonces el hielo y el viento fr�o se fueron
Y un agradable olor entr�  por la ventana
El gigante dijo:
""Creo que por fin ha llegado la primavera""
As� que sali� de la cama y vio hacia afuera
�Qu� vio?
Vio una escena maravillosa
Los ni�os hab�an entrado a trav�s 
De un peque�o hueco en la pared
Cada ni�o estaba sentado en un �rbol
Los �rboles estaban muy felices
As� que cada �rbol dio muchas flores
Los p�jaros estaban volando y cantando
Era una escena hermosa
Solo in un esquina a�n hab�a  hielo
En este esquina hab�a un ni�o peque�o
Era (tan?) peque�o que no pod�a subir al �rbol
Y estaba llorando
El �rbol a�n  estaba fr�o y con hielo
El viento del norte a�n estaba all�
""�Sube peque�o!"" dijo el �rbol
Pero el ni�o era demasiado peque�o

Quarta parte: un cambio de actitud
Cuando vio esto, el gigante se puso triste
El gigante pens�:
Ahora s�  por qu�  no ven�a la primavera
Voy a poner a ese ni�o sobre el �rbol
Y voy a destruir la pared
Tambi�n, voy a dejar que los ni�os
Vengan a jugar en mi jard�n para siempre
Estaba muy triste por lo que hab�a hecho
As� que abri� la puerta  muy despacio
Y sali� al jard�n
Cuando los ni�os lo vieron
Tuvieron miedo y se fueron corriendo
El fr�o otra vez lleg� al jard�n
Solo el ni�o peque�o no se fue
Estaba llorando as� que no vio al gigante
El gigante lo tom� con cuidado en su mano
Y lo puso sobre el �rbol
De inmediato, el �rbol dio muchas flores
Y los p�jaros vinieron a cantar
El peque�o ni�o dio un beso al  gigante
Los otros ni�os se dieron  cuenta
De que el gigante ya no era ego�sta
Los ni�os vinieron corriendo
Yo con ellos vino la primavera
El gigante les dijo los ni�os
""Este jard�n ahora es de ustedes, ni�os""
Y empez� a destruir la pared
Cuando la gente pas� por ah�
Vieron al gigante jugando con los ni�os
Era el jard�n m�s hermoso del mundo

Fin
";
        #endregion


        #region BookList

        [Fact]
        public async Task ICannotAddTheSameBookUserTwice()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
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

                await transaction.RollbackAsync();
            }

        }

        [Fact]
        public async Task ICanAddABookUser()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
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

                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task BookUsersWillShowBooksOnTheBookList()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
#if DEBUG
            // this is only used for the test bench to provide a logged in user outside of the standard app flow
            bookService.SetLoggedInUser(user);
#endif
            bookService.IsBrowse = false;
            await bookService.BookListRowsFilterAndSort(context);
            int book1Id = 7;
            int book2Id = 6;
            int userId = (int)user.Id;
            int bookUser1Id = 0;
            int bookUser2Id = 0;

            try
            {
                // act
                bookUser1Id = await bookService.BookUserCreateAndSaveAsync(context, book1Id, userId);
                bookUser2Id = await bookService.BookUserCreateAndSaveAsync(context, book2Id, userId);
                // force a re-pull of the booklist
                await bookService.BookListRowsFilterAndSort(context);
                // create a concatenated string of the titles of the bOokusers you just created
                var book1 = await DataCache.BookByIdReadAsync(book1Id, context);
                var book2 = await DataCache.BookByIdReadAsync(book2Id, context);
                List<string> titlesInList = new List<string>();
                titlesInList.Add(book1.Title);
                titlesInList.Add(book2.Title);
                var titlesInConcat = string.Join("|", titlesInList.Order());
                // now do the same for the first 2 books in your booklist
                List<string> titlesOutList = new List<string>();
                titlesOutList.Add(bookService.BookListRows[0].Title);
                titlesOutList.Add(bookService.BookListRows[1].Title);
                var titlesOutConcat = string.Join("|", titlesOutList.Order());

                // assert
                Assert.Equal(titlesInConcat, titlesOutConcat);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task ArchivingBookUsersWillTakeThemOffTheBookList()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
#if DEBUG
            // this is only used for the test bench to provide a logged in user outside of the standard app flow
            bookService.SetLoggedInUser(user);
#endif
            bookService.IsBrowse = false;
            await bookService.BookListRowsFilterAndSort(context);
            int book1Id = 7;
            int book2Id = 6;
            int userId = (int)user.Id;
            int bookUser1Id = await bookService.BookUserCreateAndSaveAsync(context, book1Id, userId);
            int bookUser2Id = await bookService.BookUserCreateAndSaveAsync(context, book2Id, userId);
            // reset the book list
            await bookService.BookListRowsFilterAndSort(context);
            // now pull the title of just book 2
            string book2Title = bookService.BookListRows[1].Title;

            try
            {
                // act
                await bookService.BookUserArchiveAsync(context, book1Id);
                // force a re-pull of the booklist
                await bookService.BookListRowsFilterAndSort(context);
                // create a concatenated string of all titles in the book liSt
                StringBuilder titles = new StringBuilder();
                foreach (var b in bookService.BookListRows)
                {
                    titles.Append(b.Title);
                }
                // assert
                Assert.Equal(titles.ToString(), book2Title);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }
        [Fact]
        public async Task FilteringTheBookListByTitleIsCaseSensitive()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
#if DEBUG
            // this is only used for the test bench to provide a logged in user outside of the standard app flow
            bookService.SetLoggedInUser(user);
#endif
            bookService.IsBrowse = false;
            await bookService.BookListRowsFilterAndSort(context);
            int book1Id = 7;    // Laura, la mujer invisible
            int book2Id = 6;    // Tierras desconocidas
            int book3Id = 14;   // LA VIDA EN NUESTRO PLANETA CAP�TULO 8

            int userId = (int)user.Id;
            int bookUser1Id = await bookService.BookUserCreateAndSaveAsync(context, book1Id, userId);
            int bookUser2Id = await bookService.BookUserCreateAndSaveAsync(context, book2Id, userId);
            int bookUser3Id = await bookService.BookUserCreateAndSaveAsync(context, book3Id, userId);
            // reset the book list
            await bookService.BookListRowsFilterAndSort(context);
            // now concat the title of just books 1 and 3, because those are the books we expect to survive the filter
            var expectedTitleConcat = string.Join("|", (
                                    bookService.BookListRows
                                        .Where(x => x.BookId == 7 || x.BookId == 14)
                                        .OrderBy(x => x.BookId)
                                        .Select(x => x.Title)
                                        .ToArray()));

            try
            {
                // act
                bookService.TitleFilter = "La"; // casing is intentional as it doesn't match either title's case
                // force a re-pull of the booklist
                await bookService.BookListRowsFilterAndSort(context);
                // create a concatenated string of all titles in the book liSt
                var resultingTitleConcat = string.Join("|", (
                                    bookService.BookListRows
                                        .OrderBy(x => x.BookId)
                                        .Select(x => x.Title)
                                        .ToArray()));
                // assert
                Assert.Equal(expectedTitleConcat, resultingTitleConcat);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }
        [Fact]
        public async Task FilteringTheBookListByTagIsCaseSensitive()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
#if DEBUG
            // this is only used for the test bench to provide a logged in user outside of the standard app flow
            bookService.SetLoggedInUser(user);
#endif
            bookService.IsBrowse = false;
            await bookService.BookListRowsFilterAndSort(context);
            int book1Id = 7;
            int book2Id = 6;
            int book3Id = 8;

            int userId = (int)user.Id;
            int bookUser1Id = await bookService.BookUserCreateAndSaveAsync(context, book1Id, userId);
            int bookUser2Id = await bookService.BookUserCreateAndSaveAsync(context, book2Id, userId);
            int bookUser3Id = await bookService.BookUserCreateAndSaveAsync(context, book3Id, userId);
            // reset the book list
            await bookService.BookListRowsFilterAndSort(context);
            // now concat the title of just books 2 and 3, because those are the books we expect to survive the filter
            var expectedTitleConcat = string.Join("|", (
                                    bookService.BookListRows
                                        .Where(x => x.BookId == 8 || x.BookId == 6)
                                        .OrderBy(x => x.BookId)
                                        .Select(x => x.Title)
                                        .ToArray()));

            try
            {
                // act
                bookService.TagsFilter = "SPAIN"; // casing is intentional
                // force a re-pull of the booklist
                await bookService.BookListRowsFilterAndSort(context);
                // create a concatenated string of all titles in the book liSt
                var resultingTitleConcat = string.Join("|", (
                                    bookService.BookListRows
                                        .OrderBy(x => x.BookId)
                                        .Select(x => x.Title)
                                        .ToArray()));
                // assert
                Assert.Equal(expectedTitleConcat, resultingTitleConcat);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }
        [Fact]
        public async Task SortByTitleDescendingWorks()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
#if DEBUG
            // this is only used for the test bench to provide a logged in user outside of the standard app flow
            bookService.SetLoggedInUser(user);
#endif
            bookService.IsBrowse = false;
            await bookService.BookListRowsFilterAndSort(context);
            int book1Id = 7;    // Laura, la mujer invisible
            int book2Id = 6;    // Tierras desconocidas
            int book3Id = 14;   // LA VIDA EN NUESTRO PLANETA CAP�TULO 8

            int userId = (int)user.Id;
            int bookUser1Id = await bookService.BookUserCreateAndSaveAsync(context, book1Id, userId);
            int bookUser2Id = await bookService.BookUserCreateAndSaveAsync(context, book2Id, userId);
            int bookUser3Id = await bookService.BookUserCreateAndSaveAsync(context, book3Id, userId);
            // reset the book list
            await bookService.BookListRowsFilterAndSort(context);
            // now concat the titles of all books, in descending order
            var expectedTitleConcat = string.Join("|", (
                                    bookService.BookListRows
                                        .OrderByDescending(x => x.Title)
                                        .Select(x => x.Title)
                                        .ToArray()));

            try
            {
                // act
                bookService.OrderBy = 4;    // title
                bookService.SortAscending = false;
                // force a re-pull of the booklist
                await bookService.BookListRowsFilterAndSort(context);
                // create a concatenated string of all titles in the book liSt
                var resultingTitleConcat = string.Join("|", (
                                    bookService.BookListRows
                                        .Select(x => x.Title)
                                        .ToArray()));
                // assert
                Assert.Equal(expectedTitleConcat, resultingTitleConcat);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }
        [Fact]
        public async Task SkippingForwardReturnsRows11Through20()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
#if DEBUG
            // this is only used for the test bench to provide a logged in user outside of the standard app flow
            bookService.SetLoggedInUser(user);
#endif
            bookService.IsBrowse = true; // set to true so you don't have to assign all the book users
            await bookService.BookListRowsFilterAndSort(context);
            string expectedRowNumsConcat = "11|12|13|14|15|16|17|18|19|20";



            try
            {
                // act
                await bookService.BookListRowsNext(context);
                var resultingRowNumsConcat = string.Join("|", (
                                    bookService.BookListRows
                                        .OrderBy(x => x.RowNumber)
                                        .Select(x => x.RowNumber.ToString())
                                        .ToArray()));
                // assert
                Assert.Equal(expectedRowNumsConcat, resultingRowNumsConcat);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }
        [Fact]
        public async Task SkippingBackwardFromRow21ReturnsRows11Through20()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
#if DEBUG
            // this is only used for the test bench to provide a logged in user outside of the standard app flow
            bookService.SetLoggedInUser(user);
#endif
            bookService.IsBrowse = true; // set to true so you don't have to assign all the book users
            await bookService.BookListRowsFilterAndSort(context);
            string expectedRowNumsConcat = "11|12|13|14|15|16|17|18|19|20";
            await bookService.BookListRowsNext(context);
            await bookService.BookListRowsNext(context); // this should put us at 21 - 30

            try
            {
                // act
                await bookService.BookListRowsPrevious(context);
                var resultingRowNumsConcat = string.Join("|", (
                                    bookService.BookListRows
                                        .OrderBy(x => x.RowNumber)
                                        .Select(x => x.RowNumber.ToString())
                                        .ToArray()));
                // assert
                Assert.Equal(expectedRowNumsConcat, resultingRowNumsConcat);
            }
            finally
            {
                // clean-up


                await transaction.RollbackAsync();
            }
        }
        [Fact]
        public async Task AddingATagToABookWillIncrementItsCount()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user1 = CommonFunctions.CreateNewTestUser(userService, context);
            var user2 = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();

            
            int userId1 = (int)user1.Id;
            int userId2 = (int)user2.Id;
            
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

                await transaction.RollbackAsync();
            }
        }
        [Fact]
        public async Task RemovingATagFromABookWillDecrementItsCount()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user1 = CommonFunctions.CreateNewTestUser(userService, context);
            var user2 = CommonFunctions.CreateNewTestUser(userService, context);
            var user3 = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
            int userId1 = (int)user1.Id;
            int userId2 = (int)user2.Id; 
            int userId3 = (int)user3.Id;
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

                await transaction.RollbackAsync();
            }
        }


        #endregion

        #region Read

        [Fact]
        public async Task ReadingBook7Page1ShowsCorrectPageWordsAndSentences()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
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

                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task UpdatingWordStatusWorks()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
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
                    context, (int)wordUser.Id, (AvailableWordUserStatus)wordUser.Status, wordUser.Translation);

                var wordUserFromDb = context.WordUsers.FirstOrDefault(x => x.Id == wordUser.Id);

                // assert
                Assert.Equal(wordUserFromDb.Status.ToString(), AvailableWordUserStatus.LEARNED.ToString());
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task UpdatingWordTranslationWorks()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
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
                    context, (int)wordUser.Id, (AvailableWordUserStatus)wordUser.Status, wordUser.Translation);

                var wordUserFromDb = context.WordUsers.FirstOrDefault(x => x.Id == wordUser.Id);

                // assert
                Assert.Equal(wordUserFromDb.Translation, newTranslation);
            }
            finally
            {
                // clean-up
                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task UpdatingWordStatusIsReflectedOnOtherPages()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
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
                    context, (int)wordUser1.Id, (AvailableWordUserStatus)wordUser1.Status, wordUser1.Translation);

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

                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task ParagraphTranslationWorks()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
            // create the book
            int bookId = await bookService.BookCreateAndSaveAsync(context, 
                _newBookTitle, _newBookLanguageCode, _newBookUrl, _newBookText);
            // add the book stats
            bookService.BookStatsCreateAndSave(context, bookId);
            // now create the book user for teh logged in user
            int userId = (int)user.Id;
            int bookUserId = await bookService.BookUserCreateAndSaveAsync(context, bookId, userId);
            string fromCode = "ES";
            string toCode = "EN-US";
            
            try
            {
                // act

                if (bookService.IsDataInitRead == false)
                {
                    await bookService.InitDataRead(context, userService, bookId);
                }
                var pp = bookService.Paragraphs[0];
                var translation = await bookService.ParagraphTranslate(context, pp, fromCode, toCode);
                var expectedTranslation = "The selfish giant";
                var actualTranslation = pp.ParagraphTranslations[0].TranslationText;

                // assert
                Assert.Equal(expectedTranslation, actualTranslation);
            }
            finally
            {
                // clean-up

                await transaction.RollbackAsync();
            }
        }

        [Fact]
        public async Task MovingPagesUpdatesTheBookmarkAndReadDate()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            using var transaction = await context.Database.BeginTransactionAsync();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var bookService = CommonFunctions.CreateBookService();
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
                int bookmark = (int)bookUserFromDb.CurrentPageID;
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

                await transaction.RollbackAsync();
            }
        }

        #endregion
    }
}