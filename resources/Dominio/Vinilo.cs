using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1.resources.Dominio
{
    internal class Vinilo
    {
        public int Idvinilo { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Artista { get; set; }
        public int Fecha { get; set; }
        public float Precio { get; set; }
        public List<string> Colaboraciones { get; set; }
        public List<string> Canciones { get; set; }
        public string FechaSalida { get; set; }
        public string Pais { get; set; }
        public string Sello { get; set; }
        public string Formato { get; set; }
        public byte[] Portada { get; set; }
        public int Cantidad { get; set; }
        public int NumeroMGS { get; set; }
        public byte[] Audio { get; set; }

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
            get { return ConvertirBytesAImagen(Portada); }
        }

        public Uri Preview
        {
            get { return ObtenerElementoMultimedia(); }
        }

        public Uri ObtenerElementoMultimedia()
        {
            if (Audio == null || Audio.Length == 0)
            {
                Console.WriteLine("No hay audio");
                return null;
            }

            string tempAudioFile = Path.Combine(Path.GetTempPath(), "tempAudio.mp3");

            try
            {
                // Guardar el array de bytes en un archivo temporal
                File.WriteAllBytes(tempAudioFile, Audio);
                Console.WriteLine("Audio cargado");
                return new Uri(tempAudioFile);

            }
            catch (Exception ex)
            {
                // Manejar excepciones según tus necesidades
                Console.WriteLine($"Error al crear el elemento multimedia: {ex.Message}");
                return null;
            }
        }
    }
}
