using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Localization
{
    public abstract class PhrasesBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void NotifyPhraseChanged([CallerMemberName] string phrase = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(phrase));
        }

    }
}
