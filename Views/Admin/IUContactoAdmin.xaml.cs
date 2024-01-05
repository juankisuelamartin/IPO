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
    public partial class IUContactoAdminA : Window
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
                CargarCarritoDesdeBaseDeDatos();

            }
        }

        public IUContactoAdminA()
        {
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
            dbManager = new DatabaseManager();
            languageManager = new LanguageManager();
            mainMethods = new MainMethods();

 
            CargarContenidoLista();
            this.Loaded += MainWindow_Loaded;

            // Suscribir a los eventos "Click" de los enlaces "Ver más..."
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
        private void Contacto_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mainMethods.ButtonGestionContacto(NombreUsuario, this);
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
        private void sobrenosotros_Click(object sender, RoutedEventArgs e)
        {
            mainMethods.sobreNosotros_Click(NombreUsuario, this);

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



       

        private void CargarContenidoLista()
        {/*
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
            */
        }



        private void lstHistorial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstContacto.SelectedItem != null)
            {
                // Accede al objeto vinculado al elemento seleccionado
                Consulta selectedConsulta = (Consulta)lstContacto.SelectedItem;


                // Llama al método x con el parámetro Id del Pedido seleccionado
                mainMethods.DetallesContactoA(NombreUsuario, selectedConsulta, this);
            }
        }



        private void CargarCarritoDesdeBaseDeDatos()
        {



            try
            {
                string query = "SELECT c.*, u.email, u.foto_perfil FROM contacto c JOIN usuarios u ON u.usuario = c.usuario";

                ;

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    dbManager.Connection.Open();
                    cmd.Parameters.AddWithValue("@IdPedido", IdPedido);


                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {


 
                            Consulta consulta = new Consulta
                            {
                                Id = Convert.ToInt32(reader["idConsulta"]),
                                usuario = reader["usuario"].ToString(),
                                Mensaje = reader["Mensaje"].ToString(),
                                Asunto = reader["Asunto"].ToString(),
                                // El campo de la portada es un byte[] (mediumblob)
                                Email = reader["email"].ToString(),
                                Fecha = Convert.ToDateTime(reader["FechaConsulta"]),



                            };

                            if (reader["Captura"] != null && reader["Captura"] != DBNull.Value) 
                            {
                                consulta.FotoConsulta =(byte[]) reader["Captura"];
                            }
                            else
                            {
                                consulta.FotoConsulta = null;
                            }

                            if (reader["foto_perfil"] != null && reader["foto_perfil"] != DBNull.Value)
                            {
                                consulta.Foto = (byte[])reader["foto_perfil"];
                            }
                            else
                            {
                                consulta.Foto = null;
                            }


                            // Agregar la instancia al ListBox
                            lstContacto.Items.Add(consulta);


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



        private void MostrarFotoPerfil(string usuario)
        {
            mainMethods.MostrarFotoPerfil(usuario, dbManager, imgPerfil, this);

        }

        
    }
}
