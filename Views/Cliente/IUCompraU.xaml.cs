using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

    public partial class IUCompraU : Window
    {

        private bool rotated = true; //Variable control menu desplegable
        private string nombreUsuario; // Agrega esta propiedad
        private readonly DatabaseManager dbManager;
        private readonly LanguageManager languageManager; // Agrega esta propiedad
        private readonly MainMethods mainMethods;
        private readonly IUFavoritos iufavoritos;
        private Usuario usuarioPrincipal;
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
                CargarDatosDesdeBaseDeDatos();
                CargarInfoUsuario();
            }
        }


        private float precioCarrito;
        public float PrecioCarrito
        {
            get { return precioCarrito; }
            set
            {
                precioCarrito = value;
                TotalValor.Content = precioCarrito.ToString("0.00") + " €";
            }
        }
        private float precioEnvio;
        public float PrecioEnvio
        {
            get { return precioEnvio; }
            set
            {
                precioEnvio = value;
               
            }
        }
        private float precioGarantia;
        public float PrecioGarantia
        {
            get { return precioGarantia; }
            set
            {
                precioGarantia = value;

            }
        }

        private List<ItemCarrito> carrito;
        public List<ItemCarrito> Carrito
        {
            get { return carrito; }
            set
            {
                carrito = value;
            }
        }



        public IUCompraU()
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

        }

        private void Contacto_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mainMethods.ContactoU(NombreUsuario, this);
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            languageManager.LanguageComboBox_SelectionChanged(sender, e, LanguageComboBox);

        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainMethods.Window_Loaded(this);
        }
        private void sobrenosotros_Click(object sender, MouseButtonEventArgs e)
        {
            mainMethods.sobreNosotros_Click(NombreUsuario, this);
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

        private void CargarDatosDesdeBaseDeDatos()
        {


            try
            {
                string query = "SELECT * FROM usuarios " +
               "WHERE usuario = @Usuario GROUP BY usuario";

                using (MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection))
                {
                    dbManager.Connection.Open();
                    cmd.Parameters.AddWithValue("@Usuario", nombreUsuario);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario usuario = new Usuario
                            {

                                NombreUsuario = reader["usuario"].ToString(),
                                NombreReal = reader["nombre"].ToString(),
                                Apellidos = reader["apellido"].ToString(),
                                Email = reader["email"].ToString(),



                            };

                            usuarioPrincipal = usuario;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la información del usuario: " + ex.Message);
            }
            finally
            {
                dbManager.Connection.Close();
            }


        }


        private void CargarInfoUsuario()
        {

            NombreyApellido.Content = usuarioPrincipal.NombreCompleto;
            UsuarioValor.Content = usuarioPrincipal.NombreUsuario;
            EmailValor.Content = usuarioPrincipal.Email;

            pagoTarjeta.Visibility = Visibility.Collapsed;
            precioGarantia = 0;
            precioEnvio = 0;
            TotalValor.Content = (precioCarrito + precioEnvio + precioGarantia).ToString("0.00") + " €";

        }

        private void PagoTarjeta_Click(object sender, RoutedEventArgs e)
        {
            pagoTarjeta.Visibility = Visibility.Visible;
        }

        private void PagoPaypal_Click(object sender, RoutedEventArgs e)
        {
            pagoTarjeta.Visibility = Visibility.Collapsed;
        }

        private void CreditNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Elimina cualquier espacio en blanco existente
            string text = textBox.Text.Replace(" ", "");

            // Formatea el texto en grupos de cuatro números separados por espacios
            string formattedText = FormatCreditCardNumber(text);

            // Actualiza el contenido del TextBox con el texto formateado
            textBox.Text = formattedText;

            // Coloca el cursor al final del texto
            textBox.CaretIndex = textBox.Text.Length;
        }

        private string FormatCreditCardNumber(string input)
        {
            // Elimina cualquier carácter que no sea un número
            string digitsOnly = new string(input.Where(char.IsDigit).ToArray());

            // Divide los números en grupos de cuatro
            IEnumerable<string> groupsOfFour = Enumerable.Range(0, (int)Math.Ceiling((double)digitsOnly.Length / 4))
                                                         .Select(i => digitsOnly.Substring(i * 4, Math.Min(4, digitsOnly.Length - i * 4)));

            // Une los grupos con espacios entre ellos
            string formattedText = string.Join(" ", groupsOfFour);

            return formattedText;
        }

        private void pagarBtn_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = MessageBox.Show(
                    "¿Seguro que quieres comprar los elementos del carrito?\n",
                    "Confirmación",
                    MessageBoxButton.YesNo
                    );

            if (result == MessageBoxResult.Yes)
            {



                if ((PagoTarjeta.IsChecked != true && PagoPaypal.IsChecked != true) || (DireccionValor.Text == "") || (DireccionValor.Text == null))
                {
                    MessageBox.Show("Rellena todos los campos");
                    return;
                }
                
                if(PagoTarjeta.IsChecked == true)
                {
                    if (NumTarjeta.Text == "" || NumTarjeta.Text == null || CVVValor.Text == "" || CVVValor.Text == null || fechaVencimiento.Text == "" || fechaVencimiento.Text == null)
                    {
                        MessageBox.Show("Rellena todos los campos");
                        return;
                    }
                }

                //

                try
                {
                    dbManager.Connection.Open();
                    using (MySqlTransaction transaction = dbManager.Connection.BeginTransaction())
                    {
                        // Inserta el nuevo artista
                        string query = "INSERT INTO pedidos (IdPedido, Usuario, CosteTotal, FormaPago, NumeroTarjeta, CVV, VencimientoTarjeta, Garantia, TipoEnvio, DireccionEntrega, FechaPedido, EstadoEntrega)" +
                        "VALUES (@IdPedido, @Usuario, @CosteTotal, @FormaPago, @NumeroTarjeta, @CVV, @Vencimiento, @Garantia, @TipoEnvio, @Direccion, @Fecha, @Estado)";


                        string query2 = "SELECT MAX(IdPedido) FROM pedidos;";
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
                       
                        Console.WriteLine("ID: " + Id + "Usuario: " + NombreUsuario + "Coste: " + TotalValor.Content);
                        dbManager.Connection.Close();

                        dbManager.Connection.Open();
                        MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@IdPedido", Id);
                        cmd.Parameters.AddWithValue("@Usuario", NombreUsuario);
                        cmd.Parameters.AddWithValue("@CosteTotal",Convert.ToDecimal(Regex.Replace(TotalValor.Content.ToString(), " €", "")) );
                        if (PagoTarjeta.IsChecked == true)
                        {
                            cmd.Parameters.AddWithValue("@FormaPago", "Tarjeta");
                            cmd.Parameters.AddWithValue("@NumeroTarjeta", NumTarjeta.Text);
                            cmd.Parameters.AddWithValue("@CVV", CVVValor.Text);
                            cmd.Parameters.AddWithValue("@Vencimiento", fechaVencimiento.Text);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@FormaPago", "Paypal");
                            cmd.Parameters.AddWithValue("@NumeroTarjeta", "");
                            cmd.Parameters.AddWithValue("@CVV", "");
                            cmd.Parameters.AddWithValue("@Vencimiento", "");
                        }

                        if (GarantiaToggle.IsChecked == true)
                        {
                            cmd.Parameters.AddWithValue("@Garantia", 1);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Garantia", 0);
                        }

                        if (envioEstandard.IsChecked == true)
                        {
                            cmd.Parameters.AddWithValue("@TipoEnvio", "Estandard");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@TipoEnvio", "Express");
                        }

                        cmd.Parameters.AddWithValue("@Direccion", DireccionEnvio.Content);
                        cmd.Parameters.AddWithValue("@Fecha", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Estado", "En Proceso");
                        cmd.ExecuteNonQuery();


                        //Inserta la información de los productos
                        foreach (ItemCarrito producto in carrito)
                        {
                            string query3 = "INSERT INTO detallesPedido (IdPedido, IdVinilo, PrecioProducto, Cantidad) VALUES (@IdPedido, @IdVinilo, @PrecioProducto, @Cantidad);";
                            MySqlCommand cmd3 = new MySqlCommand(query3, dbManager.Connection);
                            cmd3.Transaction = transaction;
                            cmd3.Parameters.AddWithValue("@IdPedido", Id);
                            cmd3.Parameters.AddWithValue("@IdVinilo", producto.Id);
                            cmd3.Parameters.AddWithValue("@PrecioProducto", producto.Precio);
                            cmd3.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                            cmd3.ExecuteNonQuery();
                        }

                        //Borar del carrito
                        string query4 = "DELETE FROM carritoUsuario WHERE Usuario = @Usuario";
                        MySqlCommand cmd4 = new MySqlCommand(query4, dbManager.Connection);
                        cmd4.Transaction = transaction;
                        cmd4.Parameters.AddWithValue("@Usuario", NombreUsuario);
                        cmd4.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar artista: " + ex.Message);
                }
                finally
                {
                    MessageBox.Show("Compra realizada con exito");
                    dbManager.Connection.Close();
                }


                //Modificar cantidad de stock de vinilos de la tabla infoVinilosCompra

                try
                {
                    dbManager.Connection.Open();
                    using (MySqlTransaction transaction = dbManager.Connection.BeginTransaction())
                    {
                        foreach (ItemCarrito producto in carrito)
                        {
                            string query = "UPDATE infoVinilosCompra SET Cantidad = Cantidad - @Cantidad WHERE IdVinilo = @IdVinilo";
                            MySqlCommand cmd = new MySqlCommand(query, dbManager.Connection);
                            cmd.Transaction = transaction;
                            cmd.Parameters.AddWithValue("@IdVinilo", producto.Id);
                            cmd.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar artista: " + ex.Message);
                }
                finally
                {
                    mainMethods.Button_Home(NombreUsuario, this);
                    dbManager.Connection.Close();
                }




            }
        }

        private void GarantiaToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            precioGarantia = 0;
            TotalValor.Content = (precioCarrito + precioEnvio + precioGarantia).ToString("0.00") + " €";
        }
        private void GarantiaToggle_Checked(object sender, RoutedEventArgs e)
        {
            precioGarantia = 2;
            TotalValor.Content = (precioCarrito + precioEnvio + precioGarantia).ToString("0.00") + " €";
        }

        private void envioEstandard_Checked(object sender, RoutedEventArgs e)
        {
            if (precioCarrito != null)
            {
                precioEnvio = 0;
                if (TotalValor != null) // Asegúrate de que TotalValor no sea null
                {
                    TotalValor.Content = (precioCarrito + precioEnvio + precioGarantia).ToString("0.00") + " €";
                }
            }
        }

        private void envioExpress_Checked(object sender, RoutedEventArgs e)
        {
            precioEnvio = 4;
            TotalValor.Content = (precioCarrito + precioEnvio + precioGarantia).ToString("0.00") + " €";
        
        }
    }
}