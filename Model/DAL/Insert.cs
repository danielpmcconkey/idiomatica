using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static class Insert
    {
        public static void Page(IdiomaticaContext context, Page page)
        {
            if (page.Id == null)
            {
                context.Pages.Add(page);
            }
            context.SaveChanges();
        }

        public static void PageUser(IdiomaticaContext context, PageUser pageUser)
        {
            if (pageUser.Id == null)
            {
                context.PageUsers.Add(pageUser);
            }
            context.SaveChanges();
        }

        public static void Paragraph(IdiomaticaContext context, Paragraph paragraph)
        {
            if (paragraph.Id == null)
            {
                context.Paragraphs.Add(paragraph);
            }
            context.SaveChanges();
        }
        public static void Sentence(IdiomaticaContext context, Sentence sentence)
        {
            if (sentence.Id == null)
            {
                context.Sentences.Add(sentence);
            }
            context.SaveChanges();
        }
        public static void Token(IdiomaticaContext context, Token token)
        {
            if (token.Id == null)
            {
                context.Tokens.Add(token);
            }
            context.SaveChanges();
        }
        public static void Word(IdiomaticaContext context, Word word)
        {
            if (word.Id == null)
            {
                context.Words.Add(word);
            }
            context.SaveChanges();
        }
        public static void WordUser(IdiomaticaContext context, WordUser wordUser)
        {
            if (wordUser.Id == null)
            {
                context.WordUsers.Add(wordUser);
            }
            context.SaveChanges();
        }
    }
}
