﻿using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;

namespace Logic
{
	

	public static class PageHelper
	{
		public static int GetWordCountOfPage(Page page, LanguageUser languageUser)
		{
			return GetWordsOfPage(page, languageUser).Length;
		}
		public static string[] GetWordsOfPage(Page page, LanguageUser languageUser)
		{
			var parser = LanguageParser.Factory.GetLanguageParser(languageUser);
			return parser.GetWordsFromPage(page);
		}
        
        /// <summary>
        ///  parses the text through the language parser and returns paragraphs, 
        ///  sentences, and tokens. it saves everything in the DB
        /// </summary>
        public static List<Paragraph> CreateParagraphsFromPageAndSave(
            IdiomaticaContext context, Page page, LanguageUser languageUser)
        {
            if(page.Id == 0)
            {
                throw new InvalidDataException("Page must have a DB ID before adding children");
            }

			List<Paragraph> paragraphs = new List<Paragraph>();

            var wordDict = WordHelper.FetchWordDictForLanguageUser(context, languageUser);

            var parser = LanguageParser.Factory.GetLanguageParser(languageUser);
			var paragraphSplits = parser.SplitTextIntoParagraphs(page.OriginalText);

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
				var sentenceSplits = parser.SplitTextIntoSentences(pText);
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
                        context, newSentence, languageUser, wordDict);
                    paragraph.Sentences.Add(newSentence);
                }
				paragraphs.Add(paragraph);
			}
			return paragraphs;
		}

        
        

        public static void RepairPage(IdiomaticaContext context, 
            Page page, LanguageUser languageUser)
        {
            // delete any existing paragraphs
            DeleteParagraphsFromPage(context, page);

            // build them back
            page.Paragraphs = CreateParagraphsFromPageAndSave(context, page, languageUser);
        }
        public static void DeleteParagraphsFromPage(IdiomaticaContext context, Page page)
        {
            var existingParagraphs = Fetch.Paragraphs(context, (p => p.PageId == page.Id));
            if (existingParagraphs != null)
            {
                foreach (var paragraph in existingParagraphs)
                {
                    context.Paragraphs.Remove(paragraph);
                }
                context.SaveChanges();
            }
        }
    }
}
