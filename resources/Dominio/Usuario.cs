using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.resources.Dominio
{
    public class Usuario
    {        
        public string NombreUsuario { get; set; }
        public string NombreReal { get; set; }
        public string Apellidos { get; set; }
        public string DireccionEnvio { get; set; }
        public string Email { get; set; }
        public string NombreCompleto
        {
            get { return NombreReal + " " + Apellidos; }
        }
    }
}
