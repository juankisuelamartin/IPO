using System;
using System.Linq;
using System.Windows.Controls;
using WpfApp1.Helpers;

namespace WpfApp1
{
    public class LanguageManager
    {
        public void LoadLanguageResources()
        {
            Translator.Initialize();
        }

        public void InitializeLanguageComboBox(ComboBox LanguageComboBox)
        {
            // Limpiar los elementos existentes
            
                LanguageComboBox.Items.Clear();

                // Configurar el ComboBox con los idiomas disponibles
                LanguageComboBox.ItemsSource = new[]
                {
                new { DisplayName = "en-US", Culture = "en-US" },
                new { DisplayName = "es-ES", Culture = "es-ES" }
            };
                LanguageComboBox.DisplayMemberPath = "DisplayName";
                LanguageComboBox.SelectedValuePath = "Culture";

        }

        public void SetLanguageComboBox(string culture, ComboBox LanguageComboBox)
        {
            LanguageComboBox.SelectedValue = culture;
        }

        public void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e, ComboBox LanguageComboBox)
        {
            if (LanguageComboBox.SelectedItem != null)
            {
                string selectedCulture = ((dynamic)LanguageComboBox.SelectedItem).Culture;
                Translator.SwitchLanguage(selectedCulture);
                Translator.SaveSelectedLanguage(selectedCulture);
            }
        }
    }
}
