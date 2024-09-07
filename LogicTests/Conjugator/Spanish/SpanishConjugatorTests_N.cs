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
    public class SpanishConjugatorTests_N
    {
        [TestMethod()]
        public void ConjugateNecesitarTest()
        {
            var context = CommonFunctions.CreateContext();
            string conjugatorName = "Logic.Conjugator.Spanish._Ar";
            var spanishVerb = context.Verbs.Where(x => x.LanguageKey == CommonFunctions.GetSpanishLanguageKey(context) && x.Conjugator == conjugatorName && x.Infinitive == "necesitar").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("necesitar", conjugationTable.Infinitive);
            Assert.AreEqual("necesitando", conjugationTable.Gerund);
            Assert.AreEqual("necesitado", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("necesito", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("necesité", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("necesitaba", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("necesitaría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("necesitaré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("necesitas", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("necesitaste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("necesitabas", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("necesitarías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("necesitarás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("necesita", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("necesitó", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("necesitaba", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("necesitaría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("necesitará", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("necesitamos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("necesitamos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("necesitábamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("necesitaríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("necesitaremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("necesitáis", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("necesitasteis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("necesitabais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("necesitaríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("necesitaréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("necesitan", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("necesitaron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("necesitaban", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("necesitarían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("necesitarán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("necesite", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("necesitara", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("necesitare", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("necesites", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("necesitaras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("necesitares", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("necesite", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("necesitara", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("necesitare", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("necesitemos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("necesitáramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("necesitáremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("necesitéis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("necesitarais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("necesitareis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("necesiten", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("necesitaran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("necesitaren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("necesita", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no necesites", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("necesite", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no necesite", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("necesitemos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no necesitemos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("necesitad", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no necesitéis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("necesiten", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no necesiten", conjugationTable.NegativeImperativeUstedes.ToString());
        }
        [TestMethod()]
        public void ConjugateNotarTest()
        {
            var context = CommonFunctions.CreateContext();
            string conjugatorName = "Logic.Conjugator.Spanish._Ar";
            var spanishVerb = context.Verbs.Where(x => x.LanguageKey == CommonFunctions.GetSpanishLanguageKey(context) && x.Conjugator == conjugatorName && x.Infinitive == "notar").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("notar", conjugationTable.Infinitive);
            Assert.AreEqual("notando", conjugationTable.Gerund);
            Assert.AreEqual("notado", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("noto", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("noté", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("notaba", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("notaría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("notaré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("notas", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("notaste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("notabas", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("notarías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("notarás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("nota", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("notó", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("notaba", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("notaría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("notará", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("notamos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("notamos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("notábamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("notaríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("notaremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("notáis", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("notasteis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("notabais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("notaríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("notaréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("notan", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("notaron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("notaban", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("notarían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("notarán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("note", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("notara", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("notare", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("notes", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("notaras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("notares", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("note", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("notara", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("notare", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("notemos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("notáramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("notáremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("notéis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("notarais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("notareis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("noten", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("notaran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("notaren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("nota", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no notes", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("note", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no note", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("notemos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no notemos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("notad", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no notéis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("noten", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no noten", conjugationTable.NegativeImperativeUstedes.ToString());
        }
    }
}
