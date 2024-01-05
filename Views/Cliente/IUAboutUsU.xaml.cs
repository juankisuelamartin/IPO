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

    public partial class IUAboutUsU : Window
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

        public IUAboutUsU()
        {
            
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager(); // Inicializa la instancia de LanguageManager
            mainMethods = new MainMethods();
            iufavoritos = new IUFavoritos();
            // Suscribir a los eventos "Click" de los enlaces "Ver más..."

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
        private void Contacto_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mainMethods.ContactoU(NombreUsuario, this);
        }

        private void IUSUARIO_Loaded(object sender, RoutedEventArgs e)
        {
            mainMethods.IUSUARIO_Loaded(dbManager, nombreUsuario, lblUltimaConex, ContentProperty, lblSaludo, this);
        }

        private void MostrarFotoPerfil(string usuario)
        {
            mainMethods.MostrarFotoPerfil(usuario, dbManager, imgPerfil, this);
        }
      
        

       

    }
}