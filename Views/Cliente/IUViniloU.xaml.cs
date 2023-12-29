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
using WpfApp1.resources.StringResources;

namespace WpfApp1.Views
{

    public partial class IUViniloU : Window
    {
        private bool rotated = true; //Variable control menu desplegable
        private string nombreUsuario; // Agrega esta propiedad
        private readonly DatabaseManager dbManager;
        private readonly LanguageManager languageManager; // Agrega esta propiedad
        private readonly MainMethods mainMethods;
        private readonly IUFavoritos iufavoritos;
        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set
            {
                nombreUsuario = value;
                // Aquí puedes llamar al método para cargar la imagen de perfil o realizar otras acciones basadas en el usuario.
                MostrarFotoPerfil(value);
                MostrarFavoritos(value);
                MostrarNovedades();
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

        private int idVinilo;
        public int IdVinilo
        {
            get { return idVinilo; }
            set
            {
                idVinilo = value;
                CargarInfoVinilo();

            }
        }

        public IUViniloU()
        {
            
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager(); // Inicializa la instancia de LanguageManager
            mainMethods = new MainMethods();
            iufavoritos = new IUFavoritos();
            // Suscribir a los eventos "Click" de los enlaces "Ver más..."
            lblverMasOft.MouseUp += VerMasOfertas_Click;
            lblverMasFav.MouseUp += VerMasFavoritos_Click;

            this.Loaded += MainWindow_Loaded;
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            languageManager.LanguageComboBox_SelectionChanged(sender, e, LanguageComboBox);
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainMethods.Window_Loaded(this);
        }



        private void Button_cerrarsesion(object sender, RoutedEventArgs e)
        {
            mainMethods.Button_cerrarsesion(this);
        }
        private void Button_Favoritos(object sender, RoutedEventArgs e)
        {
            mainMethods.Button_Favorites(nombreUsuario, this);
        }
            
        private void Button_Perfil(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Tienda(object sender, RoutedEventArgs e)
        {
            mainMethods.Button_Tienda(nombreUsuario, this);
        }

        private void Button_Home(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Carrito(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Buscar(object sender, RoutedEventArgs e)
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
            mainMethods.Button_Favorites(nombreUsuario, this);
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
            rotated = mainMethods.ImgPerfil_MouseUp(popupMarco, rotated, desplegable, this);
        }

        private void Button_Traducir(object sender, RoutedEventArgs e)
        {

        }

        private void IUSUARIO_Loaded(object sender, RoutedEventArgs e)
        {
            mainMethods.IUSUARIO_Loaded(dbManager, nombreUsuario, lblUltimaConex, ContentProperty, lblSaludo, this);
        }

        private void MostrarFotoPerfil(string usuario)
        {
            mainMethods.MostrarFotoPerfil(usuario, dbManager, imgPerfil, this);
        }
      
        private void MostrarFavoritos(string usuario)
        {

            iufavoritos.MostrarFavoritos(usuario, wrapPanelFavoritosP);
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
            iufavoritos.CambiarEstadoToggleButton(toggleButton, isChecked);
        }

        private void portadaFavoritos(MySqlDataReader reader, StackPanel stackPanel)
        {
            iufavoritos.portadaFavoritos(reader, stackPanel);
        }

        private void toggleFavoritos(MySqlDataReader reader, WrapPanel horizontalWrapPanel, StackPanel stackPanel) {
            
            iufavoritos.toggleFavoritos(reader, horizontalWrapPanel, stackPanel);
        }

        private void toggleNovedades(MySqlDataReader reader, WrapPanel horizontalWrapPanel, StackPanel stackPanel)
        {
            //Crear borde con color hexadecimal
            Border border = new Border();
            border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE4E4E4"));
            border.CornerRadius = new CornerRadius(10);
            border.Margin = new Thickness(left: 25, top: 30, right: 20, bottom: 50);

            //Crear toggleButton para cada vinilo que encuentre en favoritos y sus eventos
            ToggleButton toggleButton = new ToggleButton();
            toggleButton.Content = "Eliminar";
            toggleButton.Tag = reader["Idvinilo"]; // Almacena el ID del vinilo en el Tag del botón
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
            BitmapImage buttonImageBitmapImage = new BitmapImage(new Uri("../../Assets/Images/New.png", UriKind.Relative));
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
            iufavoritos.tituloFavoritos(titulo, stackPanel);

        }

        private void precioFavoritos(String precio, StackPanel stackPanel)
        {
            iufavoritos.precioFavoritos(precio, stackPanel);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void CargarInfoVinilo()
        {
            Console.WriteLine("ID VINILO: " + IdVinilo);
            lblTiulo.Content = IdVinilo;
        }

        private void MostrarNovedades()
        {
            try
            {
                string query = "SELECT ivc.Precio, v.Titulo, v.Portada, v.Idvinilo " +
                               "FROM vinilos v " +
                               "JOIN infoVinilosCompra ivc ON v.Idvinilo = ivc.Idvinilo " +
                               "ORDER BY v.Idvinilo DESC"; // Ordenar por ID descendente

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
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
                            Console.WriteLine(titulo + " " + precio);
                            StackPanel stackPanel = new StackPanel();
                            stackPanel.Margin = new Thickness(left: 0, top: 15, right: 0, bottom: 10);

                            //Llamadas a cargar Titulo y Precio
                            portadaFavoritos(reader, stackPanel);
                            toggleNovedades(reader, wrapPanelNovedadesP, stackPanel);
                            tituloFavoritos(titulo, stackPanel);
                            precioFavoritos(precio, stackPanel);

                            // Agrega un salto de línea después de cada número de columnas especificado
                            wrapPanelNovedadesP.Children.Add(horizontalWrapPanel);
                            horizontalWrapPanel = new WrapPanel();
                            horizontalWrapPanel.Orientation = Orientation.Horizontal;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar novedades: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }

    }
}