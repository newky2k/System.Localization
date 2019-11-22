using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Localization;
using System.Text;
using System.Threading;

namespace ExternalLangs
{
    [PhraseProvider]
    public class Phrases : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private static string _doneButton = "Done t";
        private static string _cancelButton = "Cancel t";
        private string _otherButton = "Other t";

        public string DoneButton
        {
            get { return _doneButton; }
            set
            {
                if (_doneButton != value)
                {
                    _doneButton = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DoneButton)));
                }
            }
        }
        public string CancelButton
        {
            get { return _cancelButton; }
            set
            {
                if (_cancelButton != value)
                {
                    _cancelButton = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CancelButton)));
                }
            }
        }

        public string OtherButton
        {
            get { return _otherButton; }
            set
            {
                if (_otherButton != value)
                {
                    _otherButton = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OtherButton)));
                }
            }
        }


        public Phrases()
        {
            Localizer.Register(this);
        }
    }
}
