﻿namespace PragmaticSegmenterNet.Languages
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal class KazakhLanguage : LanguageBase
    {
        private static readonly IReadOnlyList<string> AbbrevationStore = new[]
        {
            "afp", "anp", "atp", "bae", "bg", "bp", "cam", "cctv", "cd", "cez", "cgi", "cnpc", "farc", "fbi", "eiti", "epo", "er", "gp", "gps", "has", "hiv", "hrh", "http", "icu", "idf", "imd", "ime", "icu", "idf", "ip", "iso", "kaz", "kpo", "kpa",
            "kz", "kz", "mri", "nasa", "nba", "nbc", "nds", "ohl", "omlt", "ppm", "pda", "pkk", "psm", "psp", "raf", "rss", "rtl", "sas", "sme", "sms", "tnt", "udf", "uefa", "usb", "utc", "x", "zdf", "әқбк", "әқбк", "аақ", "авг.", "aбб", "аек", "ак",
            "ақ", "акцион.", "акср", "ақш", "англ", "аөсшк", "апр", "м.", "а.", "р.", "ғ.", "апр.", "аум.", "ацат", "әч", "т. б.", "б. з. б.", "б. з. б.", "б. з. д.", "б. з. д.", "биікт.", "б. т.", "биол.", "биохим", "бө", "б. э. д.", "бта", "бұұ",
            "вич", "всоонл", "геогр.", "геол.", "гленкор", "гэс", "қк", "км", "г", "млн", "млрд", "т", "ғ. с.", "ғ.", "қ.", "ғ.", "дек.", "днқ", "дсұ", "еақк", "еқыұ", "ембімұнайгаз", "ео", "еуразэқ", "еуроодақ", "еұу", "ж.", "ж.", "жж.", "жоо",
            "жіө", "жсдп", "жшс", "іім", "инта", "исаф", "камаз", "кгб", "кеу", "кг", "км²", "км²", "км³", "км³", "кимеп", "кср", "ксро", "кокп", "кхдр", "қазатомпром", "қазкср", "қазұу", "қазмұнайгаз", "қазпошта", "қазтаг", "қазұу", "қкп", "қмдб",
            "қр", "қхр", "лат.", "м²", "м²", "м³", "м³", "магатэ", "май.", "максам", "мб", "мвт", "мемл", "м", "мсоп", "мтк", "мыс.", "наса", "нато", "нквд", "нояб.", "обл.", "огпу", "окт.", "оңт.", "опек", "оеб", "өзенмұнайгаз", "өф", "пәк", "пед.",
            "ркфср", "рнқ", "рсфср", "рф", "свс", "сву", "сду", "сес", "сент.", "см", "снпс", "солт.", "солт.", "сооно", "ссро", "сср", "ссср", "ссс", "сэс", "дк", "т. б.", "т", "тв", "тереңд.", "тех.", "тжқ", "тмд", "төм.", "трлн", "тр", "т.", "и.",
            "м.", "с.", "ш.", "т.", "т. с. с.", "тэц", "уаз", "уефа", "еқыұ", "ұқк", "ұқшұ", "февр.", "фққ", "фсб", "хим.", "хқко", "шұар", "шыұ", "экон.", "экспо", "цтп", "цас", "янв.", "dvd", "жкт", "ққс", "км", "ацат", "юнеско", "ббс", "mgm",
            "жск", "зоо", "бсн", "өұқ", "оар", "боак", "эөкк", "хтқо", "әөк", "жэк", "хдо", "спбму", "аф", "сбд", "амт", "гсдп", "гсбп", "эыдұ", "нұсжп", "шыұ", "жтсх", "хдп", "эқк", "фкққ", "пиқ", "өгк", "мбф", "маж", "кота", "тж", "ук", "обб",
            "сбл", "жхл", "кмс", "бмтрк", "жққ", "бхооо", "мқо", "ржмб", "гулаг", "жко", "еэы", "еаэы", "кхдр", "рфкп", "рлдп", "хвқ", "мр", "мт", "кту", "ртж", "тим", "мемдум", "ксро", "т.с.с", "с.ш.", "ш.б.", "б.б.", "руб", "мин", "акад.", "ғ.",
            "мм", "мм."
        };

        public override IReadOnlyList<string> Abbreviations { get; } = AbbrevationStore;

        public override IReadOnlyList<string> SentenceStarters { get; } = Empty;

        public override IReadOnlyList<string> PrepositiveAbbreviations { get; } = Empty;

        public override IReadOnlyList<string> NumberAbbreviations { get; } = Empty;

        public override Regex MultiPeriodAbbreviationRegex { get; } = new Regex(@"\b\p{IsCyrillic}(?:\.\s?\p{IsCyrillic})+[.]|b[a-z](?:\.[a-z])+[.]");

        public override ICleanerBehaviour CleanerBehaviour { get; } = new KazakhCleanerBehaviour();

        public override IReadOnlyList<string> CleanedAbbreviations { get; } = Empty;

        public override IAbbreviationReplacer AbbreviationReplacer { get; }

        public KazakhLanguage()
        {
            AbbreviationReplacer = new KazakhAbbreviationReplacer(this);
        }

        private class KazakhCleanerBehaviour : ICleanerBehaviour
        {
            public Regex NoSpaceBetweenSentencesRegex { get; } = new Regex(@"(?<=[a-z\p{IsCyrillic}])\.(?=[A-ZЁА-Я][^\.]+)");
            public Rule NoSpaceBetweenSentencesRule { get; }

            public KazakhCleanerBehaviour()
            {
                NoSpaceBetweenSentencesRule = new Rule(NoSpaceBetweenSentencesRegex, ". ");
            }

            public string OnClean(string text)
            {
                return text;
            }
        }

        private class KazakhAbbreviationReplacer : AbbreviationReplacerBase
        {
            private static readonly Rule SingleUpperCaseCyrillicLetterAtStartOfLineRule = new Rule(@"(?<=^[А-ЯЁ])\.(?=\s)", "∯");
            private static readonly Rule SingleUpperCaseCyrillicLetterRule = new Rule(@"(?<=\s[А-ЯЁ])\.(?=\s)", "∯");

            public KazakhAbbreviationReplacer(ILanguage language) : base(language)
            {
            }

            public override string Replace(string text)
            {
                var result = base.Replace(text);
                result = SingleUpperCaseCyrillicLetterAtStartOfLineRule.Apply(result);
                result = SingleUpperCaseCyrillicLetterRule.Apply(result);

                return result;
            }
        }
    }
}
