using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WpfApp1.resources.Dominio;
using WpfApp1.resources.StringResources;

namespace WpfApp1.Views
{

    public partial class IUHistorialDetallesU : Window
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

        private List<ItemCarrito> carrito = new List<ItemCarrito>(); 
        public List<ItemCarrito> Carrito
        {
            get { return carrito; }
            set
            {
                carrito = value;
            }
        }

        private float preciototal;
        public float Preciototal
        {
            get { return preciototal; }
            set
            {
                preciototal = value;
            }
        }

        private int idPedido;
        public int IdPedido
        {
            get { return idPedido; }
            set
            {
                Console.WriteLine("Valor IDPEDIDO: " + value);
                idPedido = value;

                CargarCarritoDesdeBaseDeDatos();

            }
        }

        //Control para saber a que ventana volver (iuArtista VERDADERO o iuTienda FALSO)
        private int volver = 0;
        public int Volver
        {
            get { return volver; }
            set
            {
                // Verifica si el valor es true (bool) y asigna 1 si es true, 0 si es false
                volver = value;
            }
        }




        public IUHistorialDetallesU()
        {
            
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager(); // Inicializa la instancia de LanguageManager
            mainMethods = new MainMethods();
            iufavoritos = new IUFavoritos();
            // Suscribir a los eventos "Click" de los enlaces "Ver más..."
            this.Loaded += MainWindow_Loaded;
            
            //CargarInfoCarrito();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            languageManager.LanguageComboBox_SelectionChanged(sender, e, LanguageComboBox);
           

        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainMethods.Window_Loaded(this);
        }

        private void Contacto_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mainMethods.ContactoU(NombreUsuario, this);
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
            mainMethods.Button_Home(nombreUsuario, this);
        }

        private void Button_Carrito(object sender, RoutedEventArgs e)
        {
            mainMethods.Button_Carrito(nombreUsuario, this);
        }

        private void Button_Buscar(object sender, RoutedEventArgs e)
        {

        }
        private void sobrenosotros_Click(object sender, MouseButtonEventArgs e)
        {
            mainMethods.sobreNosotros_Click(NombreUsuario, this);
        }


        private void VerMasFavoritos_Click(object sender, MouseButtonEventArgs e)
        {
            mainMethods.Button_Favorites(nombreUsuario, this);
        }


        private void Button_historialCompras(object sender, RoutedEventArgs e)
        {
            mainMethods.HistorialU(NombreUsuario, this);
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void CargarCarritoDesdeBaseDeDatos()
        {



            try
            {
                string query = "SELECT v.Titulo, v.Artista, v.Portada, dp.* FROM detallesPedido dp JOIN vinilos v ON v.Idvinilo = dp.IdVinilo WHERE dp.IdPedido = @IdPedido";

;

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    dbManager.Connection.Open();
                    cmd.Parameters.AddWithValue("@IdPedido", IdPedido);
                   

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {



                            Vinilo vinilo = new Vinilo
                            {
                                Idvinilo = Convert.ToInt32(reader["Idvinilo"]),
                                Titulo = reader["Titulo"].ToString(),
                                Artista = reader["Artista"].ToString(),
                                // El campo de la portada es un byte[] (mediumblob)
                                Precio = Convert.ToSingle(reader["PrecioProducto"]),
                                Portada = (byte[])reader["Portada"],
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),


                            };
                            int cantidad = Convert.ToInt32(reader["Cantidad"]);

                            // Crear una instancia de ItemCarrito con el vinilo y la cantidad
                            ItemCarrito item = new ItemCarrito
                            {
                                Vinilo = vinilo,  // Supongamos que 'vinilo' es una instancia de la clase Vinilo
                                Cantidad = cantidad // Supongamos que 'cantidad' es la cantidad que deseas agregar
                            };


                            // Agregar la instancia al ListBox
                            lstDetalles.Items.Add(item);
                            Console.WriteLine("TITULO: " + item.Vinilo.Titulo);
                            carrito.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar vinilos: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }


        }

        

        
    }
}