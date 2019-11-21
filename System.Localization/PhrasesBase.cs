using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Localization
{
    public abstract class PhrasesBase
    {
        public static event PropertyChangedEventHandler StaticPropertyChanged = delegate { };

        protected static void NotifyPhraseChanged([CallerMemberName] string phrase = null)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(phrase));
        }

    }
}
