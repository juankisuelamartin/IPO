using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1.resources.Dominio
{
    internal class Artista
    {
        public string NombreArtistico { get; set; }
        public string NombreReal { get; set; }
        public string Genero { get; set; }
        public DateTime Fecha { get; set; }
        public Boolean IsGroup { get; set; }
        public String Descripcion { get; set; }
        public List<string> Componentes { get; set; }
        public string X { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public byte[] Imagen { get; set; }

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

        public ImageSource Caratula
        {
            get { return ConvertirBytesAImagen(Imagen); }
        }
    }
}
