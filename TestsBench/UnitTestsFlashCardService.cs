﻿using Logic.Services;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsBench
{
    public class UnitTestsFlashCardService
    {
        [Fact]
        public async Task ICanHazCheezburger()
        {
            // arrange
            var cheezburger = "yummy";
            try
            {
                // act
                cheezburger += "had";
                // assert
                Assert.Equal("yummyhad", cheezburger);
            }
            finally
            {
                // clean-up
                cheezburger = "gone";
            }
        }
        [Fact]
        public async Task ParagraphGetFullTextPullsAllSentencesText()
        {
            // arrange
            var context = CommonFunctions.CreateContext();
            var userService = CommonFunctions.CreateUserService();
            var user = CommonFunctions.CreateNewTestUser(userService, context);
            var flashCardService = CommonFunctions.CreateFlashCardService();
            int ppId = 14721;
            var pp = await context.Paragraphs
                .Where(x => x.Id == ppId)
                .Include(s => s.Sentences)
                .FirstOrDefaultAsync();

            var expected = "—Aunque pudiera, no lo haría. " +
                "Las cicatrices pueden ser útiles. " +
                "Yo tengo una en la rodilla izquierda que es un diagrama perfecto del metro de Londres. " +
                "Bueno, déjalo aquí, Hagrid, es mejor que terminemos con esto."
                ;
            try
            {
                // act
                var actual = flashCardService.ParagraphGetFullText(pp);
                // assert
                Assert.Equal(expected, actual);
            }
            finally
            {
                // clean-up
                user.LanguageUsers = new List<LanguageUser>();
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
    }
        
}
