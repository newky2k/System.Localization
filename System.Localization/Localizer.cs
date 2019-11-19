using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

namespace System.Localization
{
	public abstract class Localizer<T> : INotifyPropertyChanged where T : new()
	{
		#region Fields
		private T _phrases;
		private string _languagesFolder;
		private LanguageDefintion _selectedLanguage;
		private List<LanguageDefintion> _availableLanguages;

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion

		#region Properties

		/// <summary>
		/// List of available languages
		/// </summary>
		public List<LanguageDefintion> AvailableLanguages
		{
			get
			{
				//
				if (_availableLanguages == null)
					Load();

				return _availableLanguages;
			}

			private set { _availableLanguages = value; NotifyPropertiesChanged(nameof(AvailableLanguages), nameof(AvailableLanguageNames)); }
		}

		/// <summary>
		/// List of the display names of all the available languages
		/// </summary>
		public List<string> AvailableLanguageNames
		{
			get
			{
				return AvailableLanguages.Select(x => x.DisplayName).ToList();
			}
		}

		/// <summary>
		/// The currently selected Language
		/// </summary>
		public LanguageDefintion SelectedLanguge
		{
			get
		    {
				if (_selectedLanguage == null)
					_selectedLanguage = AvailableLanguages.First();

				return _selectedLanguage;
            }
			set { _selectedLanguage = value; NotifyPropertyChanged(nameof(SelectedLanguge)); ApplySelectedLanguage(); }
		}

		protected string LanguagesFolder
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_languagesFolder))
					_languagesFolder = Path.Combine(Path.GetDirectoryName(typeof(Localizer<>).Assembly.Location),"Languages");

				return _languagesFolder;
			}
			private set { _languagesFolder = value; }
		}

		public T Phrases
		{
			get
			{
				if (_phrases == null)
				{
					_phrases = new T();

					ApplySelectedLanguage();
				}
					

				return _phrases;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initialize the Localizer with an optional alternative location for the languge files
		/// </summary>
		/// <param name="languageFolder"></param>
		public void Init(string languageFolder = null)
		{
			_languagesFolder = languageFolder;

			Load();
		}


		/// <summary>
		/// Generate the language file for the specified language
		/// </summary>
		/// <param name="options">The language creation options for creating the language definition</param>
		/// <param name="outputPath">Location to store the generated files</param>
		/// <param name="includeCurrentValues">Include the current selected language values for all phrases</param>
		public void Generate(LanguageOptions options, string outputPath, bool includeCurrentValues = true)
		{
			//find all string properties
			var typeDef = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.PropertyType.Equals(typeof(string)));

			var definition = new LanguageDefintion()
			{
				DisplayName = options.DisplayName,
				LanguageCode = options.LanguageCode,
				LanguageSubCode = options.LanguageSubCode,
			};

			foreach (var aProp in typeDef)
			{
				var aLabel = aProp.Name;
				var currentValue = string.Empty;

				if (includeCurrentValues)
				{
					currentValue = aProp.GetValue(Phrases).ToString();
				}

				definition.Labels.Add(new LabelDefinition()
				{
					Name = aLabel,
					Phrase = currentValue,
				});
			}

			var fileName = Path.Combine(outputPath, $"{options.DisplayName}.xml");

			if (File.Exists(fileName))
				File.Delete(fileName);

			//serialize
			WriteToFile(definition, fileName);
		}

		/// <summary>
		/// Generate the language file for the specified languages
		/// </summary>
		/// <param name="options">List of LanuageOptions to create</param>
		/// <param name="outputPath">Location to store the generated files</param>
		/// <param name="includeCurrentValues">Include the current selected language values for all phrases</param>
		public void Generate(IEnumerable<LanguageOptions> options, string outputPath, bool includeCurrentValues = true)
		{
			foreach (var aOption in options)
			{
				Generate(aOption, outputPath, includeCurrentValues);
			}
		}
		/// <summary>
		/// Validate the language file to ensure it has all the available properties
		/// </summary>
		public void Validate()
		{

		}
		/// <summary>
		/// Set the language using the specified culture
		/// </summary>
		/// <param name="culture"></param>
		public void SetLanguage(CultureInfo culture)
		{
			var firstLang = AvailableLanguages.FirstOrDefault(x => x.LanguageSubCode.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));

			if (firstLang == null)
				firstLang = AvailableLanguages.FirstOrDefault(x => x.LanguageCode.Equals(culture.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase));

			SelectedLanguge = firstLang;

		}

		/// <summary>
		/// Set the language using a the display name, language code or language sub-code
		/// </summary>
		/// <param name="nameOrCodeOrSubCode"></param>
		public void SetLanguage(string nameOrCodeOrSubCode)
		{

			var firstLang = AvailableLanguages.FirstOrDefault(x => x.DisplayName.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

			if (firstLang == null)
				firstLang = AvailableLanguages.FirstOrDefault(x => x.LanguageCode.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

			if (firstLang == null)
				firstLang = AvailableLanguages.FirstOrDefault(x => x.LanguageSubCode.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

			SelectedLanguge = firstLang;
		}
		#region Private

		private void Load()
		{
			var langFiles = Directory.GetFiles(LanguagesFolder, "*.xml");

			if (langFiles.Count() == 0)
				throw new Exception("No languages files were found");

			var newLangs = new List<LanguageDefintion>();

			foreach (var aFile in langFiles)
			{
				try
				{
					var newDef = LoadDefinition(aFile);

					if (newDef.Labels.Count > 0)
					{
						newLangs.Add(newDef);
					}
				}
				catch (Exception)
				{
					Console.WriteLine($"cannot process file: {aFile}");
				}

			}

			_availableLanguages = newLangs;


		}

		private void WriteToFile(LanguageDefintion target, string fileName)
		{
			var serializer = new XmlSerializer(typeof(LanguageDefintion));

			var xWriterSetting = new XmlWriterSettings()
			{
				OmitXmlDeclaration = true,
				Indent = true,
			};

			//using (StreamWriter str = new StreamWriter(fileName))
			using (StringWriter textWriter = new StringWriter())
			using (XmlWriter xml = XmlWriter.Create(textWriter, xWriterSetting))
			{
				serializer.Serialize(xml, target, target.XmlNamespaces);

				var output = textWriter.ToString();
				output = output.Replace("<blank />", "");

				File.WriteAllText(fileName, output);
			}
		}

		private LanguageDefintion LoadDefinition(string inputFile)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(inputFile) || !File.Exists(inputFile))
					throw new Exception("Invalid input file");

				var serializer = new XmlSerializer(typeof(LanguageDefintion));

				var output = new LanguageDefintion();

				using (StreamReader str = new StreamReader(inputFile))
				{

					output = (LanguageDefintion)serializer.Deserialize(str);
				}

				return output;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void ApplySelectedLanguage()
		{
			if (SelectedLanguge == null)
				return;

			//find all string properties
			var typeDef = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.PropertyType.Equals(typeof(string)));

			foreach (var aProp in typeDef)
			{
				var propDef = SelectedLanguge[aProp.Name];

				if (propDef != null)
				{
					aProp.SetValue(Phrases, propDef.Phrase);
				}
				else
				{
					Console.WriteLine($"Label {aProp.Name} not found in selected language");
				}
			}

			NotifyPropertyChanged(nameof(Phrases));

		}

		private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void NotifyPropertiesChanged(params string[] properties)
		{
			foreach (var aProp in properties)
			{
				NotifyPropertyChanged(aProp);
			}
		}
		#endregion
		#endregion
	}
}
