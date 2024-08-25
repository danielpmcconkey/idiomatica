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

        #region helper functions

        internal virtual List<VerbConjugationPiece> GetCorePiece(int whichCore = 1)
        {
            if (whichCore == 2) return GetCorePiece2();
            if (whichCore == 3) return GetCorePiece3();
            if (whichCore == 4) return GetCorePiece4();
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }];
        }
        internal virtual List<VerbConjugationPiece> GetCorePiece2()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core2
            }];
        }
        internal virtual List<VerbConjugationPiece> GetCorePiece3()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core3
            }];
        }
        internal virtual List<VerbConjugationPiece> GetCorePiece4()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core4
            }];
        }
        internal List<VerbConjugationPiece> GetCorePieceNegativeImperative(int whichCore = 1)
        {
            if (whichCore == 2) return GetCorePieceNegativeImperative2();
            if (whichCore == 3) return GetCorePieceNegativeImperative3();
            if (whichCore == 4) return GetCorePieceNegativeImperative4();

            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = $"no {_sourceLanguageInfinitive.Core1}"
            }];
        }
        internal List<VerbConjugationPiece> GetCorePieceNegativeImperative2()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = $"no {_sourceLanguageInfinitive.Core2}"
            }];
        }
        internal List<VerbConjugationPiece> GetCorePieceNegativeImperative3()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = $"no {_sourceLanguageInfinitive.Core3}"
            }];
        }
        internal List<VerbConjugationPiece> GetCorePieceNegativeImperative4()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = $"no {_sourceLanguageInfinitive.Core4}"
            }];
        }
        internal List<VerbConjugationPiece> GetPronounPiece(string pronoun)
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 0,
                Type = AvailableVerbConjugationPieceType.PRONOUN,
                Piece = $"{pronoun} "
            }];
        }
        internal List<VerbConjugationPiece> ReplaceMiddleOfCore(string? core, string seek)
        {
            if (string.IsNullOrEmpty(core))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            if (string.IsNullOrEmpty(seek))
            {
                ErrorHandler.LogAndThrow();
                return [];
            }
            int location = core.IndexOf(seek);
            if (location < 1)
            {
                // not found; throw an error because we probably have the wrong conjugator
                ErrorHandler.LogAndThrow();
                return [];
            }
            const int firstBitStart = 0; // always
            int firstBitLength = location; // vIEn -> location is 1 length is 1
            string firstBit = core.Substring(firstBitStart, firstBitLength);

            int endBitStart = firstBitStart + firstBitLength + 2;  // vIEn -> 0 + 1 + 2 = 3
            int endBitLength = core.Length - endBitStart; // vIEn -> 4 - 3 = 1
            string endBit = core.Substring(endBitStart, endBitLength);
            if (firstBit.Length > 0 && endBit.Length > 0)
            {
                // this is the way
                return [new VerbConjugationPiece()
                {
                    Ordinal = 100,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = firstBit
                },new VerbConjugationPiece()
                {
                    Ordinal = 110,
                    Type = AvailableVerbConjugationPieceType.IRREGULAR,
                    Piece = seek
                },new VerbConjugationPiece()
                {
                    Ordinal = 120,
                    Type = AvailableVerbConjugationPieceType.CORE,
                    Piece = endBit
                }];
            }
            else
            {
                // something screwy; throw an error because we probably have the wrong conjugator
                ErrorHandler.LogAndThrow();
                return [];
            }
        }
        #endregion

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
        public virtual VerbConjugation ConjugatePresentUsted()
        {
            var conjugation = GetBasePresentUstedConjugation();
            conjugation.Pieces.AddRange(GetPresentElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentEl()
        {
            var conjugation = GetBasePresentElConjugation();
            conjugation.Pieces.AddRange(GetPresentElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentElla()
        {
            var conjugation = GetBasePresentEllaConjugation();
            conjugation.Pieces.AddRange(GetPresentElPieces());
            return conjugation;
        }
        public abstract VerbConjugation ConjugatePresentNosotros();
        public abstract VerbConjugation ConjugatePresentVosotros();
        public virtual VerbConjugation ConjugatePresentUstedes()
        {
            var conjugation = GetBasePresentUstedesConjugation();
            conjugation.Pieces.AddRange(GetPresentEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentEllos()
        {
            var conjugation = GetBasePresentEllosConjugation();
            conjugation.Pieces.AddRange(GetPresentEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentEllas()
        {
            var conjugation = GetBasePresentEllasConjugation();
            conjugation.Pieces.AddRange(GetPresentEllosPieces());
            return conjugation;
        }
        public abstract List<VerbConjugationPiece> GetPresentElPieces();
        public abstract List<VerbConjugationPiece> GetPresentEllosPieces();
        internal VerbConjugation GetBasePresentYoConjugation(int whichCore = 1)
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

            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentElConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentEllaConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentEllosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePresentEllasConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

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
        public virtual VerbConjugation ConjugatePreteriteUsted()
        {
            var conjugation = GetBasePreteriteUstedConjugation();
            conjugation.Pieces.AddRange(GetPreteriteElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteEl()
        {
            var conjugation = GetBasePreteriteElConjugation();
            conjugation.Pieces.AddRange(GetPreteriteElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteElla()
        {
            var conjugation = GetBasePreteriteEllaConjugation();
            conjugation.Pieces.AddRange(GetPreteriteElPieces());
            return conjugation;
        }
        public abstract VerbConjugation ConjugatePreteriteNosotros();
        public abstract VerbConjugation ConjugatePreteriteVosotros();
        public virtual VerbConjugation ConjugatePreteriteUstedes()
        {
            var conjugation = GetBasePreteriteUstedesConjugation();
            conjugation.Pieces.AddRange(GetPreteriteEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteEllos()
        {
            var conjugation = GetBasePreteriteEllosConjugation();
            conjugation.Pieces.AddRange(GetPreteriteEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteEllas()
        {
            var conjugation = GetBasePreteriteEllasConjugation();
            conjugation.Pieces.AddRange(GetPreteriteEllosPieces());
            return conjugation;
        }
        public abstract List<VerbConjugationPiece> GetPreteriteElPieces();
        public abstract List<VerbConjugationPiece> GetPreteriteEllosPieces();
        internal VerbConjugation GetBasePreteriteYoConjugation(int whichCore = 1)
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

            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteElConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteEllaConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteEllosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBasePreteriteEllasConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

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
        public virtual VerbConjugation ConjugateImperfectUsted()
        {
            var conjugation = GetBaseImperfectUstedConjugation();
            conjugation.Pieces.AddRange(GetImperfectElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectEl()
        {
            var conjugation = GetBaseImperfectElConjugation();
            conjugation.Pieces.AddRange(GetImperfectElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectElla()
        {
            var conjugation = GetBaseImperfectEllaConjugation();
            conjugation.Pieces.AddRange(GetImperfectElPieces());
            return conjugation;
        }
        public abstract VerbConjugation ConjugateImperfectNosotros();
        public abstract VerbConjugation ConjugateImperfectVosotros();
        public virtual VerbConjugation ConjugateImperfectUstedes()
        {
            var conjugation = GetBaseImperfectUstedesConjugation();
            conjugation.Pieces.AddRange(GetImperfectEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectEllos()
        {
            var conjugation = GetBaseImperfectEllosConjugation();
            conjugation.Pieces.AddRange(GetImperfectEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectEllas()
        {
            var conjugation = GetBaseImperfectEllasConjugation();
            conjugation.Pieces.AddRange(GetImperfectEllosPieces());
            return conjugation;
        }
        public abstract List<VerbConjugationPiece> GetImperfectElPieces();
        public abstract List<VerbConjugationPiece> GetImperfectEllosPieces();
        internal VerbConjugation GetBaseImperfectYoConjugation(int whichCore = 1)
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

            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectElConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectEllaConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectEllosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseImperfectEllasConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

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
        public virtual VerbConjugation ConjugateConditionalUsted()
        {
            var conjugation = GetBaseConditionalUstedConjugation();
            conjugation.Pieces.AddRange(GetConditionalElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalEl()
        {
            var conjugation = GetBaseConditionalElConjugation();
            conjugation.Pieces.AddRange(GetConditionalElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalElla()
        {
            var conjugation = GetBaseConditionalEllaConjugation();
            conjugation.Pieces.AddRange(GetConditionalElPieces());
            return conjugation;
        }
        public abstract VerbConjugation ConjugateConditionalNosotros();
        public abstract VerbConjugation ConjugateConditionalVosotros();
        public virtual VerbConjugation ConjugateConditionalUstedes()
        {
            var conjugation = GetBaseConditionalUstedesConjugation();
            conjugation.Pieces.AddRange(GetConditionalEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalEllos()
        {
            var conjugation = GetBaseConditionalEllosConjugation();
            conjugation.Pieces.AddRange(GetConditionalEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalEllas()
        {
            var conjugation = GetBaseConditionalEllasConjugation();
            conjugation.Pieces.AddRange(GetConditionalEllosPieces());
            return conjugation;
        }
        public abstract List<VerbConjugationPiece> GetConditionalElPieces();
        public abstract List<VerbConjugationPiece> GetConditionalEllosPieces();
        internal VerbConjugation GetBaseConditionalYoConjugation(int whichCore = 1)
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

            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalElConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalEllaConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalEllosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseConditionalEllasConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

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
        public virtual VerbConjugation ConjugateFutureUsted()
        {
            var conjugation = GetBaseFutureUstedConjugation();
            conjugation.Pieces.AddRange(GetFutureElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureEl()
        {
            var conjugation = GetBaseFutureElConjugation();
            conjugation.Pieces.AddRange(GetFutureElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureElla()
        {
            var conjugation = GetBaseFutureEllaConjugation();
            conjugation.Pieces.AddRange(GetFutureElPieces());
            return conjugation;
        }
        public abstract VerbConjugation ConjugateFutureNosotros();
        public abstract VerbConjugation ConjugateFutureVosotros();
        public virtual VerbConjugation ConjugateFutureUstedes()
        {
            var conjugation = GetBaseFutureUstedesConjugation();
            conjugation.Pieces.AddRange(GetFutureEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureEllos()
        {
            var conjugation = GetBaseFutureEllosConjugation();
            conjugation.Pieces.AddRange(GetFutureEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureEllas()
        {
            var conjugation = GetBaseFutureEllasConjugation();
            conjugation.Pieces.AddRange(GetFutureEllosPieces());
            return conjugation;
        }
        public abstract List<VerbConjugationPiece> GetFutureElPieces();
        public abstract List<VerbConjugationPiece> GetFutureEllosPieces();
        internal VerbConjugation GetBaseFutureYoConjugation(int whichCore = 1)
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

            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureElConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureEllaConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureEllosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseFutureEllasConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

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
        public virtual VerbConjugation ConjugateSubjunctivePresentUsted()
        {
            var conjugation = GetBaseSubjunctivePresentUstedConjugation();
            conjugation.Pieces.AddRange(GetSubjunctivePresentElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentEl()
        {
            var conjugation = GetBaseSubjunctivePresentElConjugation();
            conjugation.Pieces.AddRange(GetSubjunctivePresentElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentElla()
        {
            var conjugation = GetBaseSubjunctivePresentEllaConjugation();
            conjugation.Pieces.AddRange(GetSubjunctivePresentElPieces());
            return conjugation;
        }
        public abstract VerbConjugation ConjugateSubjunctivePresentNosotros();
        public abstract VerbConjugation ConjugateSubjunctivePresentVosotros();
        public virtual VerbConjugation ConjugateSubjunctivePresentUstedes()
        {
            var conjugation = GetBaseSubjunctivePresentUstedesConjugation();
            conjugation.Pieces.AddRange(GetSubjunctivePresentEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentEllos()
        {
            var conjugation = GetBaseSubjunctivePresentEllosConjugation();
            conjugation.Pieces.AddRange(GetSubjunctivePresentEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentEllas()
        {
            var conjugation = GetBaseSubjunctivePresentEllasConjugation();
            conjugation.Pieces.AddRange(GetSubjunctivePresentEllosPieces());
            return conjugation;
        }
        public abstract List<VerbConjugationPiece> GetSubjunctivePresentElPieces();
        public abstract List<VerbConjugationPiece> GetSubjunctivePresentEllosPieces();
        internal VerbConjugation GetBaseSubjunctivePresentYoConjugation(int whichCore = 1)
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

            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentElConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentEllaConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentEllosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctivePresentEllasConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

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
        public virtual VerbConjugation ConjugateSubjunctiveImperfectUsted()
        {
            var conjugation = GetBaseSubjunctiveImperfectUstedConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectEl()
        {
            var conjugation = GetBaseSubjunctiveImperfectElConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectElla()
        {
            var conjugation = GetBaseSubjunctiveImperfectEllaConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectElPieces());
            return conjugation;
        }
        public abstract VerbConjugation ConjugateSubjunctiveImperfectNosotros();
        public abstract VerbConjugation ConjugateSubjunctiveImperfectVosotros();
        public virtual VerbConjugation ConjugateSubjunctiveImperfectUstedes()
        {
            var conjugation = GetBaseSubjunctiveImperfectUstedesConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectEllos()
        {
            var conjugation = GetBaseSubjunctiveImperfectEllosConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectEllas()
        {
            var conjugation = GetBaseSubjunctiveImperfectEllasConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveImperfectEllosPieces());
            return conjugation;
        }
        public abstract List<VerbConjugationPiece> GetSubjunctiveImperfectElPieces();
        public abstract List<VerbConjugationPiece> GetSubjunctiveImperfectEllosPieces();
        internal VerbConjugation GetBaseSubjunctiveImperfectYoConjugation(int whichCore = 1)
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

            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectElConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectEllaConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectEllosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveImperfectEllasConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

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
        public virtual VerbConjugation ConjugateSubjunctiveFutureUsted()
        {
            var conjugation = GetBaseSubjunctiveFutureUstedConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveFutureElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureEl()
        {
            var conjugation = GetBaseSubjunctiveFutureElConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveFutureElPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureElla()
        {
            var conjugation = GetBaseSubjunctiveFutureEllaConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveFutureElPieces());
            return conjugation;
        }
        public abstract VerbConjugation ConjugateSubjunctiveFutureNosotros();
        public abstract VerbConjugation ConjugateSubjunctiveFutureVosotros();
        public virtual VerbConjugation ConjugateSubjunctiveFutureUstedes()
        {
            var conjugation = GetBaseSubjunctiveFutureUstedesConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveFutureEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureEllos()
        {
            var conjugation = GetBaseSubjunctiveFutureEllosConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveFutureEllosPieces());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureEllas()
        {
            var conjugation = GetBaseSubjunctiveFutureEllasConjugation();
            conjugation.Pieces.AddRange(GetSubjunctiveFutureEllosPieces());
            return conjugation;
        }
        public abstract List<VerbConjugationPiece> GetSubjunctiveFutureElPieces();
        public abstract List<VerbConjugationPiece> GetSubjunctiveFutureEllosPieces();
        internal VerbConjugation GetBaseSubjunctiveFutureYoConjugation(int whichCore = 1)
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

            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureElConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureEllaConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureEllosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseSubjunctiveFutureEllasConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

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

        internal VerbConjugation GetBaseAffirmativeImperativeTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseAffirmativeImperativeUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseAffirmativeImperativeNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseAffirmativeImperativeVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseAffirmativeImperativeUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePiece(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

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
        internal VerbConjugation GetBaseNegativeImperativeTuConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePieceNegativeImperative(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseNegativeImperativeUstedConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePieceNegativeImperative(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseNegativeImperativeNosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePieceNegativeImperative(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseNegativeImperativeVosotrosConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePieceNegativeImperative(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        internal VerbConjugation GetBaseNegativeImperativeUstedesConjugation(int whichCore = 1)
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
            conjugation.Pieces.AddRange(GetCorePieceNegativeImperative(whichCore));
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        #endregion

    }
    
}
