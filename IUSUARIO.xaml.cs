using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Lógica de interacción para IUSUARIO.xaml
    /// </summary>
    /// 



    public partial class IUSUARIO : Window
    {

        private string nombreUsuario; // Agrega esta propiedad

        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set
            {
                nombreUsuario = value;
                // Aquí puedes llamar al método para cargar la imagen de perfil o realizar otras acciones basadas en el usuario.
                MostrarFotoPerfil(value);
                MostrarFavoritos(value);

            }
        }

        private readonly DatabaseManager dbManager;


        public IUSUARIO()
        {
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            
            
            
        }

        private void Button_cerrarsesion(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
        private void Button_Favoritos(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Libre(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Perfil(object sender, RoutedEventArgs e)
        {

        }
        private void Button_tienda(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Traducir(object sender, RoutedEventArgs e)
        {

        }
        private void IUSUARIO_Loaded(object sender, RoutedEventArgs e)
        {


            string queryUltimaConexion = "SELECT ultima_conexion FROM usuarios WHERE usuario=@usuario";
            MySqlCommand cmdUltimaConexion = new MySqlCommand(queryUltimaConexion, dbManager.Connection);
            cmdUltimaConexion.Parameters.AddWithValue("@usuario", nombreUsuario);

            dbManager.Connection.Open();
            var ultimaConexion = cmdUltimaConexion.ExecuteScalar();
            dbManager.Connection.Close();

            if (ultimaConexion != DBNull.Value)
            {
                lblUltimaConex.Content = "Última conexión: " + ((DateTime)ultimaConexion).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                lblUltimaConex.Content = "No hay información de última conexión disponible.";
            }

            // Obtener la hora local del usuario
            DateTime horaActual = DateTime.Now;

            // Determinar si es buenos días, buenas tardes o buenas noches
            if (horaActual.Hour >= 6 && horaActual.Hour < 13)
            {
                lblSaludo.Content = "Buenos días: " + nombreUsuario;
            }
            else if (horaActual.Hour >= 13 && horaActual.Hour < 21)
            {
                lblSaludo.Content = "Buenas tardes: " + nombreUsuario;
            }
            else
            {
                lblSaludo.Content = "Buenas noches: " + nombreUsuario;
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

                    imgPerfil.Source = image; // Aquí estableces la imagen en el control de imagen.
                }
            }
            else
            {
                // Manejar el caso en que no hay foto de perfil.
                // mostrar una imagen predeterminada.
                var uri = new Uri("fotoperfildefault.png", UriKind.Relative);
                var image = new BitmapImage(uri);

                imgPerfil.Source = image; // Aquí también estableces la imagen en el control de imagen.
            }


        }

        private void MostrarFavoritos(string usuario)
        {
            try
            {
                string query = "SELECT ivc.Precio, v.Titulo " +
                               "FROM vinilosFavoritos vf " +
                               "JOIN vinilos v ON vf.idvinilo = v.Idvinilo " +
                               "JOIN infoVinilosCompra ivc ON v.Idvinilo = ivc.Idvinilo " +
                               "WHERE vf.usuario = @usuario";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);

                    dbManager.Connection.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("Nombre Usuario: " + usuario);

                        while (reader.Read())
                        {
                            Console.WriteLine(2);
                           
                            string cantidadDisponible = reader["Titulo"].ToString();
                            string precio = reader["Precio"].ToString();

                            StackPanel stackPanel = new StackPanel();
                            stackPanel.Margin = new Thickness(10);

                            TextBlock precioTextBlock = new TextBlock();
                            precioTextBlock.Text = precio;
                            precioTextBlock.FontFamily = new FontFamily("Bahnschrift");

                            TextBlock cantidadTextBlock = new TextBlock();
                            cantidadTextBlock.Text = cantidadDisponible;
                            cantidadTextBlock.FontFamily = new FontFamily("Bahnschrift");

                            stackPanel.Children.Add(cantidadTextBlock);
                            stackPanel.Children.Add(precioTextBlock);

                            wrapPanelFavoritos.Children.Add(stackPanel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar favoritos: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            
            double viewboxWidth = e.NewSize.Width / 800; // 800 es el ancho original de la ventana
            double viewboxHeight = e.NewSize.Height / 500; // 500 es el alto original de la ventana

            // Acceder al Rectangle y ajustar la porción de la imagen en el Viewbox
           
            /*
             * backgroundRectangle.Dispatcher.Invoke(() =>
            {
                ((ImageBrush)backgroundRectangle.Fill).Viewbox = new Rect(0, 0, viewboxWidth, viewboxHeight);
            });
            /*
            //Ajustar el tamaño de las etiquetas basándose en el ancho de la ventana
            double nuevoTamaño = e.NewSize.Width / 40; // Puedes ajustar el divisor según tus necesidades
            
            lblSaludo.FontSize = nuevoTamaño * 0.3+25; ;
            lblUltimaConex.FontSize = nuevoTamaño * 0.3+15; // Puedes ajustar el multiplicador según tus necesidades
            */
        }






    }
}
