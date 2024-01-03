using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public partial class IUArtistaU : Window
    {

        private bool rotated = true; //Variable control menu desplegable
        private string nombreUsuario; // Agrega esta propiedad
        private readonly DatabaseManager dbManager;
        private readonly LanguageManager languageManager; // Agrega esta propiedad
        private readonly MainMethods mainMethods;
        TimeSpan lastPosition = TimeSpan.Zero;
        private readonly IUFavoritos iufavoritos;
        private Artista artistaPrincipal;
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


        private Vinilo viniloPrincipal;

        public Vinilo ViniloPrincipal
        {
            get { return viniloPrincipal; }
            set
            {
                viniloPrincipal = value;
                CargarArtistaDesdeBaseDeDatos(viniloPrincipal.Artista);
                CargarInfoArtista();
            }
        }






        public IUArtistaU()
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

        private void MainWindow_LanguageChanged(object sender, EventArgs e)
        {
            lblSaludo.Content = (string)Application.Current.FindResource("Saludo");
            lblUltimaConex.Content = (string)Application.Current.FindResource("UltimaConexion");
            refreshGroupLanguage();

        }

        private void refreshGroupLanguage()
        {
            if (artistaPrincipal.IsGroup)
            {
                Fecha.Content = (string)Application.Current.FindResource("FechaCreacion");
            }
            else
            {
                Fecha.Content = (string)Application.Current.FindResource("FechaNacimiento");
            }
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            languageManager.LanguageComboBox_SelectionChanged(sender, e, LanguageComboBox);
            if (artistaPrincipal != null)
            {
                // Actualiza el contenido del control "Stock" con el nuevo idioma
                refreshGroupLanguage();
            }
            

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

        private void volverTienda_Click(object sender, RoutedEventArgs e)
        {
            mainMethods.volverTienda(NombreUsuario, ViniloPrincipal, this);
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

        private void CargarArtistaDesdeBaseDeDatos(String idartista)
        {


            try
            {
                string query = "SELECT a.*, arrss.instagram, arrss.x, arrss.facebook, arrss.youtube, GROUP_CONCAT(IFNULL(gc.Componente, '') SEPARATOR ', ') AS Componentes, " +
               "COUNT(amg.Artista) AS CantidadMeGustas FROM artistas a " +
               "JOIN artistaRRSS arrss ON a.Artista = arrss.artista " +
               "LEFT JOIN artistasMGs amg ON a.Artista = amg.Artista " +
               "LEFT JOIN grupoComponentes gc ON a.Artista = gc.Grupo " +
               "WHERE a.Artista = @Artista GROUP BY a.Artista";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    dbManager.Connection.Open();
                    cmd.Parameters.AddWithValue("@Artista", idartista);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Artista artista = new Artista
                            {

                                NombreArtistico = reader["Artista"].ToString(),
                                NombreReal = reader["NombreReal"].ToString(),
                                IsGroup = Convert.ToBoolean(reader["isGroup"]),
                                Fecha = (DateTime)reader["FechaNacimiento"],
                                Genero = reader["Genero"].ToString(),
                                Instagram = reader["instagram"].ToString(),
                                X = reader["x"].ToString(),
                                Facebook = reader["facebook"].ToString(),
                                Youtube = reader["youtube"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                NumeroMGS = Convert.ToInt32(reader["CantidadMeGustas"]),
                                Componentes = new List<string>()


                            };
                            if (reader["Imagen"] != DBNull.Value && reader["Imagen"] != null)
                            {
                                artista.Imagen = (byte[])reader["Imagen"];
                            }
                            else
                            {
                                // Si no hay imagen en la base de datos, asigna la imagen por defecto
                                artista.Imagen = ObtenerBytesDesdeImagenPorDefecto();
                            }
                            //Agregar canción al vinilo actual si hay una
                            string componentes = reader["Componentes"].ToString();
                            if (!string.IsNullOrEmpty(componentes))
                            {
                                // Dividir la cadena en canciones usando la coma como separador
                                string[] componentesArray = componentes.Split(',');

                                // Agregar cada canción a la lista del vinilo
                                foreach (string componenteIndividual in componentesArray)
                                {
                                    artista.Componentes.Add(componenteIndividual.Trim());  // Trim para eliminar posibles espacios en blanco alrededor de cada canción
                                }
                            }
                            artistaPrincipal = artista;
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
        private byte[] ObtenerBytesDesdeImagenPorDefecto()
        {

            string rutaImagenPorDefecto = "../../Assets/Images/PerfilArtista.png";

            if (File.Exists(rutaImagenPorDefecto))
            {
                return File.ReadAllBytes(rutaImagenPorDefecto);
            }

            // Si la imagen por defecto no está disponible, puedes devolver un array de bytes vacío o manejarlo de otra manera
            return new byte[0];
        }

        private void CargarInfoArtista()
        {
            
            lblTiulo.Content = artistaPrincipal.NombreArtistico;
            imgArtista.Source = artistaPrincipal.Caratula;
            lblGenero.Content = artistaPrincipal.Genero;
            textbDescripcion.Text = artistaPrincipal.Descripcion;
            nMegustas.Content = artistaPrincipal.NumeroMGS;


            //Mostrar solo las redes sociales que tenga el artista
            if (artistaPrincipal.Instagram != null && artistaPrincipal.Instagram!="") { IG.Visibility = Visibility.Visible; }
            if (artistaPrincipal.X != null && artistaPrincipal.X != "") { X.Visibility = Visibility.Visible; }
            if (artistaPrincipal.Youtube != null && artistaPrincipal.Youtube != "") { YT.Visibility = Visibility.Visible; }
            if (artistaPrincipal.Facebook != null && artistaPrincipal.Facebook != "") { FB.Visibility = Visibility.Visible; }

            if (comprobarMG())
            {
                imgMG.Source = new BitmapImage(new Uri("../../Assets/Images/MG.png", UriKind.Relative));
            }
            if (comprobarFav())
            {
                imgFav.Source = new BitmapImage(new Uri("../../Assets/Images/corazonColoreadoB.png", UriKind.Relative));
            }

            if(artistaPrincipal.IsGroup)
            {
                Componentes.Visibility = Visibility.Visible;
                NombreReal.Visibility = Visibility.Collapsed;
                FechaValor.Content = artistaPrincipal.Fecha.Year;
            }
            else
            {
                Componentes.Visibility = Visibility.Collapsed;
                NombreReal.Visibility = Visibility.Visible;
                NombreRealValor.Content = artistaPrincipal.NombreReal;
                FechaValor.Content = artistaPrincipal.Fecha;
            }
            
                   
            foreach (string componente in artistaPrincipal.Componentes)
            {
                lstComponentes.Items.Add(componente);
            }
            cargarDiscografia();
            refreshGroupLanguage();

        }

        private void cargarDiscografia()
        {
            List<Vinilo> vinilos = new List<Vinilo>();
            string query = "SELECT * FROM vinilos WHERE artista = @Artista";
            dbManager.Connection.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
            {
                cmd.Parameters.AddWithValue("@Artista", artistaPrincipal.NombreArtistico);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Vinilo vinilo = new Vinilo
                        {
                            Idvinilo = Convert.ToInt32(reader["Idvinilo"]),
                            Titulo = reader["Titulo"].ToString(),
                            Genero = reader["Genero"].ToString(),
                            
                            Portada = (byte[])reader["Portada"],


                        };
                        vinilos.Add(vinilo);
                    } 
                }
            }
            dbManager.Connection.Close();
            foreach (Vinilo vinilo in vinilos)
            {
                lstDiscos.Items.Add(vinilo);
            }
            
            lstDiscos.SelectionChanged += LstVinilos_SelectionChanged;

        }

        private void LstVinilos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Obtener el vinilo seleccionado
            Vinilo vinilo = (Vinilo)lstDiscos.SelectedItem;

            // Verificar si se seleccionó un elemento
            if (vinilo != null)
            {
                // Llamar al método volverTienda con los parámetros necesarios
                mainMethods.newiuVinilos(nombreUsuario, vinilo.Idvinilo , this);
            }
        }

            private void linkRRSS(object sender, RoutedEventArgs e)
        {
            string url="";
            if (sender == X)
            {
                url = artistaPrincipal.X;
            }
            else if (sender == FB)
            {
                url = artistaPrincipal.Facebook;
            }
            else if (sender == IG)
            {
                url = artistaPrincipal.Instagram;
            }
            else if (sender == YT)
            {
                url = artistaPrincipal.Youtube;
            }
            

            // Abre el enlace en el navegador web predeterminado
            Process.Start(new ProcessStartInfo(url));
        }


       

        private bool comprobarMG()
        {
            string query = "SELECT COUNT(*) FROM artistasMGs WHERE artista = @Artista AND usuario = @Usuario";
            dbManager.Connection.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
            {
                cmd.Parameters.AddWithValue("@Artista", artistaPrincipal.NombreArtistico);
                cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);

                // Ejecutar la consulta y obtener el resultado
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                dbManager.Connection.Close();
                // Si count es mayor que 0, significa que hay una fila que cumple con los criterios
                return count > 0;
            }
            
        }

        private bool comprobarFav()
        {
            string query = "SELECT COUNT(*) FROM artistasFavoritos WHERE Artista = @Artista AND Usuario = @Usuario";
            dbManager.Connection.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
            {
                cmd.Parameters.AddWithValue("@Artista", artistaPrincipal.NombreArtistico);
                cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);

                // Ejecutar la consulta y obtener el resultado
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                dbManager.Connection.Close();
                // Si count es mayor que 0, significa que hay una fila que cumple con los criterios
                return count > 0;
            }

        }

        private void anadirMG(object sender, RoutedEventArgs e)
        {

            string query = "INSERT INTO artistasMGs (Artista, Usuario) VALUES (@Artista, @Usuario)";
            Boolean ismg = comprobarMG();
            
            if (!ismg) //Si no le ha dado mg se guarda el mg
            {
                dbManager.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    cmd.Parameters.AddWithValue("@Artista", artistaPrincipal.NombreArtistico);
                    cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    cmd.ExecuteNonQuery();
                }
                dbManager.Connection.Close();
                imgMG.Source = new BitmapImage(new Uri("../../Assets/Images/MG.png", UriKind.Relative));
                nMegustas.Content = Convert.ToInt32(nMegustas.Content) + 1;
            }
            else
            {
                string deleteQuery = "DELETE FROM artistasMGs WHERE Artista = @Artista AND Usuario = @Usuario";

                dbManager.Connection.Open();
                using (MySqlCommand cmdDelete = new MySqlCommand(deleteQuery, dbManager.Connection))
                {
                    cmdDelete.Parameters.AddWithValue("@Artista", artistaPrincipal.NombreArtistico);
                    cmdDelete.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    cmdDelete.ExecuteNonQuery();
                }
                dbManager.Connection.Close();

                // Actualizar la interfaz de usuario u otros elementos según sea necesario
                imgMG.Source = new BitmapImage(new Uri("../../Assets/Images/MGDesactiv.png", UriKind.Relative));
                nMegustas.Content = Convert.ToInt32(nMegustas.Content) - 1;
            }

        }


        private void anadirFav(object sender, RoutedEventArgs e)
        {

            string query = "INSERT INTO artistasFavoritos (Artista, Usuario) VALUES (@Artista, @Usuario)";
            Boolean isfav = comprobarFav();

            if (!isfav) //Si no le ha dado mg se guarda el mg
            {
                dbManager.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    cmd.Parameters.AddWithValue("@Artista", artistaPrincipal.NombreArtistico);
                    cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    cmd.ExecuteNonQuery();
                }
                dbManager.Connection.Close();
                imgFav.Source = new BitmapImage(new Uri("../../Assets/Images/corazonColoreadoB.png", UriKind.Relative));

            }
            else
            {
                string deleteQuery = "DELETE FROM artistasFavoritos WHERE Artista = @Artista AND Usuario = @Usuario";

                dbManager.Connection.Open();
                using (MySqlCommand cmdDelete = new MySqlCommand(deleteQuery, dbManager.Connection))
                {
                    cmdDelete.Parameters.AddWithValue("@Artista", artistaPrincipal.NombreArtistico);
                    cmdDelete.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    cmdDelete.ExecuteNonQuery();
                }
                dbManager.Connection.Close();

                // Actualizar la interfaz de usuario u otros elementos según sea necesario
                imgFav.Source = new BitmapImage(new Uri("../../Assets/Images/Corazon.png", UriKind.Relative));

            }

        }


        private void RefreshLanguageStock()
        {
            // Obtener las cadenas desde el recurso
            string actualmenteString = (string)Application.Current.FindResource("Actualmente");
            string productosString = (string)Application.Current.FindResource("Productos");

            // Supongamos que "vinilos.Cantidad" es el número que deseas incluir
            int cantidad = viniloPrincipal.Cantidad;

            // Combina las cadenas con el valor deseado
            string mensaje = $"{actualmenteString} {cantidad} {productosString}";

            // Establecer el contenido del control "Stock" con el mensaje combinado
            
        }

        private void volverButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainMethods.Button_Tienda(nombreUsuario, this);
        }
    }
}