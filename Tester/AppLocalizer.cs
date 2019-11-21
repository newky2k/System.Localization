using System;
using System.Localization;
using System.Threading;

namespace Tester
{
	public class AppLocalizer : Localizer<Lables>
	{
		private static Lazy<AppLocalizer> _current = new Lazy<AppLocalizer>(new AppLocalizer());

		public static AppLocalizer Current
		{
			get { return _current.Value; }
		}

        static AppLocalizer()
        {
            
            Current.SetLanguage(Thread.CurrentThread.CurrentUICulture);
        }
	}
}
