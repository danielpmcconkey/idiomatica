using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Logic.Conjugator
{
    public abstract class Conjugator
    {
        protected IVerbTranslator _targetTranslator;
        protected Verb _sourceLanguageInfinitive;
        protected Verb _targetLanguageInfinitive;
        
        public Conjugator(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive,Verb targetLanguageInfinitive)
        {
            _targetTranslator = targetTranslator;
            _sourceLanguageInfinitive = sourceLanguageInfinitive;
            _targetLanguageInfinitive= targetLanguageInfinitive;
        }
        public abstract List<VerbConjugation> Conjugate();
    }
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

        internal VerbConjugationPiece GetCorePiece()
        {
            return new VerbConjugationPiece()
            {
                Ordinal = 1,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core
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
    public class SpanishConjugatorArBase : SpanishConjugator
    {
        public SpanishConjugatorArBase(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }
       
        
        #region Present
        
        public override VerbConjugation ConjugatePresentYo()
        {
            var conjugation = GetBasePresentYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "o"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentTu()
        {
            var conjugation = GetBasePresentTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "as"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentUsted()
        {
            var conjugation = GetBasePresentUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentEl()
        {
            var conjugation = GetBasePresentElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentElla()
        {
            var conjugation = GetBasePresentEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentNosotros()
        {
            var conjugation = GetBasePresentNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentVosotros()
        {
            var conjugation = GetBasePresentVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "áis"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentUstedes()
        {
            var conjugation = GetBasePresentUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentEllos()
        {
            var conjugation = GetBasePresentEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePresentEllas()
        {
            var conjugation = GetBasePresentEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            });
            return conjugation;
        }
        #endregion


        #region Preterite
       
        public override VerbConjugation ConjugatePreteriteYo()
        {
            var conjugation = GetBasePreteriteYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "é"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteTu()
        {
            var conjugation = GetBasePreteriteTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aste"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteUsted()
        {
            var conjugation = GetBasePreteriteUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ó"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteEl()
        {
            var conjugation = GetBasePreteriteElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ó"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteElla()
        {
            var conjugation = GetBasePreteriteEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ó"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteNosotros()
        {
            var conjugation = GetBasePreteriteNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteVosotros()
        {
            var conjugation = GetBasePreteriteVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "asteis"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteUstedes()
        {
            var conjugation = GetBasePreteriteUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aron"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteEllos()
        {
            var conjugation = GetBasePreteriteEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aron"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugatePreteriteEllas()
        {
            var conjugation = GetBasePreteriteEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aron"
            });
            return conjugation;
        }
        #endregion


        #region Imperfect
        
        public override VerbConjugation ConjugateImperfectYo()
        {
            var conjugation = GetBaseImperfectYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aba"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectTu()
        {
            var conjugation = GetBaseImperfectTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "abas"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectUsted()
        {
            var conjugation = GetBaseImperfectUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aba"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectEl()
        {
            var conjugation = GetBaseImperfectElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aba"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectElla()
        {
            var conjugation = GetBaseImperfectEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aba"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectNosotros()
        {
            var conjugation = GetBaseImperfectNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "abamos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectVosotros()
        {
            var conjugation = GetBaseImperfectVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "abais"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectUstedes()
        {
            var conjugation = GetBaseImperfectUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aban"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectEllos()
        {
            var conjugation = GetBaseImperfectEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aban"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateImperfectEllas()
        {
            var conjugation = GetBaseImperfectEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aban"
            });
            return conjugation;
        }
        #endregion


        #region Conditional

        public override VerbConjugation ConjugateConditionalYo()
        {
            var conjugation = GetBaseConditionalYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aría"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalTu()
        {
            var conjugation = GetBaseConditionalTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arías"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalUsted()
        {
            var conjugation = GetBaseConditionalUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aría"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalEl()
        {
            var conjugation = GetBaseConditionalElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aría"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalElla()
        {
            var conjugation = GetBaseConditionalEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aría"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalNosotros()
        {
            var conjugation = GetBaseConditionalNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aríamos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalVosotros()
        {
            var conjugation = GetBaseConditionalVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aríais"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalUstedes()
        {
            var conjugation = GetBaseConditionalUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arían"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalEllos()
        {
            var conjugation = GetBaseConditionalEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arían"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateConditionalEllas()
        {
            var conjugation = GetBaseConditionalEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arían"
            });
            return conjugation;
        }
        #endregion


        #region Future

        public override VerbConjugation ConjugateFutureYo()
        {
            var conjugation = GetBaseFutureYoConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aré"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureTu()
        {
            var conjugation = GetBaseFutureTuConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arás"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureUsted()
        {
            var conjugation = GetBaseFutureUstedConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ará"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureEl()
        {
            var conjugation = GetBaseFutureElConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ará"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureElla()
        {
            var conjugation = GetBaseFutureEllaConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ará"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureNosotros()
        {
            var conjugation = GetBaseFutureNosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aremos"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureVosotros()
        {
            var conjugation = GetBaseFutureVosotrosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aréis"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureUstedes()
        {
            var conjugation = GetBaseFutureUstedesConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arán"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureEllos()
        {
            var conjugation = GetBaseFutureEllosConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arán"
            });
            return conjugation;
        }
        public override VerbConjugation ConjugateFutureEllas()
        {
            var conjugation = GetBaseFutureEllasConjugation();
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Ordinal = 2,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arán"
            });
            return conjugation;
        }
        #endregion



    }
}
