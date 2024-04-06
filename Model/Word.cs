using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Word")]
    [Index(nameof(LanguageUserId), nameof(TextLowerCase), IsUnique = true)]
    public class Word
    {
        public int? Id { get; set; }


        #region relationships

        public int LanguageUserId { get; set; }
        public LanguageUser LanguageUser { get; set; }
        public List<Word> ParentWords { get; set; } = new List<Word>();
        public List<Word> ChildWords { get; set; } = new List<Word>();
        public AvailableStatus Status { get; set; }
        public List<Token> Tokens { get; set; } = new List<Token>();

        #endregion


        [StringLength(250)]
        public string Text { get; set; }
        [StringLength(250)]
        public string TextLowerCase { get; set; }
        [StringLength(2000)]
        public string? Translation { get; set; }
        [StringLength(250)]
        public string? Romanization { get; set; }
        public int TokenCount { get; set; } = 0; // todo: understand token vs word in original lute and make a multi-word learning phrase in idiomatica
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime StatusChanged { get; set; } = DateTime.Now;
    }
}
