using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("BookStat")]
    [PrimaryKey(nameof(BookId), nameof(Key))]
    public class BookStat
    {
        public int? BookId { get; set; }
		public string Key { get; set; }

        #region relationships
        public Book Book { get; set; }
        #endregion
		public string Value { get; set; }

        //#region old columns todo: remove old columns
        //[Column("wordcount")] public int? wordcount { get; set; }
        //[Column("distinctterms")] public int? distinctterms { get; set; }
        //[Column("distinctunknowns")] public int? distinctunknowns { get; set; }
        //[Column("unknownpercent")] public int? unknownpercent { get; set; }
        //#endregion


        //#region columns added by me
        //[Column("totalwordCount")] public int? TotalWordCount { get; set; }
        //[Column("totalunknownCount")] public int? totalunknownCount { get; set; }
        //[Column("totalnew1Count")] public int? totalnew1Count { get; set; }
        //[Column("totalnew2Count")] public int? totalnew2Count { get; set; }
        //[Column("totallearning3Count")] public int? totallearning3Count { get; set; }
        //[Column("totallearning4Count")] public int? totallearning4Count { get; set; }
        //[Column("totallearnedCount")] public int? totallearnedCount { get; set; }
        //[Column("totalwellknownCount")] public int? totalwellknownCount { get; set; }
        //[Column("totalignoredCount")] public int? totalignoredCount { get; set; }
        //[Column("totalknownPercent")] public int? TotalKnownPercent { get; set; }
        //[Column("distinctwordCount")] public int? DistinctWordCount { get; set; }
        //[Column("distinctunknownCount")] public int? distinctunknownCount { get; set; }
        //[Column("distinctnew1Count")] public int? distinctnew1Count { get; set; }
        //[Column("distinctnew2Count")] public int? distinctnew2Count { get; set; }
        //[Column("distinctlearning3Count")] public int? distinctlearning3Count { get; set; }
        //[Column("distinctlearning4Count")] public int? distinctlearning4Count { get; set; }
        //[Column("distinctlearnedCount")] public int? distinctlearnedCount { get; set; }
        //[Column("distinctwellknownCount")] public int? distinctwellknownCount { get; set; }
        //[Column("distinctignoredCount")] public int? distinctignoredCount { get; set; }
        //[Column("distinctknownPercent")] public int? DistinctKnownPercent { get; set; }
        //#endregion
    }
}
