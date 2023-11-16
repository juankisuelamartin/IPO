using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para IUSUARIO.xaml
    /// </summary>
    public partial class IUSUARIO : Window
    {
        public IUSUARIO()
        {
            InitializeComponent();
            Loaded += IUSUARIO_Loaded; // Suscribir al evento Loaded
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void IUSUARIO_Loaded(object sender, RoutedEventArgs e)
        {
            // Obtener la hora local del usuario
            DateTime horaActual = DateTime.Now;

            // Determinar si es buenos días, buenas tardes o buenas noches
            if (horaActual.Hour >= 6 && horaActual.Hour < 13)
            {
                lblSaludo.Content = "Buenos días";
            }
            else if (horaActual.Hour >= 13 && horaActual.Hour < 21)
            {
                lblSaludo.Content = "Buenas tardes";
            }
            else
            {
                lblSaludo.Content = "Buenas noches";
            }
        }
    }
}
