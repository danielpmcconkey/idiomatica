using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("vw_BookListRow", Schema = "Idioma")]
    [PrimaryKey(nameof(BookId), nameof(UserId))]
    public record BookListRow
    {
        public Int64? RowNumber { get; set; }
        public bool? IsInShelf { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public string? LanguageName { get; set; }
        public string? IsComplete { get; set; }
        public string? Title { get; set; }
        public int? TotalPages { get; set; }
        public string? Progress { get; set; }
        public decimal? ProgressPercent { get; set; }
        public int? TotalWordCount { get; set; }
        public int? DistinctWordCount { get; set; }
        public decimal? DistinctKnownPercent { get; set; }
        public decimal? DifficultyScore { get; set; }
        public bool? IsArchived { get; set; }
        public string? Tags { get; set; }
    }
}
