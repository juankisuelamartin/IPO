using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.BC;
using WpfApp1.Helpers;
using WpfApp1.Views;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseManager dbManager;

        public MainWindow()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();

            // Verifica si la opción "mantener sesión" fue seleccionada la última vez
            if (Properties.Settings.Default.KeepSession == true)
            {
                string usuario = Properties.Settings.Default.Usuario;
                string token = Properties.Settings.Default.Token;

                string query = "SELECT * FROM session_tokens WHERE user_id=@usuario AND token=@token";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@token", token);

                    dbManager.Connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        DateTime expiryTime = reader.GetDateTime("expiry_time");

                        if (DateTime.Now < expiryTime)
                        {
                            dbManager.Connection.Close();
                            Login(usuario);
                        }
                        else
                        {
                            // El token ha expirado
                            Properties.Settings.Default.KeepSession = false;
                            dbManager.Connection.Close();
                            IniciarVentana();
                        }
                    }
                    else
                    {
                        Properties.Settings.Default.KeepSession = false;
                        dbManager.Connection.Close();
                        IniciarVentana();
                    }

                    dbManager.Connection.Close();
                }
            }

            else
            {
                IniciarVentana();
            }
            
        }
        private void IniciarVentana()
        {
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
                // Si el checkbox "mantener sesión" está marcado
                if (mantener_sesion.IsChecked == true)
                {
                    // generar token - 2 semanas
                    // Guarda la opción seleccionada en las configuraciones de la aplicación

                    String token = GenerateSessionToken();
                    StoreSessionToken(usuario, token);
                }
                Login(usuario);

            }
            else
            {
                MessageBox.Show("Las credenciales proporcionadas son incorrectas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Login(string usuario)
        {

            string queryUpdate = "UPDATE usuarios SET ultima_conexion = NOW() WHERE usuario = @usuario";
            MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, dbManager.Connection);
            cmdUpdate.Parameters.AddWithValue("@usuario", usuario);
            dbManager.Connection.Open();
            cmdUpdate.ExecuteNonQuery();

            string query = "SELECT ROL FROM usuarios WHERE usuario=@usuario";
            MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            bool rol = Convert.ToBoolean(cmd.ExecuteScalar());
            dbManager.Connection.Close();

            if (rol == false)
            //USUARIO:
            {   /*
                IUSUARIO iusuario= new IUSUARIO();
                iusuario.NombreUsuario = usuario;
                iusuario.Show();
                this.Close();
                */
                IUPrincipalU iuPrincipal = new IUPrincipalU();
                iuPrincipal.NombreUsuario = usuario; // Establece la propiedad NombreUsuario antes de mostrar la ventana.
                iuPrincipal.Show();
                this.Close();
                
            }
            else
            {
                // TODO ADMIN VIEW
                MessageBox.Show("ADMIN VIEW");
            }
        }
        public static string GenerateSessionToken()
        {
            // Define la longitud del token.
            int tokenLength = 60;

            // Define los caracteres que se pueden utilizar en el token.
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            StringBuilder token = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < tokenLength; i++)
            {
                token.Append(validChars[random.Next(validChars.Length)]);
            }

            return token.ToString();
        }
        public void StoreSessionToken(string userId, string sessionToken)
        {
            string query = @"
                INSERT INTO session_tokens (user_id, token, expiry_time) 
                VALUES (@userId, @token, NOW() + INTERVAL 14 DAY)
                ON DUPLICATE KEY UPDATE token = @token, expiry_time = NOW() + INTERVAL 14 DAY";
            using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@token", sessionToken);

                dbManager.Connection.Open();
                cmd.ExecuteNonQuery();
                dbManager.Connection.Close();

                // guardamos en local el token.

                Properties.Settings.Default.KeepSession = true;
                Properties.Settings.Default.Token = sessionToken;
                Properties.Settings.Default.Usuario = userId;
                Properties.Settings.Default.Save();
            }
        }



    }
}
