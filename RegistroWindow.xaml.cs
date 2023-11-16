using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class RegistroWindow : Window
    {
        private readonly DatabaseManager dbManager;
        public RegistroWindow()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
        }
        private void ButtonIniciar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
        private void ButtonRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            string usuario = Usuario.Text;
            string password = PasswordBox.Password;
            string password2 = PasswordBox2.Password;
            string email = Email.Text;
            string nombre = Nombre.Text;
            string apellido = Apellido.Text;
            string provincia = Provincia.Text;
            string calle = Calle.Text;
            string cp = CP.Text;

            if (string.IsNullOrWhiteSpace(usuario) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido) ||
                string.IsNullOrWhiteSpace(calle) ||
                string.IsNullOrWhiteSpace(cp) ||
                string.IsNullOrEmpty(provincia) ||
                FechaNacimiento.SelectedDate == null)
            {
                MessageBox.Show("Por favor, rellene todos los campos.");
            }
            else
            {
                DateTime fechaNacimiento = FechaNacimiento.SelectedDate.Value;
                if (password != password2)
                {
                    MessageBox.Show("Las contraseñas no coinciden");
                }
                else
                {
                    String encriptado = Encriptar(password);

                    // Comprueba si el usuario es mayor de 16 años
                    if ((DateTime.Now - fechaNacimiento).TotalDays < 16 * 365.25)
                    {
                        MessageBox.Show("Debes tener al menos 16 años para registrarte.");
                    }
                    else
                    {
                        int cpint;
                        if (!int.TryParse(cp, out cpint))
                        {
                            MessageBox.Show("El código postal debe ser un número");
                        }
                        else
                        {
                            if (UsuarioExiste(usuario))
                            {
                                MessageBox.Show("El nombre de usuario ya existe");
                            }
                            else
                            {
                                RegisterUser(usuario, encriptado, email, nombre, apellido, fechaNacimiento, provincia, calle, cp);
                                MainWindow mainwindow = new MainWindow();
                                mainwindow.Show();
                                this.Close();
                            }
                           
                        }
                        
                    }
                }

            }
        }
        private bool UsuarioExiste(string usuario)
        {
            string query = "SELECT COUNT(*) FROM usuarios WHERE usuario = @usuario";
            MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            dbManager.Connection.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            dbManager.Connection.Close();
            return count > 0;
        }
        private string Encriptar(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir la entrada en bytes y calcular el hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convertir los bytes del hash a una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void RegisterUser(string usuario, string password, string email, string nombre, string apellido, DateTime fechaNacimiento, string provincia, string calle, string cp)
        {
            string query = "INSERT INTO usuarios (usuario, password, email, nombre, apellido, fecha_nacimiento) VALUES (@usuario, @password, @email, @nombre, @apellido, @fechaNacimiento)";
            MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apellido", apellido);
            cmd.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);

            string query2 = "INSERT INTO direccion (usuario, provincia, calle, cp) VALUES (@usuario, @provincia, @calle, @cp)";
            MySqlCommand cmd2 = new MySqlCommand(query2, dbManager.Connection);
            cmd2.Parameters.AddWithValue("@usuario", usuario);
            cmd2.Parameters.AddWithValue("@provincia", provincia);
            cmd2.Parameters.AddWithValue("@calle", calle);
            cmd2.Parameters.AddWithValue("@cp", cp);

            dbManager.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd2.ExecuteNonQuery();
            dbManager.Connection.Close();
            MessageBox.Show("Usuario registrado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}