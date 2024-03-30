using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Word
    {
        [Column("WoID")] public int Id { get; set; }


        #region relationships
        [Column("WoLgID")] public int LanguageId { get; set; }
        public Language Language { get; set; }
        public List<Word> ParentWords { get; set; } = new List<Word>();
        public List<Word> ChildWords { get; set; } = new List<Word>();
        [Column("WoStatus")] public int StatusId { get; set; }
        public Status Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        #endregion



        [Column("WoText")] public string Text { get; set; }
        [Column("WoTextLC")] public string WoTextLC { get; set; }
        [Column("WoTranslation")] public string? WoTranslation { get; set; }
        [Column("WoRomanization")] public string? WoRomanization { get; set; }
        [Column("WoTokenCount")] public int WoTokenCount { get; set; } = 0;
        [Column("WoCreated")] public DateTime WoCreated { get; set; } = DateTime.Now;
        [Column("WoStatusChanged")] public DateTime WoStatusChanged { get; set; } = DateTime.Now;
    }
}
