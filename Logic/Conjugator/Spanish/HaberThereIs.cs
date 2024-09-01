using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{
    public class HaberThereIs : HaberHave
    {
        public HaberThereIs(
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
        {

        }
        #region Present
        public override List<VerbConjugation> ConjugatePresent()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugatePresentEl());
            return conjugations;
        }
        public override VerbConjugation GetStructurePresentEl()
        {
            // if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

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
            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = "there is / there are: present conjugation of haber (there is)";
            }
            return conjugation;
        }
        public override List<VerbConjugationPiece> GetRootPiecesPresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ha"
            }];
        }
        public override List<VerbConjugationPiece> GetStemPiecesPresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.IRREGULAR,
                Piece = "y"
            }];
        }
        #endregion

        #region Preterite
        public override List<VerbConjugation> ConjugatePreterite()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugatePreteriteEl());
            return conjugations;
        }
        public override VerbConjugation GetStructurePreteriteEl()
        {
            // if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

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
            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = "there was / there were: preterite conjugation of haber (there is)";
            }
            return conjugation;
        }
        #endregion

        #region Imperfect
        public override List<VerbConjugation> ConjugateImperfect()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateImperfectEl());
            return conjugations;
        }
        public override VerbConjugation GetStructureImperfectEl()
        {
            // if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

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

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = "there was / there were: imperfect conjugation of haber (there is)";
            }
            return conjugation;
        }
        #endregion

        #region Conditional
        public override List<VerbConjugation> ConjugateConditional()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateConditionalEl());
            return conjugations;
        }
        public override VerbConjugation GetStructureConditionalEl()
        {
            // if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

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

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = "there would be: conditional conjugation of haber (there is)";
            }
            return conjugation;
        }
        #endregion

        #region Future
        public override List<VerbConjugation> ConjugateFuture()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateFutureEl());
            return conjugations;
        }
        public override VerbConjugation GetStructureFutureEl()
        {
            // if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

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

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = "there was / there were: preterite conjugation of haber (there is)";
            }
            return conjugation;
        }
        #endregion

        #region SubjunctivePresent
        public override List<VerbConjugation> ConjugateSubjunctivePresent()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateSubjunctivePresentEl());
            return conjugations;
        }
        public override VerbConjugation GetStructureSubjunctivePresentEl()
        {
            // if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

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
            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = "there were: subjunctive present conjugation of haber (there is)";
            }return conjugation;
        }
        #endregion

        #region SubjunctiveImperfect
        public override List<VerbConjugation> ConjugateSubjunctiveImperfect()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateSubjunctiveImperfectEl());
            return conjugations;
        }
        public override VerbConjugation GetStructureSubjunctiveImperfectEl()
        {
            // if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

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
            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = "there were: subjunctive imperfect conjugation of haber (there is)";
            }
            return conjugation;
        }
        #endregion

        #region SubjunctiveFuture
        public override List<VerbConjugation> ConjugateSubjunctiveFuture()
        {
            List<VerbConjugation> conjugations = [];
            conjugations.Add(ConjugateSubjunctiveFutureEl());
            return conjugations;
        }
        public override VerbConjugation GetStructureSubjunctiveFutureEl()
        {
            // if (_targetTranslator is null) { ErrorHandler.LogAndThrow(); return new(); }

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
            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = conjugation.Translation = "there will be: subjunctive future conjugation of haber (there is)"; ;
            }
            return conjugation;
        }
        #endregion

        public override List<VerbConjugation> ConjugateAffirmativeImperative()
        {
            return [];
        }
        public override List<VerbConjugation> ConjugateNegativeImperative()
        {
            return [];
        }
    }
}
