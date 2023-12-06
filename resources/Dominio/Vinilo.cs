using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.resources.Dominio
{
    internal class Vinilo
    {
        public int Idvinilo { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Artista { get; set; }
        public string FechaSalida { get; set; }
        public string Pais { get; set; }
        public string Sello { get; set; }
        public string Formato { get; set; }
        public byte[] Portada { get; set; }

    }
}
