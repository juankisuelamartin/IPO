using System;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseManager dbManager;

        public MainWindow()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            LoadLanguageResources();
            InitializeLanguageComboBox();
            // Restaurar el idioma seleccionado previamente
            string selectedLanguage = Translator.GetSelectedLanguage();
            if (!string.IsNullOrEmpty(selectedLanguage))
            {
                Translator.SwitchLanguage(selectedLanguage);
                SetLanguageComboBox(selectedLanguage);
            }
            // Asignar los manejadores de eventos para los cuadros de texto
            UsuarioLogin.GotFocus += TextBox_GotFocus;
            UsuarioLogin.LostFocus += TextBox_LostFocus;
            PWDLogin.GotFocus += TextBox_GotFocus;
            PWDLogin.LostFocus += TextBox_LostFocus;
            PWDLogin.Password = "Contraseña";
            this.Focus();
        }

        private void LoadLanguageResources()
        {
            Translator.Initialize();
        }

        private void InitializeLanguageComboBox()
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

        private void SetLanguageComboBox(string culture)
        {
            LanguageComboBox.SelectedValue = culture;
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem != null)
            {
                string selectedCulture = ((dynamic)LanguageComboBox.SelectedItem).Culture;
                Translator.SwitchLanguage(selectedCulture);
                Translator.SaveSelectedLanguage(selectedCulture);
            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Control control = (Control)sender;
            if (control is TextBox textBox && (textBox.Text == (string)FindResource("EnterUserText") || textBox.Text == "Contraseña"))
            {
                textBox.Text = "";
            }
            else if (control is PasswordBox passwordBox && passwordBox.Password == "Contraseña")
            {
                passwordBox.Password = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Control control = (Control)sender;
            if (control is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = (string)FindResource("EnterUserText");
            }
            else if (control is PasswordBox passwordBox && string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.Password = "Contraseña";
            }
        }

        private void ButtonRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            RegistroWindow registroWindow = new RegistroWindow();
            registroWindow.Show();
            this.Close();
        }

        private bool IsLoginValid(string usuario, string password)
        {
            String pwdencriptada = RegistroWindow.Encriptar(password);
            string query = "SELECT COUNT(*) FROM usuarios WHERE usuario=@usuario AND password=@password";
            MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@password", pwdencriptada);
            dbManager.Connection.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            dbManager.Connection.Close();
            return (count > 0);
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string usuario = UsuarioLogin.Text;
            string password = PWDLogin.Password;

            if (IsLoginValid(usuario, password))
            {
                string query = "SELECT ROL FROM usuarios WHERE usuario=@usuario";
                MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                dbManager.Connection.Open();
                bool rol = Convert.ToBoolean(cmd.ExecuteScalar());
                dbManager.Connection.Close();

                if (rol == false) 
                    //USUARIO:
                {
                    IUSUARIO iUSUARIO = new IUSUARIO();
                    iUSUARIO.Show();
                    this.Close();
                }
                else
                {
                    // TODO ADMIN VIEW
                    MessageBox.Show("ADMIN VIEW");
                }

            }
            else
            {
                MessageBox.Show("Las credenciales proporcionadas son incorrectas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
