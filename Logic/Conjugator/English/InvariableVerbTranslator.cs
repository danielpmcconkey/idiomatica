using Logic.Telemetry;
using Model;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Conjugator.English
{
    /// <summary>
    /// this is for verbs like "should" that only have 1 form
    /// </summary>
    public class InvariableVerbTranslator : EnglishVerbTranslator
    {
        public override string Translate(Verb englishVerb, VerbConjugation conjugationType)
        {
            // Imperfect, conditional, future, subjunctive future, and imperatives
            // don't have english equivalent
            bool isImperfect = (
                conjugationType.Tense == AvailableGrammaticalTense.PAST &&
                conjugationType.Aspect == AvailableGrammaticalAspect.IMPERFECT &&
                conjugationType.Mood == AvailableGrammaticalMood.INDICATIVE);
            bool isConditional = (
                conjugationType.Tense == AvailableGrammaticalTense.PRESENT &&
                conjugationType.Aspect == AvailableGrammaticalAspect.PERFECT &&
                conjugationType.Mood == AvailableGrammaticalMood.CONDITIONAL);
            bool isFuture = (
                conjugationType.Tense == AvailableGrammaticalTense.FUTURE &&
                conjugationType.Aspect == AvailableGrammaticalAspect.PERFECT &&
                conjugationType.Mood == AvailableGrammaticalMood.INDICATIVE);
            bool isSubjunctiveFuture = (
                conjugationType.Tense == AvailableGrammaticalTense.FUTURE &&
                conjugationType.Aspect == AvailableGrammaticalAspect.PERFECT &&
                conjugationType.Mood == AvailableGrammaticalMood.SUBJUNCTIVE);
            bool isImperative = (
                conjugationType.Mood == AvailableGrammaticalMood.IMPERATIVE ||
                conjugationType.Mood == AvailableGrammaticalMood.NEGATIVE_IMPERATIVE);
            
            
            if (isImperfect || isConditional || isFuture ||
                isSubjunctiveFuture || isImperative )
            {
                return $"no direct translation for this conjugation of [{englishVerb.Infinitive}]";
            }
            return base.Translate(englishVerb, conjugationType);
        }
    }
}
