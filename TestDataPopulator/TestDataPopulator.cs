using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataPopulator
{
    internal class TestDataPopulator
    {
        private Guid? _spanishLanguageKey;
        private Guid? _englishLanguageKey;
        // creat 5 flash cards
        // they all should be for very commoN words
        // give them actual translations
        // make sure one of them is for the word "de"
        // translate pp 14590
        // need to create a book in english, use in WordApiTest.cs
        // make sure we create the word "vivían"
        internal IdiomaticaContext CreateContext()
        {
            var connectionstring = "Server=localhost;Database=Idiomatica_test;Trusted_Connection=True;TrustServerCertificate=true;";
            var optionsBuilder = new DbContextOptionsBuilder<IdiomaticaContext>();
            optionsBuilder.UseSqlServer(connectionstring);
            return new IdiomaticaContext(optionsBuilder.Options);
        }
        private void AddInitialWordsAndWordRanks()
        {
            if (_spanishLanguageKey is null) throw new InvalidDataException();
            if (_englishLanguageKey is null) throw new InvalidDataException();

            var context = CreateContext();
            foreach (var word in WordRank.words)
            {
                var wordKey = Guid.NewGuid();
                var languageKey = _spanishLanguageKey;
                if (word.language == "English") languageKey = _englishLanguageKey;
                context.Words.Add(new Word
                {
                    UniqueKey = wordKey,
                    LanguageKey = languageKey,
                    Text = word.word,
                    TextLowerCase = word.word,
                    Romanization = word.word,
                });
                context.WordRanks.Add(new Model.WordRank() {
                    UniqueKey = Guid.NewGuid(),
                    LanguageKey = languageKey,
                    WordKey = wordKey,DifficultyScore = word.difficulty,
                    Ordinal = word.ordinal
                });
            }
            context.SaveChanges();
        }

    }
    
}
