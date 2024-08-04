using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBench
{
    public class UnitTestsDeepLService
    {
        [Fact]
        public void DeepLTranslatesAllSentencesInAParagraph()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            //var userService = CommonFunctions.CreateUserService();
            //var user = CommonFunctions.CreateNewTestUser(userService);
            

            var input = "—Aunque pudiera, no lo haría. " +
                "Las cicatrices pueden ser útiles. " +
                "Yo tengo una en la rodilla izquierda que es un diagrama perfecto del metro de Londres. " +
                "Bueno, déjalo aquí, Hagrid, es mejor que terminemos con esto."
                ;
            var sourceCode = "ES";
            var targetCode = "EN-US";
            var expected = "-Even if I could, I wouldn't. Scars can be useful. I have one on my left knee that is a perfect diagram of the London Underground. Well, leave it here, Hagrid, we'd better get this over with.";
            try
            {
                // act
                var actual = DeepLService.Translate(input, sourceCode, targetCode);
                // assert
                Assert.Equal(expected, actual);
            }
            finally
            {
                // clean-up
                
            }
        }
    }
}
