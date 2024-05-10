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
    /// <summary>
    /// this enum is used to prevent mistakes due to using strings to represent 
    /// langages. It maps to the codes on DeepL at 
    /// https://developers.deepl.com/docs/api-reference/languages . However, 
    /// DeepL uses hyphens that can't be used in an enum value. So we 
    /// substitute hyphens for underscores.
    /// </summary>
    public enum LanguageCodeEnum {
        BG,             // Bulgarian
        CS,             // Czech
        DA,             // Danish
        DE,             // German
        EL,             // Greek
        ENG_GB,         // English (British)
        ENG_US,         // English (American)
        ES,             // Spanish
        ET,             // Estonian
        FI,             // Finish
        FR,             // French
        HU,             // Hungarian
        ID,             // Indonesian
        IT,             // Italian
        JA,             // Japanese
        KO,             // Korean
        LT,             // Lithuanian
        LV,             // Latvian
        NB,             // Norwegian (Bokmål)
        NL,             // Dutch
        PL,             // Polish
        PT_BR,          // Portuguese (Brazilian)
        PT_PT,          // Portuguese (European)
        RO,             // Romanian
        RU,             // Russian
        SK,             // Slovak
        SL,             // Slovenian
        SV,             // Swedish
        TR,             // Turkish
        UK,             // Ukrainian
        ZH,             // Chinese (simplified)
    }

    [Table("LanguageCode", Schema = "Idioma")]
    [PrimaryKey(nameof(Code))]
    public class LanguageCode
    {
        [StringLength(25)]
        public string Code { get; set; }

        /// <summary>
        /// LanguageCodeEnum is used to convert between the DeepL codes (which 
        /// are also the key in the database) to the LanguageCodeEnum 
        /// enumerations
        /// </summary>
        [NotMapped]
        public LanguageCodeEnum LanguageCodeEnum { get {
                string enumName = Code.Replace('-', '_');
                LanguageCodeEnum outVal = LanguageCodeEnum.ENG_US;
                Enum.TryParse<LanguageCodeEnum>(enumName, out outVal);
                return outVal;
            } 
        }
        [StringLength(250)]
        public string LanguageName { get; set; }
        public List<ParagraphTranslation> ParagraphTranslations { get; set; }
        public List<User> Users { get; set; }
        public Language Language { get; set; }
    }
}
