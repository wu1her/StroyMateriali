using StroyMateriali.Models;
using System;
using System.Linq;
using System.Windows;

namespace StroyMateriali.Windows
{
    public partial class Zakazi : Window
    {
        private readonly bool pravoredactir;

        public Zakazi(bool pravo)
        {
            InitializeComponent();
            pravoredactir = pravo;

            if (pravoredactir)
                DobavitBox.Visibility = Visibility.Visible;

            ZagruzitZakazi();
        }

        private void ZagruzitZakazi()
        {
            try
            {
                using (var db = new StroyMaterialiEntities())
                {
                    var zakazi = db.Zakaz
                        .Include("Status")
                        .Include("Pynkti")
                        .OrderBy(zakaz => zakaz.Nomer)
                        .ToList();

                    Cards.Children.Clear();

                    foreach (var zakaz in zakazi)
                    {
                        var card = new ZakazCard(zakaz);

                        if (pravoredactir)
                            card.MouseLeftButtonUp += (s, e) => OtkritRedactor(zakaz.Nomer);

                        Cards.Children.Add(card);
                    }
                }
            }
            catch (Exception oshibka)
            {
                MessageBox.Show(oshibka.GetBaseException().Message, "Ошибка загрузки заказов");
            }
        }

        private void OtkritRedactor(int? nomer)
        {
            if (!pravoredactir)
                return;

            var okno = new ZakazRedactir(nomer);
            okno.Owner = this;

            if (okno.ShowDialog() == true)
                ZagruzitZakazi();
        }

        private void Dobavit_Click(object sender, RoutedEventArgs e)
        {
            OtkritRedactor(null);
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}