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
using System.Windows.Interop;
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
    public partial class IUArtistasA : Window
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

        public IUArtistasA()
        {
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager();
            mainMethods = new MainMethods();
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
            mainMethods.Button_HomeAdmin(NombreUsuario,this);
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
            mainMethods.ButtonHistorial(NombreUsuario, this);
        }
        private void Button_Ofertas(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonOfertas(NombreUsuario, this);
        }
        private void Button_Incidencias(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonIncidencias(NombreUsuario, this);
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




        private void BtnGestionVinilos_Click(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonGestionVinilos(NombreUsuario, this);
        }

        private void BtnGestionArtistas_Click(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonGestionArtistas(NombreUsuario, this);
            // Lógica para el botón Gestión Artistas
        }

        private void BtnGestionContacto_Click(object sender, RoutedEventArgs e)
        {
            mainMethods.ButtonGestionContacto(NombreUsuario, this);
            // Lógica para el botón Gestión Contacto
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

        private void Group_Checked(object sender, RoutedEventArgs e)
        {
            nombreRealInput.IsEnabled = false;
            componentesInput.IsEnabled = true;
            listComponentes.IsEnabled = true;
        }

        private void NoGroup_Checked(object sender, RoutedEventArgs e)
        {
            nombreRealInput.IsEnabled = true;
            componentesInput.IsEnabled = false;
            listComponentes.IsEnabled = false;
        }


        private void IUSUARIO_Loaded(object sender, RoutedEventArgs e)
        {

            mainMethods.IUSUARIO_Loaded(dbManager, NombreUsuario, lblUltimaConex, ContentProperty, lblSaludo, this);
        }

        private void miAniadirViniloB_Click(object sender, RoutedEventArgs e)
        {

            lstArtistas.SelectedItem = null;
            actualizarVinilo.Content = "Insertar Artista";
            // Obtén los valores de los controles de entrada
            string artista = artistaDetallesInput.Text;
            string nombrereal = nombreRealInput.Text;
            Boolean isGroup;
            if(isGroupInputNo.IsChecked == true)
            {
                isGroup = false;
            }
            else if(isGroupInputSi.IsChecked == true)
            {
                isGroup = true;
            }
            else
            {
                MessageBox.Show("Se debe marcar una de las opciones para saber si es un grupo o no:");
            }

            DateTime? fecha = FechaNacimiento.SelectedDate;
            if (fecha.HasValue)
            {
                // La fecha tiene un valor, puedes acceder a su propiedad Value
                DateTime fechaSeleccionada = fecha.Value;
            }
            else
            {

                return; // O realiza alguna otra acción según tus necesidades
            }
            string genero = generoInput.Text;
            string instagram = instagramInput.Text;
            string x = xInput.Text;
            string facebook = facebookInput.Text;
            string youtube = youtubeInput.Text;
            string canciones = componentesInput.Text;

            List<string> listaCanciones = listComponentes.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
            artistaDetallesInput.Text = "";
            nombreRealInput.Text = "";
            isGroupInputNo.IsChecked = true;
            isGroupInputNo.IsChecked = false;
            

            generoInput.Text = "";
            FechaNacimiento.SelectedDate = null;
            instagramInput.Text = "";
            xInput.Text = "";
            isGroupInputNo.IsChecked = true;
            facebookInput.Text = "";
            youtubeInput.Text = "";
            componentesInput.Text = "";
            descripcionInput.Text = "";
            string rutaRelativa = "pack://application:,,,/Assets/Images/PerfilArtista.png";
            MostrarImagen(rutaRelativa);
            listComponentes.Items.Clear();

            nombreRealInput.IsEnabled = true;
            componentesInput.IsEnabled = false;
            listComponentes.IsEnabled = false;


        }

       



       



        private void miEliminarViniloB_Click(object sender, RoutedEventArgs e)
        {
            Artista artistaSeleccionado = (Artista)lstArtistas.SelectedItem;

            if (artistaSeleccionado != null)
            {
                // Mostrar un MessageBox para confirmar la eliminación
                MessageBoxResult result = MessageBox.Show(
                    "¿Seguro que quieres eliminar el artista?\nEsta acción no se puede revertir.",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Eliminar el vinilo de la tabla 'vinilos'
                        string deleteViniloQuery = "DELETE FROM artistas WHERE Artista = @Artista";

                        using (MySqlCommand deleteCmd = new MySqlCommand(deleteViniloQuery, dbManager.Connection))
                        {
                            deleteCmd.Parameters.AddWithValue("@Artista", artistaSeleccionado.NombreArtistico);
                            dbManager.Connection.Open();
                            deleteCmd.ExecuteNonQuery();
                        }

                        // Remover el vinilo de la lista local
                        lstArtistas.Items.Remove(artistaSeleccionado);

                        MessageBox.Show("Artista eliminado correctamente de la base de datos.");
                        miAniadirViniloB_Click(null, null);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar el artista: " + ex.Message);
                    }
                    finally
                    {
                        dbManager.Connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un artista para eliminar.");
            }
        }




        private void CargarContenidoLista()
        {
            // Aquí deberías tener un método para obtener los datos de tus vinilos desde la base de datos
            List<Artista> artistas = CargarArtistasDesdeBaseDeDatos();
            
            // Limpia la lista antes de agregar nuevos elementos
            lstArtistas.Items.Clear();

            // Agrega cada vinilo a la lista
            foreach (Artista artista in artistas)
            {
                lstArtistas.Items.Add(artista);
            }

            // Asigna el evento de selección cambiada para mostrar los detalles del vinilo seleccionado
            lstArtistas.SelectionChanged += LstVinilos_SelectionChanged;

            // Selecciona el primer elemento por defecto
            if (lstArtistas.Items.Count > 0)
            {
                lstArtistas.SelectedIndex = 0;
            }
        }



        private void LstVinilos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            actualizarVinilo.Content = "Actualizar Artista";
            imagenInput.Source = null;
            // Muestra los detalles del vinilo seleccionado en la interfaz de usuario
            Artista artistaSeleccionado = (Artista)lstArtistas.SelectedItem;

            MostrarDetallesVinilo(artistaSeleccionado);
        }

        private void MostrarDetallesVinilo(Artista artista)
        {
            // Muestra los detalles del vinilo en la interfaz de usuario
            if (artista != null)
            {
                listComponentes.Items.Clear();
                artistaDetallesInput.Text = artista.NombreArtistico;
                Console.WriteLine(artista.IsGroup);
                if(artista.IsGroup == true)
                {
                    nombreRealInput.IsEnabled = false;
                    componentesInput.IsEnabled = true;
                    listComponentes.IsEnabled = true;
                    isGroupInputSi.IsChecked = true;
                }
                else
                {
                    nombreRealInput.IsEnabled = true;
                    componentesInput.IsEnabled = false;
                    listComponentes.IsEnabled = false;
                    isGroupInputNo.IsChecked = true;
                }



                nombreRealInput.Text = artista.NombreReal;
                Console.WriteLine(artista.Genero);
                ComboBoxItem genero = new ComboBoxItem();
                genero.Content = artista.Genero;
                generoInput.Items.Add(genero);
                generoInput.SelectedItem = genero;

                FechaNacimiento.SelectedDate = artista.Fecha;

                instagramInput.Text = artista.Instagram;
                xInput.Text = artista.X;
                facebookInput.Text = artista.Facebook;
                youtubeInput.Text = artista.Youtube;
                descripcionInput.Text = artista.Descripcion;
                
                // Convertir la imagen de bytes a BitmapImage
                BitmapImage bitmapImage = ConvertirBytesAImagen(artista.Imagen);

                // Asignar la imagen al control imgCaratula
                imgArtista.Source = bitmapImage;
                if (artista.Componentes != null)
                {
                    foreach (string componente in artista.Componentes)
                    {
                        ListBoxItem listBoxItem = new ListBoxItem();
                        listBoxItem.Content = componente;
                        listComponentes.Items.Add(listBoxItem);
                    }
                }

            }
        }

        private void listaComponentes_Click(object sender, RoutedEventArgs e)
        {
            // Obtiene el texto del TextBox
            string nuevoElemento = componentesInput.Text;

            // Añade el elemento a la lista
            ListBoxItem cancion = new ListBoxItem();
            cancion.Content = nuevoElemento;
            listComponentes.Items.Add(cancion);


            // Limpia el TextBox después de agregar el elemento
            componentesInput.Clear();
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


        private List<Artista> CargarArtistasDesdeBaseDeDatos()
        {
            List<Artista> listaArtistas = new List<Artista>();

            try
            {
                string query = "SELECT a.*, arrss.instagram, arrss.x, arrss.facebook, arrss.youtube, GROUP_CONCAT(IFNULL(gc.Componente, '') SEPARATOR ', ') AS Componentes " +
                               "FROM artistas a " +
                               "JOIN artistaRRSS arrss ON a.Artista = arrss.artista " +
                               "LEFT JOIN grupoComponentes gc ON a.Artista = gc.Grupo " +
                               "GROUP BY a.Artista";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    dbManager.Connection.Open();

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
                            
                            listaArtistas.Add(artista);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar artistas: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }

            return listaArtistas;
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

        private void InsertarComponentesBBDD(List<string> componentes, string grupo)
        {
            try
            {
                string insertComponenteQuery = "INSERT INTO grupoComponentes (Grupo, Componente) VALUES (@Grupo, @Componente)";

                foreach (var componente in componentes)
                {
                    using (MySqlCommand insertCmd = new MySqlCommand(insertComponenteQuery, dbManager.Connection))
                    {
                        insertCmd.Parameters.AddWithValue("@Grupo", grupo);
                        insertCmd.Parameters.AddWithValue("@Componente", componente);

                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar componentes: " + ex.Message);
            }
        }

        private void InsertarArtistaEnBBDD(Artista artista)
        {
            try
            {
                dbManager.Connection.Open();
                using (MySqlTransaction transaction = dbManager.Connection.BeginTransaction())
                {
                    // Inserta el nuevo artista
                    string query = "INSERT INTO artistas (Artista, NombreReal, Genero, FechaNacimiento, isGroup, Descripcion, Imagen)" +
                    "VALUES(@Artista, @NombreReal, @Genero, @FechaNacimiento, @isGroup, @Descripcion, @Imagen)";
                    MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@Artista", artista.NombreArtistico);
                    cmd.Parameters.AddWithValue("@NombreReal", artista.NombreReal);
                    cmd.Parameters.AddWithValue("@Genero", artista.Genero);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", artista.Fecha);
                    cmd.Parameters.AddWithValue("isGroup", artista.IsGroup);
                    cmd.Parameters.AddWithValue("@Descripcion", artista.Descripcion);
                    cmd.Parameters.AddWithValue("@Imagen", artista.Imagen);
                    cmd.ExecuteNonQuery();

                    // Inserta la información de redes sociales en artistaRRSS
                    string query2 = "INSERT INTO artistaRRSS (artista, instagram, x, facebook, youtube) VALUES (@artista, @instagram, @x, @facebook, @youtube);";
                    MySqlCommand cmd2 = new MySqlCommand(query2, dbManager.Connection);
                    cmd2.Transaction = transaction;
                    cmd2.Parameters.AddWithValue("@artista", artista.NombreArtistico);
                    cmd2.Parameters.AddWithValue("@instagram", artista.Instagram);
                    cmd2.Parameters.AddWithValue("@x", artista.X);
                    cmd2.Parameters.AddWithValue("@facebook", artista.Facebook);
                    cmd2.Parameters.AddWithValue("@youtube", artista.Instagram);
                    cmd2.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar artista: " + ex.Message);
            }
            finally
            {

                dbManager.Connection.Close();
            }
        }

        private void actualizarArtista_Click(object sender, RoutedEventArgs e)
        {
            // Obtén los valores de los controles de entrada
            string nombreArtistico = artistaDetallesInput.Text;
            string nombreReal = nombreRealInput.Text;
            Boolean isGroup=false;
            string instagram = instagramInput.Text;
            string x = xInput.Text;
            string facebook = facebookInput.Text;
            string youtube = youtubeInput.Text;

            if(isGroupInputNo.IsChecked == true)
            {
                isGroup = false;
            }
            else if(isGroupInputSi.IsChecked == true)
            {
                isGroup = true;
            }
            else
            {
                MessageBox.Show("Se debe marcar una de las opciones para saber si es un grupo o no:");
            }

            DateTime? fechaNacimiento = FechaNacimiento.SelectedDate;

            if (fechaNacimiento.HasValue)
            {
                // El objeto DateTime no es nulo, puedes proceder con más comprobaciones si es necesario.
            }
            else
            {
                MessageBox.Show("Se debe indicar una fecha");
                return;
            }

            string genero = generoInput.Text;
            string descripcion = descripcionInput.Text;


            if (actualizarVinilo.Content.ToString() == "Insertar Artista")
            {

                // Verifica que los campos no estén vacíos
                if (isGroupInputNo.IsChecked==true)
                {
                    if (string.IsNullOrWhiteSpace(nombreArtistico) || string.IsNullOrWhiteSpace(nombreReal) ||
                        string.IsNullOrWhiteSpace(genero) || string.IsNullOrWhiteSpace(descripcion))
                    {
                        MessageBox.Show("Por favor, asegúrate de que los campos esenciales esten llenos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(nombreArtistico) || 
                        string.IsNullOrWhiteSpace(genero) || string.IsNullOrWhiteSpace(descripcion))
                    {
                        MessageBox.Show("Por favor, asegúrate de que los campos esenciales esten llenos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Crea un nuevo objeto Vinilo con los valores obtenidos
                Artista artistainsertar = new Artista
                {
                    NombreArtistico = nombreArtistico,
                    NombreReal = nombreReal,
                    IsGroup = isGroup,              
                    Genero= genero,
                    Fecha = (DateTime)fechaNacimiento,
                    Descripcion = descripcion,
                    Instagram = instagram,
                    X = x,
                    Facebook = facebook,
                    Youtube = youtube
                };
                try
                {
                    dbManager.Connection.Open();
                    using (MySqlTransaction transaction = dbManager.Connection.BeginTransaction())
                    {
                        dbManager.Connection.Close();
                        List<string> listaComponentes = listComponentes.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
                        InsertarArtistaEnBBDD(artistainsertar);
                        // Insertar también las canciones
                        dbManager.Connection.Open();
                        InsertarComponentesBBDD(listaComponentes, artistainsertar.NombreArtistico);
                        dbManager.Connection.Close();


                        // Verifica si se ha seleccionado una nueva imagen
                        if (imagenInput.Source != null && imagenInput.Source is BitmapImage)
                        {
                            // Convierte la imagen a un formato que puedas almacenar en la base de datos
                            byte[] imagenBytes = ConvertirImagenAByteArray(imagenInput.Source as BitmapImage);

                            // Actualiza la imagen en la base de datos
                            ActualizarImagenBBDD(imagenBytes, nombreArtistico);
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar artista: " + ex.Message);
                    return;
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

                List<string> listaComponentes = listComponentes.Items.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();
                try
                {


                    // Verifica que tengas un identificador único del vinilo (puedes usar el índice seleccionado o algún otro identificador único)
                    int indiceSeleccionado = lstArtistas.SelectedIndex;

                    Artista artistaAntiguo = (Artista)lstArtistas.SelectedItem;
                    if (artistaAntiguo == null)
                    {
                        MessageBox.Show("El Artista ya no existe en el contexto actual.");
                    }
                    else
                    {
                        

                        // Obtén el vinilo seleccionado de la lista
                        Artista artistaSeleccionado = (Artista)lstArtistas.Items[indiceSeleccionado];

                        // Actualiza los datos del vinilo
                        artistaSeleccionado.NombreArtistico = nombreArtistico ;
                        artistaSeleccionado.NombreReal = nombreReal;
                        artistaSeleccionado.IsGroup = isGroup;
                        artistaSeleccionado.Genero = genero;
                        artistaSeleccionado.Fecha = (DateTime) fechaNacimiento;
                        artistaSeleccionado.Instagram = instagram;
                        artistaSeleccionado.X = x;
                        artistaSeleccionado.Facebook = facebook;
                        artistaSeleccionado.Youtube = youtube;
                        artistaSeleccionado.Descripcion = descripcion;
                        artistaSeleccionado.Componentes = listaComponentes;

                        // Actualiza también las canciones
                        ActualizarComponentesBBDD(listaComponentes, nombreArtistico);

                        //ActualizarListaVinilos();
                        ActualizarArtistasBBDD(artistaSeleccionado);
                      
                        // Verifica si se ha seleccionado una nueva imagen
                        if (imagenInput.Source != null && imagenInput.Source is BitmapImage)
                        {
                            // Convierte la imagen a un formato que puedas almacenar en la base de datos
                            byte[] imagenBytes = ConvertirImagenAByteArray(imagenInput.Source as BitmapImage);

                            // Actualiza la imagen en la base de datos
                            ActualizarImagenBBDD(imagenBytes, nombreArtistico);
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
                finally
                {
                    CargarContenidoLista();
                }
            }

        }

       
        private void ActualizarComponentesBBDD(List<string> componentes, String artista)
        {
            try
            {
                // Elimina las canciones antiguas asociadas a este vinilo
                string deleteQuery = "DELETE FROM grupoComponentes WHERE Grupo = @Grupo";

                using (MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, dbManager.Connection))
                {
                    deleteCmd.Parameters.AddWithValue("@Grupo", artista);

                    dbManager.Connection.Open();
                    deleteCmd.ExecuteNonQuery();
                }

                // Inserta las nuevas canciones asociadas a este vinilo con el mismo Idcancion
                string insertQuery = "INSERT INTO grupoComponentes (Grupo, Componente) VALUES (@Grupo, @Componente)";

                int Idcancion = 1; // Asigna un valor constante para Idcancion

                foreach (var componente in componentes)
                {
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, dbManager.Connection))
                    {
                        insertCmd.Parameters.AddWithValue("@Grupo", artista);
                        insertCmd.Parameters.AddWithValue("@Componente", componente);

                        insertCmd.ExecuteNonQuery();
                    }

                    // Incrementa el valor de Idcancion para la siguiente canción
                    Idcancion++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los componentes: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }

        private void miEliminarCancionB_Click(object sender, RoutedEventArgs e)
        {
            int indiceSeleccionado = listComponentes.SelectedIndex;


            if (indiceSeleccionado >= 0)
            {
                // Remueve el elemento de la colección Items del ListBox
                listComponentes.Items.RemoveAt(indiceSeleccionado);
            }


        }



        private void ActualizarImagenBBDD(byte[] imagenBytes, String artista)
        {
            try
            {
                // Crea el comando SQL para la actualización de la imagen
                string query = "UPDATE artistas SET Imagen = @Imagen WHERE Artista = @Artista";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    // Asigna los parámetros
                    cmd.Parameters.AddWithValue("@Imagen", imagenBytes);
                    cmd.Parameters.AddWithValue("@Artista", artista);

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


        private void ActualizarArtistasBBDD( Artista artista)
        {
            
            try
            {
                // Crea el comando SQL para la actualización
                string query =
            "UPDATE artistas a " +
            "JOIN artistaRRSS arrss ON a.Artista = arrss.artista " +
            "SET a.Artista = @Artista, a.NombreReal = @NombreReal, a.isGroup = @isGroup, " +
            "a.FechaNacimiento = @FechaNacimiento, a.Genero= @Genero, a.Descripcion = @Descripcion, " +
            "arrss.instagram = @instagram, arrss.x = @x, arrss.facebook = @facebook, arrss.youtube = @youtube " +

            "WHERE a.Artista = @Artista";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    // Asigna los parámetros
                    cmd.Parameters.AddWithValue("@Artista", artista.NombreArtistico);
                    cmd.Parameters.AddWithValue("@NombreReal", artista.NombreReal);
                    cmd.Parameters.AddWithValue("@isGroup", artista.IsGroup);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", artista.Fecha);
                    cmd.Parameters.AddWithValue("@Genero", artista.Genero);
                    cmd.Parameters.AddWithValue("@Descripcion", artista.Descripcion);
                    cmd.Parameters.AddWithValue("@instagram", artista.Instagram);
                    cmd.Parameters.AddWithValue("@x", artista.X);
                    cmd.Parameters.AddWithValue("@facebook", artista.Facebook);
                    cmd.Parameters.AddWithValue("@youtube", artista.Youtube);

                    //cmd.Parameters.AddWithValue("@Canciones", string.Join(",", vinilo.Canciones));

                    dbManager.Connection.Open();

                    // Ejecuta la actualización
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // La actualización se realizó con éxito
                        MessageBox.Show("Artista actualizado con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // No se actualizó ningún registro
                        MessageBox.Show("No se pudo actualizar el artista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    dbManager.Connection.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar artista: " + ex.Message);
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
            imgArtista.Source = null;
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

    }
}
