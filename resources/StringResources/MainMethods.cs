using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.resources.Dominio;
using WpfApp1.Views;
using WpfApp1.Views.Admin;
using WpfApp1.Views.Cliente;

namespace WpfApp1.resources.StringResources
{
    internal class MainMethods
    {
        public void Button_cerrarsesion(Window currentWindow)
        {
            Properties.Settings.Default.KeepSession = false;
            Properties.Settings.Default.Save();
            MainWindow mainwindow = new MainWindow();
            Window_Closing(currentWindow);
            mainwindow.Show();
            currentWindow.Close();
        }
        public void Button_Home(String NombreUsuario, Window currentWindow)
        {
            IUPrincipalU iuPrincipal = new IUPrincipalU();
            iuPrincipal.NombreUsuario = NombreUsuario;
            Window_Closing(currentWindow);
            iuPrincipal.Show();
            currentWindow.Close();
        }
        public void Button_Favorites(String NombreUsuario, Window currentWindow)
        {
            IUFavoritos iuFavoritos = new IUFavoritos();
            iuFavoritos.NombreUsuario = NombreUsuario;
            Window_Closing(currentWindow);
            iuFavoritos.Show();
            currentWindow.Close();
        }
        public void Button_Tienda(String NombreUsuario, Window currentWindow)
        {
            IUTiendaMenuU iuTienda = new IUTiendaMenuU();
            iuTienda.NombreUsuario = NombreUsuario;
            Window_Closing(currentWindow);
            iuTienda.Show();
            currentWindow.Close();
        }
        public void newiuVinilos(String NombreUsuario, int idVinilo, Window currentWindow)
        {
            IUViniloU iuVinilos = new IUViniloU();
            iuVinilos.NombreUsuario = NombreUsuario;
            iuVinilos.IdVinilo = idVinilo;
            Window_Closing(currentWindow);
            iuVinilos.Show();
            currentWindow.Close();
        }
        public Window Window_Loaded(Window window)
        {
            window.Top = Properties.Settings.Default.WindowTop;
            window.Left = Properties.Settings.Default.WindowLeft;
            window.Height = Properties.Settings.Default.WindowHeight;
            window.Width = Properties.Settings.Default.WindowWidth;

            return window;
        }
        public void Window_Closing(Window window)
        {
            Properties.Settings.Default.WindowTop = window.Top;
            Properties.Settings.Default.WindowLeft = window.Left;
            Properties.Settings.Default.WindowHeight = window.Height;
            Properties.Settings.Default.WindowWidth = window.Width;
            Properties.Settings.Default.Save();
        }
        public bool ImgPerfil_MouseUp(Border popupMarco, bool rotated, Image desplegable, Window currentWindow)
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
            return rotated;
        }
        public void IUSUARIO_Loaded(DatabaseManager dbManager, String nombreUsuario, Label lblUltimaConex, DependencyProperty ContentProperty, Label lblSaludo, Window currentWindow)
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
                lblUltimaConex.SetResourceReference(ContentProperty, "lblUltimaConex");
                lblUltimaConex.Content += ": " + ultimaConexionLocal.ToString("yyyy-MM-dd HH:mm:ss");

                // Determinar si es buenos días, buenas tardes o buenas noches
                if (ultimaConexionLocal.Hour >= 6 && ultimaConexionLocal.Hour < 13)
                {
                    lblSaludo.SetResourceReference(ContentProperty, "GoodMorningLabel");
                    lblSaludo.Content += " " + nombreUsuario;
                }
                else if (ultimaConexionLocal.Hour >= 13 && ultimaConexionLocal.Hour < 21)
                {
                    lblSaludo.SetResourceReference(ContentProperty, "GoodAfternoonLabel");
                    lblSaludo.Content += " " + nombreUsuario;
                }
                else
                {
                    lblSaludo.SetResourceReference(ContentProperty, "GoodNightLabel");
                    lblSaludo.Content += " " + nombreUsuario;
                }

            }
            else
            {
                lblUltimaConex.Content = "No hay información de última conexión disponible.";
            }

        }
        public void MostrarFotoPerfil(string usuario, DatabaseManager dbManager, Ellipse imgPerfil, Window currentWindow)
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


        //ADMIN:
        public void Button_HomeAdmin(String NombreUsuario, Window currentWindow)
        {
            IUPrincipalA iuPrincipalA = new IUPrincipalA();
            iuPrincipalA.NombreUsuario = NombreUsuario;
            Window_Closing(currentWindow);
            iuPrincipalA.Show();
            currentWindow.Close();
        }
        public void ButtonOfertas(String NombreUsuario, Window currentWindow)
        {
            IUOfertasA iuOfertasA = new IUOfertasA();
            //iuOfertasA.NombreUsuario = NombreUsuario;
            Window_Closing(currentWindow);
            iuOfertasA.Show();
            currentWindow.Close();
        }
        public void ButtonHistorial(String NombreUsuario, Window currentWindow)
        {
            IUHistorialPedidosA iuHistorialPedidosA = new IUHistorialPedidosA();
            //iuHistorialPedidosA.NombreUsuario = NombreUsuario;
            Window_Closing(currentWindow);
            iuHistorialPedidosA.Show();
            currentWindow.Close();
        }

        public void ButtonIncidencias(String NombreUsuario, Window currentWindow)
        {
            IUIncidenciasA iuIncidenciasA = new IUIncidenciasA();
            //iuIncidenciasA.NombreUsuario = NombreUsuario;
            Window_Closing(currentWindow);
            iuIncidenciasA.Show();
            currentWindow.Close();
        }

        public void ButtonGestionVinilos(String NombreUsuario, Window currentWindow)
        {
            IUVinilosA iUVinilosA = new IUVinilosA();
            Window_Closing(currentWindow);
            iUVinilosA.NombreUsuario = NombreUsuario;
            iUVinilosA.Show();
            currentWindow.Close();
        }

        public void ButtonGestionArtistas(String NombreUsuario, Window currentWindow)
        {
            IUArtistasA iUArtistasA = new IUArtistasA();
            Window_Closing(currentWindow);
            iUArtistasA.NombreUsuario = NombreUsuario;
            iUArtistasA.Show();
            currentWindow.Close();
        }
        public void ButtonGestionContacto(String NombreUsuario, Window currentWindow)
        {
            IUContactoA iUContactoA = new IUContactoA();
            Window_Closing(currentWindow);
            //iUContactoA.NombreUsuario = NombreUsuario;
            iUContactoA.Show();
            currentWindow.Close();
        }

    }

}
