using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    
    public abstract class SpanishConjugator : Conjugator
    {
        public SpanishConjugator(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base (targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }
        public override List<VerbConjugation> Conjugate()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.AddRange(ConjugatePresent());
            conjugations.AddRange(ConjugatePreterite());
            conjugations.AddRange(ConjugateImperfect());
            conjugations.AddRange(ConjugateConditional());
            conjugations.AddRange(ConjugateFuture());
            conjugations.AddRange(ConjugateSubjunctivePresent());
            conjugations.AddRange(ConjugateSubjunctiveImperfect());
            conjugations.AddRange(ConjugateSubjunctiveFuture());
            conjugations.AddRange(ConjugateAffirmativeImperative());
            conjugations.AddRange(ConjugateNegativeImperative());
            return conjugations;

        }

        #region present
        public List<VerbConjugation> ConjugatePresent()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugatePresentYo());
            conjugations.Add(ConjugatePresentTu());
            conjugations.Add(ConjugatePresentUsted());
            conjugations.Add(ConjugatePresentEl());
            conjugations.Add(ConjugatePresentElla());
            conjugations.Add(ConjugatePresentNosotros());
            conjugations.Add(ConjugatePresentVosotros());
            conjugations.Add(ConjugatePresentUstedes());
            conjugations.Add(ConjugatePresentEllos());
            conjugations.Add(ConjugatePresentEllas());
            return conjugations;
        }
        public abstract VerbConjugation ConjugatePresentYo();
        public abstract VerbConjugation ConjugatePresentTu();
        public abstract VerbConjugation ConjugatePresentUsted();
        public abstract VerbConjugation ConjugatePresentEl();
        public abstract VerbConjugation ConjugatePresentElla();
        public abstract VerbConjugation ConjugatePresentNosotros();
        public abstract VerbConjugation ConjugatePresentVosotros();
        public abstract VerbConjugation ConjugatePresentUstedes();
        public abstract VerbConjugation ConjugatePresentEllos();
        public abstract VerbConjugation ConjugatePresentEllas();
        internal VerbConjugation GetBasePresentYoConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };

            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentElConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentEllaConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentEllosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentEllasConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        #region preterite
        public List<VerbConjugation> ConjugatePreterite()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugatePreteriteYo());
            conjugations.Add(ConjugatePreteriteTu());
            conjugations.Add(ConjugatePreteriteUsted());
            conjugations.Add(ConjugatePreteriteEl());
            conjugations.Add(ConjugatePreteriteElla());
            conjugations.Add(ConjugatePreteriteNosotros());
            conjugations.Add(ConjugatePreteriteVosotros());
            conjugations.Add(ConjugatePreteriteUstedes());
            conjugations.Add(ConjugatePreteriteEllos());
            conjugations.Add(ConjugatePreteriteEllas());
            return conjugations;
        }
        public abstract VerbConjugation ConjugatePreteriteYo();
        public abstract VerbConjugation ConjugatePreteriteTu();
        public abstract VerbConjugation ConjugatePreteriteUsted();
        public abstract VerbConjugation ConjugatePreteriteEl();
        public abstract VerbConjugation ConjugatePreteriteElla();
        public abstract VerbConjugation ConjugatePreteriteNosotros();
        public abstract VerbConjugation ConjugatePreteriteVosotros();
        public abstract VerbConjugation ConjugatePreteriteUstedes();
        public abstract VerbConjugation ConjugatePreteriteEllos();
        public abstract VerbConjugation ConjugatePreteriteEllas();
        internal VerbConjugation GetBasePreteriteYoConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };

            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteElConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteEllaConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteEllosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteEllasConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        #region imperfect
        public List<VerbConjugation> ConjugateImperfect()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateImperfectYo());
            conjugations.Add(ConjugateImperfectTu());
            conjugations.Add(ConjugateImperfectUsted());
            conjugations.Add(ConjugateImperfectEl());
            conjugations.Add(ConjugateImperfectElla());
            conjugations.Add(ConjugateImperfectNosotros());
            conjugations.Add(ConjugateImperfectVosotros());
            conjugations.Add(ConjugateImperfectUstedes());
            conjugations.Add(ConjugateImperfectEllos());
            conjugations.Add(ConjugateImperfectEllas());
            return conjugations;
        }
        public abstract VerbConjugation ConjugateImperfectYo();
        public abstract VerbConjugation ConjugateImperfectTu();
        public abstract VerbConjugation ConjugateImperfectUsted();
        public abstract VerbConjugation ConjugateImperfectEl();
        public abstract VerbConjugation ConjugateImperfectElla();
        public abstract VerbConjugation ConjugateImperfectNosotros();
        public abstract VerbConjugation ConjugateImperfectVosotros();
        public abstract VerbConjugation ConjugateImperfectUstedes();
        public abstract VerbConjugation ConjugateImperfectEllos();
        public abstract VerbConjugation ConjugateImperfectEllas();
        internal VerbConjugation GetBaseImperfectYoConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };

            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectElConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectEllaConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectEllosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectEllasConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        #region conditional
        public List<VerbConjugation> ConjugateConditional()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateConditionalYo());
            conjugations.Add(ConjugateConditionalTu());
            conjugations.Add(ConjugateConditionalUsted());
            conjugations.Add(ConjugateConditionalEl());
            conjugations.Add(ConjugateConditionalElla());
            conjugations.Add(ConjugateConditionalNosotros());
            conjugations.Add(ConjugateConditionalVosotros());
            conjugations.Add(ConjugateConditionalUstedes());
            conjugations.Add(ConjugateConditionalEllos());
            conjugations.Add(ConjugateConditionalEllas());
            return conjugations;
        }
        public abstract VerbConjugation ConjugateConditionalYo();
        public abstract VerbConjugation ConjugateConditionalTu();
        public abstract VerbConjugation ConjugateConditionalUsted();
        public abstract VerbConjugation ConjugateConditionalEl();
        public abstract VerbConjugation ConjugateConditionalElla();
        public abstract VerbConjugation ConjugateConditionalNosotros();
        public abstract VerbConjugation ConjugateConditionalVosotros();
        public abstract VerbConjugation ConjugateConditionalUstedes();
        public abstract VerbConjugation ConjugateConditionalEllos();
        public abstract VerbConjugation ConjugateConditionalEllas();
        internal VerbConjugation GetBaseConditionalYoConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };

            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalElConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalEllaConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalEllosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalEllasConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        #region future
        public List<VerbConjugation> ConjugateFuture()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateFutureYo());
            conjugations.Add(ConjugateFutureTu());
            conjugations.Add(ConjugateFutureUsted());
            conjugations.Add(ConjugateFutureEl());
            conjugations.Add(ConjugateFutureElla());
            conjugations.Add(ConjugateFutureNosotros());
            conjugations.Add(ConjugateFutureVosotros());
            conjugations.Add(ConjugateFutureUstedes());
            conjugations.Add(ConjugateFutureEllos());
            conjugations.Add(ConjugateFutureEllas());
            return conjugations;
        }
        public abstract VerbConjugation ConjugateFutureYo();
        public abstract VerbConjugation ConjugateFutureTu();
        public abstract VerbConjugation ConjugateFutureUsted();
        public abstract VerbConjugation ConjugateFutureEl();
        public abstract VerbConjugation ConjugateFutureElla();
        public abstract VerbConjugation ConjugateFutureNosotros();
        public abstract VerbConjugation ConjugateFutureVosotros();
        public abstract VerbConjugation ConjugateFutureUstedes();
        public abstract VerbConjugation ConjugateFutureEllos();
        public abstract VerbConjugation ConjugateFutureEllas();
        internal VerbConjugation GetBaseFutureYoConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };

            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureElConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureEllaConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureEllosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureEllasConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        #region SubjunctivePresent
        public List<VerbConjugation> ConjugateSubjunctivePresent()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateSubjunctivePresentYo());
            conjugations.Add(ConjugateSubjunctivePresentTu());
            conjugations.Add(ConjugateSubjunctivePresentUsted());
            conjugations.Add(ConjugateSubjunctivePresentEl());
            conjugations.Add(ConjugateSubjunctivePresentElla());
            conjugations.Add(ConjugateSubjunctivePresentNosotros());
            conjugations.Add(ConjugateSubjunctivePresentVosotros());
            conjugations.Add(ConjugateSubjunctivePresentUstedes());
            conjugations.Add(ConjugateSubjunctivePresentEllos());
            conjugations.Add(ConjugateSubjunctivePresentEllas());
            return conjugations;
        }
        public abstract VerbConjugation ConjugateSubjunctivePresentYo();
        public abstract VerbConjugation ConjugateSubjunctivePresentTu();
        public abstract VerbConjugation ConjugateSubjunctivePresentUsted();
        public abstract VerbConjugation ConjugateSubjunctivePresentEl();
        public abstract VerbConjugation ConjugateSubjunctivePresentElla();
        public abstract VerbConjugation ConjugateSubjunctivePresentNosotros();
        public abstract VerbConjugation ConjugateSubjunctivePresentVosotros();
        public abstract VerbConjugation ConjugateSubjunctivePresentUstedes();
        public abstract VerbConjugation ConjugateSubjunctivePresentEllos();
        public abstract VerbConjugation ConjugateSubjunctivePresentEllas();
        internal VerbConjugation GetBaseSubjunctivePresentYoConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };

            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentElConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentEllaConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentEllosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentEllasConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        #region SubjunctiveImperfect
        public List<VerbConjugation> ConjugateSubjunctiveImperfect()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateSubjunctiveImperfectYo());
            conjugations.Add(ConjugateSubjunctiveImperfectTu());
            conjugations.Add(ConjugateSubjunctiveImperfectUsted());
            conjugations.Add(ConjugateSubjunctiveImperfectEl());
            conjugations.Add(ConjugateSubjunctiveImperfectElla());
            conjugations.Add(ConjugateSubjunctiveImperfectNosotros());
            conjugations.Add(ConjugateSubjunctiveImperfectVosotros());
            conjugations.Add(ConjugateSubjunctiveImperfectUstedes());
            conjugations.Add(ConjugateSubjunctiveImperfectEllos());
            conjugations.Add(ConjugateSubjunctiveImperfectEllas());
            return conjugations;
        }
        public abstract VerbConjugation ConjugateSubjunctiveImperfectYo();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectTu();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectUsted();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectEl();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectElla();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectNosotros();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectVosotros();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectUstedes();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectEllos();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectEllas();
        internal VerbConjugation GetBaseSubjunctiveImperfectYoConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };

            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectElConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectEllaConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectEllosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectEllasConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        #region SubjunctiveFuture
        public List<VerbConjugation> ConjugateSubjunctiveFuture()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateSubjunctiveFutureYo());
            conjugations.Add(ConjugateSubjunctiveFutureTu());
            conjugations.Add(ConjugateSubjunctiveFutureUsted());
            conjugations.Add(ConjugateSubjunctiveFutureEl());
            conjugations.Add(ConjugateSubjunctiveFutureElla());
            conjugations.Add(ConjugateSubjunctiveFutureNosotros());
            conjugations.Add(ConjugateSubjunctiveFutureVosotros());
            conjugations.Add(ConjugateSubjunctiveFutureUstedes());
            conjugations.Add(ConjugateSubjunctiveFutureEllos());
            conjugations.Add(ConjugateSubjunctiveFutureEllas());
            return conjugations;
        }
        public abstract VerbConjugation ConjugateSubjunctiveFutureYo();
        public abstract VerbConjugation ConjugateSubjunctiveFutureTu();
        public abstract VerbConjugation ConjugateSubjunctiveFutureUsted();
        public abstract VerbConjugation ConjugateSubjunctiveFutureEl();
        public abstract VerbConjugation ConjugateSubjunctiveFutureElla();
        public abstract VerbConjugation ConjugateSubjunctiveFutureNosotros();
        public abstract VerbConjugation ConjugateSubjunctiveFutureVosotros();
        public abstract VerbConjugation ConjugateSubjunctiveFutureUstedes();
        public abstract VerbConjugation ConjugateSubjunctiveFutureEllos();
        public abstract VerbConjugation ConjugateSubjunctiveFutureEllas();
        internal VerbConjugation GetBaseSubjunctiveFutureYoConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };

            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureElConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureEllaConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureEllosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureEllasConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        #region AffirmativeImperative
        public List<VerbConjugation> ConjugateAffirmativeImperative()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateAffirmativeImperativeTu());
            conjugations.Add(ConjugateAffirmativeImperativeUsted());
            conjugations.Add(ConjugateAffirmativeImperativeNosotros());
            conjugations.Add(ConjugateAffirmativeImperativeVosotros());
            conjugations.Add(ConjugateAffirmativeImperativeUstedes());
            return conjugations;
        }
        public abstract VerbConjugation ConjugateAffirmativeImperativeTu();
        public abstract VerbConjugation ConjugateAffirmativeImperativeUsted();
        public abstract VerbConjugation ConjugateAffirmativeImperativeNosotros();
        public abstract VerbConjugation ConjugateAffirmativeImperativeVosotros();
        public abstract VerbConjugation ConjugateAffirmativeImperativeUstedes();
        
        internal VerbConjugation GetBaseAffirmativeImperativeTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseAffirmativeImperativeUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseAffirmativeImperativeNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseAffirmativeImperativeVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseAffirmativeImperativeUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePiece());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        #region NegativeImperative
        public List<VerbConjugation> ConjugateNegativeImperative()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateNegativeImperativeTu());
            conjugations.Add(ConjugateNegativeImperativeUsted());
            conjugations.Add(ConjugateNegativeImperativeNosotros());
            conjugations.Add(ConjugateNegativeImperativeVosotros());
            conjugations.Add(ConjugateNegativeImperativeUstedes());
            return conjugations;
        }
        public abstract VerbConjugation ConjugateNegativeImperativeTu();
        public abstract VerbConjugation ConjugateNegativeImperativeUsted();
        public abstract VerbConjugation ConjugateNegativeImperativeNosotros();
        public abstract VerbConjugation ConjugateNegativeImperativeVosotros();
        public abstract VerbConjugation ConjugateNegativeImperativeUstedes();
        internal VerbConjugation GetBaseNegativeImperativeTuConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePieceNegativeImperative());
            conjugation.Pieces.Add(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseNegativeImperativeUstedConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePieceNegativeImperative());
            conjugation.Pieces.Add(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseNegativeImperativeNosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePieceNegativeImperative());
            conjugation.Pieces.Add(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseNegativeImperativeVosotrosConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePieceNegativeImperative());
            conjugation.Pieces.Add(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseNegativeImperativeUstedesConjugation()
        {
            if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE
            };
            conjugation.Pieces.Add(GetCorePieceNegativeImperative());
            conjugation.Pieces.Add(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

        internal VerbConjugationPiece GetCorePiece()
        {
            return new VerbConjugationPiece()
            {
                Ordinal = 1,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core
            };
        }
        internal VerbConjugationPiece GetCorePieceNegativeImperative()
        {
            return new VerbConjugationPiece()
            {
                Ordinal = 1,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = $"no {_sourceLanguageInfinitive.Core}" 
            };
        }
        internal VerbConjugationPiece GetPronounPiece(string pronoun)
        {
            return new VerbConjugationPiece()
            {
                Ordinal = 0,
                Type = AvailableVerbConjugationPieceType.PRONOUN,
                Piece = $"{pronoun} "
            };
        }
    }
    
}
