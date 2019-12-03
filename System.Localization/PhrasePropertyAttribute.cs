using System;
using System.Collections.Generic;
using System.Text;

namespace System.Localization
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PhrasePropertyAttribute : Attribute
    {
        public PhrasePropertyAttribute(string phraseKey)
        {
            PhraseKey = phraseKey;
        }

        public string PhraseKey { get; private set; }
    }
}
