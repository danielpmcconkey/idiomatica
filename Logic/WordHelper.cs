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
        public static Word CreateEmptyWord(LanguageUser languageUser)
        {
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

        public static Dictionary<string, Word> FetchWordDictForLanguageUser(
            IdiomaticaContext context, LanguageUser languageUser)
        {
            Dictionary<string, Word> wordDict = new Dictionary<string, Word>();

            if (languageUser == null) return wordDict;

            Expression<Func<Word, bool>> filter =
                (x => x.LanguageUser.LanguageId == languageUser.LanguageId);
            var allWordsInLanguage = Fetch.WordsAndChildrenAndParents(context, filter);

            foreach (var word in allWordsInLanguage)
            {
                // TextLowerCase and LanguageId are a unique key in the database
                // so no need to check if it already exists
                wordDict.Add(word.TextLowerCase, word);
            }

            return wordDict;
        }
    }
}
