using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Localization.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace System.Localization
{
	public static class Localizer
	{
        //private static Lazy<Localizer> _current = new Lazy<Localizer>(() => new Localizer());

        //public static Localizer Current
        //{
        //    get { return _current.Value;}
        //}

        #region Fields
        private const string defaultLang = "en";
		private static string _languagesFolder;
		private static Language _selectedLanguage;


        private static LanguagesCollection _languages = new LanguagesCollection();
        private static LanguageSetCollection _langSets = new LanguageSetCollection();

        private static List<Type> _phrasesTypes = new List<Type>();

        #endregion

        #region Properties

        public static LanguagesCollection AvailableLanguages
        {
            get 
            {
                if (_languages.Count == 0)
                {
                    Load();

                    ApplySelectedLanguage();
                }

                return _languages; 
            }
            set { _languages = value; }
        }

		/// <summary>
		/// List of the display names of all the available languages
		/// </summary>
		public static List<string> AvailableLanguageNames
		{
			get
			{
				return AvailableLanguages.Select(x => x.DisplayName).ToList();
			}
		}

        public static event EventHandler SelectedLangugeChanged;
        /// <summary>
        /// The currently selected Language
        /// </summary>
        public static Language SelectedLanguge
		{
			get
		    {
				if (_selectedLanguage == null)
                {
                    _selectedLanguage = AvailableLanguages.FirstOrDefault(x => x.LanguageCode.Equals(defaultLang, StringComparison.OrdinalIgnoreCase));

                    if (_selectedLanguage == null)
                        _selectedLanguage = AvailableLanguages.FirstOrDefault();
                }
					
				return _selectedLanguage;
            }
			set 
            { 
                _selectedLanguage = value;
                SelectedLangugeChanged?.Invoke(null, EventArgs.Empty);
                ApplySelectedLanguage(); }
		}

		private static string LanguagesFolder
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_languagesFolder))
					_languagesFolder = Path.Combine(Path.GetDirectoryName(typeof(Localizer).Assembly.Location),"Languages");

				return _languagesFolder;
			}
			set { _languagesFolder = value; }
		}

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the Localizer with an optional alternative location for the languge files
        /// </summary>
        /// <param name="languageFolder"></param>
        public static void Init(string languageFolder = null)
		{
			_languagesFolder = languageFolder;

			Load();

            ApplySelectedLanguage();
        }

        /// <summary>
        /// Generate the language file for the specified language
        /// </summary>
        /// <param name="options">The language creation options for creating the language definition</param>
        /// <param name="outputPath">Location to store the generated files</param>
        /// <param name="includeCurrentValues">Include the current selected language values for all phrases</param>
        public static void Generate<T>(LanguageOptions options, string outputPath, bool includeCurrentValues = true)
        { 

			//find all string properties
			var typeDef = typeof(T).GetProperties(BindingFlags.Static | BindingFlags.Public).Where(x => x.PropertyType.Equals(typeof(string)));

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
                    currentValue = aProp.GetValue(null).ToString();
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
        public static void Generate<T>(IEnumerable<LanguageOptions> options, string outputPath, bool includeCurrentValues = true)
        {
			foreach (var aOption in options)
			{
				Generate<T>(aOption, outputPath, includeCurrentValues);
			}
		}
        /// <summary>
        /// Validate the language file to ensure it has all the available properties
        /// </summary>
        public static void Validate()
		{

		}
        /// <summary>
        /// Set the language using the specified culture
        /// </summary>
        /// <param name="culture"></param>
        public static void SetLanguage(CultureInfo culture)
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
        public static void SetLanguage(string nameOrCodeOrSubCode)
		{
            //see if you can match the display name
			var firstLang = AvailableLanguages.FirstOrDefault(x => x.DisplayName.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

            //then see if you can match the language code
			if (firstLang == null)
				firstLang = AvailableLanguages.FirstOrDefault(x => x.LanguageCode.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

            //then see if you can matrch the sub code
			if (firstLang == null)
				firstLang = AvailableLanguages.FirstOrDefault(x => x.LanguageSubCode.Equals(nameOrCodeOrSubCode, StringComparison.OrdinalIgnoreCase));

            //if still not found and it includes a hyphen find the first matching languge
            if (firstLang == null && nameOrCodeOrSubCode.Contains("-"))
            {
                var langCode = nameOrCodeOrSubCode.Substring(0, nameOrCodeOrSubCode.IndexOf("-"));

                firstLang = AvailableLanguages.FirstOrDefault(x => x.LanguageCode.Equals(langCode, StringComparison.OrdinalIgnoreCase));
            }
 
            SelectedLanguge = firstLang;
		}

        public static void Register<T>()
        {
            var rType = typeof(T);

            if (_phrasesTypes.Contains(rType))
                _phrasesTypes.Add(rType);

            ApplySelectedLanguage();
        }

        #region Private

        private static void Load()
		{            
            LoadPhraseProviders();

            //load embedded resources first
            LoadResources();

            //load any external files
            LoadFiles();
        }

        private static void WriteToFile(LanguageDefintion target, string fileName)
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

        /// <summary>
        /// Loads the phrase providers.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void LoadPhraseProviders()
        {
            
            var asm = Assembly.GetEntryAssembly();

            //Find items in the main app assembly
            var types = asm.GetTypes();
            foreach (var aType in types)
            {
                var attrs = aType.GetCustomAttributes<PhraseProviderAttribute>();

                if (attrs.Any())
                {
                    if (!_phrasesTypes.Contains(aType))
                        _phrasesTypes.Add(aType);
                }

            }

            //find a External Lang providers
            var attts = asm.GetCustomAttributes<ExternalPhraseProviderAttribute>();

            if (attts.Any())
            {
                foreach (var attr in attts)
                {
                    var aType = attr.ProviderType;

                    if (!_phrasesTypes.Contains(aType))
                        _phrasesTypes.Add(aType);
                }
            }



        }

        /// <summary>
        /// Load files from the assembly
        /// </summary>
        /// <param name="langs"></param>
        private static void LoadResources()
        {
            var curAssms = new List<Assembly>();

            foreach (var aTtpe in _phrasesTypes)
            {
                var curAsm = aTtpe.Assembly;

                //check to see if the assembly has already been loaded
                if (!curAssms.Contains(curAsm))
                {
                    var resources = curAsm.GetManifestResourceNames().Where(x => x.ToLower().Contains("languages")).ToList();

                    if (resources.Any())
                    {
                        foreach (var resource in resources)
                        {
                            var langStream = curAsm.GetManifestResourceStream(resource);

                            var newLang = LoadDefinition(langStream);

                            AddLanguage(newLang);
                        }
                    }

                    curAssms.Add(curAsm);
                }

            }


        }

        private static void LoadFiles()
        {
            if (!Directory.Exists(LanguagesFolder))
                return;
            var langFiles = Directory.GetFiles(LanguagesFolder, "*.xml");

            if (langFiles.Count() == 0)
                return;


            foreach (var aFile in langFiles)
            {
                try
                {
                    var newDef = LoadDefinition(aFile);

                    if (newDef.Labels.Count > 0)
                    {

                        AddLanguage(newDef);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"cannot process file: {aFile}");
                }

            }
 
        }

        private static void AddLanguage(LanguageDefintion langDef)
        {
            if (_langSets.Contains(langDef))
            {
                var langSet = _langSets[langDef];

                langSet.AddPhrases(langDef.Labels);
            }
            else
            {
                if (!_languages.Contains(langDef))
                    _languages.Add(new Language(langDef));

                var langSet = new LanguageSet(_languages[langDef]);

                langSet.AddPhrases(langDef.Labels);

                _langSets.Add(langSet);
            }
        }

        private static LanguageDefintion LoadDefinition(string inputFile)
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

        private static LanguageDefintion LoadDefinition(Stream stream)
        {
            try
            {

                var serializer = new XmlSerializer(typeof(LanguageDefintion));

                var output = new LanguageDefintion();

                output = (LanguageDefintion)serializer.Deserialize(stream);

                return output;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ApplySelectedLanguage()
		{
			if (SelectedLanguge == null)
				return;

            if (_phrasesTypes == null || _phrasesTypes.Count == 0)
                return;

            var langSet = _langSets[SelectedLanguge];

            foreach (var phrasesType in _phrasesTypes)
            {
                //find all string properties
                var typeDef = phrasesType.GetProperties(BindingFlags.Static | BindingFlags.Public).Where(x => x.PropertyType.Equals(typeof(string)));

                foreach (var aProp in typeDef)
                {
                    var propDef = langSet[aProp.Name];

                    if (propDef != null)
                    {
                        aProp.SetValue(null, propDef.Value);
                    }
                    else
                    {
                        Console.WriteLine($"Label {aProp.Name} not found in selected language");
                    }
                }
            }

        }
        #endregion

        #endregion

        
    }
}
