using System;
using System.Collections.Generic;
using System.Text;

namespace System.Localization
{
    public class PhraseNotFoundException : Exception
    {
        public PhraseNotFoundException(string message) : base(message) 
        {

        }
    }
}
