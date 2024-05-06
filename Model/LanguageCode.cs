using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("LanguageCode", Schema = "Idioma")]
    [PrimaryKey(nameof(Code))]
    public class LanguageCode
    {
        [StringLength(25)]
        public string Code { get; set; }
        [StringLength(250)]
        public string LanguageName { get; set; }
        public List<ParagraphTranslation> ParagraphTranslations { get; set; }
        public List<User> Users { get; set; }
        public Language Language { get; set; }
    }
}
