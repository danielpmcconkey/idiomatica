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
        /// this will delete any existing DB tokens
        /// </summary>
        public static void CreateTokensFromSentenceAndSave(IdiomaticaContext context,
            Sentence sentence, Language language, Dictionary<string, Word> commonWordDict)
        {
            if (sentence == null)
            {
                throw new ArgumentNullException("Sentence cannot be null when creating tokens");
            }
            if (sentence.Id == 0)
            {
                throw new ArgumentException("Sentence must have a DB ID before adding tokens");
            }
            if (sentence.Text == null)
            {
                throw new ArgumentNullException("Sentence text cannot be null when creating tokens");
            }
            if (commonWordDict == null) 
            {
                throw new ArgumentNullException("word dictionary cannot be null when creating tokens");
            }

            // check if any already exist
            var existingTokens = context.Tokens.Where(t => t.SentenceId == sentence.Id);
            foreach( var existingToken in existingTokens )
            {
                context.Tokens.Remove(existingToken);
            }
            context.SaveChanges();

            // parse new ones

            List<Token> tokens = new List<Token>();

            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var wordsSplits = parser.SegmentTextByWordsKeepingPunctuation(sentence.Text);

            for (int i = 0; i < wordsSplits.Length; i++)
            {
                var wordSplit = wordsSplits[i];
                var cleanWord = parser.StipNonWordCharacters(wordSplit).ToLower();
                Word? existingWord = null;
                // check if the word is in the word dict
                if(commonWordDict.ContainsKey(cleanWord))
                {
                    existingWord = commonWordDict[cleanWord];
                }
                else
                {
                    // check if the word is already in the DB
                    existingWord = context.Words
                        .Where(w => w.LanguageId == language.Id && w.TextLowerCase == cleanWord)
                        .FirstOrDefault();
                }
                if (existingWord == null)
                {
                    if (cleanWord == string.Empty)
                    {
                        // there shouldn't be any empty words. that would mean
                        // that all non-word characters were stripped out and
                        // only left an empty string. Something like a string
                        // of punctuation or a quotation from another language
                        // that uses different characters. either way, create
                        // an empty word and move on
                        var emptyWord = WordHelper.CreateAndSaveNewWord(context, 
                            language, string.Empty, string.Empty);
                        emptyWord.Language = language;
                        commonWordDict[cleanWord] = emptyWord;
                        existingWord = emptyWord;
                    }
                    else
                    {
                        // this is a newly encountered word. save it to the DB
                        // todo: add actual romanization lookup here
                        var newWord = WordHelper.CreateAndSaveNewWord(context,
                            language, cleanWord, cleanWord);
                        //newWord.Language = language;
                        commonWordDict.Add(cleanWord, newWord);
                        existingWord = newWord;
                    }
                }
                Token token = new Token()
                {
                    Display = $"{wordSplit} ", // add the space that you previously took out
                    SentenceId = (int)sentence.Id,
                    Ordinal = i,
                    WordId = (int)existingWord.Id
                };
                Insert.Token(context, token);
                //token.Sentence = sentence;
                //token.Word = existingWord;
                //tokens.Add(token);
            }
            return;// tokens;
        }
    }
}
