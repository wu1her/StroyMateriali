using StroyMateriali.Models;
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

namespace StroyMateriali.Windows
{
    /// <summary>
    /// Логика взаимодействия для Katalog.xaml
    /// </summary>
    public partial class Katalog : Window
    {
        public Katalog(Polzovatel polzovatel)
        {
            InitializeComponent();
            var pravoredaktir = false;
            if (polzovatel != null)
                UserIma.Text = polzovatel.Ima;
            else
                UserIma.Text = "Гость";

            using(var db = new StroyMaterialiEntities())
            {
                var Tovary = db.Tovar.ToList();

                foreach (var tovar in Tovary)
                {
                    var card = new TovarCard(tovar);
                    Cards.Children.Add(card);
                }

                if (polzovatel != null && db.Polzovatel.Any
                    (z1 => z1.Login == polzovatel.Login && z1.Role.Any
                    (rol => rol.ID_Role == 1 || rol.ID_Role == 2)))
                {
                    PoiskandFiltr.Visibility = Visibility.Visible;
                }
                if (polzovatel != null && db.Polzovatel.Any
                     (z1 => z1.Login == polzovatel.Login && z1.Role.Any
                     (rol => rol.ID_Role == 1)))
                {
                    TovarAdd.Visibility = Visibility.Visible;
                    pravoredaktir = true;
                }

                var proizvoditel = db.Proizvoditel.ToList();

                FiltrProizvoditel.ItemsSource = proizvoditel;
                FiltrProizvoditel.SelectedIndex = 0;
            }
        }

        private void TovarAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
