using Logic.Telemetry;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator
{
    public class EnglishVerbTranslator: IVerbTranslator
    {
        public string Translate(Verb englishVerb, VerbConjugation conjugationType)
        {
            string pronoun = GetPronoun(conjugationType);
            string verbHelper = GetVerbHelper(conjugationType);
            string conjugation = GetConjugation(englishVerb, conjugationType);

            return $"{pronoun} {verbHelper} {conjugation}";
        }
        private string GetPronoun(VerbConjugation conjugationType)
        {
            if (conjugationType.Mood == AvailableGrammaticalMood.NEGATIVE_IMPERATIVE ||
                    conjugationType.Mood == AvailableGrammaticalMood.IMPERATIVE)
            {
                return string.Empty;
            }

            if (conjugationType.Person == AvailableGrammaticalPerson.FIRSTPERSON)
            {
                if (conjugationType.Number == AvailableGrammaticalNumber.PLURAL)
                {
                    return "we";
                }
                return "I";
            }
            if (conjugationType.Person == AvailableGrammaticalPerson.SECONDPERSON ||
                conjugationType.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL)
            {
                return "you";
            }
            if (conjugationType.Person == AvailableGrammaticalPerson.THIRDPERSON)
            {
                if (conjugationType.Number == AvailableGrammaticalNumber.SINGULAR)
                {
                    if (conjugationType.Gender == AvailableGrammaticalGender.MASCULINE)
                    {
                        return "he";
                    }
                    if (conjugationType.Gender == AvailableGrammaticalGender.FEMININE)
                    {
                        return "she";
                    }
                }
                if (conjugationType.Number == AvailableGrammaticalNumber.PLURAL)
                {
                    return "they";
                }
            }
            
            return string.Empty; 
        }
        private string GetVerbHelper(VerbConjugation conjugationType)
        {
            if (conjugationType.Tense == AvailableGrammaticalTense.FUTURE)
            {
                return "will";
            }
            if (conjugationType.Mood == AvailableGrammaticalMood.CONDITIONAL)
            {
                return "would";
            }
            if (conjugationType.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                conjugationType.Mood == AvailableGrammaticalMood.INDICATIVE &&
                conjugationType.Number == AvailableGrammaticalNumber.SINGULAR)
            {
                if (conjugationType.Person == AvailableGrammaticalPerson.FIRSTPERSON ||
                    conjugationType.Person == AvailableGrammaticalPerson.THIRDPERSON)
                {
                    return "was";
                }
            }
            if (conjugationType.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                conjugationType.Mood == AvailableGrammaticalMood.INDICATIVE)
            {
                if (conjugationType.Person == AvailableGrammaticalPerson.SECONDPERSON ||
                    conjugationType.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL ||
                    conjugationType.Number == AvailableGrammaticalNumber.PLURAL)
                {
                    return "were";
                }
            }
            if(conjugationType.Mood == AvailableGrammaticalMood.IMPERATIVE &&
                conjugationType.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                conjugationType.Number == AvailableGrammaticalNumber.PLURAL)
            {
                return "let's";
            }
            if (conjugationType.Mood == AvailableGrammaticalMood.NEGATIVE_IMPERATIVE &&
                conjugationType.Person == AvailableGrammaticalPerson.FIRSTPERSON &&
                conjugationType.Number == AvailableGrammaticalNumber.PLURAL)
            {
                return "let's not";
            }
            if (conjugationType.Mood == AvailableGrammaticalMood.NEGATIVE_IMPERATIVE &&
                (conjugationType.Person == AvailableGrammaticalPerson.SECONDPERSON ||
                conjugationType.Person == AvailableGrammaticalPerson.SECONDPERSON_FORMAL))
            {
                return "don't";
            }
            return string.Empty;
        }
        private string GetConjugation(Verb englishVerb, VerbConjugation conjugationType)
        {
            if (string.IsNullOrEmpty(englishVerb.Core1)) { ErrorHandler.LogAndThrow(); return ""; }
            if (string.IsNullOrEmpty(englishVerb.Gerund)) { ErrorHandler.LogAndThrow(); return ""; }
            if (string.IsNullOrEmpty(englishVerb.Core2)) 
                { ErrorHandler.LogAndThrow(); return ""; }
            if (string.IsNullOrEmpty(englishVerb.Core3))
            { ErrorHandler.LogAndThrow(); return ""; }

            if (conjugationType.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                conjugationType.Mood == AvailableGrammaticalMood.INDICATIVE)
            {
                return englishVerb.Gerund;
            }
            if (conjugationType.Tense == AvailableGrammaticalTense.PAST)
            {
                if (conjugationType.Aspect == AvailableGrammaticalAspect.PERFECT &&
                    conjugationType.Mood == AvailableGrammaticalMood.INDICATIVE)
                {
                    return englishVerb.Core2;
                }
                if (conjugationType.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                    conjugationType.Mood == AvailableGrammaticalMood.SUBJUNCTIVE)
                {
                    return englishVerb.Core2;
                }
            }
            if (conjugationType.Person == AvailableGrammaticalPerson.THIRDPERSON &&
                conjugationType.Number == AvailableGrammaticalNumber.SINGULAR &&
                conjugationType.Tense == AvailableGrammaticalTense.PRESENT)
            {
                if (conjugationType.Mood == AvailableGrammaticalMood.INDICATIVE ||
                    conjugationType.Mood == AvailableGrammaticalMood.SUBJUNCTIVE)
                {
                    return englishVerb.Core3;
                }
            }
            return englishVerb.Core1;
        }
    }
}
