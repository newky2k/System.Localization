using System;
using System.Collections.Generic;
using System.Localization;
using System.Text;
using System.Threading;

namespace ExternalLangs
{
    [PhraseProvider]
    public class Phrases
    {
        private static string _doneButton = "Done t";
        private static string _cancelButton = "Cancel t";


        public static event EventHandler DoneButtonChanged;
        public static string DoneButton
        {
            get { return _doneButton; }
            set 
            {
                _doneButton = value; DoneButtonChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static event EventHandler CancelButtonChanged;
        public static string CancelButton
        {
            get { return _cancelButton; }
            set { _cancelButton = value; CancelButtonChanged?.Invoke(null, EventArgs.Empty); }
        }


    }
}
