using Azure;
using Logic.UILabels;
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
        /// <summary>
        /// this assumes the caller has already checked that the tokens aren't in the DB
        /// todo: is it possible to enforce this?
        /// </summary>

        public static List<Token> CreateTokensFromSentenceAndSave(IdiomaticaContext context,
            Sentence sentence, LanguageUser languageUser, Dictionary<string, Word> wordsDict)
        {
            if (sentence.Id == 0)
            {
                throw new InvalidDataException("Sentence must have a DB ID before adding children");
            }

            List<Token> tokens = new List<Token>();

            var parser = LanguageParser.Factory.GetLanguageParser(languageUser);
            var wordsSplits = parser.GetWordsFromText(sentence.Text, true);


            for (int i = 0; i < wordsSplits.Length; i++)
            {
                var wordSplit = wordsSplits[i];
                var cleanWord = parser.StripAllButWordCharacters(wordSplit).ToLower();
                if (!wordsDict.ContainsKey(cleanWord))
                {
                    if (cleanWord == string.Empty)
                    {
                        // todo: figure out how to handle numbers in languageParser
                        var emptyWord = CreateEmptyWord(languageUser);
                        Insert.Word(context, emptyWord);
                        emptyWord.LanguageUser = languageUser;
                        wordsDict.Add(string.Empty, emptyWord);
                    }
                    else
                    {
                        // this is a newly encountered word. create it and add to the dict
                        // todo: add actual romanization lookup here
                        var unknownWord = CreateUnknownWord(languageUser, cleanWord, cleanWord);
                        Insert.Word(context, unknownWord);
                        unknownWord.LanguageUser = languageUser;
                        wordsDict.Add(cleanWord, unknownWord);
                    }
                }
                var wordObject = wordsDict[cleanWord];
                Token token = new Token()
                {
                    Display = $"{wordSplit} ", // add the space that you previously took out
                    SentenceId = (int)sentence.Id,
                    Ordinal = i,
                    WordId = (int)wordObject.Id
                };
                Insert.Token(context, token);
                token.Sentence = sentence;
                token.Word = wordObject;
                tokens.Add(token);
            }
            return tokens;
        }

        public static Word CreateEmptyWord(LanguageUser languageUser)
        {
            // todo: move the empty word population somewhere else

            Word emptyWord = new Word()
            {
                LanguageUserId = (int)languageUser.Id,
                Romanization = string.Empty,
                ChildWords = new List<Word>(),
                ParentWords = new List<Word>(),
                Text = string.Empty,
                TextLowerCase = string.Empty,
                Status = AvailableStatus.IGNORED,
                Translation = string.Empty
            };
            return emptyWord;

        }
        public static Word CreateUnknownWord( 
            LanguageUser languageUser, string text, string romanization)
        {
            // todo: move the new word population somewhere else

            Word newWord = new Word()
            {
                LanguageUserId = (int)languageUser.Id,
                Romanization = romanization,
                ChildWords = new List<Word>(),
                ParentWords = new List<Word>(),
                Text = text.ToLower(),
                TextLowerCase = text.ToLower(),
                Status = AvailableStatus.UNKNOWN,
                Translation = string.Empty
            };
            return newWord;

        }
    }
}
