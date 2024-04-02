﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Model
{
    public class Sentence
    {
        public int Id { get; set; }

        #region relationships
        public int ParagraphId { get; set; }
        public Paragraph Paragraph { get; set; }
        #endregion

        public int Order { get; set; }
        public string? Text { get; set; }
    }
}
