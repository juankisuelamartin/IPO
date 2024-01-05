using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WpfApp1.resources.Dominio
{
    public class Consulta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string usuario { get; set; }
        public string Mensaje { get; set; }
        public byte[] FotoConsulta { get; set; }
        public string Email { get; set; }
        public string Asunto { get; set; }

        public string hasScreenshot
        {
            get { 
                if (FotoConsulta == null)
                {
                    return "No";
                }
                else
                {
                    return "Si";
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
        public ImageSource Captura
        {
            get { return ConvertirBytesAImagen(FotoConsulta); }
        }
    }
}
