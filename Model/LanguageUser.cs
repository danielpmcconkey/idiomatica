using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class LanguageUser
    {
        public int Id { get; set; }

        #region relationships
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Book> Books { get; set; }
        public List<Word> Words { get; set; } 

        #endregion

        public int TotalWordsRead { get; set; } = 0;
    }
}
