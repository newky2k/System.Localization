﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Localization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

[assembly: ExternalPhraseProvider(typeof(ExternalLangs.Phrases))]
namespace LocalisationSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

           // Localizer.SetLanguage("en");
        }
    }
}
