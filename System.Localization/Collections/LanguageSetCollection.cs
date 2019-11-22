using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace System.Localization.Collections
{
    public class LanguageSetCollection : ObservableCollection<LanguageSet>
    {

        public LanguageSet this[LanguageDefintion defintion]
        {
            get
            {
                //see if you can match the display name
                var firstLang = Items.FirstOrDefault(x => x.Language.DisplayName.Equals(defintion.DisplayName, StringComparison.OrdinalIgnoreCase));

                //then see if you can match the language code
                if (firstLang == null)
                    firstLang = Items.FirstOrDefault(x => x.Language.LanguageCode.Equals(defintion.LanguageCode, StringComparison.OrdinalIgnoreCase));

                //then see if you can matrch the sub code
                if (firstLang == null)
                    firstLang = Items.FirstOrDefault(x => x.Language.LanguageSubCode.Equals(defintion.LanguageSubCode, StringComparison.OrdinalIgnoreCase));

                return firstLang;
            }
        }

        /// <summary>
        /// Get the language set for the specified language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public LanguageSet this[Language language]
        {
            get
            {
                var first = Items.FirstOrDefault(x => x.Language.Equals(language));

                return first;
            }
        }
        public bool Contains(Language lang)
        {
            return Items.Where(x => x.Language.Equals(lang)).Any();
        }

        public bool Contains(string nameOrCodeOrSubCode)
        {
            //see if you can match the display name
            var firstLang = Items.FirstOrDefault(x => x.Language.DisplayName.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

            //then see if you can match the language code
            if (firstLang == null)
                firstLang = Items.FirstOrDefault(x => x.Language.LanguageCode.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

            //then see if you can matrch the sub code
            if (firstLang == null)
                firstLang = Items.FirstOrDefault(x => x.Language.LanguageSubCode.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

            return (firstLang != null);
        }

        public bool Contains(LanguageDefintion lang)
        {
            //see if you can match the display name
            var firstLang = Items.FirstOrDefault(x => x.Language.DisplayName.Equals(lang.DisplayName, StringComparison.OrdinalIgnoreCase));

            //then see if you can match the language code
            if (firstLang == null)
                firstLang = Items.FirstOrDefault(x => x.Language.LanguageCode.Equals(lang.LanguageCode, StringComparison.OrdinalIgnoreCase));

            //then see if you can matrch the sub code
            if (firstLang == null)
                firstLang = Items.FirstOrDefault(x => x.Language.LanguageSubCode.Equals(lang.LanguageSubCode, StringComparison.OrdinalIgnoreCase));

            return (firstLang != null);
        }
    }
}
