using Logic.Telemetry;
using Model;
using Model.Enums;
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
            IVerbTranslator? targetTranslator, Verb sourceLanguageInfinitive, Verb? targetLanguageInfinitive) :
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

        public virtual List<VerbConjugation> ConjugatePresent()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 10,
            };

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"yo\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentTu()
        {


            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 20,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }

            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentEl()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 30,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"él\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentElla()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 40,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"ella\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 50,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 60,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 70,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentEllos()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 80,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"ellos\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentEllas()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 90,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"ellas\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePresentUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 100,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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

        public virtual List<VerbConjugation> ConjugatePreterite()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 110,
            };

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"yo\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteTu()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 120,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteEl()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 130,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"él\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteElla()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 140,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"ella\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 150,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 160,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 170,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteEllos()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 180,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"ellos\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteEllas()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 190,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"ellas\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructurePreteriteUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 200,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: preterite \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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

        public virtual List<VerbConjugation> ConjugateImperfect()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 210,
            };

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"yo\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectTu()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 220,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectEl()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 230,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"él\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectElla()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 240,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"ella\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 250,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 260,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 270,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectEllos()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 280,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"ellos\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectEllas()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 290,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"ellas\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureImperfectUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 300,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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

        public virtual List<VerbConjugation> ConjugateConditional()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 310,
            };

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"yo\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalTu()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 320,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalEl()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 330,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"él\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalElla()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 340,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"ella\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 350,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 360,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 370,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalEllos()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 380,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"ellos\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalEllas()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 390,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"ellas\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureConditionalUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.CONDITIONAL,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 400,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: conditional \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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
        public virtual List<VerbConjugation> ConjugateFuture()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 410,
            };

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"yo\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureTu()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 420,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureEl()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 430,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"él\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureElla()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 440,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"ella\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 450,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 460,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 470,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureEllos()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 480,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"ellos\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureEllas()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 490,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"ellas\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureFutureUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.INDICATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 500,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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

        public virtual List<VerbConjugation> ConjugateSubjunctivePresent()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 510,
            };

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"yo\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentTu()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 520,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentEl()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 530,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"él\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentElla()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 540,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"ella\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 550,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 560,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 570,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentEllos()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 580,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"ellos\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentEllas()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 590,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"ellas\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctivePresentUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 600,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: present subjunctive \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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

        public virtual List<VerbConjugation> ConjugateSubjunctiveImperfect()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 610,
            };

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"yo\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectTu()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 620,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectEl()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 630,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"él\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectElla()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 640,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"ella\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 650,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 660,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 670,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectEllos()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 680,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"ellos\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectEllas()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 690,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"ellas\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveImperfectUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PAST,
                Aspect = AvailableGrammaticalAspect.IMPERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 700,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: imperfect subjunctive \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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
        public virtual List<VerbConjugation> ConjugateSubjunctiveFuture()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 710,
            };

            conjugation.Pieces.AddRange(GetPronounPiece("yo"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"yo\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureTu()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 720,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureEl()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 730,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("él"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"él\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureElla()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 740,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ella"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"ella\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 750,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 760,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 770,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureEllos()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.MASCULINE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 780,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellos"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"ellos\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureEllas()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.THIRDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.FEMININE,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 790,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ellas"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"ellas\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureSubjunctiveFutureUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.FUTURE,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.SUBJUNCTIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 800,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: future subjunctive \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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
        public virtual List<VerbConjugation> ConjugateAffirmativeImperative()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 810,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("tú"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: affirmative imperative \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureAffirmativeImperativeUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 820,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("usted"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: affirmative imperative \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureAffirmativeImperativeNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 830,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("nosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: affirmative imperative \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureAffirmativeImperativeVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 840,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("vosotros"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: affirmative imperative \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureAffirmativeImperativeUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 850,
            };
            conjugation.Pieces.AddRange(GetPronounPiece("ustedes"));

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: affirmative imperative \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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
        public virtual List<VerbConjugation> ConjugateNegativeImperative()
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
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 910,
            };
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: negative imperative \"tú\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureNegativeImperativeUsted()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.SINGULAR,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 920,
            };
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: negative imperative \"usted\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureNegativeImperativeNosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.FIRSTPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 930,
            };
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: negative imperative \"nosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureNegativeImperativeVosotros()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 940,
            };
            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: negative imperative \"vosotros\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
            return conjugation;
        }
        public virtual VerbConjugation GetStructureNegativeImperativeUstedes()
        {
            

            var conjugation = new VerbConjugation()
            {
                Verb = _sourceLanguageInfinitive,
                Person = AvailableGrammaticalPerson.SECONDPERSON_FORMAL,
                Number = AvailableGrammaticalNumber.PLURAL,
                Gender = AvailableGrammaticalGender.ANY,
                Tense = AvailableGrammaticalTense.PRESENT,
                Aspect = AvailableGrammaticalAspect.PERFECT,
                Mood = AvailableGrammaticalMood.NEGATIVE_IMPERATIVE,
                Voice = AvailableGrammaticalVoice.ACTIVE,
                Ordinal = 950,
            };

            conjugation.Pieces.Add(new VerbConjugationPiece()
            {
                Type = AvailableVerbConjugationPieceType.CORE,
                Ordinal = 0,
                Piece = "no "
            });

            if (_targetTranslator is not null && _targetLanguageInfinitive is not null)
            {
                string? targetTranslation = _targetTranslator.Translate(_targetLanguageInfinitive, conjugation);
                conjugation.Translation = $"{targetTranslation}: negative imperative \"ustedes\" conjugation of {_sourceLanguageInfinitive.Infinitive}";
            }
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
