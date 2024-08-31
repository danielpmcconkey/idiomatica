using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    public class SpanishConjugationTable
    {
        public SpanishConjugationTable(Verb verb, List<VerbConjugation> conjugations)
        {
            _conjugations = conjugations;
            Infinitive = verb.Infinitive;
            Gerund = verb.Gerund;
            PastParticiple = verb.PastParticiple;
            ApplyPresentConjugations();
            ApplyPreteriteConjugations();
            ApplyImperfectConjugations();
            ApplyConditionalConjugations();
            ApplyFutureConjugations();
            ApplySubjunctivePresentConjugations();
            ApplySubjunctiveImperfectConjugations();
            ApplySubjunctiveFutureConjugations();
            ApplyAffirmativeImperativeConjugations();
            ApplyNegativeImperativeConjugations();
        }
        private List<VerbConjugation> _conjugations = [];

        public string? Infinitive { get; set; }
        public string? Gerund { get; set; }
        public string? PastParticiple { get; set; }

        public VerbConjugation? PresentYo { get; set; }
        public VerbConjugation? PresentTu { get; set; }
        public VerbConjugation? PresentEl { get; set; }
        public VerbConjugation? PresentElla { get; set; }
        public VerbConjugation? PresentUsted { get; set; }
        public VerbConjugation? PresentNosotros { get; set; }
        public VerbConjugation? PresentVosotros { get; set; }
        public VerbConjugation? PresentUstedes { get; set; }
        public VerbConjugation? PresentEllos { get; set; }
        public VerbConjugation? PresentEllas { get; set; }

        public VerbConjugation? PreteriteYo { get; set; }
        public VerbConjugation? PreteriteTu { get; set; }
        public VerbConjugation? PreteriteEl { get; set; }
        public VerbConjugation? PreteriteElla { get; set; }
        public VerbConjugation? PreteriteUsted { get; set; }
        public VerbConjugation? PreteriteNosotros { get; set; }
        public VerbConjugation? PreteriteVosotros { get; set; }
        public VerbConjugation? PreteriteUstedes { get; set; }
        public VerbConjugation? PreteriteEllos { get; set; }
        public VerbConjugation? PreteriteEllas { get; set; }

        public VerbConjugation? ImperfectYo { get; set; }
        public VerbConjugation? ImperfectTu { get; set; }
        public VerbConjugation? ImperfectEl { get; set; }
        public VerbConjugation? ImperfectElla { get; set; }
        public VerbConjugation? ImperfectUsted { get; set; }
        public VerbConjugation? ImperfectNosotros { get; set; }
        public VerbConjugation? ImperfectVosotros { get; set; }
        public VerbConjugation? ImperfectUstedes { get; set; }
        public VerbConjugation? ImperfectEllos { get; set; }
        public VerbConjugation? ImperfectEllas { get; set; }

        public VerbConjugation? ConditionalYo { get; set; }
        public VerbConjugation? ConditionalTu { get; set; }
        public VerbConjugation? ConditionalEl { get; set; }
        public VerbConjugation? ConditionalElla { get; set; }
        public VerbConjugation? ConditionalUsted { get; set; }
        public VerbConjugation? ConditionalNosotros { get; set; }
        public VerbConjugation? ConditionalVosotros { get; set; }
        public VerbConjugation? ConditionalUstedes { get; set; }
        public VerbConjugation? ConditionalEllos { get; set; }
        public VerbConjugation? ConditionalEllas { get; set; }

        public VerbConjugation? FutureYo { get; set; }
        public VerbConjugation? FutureTu { get; set; }
        public VerbConjugation? FutureEl { get; set; }
        public VerbConjugation? FutureElla { get; set; }
        public VerbConjugation? FutureUsted { get; set; }
        public VerbConjugation? FutureNosotros { get; set; }
        public VerbConjugation? FutureVosotros { get; set; }
        public VerbConjugation? FutureUstedes { get; set; }
        public VerbConjugation? FutureEllos { get; set; }
        public VerbConjugation? FutureEllas { get; set; }

        public VerbConjugation? SubjunctivePresentYo { get; set; }
        public VerbConjugation? SubjunctivePresentTu { get; set; }
        public VerbConjugation? SubjunctivePresentEl { get; set; }
        public VerbConjugation? SubjunctivePresentElla { get; set; }
        public VerbConjugation? SubjunctivePresentUsted { get; set; }
        public VerbConjugation? SubjunctivePresentNosotros { get; set; }
        public VerbConjugation? SubjunctivePresentVosotros { get; set; }
        public VerbConjugation? SubjunctivePresentUstedes { get; set; }
        public VerbConjugation? SubjunctivePresentEllos { get; set; }
        public VerbConjugation? SubjunctivePresentEllas { get; set; }

        public VerbConjugation? SubjunctiveImperfectYo { get; set; }
        public VerbConjugation? SubjunctiveImperfectTu { get; set; }
        public VerbConjugation? SubjunctiveImperfectEl { get; set; }
        public VerbConjugation? SubjunctiveImperfectElla { get; set; }
        public VerbConjugation? SubjunctiveImperfectUsted { get; set; }
        public VerbConjugation? SubjunctiveImperfectNosotros { get; set; }
        public VerbConjugation? SubjunctiveImperfectVosotros { get; set; }
        public VerbConjugation? SubjunctiveImperfectUstedes { get; set; }
        public VerbConjugation? SubjunctiveImperfectEllos { get; set; }
        public VerbConjugation? SubjunctiveImperfectEllas { get; set; }

        public VerbConjugation? SubjunctiveFutureYo { get; set; }
        public VerbConjugation? SubjunctiveFutureTu { get; set; }
        public VerbConjugation? SubjunctiveFutureEl { get; set; }
        public VerbConjugation? SubjunctiveFutureElla { get; set; }
        public VerbConjugation? SubjunctiveFutureUsted { get; set; }
        public VerbConjugation? SubjunctiveFutureNosotros { get; set; }
        public VerbConjugation? SubjunctiveFutureVosotros { get; set; }
        public VerbConjugation? SubjunctiveFutureUstedes { get; set; }
        public VerbConjugation? SubjunctiveFutureEllos { get; set; }
        public VerbConjugation? SubjunctiveFutureEllas { get; set; }

        public VerbConjugation? AffirmativeImperativeTu { get; set; }
        public VerbConjugation? AffirmativeImperativeUsted { get; set; }
        public VerbConjugation? AffirmativeImperativeNosotros { get; set; }
        public VerbConjugation? AffirmativeImperativeVosotros { get; set; }
        public VerbConjugation? AffirmativeImperativeUstedes { get; set; }

        public VerbConjugation? NegativeImperativeTu { get; set; }
        public VerbConjugation? NegativeImperativeUsted { get; set; }
        public VerbConjugation? NegativeImperativeNosotros { get; set; }
        public VerbConjugation? NegativeImperativeVosotros { get; set; }
        public VerbConjugation? NegativeImperativeUstedes { get; set; }


        private void ApplyPresentConjugations()
        {
            PresentYo = _conjugations.Where(x =>
                    x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                    x.Number == AvailableGrammaticalNumber.SINGULAR &&
                    x.Gender == AvailableGrammaticalGender.ANY &&
                    x.Tense == AvailableGrammaticalTense.PRESENT &&
                    x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                    x.Mood == AvailableGrammaticalMood.INDICATIVE
                ).FirstOrDefault();
            PresentTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PresentUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PresentEl = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PresentElla = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PresentNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PresentVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PresentUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PresentEllos = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PresentEllas = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
        }
        private void ApplyPreteriteConjugations()
        {
            PreteriteYo = _conjugations.Where(x =>
                    x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                    x.Number == AvailableGrammaticalNumber.SINGULAR &&
                    x.Gender == AvailableGrammaticalGender.ANY &&
                    x.Tense == AvailableGrammaticalTense.PAST &&
                    x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                    x.Mood == AvailableGrammaticalMood.INDICATIVE
                ).FirstOrDefault();
            PreteriteTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PreteriteUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PreteriteEl = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PreteriteElla = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PreteriteNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PreteriteVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PreteriteUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PreteriteEllos = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            PreteriteEllas = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
        }
        private void ApplyImperfectConjugations()
        {
            ImperfectYo = _conjugations.Where(x =>
                    x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                    x.Number == AvailableGrammaticalNumber.SINGULAR &&
                    x.Gender == AvailableGrammaticalGender.ANY &&
                    x.Tense == AvailableGrammaticalTense.PAST &&
                    x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                    x.Mood == AvailableGrammaticalMood.INDICATIVE
                ).FirstOrDefault();
            ImperfectTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            ImperfectUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            ImperfectEl = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            ImperfectElla = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            ImperfectNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            ImperfectVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            ImperfectUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            ImperfectEllos = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            ImperfectEllas = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
        }
        private void ApplyConditionalConjugations()
        {
            ConditionalYo = _conjugations.Where(x =>
                    x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                    x.Number == AvailableGrammaticalNumber.SINGULAR &&
                    x.Gender == AvailableGrammaticalGender.ANY &&
                    x.Tense == AvailableGrammaticalTense.PRESENT &&
                    x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                    x.Mood == AvailableGrammaticalMood.CONDITIONAL
                ).FirstOrDefault();
            ConditionalTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.CONDITIONAL
            ).FirstOrDefault();
            ConditionalUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.CONDITIONAL
            ).FirstOrDefault();
            ConditionalEl = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.CONDITIONAL
            ).FirstOrDefault();
            ConditionalElla = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.CONDITIONAL
            ).FirstOrDefault();
            ConditionalNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.CONDITIONAL
            ).FirstOrDefault();
            ConditionalVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.CONDITIONAL
            ).FirstOrDefault();
            ConditionalUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.CONDITIONAL
            ).FirstOrDefault();
            ConditionalEllos = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.CONDITIONAL
            ).FirstOrDefault();
            ConditionalEllas = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.CONDITIONAL
            ).FirstOrDefault();
        }
        private void ApplyFutureConjugations()
        {
            FutureYo = _conjugations.Where(x =>
                    x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                    x.Number == AvailableGrammaticalNumber.SINGULAR &&
                    x.Gender == AvailableGrammaticalGender.ANY &&
                    x.Tense == AvailableGrammaticalTense.FUTURE &&
                    x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                    x.Mood == AvailableGrammaticalMood.INDICATIVE
                ).FirstOrDefault();
            FutureTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            FutureUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            FutureEl = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            FutureElla = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            FutureNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            FutureVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            FutureUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            FutureEllos = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
            FutureEllas = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.INDICATIVE
            ).FirstOrDefault();
        }

        private void ApplySubjunctivePresentConjugations()
        {
            SubjunctivePresentYo = _conjugations.Where(x =>
                    x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                    x.Number == AvailableGrammaticalNumber.SINGULAR &&
                    x.Gender == AvailableGrammaticalGender.ANY &&
                    x.Tense == AvailableGrammaticalTense.PRESENT &&
                    x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                    x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
                ).FirstOrDefault();
            SubjunctivePresentTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctivePresentUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctivePresentEl = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctivePresentElla = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctivePresentNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctivePresentVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctivePresentUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctivePresentEllos = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctivePresentEllas = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
        }
        private void ApplySubjunctiveImperfectConjugations()
        {
            SubjunctiveImperfectYo = _conjugations.Where(x =>
                    x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                    x.Number == AvailableGrammaticalNumber.SINGULAR &&
                    x.Gender == AvailableGrammaticalGender.ANY &&
                    x.Tense == AvailableGrammaticalTense.PAST &&
                    x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                    x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
                ).FirstOrDefault();
            SubjunctiveImperfectTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveImperfectUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveImperfectEl = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveImperfectElla = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveImperfectNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveImperfectVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveImperfectUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveImperfectEllos = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveImperfectEllas = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.PAST &&
                x.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
        }
        private void ApplySubjunctiveFutureConjugations()
        {
            SubjunctiveFutureYo = _conjugations.Where(x =>
                    x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                    x.Number == AvailableGrammaticalNumber.SINGULAR &&
                    x.Gender == AvailableGrammaticalGender.ANY &&
                    x.Tense == AvailableGrammaticalTense.FUTURE &&
                    x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                    x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
                ).FirstOrDefault();
            SubjunctiveFutureTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveFutureUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveFutureEl = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveFutureElla = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveFutureNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveFutureVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveFutureUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveFutureEllos = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.MASCULINE &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
            SubjunctiveFutureEllas = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.FEMININE &&
                x.Tense == AvailableGrammaticalTense.FUTURE &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.SUBJUNCTIVE
            ).FirstOrDefault();
        }

        private void ApplyAffirmativeImperativeConjugations()
        {
            AffirmativeImperativeTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.IMPERATIVE
            ).FirstOrDefault();
            AffirmativeImperativeUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.IMPERATIVE
            ).FirstOrDefault();
            AffirmativeImperativeNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.IMPERATIVE
            ).FirstOrDefault();
            AffirmativeImperativeVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.IMPERATIVE
            ).FirstOrDefault();
            AffirmativeImperativeUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.IMPERATIVE
            ).FirstOrDefault();
        }
        private void ApplyNegativeImperativeConjugations()
        {
            NegativeImperativeTu = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.NEGATIVE_IMPERATIVE
            ).FirstOrDefault();
            NegativeImperativeUsted = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.SINGULAR &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.NEGATIVE_IMPERATIVE
            ).FirstOrDefault();
            NegativeImperativeNosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.NEGATIVE_IMPERATIVE
            ).FirstOrDefault();
            NegativeImperativeVosotros = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.NEGATIVE_IMPERATIVE
            ).FirstOrDefault();
            NegativeImperativeUstedes = _conjugations.Where(x =>
                x.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL &&
                x.Number == AvailableGrammaticalNumber.PLURAL &&
                x.Gender == AvailableGrammaticalGender.ANY &&
                x.Tense == AvailableGrammaticalTense.PRESENT &&
                x.Aspect == AvailableGrammaticalAspect.PERFECT &&
                x.Mood == AvailableGrammaticalMood.NEGATIVE_IMPERATIVE
            ).FirstOrDefault();
        }
    }
}
