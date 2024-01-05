using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1.resources.Dominio
{
    public class Pedido
    {
        public int Id { get; set; }
        public List<ItemCarrito> carrito { get; set; }
        public string usuario { get; set; }
        public float CosteTotal { get; set; }
        public string FormaPago { get; set; }
        public string NumeroTarjeta { get; set; }
        public string CVV { get; set; }
        public string Vencimiento { get; set; }
        public bool Garantia { get; set; }
        public String TipoEnvio { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public Pedido()
        {
            carrito = new List<ItemCarrito>();
        }
        public string hasGarantia{
            get
            {
                if (Garantia)
                {
                    return "Si";
                }
                else
                {
                    return "No";
                }
            }
            
        }


        public byte[] Foto { get; set; }


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

        public ImageSource ImagenUsuario
        {
            get { return ConvertirBytesAImagen(Foto); }
        }
    }
}
