using Model;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class SentenceHelper
    {
        public static List<Token> TokenizeSentence(IdiomaticaContext context,
            Sentence sentence, LanguageUser languageUser, Dictionary<string, Word> wordsDict)
        {
            List<Token> tokens = new List<Token>();

            var parser = LanguageParser.Factory.GetLanguageParser(languageUser.Language);
            var wordsSplits = parser.GetWordsFromText(sentence.Text, true);
            for (int i = 0; i < wordsSplits.Length; i++)
            {
                var wordSplit = wordsSplits[i];
                var cleanWord = parser.StripAllButWordCharacters(wordSplit).ToLower();
                if(!wordsDict.ContainsKey(cleanWord))
                {
                    // todo: come up with error codes and put the human readable into the language packs
                    throw new InvalidDataException($"Words dictionary does not contain the word: \"{cleanWord}\"");
                }
                var wordObject = wordsDict[cleanWord];
                Token token = new Token() 
                {
                    Display = $"{wordSplit} ", // add the space that you previously took out
                    SentenceId = sentence.Id, 
                    Ordinal = i, 
                    WordId = wordObject.Id, 
                    //Word = wordObject, 
                    //Sentence = sentence
                };
                tokens.Add(token);
                context.Tokens.Add(token);
            }
            sentence.Tokens = tokens;
            context.SaveChanges();
            return tokens;
        }
    }
}
