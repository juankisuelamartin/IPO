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

namespace WpfApp1.Views
{
    public partial class IUFavoritos : Window
    {
        private bool rotated = true; //Variable control menu desplegable
        private string nombreUsuario; // Agrega esta propiedad
        private readonly DatabaseManager dbManager;
        private readonly LanguageManager languageManager; // Agrega esta propiedad

        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set
            {
                nombreUsuario = value;
                // Aquí puedes llamar al método para cargar la imagen de perfil o realizar otras acciones basadas en el usuario.
                MostrarFotoPerfil(value);
                MostrarFavoritos(value);
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

        public IUFavoritos()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager(); // Inicializa la instancia de LanguageManager
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            languageManager.LanguageComboBox_SelectionChanged(sender, e, LanguageComboBox);
        }
    

private void Button_cerrarsesion(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.KeepSession = false;
            Properties.Settings.Default.Save();
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
        private void Button_Favoritos(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Perfil(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Tienda(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Home(object sender, RoutedEventArgs e)
        {
            IUPrincipalU iuPrincipal= new IUPrincipalU();
            iuPrincipal.NombreUsuario = this.NombreUsuario;
            iuPrincipal.Show();
            this.Close();
        }

        private void Button_Carrito(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Buscar(object sender, RoutedEventArgs e)
        {

        }

        private void Button_historialCompras(object sender, RoutedEventArgs e)
        {

        }

        private void ProfileMenuPopup_Closed(object sender, EventArgs e)
        {
            // Se llama cuando el Popup se cierra
           // imgPerfilBorder.Visibility = Visibility.Collapsed;
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

        private void MostrarFavoritos(string usuario)
        {
            try
            {
                string query = "SELECT ivc.Precio, v.Titulo, v.Portada, v.Idvinilo " +
                               "FROM vinilosFavoritos vf " +
                               "JOIN vinilos v ON vf.idvinilo = v.Idvinilo " +
                               "JOIN infoVinilosCompra ivc ON v.Idvinilo = ivc.Idvinilo " +
                               "WHERE vf.usuario = @usuario";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    dbManager.Connection.Open();


                    // Agrega un WrapPanel horizontal para organizar los StackPanels en columnas
                    WrapPanel horizontalWrapPanel = new WrapPanel();
                    horizontalWrapPanel.Orientation = Orientation.Horizontal;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            string titulo = reader["Titulo"].ToString();
                            string precio = reader["Precio"].ToString();

                            StackPanel stackPanel = new StackPanel();
                            stackPanel.Margin=new Thickness(left: 0, top: 15, right:0, bottom: 10);


                            //Lamadas cargar Tiulo y Precio
                            portadaFavoritos(reader, stackPanel);
                            toggleFavoritos(reader, wrapPanelFavoritos, stackPanel);
                            tituloFavoritos(titulo, stackPanel);
                            precioFavoritos(precio, stackPanel);


                            // Agrega un salto de línea después de cada número de columnas especificado
                            wrapPanelFavoritos.Children.Add(horizontalWrapPanel);
                            horizontalWrapPanel = new WrapPanel();
                            horizontalWrapPanel.Orientation = Orientation.Horizontal;

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

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = (ToggleButton)sender;

            // Llama al método común para manejar el cambio de estado (solo Checked)
            CambiarEstadoToggleButton(toggleButton, isChecked: true);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = (ToggleButton)sender;

            // Llama al método común para manejar el cambio de estado (solo Unchecked)
            CambiarEstadoToggleButton(toggleButton, isChecked: false);
        }

        private void CambiarEstadoToggleButton(ToggleButton toggleButton, bool isChecked)
        {
            int idVinilo = (int)toggleButton.Tag;

            // Aquí ejecuta la lógica para agregar o eliminar el vinilo de la tabla de favoritos
            try
            {
                string usuario = nombreUsuario;
                string insertQuery = "INSERT INTO vinilosFavoritos (usuario, idvinilo) VALUES (@usuario, @idvinilo)";
                string deleteQuery = "DELETE FROM vinilosFavoritos WHERE usuario=@usuario AND idvinilo=@idvinilo";

                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, dbManager.Connection))
                using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, dbManager.Connection))
                {
                    insertCmd.Parameters.AddWithValue("@usuario", usuario);
                    insertCmd.Parameters.AddWithValue("@idvinilo", idVinilo);

                    deleteCmd.Parameters.AddWithValue("@usuario", usuario);
                    deleteCmd.Parameters.AddWithValue("@idvinilo", idVinilo);

                    dbManager.Connection.Open();

                    if (isChecked)
                    {
                        // Solo ejecuta la lógica de eliminación si el botón está siendo marcado (Checked)
                        deleteCmd.ExecuteNonQuery();

                        // Eliminación exitosa
                        MessageBox.Show("Vinilo eliminado de favoritos correctamente.");
                    }
                    else
                    {
                        // Solo ejecuta la lógica de inserción si el botón está siendo desmarcado (Unchecked)
                        bool isInserted = insertCmd.ExecuteNonQuery() > 0;

                        if (isInserted)
                        {
                            // Inserción exitosa
                            MessageBox.Show("Vinilo añadido a favoritos correctamente.");
                        }
                        else
                        {
                            // No se encontró el vinilo en favoritos, eliminarlo
                            deleteCmd.ExecuteNonQuery();

                            // Eliminación exitosa
                            MessageBox.Show("No se ha podido añadir de nuevo a favoritos.");
                        }
                    }

                    // Crear un Image con la imagen deseada
                    Image buttonImage = new Image();
                    BitmapImage buttonImageBitmapImage;

                    if (isChecked)
                    {


                        buttonImageBitmapImage = new BitmapImage(new Uri("../Assets/Images/Corazon.png", UriKind.Relative));
                    }
                    else
                    {
                        buttonImageBitmapImage = new BitmapImage(new Uri("../Assets/Images/CorazonColoreadoB.png", UriKind.Relative));
                    }

                    buttonImage.Source = buttonImageBitmapImage;
                    buttonImage.Width = 40; // ajusta el tamaño según tus necesidades
                    buttonImage.Height = 40;

                    // Establecer la imagen como contenido del botón
                    toggleButton.Content = buttonImage;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar/eliminar el vinilo de favoritos: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }

        private void portadaFavoritos(MySqlDataReader reader, StackPanel stackPanel)
        {
            // Verificar si la columna Portada es NULL
            if (!(reader["Portada"] is DBNull))
            {
                byte[] portadaBytes = (byte[])reader["Portada"];

                // Crear una imagen a partir de los bytes de la portada
                Image portadaImage = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                using (MemoryStream stream = new MemoryStream(portadaBytes))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                    // Ajustar el tamaño máximo de la imagen a 100x100
                    bitmapImage.DecodePixelWidth = 100;
                    bitmapImage.DecodePixelHeight = 100;

                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                }

                // Crear un contenedor para la imagen con un fondo blanco
                Grid imageContainer = new Grid();
                imageContainer.Background = Brushes.White;

                // Crear una imagen con un tamaño fijo de 100x100
                Image finalImage = new Image();
                finalImage.Source = bitmapImage;
                finalImage.Width = 90;
                finalImage.Height = 100;

                // Centrar la imagen dentro del contenedor
                imageContainer.Children.Add(finalImage);
                imageContainer.HorizontalAlignment = HorizontalAlignment.Center;

                stackPanel.Children.Add(imageContainer);
            }
            else
            {
                // Si la columna Portada es NULL, usar una imagen por defecto
                Image defaultImage = new Image();
                BitmapImage defaultBitmapImage = new BitmapImage(new Uri("../Assets/Images/ViniloX.jpg", UriKind.Relative));
                defaultImage.Source = defaultBitmapImage;
                defaultImage.Width = 100; // Ajusta el ancho según tus necesidades


                stackPanel.Children.Add(defaultImage);
            }
        }

        private void toggleFavoritos(MySqlDataReader reader, WrapPanel horizontalWrapPanel, StackPanel stackPanel) {

            //Crear borde con color hexadecimal
            Border border = new Border();
            border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE4E4E4"));
            border.CornerRadius = new CornerRadius(10);
            border.Margin = new Thickness(left: 25, top: 30, right: 20, bottom: 50);

            //Crear toggleButton para cada vinilo que encuentre en favoritos y sus eventos
            ToggleButton toggleButton = new ToggleButton();
            toggleButton.Content = "Eliminar";
            toggleButton.Tag = reader["Idvinilo"]; // Almacena el ID del vinilo en el Tag del botón
            toggleButton.Checked += ToggleButton_Checked; // Asigna el manejador de eventos al hacer clic
            toggleButton.Unchecked += ToggleButton_Unchecked;
            toggleButton.Width = 45; // ajusta el tamaño según tus necesidades
            toggleButton.Height = 45;

            // Establecer propiedades para quitar el borde
            toggleButton.BorderThickness = new Thickness(0);
            toggleButton.Background = Brushes.Transparent;

            toggleButton.FocusVisualStyle = null;
            toggleButton.BorderBrush = Brushes.Transparent;
            toggleButton.Background = Brushes.Transparent;

            // Crear un Image con la imagen por defecto
            Image buttonImage = new Image();
            BitmapImage buttonImageBitmapImage = new BitmapImage(new Uri("../Assets/Images/CorazonColoreadoB.png", UriKind.Relative));
            buttonImage.Source = buttonImageBitmapImage;
            buttonImage.Width = 40; // ajusta el tamaño según tus necesidades
            buttonImage.Height = 40;

            toggleButton.Content = buttonImage;


            //TODO: Elegir mejor posicion: 95, -180, 0, 0 Esquina Sup Derecha
            //                             80, -20, 0, 0  Esquina Inf Derecha
            toggleButton.Margin = new Thickness(95, -180, 0, 0);
            toggleButton.FocusVisualStyle = null;
            stackPanel.Children.Add(toggleButton);

            border.Child = stackPanel;
            horizontalWrapPanel.Children.Add(border);

        }

        private void tituloFavoritos(String titulo, StackPanel stackPanel)
        {
            // Establecer un límite máximo de caracteres para el título
            int maxCaracteres = 15; // Puedes ajustar según tus necesidades
            string tituloRecortado = titulo.Length > maxCaracteres ? titulo.Substring(0, maxCaracteres) + "..." : titulo;
            TextBlock tituloTextBlock = new TextBlock();
            tituloTextBlock.Text = tituloRecortado; // Añadir etiqueta Título
            tituloTextBlock.FontFamily = new FontFamily("Bahnschrift");
            tituloTextBlock.FontWeight = FontWeights.Bold;
            tituloTextBlock.HorizontalAlignment = HorizontalAlignment.Center; // Alinear al centro
            tituloTextBlock.Margin=new Thickness(0,10,0,3);

            // Establecer un Tooltip para mostrar el título completo
            tituloTextBlock.ToolTip = titulo;

            stackPanel.Children.Add(tituloTextBlock);

        }

        private void precioFavoritos(String precio, StackPanel stackPanel)
        {
            TextBlock precioTextBlock = new TextBlock();
            precioTextBlock.Text = precio + " €"; // Añadir etiqueta Precio
            precioTextBlock.FontFamily = new FontFamily("Bahnschrift");
            precioTextBlock.HorizontalAlignment = HorizontalAlignment.Center; // Alinear al centro

            stackPanel.Children.Add(precioTextBlock);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }


    }
}