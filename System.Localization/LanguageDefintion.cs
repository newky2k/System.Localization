using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace System.Localization
{
	public class LanguageDefintion
	{
		#region Generator Members
		private XmlSerializerNamespaces _xmlnamespaces;

		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces XmlNamespaces
		{
			get { return this._xmlnamespaces; }
		}

		#endregion

		[XmlAttribute(AttributeName = "displayname")]
		public string DisplayName { get; set; }

		[XmlAttribute(AttributeName = "code")]
		public string LanguageCode { get; set; }

		[XmlAttribute(AttributeName = "subcode")]
		public string LanguageSubCode { get; set; }

		[XmlElement(ElementName = "label")]
		public List<LabelDefinition> Labels { get; set; } 

		/// <summary>
		/// Get the relavane label
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public LabelDefinition this[string key]
		{
			get
			{
				return Labels.FirstOrDefault(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
			}
		}

		public LanguageDefintion()
		{
			_xmlnamespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty, "urn:system") });

			Labels = new List<LabelDefinition>();
		}

		
	}
}
