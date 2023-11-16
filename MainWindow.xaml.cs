using System;
using System.Data.SqlClient;
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
            // Asigna los manejadores de eventos para los cuadros de texto
            UsuarioLogin.GotFocus += TextBox_GotFocus;
            UsuarioLogin.LostFocus += TextBox_LostFocus;
            PWDLogin.GotFocus += TextBox_GotFocus;
            PWDLogin.LostFocus += TextBox_LostFocus;
            PWDLogin.Password = "Contraseña";
            this.Focus();
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Manejador de evento GotFocus
            Control control = (Control)sender;
            if (control is TextBox textBox && (textBox.Text == "Ingrese Usuario" || textBox.Text == "Contraseña"))
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
            // Manejador de evento LostFocus
            Control control = (Control)sender;
            if (control is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                // Restablecer el texto por defecto si está vacío
                textBox.Text = (textBox.Name == "UsuarioLogin") ? "Ingrese Usuario" : "Contraseña";
            }
            else if (control is PasswordBox passwordBox && string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                // Restablecer el texto por defecto si está vacío
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
            string query = "SELECT COUNT(*) FROM usuarios WHERE usuario=@usuario AND password=@password";
            MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@password", password);
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
                MessageBox.Show("Inicio de sesión exitoso", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Las credenciales proporcionadas son incorrectas.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
