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
                Status = AvailableWordUserStatus.UNKNOWN,
                Translation = string.Empty
            };
            Insert.WordUser(context, newWordUser);
            return newWordUser;
        }
    }
}
