using DeepL;
using Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataPopulator
{
    
    internal class LanguagesToPopulate
    {
        internal static (
            string Name,
            AvailableLanguageCode Code,
            string ParserType,
            bool IsImplementedForLearning,
            bool IsImplementedForUI,
            bool IsImplementedForTranslation
            )[] languages = [
                ("Spanish", AvailableLanguageCode.ES, "spacedel", true, false, false),
                ("English (American)", AvailableLanguageCode.EN_US, "spacedel", true, true, true),
                ("English (British)", AvailableLanguageCode.EN_GB, "spacedel", true, true, true),
                ("Bulgarian", AvailableLanguageCode.BG , "spacedel", false, false, false),
                ("Czech", AvailableLanguageCode.CS , "spacedel", false, false, false),
                ("Danish", AvailableLanguageCode.DA , "spacedel", false, false, false),
                ("German", AvailableLanguageCode.DE , "spacedel", false, false, false),
                ("Greek", AvailableLanguageCode.EL , "spacedel", false, false, false),
                ("Estonian", AvailableLanguageCode.ET , "spacedel", false, false, false),
                ("Finish", AvailableLanguageCode.FI , "spacedel", false, false, false),
                ("French", AvailableLanguageCode.FR , "spacedel", false, false, false),
                ("Hungarian", AvailableLanguageCode.HU , "spacedel", false, false, false),
                ("Indonesian", AvailableLanguageCode.ID , "", false, false, false),
                ("Italian", AvailableLanguageCode.IT , "spacedel", false, false, false),
                ("Japanese", AvailableLanguageCode.JA , "", false, false, false),
                ("Korean", AvailableLanguageCode.KO , "", false, false, false),
                ("Lithuanian", AvailableLanguageCode.LT , "spacedel", false, false, false),
                ("Latvian", AvailableLanguageCode.LV , "spacedel", false, false, false),
                ("Norwegian (Bokmål)", AvailableLanguageCode.NB , "spacedel", false, false, false),
                ("Dutch", AvailableLanguageCode.NL , "spacedel", false, false, false),
                ("Polish", AvailableLanguageCode.PL , "spacedel", false, false, false),
                ("Portuguese (Brazilian)", AvailableLanguageCode.PT_BR , "spacedel", false, false, false),
                ("Portuguese (European)", AvailableLanguageCode.PT_PT , "spacedel", false, false, false),
                ("Romanian", AvailableLanguageCode.RO , "spacedel", false, false, false),
                ("Russian", AvailableLanguageCode.RU , "spacedel", false, false, false),
                ("Slovak", AvailableLanguageCode.SK , "spacedel", false, false, false),
                ("Slovenian", AvailableLanguageCode.SL , "spacedel", false, false, false),
                ("Swedish", AvailableLanguageCode.SV , "spacedel", false, false, false),
                ("Turkish", AvailableLanguageCode.TR , "spacedel", false, false, false),
                ("Ukrainian", AvailableLanguageCode.UK , "spacedel", false, false, false),
                ("Chinese (simplified)", AvailableLanguageCode.ZH , "", false, false, false),
            ];
    }
}
