using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ReadDataPacket
    {
        public Dictionary<string, Word>? AllWordsInPage { get; set; } = null;
        public Page? CurrentPage { get; set; } = null;
        public PageUser? CurrentPageUser { get; set; } = null;
        public Book? Book { get; set; } = null;
        public int BookCurrentPageNum
        {
            get
            {
                if (CurrentPage is null) return 0;
                return (int)CurrentPage.Ordinal;
            }
        }
        public string? BookTitle
        {
            get
            {
                if (Book == null) return null;
                return Book.Title;
            }
        }
        public int BookTotalPageCount { get; set; } = 0;
        public BookUser? BookUser { get; set; } = null;
        public List<BookUserStat>? BookUserStats { get; set; } = null;
        public Language? Language { get; set; } = null;
        /// <summary>
        /// the language the book is written in
        /// </summary>
        public Language? LanguageFromCode { get; set; } = null;
        /// <summary>
        /// The user's preferred UI language
        /// </summary>
        public Language? LanguageToCode { get; set; } = null;
        public LanguageUser? LanguageUser { get; set; } = null;
        public User? LoggedInUser { get; set; } = null;
        public List<Paragraph>? Paragraphs { get; set; } = null;
        public List<Sentence>? Sentences { get; set; } = null;
        public List<Token>? Tokens { get; set; } = null;
    }
}
