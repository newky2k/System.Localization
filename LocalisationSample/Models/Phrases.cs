using System;
using System.Collections.Generic;
using System.Localization;
using System.Text;
using System.Threading;

namespace LocalisationSample
{
    public class Phrases : PhrasesBase
    {
        private static string _doneButton = "Done t";
        private static string _cancelButton = "Cancel t";

        public static string DoneButton
        {
            get { return _doneButton; }
            set { _doneButton = value; NotifyPhraseChanged(); }
        }


        public static string CancelButton
        {
            get { return _cancelButton; }
            set { _cancelButton = value; NotifyPhraseChanged(); }
        }

        static Phrases()
        {
            Localizer.Current.Register<Phrases>();
        }
    }
}
