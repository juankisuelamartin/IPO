using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Helpers;
using WpfApp1.Views.Admin;

namespace WpfApp1.Views
{

    public partial class IUPrincipalA : Window
    {
        private bool rotated = true; //Variable control menu desplegable
        private string nombreUsuario; // Agrega esta propiedad
        private readonly LanguageManager languageManager; // Agrega esta propiedad
        private readonly DatabaseManager dbManager;

        public string NombreUsuario

        {
            get { return nombreUsuario; }
            set
            {
                nombreUsuario = value;
                // Aquí puedes llamar al método para cargar la imagen de perfil o realizar otras acciones basadas en el usuario.
                MostrarFotoPerfil(value);

                languageManager.LoadLanguageResources();
                languageManager.InitializeLanguageComboBox(LanguageComboBox);
                // Restaurar el idioma seleccionado previamente

                string selectedLanguage = Translator.GetSelectedLanguage();
                if (!string.IsNullOrEmpty(selectedLanguage))
                {
                    Translator.SwitchLanguage(selectedLanguage);
                    languageManager.SetLanguageComboBox(selectedLanguage, LanguageComboBox);

                }

            }
        }


        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            languageManager.LanguageComboBox_SelectionChanged(sender, e, LanguageComboBox);
        }

        public IUPrincipalA()
        {
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager();
            // Suscribir a los eventos "Click" de los enlaces "Ver más..."
            lblverMasNov.MouseUp += VerMasNovedades_Click;
            lblverMasOft.MouseUp += VerMasOfertas_Click;
            lblverMasFav.MouseUp += VerMasFavoritos_Click;
        }



        private void Button_Home(object sender, RoutedEventArgs e)
        {
            IUPrincipalA iUPrincipalA = new IUPrincipalA();
            iUPrincipalA.NombreUsuario = this.NombreUsuario;
            iUPrincipalA.Show();
            this.Close();

        }

        private void Button_Ofertas(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Historial(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Incidencias(object sender, RoutedEventArgs e)
        {

        }

        private void VerMasNovedades_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void VerMasOfertas_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void VerMasFavoritos_Click(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Button_Perfil(object sender, RoutedEventArgs e)
        {

        }

        private void Button_cerrarsesion(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.KeepSession = false;
            Properties.Settings.Default.Save();
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }

        private void Button_historialCompras(object sender, RoutedEventArgs e)
        {

        }

        private void BtnGestion(object sender, RoutedEventArgs e)
        {
            // Si el Popup está abierto, ciérralo; de lo contrario, ábrelo
            if (popupGestiones.IsOpen == true)
            {

                popupGestiones.IsOpen = false;


            }
            else
            {
                // Abre el Popup

                popupGestiones.IsOpen = true;
                // Asigna el foco al Popup para que pueda manejar eventos de clic

            }
        }




        private void BtnGestionVinilos_Click(object sender, RoutedEventArgs e)
        {
            IUVinilosA iUVinilosA = new IUVinilosA();
            iUVinilosA.NombreUsuario = this.NombreUsuario;
            iUVinilosA.Show();
            this.Close();
        }

        private void BtnGestionArtistas_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para el botón Gestión Artistas
        }

        private void BtnGestionContacto_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para el botón Gestión Contacto
        }

        private void imgPerfil_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Si el Popup está abierto, ciérralo; de lo contrario, ábrelo
            if (popupMarco.Visibility == Visibility.Visible)
            {

                popupMarco.Visibility = Visibility.Collapsed;


            }
            else
            {
                // Abre el Popup

                popupMarco.Visibility = Visibility.Visible;
                // Asigna el foco al Popup para que pueda manejar eventos de clic

            }

            if (rotated)
            {
                // Si ya está rotado, restaurar la rotación a 0º
                desplegable.RenderTransform = new RotateTransform(0);
                desplegable.Margin = new Thickness(-20, 0, 0, 0);
            }
            else
            {
                // Rotar a -90º
                desplegable.RenderTransform = new RotateTransform(90);
                desplegable.Margin = new Thickness(0, 0, 0, 0);

            }
            // Alternar el estado de rotación
            rotated = !rotated;
        }

        private void Button_Traducir(object sender, RoutedEventArgs e)
        {

        }

        private void IUSUARIO_Loaded(object sender, RoutedEventArgs e)
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

            string queryUltimaConexion = "SELECT ultima_conexion FROM usuarios WHERE usuario=@usuario";
            MySqlCommand cmdUltimaConexion = new MySqlCommand(queryUltimaConexion, dbManager.Connection);
            cmdUltimaConexion.Parameters.AddWithValue("@usuario", nombreUsuario);

            dbManager.Connection.Open();
            var ultimaConexion = cmdUltimaConexion.ExecuteScalar();
            dbManager.Connection.Close();

            if (ultimaConexion != DBNull.Value)
            {
                DateTime ultimaConexionLocal = TimeZoneInfo.ConvertTimeFromUtc((DateTime)ultimaConexion, localTimeZone);
                lblUltimaConex.Content = "Última conexión: " + ultimaConexionLocal.ToString("yyyy-MM-dd HH:mm:ss");

                // Determinar si es buenos días, buenas tardes o buenas noches
                if (ultimaConexionLocal.Hour >= 6 && ultimaConexionLocal.Hour < 13)
                {
                    lblSaludo.Content = "Buenos días: " + nombreUsuario;
                }
                else if (ultimaConexionLocal.Hour >= 13 && ultimaConexionLocal.Hour < 21)
                {
                    lblSaludo.Content = "Buenas tardes: " + nombreUsuario;
                }
                else
                {
                    lblSaludo.Content = "Buenas noches: " + nombreUsuario;
                }
            }
            else
            {
                lblUltimaConex.Content = "No hay información de última conexión disponible.";
            }

        }

        private void MostrarFotoPerfil(string usuario)
        {

            string query = "SELECT foto_perfil FROM usuarios WHERE usuario=@usuario";
            MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
            cmd.Parameters.AddWithValue("@usuario", usuario);

            dbManager.Connection.Open();
            var result = cmd.ExecuteScalar();
            dbManager.Connection.Close();

            if (result != DBNull.Value)
            {
                byte[] fotoPerfil = (byte[])result;
                using (var ms = new MemoryStream(fotoPerfil))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();


                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 90; // ajusta el tamaño según tus necesidades
                    ellipse.Height = 90;

                    // Establecer el contenedor Ellipse como contenido de imgPerfil
                    imgPerfil.Fill = new ImageBrush(image);
                }
            }

        }


    }
}