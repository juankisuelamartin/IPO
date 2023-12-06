using System;
using System.Threading;
using System.Windows;

namespace WpfApp1.Helpers
{
    public static class Translator
    {
        private static ResourceDictionary dict;

        public static void Initialize()
        {
            dict = new ResourceDictionary();
            SetLanguage(Thread.CurrentThread.CurrentCulture.ToString());
        }

        public static void SetLanguage(string culture)
        {
            dict.Source = new Uri($"../../resources/StringResources/StringResources.{culture}.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        public static void SwitchLanguage(string culture)
        {
            Application.Current.Resources.MergedDictionaries.Remove(dict);
            SetLanguage(culture);
        }

        public static void SaveSelectedLanguage(string culture)
        {
            Properties.Settings.Default.SelectedLanguage = culture;
            Properties.Settings.Default.Save();
        }

        public static string GetSelectedLanguage()
        {
            return Properties.Settings.Default.SelectedLanguage;
        }
    }
}
