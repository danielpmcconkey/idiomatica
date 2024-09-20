using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Enums;

namespace Model
{
    
    [Table("LogMessage", Schema = "dbo")]
    [PrimaryKey(nameof(Id))]
    public class LogMessage
    {
        #region required data

        [Required] public required Guid Id { get; set; }
        [Required] public required DateTimeOffset Logged {  get; set; }

        [Required] public required AvailableLogMessageTypes MessageType { get; set; }
        [Required] public required string MemberName { get; set; }
        [Required] public required string SourceFilePath { get; set; }
        [Required] public required int SourceLineNumber { get; set; }

        [StringLength(2000)]
        [Required] public required string Message { get; set; }


        #endregion
        [Column(TypeName = "TEXT")]
        public string? Detail {  get; set; }

    }
}
