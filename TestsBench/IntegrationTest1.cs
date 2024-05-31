using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Model.DAL;
using System.Net;

namespace TestsBench.Tests
{
    public class IntegrationTest1
    {
        
        private IdiomaticaContext CreateContext() 
        {
            
            var connectionstring = "Server=localhost;Database=Idiomatica;Trusted_Connection=True;TrustServerCertificate=true;";
            var optionsBuilder = new DbContextOptionsBuilder<IdiomaticaContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            return new IdiomaticaContext(optionsBuilder.Options);
        }

        [Fact]
        public async Task ThereAreBooks()
        {
            // arrange
            var context = CreateContext();

            // act
            var book = context.Books.FirstOrDefault();
            // assert
            Assert.NotNull(book);
            Assert.True(book.Id > 0);
        }
    }
}