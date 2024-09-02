using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    [Table("WordTranslation", Schema = "Idioma")]
    [PrimaryKey(nameof(UniqueKey))]
    public class WordTranslation
    {
        public Guid? UniqueKey { get; set; }


        #region relationships

        public int? LanguageToId { get; set; }
        public Language? LanguageTo { get; set; }
        public int? WordId { get; set; }
        public Word? Word { get; set; }
        public Guid? VerbKey { get; set; }
        public Verb? Verb { get; set; }

        #endregion


        [StringLength(2000)]
        public string? Translation { get; set; }
        public AvailablePartOfSpeech? PartOfSpeech { get; set; }

    }
}

