using Microsoft.EntityFrameworkCore;
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
        public static Word GetWordById(IdiomaticaContext context, int id)
        {
            Func<Word, bool> filter = (x => x.Id == id);
            return GetWords(context, filter).FirstOrDefault();
        }
        public static List<Word> GetWordsForLanguageUser(IdiomaticaContext context, LanguageUser languageUser)
        {
            Func<Word, bool> filter = (x => x.LanguageUserId == languageUser.Id);
            return GetWords(context, filter);
        }
        public static List<Word> GetWords(IdiomaticaContext context, Func<Word, bool> filter)
        {
            return context.Words
                .Include(w => w.ParentWords)
                .Include(w => w.ChildWords)
                .Include(w => w.Status)
                .Include(w => w.LanguageUser)
                .Where(filter)
                .ToList();
        }
        public static Dictionary<string, Word> GetWordDictForLanguageUser(LanguageUser languageUser)
        {
            Dictionary<string, Word> wordDict = new Dictionary<string, Word>();

            if(languageUser == null) return wordDict;

            using (var context = new IdiomaticaContext())
            {
                Func<Word, bool> filter = (x => x.LanguageUser.LanguageId == languageUser.LanguageId);
                var allWordsInLanguage = GetWords(context, filter);

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
