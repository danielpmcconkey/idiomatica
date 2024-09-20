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
using Model.DAL;
using Microsoft.EntityFrameworkCore;

namespace Logic.Conjugator.Spanish.Tests
{
    /// <summary>
    /// this class was largely created via automation using the spreadsheet
    /// titled "verb conjugations" in my Google sheets
    /// </summary>
    [TestClass()]
    public class SpanishConjugatorTests_F
    {
        [TestMethod()]
        public void ConjugateFaltarTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string conjugatorName = "Logic.Conjugator.Spanish._Ar";
            var spanishVerb = context.Verbs.Where(x => x.LanguageId == CommonFunctions.GetSpanishLanguageId(dbContextFactory) && x.Conjugator == conjugatorName && x.Infinitive == "faltar").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("faltar", conjugationTable.Infinitive);
            Assert.AreEqual("faltando", conjugationTable.Gerund);
            Assert.AreEqual("faltado", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("falto", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("falté", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("faltaba", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("faltaría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("faltaré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("faltas", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("faltaste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("faltabas", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("faltarías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("faltarás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("falta", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("faltó", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("faltaba", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("faltaría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("faltará", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("faltamos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("faltamos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("faltábamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("faltaríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("faltaremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("faltáis", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("faltasteis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("faltabais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("faltaríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("faltaréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("faltan", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("faltaron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("faltaban", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("faltarían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("faltarán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("falte", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("faltara", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("faltare", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("faltes", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("faltaras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("faltares", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("falte", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("faltara", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("faltare", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("faltemos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("faltáramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("faltáremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("faltéis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("faltarais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("faltareis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("falten", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("faltaran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("faltaren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("falta", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no faltes", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("falte", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no falte", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("faltemos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no faltemos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("faltad", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no faltéis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("falten", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no falten", conjugationTable.NegativeImperativeUstedes.ToString());
        }
        [TestMethod()]
        public void ConjugateFuncionarTest()
        {
            var dbContextFactory = CommonFunctions.GetRequiredService<IDbContextFactory<IdiomaticaContext>>();
            var context = dbContextFactory.CreateDbContext();
            string conjugatorName = "Logic.Conjugator.Spanish._Ar";
            var spanishVerb = context.Verbs.Where(x => x.LanguageId == CommonFunctions.GetSpanishLanguageId(dbContextFactory) && x.Conjugator == conjugatorName && x.Infinitive == "funcionar").FirstOrDefault();
            Assert.IsNotNull(spanishVerb);
            var conjugator = Logic.Conjugator.Factory.Get(conjugatorName, null, spanishVerb, null);
            Assert.IsNotNull(conjugator);
            var conjugations = conjugator.Conjugate();
            Logic.Conjugator.Spanish.SpanishConjugationTable conjugationTable = new(spanishVerb, conjugations);
            Assert.AreEqual("funcionar", conjugationTable.Infinitive);
            Assert.AreEqual("funcionando", conjugationTable.Gerund);
            Assert.AreEqual("funcionado", conjugationTable.PastParticiple);
            Assert.IsNotNull(conjugationTable.PresentYo); Assert.AreEqual("funciono", conjugationTable.PresentYo.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteYo); Assert.AreEqual("funcioné", conjugationTable.PreteriteYo.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectYo); Assert.AreEqual("funcionaba", conjugationTable.ImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalYo); Assert.AreEqual("funcionaría", conjugationTable.ConditionalYo.ToString());
            Assert.IsNotNull(conjugationTable.FutureYo); Assert.AreEqual("funcionaré", conjugationTable.FutureYo.ToString());
            Assert.IsNotNull(conjugationTable.PresentTu); Assert.AreEqual("funcionas", conjugationTable.PresentTu.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteTu); Assert.AreEqual("funcionaste", conjugationTable.PreteriteTu.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectTu); Assert.AreEqual("funcionabas", conjugationTable.ImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalTu); Assert.AreEqual("funcionarías", conjugationTable.ConditionalTu.ToString());
            Assert.IsNotNull(conjugationTable.FutureTu); Assert.AreEqual("funcionarás", conjugationTable.FutureTu.ToString());
            Assert.IsNotNull(conjugationTable.PresentEl); Assert.AreEqual("funciona", conjugationTable.PresentEl.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEl); Assert.AreEqual("funcionó", conjugationTable.PreteriteEl.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEl); Assert.AreEqual("funcionaba", conjugationTable.ImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEl); Assert.AreEqual("funcionaría", conjugationTable.ConditionalEl.ToString());
            Assert.IsNotNull(conjugationTable.FutureEl); Assert.AreEqual("funcionará", conjugationTable.FutureEl.ToString());
            Assert.IsNotNull(conjugationTable.PresentNosotros); Assert.AreEqual("funcionamos", conjugationTable.PresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteNosotros); Assert.AreEqual("funcionamos", conjugationTable.PreteriteNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectNosotros); Assert.AreEqual("funcionábamos", conjugationTable.ImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalNosotros); Assert.AreEqual("funcionaríamos", conjugationTable.ConditionalNosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureNosotros); Assert.AreEqual("funcionaremos", conjugationTable.FutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentVosotros); Assert.AreEqual("funcionáis", conjugationTable.PresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteVosotros); Assert.AreEqual("funcionasteis", conjugationTable.PreteriteVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectVosotros); Assert.AreEqual("funcionabais", conjugationTable.ImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalVosotros); Assert.AreEqual("funcionaríais", conjugationTable.ConditionalVosotros.ToString());
            Assert.IsNotNull(conjugationTable.FutureVosotros); Assert.AreEqual("funcionaréis", conjugationTable.FutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.PresentEllos); Assert.AreEqual("funcionan", conjugationTable.PresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.PreteriteEllos); Assert.AreEqual("funcionaron", conjugationTable.PreteriteEllos.ToString());
            Assert.IsNotNull(conjugationTable.ImperfectEllos); Assert.AreEqual("funcionaban", conjugationTable.ImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.ConditionalEllos); Assert.AreEqual("funcionarían", conjugationTable.ConditionalEllos.ToString());
            Assert.IsNotNull(conjugationTable.FutureEllos); Assert.AreEqual("funcionarán", conjugationTable.FutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentYo); Assert.AreEqual("funcione", conjugationTable.SubjunctivePresentYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectYo); Assert.AreEqual("funcionara", conjugationTable.SubjunctiveImperfectYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureYo); Assert.AreEqual("funcionare", conjugationTable.SubjunctiveFutureYo.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentTu); Assert.AreEqual("funciones", conjugationTable.SubjunctivePresentTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectTu); Assert.AreEqual("funcionaras", conjugationTable.SubjunctiveImperfectTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureTu); Assert.AreEqual("funcionares", conjugationTable.SubjunctiveFutureTu.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEl); Assert.AreEqual("funcione", conjugationTable.SubjunctivePresentEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEl); Assert.AreEqual("funcionara", conjugationTable.SubjunctiveImperfectEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEl); Assert.AreEqual("funcionare", conjugationTable.SubjunctiveFutureEl.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentNosotros); Assert.AreEqual("funcionemos", conjugationTable.SubjunctivePresentNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectNosotros); Assert.AreEqual("funcionáramos", conjugationTable.SubjunctiveImperfectNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureNosotros); Assert.AreEqual("funcionáremos", conjugationTable.SubjunctiveFutureNosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentVosotros); Assert.AreEqual("funcionéis", conjugationTable.SubjunctivePresentVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectVosotros); Assert.AreEqual("funcionarais", conjugationTable.SubjunctiveImperfectVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureVosotros); Assert.AreEqual("funcionareis", conjugationTable.SubjunctiveFutureVosotros.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctivePresentEllos); Assert.AreEqual("funcionen", conjugationTable.SubjunctivePresentEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveImperfectEllos); Assert.AreEqual("funcionaran", conjugationTable.SubjunctiveImperfectEllos.ToString());
            Assert.IsNotNull(conjugationTable.SubjunctiveFutureEllos); Assert.AreEqual("funcionaren", conjugationTable.SubjunctiveFutureEllos.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeTu); Assert.AreEqual("funciona", conjugationTable.AffirmativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeTu); Assert.AreEqual("no funciones", conjugationTable.NegativeImperativeTu.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUsted); Assert.AreEqual("funcione", conjugationTable.AffirmativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUsted); Assert.AreEqual("no funcione", conjugationTable.NegativeImperativeUsted.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeNosotros); Assert.AreEqual("funcionemos", conjugationTable.AffirmativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeNosotros); Assert.AreEqual("no funcionemos", conjugationTable.NegativeImperativeNosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeVosotros); Assert.AreEqual("funcionad", conjugationTable.AffirmativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeVosotros); Assert.AreEqual("no funcionéis", conjugationTable.NegativeImperativeVosotros.ToString());
            Assert.IsNotNull(conjugationTable.AffirmativeImperativeUstedes); Assert.AreEqual("funcionen", conjugationTable.AffirmativeImperativeUstedes.ToString());
            Assert.IsNotNull(conjugationTable.NegativeImperativeUstedes); Assert.AreEqual("no funcionen", conjugationTable.NegativeImperativeUstedes.ToString());
        }

    }
}
