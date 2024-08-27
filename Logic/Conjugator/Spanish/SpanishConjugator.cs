using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.Spanish
{

    public abstract class SpanishConjugator : Conjugator
    {
        public SpanishConjugator(
            IVerbTranslator targetTranslator, Verb sourceLanguageInfinitive, Verb targetLanguageInfinitive) :
                base(targetTranslator, sourceLanguageInfinitive, targetLanguageInfinitive)
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

        internal virtual List<VerbConjugationPiece> GetCorePiece()
        {
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

            int endBitStart = firstBitStart + firstBitLength + seek.Length;  // vIEn -> 0 + 1 + 2 = 3
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
        internal List<VerbConjugationPiece> ReplaceRightOfCore(string? core, string seek)
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

            if (firstBit.Length > 0)
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

        #region Present

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

        public virtual VerbConjugation GetStructurePresentYo()
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

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentTu()
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
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentEl()
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
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentElla()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentUsted()
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
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentNosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentVosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentEllos()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentEllas()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentUstedes()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación del presente de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesPresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }]; ;
        }
        public virtual List<VerbConjugationPiece> GetRootPiecesPresentTu() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesPresentEl() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesPresentNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesPresentVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesPresentEllos() { return GetCorePiece(); }

        public virtual List<VerbConjugationPiece> GetStemPiecesPresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "o"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "as"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPresentNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "áis"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "an"
            }];
        }

        public virtual VerbConjugation ConjugatePresentYo()
        {
            var conjugation = GetStructurePresentYo();
            conjugation.Pieces.AddRange(GetRootPiecesPresentYo());
            conjugation.Pieces.AddRange(GetStemPiecesPresentYo());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentTu()
        {
            var conjugation = GetStructurePresentTu();
            conjugation.Pieces.AddRange(GetRootPiecesPresentTu());
            conjugation.Pieces.AddRange(GetStemPiecesPresentTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentEl()
        {
            var conjugation = GetStructurePresentEl();
            conjugation.Pieces.AddRange(GetRootPiecesPresentEl());
            conjugation.Pieces.AddRange(GetStemPiecesPresentEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentElla()
        {
            var conjugation = GetStructurePresentElla();
            conjugation.Pieces.AddRange(GetRootPiecesPresentEl());
            conjugation.Pieces.AddRange(GetStemPiecesPresentEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentUsted()
        {
            var conjugation = GetStructurePresentUsted();
            conjugation.Pieces.AddRange(GetRootPiecesPresentEl());
            conjugation.Pieces.AddRange(GetStemPiecesPresentEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentNosotros()
        {
            var conjugation = GetStructurePresentNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesPresentNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesPresentNosotros());
            return conjugation;
        }

        public virtual VerbConjugation ConjugatePresentVosotros()
        {
            var conjugation = GetStructurePresentVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesPresentVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesPresentVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentEllos()
        {
            var conjugation = GetStructurePresentEllos();
            conjugation.Pieces.AddRange(GetRootPiecesPresentEllos());
            conjugation.Pieces.AddRange(GetStemPiecesPresentEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentEllas()
        {
            var conjugation = GetStructurePresentEllas();
            conjugation.Pieces.AddRange(GetRootPiecesPresentEllos());
            conjugation.Pieces.AddRange(GetStemPiecesPresentEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePresentUstedes()
        {
            var conjugation = GetStructurePresentUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesPresentEllos());
            conjugation.Pieces.AddRange(GetStemPiecesPresentEllos());
            return conjugation;
        }

        #endregion


        #region Preterite

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

        public virtual VerbConjugation GetStructurePreteriteYo()
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

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteTu()
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
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteEl()
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
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteElla()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteUsted()
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
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteNosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteVosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteEllos()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteEllas()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteUstedes()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación pretérita de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesPreteriteYo() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesPreteriteTu() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesPreteriteEl() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesPreteriteNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesPreteriteVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesPreteriteEllos() { return GetCorePiece(); }

        public virtual List<VerbConjugationPiece> GetStemPiecesPreteriteYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "é"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPreteriteTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aste"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPreteriteEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ó"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPreteriteNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "amos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPreteriteVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "asteis"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesPreteriteEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aron"
            }];
        }

        public virtual VerbConjugation ConjugatePreteriteYo()
        {
            var conjugation = GetStructurePreteriteYo();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteYo());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteYo());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteTu()
        {
            var conjugation = GetStructurePreteriteTu();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteTu());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteEl()
        {
            var conjugation = GetStructurePreteriteEl();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteEl());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteElla()
        {
            var conjugation = GetStructurePreteriteElla();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteEl());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteUsted()
        {
            var conjugation = GetStructurePreteriteUsted();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteEl());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteNosotros()
        {
            var conjugation = GetStructurePreteriteNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteNosotros());
            return conjugation;
        }

        public virtual VerbConjugation ConjugatePreteriteVosotros()
        {
            var conjugation = GetStructurePreteriteVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteEllos()
        {
            var conjugation = GetStructurePreteriteEllos();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteEllos());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteEllas()
        {
            var conjugation = GetStructurePreteriteEllas();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteEllos());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugatePreteriteUstedes()
        {
            var conjugation = GetStructurePreteriteUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesPreteriteEllos());
            conjugation.Pieces.AddRange(GetStemPiecesPreteriteEllos());
            return conjugation;
        }

        #endregion


        #region Imperfect

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

        public virtual VerbConjugation GetStructureImperfectYo()
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

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectTu()
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
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectEl()
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
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectElla()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectUsted()
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
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectNosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectVosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectEllos()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectEllas()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectUstedes()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación imperfecta de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }]; ;
        }
        public virtual List<VerbConjugationPiece> GetRootPiecesImperfectTu() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesImperfectEl() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesImperfectNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesImperfectVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesImperfectEllos() { return GetCorePiece(); }

        public virtual List<VerbConjugationPiece> GetStemPiecesImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aba"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "abas"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aba"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ábamos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "abais"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aban"
            }];
        }

        public virtual VerbConjugation ConjugateImperfectYo()
        {
            var conjugation = GetStructureImperfectYo();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectYo());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectYo());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectTu()
        {
            var conjugation = GetStructureImperfectTu();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectTu());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectEl()
        {
            var conjugation = GetStructureImperfectEl();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectEl());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectElla()
        {
            var conjugation = GetStructureImperfectElla();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectEl());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectUsted()
        {
            var conjugation = GetStructureImperfectUsted();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectEl());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectNosotros()
        {
            var conjugation = GetStructureImperfectNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectNosotros());
            return conjugation;
        }

        public virtual VerbConjugation ConjugateImperfectVosotros()
        {
            var conjugation = GetStructureImperfectVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectEllos()
        {
            var conjugation = GetStructureImperfectEllos();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectEllos());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectEllas()
        {
            var conjugation = GetStructureImperfectEllas();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectEllos());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateImperfectUstedes()
        {
            var conjugation = GetStructureImperfectUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesImperfectEllos());
            conjugation.Pieces.AddRange(GetStemPiecesImperfectEllos());
            return conjugation;
        }

        #endregion


        #region Conditional

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

        public virtual VerbConjugation GetStructureConditionalYo()
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

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalTu()
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
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalEl()
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
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalElla()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalUsted()
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
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalNosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalVosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalEllos()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalEllas()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalUstedes()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación condicional de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesConditionalYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }]; ;
        }
        public virtual List<VerbConjugationPiece> GetRootPiecesConditionalTu() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesConditionalEl() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesConditionalNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesConditionalVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesConditionalEllos() { return GetCorePiece(); }

        public virtual List<VerbConjugationPiece> GetStemPiecesConditionalYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aría"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesConditionalTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arías"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesConditionalEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aría"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesConditionalNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aríamos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesConditionalVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aríais"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesConditionalEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arían"
            }];
        }

        public virtual VerbConjugation ConjugateConditionalYo()
        {
            var conjugation = GetStructureConditionalYo();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalYo());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalYo());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalTu()
        {
            var conjugation = GetStructureConditionalTu();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalTu());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalEl()
        {
            var conjugation = GetStructureConditionalEl();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalEl());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalElla()
        {
            var conjugation = GetStructureConditionalElla();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalEl());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalUsted()
        {
            var conjugation = GetStructureConditionalUsted();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalEl());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalNosotros()
        {
            var conjugation = GetStructureConditionalNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalNosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalVosotros()
        {
            var conjugation = GetStructureConditionalVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalEllos()
        {
            var conjugation = GetStructureConditionalEllos();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalEllos());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalEllas()
        {
            var conjugation = GetStructureConditionalEllas();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalEllos());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateConditionalUstedes()
        {
            var conjugation = GetStructureConditionalUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesConditionalEllos());
            conjugation.Pieces.AddRange(GetStemPiecesConditionalEllos());
            return conjugation;
        }

        #endregion


        #region Future
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

        public virtual VerbConjugation GetStructureFutureYo()
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

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureTu()
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
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureEl()
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
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureElla()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureUsted()
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
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureNosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureVosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureEllos()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureEllas()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureUstedes()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de la conjugación «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }]; ;
        }
        public virtual List<VerbConjugationPiece> GetRootPiecesFutureTu() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesFutureEl() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesFutureNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesFutureVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesFutureEllos() { return GetCorePiece(); }

        public virtual List<VerbConjugationPiece> GetStemPiecesFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aré"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arás"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ará"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aremos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aréis"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arán"
            }];
        }

        public virtual VerbConjugation ConjugateFutureYo()
        {
            var conjugation = GetStructureFutureYo();
            conjugation.Pieces.AddRange(GetRootPiecesFutureYo());
            conjugation.Pieces.AddRange(GetStemPiecesFutureYo());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureTu()
        {
            var conjugation = GetStructureFutureTu();
            conjugation.Pieces.AddRange(GetRootPiecesFutureTu());
            conjugation.Pieces.AddRange(GetStemPiecesFutureTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureEl()
        {
            var conjugation = GetStructureFutureEl();
            conjugation.Pieces.AddRange(GetRootPiecesFutureEl());
            conjugation.Pieces.AddRange(GetStemPiecesFutureEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureElla()
        {
            var conjugation = GetStructureFutureElla();
            conjugation.Pieces.AddRange(GetRootPiecesFutureEl());
            conjugation.Pieces.AddRange(GetStemPiecesFutureEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureUsted()
        {
            var conjugation = GetStructureFutureUsted();
            conjugation.Pieces.AddRange(GetRootPiecesFutureEl());
            conjugation.Pieces.AddRange(GetStemPiecesFutureEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureNosotros()
        {
            var conjugation = GetStructureFutureNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesFutureNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesFutureNosotros());
            return conjugation;
        }

        public virtual VerbConjugation ConjugateFutureVosotros()
        {
            var conjugation = GetStructureFutureVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesFutureVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesFutureVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureEllos()
        {
            var conjugation = GetStructureFutureEllos();
            conjugation.Pieces.AddRange(GetRootPiecesFutureEllos());
            conjugation.Pieces.AddRange(GetStemPiecesFutureEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureEllas()
        {
            var conjugation = GetStructureFutureEllas();
            conjugation.Pieces.AddRange(GetRootPiecesFutureEllos());
            conjugation.Pieces.AddRange(GetStemPiecesFutureEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateFutureUstedes()
        {
            var conjugation = GetStructureFutureUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesFutureEllos());
            conjugation.Pieces.AddRange(GetStemPiecesFutureEllos());
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

        public virtual VerbConjugation GetStructureSubjunctivePresentYo()
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

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentTu()
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
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentEl()
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
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentElla()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentUsted()
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
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentNosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentVosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentEllos()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentEllas()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentUstedes()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en presente de subjuntivo de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }]; ;
        }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentTu() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEl() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctivePresentEllos() { return GetCorePiece(); }

        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "e"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "es"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "e"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "emos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éis"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctivePresentEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "en"
            }];
        }

        public virtual VerbConjugation ConjugateSubjunctivePresentYo()
        {
            var conjugation = GetStructureSubjunctivePresentYo();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentYo());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentYo());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentTu()
        {
            var conjugation = GetStructureSubjunctivePresentTu();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentTu());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentEl()
        {
            var conjugation = GetStructureSubjunctivePresentEl();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentEl());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentElla()
        {
            var conjugation = GetStructureSubjunctivePresentElla();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentEl());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentUsted()
        {
            var conjugation = GetStructureSubjunctivePresentUsted();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentEl());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentNosotros()
        {
            var conjugation = GetStructureSubjunctivePresentNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentNosotros());
            return conjugation;
        }

        public virtual VerbConjugation ConjugateSubjunctivePresentVosotros()
        {
            var conjugation = GetStructureSubjunctivePresentVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentEllos()
        {
            var conjugation = GetStructureSubjunctivePresentEllos();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentEllos());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentEllas()
        {
            var conjugation = GetStructureSubjunctivePresentEllas();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentEllos());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctivePresentUstedes()
        {
            var conjugation = GetStructureSubjunctivePresentUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctivePresentEllos());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctivePresentEllos());
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

        public virtual VerbConjugation GetStructureSubjunctiveImperfectYo()
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

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectTu()
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
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectEl()
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
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectElla()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectUsted()
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
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectNosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectVosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectEllos()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectEllas()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectUstedes()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación en imperfecto de subjuntivo de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }]; ;
        }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectTu() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEl() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveImperfectEllos() { return GetCorePiece(); }

        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ara"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aras"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ara"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "áramos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "arais"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveImperfectEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aran"
            }];
        }

        public virtual VerbConjugation ConjugateSubjunctiveImperfectYo()
        {
            var conjugation = GetStructureSubjunctiveImperfectYo();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectYo());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectYo());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectTu()
        {
            var conjugation = GetStructureSubjunctiveImperfectTu();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectTu());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectEl()
        {
            var conjugation = GetStructureSubjunctiveImperfectEl();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectEl());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectElla()
        {
            var conjugation = GetStructureSubjunctiveImperfectElla();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectEl());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectUsted()
        {
            var conjugation = GetStructureSubjunctiveImperfectUsted();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectEl());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectNosotros()
        {
            var conjugation = GetStructureSubjunctiveImperfectNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectNosotros());
            return conjugation;
        }

        public virtual VerbConjugation ConjugateSubjunctiveImperfectVosotros()
        {
            var conjugation = GetStructureSubjunctiveImperfectVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectEllos()
        {
            var conjugation = GetStructureSubjunctiveImperfectEllos();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectEllos());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectEllas()
        {
            var conjugation = GetStructureSubjunctiveImperfectEllas();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectEllos());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveImperfectUstedes()
        {
            var conjugation = GetStructureSubjunctiveImperfectUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveImperfectEllos());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveImperfectEllos());
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

        public virtual VerbConjugation GetStructureSubjunctiveFutureYo()
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

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «yo» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureTu()
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
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureEl()
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
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «él» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureElla()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ella» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureUsted()
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
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureNosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureVosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureEllos()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ellos» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureEllas()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ellas» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureUstedes()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: futuro de subjuntivo de la conjugación «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }]; ;
        }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureTu() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEl() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesSubjunctiveFutureEllos() { return GetCorePiece(); }

        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureYo()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "are"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ares"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEl()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "are"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "áremos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "areis"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesSubjunctiveFutureEllos()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "aren"
            }];
        }

        public virtual VerbConjugation ConjugateSubjunctiveFutureYo()
        {
            var conjugation = GetStructureSubjunctiveFutureYo();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureYo());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureYo());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureTu()
        {
            var conjugation = GetStructureSubjunctiveFutureTu();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureTu());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureEl()
        {
            var conjugation = GetStructureSubjunctiveFutureEl();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureEl());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureElla()
        {
            var conjugation = GetStructureSubjunctiveFutureElla();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureEl());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureUsted()
        {
            var conjugation = GetStructureSubjunctiveFutureUsted();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureEl());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureEl());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureNosotros()
        {
            var conjugation = GetStructureSubjunctiveFutureNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureNosotros());
            return conjugation;
        }

        public virtual VerbConjugation ConjugateSubjunctiveFutureVosotros()
        {
            var conjugation = GetStructureSubjunctiveFutureVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureEllos()
        {
            var conjugation = GetStructureSubjunctiveFutureEllos();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureEllos());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureEllas()
        {
            var conjugation = GetStructureSubjunctiveFutureEllas();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureEllos());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureEllos());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateSubjunctiveFutureUstedes()
        {
            var conjugation = GetStructureSubjunctiveFutureUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesSubjunctiveFutureEllos());
            conjugation.Pieces.AddRange(GetStemPiecesSubjunctiveFutureEllos());
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

        public virtual VerbConjugation GetStructureAffirmativeImperativeTu()
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
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureAffirmativeImperativeUsted()
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
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureAffirmativeImperativeNosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureAffirmativeImperativeVosotros()
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
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureAffirmativeImperativeUstedes()
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
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación afirmativa imperativa «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }]; ;
        }
        public virtual List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUsted() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesAffirmativeImperativeUstedes() { return GetCorePiece(); }


        public virtual List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "a"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "e"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "emos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "ad"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesAffirmativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "en"
            }];
        }


        public virtual VerbConjugation ConjugateAffirmativeImperativeTu()
        {
            var conjugation = GetStructureAffirmativeImperativeTu();
            conjugation.Pieces.AddRange(GetRootPiecesAffirmativeImperativeTu());
            conjugation.Pieces.AddRange(GetStemPiecesAffirmativeImperativeTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateAffirmativeImperativeUsted()
        {
            var conjugation = GetStructureAffirmativeImperativeUsted();
            conjugation.Pieces.AddRange(GetRootPiecesAffirmativeImperativeUsted());
            conjugation.Pieces.AddRange(GetStemPiecesAffirmativeImperativeUsted());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateAffirmativeImperativeNosotros()
        {
            var conjugation = GetStructureAffirmativeImperativeNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesAffirmativeImperativeNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesAffirmativeImperativeNosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateAffirmativeImperativeVosotros()
        {
            var conjugation = GetStructureAffirmativeImperativeVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesAffirmativeImperativeVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesAffirmativeImperativeVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateAffirmativeImperativeUstedes()
        {
            var conjugation = GetStructureAffirmativeImperativeUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesAffirmativeImperativeUstedes());
            conjugation.Pieces.AddRange(GetStemPiecesAffirmativeImperativeUstedes());
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

        public virtual VerbConjugation GetStructureNegativeImperativeTu()
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
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «tú» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureNegativeImperativeUsted()
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
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «usted» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureNegativeImperativeNosotros()
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
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «nosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureNegativeImperativeVosotros()
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
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «vosotros» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }
        public virtual VerbConjugation GetStructureNegativeImperativeUstedes()
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

            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
            conjugation.Translation = $"{targetTranslation}: conjugación negativo imperativo de «ustedes» de {_sourceLanguageInfinitive.Infinitive}";

            return conjugation;
        }

        public virtual List<VerbConjugationPiece> GetRootPiecesNegativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 100,
                Type = AvailableVerbConjugationPieceType.CORE,
                Piece = _sourceLanguageInfinitive.Core1
            }]; ;
        }
        public virtual List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUsted() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesNegativeImperativeNosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesNegativeImperativeVosotros() { return GetCorePiece(); }
        public virtual List<VerbConjugationPiece> GetRootPiecesNegativeImperativeUstedes() { return GetCorePiece(); }


        public virtual List<VerbConjugationPiece> GetStemPiecesNegativeImperativeTu()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "es"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUsted()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "e"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesNegativeImperativeNosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "emos"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesNegativeImperativeVosotros()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "éis"
            }];
        }
        public virtual List<VerbConjugationPiece> GetStemPiecesNegativeImperativeUstedes()
        {
            return [new VerbConjugationPiece()
            {
                Ordinal = 200,
                Type = AvailableVerbConjugationPieceType.REGULAR,
                Piece = "en"
            }];
        }


        public virtual VerbConjugation ConjugateNegativeImperativeTu()
        {
            var conjugation = GetStructureNegativeImperativeTu();
            conjugation.Pieces.AddRange(GetRootPiecesNegativeImperativeTu());
            conjugation.Pieces.AddRange(GetStemPiecesNegativeImperativeTu());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateNegativeImperativeUsted()
        {
            var conjugation = GetStructureNegativeImperativeUsted();
            conjugation.Pieces.AddRange(GetRootPiecesNegativeImperativeUsted());
            conjugation.Pieces.AddRange(GetStemPiecesNegativeImperativeUsted());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateNegativeImperativeNosotros()
        {
            var conjugation = GetStructureNegativeImperativeNosotros();
            conjugation.Pieces.AddRange(GetRootPiecesNegativeImperativeNosotros());
            conjugation.Pieces.AddRange(GetStemPiecesNegativeImperativeNosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateNegativeImperativeVosotros()
        {
            var conjugation = GetStructureNegativeImperativeVosotros();
            conjugation.Pieces.AddRange(GetRootPiecesNegativeImperativeVosotros());
            conjugation.Pieces.AddRange(GetStemPiecesNegativeImperativeVosotros());
            return conjugation;
        }
        public virtual VerbConjugation ConjugateNegativeImperativeUstedes()
        {
            var conjugation = GetStructureNegativeImperativeUstedes();
            conjugation.Pieces.AddRange(GetRootPiecesNegativeImperativeUstedes());
            conjugation.Pieces.AddRange(GetStemPiecesNegativeImperativeUstedes());
            return conjugation;
        }

        #endregion

    }

}
