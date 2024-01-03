using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1.resources.Dominio
{
    public class ItemCarrito
    {
        public Vinilo Vinilo { get; set; }
        public int Cantidad { get; set; }
        public string NombreyTitulo { 
            get { 
                return Vinilo.Artista + " - " + Vinilo.Titulo;
            } 
        }
        public String Artista { get { return Vinilo.Artista; } }
        public float Precio { get { return Vinilo.Precio; }
                                set { Precio = value; }
        }
        public ImageSource Imagen { get { return Vinilo.Caratula; } }

        public string PrecioFormateado
        {
            get { return Vinilo.Precio.ToString("0.00") + " €"; }
        }
    }
}
