using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Paragraph")]
    public class Paragraph
	{
        public int? Id { get; set; }

        #region relationships
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();
        public int PageId { get; set; }
        public Page Page { get; set; } 
        #endregion
        
        public int Ordinal { get; set; }
	}
}
