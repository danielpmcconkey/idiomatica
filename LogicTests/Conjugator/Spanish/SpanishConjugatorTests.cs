using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Conjugator.Spanish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Logic.Conjugator.Spanish.Tests
{
    [TestClass()]
    public class SpanishConjugatorTests
    {
        [TestMethod()]
        public void ConjugatePreferirTest()
        {
            Verb spanishVerb = new() { Infinitive = "preferir", Core1 = "prefer", Core2 = "prefier", Core3 = "prefir", Gerund = "prefiriendo", PastParticiple = "preferido", };
            Logic.Conjugator.Spanish.Preferir conjugator = new(null, spanishVerb, null);
            var conjugations = conjugator.Conjugate();
            Assert.Fail();
        }
    }
}