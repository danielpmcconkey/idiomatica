﻿using Microsoft.EntityFrameworkCore;
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
    [Table("BookUserStat", Schema = "Idioma")]
    [PrimaryKey(nameof(BookId), nameof(LanguageUserId), nameof(Key))]
    public class BookUserStat
    {
        #region required data

        [Required] public required Guid BookId { get; set; }
        [Required] public required Guid LanguageUserId { get; set; }
        [Required] public required AvailableBookUserStat Key { get; set; }

        #endregion

        public Book? Book { get; set; }
        public LanguageUser? LanguageUser { get; set; }

        [StringLength(250)]
        public string? ValueString { get; set; }

        [Column(TypeName ="numeric(10,4)")]
        public decimal? ValueNumeric { get; set; }
    }
}
