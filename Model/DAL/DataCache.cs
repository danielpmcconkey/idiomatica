using Model.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        // note: we should created a cache registry to make the deletion an easier thing

        private static ConcurrentDictionary<Guid, Book> BookById = [];
        private static ConcurrentDictionary<Guid, List<BookListRow>> BookListRowsByUserId = [];
        private static ConcurrentDictionary<(Guid bookId, Guid userId), BookListRow?> BookListRowByBookIdAndUserId = [];
        private static ConcurrentDictionary<(Guid bookId, AvailableBookStat statKey), BookStat> BookStatByBookIdAndStatKey = [];
        private static ConcurrentDictionary<Guid, List<BookStat>> BookStatsByBookId = [];
        private static ConcurrentDictionary<Guid, List<BookTag>> BookTagsByBookId = new ConcurrentDictionary<Guid, List<BookTag>>();
        private static ConcurrentDictionary<Guid, BookUser> BookUserById = new ConcurrentDictionary<Guid, BookUser>();
        private static ConcurrentDictionary<(Guid bookId, Guid userId), BookUser> BookUserByBookIdAndUserId = new ConcurrentDictionary<(Guid bookId, Guid userId), BookUser>();
        private static ConcurrentDictionary<(Guid bookId, Guid userId), List<BookUserStat>> BookUserStatsByBookIdAndUserId = new ConcurrentDictionary<(Guid bookId, Guid userId), List<BookUserStat>>();
        private static ConcurrentDictionary<Guid, FlashCard?> FlashCardById = new ConcurrentDictionary<Guid, FlashCard?>();
        private static ConcurrentDictionary<Guid, FlashCard?> FlashCardAndFullRelationshipsById = new ConcurrentDictionary<Guid, FlashCard?>();
        private static ConcurrentDictionary<Guid, FlashCard?> FlashCardByWordUserId = new();
        //private static ConcurrentDictionary<(Guid languageUserId, int take), List<FlashCard>> FlashCardsActiveAndFullRelationshipsByLanguageUserId = new ();
        private static ConcurrentDictionary<Guid, FlashCardAttempt?> FlashCardAttemptById = new ConcurrentDictionary<Guid, FlashCardAttempt?>();
        private static ConcurrentDictionary<Guid, List<FlashCardAttempt>> FlashCardAttemptsByFlashCardId = new ConcurrentDictionary<Guid, List<FlashCardAttempt>>();
        private static ConcurrentDictionary<Guid, FlashCardParagraphTranslationBridge> FlashCardParagraphTranslationBridgeById = [];
        private static ConcurrentDictionary<(Guid flashCardId, Guid uiLanguageId), List<FlashCardParagraphTranslationBridge>>
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode = [];
        private static ConcurrentDictionary<AvailableLanguageCode, Language> LanguageByCode = [];
        private static ConcurrentDictionary<Guid, Language> LanguageById = [];
        private static ConcurrentDictionary<Guid, LanguageUser> LanguageUserById = new ConcurrentDictionary<Guid, LanguageUser>();
        private static ConcurrentDictionary<(Guid languageId, Guid userId), LanguageUser> LanguageUserByLanguageIdAndUserId = new ConcurrentDictionary<(Guid languageId, Guid userId), LanguageUser>();
        private static ConcurrentDictionary<Guid, List<LanguageUser>> LanguageUsersAndLanguageByUserId = new ConcurrentDictionary<Guid, List<LanguageUser>>();
        private static ConcurrentDictionary<Guid, Page> PageById = [];
        private static ConcurrentDictionary<(int ordinal, Guid bookId), Page> PageByOrdinalAndBookId = [];
        private static ConcurrentDictionary<Guid, List<Page>> PagesByBookId = [];
        private static ConcurrentDictionary<Guid, PageUser> PageUserById = [];
        private static ConcurrentDictionary<(Guid pageId, Guid languageUserId), PageUser> PageUserByPageIdAndLanguageUserId = [];
        private static ConcurrentDictionary<(Guid languageUserId, int ordinal, Guid bookId), PageUser> PageUserByLanguageUserIdOrdinalAndBookId = [];
        private static ConcurrentDictionary<Guid, List<PageUser>> PageUsersByBookUserId = [];
        private static ConcurrentDictionary<Guid, Paragraph> ParagraphById = [];
        private static ConcurrentDictionary<Guid, List<Paragraph>> ParagraphsByPageId = [];
        private static ConcurrentDictionary<Guid, ParagraphTranslation> ParagraphTranslationById = [];
        private static ConcurrentDictionary<Guid, List<ParagraphTranslation>> ParagraphTranslationsByParagraphId = [];
        private static ConcurrentDictionary<Guid, Sentence> SentenceById = [];
        private static ConcurrentDictionary<Guid, List<Sentence>> SentencesByPageId = [];
        private static ConcurrentDictionary<Guid, List<Sentence>> SentencesByParagraphId = [];
        private static ConcurrentDictionary<Guid, Token> TokenById = new ConcurrentDictionary<Guid, Token>();
        private static ConcurrentDictionary<Guid, List<Token>> TokensByPageId = new ConcurrentDictionary<Guid, List<Token>>();
        private static ConcurrentDictionary<Guid, List<Token>> TokensBySentenceId = new ConcurrentDictionary<Guid, List<Token>>();
        private static ConcurrentDictionary<Guid, List<Token>> TokensAndWordsBySentenceId = new ConcurrentDictionary<Guid, List<Token>>();
        private static ConcurrentDictionary<string, User> UserByApplicationUserId = new ConcurrentDictionary<string, User>();
        private static ConcurrentDictionary<Guid, Language?> UserSettingUiLanguageByUserId = new();
        private static ConcurrentDictionary<Guid, List<UserSetting>?> UserSettingsByUserId = new();
        private static ConcurrentDictionary<Guid, List<Word>> WordsCommon1000ByLanguageId = new ConcurrentDictionary<Guid, List<Word>>();
        private static ConcurrentDictionary<Guid, Word?> WordById = new ConcurrentDictionary<Guid, Word?>();
        private static ConcurrentDictionary<(Guid languageId, string textLower), Word> WordByLanguageIdAndTextLower = new ConcurrentDictionary<(Guid languageId, string textLower), Word>();
        private static ConcurrentDictionary<Guid, List<Word>> WordsBySentenceId = new ConcurrentDictionary<Guid, List<Word>>();
        private static ConcurrentDictionary<Guid, List<Word>> WordsByBookId = new ConcurrentDictionary<Guid, List<Word>>();
        private static ConcurrentDictionary<Guid, Dictionary<string, Word>> WordsDictByBookId = new ConcurrentDictionary<Guid, Dictionary<string, Word>>();
        private static ConcurrentDictionary<Guid, Dictionary<string, Word>> WordsDictByPageId = new ConcurrentDictionary<Guid, Dictionary<string, Word>>();
        private static ConcurrentDictionary<Guid, List<Word>> WordsAndTokensAndSentencesAndParagraphsByWordId = new ConcurrentDictionary<Guid, List<Word>>();
        private static ConcurrentDictionary<(Guid pageId, Guid languageUserId), Dictionary<string, Word>> WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserId = [];
        private static ConcurrentDictionary<Guid, List<WordTranslation>> WordTranslationsByWordId = [];
        private static ConcurrentDictionary<Guid, WordUser> WordUserById = new();
        private static ConcurrentDictionary<Guid, WordUser> WordUserAndLanguageUserAndLanguageById = new();
        private static ConcurrentDictionary<(Guid wordId, Guid userId), WordUser> WordUserByWordIdAndUserId = new();
        private static ConcurrentDictionary<(Guid bookId, Guid uselanguageUserIdrId), List<WordUser>> WordUsersByBookIdAndLanguageUserId = new();
        private static ConcurrentDictionary<(Guid bookId, Guid userId), Dictionary<string, WordUser>> WordUsersDictByBookIdAndUserId = new();
        private static ConcurrentDictionary<(Guid pageId, Guid userId), Dictionary<string, WordUser>> WordUsersDictByPageIdAndUserId = new();
        private static ConcurrentDictionary<(Guid userId, Guid languageId), List<WordUser>> WordUsersByUserIdAndLanguageId = new();









        public static void DeleteAllCache()
        {
            BookById = [];
            BookListRowsByUserId = [];
            BookListRowByBookIdAndUserId = [];
            BookStatByBookIdAndStatKey = [];
            BookStatsByBookId = [];
            BookTagsByBookId = [];
            BookUserById = new ConcurrentDictionary<Guid, BookUser>();
            BookUserByBookIdAndUserId = new ConcurrentDictionary<(Guid bookId, Guid userId), BookUser>();
            BookUserStatsByBookIdAndUserId = new ConcurrentDictionary<(Guid bookId, Guid userId), List<BookUserStat>>();
            FlashCardById = new ConcurrentDictionary<Guid, FlashCard?>();
            FlashCardAndFullRelationshipsById = new ConcurrentDictionary<Guid, FlashCard?>();
            FlashCardByWordUserId = new();
            FlashCardAttemptById = new ConcurrentDictionary<Guid, FlashCardAttempt?>();
            FlashCardAttemptsByFlashCardId = new ConcurrentDictionary<Guid, List<FlashCardAttempt>>();
            FlashCardParagraphTranslationBridgeById = [];
            FlashCardParagraphTranslationBridgesByFlashCardIdAndUiLanguageCode = [];
            LanguageByCode = [];
            LanguageById = [];
            LanguageUserById = new ConcurrentDictionary<Guid, LanguageUser>();
            LanguageUserByLanguageIdAndUserId = new ConcurrentDictionary<(Guid languageId, Guid userId), LanguageUser>();
            LanguageUsersAndLanguageByUserId = new ConcurrentDictionary<Guid, List<LanguageUser>>();
            PageById = [];
            PageByOrdinalAndBookId = [];
            PagesByBookId = [];
            PageUserById = [];
            PageUserByPageIdAndLanguageUserId = [];
            PageUserByLanguageUserIdOrdinalAndBookId = [];
            PageUsersByBookUserId = [];
            ParagraphById = [];
            ParagraphsByPageId = [];
            ParagraphTranslationById = [];
            ParagraphTranslationsByParagraphId = [];
            SentenceById = [];
            SentencesByPageId = [];
            SentencesByParagraphId = [];
            TokenById = new ConcurrentDictionary<Guid, Token>();
            TokensByPageId = new ConcurrentDictionary<Guid, List<Token>>();
            TokensBySentenceId = new ConcurrentDictionary<Guid, List<Token>>();
            TokensAndWordsBySentenceId = new ConcurrentDictionary<Guid, List<Token>>();
            UserByApplicationUserId = new ConcurrentDictionary<string, User>();
            UserSettingUiLanguageByUserId = new();
            UserSettingsByUserId = new();
            WordsCommon1000ByLanguageId = new ConcurrentDictionary<Guid, List<Word>>();
            WordById = new ConcurrentDictionary<Guid, Word?>();
            WordByLanguageIdAndTextLower = new ConcurrentDictionary<(Guid languageId, string textLower), Word>();
            WordsBySentenceId = new ConcurrentDictionary<Guid, List<Word>>();
            WordsByBookId = new ConcurrentDictionary<Guid, List<Word>>();
            WordsDictByBookId = new ConcurrentDictionary<Guid, Dictionary<string, Word>>();
            WordsDictByPageId = new ConcurrentDictionary<Guid, Dictionary<string, Word>>();
            WordsAndTokensAndSentencesAndParagraphsByWordId = new ConcurrentDictionary<Guid, List<Word>>();
            WordsDictWithWordUsersAndTranslationsByPageIdAndLanguageUserId = [];
            WordTranslationsByWordId = [];
            WordUserById = new();
            WordUserAndLanguageUserAndLanguageById = new();
            WordUserByWordIdAndUserId = new();
            WordUsersByBookIdAndLanguageUserId = new();
            WordUsersDictByBookIdAndUserId = new();
            WordUsersDictByPageIdAndUserId = new();
            WordUsersByUserIdAndLanguageId = new();

        }
    }
}
