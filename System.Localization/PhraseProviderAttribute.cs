using System;
using System.Collections.Generic;
using System.Text;

namespace System.Localization
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PhraseProviderAttribute :Attribute
    {
        //public PhraseProviderAttribute(Type type)
        //{
        //    ProviderType = type;
        //}

        //public Type ProviderType { get; set; }
    }
}
