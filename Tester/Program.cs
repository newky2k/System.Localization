using System;
using System.IO;
using System.Localization;
using System.Threading;

namespace Tester
{
	class Program
	{
		static void Main(string[] args)
		{
			//AppLocalizer.Current.Init();
			AppLocalizer.Current.SetLanguage(Thread.CurrentThread.CurrentUICulture);

			var names = AppLocalizer.Current.AvailableLanguageNames;

			names.ForEach(Console.WriteLine);
			

			Console.WriteLine(AppLocalizer.Current.Phrases.DoneButton);
			Console.WriteLine(AppLocalizer.Current.Phrases.CancelButton);

			var currentLocation = Path.GetDirectoryName(typeof(Program).Assembly.Location);

			var fileName = Path.Combine(currentLocation,"English.xml");

			var english = new LanguageOptions()
			{
				DisplayName = "English",
				LanguageCode = "en",
				LanguageSubCode = "en-gb",
			};

			AppLocalizer.Current.Generate(english, currentLocation);

			
			Console.ReadLine();
		}
	}
}
