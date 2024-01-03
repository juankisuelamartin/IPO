using MaterialDesignThemes.Wpf;
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
using System.Text.RegularExpressions;
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
    public partial class IUOfertasA : Window
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

        public IUOfertasA()
        {
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager();
            mainMethods = new MainMethods();

            spDetallesOferta.Visibility = Visibility.Hidden;
            CargarContenidoLista();
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



        private int InsertarOfertaEnBBDD(int lastIdvinilo)
        {

            try
            {
                dbManager.Connection.Open();
                using (MySqlTransaction transaction = dbManager.Connection.BeginTransaction())
                {
                    string inicio = FechaInicioPicker.Text;
                    string fin = FechaFinPicker.Text;
                    if(inicio == fin || DateTime.Parse(inicio) > DateTime.Parse(fin))
                    {
                        MessageBox.Show("La fecha de inicio no puede ser igual o mayor que la fecha de fin");
                        return -1;
                    }

                    // Inserta el nuevo vinilo
                    string query = "INSERT INTO ofertas (idVinilo, fechaInicio, fechaFin, descuento) " +
                    "VALUES (@lastIdvinilo, @FechaInicio, @FechaFin, @Descuento)";
                    MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@lastIdvinilo", lastIdvinilo);
                    Console.WriteLine("IDVINILO: " + lastIdvinilo);
                    cmd.Parameters.AddWithValue("@FechaInicio", inicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fin);
                    cmd.Parameters.AddWithValue("@Descuento", descuentoValor.Text);

                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar oferta: " + ex.Message);
                return -1;
            }
            finally
            {
                dbManager.Connection.Close();
            }
            return 0;
        }



        private void miEliminarOfertaB_Click(object sender, RoutedEventArgs e)
        {
            Vinilo viniloSeleccionado = (Vinilo)lstVinilos.SelectedItem;

            if (viniloSeleccionado != null)
            {
                // Mostrar un MessageBox para confirmar la eliminación
                MessageBoxResult result = MessageBox.Show(
                    "¿Seguro que quieres eliminar la oferta del vinilo seleccionado?\nEsta acción no se puede revertir.",
                    "Confirmación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Eliminar el vinilo de la tabla 'vinilos'
                        string deleteViniloQuery = "DELETE FROM ofertas WHERE Idvinilo = @Idvinilo";

                        using (MySqlCommand deleteCmd = new MySqlCommand(deleteViniloQuery, dbManager.Connection))
                        {
                            deleteCmd.Parameters.AddWithValue("@Idvinilo", viniloSeleccionado.Idvinilo);
                            dbManager.Connection.Open();
                            deleteCmd.ExecuteNonQuery();
                        }

                        

                        MessageBox.Show("Oferta eliminada correctamente de la base de datos.");
                        descuentoValor.Text = "";
                        FechaFinPicker.SelectedDate = null;
                        FechaInicioPicker.SelectedDate = null;
                        actualizarOferta.Content = "Insertar Oferta";


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar la oferta: " + ex.Message);
                    }
                    finally
                    {
                        dbManager.Connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona una oferta para eliminar.");
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
                lstVinilos.SelectedIndex = -1;
            }
        }



        private void LstVinilos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string query = "SELECT * FROM ofertas WHERE idVinilo = @IdVinilo";

            using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
            {
                dbManager.Connection.Open();

                int id = lstVinilos.SelectedIndex + 1;
                cmd.Parameters.AddWithValue("@IdVinilo", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Se encontró una fila

                        actualizarOferta.Content = "Actualizar Oferta";
                        FechaFinPicker.Text = reader["fechaFin"].ToString();
                        FechaInicioPicker.Text = reader["fechaInicio"].ToString();
                        descuentoValor.Text = reader["descuento"].ToString();
                    }
                    else
                    {
                        // No se encontró una fila, es un nuevo registro
                        actualizarOferta.Content = "Insertar Oferta";
                        FechaFinPicker.SelectedDate = null;
                        FechaInicioPicker.SelectedDate = null;
                        descuentoValor.Text = "";
                    }

                    reader.Close(); // Cierra el DataReader antes de ejecutar otra consulta
                    Vinilo viniloSeleccionado = (Vinilo)lstVinilos.SelectedItem;

                    MostrarDetallesOferta(viniloSeleccionado);
                }
            }

            dbManager.Connection.Close();
        }






        private void MostrarDetallesOferta(Vinilo vinilo)
        {
            // Muestra los detalles del vinilo en la interfaz de usuario
            if (vinilo != null)
            {
                if(lstVinilos.SelectedIndex != -1)
                {
                    Console.WriteLine("INDEX: " + lstVinilos.SelectedIndex);
                    spDetallesOferta.Visibility = Visibility.Visible;
                    lblTituloDetalles.Content = vinilo.Titulo;
                    precioValor.Content = vinilo.Precio + " €";
                    // Convertir la imagen de bytes a BitmapImage
                    BitmapImage bitmapImage = ConvertirBytesAImagen(vinilo.Portada);

                    // Asignar la imagen al control imgCaratula
                    imgCaratula.Source = bitmapImage; 
                }
                
            }
            else
            {
                // Hide the details panel if no vinyl is selected
                spDetallesOferta.Visibility = Visibility.Hidden;
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

      

        private void actualizarVinilo_Click(object sender, RoutedEventArgs e)
        {
            // Obtén los valores de los controles de entrada
            

            
            int viniloId;

            if (actualizarOferta.Content.ToString() == "Insertar Oferta")
            {
                // Verifica que los campos no estén vacíos
                if (string.IsNullOrWhiteSpace(FechaFinPicker.Text) || string.IsNullOrWhiteSpace(FechaInicioPicker.Text) || string.IsNullOrWhiteSpace(descuentoValor.Text))
                { 
                    MessageBox.Show("Por favor, asegúrate de que los campos esenciales esten llenos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                // Crea un nuevo objeto Vinilo con los valores obtenidos

                try
                {
                    dbManager.Connection.Open();
                    using (MySqlTransaction transaction = dbManager.Connection.BeginTransaction())
                    {
                        dbManager.Connection.Close();
                        int indiceSeleccionado = lstVinilos.SelectedIndex;

                        if (InsertarOfertaEnBBDD(indiceSeleccionado + 1) == -1) return;
                        // Insertar también las canciones
                        dbManager.Connection.Open();

                        dbManager.Connection.Close();


                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar oferta: " + ex.Message);
                }
                finally
                {
                    dbManager.Connection.Close();
                    
                }
                MessageBox.Show("Insertado con exito");
                CargarContenidoLista();



            }
            else
            {

                
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

                        //ActualizarListaVinilos();
                        ActualizarOfertaBBDD(viniloSeleccionado, viniloId);

                        
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

        private void ActualizarOfertaBBDD(Vinilo vinilo, int Id)
        {
            Console.WriteLine(Id);
            try
            {
                // Crea el comando SQL para la actualización
                string query =
                    "UPDATE ofertas SET fechaInicio = @FechaInicio, fechaFin = @FechaFin, descuento = @Descuento "+
                    "WHERE idVinilo = @IdVinilo";

                string inicio = FechaInicioPicker.Text;
                string fin = FechaFinPicker.Text;
                if (inicio == fin || DateTime.Parse(inicio) > DateTime.Parse(fin))
                {
                    MessageBox.Show("La fecha de inicio no puede ser igual o mayor que la fecha de fin");
                    return;
                }
                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    // Asigna los parámetros
                    cmd.Parameters.AddWithValue("@FechaInicio", FechaInicioPicker.Text);
                    cmd.Parameters.AddWithValue("@FechaFin", FechaFinPicker.Text);
                    cmd.Parameters.AddWithValue("@Descuento", descuentoValor.Text);

                    //cmd.Parameters.AddWithValue("@Canciones", string.Join(",", vinilo.Canciones));
                    cmd.Parameters.AddWithValue("@IdVinilo", Id); // Asume que Id es la clave primaria

                    dbManager.Connection.Open();

                    // Ejecuta la actualización
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // La actualización se realizó con éxito
                        MessageBox.Show("Oferta actualizada con éxito", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // No se actualizó ningún registro
                        MessageBox.Show("No se pudo actualizar la oferta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    dbManager.Connection.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar oferta: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }
        }


        private void MostrarFotoPerfil(string usuario)
        {
            mainMethods.MostrarFotoPerfil(usuario, dbManager, imgPerfil, this);

        }

        private void descuentoValor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (descuentoValor.Text != "")
            {
                precioNuevo.Visibility = Visibility.Visible;
                nuevoPrecio.Visibility = Visibility.Visible;
                double descuento = Convert.ToDouble(descuentoValor.Text) / 100;

                Console.WriteLine("DESCUENTO: " + descuento + " PRECIO: " + precioValor.Content.ToString());
                double precio = Convert.ToDouble(Regex.Replace(precioValor.Content.ToString(), " €", ""));
                Console.WriteLine(precio);

                precioNuevo.Content = precio - (precio * descuento) + " €";
            }
            else {
                precioNuevo.Visibility = Visibility.Collapsed;
                nuevoPrecio.Visibility = Visibility.Collapsed;
            }

        }
    }
}
