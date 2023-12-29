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

namespace WpfApp1.Views.Cliente
{

    public partial class IUTiendaMenuU : Window
    {
        private List<Border> todosLosElementos = new List<Border>();
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

        public IUTiendaMenuU()
        {
            
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager(); // Inicializa la instancia de LanguageManager
            mainMethods = new MainMethods();
            iufavoritos = new IUFavoritos();
            // Suscribir a los eventos "Click" de los enlaces "Ver más..."
            MostrarNovedades();
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
            mainMethods.Button_Tienda(nombreUsuario, this);
        }
        private void Button_Tienda(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Home(object sender, RoutedEventArgs e)
        {
            mainMethods.Button_Home(nombreUsuario, this);
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
            rotated=mainMethods.ImgPerfil_MouseUp(popupMarco, rotated, desplegable, this);
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

        private void TxtBusqueda_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Cuando el usuario hace clic en el cuadro de texto, borra el texto por defecto
            if (txtBusqueda.Text == "Buscar...")
            {
                txtBusqueda.Text = string.Empty;
            }
        }

        private void TxtBusqueda_LostFocus(object sender, RoutedEventArgs e)
        {
            // Cuando el cuadro de texto pierde el foco y está vacío, restaura el texto por defecto
            if (string.IsNullOrWhiteSpace(txtBusqueda.Text))
            {
                txtBusqueda.Text = "Buscar...";
            }
        }

        private void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Buscar_Click(sender, e);
            }
        }

        private void TxtBusqueda_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Puedes realizar alguna acción adicional cuando el texto cambia, si es necesario
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Obtener el texto de búsqueda
                string textoBusqueda = txtBusqueda.Text.ToLower(); // Convertir a minúsculas para hacer la búsqueda sin distinción entre mayúsculas y minúsculas

                // Verificar si el texto de búsqueda está vacío
                if (string.IsNullOrWhiteSpace(textoBusqueda) || textoBusqueda.Equals ("buscar..."))
                {

                    // Mostrar todos los elementos nuevamente si el buscador está vacío
                    foreach (Border border in todosLosElementos)
                    {
                        border.Visibility = Visibility.Visible;
                    }

                    
                    return;
                }


                // Iterar a través de los elementos originales y ocultar/mostrar según la búsqueda
                foreach (UIElement elemento in wrapPanelVinilosP.Children)
                {
                    if (elemento is Border border)
                    {
                        // Obtener el StackPanel dentro del Border (suponiendo que esté en la propiedad Child)
                        if (border.Child is StackPanel stackPanel)
                        {
                            // Obtener el título del StackPanel
                            string titulo = ObtenerTitulo(stackPanel);
                            Console.WriteLine("TITULO: " + titulo + ", BUSQUEDA " + textoBusqueda);

                            // Comprobar si el título contiene el texto de búsqueda
                            if (!string.IsNullOrEmpty(titulo) && titulo.ToLower().Contains(textoBusqueda))
                            {
                                // Mostrar el Border estableciendo Visibility.Visible
                                border.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                // Ocultar el Border estableciendo Visibility.Collapsed
                                border.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }

                // Contar los elementos visibles
                int resultadosVisibles = wrapPanelVinilosP.Children.OfType<Border>().Count(b => b.Visibility == Visibility.Visible);

                // Mostrar un mensaje si no se encontraron resultados
                if (resultadosVisibles == 0)
                {
                    MessageBox.Show("No se encontraron resultados para la búsqueda.");
                }
                else
                {
                    MessageBox.Show("Se encontraron " + resultadosVisibles + " resultados para la búsqueda.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar la búsqueda: " + ex.Message);
            }
        }



        private string ObtenerTitulo(StackPanel stackPanel)
        {
            // Buscar el control dentro del StackPanel que contiene el título (supongamos que es un TextBlock)
            TextBlock tituloTextBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();

            // Extraer el valor del TextBlock si se encuentra
            if (tituloTextBlock != null)
            {
                return tituloTextBlock.Text;
            }

            return string.Empty;
        }



        private void bordesNovedades(MySqlDataReader reader, WrapPanel horizontalWrapPanel, StackPanel stackPanel)
        {
            //Crear borde con color hexadecimal
            Border border = new Border();
            border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE4E4E4"));
            border.CornerRadius = new CornerRadius(10);
            border.Margin = new Thickness(left: 25, top: 30, right: 25, bottom: 10);
            border.Width = 140;
            border.Tag = reader["Idvinilo"];
            border.Style = border.Style = (Style)FindResource("ManoStyleBorder");

            border.Child = stackPanel;
            // Asignar el evento clic al borde
            border.PreviewMouseDown += Elemento_PreviewMouseDown;
            horizontalWrapPanel.Children.Add(border);
        }

        private void Elemento_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Obtener el Border que generó el evento
            if (sender is Border clickedBorder)
            {
                // Obtener el IdVinilo del Tag del Border
                if (clickedBorder.Tag != null && int.TryParse(clickedBorder.Tag.ToString(), out int idVinilo))
                {
                    // Crear y mostrar la nueva ventana
                    IUViniloU iuVinilo = new IUViniloU();
                    iuVinilo.NombreUsuario = NombreUsuario;
                    iuVinilo.IdVinilo = idVinilo; // Asignar el IdVinilo
                    mainMethods.Window_Closing(this);
                    iuVinilo.Show();
                    this.Close();
                }
            }
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

                            tituloFavoritos(titulo, stackPanel);
                            precioFavoritos(precio, stackPanel);
                            bordesNovedades(reader, wrapPanelVinilosP, stackPanel);



                            // Agrega un salto de línea después de cada número de columnas especificado
                            wrapPanelVinilosP.Children.Add(horizontalWrapPanel);
                            horizontalWrapPanel = new WrapPanel();
                            horizontalWrapPanel.Orientation = Orientation.Horizontal;

                            foreach (UIElement elemento in wrapPanelVinilosP.Children)
                            {
                                if (elemento is Border border)
                                {
                                    todosLosElementos.Add(border);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar tienda: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }




    }
}