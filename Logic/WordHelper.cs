using Model;
using Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class WordHelper
    {
        public static Dictionary<string, Word> GetWordDictForLanguageUser(LanguageUser languageUser)
        {
            Dictionary<string, Word> wordDict = new Dictionary<string, Word>();
            using (var context = new IdiomaticaContext())
            {
                Func<Word, bool> filter = (x => x.LanguageUser.LanguageId == languageUser.LanguageId);
                var allWordsInLanguage = Fetch.Words(context, filter);

                foreach (var word in allWordsInLanguage)
                {
                    // TextLowerCase and LanguageId are a unique key in the database
                    wordDict.Add(word.TextLowerCase, word);
                }
            }
            return wordDict;
        }
    }
}
