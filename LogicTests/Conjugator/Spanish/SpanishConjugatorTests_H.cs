using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Conjugator.Spanish;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
// using Xunit.Sdk;
using Model.DAL;
using Microsoft.EntityFrameworkCore;
using LogicTests;

namespace Logic.Conjugator.Spanish.Tests
{
    /// <summary>
    /// this class was largely created via automation using the spreadsheet
    /// titled "verb conjugations" in my Google sheets
    /// </summary>
    [TestClass()]
    public class SpanishConjugatorTests_H
    {
        [TestMethod()]
        public void ConjugateHaberToHaveTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string conjugatorName = "Logic.Conjugator.Spanish.HaberHave";
            var spanishVerb = context.Verbs.Where(x => x.LanguageId == CommonFunctions.GetSpanishLanguageId(dbContextFactory) && x.Conjugator == conjugatorName && x.Infinitive == "haber").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("haber", conjugationTable.Infinitive);
            Assert.AreEqual("habiendo", conjugationTable.Gerund);
            Assert.AreEqual("habido", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("he", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("hube", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("había", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("habría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("habré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("has", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("hubiste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("habías", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("habrías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("habrás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("ha", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("hubo", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("había", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("habría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("habrá", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("hemos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("hubimos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("habíamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("habríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("habremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("habéis", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("hubisteis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("habíais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("habríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("habréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("han", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("hubieron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("habían", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("habrían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("habrán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("haya", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("hubiera", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("hubiere", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("hayas", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("hubieras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("hubieres", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("haya", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("hubiera", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("hubiere", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("hayamos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("hubiéramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("hubiéremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("hayáis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("hubierais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("hubiereis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("hayan", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("hubieran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("hubieren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("he", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no hayas", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("haya", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no haya", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("hayamos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no hayamos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("habed", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no hayáis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("hayan", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no hayan", conjugationTable.NegativeImperativeUstedes.ToString());
        }
        [TestMethod()]
        public void ConjugateHaberThereIsTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string conjugatorName = "Logic.Conjugator.Spanish.HaberThereIs";
            var spanishVerb = context.Verbs.Where(x => x.LanguageId == CommonFunctions.GetSpanishLanguageId(dbContextFactory) && x.Conjugator == conjugatorName && x.Infinitive == "haber").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("haber", conjugationTable.Infinitive);
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("hay", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("hubo", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("había", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("habría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("habrá", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("haya", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("hubiera", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("hubiere", conjugationTable.SubjunctiveFutureEl.ToString());

        }
        [TestMethod()]
        public void ConjugateHablarTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string conjugatorName = "Logic.Conjugator.Spanish._Ar";
            var spanishVerb = context.Verbs.Where(x => x.LanguageId == CommonFunctions.GetSpanishLanguageId(dbContextFactory) && x.Conjugator == conjugatorName && x.Infinitive == "hablar").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("hablar", conjugationTable.Infinitive);
            Assert.AreEqual("hablando", conjugationTable.Gerund);
            Assert.AreEqual("hablado", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("hablo", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("hablé", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("hablaba", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("hablaría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("hablaré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("hablas", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("hablaste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("hablabas", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("hablarías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("hablarás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("habla", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("habló", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("hablaba", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("hablaría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("hablará", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("hablamos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("hablamos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("hablábamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("hablaríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("hablaremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("habláis", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("hablasteis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("hablabais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("hablaríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("hablaréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("hablan", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("hablaron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("hablaban", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("hablarían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("hablarán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("hable", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("hablara", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("hablare", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("hables", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("hablaras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("hablares", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("hable", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("hablara", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("hablare", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("hablemos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("habláramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("habláremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("habléis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("hablarais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("hablareis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("hablen", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("hablaran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("hablaren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("habla", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no hables", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("hable", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no hable", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("hablemos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no hablemos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("hablad", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no habléis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("hablen", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no hablen", conjugationTable.NegativeImperativeUstedes.ToString());
        }
        [TestMethod()]
        public void ConjugateHacerTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string conjugatorName = "Logic.Conjugator.Spanish.Hacer";
            var spanishVerb = context.Verbs.Where(x => x.LanguageId == CommonFunctions.GetSpanishLanguageId(dbContextFactory) && x.Conjugator == conjugatorName && x.Infinitive == "hacer").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("hacer", conjugationTable.Infinitive);
            Assert.AreEqual("haciendo", conjugationTable.Gerund);
            Assert.AreEqual("hecho", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("hago", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("hice", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("hacía", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("haría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("haré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("haces", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("hiciste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("hacías", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("harías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("harás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("hace", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("hizo", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("hacía", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("haría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("hará", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("hacemos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("hicimos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("hacíamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("haríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("haremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("hacéis", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("hicisteis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("hacíais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("haríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("haréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("hacen", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("hicieron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("hacían", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("harían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("harán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("haga", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("hiciera", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("hiciere", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("hagas", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("hicieras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("hicieres", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("haga", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("hiciera", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("hiciere", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("hagamos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("hiciéramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("hiciéremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("hagáis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("hicierais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("hiciereis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("hagan", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("hicieran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("hicieren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("haz", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no hagas", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("haga", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no haga", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("hagamos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no hagamos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("haced", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no hagáis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("hagan", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no hagan", conjugationTable.NegativeImperativeUstedes.ToString());
        }


    }
}
