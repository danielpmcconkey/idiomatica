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
        public static void Page(Page page)
        {
            using var context = new IdiomaticaContext();
            {
                if (page.Id == null)
                {
                    context.Pages.Add(page);
                }
                context.SaveChanges();
            }
        }
        public static void Paragraph(Paragraph paragraph)
        {
            using var context = new IdiomaticaContext();
            {
                if (paragraph.Id == null)
                {
                    context.Paragraphs.Add(paragraph);
                }
                context.SaveChanges();
            }
        }
        public static void Sentence(Sentence sentence)
        {
            using var context = new IdiomaticaContext();
            {
                if (sentence.Id == null)
                {
                    context.Sentences.Add(sentence);
                }
                context.SaveChanges();
            }
        }
        public static void Token(Token token)
        {
            using var context = new IdiomaticaContext();
            {
                if (token.Id == null)
                {
                    context.Tokens.Add(token);
                }
                context.SaveChanges();
            }
        }
        public static void Word(Word word)
        {
            using var context = new IdiomaticaContext();
            {
                if (word.Id == null)
                {
                    context.Words.Add(word);
                }
                context.SaveChanges();
            }
        }
    }
}
