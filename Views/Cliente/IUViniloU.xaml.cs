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

    public partial class IUViniloU : Window
    {
        private Vinilo viniloPrincipal;
        private bool rotated = true; //Variable control menu desplegable
        private string nombreUsuario; // Agrega esta propiedad
        private readonly DatabaseManager dbManager;
        private readonly LanguageManager languageManager; // Agrega esta propiedad
        private readonly MainMethods mainMethods;
        TimeSpan lastPosition = TimeSpan.Zero;
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

        private int idVinilo;
        public int IdVinilo
        {
            get { return idVinilo; }
            set
            {
                idVinilo = value;
                CargarViniloDesdeBaseDeDatos(idVinilo);
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
            this.Loaded += MainWindow_Loaded;
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            languageManager.LanguageComboBox_SelectionChanged(sender, e, LanguageComboBox);
            if (viniloPrincipal != null)
            {
                // Actualiza el contenido del control "Stock" con el nuevo idioma
                RefreshLanguageStock();
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

        private void CargarViniloDesdeBaseDeDatos(int idVinilo)
        {


            try
            {
                string query = "SELECT v.*, ivc.Precio, ivc.Cantidad, COUNT(vm.idVinilo) AS CantidadMeGustas, "+
                                "GROUP_CONCAT(cv.Cancion SEPARATOR ', ') AS Canciones FROM vinilos v "+
                                "JOIN infoVinilosCompra ivc ON v.Idvinilo = ivc.Idvinilo "+
                                "LEFT JOIN cancionesVinilo cv ON v.Idvinilo = cv.Idvinilo "+
                                "LEFT JOIN vinilosMGs vm ON v.Idvinilo = vm.idVinilo "+
                                "WHERE v.Idvinilo = @Idvinilo GROUP BY v.Idvinilo";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    dbManager.Connection.Open();
                    cmd.Parameters.AddWithValue("@Idvinilo", idVinilo);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Vinilo vinilo = new Vinilo
                            {
                                Idvinilo = Convert.ToInt32(reader["Idvinilo"]),
                                Titulo = reader["Titulo"].ToString(),
                                Genero = reader["Genero"].ToString(),
                                Artista = reader["Artista"].ToString(),
                                FechaSalida = reader["FechaSalida"].ToString(),
                                Pais = reader["Pais"].ToString(),
                                Sello = reader["Sello"].ToString(),
                                Formato = reader["Formato"].ToString(),
                                Fecha = Convert.ToInt32(reader["FechaSalida"]),
                                // El campo de la portada es un byte[] (mediumblob)
                                Precio = Convert.ToSingle(reader["Precio"]),
                                Canciones = new List<string>(),
                                Portada = (byte[])reader["Portada"],
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                NumeroMGS = Convert.ToInt32(reader["CantidadMeGustas"])
                                
                            };

                            if (!reader.IsDBNull(reader.GetOrdinal("Preview")))
                            {
                                viniloPrincipal.Audio = (byte[])reader["Preview"];
                            }
                            // Agregar canción al vinilo actual si hay una
                            string cancion = reader["Canciones"].ToString();
                            if (!string.IsNullOrEmpty(cancion))
                            {
                                // Dividir la cadena en canciones usando la coma como separador
                                string[] cancionesArray = cancion.Split(',');

                                // Agregar cada canción a la lista del vinilo
                                foreach (string cancionIndividual in cancionesArray)
                                {
                                    vinilo.Canciones.Add(cancionIndividual.Trim());  // Trim para eliminar posibles espacios en blanco alrededor de cada canción
                                }
                            }

                            viniloPrincipal = vinilo;

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

        private void CargarInfoVinilo()
        {
            
            lblTiulo.Content = viniloPrincipal.Titulo;
            imgVinilo.Source = viniloPrincipal.Caratula;
            lblArtista.Content = viniloPrincipal.Artista;
            lblPrecio.Content = viniloPrincipal.Precio + "€";
            nMegustas.Content = viniloPrincipal.NumeroMGS;
            if (comprobarMG())
            {
                imgMG.Source = new BitmapImage(new Uri("../../Assets/Images/MG.png", UriKind.Relative));
            }
            if (comprobarFav())
            {
                imgFav.Source = new BitmapImage(new Uri("../../Assets/Images/corazonColoreadoB.png", UriKind.Relative));
            }

            GeneroValor.Content = viniloPrincipal.Genero;
            FormatoValor.Content = viniloPrincipal.Formato;
            SelloValor.Content = viniloPrincipal.Sello;
            PaisValor.Content = viniloPrincipal.Pais;
            
            foreach (string cancion in viniloPrincipal.Canciones)
            {
                lstCanciones.Items.Add(cancion);
            }
            MEPreview.Source = viniloPrincipal.Preview;

            RefreshLanguageStock();



        }

        private void anadirCarrito_Click(object sender, RoutedEventArgs e)
        {
            int cantidad = Convert.ToInt32(cantidadInput.Text);

            if (cantidad > 0)
            {
                if (cantidad > viniloPrincipal.Cantidad)
                {
                    MessageBox.Show("Lo sentimos, actualmente solo hay " + viniloPrincipal.Cantidad + " productos de este tipo en stock.");
                    return;
                }
                // Verificar si el vinilo ya está en el carrito del usuario
                string queryVerificar = "SELECT COUNT(*) FROM carritoUsuario WHERE idVinilo = @IdVinilo AND usuario = @Usuario";
                dbManager.Connection.Open();
                using (MySqlCommand cmdVerificar = new MySqlCommand(queryVerificar, dbManager.Connection))
                {
                    cmdVerificar.Parameters.AddWithValue("@IdVinilo", idVinilo);
                    cmdVerificar.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    int count = Convert.ToInt32(cmdVerificar.ExecuteScalar());

                    if (count > 0)
                    {
                        // Si el vinilo ya está en el carrito, actualizar la cantidad
                        string queryActualizar = "UPDATE carritoUsuario SET cantidad = @Cantidad WHERE idVinilo = @IdVinilo AND usuario = @Usuario";
                        using (MySqlCommand cmdActualizar = new MySqlCommand(queryActualizar, dbManager.Connection))
                        {
                            cmdActualizar.Parameters.AddWithValue("@IdVinilo", idVinilo);
                            cmdActualizar.Parameters.AddWithValue("@Usuario", nombreUsuario);
                            cmdActualizar.Parameters.AddWithValue("@Cantidad", cantidad);

                            cmdActualizar.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Si el vinilo no está en el carrito, insertar un nuevo registro
                        string queryInsertar = "INSERT INTO carritoUsuario (idVinilo, usuario, cantidad) VALUES (@IdVinilo, @Usuario, @Cantidad)";
                        using (MySqlCommand cmdInsertar = new MySqlCommand(queryInsertar, dbManager.Connection))
                        {
                            cmdInsertar.Parameters.AddWithValue("@IdVinilo", idVinilo);
                            cmdInsertar.Parameters.AddWithValue("@Usuario", nombreUsuario);
                            cmdInsertar.Parameters.AddWithValue("@Cantidad", cantidad);

                            cmdInsertar.ExecuteNonQuery();
                        }
                    }
                }

                dbManager.Connection.Close();
                MessageBox.Show("Añadido al carrito");
            }
            else
            {
                MessageBox.Show("Introduce una cantidad válida");
            }
        }


        private void aumentarCantidad(object sender, RoutedEventArgs e)
        {
            cantidadInput.Text = (Convert.ToInt32(cantidadInput.Text) + 1).ToString();
        }
        private void disminuirCantidad(object sender, RoutedEventArgs e)
        {
            cantidadInput.Text = (Convert.ToInt32(cantidadInput.Text) - 1).ToString();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            MEPreview.Position = lastPosition;
            MEPreview.Play();  // Iniciar la reproducción desde el principio
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            MEPreview.Pause();  // Detener la reproducción
            lastPosition = MEPreview.Position;
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            MEPreview.Stop();  // Detener la reproducción
            MEPreview.Position = TimeSpan.Zero;  // Reiniciar la posición a cero
            MEPreview.Play();  // Iniciar la reproducción desde el principio
        }

        private bool comprobarMG()
        {
            string query = "SELECT COUNT(*) FROM vinilosMGs WHERE idVinilo = @IdVinilo AND usuario = @Usuario";
            dbManager.Connection.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
            {
                cmd.Parameters.AddWithValue("@IdVinilo", idVinilo);
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
            string query = "SELECT COUNT(*) FROM vinilosFavoritos WHERE idvinilo = @IdVinilo AND usuario = @Usuario";
            dbManager.Connection.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
            {
                cmd.Parameters.AddWithValue("@IdVinilo", idVinilo);
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

            string query = "INSERT INTO vinilosMGs (idVinilo, usuario) VALUES (@IdVinilo, @Usuario)";
            Boolean ismg = comprobarMG();
            
            if (!ismg) //Si no le ha dado mg se guarda el mg
            {
                dbManager.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    cmd.Parameters.AddWithValue("@IdVinilo", idVinilo);
                    cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    cmd.ExecuteNonQuery();
                }
                dbManager.Connection.Close();
                imgMG.Source = new BitmapImage(new Uri("../../Assets/Images/MG.png", UriKind.Relative));
                nMegustas.Content = Convert.ToInt32(nMegustas.Content) + 1;
            }
            else
            {
                string deleteQuery = "DELETE FROM vinilosMGs WHERE idVinilo = @IdVinilo AND usuario = @Usuario";

                dbManager.Connection.Open();
                using (MySqlCommand cmdDelete = new MySqlCommand(deleteQuery, dbManager.Connection))
                {
                    cmdDelete.Parameters.AddWithValue("@IdVinilo", idVinilo);
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

            string query = "INSERT INTO vinilosFavoritos (idvinilo, usuario) VALUES (@IdVinilo, @Usuario)";
            Boolean isfav = comprobarFav();

            if (!isfav) //Si no le ha dado mg se guarda el mg
            {
                dbManager.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    cmd.Parameters.AddWithValue("@IdVinilo", idVinilo);
                    cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    cmd.ExecuteNonQuery();
                }
                dbManager.Connection.Close();
                imgFav.Source = new BitmapImage(new Uri("../../Assets/Images/corazonColoreadoB.png", UriKind.Relative));

            }
            else
            {
                string deleteQuery = "DELETE FROM vinilosFavoritos WHERE idvinilo = @IdVinilo AND usuario = @Usuario";

                dbManager.Connection.Open();
                using (MySqlCommand cmdDelete = new MySqlCommand(deleteQuery, dbManager.Connection))
                {
                    cmdDelete.Parameters.AddWithValue("@IdVinilo", idVinilo);
                    cmdDelete.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    cmdDelete.ExecuteNonQuery();
                }
                dbManager.Connection.Close();

                // Actualizar la interfaz de usuario u otros elementos según sea necesario
                imgFav.Source = new BitmapImage(new Uri("../../Assets/Images/Corazon.png", UriKind.Relative));

            }

        }

        private void MainWindow_LanguageChanged(object sender, EventArgs e)
        {
            // Actualizar el contenido cuando cambie el idioma
            RefreshLanguageStock();
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
            Stock.Content = mensaje;
        }

        private void volverButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainMethods.Button_Tienda(nombreUsuario, this);
        }
    }
}