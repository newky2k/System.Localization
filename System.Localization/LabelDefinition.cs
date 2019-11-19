using System;
using System.Xml.Serialization;

namespace System.Localization
{
	public class LabelDefinition
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		[XmlText]
		public string Phrase { get; set; }


		public LabelDefinition()
		{
		}
	}
}
