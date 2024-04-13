using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class WordHelper
    {
        public static Word? FetchWordByText(IdiomaticaContext context, Language language, string text)
        {
            return context.Words
                .Where(w => w.LanguageId == language.Id && w.TextLowerCase == text.ToLower())
                .FirstOrDefault();
        }
        
        public static Word CreateAndSaveNewWord(IdiomaticaContext context, 
            Language language, string text, string romanization)
        {
            if (language == null) throw new ArgumentNullException("language cannot be null when saving a new word");
            if (language.Id == 0) throw new ArgumentException("language ID cannot be 0 when saving a new word");
            Word newWord = new Word()
            {
                LanguageId = (int)language.Id,
                Romanization = romanization,
                Text = text.ToLower(),
                TextLowerCase = text.ToLower(),
            };
            Insert.Word(context, newWord);
            return newWord;
        }
        public static WordUser CreateAndSaveUnknownWordUser(IdiomaticaContext context,
            LanguageUser languageUser, Word word)
        {
            if (languageUser == null) throw new ArgumentNullException("languageUser cannot be null when saving new WordUser");
            if (languageUser.Id == 0) throw new ArgumentException("languageuser.Id cannot be 0 when saving new WordUser");
            if (word == null) throw new ArgumentNullException("word cannot be null when saving new WordUser");
            WordUser newWordUser = new WordUser()
            {
                LanguageUserId = (int)languageUser.Id,
                WordId = (int)word.Id,
                Word = word,
                Status = AvailableWordUserStatus.UNKNOWN,
                Translation = string.Empty
            };
            Insert.WordUser(context, newWordUser);
            return newWordUser;
        }

        public static Dictionary<string, WordUser> FetchWordUserDictForLanguageUser(
            IdiomaticaContext context, LanguageUser languageUser)
        {
            Dictionary<string, WordUser> wordDict = new Dictionary<string, WordUser>();

            if (languageUser == null) return wordDict;

            
            var allWordUsersInLanguage = context.WordUsers
                .Where(wu => wu.LanguageUserId == languageUser.Id)
                .Include(wu => wu.Word);

            foreach (var wordUser in allWordUsersInLanguage)
            {
                if (wordUser.Word is null) continue;

                // TextLowerCase and LanguageId are a unique key in the database
                // so no need to check if it already exists before adding
                wordDict.Add(wordUser.Word.TextLowerCase, wordUser);
            }

            return wordDict;
        }
        public static Dictionary<string, Word> FetchCommonWordDictForLanguage(
            IdiomaticaContext context, Language language)
        {
            if (language == null) throw new ArgumentNullException("language cannot be null when fetching common words");
            if (language.Id == 0) throw new ArgumentException("Language ID cannot be zero when fetching common words");
            
            Dictionary<string, Word> wordDict = new Dictionary<string, Word>();

            var topWordsInLanguage = context.Words.FromSql($"""
                SELECT TOP (1000) 
                        w.Id
                    , w.LanguageUserId
                    , w.Status
                    , w.Text
                    , w.TextLowerCase
                    , w.Translation
                    , w.Romanization
                    , w.TokenCount
                    , w.Created
                    , w.StatusChanged
                    --, count(t.Id) as numberOfUsages
                FROM Word w
                join Token t on w.Id = t.WordId
                where w.LanguageUserId = {language.Id}
                group by 
                      w.Id
                    , w.LanguageUserId
                    , w.Status
                    , w.Text
                    , w.TextLowerCase
                    , w.Translation
                    , w.Romanization
                    , w.TokenCount
                    , w.Created
                    , w.StatusChanged
                order by count(t.Id) desc
                """);

            foreach (var word in topWordsInLanguage)
            {
                
                // TextLowerCase and LanguageId are a unique key in the database
                // so no need to check if it already exists before adding
                wordDict.Add(word.TextLowerCase, word);
            }
            throw new NotImplementedException("above query needs to change based on new data model");
            return wordDict;
        }
    }
}
