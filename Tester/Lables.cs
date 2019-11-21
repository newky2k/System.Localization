using System;
using System.ComponentModel;
using System.Localization;

namespace Tester
{
	public class Lables : PhrasesBase
	{
        private string _doneButton = "Done t";
        private string _cancelButton = "Cancel t";

        public string DoneButton
        {
            get { return _doneButton; }
            set { _doneButton = value; NotifyPhraseChanged(); }
        }

        
        public string CancelButton
        {
            get { return _cancelButton; }
            set { _cancelButton = value; NotifyPhraseChanged(); }
        }


    }
}
