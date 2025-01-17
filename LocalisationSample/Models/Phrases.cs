﻿using System;
using System.Collections.Generic;
using System.Localization;
using System.Text;
using System.Threading;

namespace LocalisationSample
{
    [PhraseProvider]
    public class Phrases
    {
        private static string _doneButton = "Done t";
        private static string _cancelButton = "Cancel t";
        private static string _otherButton = "Other t";


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

        public static event EventHandler OtherButtonChanged;
        public static string OtherButton
        {
            get { return _otherButton; }
            set { _otherButton = value; OtherButtonChanged?.Invoke(null, EventArgs.Empty); }
        }

        public Phrases()
        {
            
        }

    }
}
