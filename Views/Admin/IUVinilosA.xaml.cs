using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.BC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using WpfApp1.Helpers;
using WpfApp1.resources.Dominio;
using WpfApp1.resources.StringResources;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1.Views.Admin
{
    /// <summary>
    /// Lógica de interacción para IUVinilosA.xaml
    /// </summary>
    public partial class IUVinilosA : Window
    {
        TimeSpan lastPosition = TimeSpan.Zero;
        private bool rotated = true; //Variable control menu desplegable
        private string nombreUsuario; // Agrega esta propiedad
        private readonly LanguageManager languageManager; // Agrega esta propiedad
        private readonly DatabaseManager dbManager;
        private readonly MainMethods mainMethods;

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

        public IUVinilosA()
        {
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager();
            mainMethods = new MainMethods();
            CargarArtistasEnComboBox();
            CargarContenidoLista();
            miAniadirViniloB_Click(null, null);
            this.Loaded += MainWindow_Loaded;
            // Suscribir a los eventos "Click" de los enlaces "Ver más..."
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainMethods.Window_Loaded(this);
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            languageManager.LanguageComboBox_SelectionChanged(sender, e, LanguageComboBox);
        }




        private void Button_Home(object sender, RoutedEventArgs e)
        {
            mainMethods.Button_HomeAdmin(NombreUsuario, this);
        }

        private void VerMasNovedades_Click(object sender, MouseButtonEventArgs e)
        {
            mainMethods.Window_Closing(this);
        }

        private void VerMasOfertas(object sender, MouseButtonEventArgs e)
        {
            mainMethods.Window_Closing(this);
        }

        private void VerMasFavoritos_Click(object sender, MouseButtonEventArgs e)
        {
            mainMethods.Window_Closing(this);
        }

        private void Button_Perfil(object sender, RoutedEventArgs e)
        {
            mainMethods.Window_Closing(this);
        }

        private void Button_cerrarsesion(object sender, RoutedEventArgs e)
        {
            mainMethods.Button_cerrarsesion(this);
        }

        private void Button_Historial(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonHistorial(NombreUsuario,this);
        }
        private void Button_Ofertas(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonOfertas(NombreUsuario, this);
        }
        private void Button_Incidencias(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonIncidencias(NombreUsuario, this);
        }
        private void BtnGestionVinilos_Click(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonGestionVinilos(NombreUsuario, this);
        }

        private void BtnGestionArtistas_Click(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonGestionArtistas(NombreUsuario, this);
 
        }

        private void BtnGestionContacto_Click(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonGestionContacto(NombreUsuario, this);

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

            mainMethods.IUSUARIO_Loaded(dbManager, NombreUsuario, lblUltimaConex, ContentProperty, lblSaludo, this);
        }

        private void miAniadirViniloB_Click(object sender, RoutedEventArgs e)
        {
            
            lstVinilos.SelectedItem = null;
            actualizarVinilo.Content = "Insertar Vinilo";
            // Obtén los valores de los controles de entrada
            string titulo = tituloDetallesInput.Text;
            string artistas = artistasDetallesInput.Text;
            string precio = precioInput.Text;
            string formato = formatoInput.Text;
            string anio = anioInput.Text;
            string genero = generoInput.Text;
            string pais = paisInput.Text;
            string sello = selloInput.Text;
            string canciones = cancionesInput.Text;

            List<string> listaCanciones = listCanciones.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
                tituloDetallesInput.Text = "";
                artistasDetallesInput.Text = "";
                precioInput.Text = "";
                formatoInput.Text = "";
                anioInput.Text = "";
                generoInput.Text = "";
                paisInput.Text = "";
                selloInput.Text = "";
                cancionesInput.Text = "";
                string rutaRelativa = "pack://application:,,,/Assets/Images/ViniloX.jpg";
                MostrarImagen(rutaRelativa);
                listCanciones.Items.Clear(); 

               
            
        }

        private void InsertarCancionesBBDD(List<string> canciones, int Idvinilo, DatabaseManager dbmanager)
        {
            try
            {
                // Inserta las nuevas canciones asociadas a este vinilo con el mismo Idcancion
                string insertQuery = "INSERT INTO cancionesVinilo (Idvinilo, Cancion) VALUES (@Idvinilo, @Titulo)";

                int Idcancion = 1; // Asigna un valor constante para Idcancion

                foreach (var cancion in canciones)
                {
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, dbManager.Connection))
                    {
                        insertCmd.Parameters.AddWithValue("@Idvinilo", Idvinilo);
                        insertCmd.Parameters.AddWithValue("@Titulo", cancion);

                        insertCmd.ExecuteNonQuery();
                    }

                    // Incrementa el valor de Idcancion para la siguiente canción
                    Idcancion++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar las canciones: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }


        private void InsertarViniloEnBBDD(Vinilo vinilo, int lastIdvinilo)
        {

            try
            {
                dbManager.Connection.Open();
                using (MySqlTransaction transaction = dbManager.Connection.BeginTransaction())
                {

                    // Inserta el nuevo vinilo
                    string query = "INSERT INTO vinilos (Idvinilo, Titulo, Artista, Formato, FechaSalida, Genero, Pais, Sello) " +
                    "VALUES (@lastIdvinilo, @Titulo, @Artista, @Formato, @FechaSalida, @Genero, @Pais, @Sello)";
                    MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@lastIdvinilo", lastIdvinilo);
                    cmd.Parameters.AddWithValue("@Titulo", vinilo.Titulo);
                    cmd.Parameters.AddWithValue("@Artista", vinilo.Artista);
                    cmd.Parameters.AddWithValue("@Formato", vinilo.Formato);
                    cmd.Parameters.AddWithValue("@FechaSalida", vinilo.FechaSalida);
                    cmd.Parameters.AddWithValue("@Genero", vinilo.Genero);
                    cmd.Parameters.AddWithValue("@Pais", vinilo.Pais);
                    cmd.Parameters.AddWithValue("@Sello", vinilo.Sello);
                    cmd.ExecuteNonQuery();

                    // Inserta el precio en infoVinilosCompra
                    string query2 = "INSERT INTO infoVinilosCompra (Idvinilo, Precio, Cantidad) VALUES (@lastIdvinilo, @Precio, 1);";
                    MySqlCommand cmd2 = new MySqlCommand(query2, dbManager.Connection);
                    cmd2.Transaction = transaction;
                    cmd2.Parameters.AddWithValue("@lastIdvinilo", lastIdvinilo);
                    cmd2.Parameters.AddWithValue("@Precio", vinilo.Precio);
                    cmd2.ExecuteNonQuery();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar vinilo: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }



        private void miEliminarViniloB_Click(object sender, RoutedEventArgs e)
        {
            Vinilo viniloSeleccionado = (Vinilo)lstVinilos.SelectedItem;

            if (viniloSeleccionado != null)
            {
                // Mostrar un MessageBox para confirmar la eliminación
                MessageBoxResult result = MessageBox.Show(
                    "¿Seguro que quieres eliminar el vinilo?\nEsta acción no se puede revertir.",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Eliminar el vinilo de la tabla 'vinilos'
                        string deleteViniloQuery = "DELETE FROM vinilos WHERE Idvinilo = @Idvinilo";

                        using (MySqlCommand deleteCmd = new MySqlCommand(deleteViniloQuery, dbManager.Connection))
                        {
                            deleteCmd.Parameters.AddWithValue("@Idvinilo", viniloSeleccionado.Idvinilo);
                            dbManager.Connection.Open();
                            deleteCmd.ExecuteNonQuery();
                        }

                        // Remover el vinilo de la lista local
                        lstVinilos.Items.Remove(viniloSeleccionado);

                        MessageBox.Show("Vinilo eliminado correctamente de la base de datos.");
                        miAniadirViniloB_Click(null, null);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el vinilo: " + ex.Message);
                    }
                    finally
                    {
                        dbManager.Connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un vinilo para eliminar.");
            }
        }




        private void CargarContenidoLista()
        {
            // Aquí deberías tener un método para obtener los datos de tus vinilos desde la base de datos
            List<Vinilo> vinilos = CargarVinilosDesdeBaseDeDatos();

            // Limpia la lista antes de agregar nuevos elementos
            lstVinilos.Items.Clear();

            // Agrega cada vinilo a la lista
            foreach (Vinilo vinilo in vinilos)
            {
                lstVinilos.Items.Add(vinilo);
            }

            // Asigna el evento de selección cambiada para mostrar los detalles del vinilo seleccionado
            lstVinilos.SelectionChanged += LstVinilos_SelectionChanged;

            // Selecciona el primer elemento por defecto
            if (lstVinilos.Items.Count > 0)
            {
                lstVinilos.SelectedIndex = 0;
            }
        }



        private void LstVinilos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualizarVinilo.Content = "Actualizar Vinilo";
            imagenInput.Source = null;
            // Muestra los detalles del vinilo seleccionado en la interfaz de usuario
            Vinilo viniloSeleccionado = (Vinilo)lstVinilos.SelectedItem;
            
            MostrarDetallesVinilo(viniloSeleccionado);
        }

        public List<string> ObtenerListaArtistas()
        {
            List<string> artistas = new List<string>();

            try
            {
                string query = "SELECT Artista FROM artistas";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    dbManager.Connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nombreArtista = reader.GetString("Artista");
                            artistas.Add(nombreArtista);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la lista de artistas: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }

            return artistas;
        }

        private void CargarArtistasEnComboBox()
        {
            List<string> artistas = ObtenerListaArtistas();

            // Limpia los elementos existentes en el ComboBox
            artistasDetallesInput.Items.Clear();

            // Agrega los artistas recuperados al ComboBox
            foreach (string artista in artistas)
            {
                artistasDetallesInput.Items.Add(artista);
            }
        }

        private void MostrarDetallesVinilo(Vinilo vinilo)
        {
            // Muestra los detalles del vinilo en la interfaz de usuario
            if (vinilo != null)
            {
                listCanciones.Items.Clear();
                tituloDetallesInput.Text = vinilo.Titulo;

                ComboBoxItem artista = new ComboBoxItem();
                artista.Content = vinilo.Artista;
                artistasDetallesInput.Items.Add(artista);
                artistasDetallesInput.SelectedItem = artista;

                precioInput.Text = vinilo.Precio.ToString();

                ComboBoxItem formato = new ComboBoxItem();
                formato.Content = vinilo.Formato;
                formatoInput.Items.Add(formato);
                formatoInput.SelectedItem = formato;

                selloInput.Text = vinilo.Sello;
                anioInput.Text = vinilo.Fecha.ToString();

                ComboBoxItem genero = new ComboBoxItem();
                genero.Content = vinilo.Genero;
                generoInput.Items.Add(genero);
                generoInput.SelectedItem = genero;

                ComboBoxItem pais = new ComboBoxItem();
                pais.Content = vinilo.Pais;
                paisInput.Items.Add(pais);
                paisInput.SelectedItem = pais;

                
                // Convertir la imagen de bytes a BitmapImage
                BitmapImage bitmapImage = ConvertirBytesAImagen(vinilo.Portada);

                // Asignar la imagen al control imgCaratula
                imgCaratula.Source = bitmapImage;

                foreach (string cancion in vinilo.Canciones)
                {
                    ListBoxItem listBoxItem = new ListBoxItem();
                    listBoxItem.Content = cancion;
                    listCanciones.Items.Add(listBoxItem);
                }

            }
        }
        
        private void listaCanciones_Click(object sender, RoutedEventArgs e)
        {
            // Obtiene el texto del TextBox
            string nuevoElemento = cancionesInput.Text;

            // Añade el elemento a la lista
            ListBoxItem cancion = new ListBoxItem();
            cancion.Content = nuevoElemento;
            listCanciones.Items.Add(cancion);


            // Limpia el TextBox después de agregar el elemento
            cancionesInput.Clear();
        }

        private void SelectAudioFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de Audio|*.mp3;*.wav;*.wma|Todos los archivos|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Obtener la ruta del archivo de audio seleccionado
                string audioPath = openFileDialog.FileName;

                try
                {
                    // Validar que el archivo seleccionado tenga una extensión de audio permitida
                    string[] allowedExtensions = { ".mp3", ".wav", ".wma" };
                    string extension = System.IO.Path.GetExtension(audioPath);

                    if (allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        // Configurar la fuente del MediaElement para reproducir el audio
                        audioPlayer.Source = new Uri(audioPath);

                        // Mostrar el control de reproducción de audio
                        audioPlayer.Visibility = Visibility.Visible;
                        Reproductor.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show($"Formato de archivo no válido. Selecciona un archivo de audio permitido.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar y reproducir el audio: {ex.Message}");
                }
            }
        }
        private BitmapImage ConvertirBytesAImagen(byte[] bytesImagen)
        {
            if (bytesImagen == null || bytesImagen.Length == 0)
            {
                return null;
            }

            BitmapImage imagen = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(bytesImagen))
            {
                stream.Position = 0;
                imagen.BeginInit();
                imagen.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                imagen.CacheOption = BitmapCacheOption.OnLoad;
                imagen.UriSource = null;
                imagen.StreamSource = stream;
                imagen.EndInit();
            }

            return imagen;
        }


        private List<Vinilo> CargarVinilosDesdeBaseDeDatos()
        {
            List<Vinilo> listaVinilos = new List<Vinilo>();

            try
            {
                string query = "SELECT v.*, ivc.Precio, GROUP_CONCAT(cv.Cancion SEPARATOR ', ') AS Canciones " +
                               "FROM vinilos v " +
                               "JOIN infoVinilosCompra ivc ON v.Idvinilo = ivc.Idvinilo " +
                               "LEFT JOIN cancionesVinilo cv ON v.Idvinilo = cv.Idvinilo " +
                               "GROUP BY v.Idvinilo";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    dbManager.Connection.Open();

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
                                Portada = (byte[])reader["Portada"]
                            };

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

                            listaVinilos.Add(vinilo);
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

            return listaVinilos;
        }

        private byte[] ObtenerBytesDesdeImagenPorDefecto()
        {
            // Aquí deberías cargar tu imagen por defecto y convertirla a un array de bytes
            // A modo de ejemplo, si la imagen por defecto está en un archivo, podrías hacer algo así:

            string rutaImagenPorDefecto = "../../Assets/Images/ViniloX.png";

            if (File.Exists(rutaImagenPorDefecto))
            {
                return File.ReadAllBytes(rutaImagenPorDefecto);
            }

            // Si la imagen por defecto no está disponible, puedes devolver un array de bytes vacío o manejarlo de otra manera
            return new byte[0];
        }

        private void actualizarVinilo_Click(object sender, RoutedEventArgs e)
        {
            // Obtén los valores de los controles de entrada
            string titulo = tituloDetallesInput.Text;
            string artistas = artistasDetallesInput.Text;
            string precio = precioInput.Text;
            string formato = formatoInput.Text;
            string anio = anioInput.Text;
            string genero = generoInput.Text;
            string pais = paisInput.Text;
            string sello = selloInput.Text;
            int viniloId;

            if (actualizarVinilo.Content.ToString() == "Insertar Vinilo")
            {
                // Verifica que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(artistas) || string.IsNullOrWhiteSpace(precio) ||
                    string.IsNullOrWhiteSpace(formato) || string.IsNullOrWhiteSpace(anio) || string.IsNullOrWhiteSpace(genero) ||
                    string.IsNullOrWhiteSpace(pais) || string.IsNullOrWhiteSpace(sello))
                {
                    MessageBox.Show("Por favor, asegúrate de que los campos esenciales esten llenos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Crea un nuevo objeto Vinilo con los valores obtenidos
                Vinilo viniloinsertar = new Vinilo
                {
                    Titulo = titulo,
                    Artista = artistas,
                    Precio = float.Parse(precio),
                    Formato = formato,
                    FechaSalida = anio,
                    Genero = genero,
                    Pais = pais,
                    Sello = sello
                };
                try
                {
                    dbManager.Connection.Open();
                    using (MySqlTransaction transaction = dbManager.Connection.BeginTransaction())
                    {
                        string query3 = "SELECT MAX(Idvinilo) FROM vinilos;";
                        MySqlCommand cmd3 = new MySqlCommand(query3, dbManager.Connection);
                        cmd3.Transaction = transaction;
                        viniloId = 1 + Convert.ToInt32(cmd3.ExecuteScalar());
                        dbManager.Connection.Close();
                        List<string> listaCanciones = listCanciones.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
                        InsertarViniloEnBBDD(viniloinsertar, viniloId);
                        // Insertar también las canciones
                        dbManager.Connection.Open();
                        InsertarCancionesBBDD(listaCanciones, viniloId, dbManager);
                        dbManager.Connection.Close() ;

                        if (!string.IsNullOrEmpty(audioPlayer.Source?.AbsolutePath))
                        {
                            // Lógica para guardar la ruta del archivo de audio en la base de datos
                            GuardarRutaPreviewEnBD(audioPlayer.Source.AbsolutePath, viniloId);
                        }
                        // Verifica si se ha seleccionado una nueva imagen
                        if (imagenInput.Source != null && imagenInput.Source is BitmapImage)
                        {
                            // Convierte la imagen a un formato que puedas almacenar en la base de datos
                            byte[] imagenBytes = ConvertirImagenAByteArray(imagenInput.Source as BitmapImage);

                            // Actualiza la imagen en la base de datos
                            ActualizarImagenBBDD(imagenBytes, viniloId);
                        }
                        
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar vinilo: " + ex.Message);
                }
                finally
                {
                    dbManager.Connection.Close();
                    MessageBox.Show("Insertado con exito");
                    CargarContenidoLista();
                    miAniadirViniloB_Click(null, null);
                }
                

            }
            else
            {

                List<string> listaCanciones = listCanciones.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
                try
                {


                    // Verifica que tengas un identificador único del vinilo (puedes usar el índice seleccionado o algún otro identificador único)
                    int indiceSeleccionado = lstVinilos.SelectedIndex;

                    Vinilo viniloAntiguo = (Vinilo)lstVinilos.SelectedItem;
                    if (viniloAntiguo == null)
                    {
                        MessageBox.Show("El Vinilo ya no existe en el contexto actual.");
                    }
                    else
                    {
                        viniloId = viniloAntiguo.Idvinilo;

                        // Obtén el vinilo seleccionado de la lista
                        Vinilo viniloSeleccionado = (Vinilo)lstVinilos.Items[indiceSeleccionado];

                        // Actualiza los datos del vinilo
                        viniloSeleccionado.Titulo = titulo;
                        viniloSeleccionado.Artista = artistas;
                        viniloSeleccionado.Precio = float.Parse(precio);
                        viniloSeleccionado.Formato = formato;
                        viniloSeleccionado.FechaSalida = anio;
                        viniloSeleccionado.Genero = genero;
                        viniloSeleccionado.Pais = pais;
                        viniloSeleccionado.Sello = sello;
                        viniloSeleccionado.Canciones = listaCanciones;

                        // Actualiza también las canciones
                        ActualizarCancionesBBDD(listaCanciones, viniloId);

                        //ActualizarListaVinilos();
                        ActualizarVinilosBBDD(viniloSeleccionado, viniloId);
                        if (!string.IsNullOrEmpty(audioPlayer.Source?.AbsolutePath))
                        {
                            // Lógica para guardar la ruta del archivo de audio en la base de datos
                            GuardarRutaPreviewEnBD(audioPlayer.Source.AbsolutePath, viniloId);
                        }
                        // Verifica si se ha seleccionado una nueva imagen
                        if (imagenInput.Source != null && imagenInput.Source is BitmapImage)
                        {
                            // Convierte la imagen a un formato que puedas almacenar en la base de datos
                            byte[] imagenBytes = ConvertirImagenAByteArray(imagenInput.Source as BitmapImage);

                            // Actualiza la imagen en la base de datos
                            ActualizarImagenBBDD(imagenBytes, viniloId);
                        }
                    }

                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("El elemento ya no existe.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inesperado: {ex.Message}");
                }
            }

        }

        private void GuardarRutaPreviewEnBD(string rutaArchivo, int Idvinilo)
        {
            try
            {
                // Convierte el archivo de audio a bytes
                byte[] audioBytes = File.ReadAllBytes(rutaArchivo);

                // Inserta los bytes del audio asociados a este vinilo
                string insertQuery = "UPDATE vinilos SET Preview = @PreviewAudio WHERE Idvinilo = @Idvinilo";


                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, dbManager.Connection))
                {
                    insertCmd.Parameters.AddWithValue("@Idvinilo", Idvinilo);
                    insertCmd.Parameters.AddWithValue("@PreviewAudio", audioBytes);

                    dbManager.Connection.Open();
                    insertCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el audio en la base de datos: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }



        private void ActualizarCancionesBBDD(List<string> canciones, int Idvinilo)
        {
            try
            {
                // Elimina las canciones antiguas asociadas a este vinilo
                string deleteQuery = "DELETE FROM cancionesVinilo WHERE Idvinilo = @Idvinilo";

                using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, dbManager.Connection))
                {
                    deleteCmd.Parameters.AddWithValue("@Idvinilo", Idvinilo);

                    dbManager.Connection.Open();
                    deleteCmd.ExecuteNonQuery();
                }

                // Inserta las nuevas canciones asociadas a este vinilo con el mismo Idcancion
                string insertQuery = "INSERT INTO cancionesVinilo (Idvinilo, Cancion) VALUES (@Idvinilo, @Titulo)";

                int Idcancion = 1; // Asigna un valor constante para Idcancion

                foreach (var cancion in canciones)
                {
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, dbManager.Connection))
                    {
                        insertCmd.Parameters.AddWithValue("@Idvinilo", Idvinilo);
                        insertCmd.Parameters.AddWithValue("@Titulo", cancion);

                        insertCmd.ExecuteNonQuery();
                    }

                    // Incrementa el valor de Idcancion para la siguiente canción
                    Idcancion++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar las canciones: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }

        private void miEliminarCancionB_Click(object sender, RoutedEventArgs e)
        {
            int indiceSeleccionado = listCanciones.SelectedIndex;


            if (indiceSeleccionado >= 0)
            {
                // Remueve el elemento de la colección Items del ListBox
                listCanciones.Items.RemoveAt(indiceSeleccionado);
            }


        }
       


        private void ActualizarImagenBBDD(byte[] imagenBytes, int Id)
        {
            try
            {
                // Crea el comando SQL para la actualización de la imagen
                string query = "UPDATE vinilos SET Portada = @Imagen WHERE Idvinilo = @Id";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    // Asigna los parámetros
                    cmd.Parameters.AddWithValue("@Imagen", imagenBytes);
                    cmd.Parameters.AddWithValue("@Id", Id);

                    dbManager.Connection.Open();

                    // Ejecuta la actualización
                    cmd.ExecuteNonQuery();

                    // Cierra la conexión
                    dbManager.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar la imagen: " + ex.Message);
            }
        }

        private byte[] ConvertirImagenAByteArray(BitmapImage bitmapImage)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new JpegBitmapEncoder(); // Puedes cambiarlo según el formato de imagen que estés utilizando
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);

                return stream.ToArray();
            }
        }


        private void ActualizarVinilosBBDD(Vinilo vinilo, int Id)
        {
            Console.WriteLine(Id);
            try
            {
                // Crea el comando SQL para la actualización
                string query =
            "UPDATE vinilos v " +
            "JOIN infoVinilosCompra ivc ON v.Idvinilo = ivc.Idvinilo " +
            "SET v.Titulo = @Titulo, v.Artista = @Artista, ivc.Precio = @Precio, " +
            "v.Formato = @Formato, v.FechaSalida = @FechaSalida, v.Genero = @Genero, " +
            "v.Pais = @Pais, v.Sello = @Sello " +
            //TODO: METER CANCIONES//"v.Canciones = @Canciones " +
            "WHERE v.Idvinilo = @Id";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    // Asigna los parámetros
                    cmd.Parameters.AddWithValue("@Titulo", vinilo.Titulo);
                    cmd.Parameters.AddWithValue("@Artista", vinilo.Artista);
                    cmd.Parameters.AddWithValue("@Precio", vinilo.Precio);
                    cmd.Parameters.AddWithValue("@Formato", vinilo.Formato);
                    cmd.Parameters.AddWithValue("@FechaSalida", vinilo.FechaSalida);
                    cmd.Parameters.AddWithValue("@Genero", vinilo.Genero);
                    cmd.Parameters.AddWithValue("@Pais", vinilo.Pais);
                    cmd.Parameters.AddWithValue("@Sello", vinilo.Sello);
                    //cmd.Parameters.AddWithValue("@Canciones", string.Join(",", vinilo.Canciones));
                    cmd.Parameters.AddWithValue("@Id", Id); // Asume que Id es la clave primaria

                    dbManager.Connection.Open();

                    // Ejecuta la actualización
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // La actualización se realizó con éxito
                        MessageBox.Show("Vinilo actualizado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // No se actualizó ningún registro
                        MessageBox.Show("No se pudo actualizar el vinilo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    dbManager.Connection.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar vinilo: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }


        private void SeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Todos los archivos|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Obtiene la ruta del archivo seleccionado
                string rutaImagen = openFileDialog.FileName;

                // Muestra la imagen en un control Image
                MostrarImagen(rutaImagen);
            }
        }

        private void MostrarImagen(string rutaImagen)
        {
            // borramos anterior imagen;
            imagenInput.Source = null;
            imgCaratula.Source = null;
            try
            {
                // Crea un objeto BitmapImage desde la ruta de la imagen
                BitmapImage bitmapImage = new BitmapImage(new Uri(rutaImagen));

                // Asigna la imagen al control Image
                imagenInput.Source = bitmapImage;
                imagenInput.Width = 120;
                imagenInput.Height = 120;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen: " + ex.Message);
            }
        }

        private void MostrarFotoPerfil(string usuario)
        {
            mainMethods.MostrarFotoPerfil(usuario, dbManager, imgPerfil, this);

        }



        private void Play_Click(object sender, RoutedEventArgs e)
        {
            audioPlayer.Position = lastPosition;
            audioPlayer.Play();  // Iniciar la reproducción desde el principio
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            audioPlayer.Pause();  // Detener la reproducción
            lastPosition = audioPlayer.Position;
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            audioPlayer.Stop();  // Detener la reproducción
            audioPlayer.Position = TimeSpan.Zero;  // Reiniciar la posición a cero
            audioPlayer.Play();  // Iniciar la reproducción desde el principio
        }
    }
}
