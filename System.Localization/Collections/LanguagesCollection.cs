using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System.Localization.Collections
{
    public class LanguagesCollection : ObservableCollection<Language>
    {
        public Language this[LanguageDefintion defintion]
        {
            get
            {
                //see if you can match the display name
                var firstLang = Items.FirstOrDefault(x => x.DisplayName.Equals(defintion.DisplayName, StringComparison.OrdinalIgnoreCase));

                //then see if you can match the language code
                if (firstLang == null)
                    firstLang = Items.FirstOrDefault(x => x.LanguageCode.Equals(defintion.LanguageCode, StringComparison.OrdinalIgnoreCase));

                //then see if you can matrch the sub code
                if (firstLang == null)
                    firstLang = Items.FirstOrDefault(x => x.LanguageSubCode.Equals(defintion.LanguageSubCode, StringComparison.OrdinalIgnoreCase));

                return firstLang;
            }
        }
        public bool Contains(string nameOrCodeOrSubCode)
        {
            //see if you can match the display name
            var firstLang = Items.FirstOrDefault(x => x.DisplayName.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

            //then see if you can match the language code
            if (firstLang == null)
                firstLang = Items.FirstOrDefault(x => x.LanguageCode.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

            //then see if you can matrch the sub code
            if (firstLang == null)
                firstLang = Items.FirstOrDefault(x => x.LanguageSubCode.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

            return (firstLang != null);
        }

        public bool Contains(LanguageDefintion lang)
        {
            //see if you can match the display name
            var firstLang = Items.FirstOrDefault(x => x.DisplayName.Equals(lang.DisplayName, StringComparison.OrdinalIgnoreCase));

            //then see if you can match the language code
            if (firstLang == null)
                firstLang = Items.FirstOrDefault(x => x.LanguageCode.Equals(lang.LanguageCode, StringComparison.OrdinalIgnoreCase));

            //then see if you can matrch the sub code
            if (firstLang == null)
                firstLang = Items.FirstOrDefault(x => x.LanguageSubCode.Equals(lang.LanguageSubCode, StringComparison.OrdinalIgnoreCase));

            return (firstLang != null);
        }
    }
}
