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
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public string? LanguageName { get; set; }
        public string? IsComplete { get; set; }
        public string? Title { get; set; }
        public string? Progress { get; set; }
        public decimal? ProgressPercent { get; set; }
        public decimal? TotalWordCount { get; set; }
        public decimal? TotalKnownPercent { get; set; }
        public decimal? DistinctWordCount { get; set; }
        public decimal? DistinctKnownPercent { get; set; }
    }
}
