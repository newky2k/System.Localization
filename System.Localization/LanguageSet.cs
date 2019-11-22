using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Localization
{
    public class LanguageSet
    {
        public Language Language { get; set; }

        public List<Phrase> Phrases { get; set; }

        public Phrase this[string phraseName]
        {
            get
            {
                var first = Phrases.FirstOrDefault(x => x.Name.Equals(phraseName, StringComparison.OrdinalIgnoreCase));

                return first;
            }
        }

        public LanguageSet()
        {
            Phrases = new List<Phrase>();
        }

        public LanguageSet(Language lang) : this()
        {
            Language = lang;
        }

        public LanguageSet(LanguageDefintion langDef) : this()
        {
            
        }
        public void AddPhrases(IEnumerable<LabelDefinition> phrases)
        {
            foreach (var phrase in phrases)
            {
                var existing = Phrases.FirstOrDefault(x => x.Name.Equals(phrase.Name));

                if (existing == null)
                {
                    Phrases.Add(new Phrase()
                    {
                        Name = phrase.Name,
                        Value = phrase.Phrase,
                    });
                }
            }
        }
    }
}
