using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;

namespace Logic
{
	

	public static class PageHelper
	{
        /// <summary>
        /// parses the text through the language parser and returns paragraphs, 
        /// sentences, and tokens. it saves everything in the DB
        /// </summary>
        public static List<Paragraph> ParseParagraphsFromPageAndSave(
            IdiomaticaContext context, Page page, Language language, Dictionary<string, Word> commonWordDict)
        {

            if(page == null) throw new ArgumentNullException("page cannot be null when parsing paragraphs");
            if (page.Id == 0)
            {
                throw new InvalidDataException("Page must have a DB ID before adding children");
            }
            List<Paragraph> paragraphs = new List<Paragraph>();
            var parser = LanguageParser.Factory.GetLanguageParser(language);
            var paragraphSplits = parser.SegmentTextByParagraphs(page.OriginalText);

            int paragraphOrder = 0;
            foreach (var pText in paragraphSplits)
            {
                if (pText == string.Empty) continue;
                Paragraph paragraph = new Paragraph()
                {
                    Ordinal = paragraphOrder,
                    PageId = (int)page.Id
                };
                Insert.Paragraph(context, paragraph);
                paragraph.Page = page;

                paragraphOrder++;
                paragraph.Sentences = new List<Sentence>();
                var sentenceSplits = parser.SegmentTextBySentences(pText);
                for (int i = 0; i < sentenceSplits.Length; i++)
                {
                    var sentenceSplit = sentenceSplits[i];

                    var newSentence = new Sentence()
                    {
                        ParagraphId = (int)paragraph.Id,
                        Text = sentenceSplit,
                        Ordinal = i,
                    };
                    Insert.Sentence(context, newSentence);
                    newSentence.Paragraph = paragraph;


                    newSentence.Tokens = SentenceHelper.CreateTokensFromSentenceAndSave(
                        context, newSentence, language, commonWordDict);
                    paragraph.Sentences.Add(newSentence);
                }
                paragraphs.Add(paragraph);
            }
            return paragraphs;
        }
        public static PageUser CreatePageUserAndSave(IdiomaticaContext context,
            Page page, BookUser bookUser, Dictionary<string, Word> commonWordDict,
            Dictionary<string, WordUser> allWordUsersInLanguage)
        {
            if (page == null) throw new ArgumentNullException("page cannot be null when creating new PageUser");
            if (page.Id == 0) throw new ArgumentException("page ID cannot be 0 when creating new PageUser");
            if (bookUser == null) throw new ArgumentNullException("bookUser cannot be null when creating new PageUser");
            if (bookUser.LanguageUser == null) throw new ArgumentNullException("LanguageUser cannot be null when creating new PageUser");
            if (bookUser.LanguageUser.Language == null) throw new ArgumentNullException("language cannot be null when creating new PageUser");
            if (commonWordDict == null) throw new ArgumentNullException("commonWordDict cannot be null when creating new PageUser");

            // has the page ever been parsed?
            if (page.Paragraphs == null)
            {
                page.Paragraphs = ParseParagraphsFromPageAndSave(
                    context, page, bookUser.LanguageUser.Language, commonWordDict);
            }
            var pageUser = new PageUser() { BookUser = bookUser,
                BookUserId = bookUser.Id,
                Page = page,
                PageId = (int)page.Id
            };
            Insert.PageUser(context, pageUser);

            // now we need to add wordusers as needed
            foreach(var kvp in commonWordDict)
            {
                if (allWordUsersInLanguage.ContainsKey(kvp.Key)) continue;

                // need to create
                var newWordUser = WordHelper.CreateAndSaveUnknownWordUser(context,
                    bookUser.LanguageUser, kvp.Value);
                // and add to the dict
                allWordUsersInLanguage[kvp.Key] = newWordUser;
            }
            context.SaveChanges();
            return pageUser;
        }
        
        
        //public static int GetWordCountOfPage(Page page, LanguageUser languageUser)
        //{
        //	return GetWordsOfPage(page, languageUser).Length;
        //}
        //public static string[] GetWordsOfPage(Page page, LanguageUser languageUser)
        //{
        //	var parser = LanguageParser.Factory.GetLanguageParser(languageUser);
        //	return parser.GetWordsFromPage(page);
        //}

        //      










    //}




    //      public static void RepairPage(IdiomaticaContext context, 
    //          Page page, LanguageUser languageUser)
    //      {
    //          // delete any existing paragraphs
    //          DeleteParagraphsFromPage(context, page);

    //          // build them back
    //          page.Paragraphs = CreateParagraphsFromPageAndSave(context, page, languageUser);
    //      }
    //      public static void DeleteParagraphsFromPage(IdiomaticaContext context, Page page)
    //      {
    //          var existingParagraphs = Fetch.Paragraphs(context, (p => p.PageId == page.Id));
    //          if (existingParagraphs != null)
    //          {
    //              foreach (var paragraph in existingParagraphs)
    //              {
    //                  context.Paragraphs.Remove(paragraph);
    //              }
    //              context.SaveChanges();
    //          }
    //      }
}
}
