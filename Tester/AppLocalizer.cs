using System;
using System.Localization;

namespace Tester
{
	public class AppLocalizer : Localizer<Lables>
	{
		private static Lazy<AppLocalizer> _current = new Lazy<AppLocalizer>(new AppLocalizer());

		public static AppLocalizer Current
		{
			get { return _current.Value; }
		}
	}
}
