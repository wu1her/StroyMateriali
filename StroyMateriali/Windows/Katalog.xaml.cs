using StroyMateriali.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace StroyMateriali.Windows
{
    public partial class Katalog : Window
    {
        private List<Tovar> vsetovary;
        private bool pravoredactir;
        public Katalog(Polzovatel polzovatel)
        {
            
            InitializeComponent();
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
                
                if (polzovatel != null)
                {
                    if (polzovatel.Role.Any(rol => rol.ID_Role == 1 || rol.ID_Role == 2))
                        PoiskandFiltr.Visibility = Visibility.Visible;
                    if (polzovatel.Role.Any(rol => rol.ID_Role == 1))
                    {
                        TovarAdd.Visibility = Visibility.Visible;
                        pravoredactir = true;
                    }
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
                    if (pravoredactir == true)
                    {
                        card.MouseDoubleClick += (s, e) => 
                        {
                            var okno = new TovarRedactor();
                            okno.Show();
                        };
                    }
                    Cards.Children.Add(card);
                }
            } catch {MessageBox.Show("Ошибка данных");}
        }
        private void TovarAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ObnovaKatalog(object sender, RoutedEventArgs e)
        {
            Obnova();
        }
    }
}
