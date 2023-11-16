using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string password = Password.Text;
            string email = Email.Text;
            string nombre = Nombre.Text;
            string apellido = Apellido.Text;
            DateTime fechaNacimiento = FechaNacimiento.SelectedDate.Value;

            RegisterUser(usuario, password, email, nombre, apellido, fechaNacimiento);
        }

        private void RegisterUser(string usuario, string password, string email, string nombre, string apellido, DateTime fechaNacimiento)
        {
            string query = "INSERT INTO usuarios (usuario, password, email, nombre, apellido, fecha_nacimiento) VALUES (@usuario, @password, @email, @nombre, @apellido, @fechaNacimiento)";
            MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apellido", apellido);
            cmd.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento);
            dbManager.Connection.Open();
            cmd.ExecuteNonQuery();
            dbManager.Connection.Close();
            MessageBox.Show("Usuario registrado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}