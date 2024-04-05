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
        public static Dictionary<string, Word> FetchWordDictForLanguageUser(LanguageUser languageUser)
        {
            Dictionary<string, Word> wordDict = new Dictionary<string, Word>();

            if (languageUser == null) return wordDict;

            Expression<Func<Word, bool>> filter =
                (x => x.LanguageUser.LanguageId == languageUser.LanguageId);
            var allWordsInLanguage = Fetch.WordsAndChildrenAndParents(filter);

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
