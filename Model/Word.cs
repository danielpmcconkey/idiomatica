using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Word")]
    public class Word
    {
        public int Id { get; set; }


        #region relationships

        public int LanguageUserId { get; set; }
        public LanguageUser LanguageUser { get; set; }
        public List<Word> ParentWords { get; set; } = new List<Word>();
        public List<Word> ChildWords { get; set; } = new List<Word>();
        public int StatusId { get; set; }
        public Status Status { get; set; }
        
        #endregion



        public string Text { get; set; }
        public string TextLowerCase { get; set; }
        public string? Translation { get; set; }
        public string? Romanization { get; set; }
        public int TokenCount { get; set; } = 0; // todo: understand token vs word
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime StatusChanged { get; set; } = DateTime.Now;
    }
}
