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
    public class SpanishConjugatorTests_O
    {
        [TestMethod()]
        public void ConjugateOcurrirTest()
        {
            var context = CommonFunctions.CreateContext();
            string conjugatorName = "Logic.Conjugator.Spanish._Ir";
            var spanishVerb = context.Verbs.Where(x => x.LanguageKey == CommonFunctions.GetSpanishLanguageKey(context) && x.Conjugator == conjugatorName && x.Infinitive == "ocurrir").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("ocurrir", conjugationTable.Infinitive);
            Assert.AreEqual("ocurriendo", conjugationTable.Gerund);
            Assert.AreEqual("ocurrido", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("ocurro", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("ocurrí", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("ocurría", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("ocurriría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("ocurriré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("ocurres", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("ocurriste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("ocurrías", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("ocurrirías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("ocurrirás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("ocurre", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("ocurrió", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("ocurría", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("ocurriría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("ocurrirá", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("ocurrimos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("ocurrimos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("ocurríamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("ocurriríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("ocurriremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("ocurrís", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("ocurristeis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("ocurríais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("ocurriríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("ocurriréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("ocurren", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("ocurrieron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("ocurrían", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("ocurrirían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("ocurrirán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("ocurra", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("ocurriera", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("ocurriere", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("ocurras", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("ocurrieras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("ocurrieres", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("ocurra", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("ocurriera", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("ocurriere", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("ocurramos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("ocurriéramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("ocurriéremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("ocurráis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("ocurrierais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("ocurriereis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("ocurran", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("ocurrieran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("ocurrieren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("ocurre", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no ocurras", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("ocurra", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no ocurra", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("ocurramos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no ocurramos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("ocurrid", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no ocurráis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("ocurran", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no ocurran", conjugationTable.NegativeImperativeUstedes.ToString());
        }
        [TestMethod()]
        public void ConjugateOírTest()
        {
            var context = CommonFunctions.CreateContext();
            string conjugatorName = "Logic.Conjugator.Spanish.Oir";
            var spanishVerb = context.Verbs.Where(x => x.LanguageKey == CommonFunctions.GetSpanishLanguageKey(context) && x.Conjugator == conjugatorName && x.Infinitive == "oír").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("oír", conjugationTable.Infinitive);
            Assert.AreEqual("oyendo", conjugationTable.Gerund);
            Assert.AreEqual("oído", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("oigo", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("oí", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("oía", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("oiría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("oiré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("oyes", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("oíste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("oías", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("oirías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("oirás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("oye", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("oyó", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("oía", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("oiría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("oirá", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("oímos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("oímos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("oíamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("oiríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("oiremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("oís", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("oísteis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("oíais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("oiríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("oiréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("oyen", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("oyeron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("oían", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("oirían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("oirán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("oiga", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("oyera", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("oyere", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("oigas", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("oyeras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("oyeres", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("oiga", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("oyera", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("oyere", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("oigamos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("oyéramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("oyéremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("oigáis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("oyerais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("oyereis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("oigan", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("oyeran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("oyeren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("oye", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no oigas", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("oiga", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no oiga", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("oigamos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no oigamos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("oíd", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no oigáis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("oigan", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no oigan", conjugationTable.NegativeImperativeUstedes.ToString());
        }
    }
}
