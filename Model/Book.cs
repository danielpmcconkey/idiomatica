using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Book
    {
        [Column("BkID")] public int Id { get; set; }

        #region relationships
        public List<Text> Texts { get; set; } 
        public List<BookTag> BookTags { get; set; }
        [Column("BkLgID")] public int LanguageId { get; set; }
        public Language Language { get; set; }
        public BookStat BookStat { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        #endregion

        [Column("BkTitle")] public string BkTitle { get; set; }
        [Column("BkSourceURI")] public string? BkSourceURI { get; set; }
        [Column("BkArchived")] public int BkArchived { get; set; } = 0;
        [Column("BkCurrentTxID")] public int BkCurrentTxID { get; set; } = 0;
        [Column("BkWordCount")] public int? BkWordCount { get; set; }
        [Column("BkAudioFilename")] public string? BkAudioFilename { get; set; }
        [Column("BkAudioCurrentPos")] public float? BkAudioCurrentPos { get; set; }
        [Column("BkAudioBookmarks")] public string? BkAudioBookmarks { get; set; }
		[Column("IsComplete")] public Int16 _IsComplete { get; set; } = 0; // sqlite can't hold a bool
		[NotMapped] public bool IsComplete { 
			get { return CheckIsComplete(); } 
			set { UpdateIsComplete(value); } 
		}
		[Column("TotalPages")] public int TotalPages { get; set; } = 0;
		[Column("LastPageRead")] public int LastPageRead { get; set; } = 0;

		private bool CheckIsComplete()
		{
			if (_IsComplete == 1) return true; 
			return false;
		}
		private void UpdateIsComplete(bool isComplete)
		{
			if(isComplete) _IsComplete = 1;
			else _IsComplete = 0;
		}
	}
	
}
