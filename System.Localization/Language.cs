using System;
using System.Collections.Generic;
using System.Text;

namespace System.Localization
{
    public class Language
    {
        public string DisplayName { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageSubCode { get; set; }

        public Language()
        {

        }

        public Language(LanguageDefintion langDef)
        {
            DisplayName = langDef.DisplayName;
            LanguageCode = langDef.LanguageCode;
            LanguageSubCode = langDef.LanguageSubCode;
        }
    }
}
