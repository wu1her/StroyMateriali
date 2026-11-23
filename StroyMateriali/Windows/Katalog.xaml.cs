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
        private List<Tovar> vsetovary;
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
                vsetovary = db.Tovar
                    .Include("Proizvoditel")
                    .Include("Postavshik")
                    .Include("Kategoria")
                    .ToList();

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

                var proizvoditeli = db.Proizvoditel
                    .Select(proizvoditel => proizvoditel.Ima)
                    .ToList();

                proizvoditeli.Insert(0, "Все производители");
                FiltrProizvoditel.ItemsSource = proizvoditeli;
                FiltrProizvoditel.SelectedIndex = 0;

                Obnova();
            }
        }

        private void Obnova()
        {
            try
            {
                if (vsetovary == null)
                    return;

                var tovary = vsetovary.AsEnumerable();
                var vybor = FiltrProizvoditel.SelectedItem.ToString();

                var poisk = Poisk.Text.Trim().ToLower();
                tovary = tovary.Where(
                    tovar => tovar.Ima.ToLower().Contains(poisk));

                if (FiltrProizvoditel.SelectedIndex > 0)
                {
                    tovary = tovary.Where(
                        tovar => tovar.Proizvoditel.Any(proizvoditel => proizvoditel.Ima == vybor));
                }

                if (Sortirovka.SelectedIndex == 0)
                    tovary = tovary.OrderBy(tovar => tovar.Cena);
                if (Sortirovka.SelectedIndex == 1)
                    tovary = tovary.OrderByDescending(tovar => tovar.Cena);
                if (Sortirovka.SelectedIndex == 2)
                    tovary = tovary.OrderBy(tovar => tovar.Kolichestvo);
                if (Sortirovka.SelectedIndex == 3)
                    tovary = tovary.OrderByDescending(tovar => tovar.Kolichestvo);
                if (Sortirovka.SelectedIndex == 4)
                    tovary = tovary.OrderBy(tovar => tovar.Skidka);
                if (Sortirovka.SelectedIndex == 5)
                    tovary = tovary.OrderByDescending(tovar => tovar.Skidka);

                Cards.Children.Clear();
                foreach (var tovar in tovary)
                {
                    var card = new TovarCard(tovar);
                    Cards.Children.Add(card);
                }
            } catch { }
        }
        private void TovarAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FiltrProizvoditelChanged(object sender, SelectionChangedEventArgs e)
        {
            Obnova();
        }
        private void SortirovkaChanged(object sender, SelectionChangedEventArgs e)
        {
            Obnova();
        }

        private void PoiskChanged(object sender, TextChangedEventArgs e)
        {
            Obnova();
        }
    }
}
