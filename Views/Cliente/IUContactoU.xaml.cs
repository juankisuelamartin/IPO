using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
using WpfApp1.resources.Dominio;
using WpfApp1.resources.StringResources;

namespace WpfApp1.Views
{

    public partial class IUContactoU : Window
    {
        private bool ImgChanged = false;
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

        public IUContactoU()
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
        private void Contacto_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mainMethods.ContactoU(NombreUsuario, this);
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

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = (TextBox)sender;
                int caretIndex = textBox.CaretIndex;

                // Insertar un salto de línea en la posición del cursor
                textBox.Text = textBox.Text.Insert(caretIndex, Environment.NewLine);

                // Mover el cursor después del salto de línea
                textBox.CaretIndex = caretIndex + Environment.NewLine.Length;

                // Consumir el evento para evitar que se añada un salto de línea adicional
                e.Handled = true;
            }
        }

        private void Contacto_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
            Captura.Source = null;
            try
            {
                // Crea un objeto BitmapImage desde la ruta de la imagen
                BitmapImage bitmapImage = new BitmapImage(new Uri(rutaImagen));

                // Asigna la imagen al control Image
                Captura.Source = bitmapImage;
                Captura.Width = 200;
                Captura.Height = 200;
                anadirCaptura.Visibility = Visibility.Collapsed;
                Contacto.Height = 200;
                ImgChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la imagen: " + ex.Message);
            }
        }

        private void Button_Enviar(object sender, RoutedEventArgs e)
        {
            if (txtConsulta.Text != null)
            {
                MessageBoxResult result = MessageBox.Show(
                       "¿Seguro que quieres enviar esta consulta?\n",
                       "Confirmación",
                       MessageBoxButton.YesNo
                       );
                try
                {
                    dbManager.Connection.Open();
                    using (MySqlTransaction transaction = dbManager.Connection.BeginTransaction())
                    {
                        if (result == MessageBoxResult.Yes)
                        {
                            string query = "INSERT INTO contacto (idConsulta, usuario, Mensaje, Captura, FechaConsulta, Asunto) " +
                                            "VALUES(@Id, @Usuario, @Mensaje, @Captura, @FechaConsulta, @Asunto)";

                            string query2 = "SELECT MAX(idConsulta) FROM contacto;";
                            MySqlCommand cmd2 = new MySqlCommand(query2, dbManager.Connection);
                            cmd2.Transaction = transaction;

                            int Id;
                            if (cmd2.ExecuteScalar() == DBNull.Value)
                            {
                                Id = 0;
                            }
                            else
                            {
                                Id = 1 + Convert.ToInt32(cmd2.ExecuteScalar());
                            }

                            dbManager.Connection.Close();

                            dbManager.Connection.Open();
                            MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@Id", Id);
                            cmd.Parameters.AddWithValue("@Usuario", NombreUsuario);
                            cmd.Parameters.AddWithValue("@Mensaje", txtConsulta.Text);

                            if(ImgChanged)
                            {
                                MemoryStream ms = new MemoryStream();
                                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create((BitmapImage)Captura.Source));
                                encoder.Save(ms);
                                byte[] bitmapData = ms.ToArray();
                                cmd.Parameters.AddWithValue("@Captura", bitmapData);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Captura", DBNull.Value);
                            }
                            cmd.Parameters.AddWithValue("@FechaConsulta", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Asunto", txtAsunto.Text);
                            cmd.ExecuteNonQuery();
                            transaction.Commit();

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al mandar consulta: " + ex.Message);
                }
                finally
                {
                    MessageBox.Show("Consulta realizada con exito");
                    dbManager.Connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Asegurese de rellenar todos los campos esenciales");
            }
        }
    }
}