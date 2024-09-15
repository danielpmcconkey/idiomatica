using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Conjugator.Spanish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Xunit.Sdk;
using LogicTests;

namespace Logic.Conjugator.Spanish.Tests
{
    /// <summary>
    /// this class was largely created via automation using the spreadsheet
    /// titled "verb conjugations" in my Google sheets
    /// </summary>
    [TestClass()]
    public class SpanishConjugatorTests_U
    {
        [TestMethod()]
        public void ConjugateUsarTest()
        {
            var context = CommonFunctions.CreateContext();
            string conjugatorName = "Logic.Conjugator.Spanish._Ar";
            var spanishVerb = context.Verbs.Where(x => x.LanguageId == CommonFunctions.GetSpanishLanguageId(context) && x.Conjugator == conjugatorName && x.Infinitive == "usar").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("usar", conjugationTable.Infinitive);
            Assert.AreEqual("usando", conjugationTable.Gerund);
            Assert.AreEqual("usado", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("uso", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("usé", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("usaba", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("usaría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("usaré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("usas", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("usaste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("usabas", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("usarías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("usarás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("usa", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("usó", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("usaba", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("usaría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("usará", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("usamos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("usamos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("usábamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("usaríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("usaremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("usáis", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("usasteis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("usabais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("usaríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("usaréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("usan", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("usaron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("usaban", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("usarían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("usarán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("use", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("usara", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("usare", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("uses", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("usaras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("usares", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("use", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("usara", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("usare", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("usemos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("usáramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("usáremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("uséis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("usarais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("usareis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("usen", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("usaran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("usaren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("usa", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no uses", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("use", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no use", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("usemos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no usemos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("usad", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no uséis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("usen", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no usen", conjugationTable.NegativeImperativeUstedes.ToString());
        }
    }
}
