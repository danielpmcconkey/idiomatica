﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public AvailableBookStat Key { get; set; }

        #region relationships
        public Book Book { get; set; }
        #endregion
        [StringLength(250)]
        public string Value { get; set; }
    }
    [Table("BookStatNew")]
    [PrimaryKey(nameof(BookId), nameof(Key))]
    public class BookStatNew
    {
        public int? BookId { get; set; }
        public AvailableBookStat Key { get; set; }

        #region relationships
        public Book Book { get; set; }
        #endregion
        [StringLength(250)]
        public string Value { get; set; }
    }
}
